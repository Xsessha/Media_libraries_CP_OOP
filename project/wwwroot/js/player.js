// Перевірка наявності глобального аудіо
if (!window.musicAudio) {
    window.musicAudio = new Audio();
}

window.PlayerState = {
    init: function () {
        console.log("=== Player Engine Start (v2) ==="); 
        this.updateUI();

        window.musicAudio.onerror = (e) => {
            console.error("❌ Audio Error! Деталі:", window.musicAudio.error);
        };

        window.musicAudio.onloadedmetadata = () => {
            const totalTime = document.getElementById('total-time');
            if (totalTime && window.musicAudio.duration) {
                const mins = Math.floor(window.musicAudio.duration / 60);
                const secs = Math.floor(window.musicAudio.duration % 60);
                totalTime.innerText = mins + ':' + (secs < 10 ? '0' : '') + secs;
            }
        };

        window.musicAudio.onplay = () => {
            console.log("▶ Track is playing!");
            this.syncIcon(true);
            const statusEl = document.getElementById('player-status');
            if (statusEl) statusEl.innerText = "Playing";
        };

        window.musicAudio.onpause = () => {
            console.log("⏸ Track paused.");
            this.syncIcon(false);
            const statusEl = document.getElementById('player-status');
            if (statusEl) statusEl.innerText = "Paused";
        };

        window.musicAudio.onended = () => {
            const statusEl = document.getElementById('player-status');
            if (statusEl) statusEl.innerText = "Stopped";
            this.nextTrack();
        };

        window.musicAudio.ontimeupdate = () => {
            const fill = document.getElementById('progress-fill');
            const currentTimeEl = document.getElementById('current-time');

            if (window.musicAudio.duration) {
                const pct = (window.musicAudio.currentTime / window.musicAudio.duration) * 100;
                if (fill) fill.style.width = pct + '%';

                if (currentTimeEl) {
                    const mins = Math.floor(window.musicAudio.currentTime / 60);
                    const secs = Math.floor(window.musicAudio.currentTime % 60);
                    currentTimeEl.innerText = mins + ':' + (secs < 10 ? '0' : '') + secs;
                }
            }
        };

        const progressBar = document.getElementById('progress-bar');
        if (progressBar) {
            progressBar.addEventListener('click', (e) => {
                const rect = progressBar.getBoundingClientRect();
                const percent = (e.clientX - rect.left) / rect.width;
                if (window.musicAudio.duration) {
                    window.musicAudio.currentTime = percent * window.musicAudio.duration;
                }
            });
        }

        const volumeRange = document.getElementById('volume-range');
        if (volumeRange) {
            window.musicAudio.volume = volumeRange.value;
            volumeRange.addEventListener('input', (e) => {
                window.musicAudio.volume = e.target.value;
            });
        }
    },

    playQueue: function (tracks, index) {
        console.log("1. Setting queue. Size:", tracks.length, "Index:", index);
        if (!tracks || tracks.length === 0) return;

        localStorage.setItem('music_queue', JSON.stringify(tracks));
        localStorage.setItem('music_index', index.toString());

        this.loadAndPlay(true);
    },

    loadAndPlay: function (force) {
        console.log("2. loadAndPlay started");
        const queue = JSON.parse(localStorage.getItem('music_queue')) || [];
        const index = parseInt(localStorage.getItem('music_index')) || 0;
        const track = queue[index];

        console.log("3. Track loaded from queue:", track);

        if (!track) {
            console.error("❌ Track not found at index:", index);
            return;
        }

        // ШУКАЄМО ШЛЯХ ДО ФАЙЛУ (враховуємо, що C# може віддавати поля з великої літери)
        const filePath = track.filename || track.FileName || track.filePath || track.FilePath || track.url || track.Url;

        if (!filePath) {
            console.error("❌ Немає шляху до файлу! Ось що є в об'єкті track:", track);
            return;
        }

        const src = filePath.startsWith('http') ? filePath : "/media/" + filePath;
        console.log("🎵 4. Trying to load audio from:", src);

        if (window.musicAudio.src !== window.location.origin + src && window.musicAudio.src !== src) {
            window.musicAudio.pause();
            window.musicAudio.currentTime = 0;
            window.musicAudio.src = src;
            window.musicAudio.load();
        }

        this.updateUI();

        if (force) {
            console.log("5. Calling audio.play()...");
            window.musicAudio.play()
                .then(() => console.log("✅ Play() promise resolved! Звук має бути!"))
                .catch(e => console.error("❌ Playback blocked:", e));
        }
    },

    togglePlay: function () {
        if (!window.musicAudio.src) {
            this.loadAndPlay(true);
        } else {
            window.musicAudio.paused ? window.musicAudio.play() : window.musicAudio.pause();
        }
    },

    nextTrack: function () {
        const queue = JSON.parse(localStorage.getItem('music_queue')) || [];
        if (queue.length === 0) return;

        let index = (parseInt(localStorage.getItem('music_index')) || 0) + 1;
        if (index >= queue.length) index = 0;

        localStorage.setItem('music_index', index.toString());
        this.loadAndPlay(true);
    },

    prevTrack: function () {
        const queue = JSON.parse(localStorage.getItem('music_queue')) || [];
        if (queue.length === 0) return;

        let index = (parseInt(localStorage.getItem('music_index')) || 0) - 1;
        if (index < 0) index = queue.length - 1;

        localStorage.setItem('music_index', index.toString());
        this.loadAndPlay(true);
    },

    updateUI: function () {
        const queue = JSON.parse(localStorage.getItem('music_queue')) || [];
        const index = parseInt(localStorage.getItem('music_index')) || 0;
        const track = queue[index];

        if (!track) return;

        const titleEl = document.getElementById('player-title');
        const artistEl = document.getElementById('player-artist');
        // Враховуємо C# PascalCase
        if (titleEl) titleEl.innerText = track.title || track.Title || "Unknown";
        if (artistEl) artistEl.innerText = track.artist || track.Artist || "";

        const coverEl = document.getElementById('player-cover');
        if (coverEl) {
            const coverPath = track.coverPath || track.CoverPath;
            if (coverPath) {
                coverEl.src = coverPath;
            } else if (track.id || track.Id) {
                coverEl.src = `https://picsum.photos/seed/${track.id || track.Id}/300/300`;
            } else {
                coverEl.src = "/images/default-cover.png";
            }
        }
    },

    syncIcon: function (playing) {
        const btn = document.getElementById('btn-play-pause');
        if (!btn) return;

        if (playing) {
            btn.innerHTML = `<svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor"><rect x="6" y="4" width="4" height="16"></rect><rect x="14" y="4" width="4" height="16"></rect></svg>`;
        } else {
            btn.innerHTML = `<svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor"><polygon points="5 3 19 12 5 21 5 3"/></svg>`;
        }
    }
};

document.addEventListener('DOMContentLoaded', () => window.PlayerState.init());