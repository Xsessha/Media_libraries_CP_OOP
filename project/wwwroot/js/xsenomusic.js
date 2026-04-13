/* ============================================
   XSenoMusic — JavaScript Utilities & UI
   ============================================ */

// ---- SVG Icon Templates ----
const Icons = {
  heart: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/></svg>`,
  heartFilled: `<svg viewBox="0 0 24 24" fill="#ef4444" stroke="#ef4444" stroke-width="2"><path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/></svg>`,
  star: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"/></svg>`,
  starFilled: `<svg viewBox="0 0 24 24" fill="#facc15" stroke="#facc15" stroke-width="2"><polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"/></svg>`
};

// ---- Favorites (Запасна логіка для інших сторінок) ----
const Favorites = {
  ids: JSON.parse(localStorage.getItem('xseno_favorites') || '[]'),

  toggle(trackId) {
    const idx = this.ids.indexOf(trackId);
    if (idx === -1) {
      this.ids.push(trackId);
    } else {
      this.ids.splice(idx, 1);
    }
    localStorage.setItem('xseno_favorites', JSON.stringify(this.ids));
    this.updateButtons();
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

// ---- Search / Filter (Для таблиць) ----
function filterTracks(inputId, tableId) {
  const query = document.getElementById(inputId).value.toLowerCase();
  const rows = document.querySelectorAll('#' + tableId + ' tbody tr');
  rows.forEach(row => {
    const text = row.textContent.toLowerCase();
    row.style.display = text.includes(query) ? '' : 'none';
  });
}

// ---- Sort (Для таблиць) ----
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

// ---- Init (Запуск при завантаженні сторінки) ----
document.addEventListener('DOMContentLoaded', () => {
  // Завантаження лайків з сервера
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

  // Підсвітка активного посилання в боковому меню (Sidebar)
  const currentPath = window.location.pathname;
  document.querySelectorAll('.sidebar-nav a').forEach(link => {
    const href = link.getAttribute('href');
    if (href === currentPath || (href !== '/' && currentPath.startsWith(href))) {
      link.classList.add('active');
    }
  });
});