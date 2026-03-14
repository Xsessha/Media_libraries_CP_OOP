/* ============================================
   XSenoMusic — JavaScript Player & Interactions
   ============================================ */

// ---- SVG Icon Templates ----
const Icons = {
  play: `<svg viewBox="0 0 24 24" fill="currentColor"><polygon points="5 3 19 12 5 21 5 3"/></svg>`,
  pause: `<svg viewBox="0 0 24 24" fill="currentColor"><rect x="6" y="4" width="4" height="16"/><rect x="14" y="4" width="4" height="16"/></svg>`,
  prev: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><polygon points="19 20 9 12 19 4 19 20"/><line x1="5" y1="19" x2="5" y2="5"/></svg>`,
  next: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><polygon points="5 4 15 12 5 20 5 4"/><line x1="19" y1="5" x2="19" y2="19"/></svg>`,
  shuffle: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="16 3 21 3 21 8"/><line x1="4" y1="20" x2="21" y2="3"/><polyline points="21 16 21 21 16 21"/><line x1="15" y1="15" x2="21" y2="21"/><line x1="4" y1="4" x2="9" y2="9"/></svg>`,
  heart: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/></svg>`,
  heartFilled: `<svg viewBox="0 0 24 24" fill="#ef4444" stroke="#ef4444" stroke-width="2"><path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/></svg>`,
  search: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>`,
  volume: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"><polygon points="11 5 6 9 2 9 2 15 6 15 11 19 11 5"/><path d="M15.54 8.46a5 5 0 0 1 0 7.07"/><path d="M19.07 4.93a10 10 0 0 1 0 14.14"/></svg>`,
  star: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"/></svg>`,
  starFilled: `<svg viewBox="0 0 24 24" fill="#facc15" stroke="#facc15" stroke-width="2"><polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"/></svg>`,
  download: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><polyline points="7 10 12 15 17 10"/><line x1="12" y1="15" x2="12" y2="3"/></svg>`,
  plus: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"><line x1="12" y1="5" x2="12" y2="19"/><line x1="5" y1="12" x2="19" y2="12"/></svg>`,
};

