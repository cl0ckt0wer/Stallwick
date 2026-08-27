window.stallwickGeolocation = {
    getCurrentPosition: function () {
        return new Promise((resolve, reject) => {
            if (!navigator.geolocation) {
                reject('Geolocation is not supported by this browser.');
                return;
            }
            navigator.geolocation.getCurrentPosition(
                position => resolve({
                    latitude: position.coords.latitude,
                    longitude: position.coords.longitude
                }),
                error => reject(error.message),
                { enableHighAccuracy: false, timeout: 10000, maximumAge: 60000 });
        });
    }
};
