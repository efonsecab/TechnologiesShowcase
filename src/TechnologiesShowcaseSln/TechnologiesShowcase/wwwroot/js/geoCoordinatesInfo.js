export function getCurrentLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(ongetCurrentPositionSuccess, ongetCurrentPositionError)
    }
}

export function ongetCurrentPositionSuccess(position) {
    //DotNet.invokeMethodAsync('{APP ASSEMBLY}', 'UpdateMessageCaller', name);
    DotNet.invokeMethodAsync('TechnologiesShowcase', 'ongetCurrentLocationSuccess',
        position.coords.latitude, position.coords.longitude);
}

export function ongetCurrentPositionError(error) {
}