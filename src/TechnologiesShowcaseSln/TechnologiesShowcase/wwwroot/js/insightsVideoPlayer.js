export function initializePlayer(videoElement, canvasElement, fragmentsData, facesData, timeScale) {
    var renderedEventIds = [];
    var ctx = canvasElement.getContext("2d");
    ctx.rect(10, 100, 54, 90);
    var updateCanvas  =
        function () {
            //ctx.rect(0, 0, canvasElement.width, canvasElement.height);
            ctx.clearRect(0, 0, canvasElement.width, canvasElement.height);
            ctx.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);
            ctx.stroke();
            fragmentsData.forEach(fragment => {
                var fragmentStart = fragment.start;
                var fragmentDuration = fragment.duration;
                var fragmentEnd = fragmentStart + fragmentDuration;
                var currentTimeInMilliseconds = videoElement.currentTime * timeScale;
                if (currentTimeInMilliseconds >= fragmentStart && currentTimeInMilliseconds <= fragmentEnd) {
                    if (fragment.events) {
                        fragment.events.forEach(eventMainItem => {
                            eventMainItem.forEach(eventSubItem => {
                                var matchingRenderedId = renderedEventIds.find(mId => mId == eventSubItem.id);
                                if (!matchingRenderedId) {
                                    debugger;
                                    var matchingFace = facesData.find(faceItem => { return faceItem.id == eventSubItem.id });
                                    if (matchingFace) {
                                        debugger;
                                        var x = eventSubItem.x;
                                        var y = eventSubItem.y;
                                        var faceWidth = eventSubItem.width;
                                        var faceHeight = eventSubItem.height;
                                        var canvasElementWidth = canvasElement.width;
                                        var canvasElementHeight = canvasElement.height;

                                        var convertedX = canvasElementWidth * x;
                                        var convertedY = canvasElementHeight * y;
                                        var convertedWidth = faceWidth * canvasElementWidth;
                                        var convertedHeight = faceHeight * canvasElementHeight;
                                        ctx.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);
                                        ctx.rect(convertedX, convertedY, convertedWidth, convertedHeight);
                                        ctx.font = "10px Comic Sans MS";
                                        ctx.fillStyle = "red";
                                        ctx.textAlign = "center";
                                        if (convertedY + convertedHeight >= canvasElementHeight) {
                                            ctx.fillText(matchingFace.name, convertedX, convertedY);
                                        }
                                        else {
                                            ctx.fillText(matchingFace.name, convertedX, convertedY + convertedHeight + 10);
                                        }
                                        ctx.stroke();
                                        renderedEventIds.push(eventSubItem.id);
                                    }
                                    /*
                                     * 1                x
                                     * MaxWidth         width
                                     * */

                                    console.log(eventSubItem);
                                }
                            });
                        });
                    }
                }
            });
        }
    setInterval(updateCanvas, 100);
}