// Leaflet map initialization and interaction for EcoRoute waste bins management
// This script handles all map-related functionality for the Bins Razor page

// Global map and marker variables
let mainMap = null;
let currentMarker = null;
let detailMaps = {};

// Initialize the main Leaflet map for the application
function initializeLeafletMap(dotNetRef) {
    // Include Leaflet CSS in head if not already included
    if (!document.getElementById('leaflet-css')) {
        const leafletCss = document.createElement('link');
        leafletCss.id = 'leaflet-css';
        leafletCss.rel = 'stylesheet';
        leafletCss.href = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.css';
        leafletCss.integrity = 'sha256-p4NxAoJBhIIN+hmNHrzRCf9tD/miZyoHS5obTRR9BMY=';
        leafletCss.crossOrigin = '';
        document.head.appendChild(leafletCss);
    }

    // Include Leaflet JS if not already included
    if (!window.L && !document.getElementById('leaflet-js')) {
        return new Promise((resolve) => {
            const leafletScript = document.createElement('script');
            leafletScript.id = 'leaflet-js';
            leafletScript.src = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.js';
            leafletScript.integrity = 'sha256-20nQCchB9co0qIjJZRGuk2/Z9VM+kNiyxNV1lvTlZBo=';
            leafletScript.crossOrigin = '';
            leafletScript.onload = () => {
                // After Leaflet is loaded, set up event handling and save the dotNetRef
                window.mapDotNetRef = dotNetRef;
                resolve();
            };
            document.body.appendChild(leafletScript);
        });
    } else if (window.L) {
        // Leaflet already loaded, just save the dotNetRef
        window.mapDotNetRef = dotNetRef;
        return Promise.resolve();
    }
}

// Load the main map on the edit/create modal
function loadMap(latitude, longitude) {
    // Wait for the DOM to be ready and Leaflet to be loaded
    setTimeout(() => {
        if (!window.L) {
            console.error("Leaflet is not loaded yet.");
            return;
        }

        const mapContainer = document.getElementById('map');
        if (!mapContainer) {
            console.error("Map container not found.");
            return;
        }

        // Use default coordinates if not provided
        const lat = latitude || 41.0082;
        const lng = longitude || 28.9784;

        // Destroy previous map instance if it exists
        if (mainMap) {
            mainMap.remove();
            mainMap = null;
        }

        // Create a new map instance
        mainMap = L.map('map').setView([lat, lng], 13);

        // Add the tile layer (OpenStreetMap)
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            maxZoom: 19
        }).addTo(mainMap);

        // Add a marker for the current position
        currentMarker = L.marker([lat, lng], {
            draggable: true
        }).addTo(mainMap);

        // Update coordinates when marker is dragged
        currentMarker.on('dragend', function (event) {
            const marker = event.target;
            const position = marker.getLatLng();
            window.mapDotNetRef.invokeMethodAsync('UpdateCoordinates', position.lat, position.lng);
        });

        // Update coordinates when the map is clicked
        mainMap.on('click', function (event) {
            currentMarker.setLatLng(event.latlng);
            window.mapDotNetRef.invokeMethodAsync('UpdateCoordinates', event.latlng.lat, event.latlng.lng);
        });

        // Fix the map display issue by triggering a resize after the map is shown
        setTimeout(() => {
            mainMap.invalidateSize();
        }, 100);

    }, 100); // Small delay to ensure DOM is ready
}

// Load a view-only map for the location modal

function loadLocationViewMap(containerId, latitude, longitude) {
    // Wait for the DOM to be ready and Leaflet to be loaded
    setTimeout(() => {
        if (!window.L) {
            console.error("Leaflet is not loaded yet.");
            return;
        }

        const mapContainer = document.getElementById(containerId);
        if (!mapContainer) {
            console.error(`Map container with ID ${containerId} not found.`);
            return;
        }

        // Use default coordinates if not provided
        const lat = latitude || 41.0082;
        const lng = longitude || 28.9784;

        // Create a new map instance
        const viewMap = L.map(containerId).setView([lat, lng], 15);

        // Add the tile layer (OpenStreetMap)
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            maxZoom: 19
        }).addTo(viewMap);

        // Add a marker for the position
        L.marker([lat, lng]).addTo(viewMap);

        // Fix the map display issue by triggering a resize after the map is shown
        setTimeout(() => {
            viewMap.invalidateSize();
        }, 100);

    }, 100); // Small delay to ensure DOM is ready
}

// Load small detail maps for expanded rows
// loadDetailMap fonksiyonunu güncelleyin
// loadDetailMap fonksiyonunu şu şekilde değiştirin
function loadDetailMap(containerId, latitude, longitude) {
    console.log(`Trying to load detail map for ${containerId} at coordinates: ${latitude}, ${longitude}`);

    // DOM hazır olana kadar bekle
    setTimeout(function () {
        try {
            // Leaflet var mı kontrol et
            if (!window.L) {
                console.error("Leaflet library not found! Make sure it's properly loaded before calling maps.");
                return;
            }

            // DOM elementi var mı?
            var container = document.getElementById(containerId);
            if (!container) {
                console.error(`Map container with id '${containerId}' not found!`);
                return;
            }

            // Eğer önceki harita varsa temizle
            if (detailMaps[containerId]) {
                console.log(`Removing existing map for ${containerId}`);
                detailMaps[containerId].remove();
                delete detailMaps[containerId];
            }

            console.log(`Creating new map for ${containerId}`);

            // Varsayılan değerleri kullan eğer koordinatlar geçersizse
            var lat = latitude || 41.0082;
            var lng = longitude || 28.9784;

            // Haritayı oluştur
            var detailMap = L.map(containerId, {
                zoomControl: false,
                attributionControl: false,
                dragging: false,
                scrollWheelZoom: false,
                doubleClickZoom: false
            }).setView([lat, lng], 14);

            // Harita katmanı ekle
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                maxZoom: 19
            }).addTo(detailMap);

            // İşaretçi ekle
            L.marker([lat, lng]).addTo(detailMap);

            // Referansı sakla
            detailMaps[containerId] = detailMap;

            // Haritayı canlandır
            setTimeout(function () {
                console.log(`Refreshing map display for ${containerId}`);
                detailMap.invalidateSize();
            }, 800);

            console.log(`Detail map for ${containerId} created successfully`);
        } catch (error) {
            console.error(`Error creating map for ${containerId}:`, error);
        }
    }, 300); // DOM için yeterli süre bekle
} window.addEventListener('resize', function () {
    if (mainMap) {
        mainMap.invalidateSize();
    }

    // Update all detail maps
    for (const id in detailMaps) {
        if (detailMaps[id]) {
            detailMaps[id].invalidateSize();
        }
    }
});