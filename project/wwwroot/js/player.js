// ===== GLOBAL AUDIO =====
if (!window.musicAudio) {
    window.musicAudio = new Audio();
}

// ===== PLAYER STATE =====
window.PlayerState = {

    init: function () {
        console.log("🎧 Player started");

        this.updateUI();

        // ===== AUDIO EVENTS =====
        window.musicAudio.onplay = () => {
            this.syncIcon(true);
            this.setStatus("Playing");
            this.saveToHistory();
        };

        window.musicAudio.onpause = () => {
            this.syncIcon(false);
            this.setStatus("Paused");
        };

        window.musicAudio.onended = () => {
            this.setStatus("Stopped");
            this.nextTrack();
        };

        window.musicAudio.onerror = () => {
            console.error("❌ Audio error:", window.musicAudio.error);
        };

        window.musicAudio.ontimeupdate = () => {
            this.updateProgress();
        };

        window.musicAudio.onloadedmetadata = () => {
            this.updateDuration();
        };

        // ===== PROGRESS CLICK =====
        const bar = document.getElementById("progress-bar");
        if (bar) {
            bar.addEventListener("click", (e) => {
                const rect = bar.getBoundingClientRect();
                const percent = (e.clientX - rect.left) / rect.width;
                if (window.musicAudio.duration) {
                    window.musicAudio.currentTime = percent * window.musicAudio.duration;
                }
            });
        }

        // ===== VOLUME =====
        const volume = document.getElementById("volume-range");
        if (volume) {
            window.musicAudio.volume = volume.value;
            volume.addEventListener("input", (e) => {
                window.musicAudio.volume = e.target.value;
            });
        }
    },

    // ===== PLAY QUEUE =====
    playQueue: function (tracks, index) {
        if (!tracks || tracks.length === 0) return;

        localStorage.setItem("music_queue", JSON.stringify(tracks));
        localStorage.setItem("music_index", index || 0);

        this.loadAndPlay(true);
    },

    // ===== LOAD + PLAY =====
    loadAndPlay: function (force) {
        const queue = JSON.parse(localStorage.getItem("music_queue")) || [];
        const index = parseInt(localStorage.getItem("music_index")) || 0;
        const track = queue[index];

        if (!track) return;

        console.log("🎵 Playing:", track);

        const file = track.filename || track.FileName || track.url;
        if (!file) {
            console.error("❌ No file!");
            return;
        }

        // === ВИПРАВЛЕННЯ ШЛЯХУ ДО ФАЙЛУ (щоб не було 404 Not Found) ===
        let src = file;
        if (!src.startsWith("http")) {
            if (!src.startsWith("/")) src = "/" + src; // додаємо /, якщо немає
            if (!src.includes("/media/")) src = "/media" + src; // додаємо /media/, якщо немає
            // Залізобетонно прибираємо дублювання, якщо вони якось утворились
            src = src.replace(/\/media\/\/media\//g, "/media/").replace(/\/media\/media\//g, "/media/");
        }

        if (window.musicAudio.src !== window.location.origin + src) {
            window.musicAudio.pause();
            window.musicAudio.currentTime = 0;
            window.musicAudio.src = src;
            window.musicAudio.load();
        }

        this.updateUI();

        if (force) {
            window.musicAudio.play().catch(e => console.log("Blocked:", e));
        }
    },

    // ===== CONTROLS =====
    togglePlay: function () {
        if (!window.musicAudio.src) {
            this.loadAndPlay(true);
            return;
        }

        window.musicAudio.paused
            ? window.musicAudio.play()
            : window.musicAudio.pause();
    },

    nextTrack: function () {
        const queue = JSON.parse(localStorage.getItem("music_queue")) || [];
        if (queue.length === 0) return;

        let index = (parseInt(localStorage.getItem("music_index")) || 0) + 1;
        if (index >= queue.length) index = 0;

        localStorage.setItem("music_index", index);
        this.loadAndPlay(true);
    },

    prevTrack: function () {
        const queue = JSON.parse(localStorage.getItem("music_queue")) || [];
        if (queue.length === 0) return;

        let index = (parseInt(localStorage.getItem("music_index")) || 0) - 1;
        if (index < 0) index = queue.length - 1;

        localStorage.setItem("music_index", index);
        this.loadAndPlay(true);
    },

    stop: function () {
        window.musicAudio.pause();
        window.musicAudio.currentTime = 0;
        this.setStatus("Stopped");
    },

    // ===== UI =====
    updateUI: function () {
        const queue = JSON.parse(localStorage.getItem("music_queue")) || [];
        const index = parseInt(localStorage.getItem("music_index")) || 0;
        const track = queue[index];

        if (!track) return;

        console.log("🎨 UI:", track);

        const title = document.getElementById("player-title");
        const artist = document.getElementById("player-artist");
        const cover = document.getElementById("player-cover");

        if (title) title.innerText = track.title || track.Title || "Unknown";
        if (artist) artist.innerText = track.artist || track.Artist || "";

        if (cover) {
            let img =
                track.coverPath ||
                track.coverUrl ||
                `https://picsum.photos/seed/${track.id || track.Id}/300/300`;

            cover.src = img;
            cover.style.display = "block";
        }
    },

    // ===== STATUS =====
    setStatus: function (text) {
        const el = document.getElementById("player-status");
        if (el) el.innerText = text;
    },

    // ===== PROGRESS =====
    updateProgress: function () {
        const fill = document.getElementById("progress-fill");
        const time = document.getElementById("current-time");

        if (!window.musicAudio.duration) return;

        const pct = (window.musicAudio.currentTime / window.musicAudio.duration) * 100;
        if (fill) fill.style.width = pct + "%";

        if (time) {
            const m = Math.floor(window.musicAudio.currentTime / 60);
            const s = Math.floor(window.musicAudio.currentTime % 60);
            time.innerText = `${m}:${s < 10 ? "0" : ""}${s}`;
        }
    },

    updateDuration: function () {
        const el = document.getElementById("total-time");
        if (!el || !window.musicAudio.duration) return;

        const m = Math.floor(window.musicAudio.duration / 60);
        const s = Math.floor(window.musicAudio.duration % 60);
        el.innerText = `${m}:${s < 10 ? "0" : ""}${s}`;
    },

    // ===== ICON =====
    syncIcon: function (playing) {
        const btn = document.getElementById("btn-play-pause");
        if (!btn) return;

        btn.innerHTML = playing
            ? `<svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor">
                 <rect x="6" y="4" width="4" height="16"></rect>
                 <rect x="14" y="4" width="4" height="16"></rect>
               </svg>`
            : `<svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor">
                 <polygon points="5 3 19 12 5 21 5 3"/>
               </svg>`;
    },

    // ===== HISTORY =====
    saveToHistory: function () {
        try {
            const queue = JSON.parse(localStorage.getItem("music_queue")) || [];
            const index = parseInt(localStorage.getItem("music_index")) || 0;
            const track = queue[index];

            if (!track || !track.id) return;

            fetch('/Playback/Record', {
                method: 'POST',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                body: 'mediaItemId=' + encodeURIComponent(track.id)
            });
        } catch (e) {
            console.log("History error:", e);
        }
    }
};

// ===== INIT ТА ЛОГІКА РЕЙТИНГУ =====
document.addEventListener("DOMContentLoaded", () => {
    // Запускаємо плеєр
    window.PlayerState.init();

    // === ЛОГІКА ДЛЯ КЛІКУ ПО ЗІРОЧКАХ РЕЙТИНГУ ===
    const stars = document.querySelectorAll('.rating-star'); 

    stars.forEach(star => {
        star.addEventListener('click', function(e) {
            e.preventDefault();

            // Отримуємо дані з атрибутів зірочки
            const trackId = this.getAttribute('data-track-id');
            const ratingValue = this.getAttribute('data-rating');

            if (!trackId || !ratingValue) return;

            // Відправляємо дані на сервер
            fetch(`/Rating/SetRating?trackId=${trackId}&rating=${ratingValue}`, {
                method: 'POST'
            })
            .then(response => {
                if (!response.ok) throw new Error("Помилка на сервері");
                return response.json();
            })
            .then(data => {
                console.log("Оцінка збережена. Новий середній рейтинг:", data.average);
                
                // Оновлюємо відображення рейтингу на сторінці
                const displayEl = document.getElementById(`rating-display-${trackId}`);
                if (displayEl) {
                    displayEl.innerText = data.average;
                }
                
                // Додатково можна додати анімацію чи колір для натиснутих зірочок тут
            })
            .catch(error => {
                console.error('Помилка при відправці рейтингу:', error);
            });
        });
    });
});