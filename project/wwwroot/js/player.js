// Compatibility wrapper for legacy Home/Index use
function playSong(filename, title, artist, coverUrl) {
    // Use the modern player system, falling back to a default if not available
    if (typeof playTrack === 'function') {
        playTrack(null, filename, title || filename, artist || 'Unknown artist', coverUrl || '/media/default-cover.png');
    } else {
        let audio = document.getElementById('audioPlayer');
        if (!audio) {
            audio = new Audio('/media/' + filename);
            audio.id = 'audioPlayer';
            document.body.appendChild(audio);
        }
        audio.src = '/media/' + filename;
        audio.play();
    }
}