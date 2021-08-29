var context;    // Audio context
var buf;        // Audio buffer

function _base64ToArrayBuffer(base64) {
    var binary_string = window.atob(base64);
    var len = binary_string.length;
    var bytes = new Uint8Array(len);
    for (var i = 0; i < len; i++) {
        bytes[i] = binary_string.charCodeAt(i);
    }
    return bytes;
}

export function playAudioBytes(base64Data) {
    var byteArray = _base64ToArrayBuffer(base64Data);
    if (!window.AudioContext) {
        if (!window.webkitAudioContext) {
            alert("Your browser does not support any AudioContext and cannot play back this audio.");
            return;
        }
        window.AudioContext = window.webkitAudioContext;
    }

    context = new AudioContext({
        sampleRate:"16000"
    });
    playByteArray(byteArray);
}

function playByteArray(byteArray) {

    context.decodeAudioData(byteArray.buffer, play);
    //play(bytes);
}

function play(audioBuffer) {
    var source = context.createBufferSource();
    source.buffer = audioBuffer;
    source.connect(context.destination);
    source.start(0);
}