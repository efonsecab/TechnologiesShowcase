export var video;
export var canvas;
export var image;

export function getAllDevices()
{
    if (navigator.mediaDevices && navigator.mediaDevices.enumerateDevices) {
        return navigator.mediaDevices.enumerateDevices().then(function (devices) {
            var allVideoInputs = devices.filter(p => p.kind == "videoinput");
            return allVideoInputs;
            //allVideoInputs.forEach(function (device) {
            //    console.log(device.kind + ": " + device.label +
            //        " id = " + device.deviceId);
            //});
        });
    }
}
export function startCamera(videoElement, canvasElement, imageElement) {
    video = videoElement;
    canvas = canvasElement;
    image = imageElement;
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        return navigator.mediaDevices.getUserMedia({
            audio: false,
            video: {
                facingMode: "environment"
            }
        }).then(function (stream) {
            videoElement.srcObject = stream;
            setInterval(updateCanvas, 250);
        });
    }
}

export function updateCanvas()
{
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    canvas.getContext("2d").drawImage(video, 0, 0);
    image.src = canvas.toDataURL("image/png");
}

export function takePhoto(videoElement, canvasElement, imageElement) {
    canvasElement.width = videoElement.videoWidth;
    canvasElement.height = videoElement.videoHeight;
    canvasElement.getContext("2d").drawImage(videoElement, 0, 0);
    var pngDataUrl = canvasElement.toDataURL("image/png");
    var imageBinaryData = canvasElement.toDataURL("image/png")
        .replace("data:", "")
        .replace(/^.+,/, "");
    // Other browsers will fall back to image/png
    imageElement.src = canvasElement.toDataURL("image/png");
    return imageBinaryData;
}