// ---- Player State ----
const PlayerState = {
  audio: new Audio(),
  currentTrack: null,
  playlist: [],
  currentIndex: -1,
  isPlaying: false,
  isShuffle: false,
  volume: 0.7,

  init() {
    this.audio.volume = this.volume;
    this.clearPlayerUI();
    this.audio.addEventListener('timeupdate', () => this.updateProgress());
    this.audio.addEventListener('ended', () => this.nextTrack());
    this.audio.addEventListener('loadedmetadata', () => this.updateDuration());

    // Progress bar click
    const progressBar = document.getElementById('progress-bar');
    if (progressBar) {
      progressBar.addEventListener('click', (e) => {
        const rect = progressBar.getBoundingClientRect();
        const pct = (e.clientX - rect.left) / rect.width;
        this.audio.currentTime = pct * this.audio.duration;
      });
    }

    // Volume bar click
    const volumeBar = document.getElementById('volume-bar');
    if (volumeBar) {
      volumeBar.addEventListener('click', (e) => {
        const rect = volumeBar.getBoundingClientRect();
        const pct = Math.max(0, Math.min(1, (e.clientX - rect.left) / rect.width));
        this.setVolume(pct);
      });
    }

    // Volume range slider
    const volumeRange = document.getElementById('volume-range');
    if (volumeRange) {
      volumeRange.value = this.volume;
      volumeRange.addEventListener('input', (e) => {
        const value = parseFloat(e.target.value);
        this.setVolume(value);
      });
    }
  },

  updateProgress() {
    if (!this.audio.duration) return;
    const pct = (this.audio.currentTime / this.audio.duration) * 100;
    const fill = document.getElementById('progress-fill');
    const currentTime = document.getElementById('current-time');
    if (fill) fill.style.width = pct + '%';
    if (currentTime) currentTime.textContent = this.formatTime(this.audio.currentTime);
  },

  updateDuration() {
    const el = document.getElementById('total-time');
    if (el) el.textContent = this.formatTime(this.audio.duration);
  },

  formatTime(sec) {
    if (!sec || isNaN(sec)) return '0:00';
    const m = Math.floor(sec / 60);
    const s = Math.floor(sec % 60);
    return m + ':' + (s < 10 ? '0' : '') + s;
  },

  play() {
    this.audio.play();
    this.isPlaying = true;
    const player = document.getElementById('music-player');
    if (player) player.classList.add('playing');
    const status = document.getElementById('player-status');
    if (status) status.textContent = 'Playing';
    this.updatePlayButton();
  },

  pause() {
    this.audio.pause();
    this.isPlaying = false;
    const status = document.getElementById('player-status');
    if (status) status.textContent = 'Paused';
    this.updatePlayButton();
  },

  stop() {
    this.audio.pause();
    this.audio.currentTime = 0;
    this.audio.src = '';
    this.isPlaying = false;
    const player = document.getElementById('music-player');
    if (player) player.classList.remove('playing');
    const status = document.getElementById('player-status');
    if (status) status.textContent = 'Stopped';
    this.clearPlayerUI();
    this.updatePlayButton();
  },

  clearPlayerUI() {
    this.currentTrack = null;
    const player = document.getElementById('music-player');
    const cover = document.getElementById('player-cover');
    const titleEl = document.getElementById('player-title');
    const artistEl = document.getElementById('player-artist');
    const fill = document.getElementById('progress-fill');
    const currentTime = document.getElementById('current-time');

    if (player) player.classList.remove('playing');
    if (cover) {
      cover.classList.add('hidden');
      cover.src = '/media/default-cover.png';
    }
    if (titleEl) titleEl.textContent = 'No track selected';
    if (artistEl) artistEl.textContent = '';
    const status = document.getElementById('player-status');
    if (status) status.textContent = 'Stopped';
    if (fill) fill.style.width = '0%';
    if (currentTime) currentTime.textContent = '0:00';
  },

  togglePlay() {
    if (this.isPlaying) this.pause();
    else this.play();
  },

  updatePlayButton() {
    const btn = document.getElementById('btn-play-pause');
    if (btn) btn.innerHTML = this.isPlaying ? Icons.pause : Icons.play;
  },

  nextTrack() {
    if (this.playlist.length === 0) return;
    if (this.isShuffle) {
      this.currentIndex = Math.floor(Math.random() * this.playlist.length);
    } else {
      this.currentIndex = (this.currentIndex + 1) % this.playlist.length;
    }
    const t = this.playlist[this.currentIndex];
    this.loadAndPlay(t.filename, t.title, t.artist, t.coverUrl);
  },

  prevTrack() {
    if (this.playlist.length === 0) return;
    if (this.audio.currentTime > 3) {
      this.audio.currentTime = 0;
      return;
    }
    this.currentIndex = (this.currentIndex - 1 + this.playlist.length) % this.playlist.length;
    const t = this.playlist[this.currentIndex];
    this.loadAndPlay(t.filename, t.title, t.artist, t.coverUrl);
  },

  toggleShuffle() {
    this.isShuffle = !this.isShuffle;
    const btn = document.getElementById('btn-shuffle');
    if (btn) btn.classList.toggle('active', this.isShuffle);
  },

  loadAndPlay(filename, title, artist, coverUrl) {
    this.audio.src = '/media/' + filename;
    this.currentTrack = { filename, title, artist, coverUrl };
    this.updatePlayerUI();
    this.play();
  },

  setVolume(value) {
    const volume = Math.min(1, Math.max(0, value));
    this.volume = volume;
    this.audio.volume = volume;

    const fill = document.getElementById('volume-fill');
    if (fill) fill.style.width = (volume * 100) + '%';

    const range = document.getElementById('volume-range');
    if (range) range.value = volume;
  },

  updatePlayerUI() {
    const t = this.currentTrack;
    const player = document.getElementById('music-player');
    const cover = document.getElementById('player-cover');
    const titleEl = document.getElementById('player-title');
    const artistEl = document.getElementById('player-artist');

    if (!t) {
      if (player) player.classList.remove('playing');
      this.clearPlayerUI();
      return;
    }

    if (player) player.classList.add('playing');
    if (cover) {
      cover.src = t.coverUrl || '/media/default-cover.png';
      cover.classList.remove('hidden');
      cover.style.display = 'block';
    }

    if (titleEl) titleEl.textContent = t.title || t.filename || 'Unknown track';
    if (artistEl) artistEl.textContent = t.artist || 'Unknown artist';

    // Reset progress
    const fill = document.getElementById('progress-fill');
    const ct = document.getElementById('current-time');
    if (fill) fill.style.width = '0%';
    if (ct) ct.textContent = '0:00';
  }
};

