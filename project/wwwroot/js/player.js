// ===== GLOBAL AUDIO =====
if (!window.musicAudio) {
    window.musicAudio = new Audio();
}

// ===== PLAYER STATE =====
window.PlayerState = {

    isShuffle: false,

    init: function () {
        console.log("🎧 Player started");

        this.updateUI();

        // ===== LOAD SAVED VOLUME =====
        const volume = document.getElementById("volume-range");
        const savedVolume = localStorage.getItem("music_volume");

        if (volume) {
            const vol = savedVolume ? parseFloat(savedVolume) : 0.7;
            volume.value = vol;
            window.musicAudio.volume = vol;

            volume.addEventListener("input", (e) => {
                const val = parseFloat(e.target.value);
                window.musicAudio.volume = val;
                localStorage.setItem("music_volume", val);
            });
        }

        // ===== AUDIO EVENTS =====
        window.musicAudio.onplay = () => {
            this.syncIcon(true);
            this.setStatus("Playing");
        };

        window.musicAudio.onpause = () => {
            this.syncIcon(false);
            this.setStatus("Paused");
        };

        window.musicAudio.onended = () => {
            this.nextTrack();
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
                    window.musicAudio.currentTime =
                        percent * window.musicAudio.duration;
                }
            });
        }
    },

    // ===== SHUFFLE =====
    toggleShuffle: function () {
        this.isShuffle = !this.isShuffle;

        const btn = document.getElementById("btn-shuffle");

        if (btn) {
            if (this.isShuffle) {
                btn.style.color = "#1DB954";
                btn.style.transform = "scale(1.1)";
            } else {
                btn.style.color = "#ffffff";
                btn.style.transform = "scale(1)";
            }
        }

        console.log("Shuffle:", this.isShuffle);
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

        const file = track.filename || track.FileName || track.url;
        if (!file) return;

        let src = file;

        if (!src.startsWith("http")) {
            if (!src.startsWith("/")) src = "/" + src;
            if (!src.includes("/media/")) src = "/media" + src;
        }

        if (window.musicAudio.src !== window.location.origin + src) {
            window.musicAudio.pause();
            window.musicAudio.currentTime = 0;
            window.musicAudio.src = src;
            window.musicAudio.load();
        }

        this.updateUI();

        if (force) {
            window.musicAudio.play().catch(() => {});
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

    // 🔥 FIXED SHUFFLE NEXT
    nextTrack: function () {
        const queue = JSON.parse(localStorage.getItem("music_queue")) || [];
        if (queue.length === 0) return;

        let currentIndex = parseInt(localStorage.getItem("music_index")) || 0;
        let index;

        if (this.isShuffle) {
            if (queue.length === 1) return;

            do {
                index = Math.floor(Math.random() * queue.length);
            } while (index === currentIndex);

        } else {
            index = currentIndex + 1;
            if (index >= queue.length) index = 0;
        }

        localStorage.setItem("music_index", index);
        this.loadAndPlay(true);
    },

    // 🔥 FIXED SHUFFLE PREV
    prevTrack: function () {
        const queue = JSON.parse(localStorage.getItem("music_queue")) || [];
        if (queue.length === 0) return;

        let currentIndex = parseInt(localStorage.getItem("music_index")) || 0;
        let index;

        if (this.isShuffle) {
            if (queue.length === 1) return;

            do {
                index = Math.floor(Math.random() * queue.length);
            } while (index === currentIndex);

        } else {
            index = currentIndex - 1;
            if (index < 0) index = queue.length - 1;
        }

        localStorage.setItem("music_index", index);
        this.loadAndPlay(true);
    },

    // ===== UI =====
    updateUI: function () {
        const queue = JSON.parse(localStorage.getItem("music_queue")) || [];
        const index = parseInt(localStorage.getItem("music_index")) || 0;
        const track = queue[index];

        if (!track) return;

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

        const pct =
            (window.musicAudio.currentTime / window.musicAudio.duration) * 100;

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
    }
};

// ===== INIT =====
document.addEventListener("DOMContentLoaded", () => {
    window.PlayerState.init();
});