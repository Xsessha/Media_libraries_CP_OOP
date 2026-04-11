// wwwroot/js/player.js

// Перевірка наявності глобального аудіо
if (!window.musicAudio) {
    window.musicAudio = new Audio();
}

window.PlayerState = {
    init: function () {
        console.log("=== Player Engine Start ===");
        this.updateUI();

        window.musicAudio.onplay = () => this.syncIcon(true);
        window.musicAudio.onpause = () => this.syncIcon(false);
        window.musicAudio.onended = () => this.nextTrack();

        window.musicAudio.ontimeupdate = () => {
            const fill = document.getElementById('progress-fill');
            if (fill && window.musicAudio.duration) {
                const pct = (window.musicAudio.currentTime / window.musicAudio.duration) * 100;
                fill.style.width = pct + '%';
            }
        };
    },

    playQueue: function (tracks, index) {
        console.log("Setting queue. Size:", tracks.length, "Index:", index);
        if (!tracks || tracks.length === 0) return;

        localStorage.setItem('music_queue', JSON.stringify(tracks));
        localStorage.setItem('music_index', index.toString());
        
        this.loadAndPlay(true);
    },

    loadAndPlay: function (force) {
        const queue = JSON.parse(localStorage.getItem('music_queue')) || [];
        const index = parseInt(localStorage.getItem('music_index')) || 0;
        const track = queue[index];

        if (!track) {
            console.error("Track not found at index:", index);
            return;
        }

        const src = track.filename.startsWith('http') ? track.filename : "/media/" + track.filename;
        
        if (window.musicAudio.src !== window.location.origin + src) {
            window.musicAudio.src = src;
        }

        if (force) {
            window.musicAudio.play().catch(e => console.warn("Playback blocked."));
        }
        this.updateUI();
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

        document.getElementById('player-title').innerText = track.title || "Unknown";
        document.getElementById('player-artist').innerText = track.artist || "";
        const cover = document.getElementById('player-cover');
        if (cover) {
            cover.src = track.coverPath || "/media/default-cover.png";
            cover.style.display = 'block';
        }
    },

    syncIcon: function (playing) {
        const btn = document.getElementById('btn-play-pause');
        if (btn) btn.innerHTML = playing ? "⏸" : "▶";
    }
};

// Запуск при завантаженні
document.addEventListener('DOMContentLoaded', () => window.PlayerState.init());