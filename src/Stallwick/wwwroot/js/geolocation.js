window.stallwickGeolocation = {
    getCurrentPosition: function () {
        return new Promise((resolve, reject) => {
            if (!navigator.geolocation) {
                reject('Geolocation is not supported by this browser.');
                return;
            }
            navigator.geolocation.getCurrentPosition(
                // C# interop deserializes these property names exactly.
                position => resolve({
                    Latitude: position.coords.latitude,
                    Longitude: position.coords.longitude
                }),
                error => reject(error.message),
                { enableHighAccuracy: false, timeout: 10000, maximumAge: 60000 });
        });
    }
};
