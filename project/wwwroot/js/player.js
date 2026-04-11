// --- ЛОГІКА ДЛЯ ПЛЕЙЛИСТІВ ---
let playlistQueue = [];
let currentQueueIndex = 0;

// Цю функцію ми будемо викликати кнопкою "Play All"
function playPlaylist(tracks) {
    if (!tracks || tracks.length === 0) {
        alert("Плейлист порожній!");
        return;
    }
    
    playlistQueue = tracks;
    currentQueueIndex = 0;
    
    playCurrentQueueTrack();
}

function playCurrentQueueTrack() {
    // Якщо треки закінчилися
    if (currentQueueIndex >= playlistQueue.length) {
        console.log("Плейлист завершено!");
        return;
    }

    let track = playlistQueue[currentQueueIndex];
    
    // 1. ВИКОРИСТОВУЄМО ВАШУ ФУНКЦІЮ! 
    // Вона сама оновить UI плеєра і запустить музику
    playSong(track.filename, track.title, track.artist, null);

    // 2. Шукаємо аудіо-тег, щоб знати, коли пісня закінчиться.
    // Робимо невелику затримку (300мс), щоб playSong встиг створити тег <audio>, якщо його ще не було
    setTimeout(() => {
        let audio = document.getElementById('audioPlayer') || document.querySelector('audio');
        
        if (audio) {
            // Коли пісня закінчується (onended) -> збільшуємо індекс і граємо наступну
            audio.onended = function() {
                currentQueueIndex++;
                playCurrentQueueTrack();
            };
        }
    }, 300);
}