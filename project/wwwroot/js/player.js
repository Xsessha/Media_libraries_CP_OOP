let player = document.getElementById("audioPlayer")

function playSong(filename){
    player.src="/media/"+filename
    player.play()
}