// ---- Global playTrack function (called from Razor views) ----
function playTrack(trackId, filename, title, artist, coverUrl) {
  // If called with just filename and title (legacy), fill defaults
  if (typeof title === 'undefined') {
    title = filename;
    filename = trackId;
    trackId = null;
  }

  artist = artist || '';
  coverUrl = coverUrl || '/media/default-cover.png';
  PlayerState.loadAndPlay(filename, title, artist, coverUrl);

  // Build a minimal playlist so next/prev buttons work properly.
  if (trackId !== null && typeof trackId !== 'undefined') {
    PlayerState.playlist = [{ id: trackId, filename, title, artist, coverUrl }];
    PlayerState.currentIndex = 0;

    // Record playback on server (optional; requires login)
    fetch('/Playback/Record', {
      method: 'POST',
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      body: 'mediaItemId=' + encodeURIComponent(trackId)
    }).catch(() => {});
  }
}

// ---- Favorites ----
const Favorites = {
  ids: JSON.parse(localStorage.getItem('xseno_favorites') || '[]'),

  toggle(trackId) {
    const idx = this.ids.indexOf(trackId);
    const isNowFavorite = idx === -1;

    if (isNowFavorite) {
      this.ids.push(trackId);
    } else {
      this.ids.splice(idx, 1);
    }

    localStorage.setItem('xseno_favorites', JSON.stringify(this.ids));
    this.updateButtons();

    // Persist on server
    fetch('/Favorites/Toggle', {
      method: 'POST',
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      body: 'mediaItemId=' + encodeURIComponent(trackId)
    }).catch(() => {});
  },

  isFav(trackId) {
    return this.ids.includes(trackId);
  },

  updateButtons() {
    document.querySelectorAll('.btn-favorite').forEach(btn => {
      const id = btn.dataset.trackId;
      if (id) {
        btn.innerHTML = this.isFav(id) ? Icons.heartFilled : Icons.heart;
        btn.classList.toggle('active', this.isFav(id));
      }
    });
  }
};

// ---- Rating ----
function setRating(trackId, rating) {
  // Update stars UI
  const container = document.querySelector(`.rating-stars[data-track-id="${trackId}"]`);
  if (container) {
    container.querySelectorAll('.star').forEach((star, i) => {
      star.innerHTML = i < rating ? Icons.starFilled : Icons.star;
      star.classList.toggle('filled', i < rating);
    });
  }
  // POST to server
  fetch('/Rating/SetRating?trackId=' + encodeURIComponent(trackId) + '&rating=' + rating, { method: 'POST' }).catch(() => {});
}

// ---- Search / Filter ----
function filterTracks(inputId, tableId) {
  const query = document.getElementById(inputId).value.toLowerCase();
  const rows = document.querySelectorAll('#' + tableId + ' tbody tr');
  rows.forEach(row => {
    const text = row.textContent.toLowerCase();
    row.style.display = text.includes(query) ? '' : 'none';
  });
}

// ---- Sort ----
function sortTracks(selectId, tableId) {
  const sortBy = document.getElementById(selectId).value;
  const tbody = document.querySelector('#' + tableId + ' tbody');
  if (!tbody) return;
  const rows = Array.from(tbody.querySelectorAll('tr'));

  rows.sort((a, b) => {
    switch (sortBy) {
      case 'title':
        return (a.dataset.title || '').localeCompare(b.dataset.title || '');
      case 'rating':
        return (parseInt(b.dataset.rating) || 0) - (parseInt(a.dataset.rating) || 0);
      case 'date':
        return (b.dataset.date || '').localeCompare(a.dataset.date || '');
      default:
        return 0;
    }
  });

  rows.forEach(row => tbody.appendChild(row));
}

// ---- Export Playlist ----
function exportPlaylist(playlistData) {
  const json = JSON.stringify(playlistData, null, 2);
  const blob = new Blob([json], { type: 'application/json' });
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = (playlistData.name || 'playlist') + '.json';
  a.click();
  URL.revokeObjectURL(url);
}

// ---- Init ----
document.addEventListener('DOMContentLoaded', () => {
  PlayerState.init();

  // Load favorites from server (if logged in)
  fetch('/Favorites/Get')
    .then(r => r.json())
    .then(data => {
      if (data?.ids && Array.isArray(data.ids)) {
        Favorites.ids = data.ids;
        localStorage.setItem('xseno_favorites', JSON.stringify(Favorites.ids));
      }
    })
    .catch(() => {})
    .finally(() => {
      Favorites.updateButtons();
    });

  // Highlight active sidebar link
  const currentPath = window.location.pathname;
  document.querySelectorAll('.sidebar-nav a').forEach(link => {
    const href = link.getAttribute('href');
    if (href === currentPath || (href !== '/' && currentPath.startsWith(href))) {
      link.classList.add('active');
    }
  });
});
