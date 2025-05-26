// ===========================
// UNIFIED GOOGLE MAPS INTEGRATION
// ===========================

// Global variables - Single definitions to avoid conflicts
let mainMap;
let locationPickerMap;
let markers = [];
let routePolylines = [];
let truckMarkers = [];
let wasteBinMarkers = [];
let markerCluster = null;
let currentInfoWindow = null;
let sidebarOpen = false;
let dotNetRef = null;
let selectedBinId = null;
let selectedRouteId = null;
let userLocationMarker = null;
let isDarkMode = false;
let activeSimulations = {};
let simulationIntervals = {};
let directionsService = null;

// Constants - Unified definitions
const DEFAULT_CENTER = { lat: 41.1634, lng: 27.7951 }; // Çorlu merkez
const DEFAULT_ZOOM = 13;
const FOCUS_ZOOM = 16;
const DETAIL_ZOOM = 14;
const MAX_FOCUS_ZOOM = 18;
const DEFAULT_TILT = 67.5;
const MAP_ID = '8b70db4a26fb9f4cd11929e3';
const PRIMARY_COLOR = '#3B82F4';
const RECYCLING_GREEN = "#10B981";
const SIMULATION_SPEED_KMH = 40;

// ===========================
// COMMON UTILITIES
// ===========================

// Utility function to validate and convert coordinates
function safeCoordinate(coord) {
    if (!coord) return null;

    // Handle Google Maps LatLng objects
    if (typeof coord.lat === 'function' && typeof coord.lng === 'function') {
        const lat = coord.lat();
        const lng = coord.lng();
        if (isFinite(lat) && isFinite(lng) && lat >= -90 && lat <= 90 && lng >= -180 && lng <= 180) {
            return { lat: lat, lng: lng };
        }
    }

    // Handle plain objects
    if (typeof coord.lat === 'number' && typeof coord.lng === 'number') {
        if (isFinite(coord.lat) && isFinite(coord.lng) &&
            coord.lat >= -90 && coord.lat <= 90 &&
            coord.lng >= -180 && coord.lng <= 180) {
            return { lat: coord.lat, lng: coord.lng };
        }
    }

    return null;
}

// Safe coordinate interpolation
function interpolateCoordinates(start, end, progress) {
    const safeStart = safeCoordinate(start);
    const safeEnd = safeCoordinate(end);

    if (!safeStart || !safeEnd || !isFinite(progress)) {
        return safeStart || DEFAULT_CENTER;
    }

    progress = Math.max(0, Math.min(1, progress));

    return {
        lat: safeStart.lat + (safeEnd.lat - safeStart.lat) * progress,
        lng: safeStart.lng + (safeEnd.lng - safeStart.lng) * progress
    };
}

// Debounced progress update to prevent spam
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Dark mode detection and theme management
function detectDarkMode() {
    isDarkMode = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches ||
        document.documentElement.classList.contains('dark');
    return isDarkMode;
}

// Watch for dark mode changes
if (window.matchMedia) {
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
        isDarkMode = e.matches || document.documentElement.classList.contains('dark');
        updateMapTheme();
    });
}

// Observer for dark class changes
const observer = new MutationObserver((mutations) => {
    mutations.forEach((mutation) => {
        if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
            const newDarkMode = document.documentElement.classList.contains('dark');
            if (newDarkMode !== isDarkMode) {
                isDarkMode = newDarkMode;
                updateMapTheme();
            }
        }
    });
});

observer.observe(document.documentElement, {
    attributes: true,
    attributeFilter: ['class']
});

function updateMapTheme() {
    if (mainMap) {
        mainMap.setOptions({
            styles: getMapStyles()
        });
    }
    if (locationPickerMap) {
        locationPickerMap.setOptions({
            styles: getMapStyles()
        });
    }
}

function getMapStyles() {
    detectDarkMode();

    if (isDarkMode) {
        return [
            // Dark theme styles
            { elementType: "geometry", stylers: [{ color: "#0f0f0f" }] },
            { elementType: "labels.text.stroke", stylers: [{ color: "#0f0f0f" }] },
            { elementType: "labels.text.fill", stylers: [{ color: "#757575" }] },
            {
                featureType: "administrative.locality",
                elementType: "labels.text.fill",
                stylers: [{ color: "#c4c4c4" }]
            },
            {
                featureType: "administrative.country",
                elementType: "geometry.stroke",
                stylers: [{ color: "#4e5c6e" }]
            },
            {
                featureType: "poi",
                elementType: "labels.text.fill",
                stylers: [{ color: "#6b7280" }]
            },
            {
                featureType: "poi.park",
                elementType: "geometry.fill",
                stylers: [{ color: "#1a2e1a" }]
            },
            {
                featureType: "poi.business",
                stylers: [{ visibility: "off" }]
            },
            {
                featureType: "road",
                elementType: "geometry",
                stylers: [{ color: "#1f1f1f" }]
            },
            {
                featureType: "road",
                elementType: "geometry.stroke",
                stylers: [{ color: "#0a0a0a" }]
            },
            {
                featureType: "road.highway",
                elementType: "geometry",
                stylers: [{ color: "#2d2d2d" }]
            },
            {
                featureType: "road.highway",
                elementType: "geometry.stroke",
                stylers: [{ color: "#1a1a1a" }]
            },
            {
                featureType: "road.arterial",
                elementType: "geometry",
                stylers: [{ color: "#262626" }]
            },
            {
                featureType: "road.local",
                elementType: "geometry",
                stylers: [{ color: "#1a1a1a" }]
            },
            {
                featureType: "water",
                elementType: "geometry.fill",
                stylers: [{ color: "#0a1a2a" }]
            },
            {
                featureType: "water",
                elementType: "labels.text.fill",
                stylers: [{ color: "#4a90a4" }]
            },
            {
                featureType: "transit",
                elementType: "geometry",
                stylers: [{ color: "#1e1e1e" }]
            },
            {
                featureType: "transit.station",
                elementType: "labels.text.fill",
                stylers: [{ color: "#757575" }]
            },
            {
                featureType: "landscape.man_made",
                elementType: "geometry.fill",
                stylers: [{ color: "#1a1a1a" }]
            },
            {
                featureType: "landscape.natural",
                elementType: "geometry.fill",
                stylers: [{ color: "#0d0d0d" }]
            }
        ];
    } else {
        return [
            // Light theme styles
            {
                "featureType": "all",
                "elementType": "geometry",
                "stylers": [{ "color": "#f5f5f5" }]
            },
            {
                "featureType": "landscape.man_made",
                "elementType": "geometry.fill",
                "stylers": [{ "color": "#e8f0e4" }]
            },
            {
                "featureType": "poi.park",
                "elementType": "geometry.fill",
                "stylers": [{ "color": "#c8e6c9" }]
            },
            {
                "featureType": "water",
                "elementType": "geometry.fill",
                "stylers": [{ "color": "#bbdefb" }]
            },
            {
                "featureType": "road",
                "elementType": "geometry.fill",
                "stylers": [{ "color": "#ffffff" }]
            },
            {
                "featureType": "road",
                "elementType": "geometry.stroke",
                "stylers": [{ "color": "#e0e0e0" }]
            }
        ];
    }
}

// Load MarkerClusterer library if not available
function loadMarkerClusterer() {
    return new Promise((resolve, reject) => {
        // Check if MarkerClusterer is already available
        if (window.MarkerClusterer || window.markerClusterer) {
            console.log("MarkerClusterer already available");
            resolve();
            return;
        }

        // Try loading the newer version first
        const script1 = document.createElement('script');
        script1.src = 'https://unpkg.com/@googlemaps/markerclusterer/dist/index.min.js';
        script1.onload = () => {
            console.log("New MarkerClusterer loaded successfully");
            resolve();
        };
        script1.onerror = () => {
            console.log("Failed to load new MarkerClusterer, trying legacy version");

            // Fallback to legacy version
            const script2 = document.createElement('script');
            script2.src = 'https://cdnjs.cloudflare.com/ajax/libs/js-marker-clusterer/1.0.0/markerclusterer.min.js';
            script2.onload = () => {
                console.log("Legacy MarkerClusterer loaded successfully");
                resolve();
            };
            script2.onerror = () => {
                console.error("Failed to load any MarkerClusterer library");
                reject(new Error("MarkerClusterer could not be loaded"));
            };
            document.head.appendChild(script2);
        };
        document.head.appendChild(script1);
    });
}

// Common smooth pan function
function smoothPanTo(map, position) {
    const safePos = safeCoordinate(position);
    if (!safePos) {
        console.error("Invalid coordinates for smoothPanTo:", position);
        return;
    }

    try {
        const currentCenter = map.getCenter();
        const currentLat = currentCenter.lat();
        const currentLng = currentCenter.lng();
        const targetLat = safePos.lat;
        const targetLng = safePos.lng;

        if (!isFinite(currentLat) || !isFinite(currentLng) ||
            !isFinite(targetLat) || !isFinite(targetLng)) {
            console.error("Invalid coordinate calculation in smoothPanTo");
            return;
        }

        // Calculate distance for animation timing
        const distance = google.maps.geometry.spherical.computeDistanceBetween(
            new google.maps.LatLng(currentLat, currentLng),
            new google.maps.LatLng(targetLat, targetLng)
        );

        const frames = distance > 1000 ? 60 : 30;
        const duration = distance > 1000 ? 1200 : 800;
        let frame = 0;

        const animate = () => {
            if (frame >= frames) {
                try {
                    map.panTo(safePos);
                } catch (e) {
                    console.error("Error in final panTo:", e);
                }
                return;
            }

            // Cubic bezier easing (ease-in-out)
            const t = frame / frames;
            const progress = t < 0.5 ?
                4 * t * t * t :
                1 - Math.pow(-2 * t + 2, 3) / 2;

            const lat = currentLat + (targetLat - currentLat) * progress;
            const lng = currentLng + (targetLng - currentLng) * progress;

            if (!isFinite(lat) || !isFinite(lng)) {
                console.error("Animation calculated invalid coordinates:", { lat, lng });
                return;
            }

            try {
                map.panTo({ lat, lng });
            } catch (e) {
                console.error("Error during animation panTo:", e);
                return;
            }

            frame++;
            requestAnimationFrame(animate);
        };

        animate();
    } catch (error) {
        console.error("Error in smoothPanTo:", error);
        map.panTo(safePos);
    }
}

// Common smooth zoom function
function smoothZoomTo(map, targetZoom) {
    const currentZoom = map.getZoom();
    if (currentZoom === targetZoom) return;

    const zoomDifference = Math.abs(targetZoom - currentZoom);
    const stepSize = zoomDifference > 3 ? 1 : 0.5;
    const zoomStep = currentZoom < targetZoom ? stepSize : -stepSize;

    let zoom = currentZoom;
    const duration = zoomDifference > 3 ? 200 : 150;

    const zoomInterval = setInterval(() => {
        if ((zoomStep > 0 && zoom >= targetZoom) || (zoomStep < 0 && zoom <= targetZoom)) {
            clearInterval(zoomInterval);
            map.setZoom(targetZoom);
            return;
        }

        zoom += zoomStep;
        map.setZoom(Math.round(zoom * 2) / 2);
    }, duration);
}

// Darken color utility
function darkenColor(color, percent) {
    const hex = color.replace('#', '');
    const num = parseInt(hex, 16);
    const amt = Math.round(2.55 * percent);
    const R = Math.max(0, Math.min(255, (num >> 16) - amt));
    const G = Math.max(0, Math.min(255, (num >> 8 & 0x00FF) - amt));
    const B = Math.max(0, Math.min(255, (num & 0x0000FF) - amt));
    return "#" + (0x1000000 + R * 0x10000 + G * 0x100 + B).toString(16).slice(1);
}

// ===========================
// ICON CREATION FUNCTIONS
// ===========================

function getDepotIcon(scale = 1.5) {
    const size = 45 * scale;
    const color = isDarkMode ? '#F59E0B' : '#D97706';
    const strokeColor = isDarkMode ? '#FBBF24' : '#B45309';

    const svg = `
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 ${size + 16} ${size + 20}" width="${size + 16}" height="${size + 20}">
                <defs>
                    <filter id="shadow-depot-${scale}" x="-50%" y="-50%" width="200%" height="200%">
                        <feDropShadow dx="0" dy="4" stdDeviation="6" flood-opacity="0.5"/>
                    </filter>
                    <radialGradient id="grad-depot-${scale}" cx="30%" cy="30%" r="70%">
                        <stop offset="0%" style="stop-color:${color};stop-opacity:1" />
                        <stop offset="100%" style="stop-color:${strokeColor};stop-opacity:1" />
                    </radialGradient>
                </defs>
                <circle cx="${(size + 16) / 2}" cy="${size / 2 + 8}" r="${size / 2}"
                        fill="url(#grad-depot-${scale})"
                        stroke="white"
                        stroke-width="4"
                        opacity="0.95"
                        filter="url(#shadow-depot-${scale})"/>
                <path d="M${(size + 16) / 2},${size + 8} L${(size + 16) / 2 + 8},${size + 20} L${(size + 16) / 2 - 8},${size + 20} Z"
                      fill="url(#grad-depot-${scale})"
                      stroke="white"
                      stroke-width="3"/>
                <g transform="translate(${(size + 16) / 2}, ${size / 2 + 8}) scale(1.9) translate(-12,-12)">
                    <rect x="3" y="12" width="18" height="10" fill="white" opacity="0.95" rx="1"/>
                    <polygon points="12,4 3,10 21,10" fill="white" opacity="0.95"/>
                    <rect x="5" y="13" width="2" height="8" fill="${color}" opacity="0.8"/>
                    <rect x="9" y="13" width="2" height="8" fill="${color}" opacity="0.8"/>
                    <rect x="13" y="13" width="2" height="8" fill="${color}" opacity="0.8"/>
                    <rect x="17" y="13" width="2" height="8" fill="${color}" opacity="0.8"/>
                    <rect x="11" y="4" width="1.5" height="6" fill="white" opacity="0.9"/>
                    <polygon points="12.5,4 12.5,7 18,6 12.5,5" fill="#DC2626" opacity="0.9"/>
                    <rect x="10" y="17" width="4" height="4" fill="${color}" opacity="0.9" rx="0.5"/>
                    <rect x="6" y="14" width="1.5" height="1.5" fill="${color}" opacity="0.6"/>
                    <rect x="16" y="14" width="1.5" height="1.5" fill="${color}" opacity="0.6"/>
                </g>
            </svg>
        `;

    return 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(svg)));
}

function getTruckIcon(routeColor = '#4285F4', scale = 1.5) {
    const size = 36 * scale;
    const strokeColor = darkenColor(routeColor, 20);

    const svg = `
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 ${size + 16} ${size + 20}" width="${size + 16}" height="${size + 20}">
                <defs>
                    <filter id="shadow-truck-${scale}" x="-50%" y="-50%" width="200%" height="200%">
                        <feDropShadow dx="0" dy="3" stdDeviation="4" flood-opacity="0.4"/>
                    </filter>
                    <radialGradient id="grad-truck-${scale}" cx="30%" cy="30%" r="70%">
                        <stop offset="0%" style="stop-color:${routeColor};stop-opacity:1" />
                        <stop offset="100%" style="stop-color:${strokeColor};stop-opacity:1" />
                    </radialGradient>
                </defs>
                <circle cx="${(size + 16) / 2}" cy="${size / 2 + 8}" r="${size / 2}"
                        fill="url(#grad-truck-${scale})"
                        stroke="white"
                        stroke-width="3"
                        opacity="0.95"
                        filter="url(#shadow-truck-${scale})"/>
                <path d="M${(size + 16) / 2},${size + 8} L${(size + 16) / 2 + 6},${size + 18} L${(size + 16) / 2 - 6},${size + 18} Z"
                      fill="url(#grad-truck-${scale})"
                      stroke="white"
                      stroke-width="2"/>
                <g transform="translate(${(size + 16) / 2 - 10}, ${size / 2 + 8 - 10}) scale(1.2)">
                    <rect x="2" y="8" width="12" height="8" fill="white" opacity="0.9" rx="1"/>
                    <rect x="14" y="10" width="6" height="6" fill="white" opacity="0.9" rx="1"/>
                    <rect x="3" y="9" width="4" height="3" fill="${routeColor}" opacity="0.7"/>
                    <rect x="15" y="11" width="3" height="3" fill="${routeColor}" opacity="0.7"/>
                    <circle cx="5" cy="17" r="1.5" fill="white" opacity="0.9"/>
                    <circle cx="11" cy="17" r="1.5" fill="white" opacity="0.9"/>
                    <circle cx="17" cy="17" r="1.5" fill="white" opacity="0.9"/>
                </g>
            </svg>
        `;

    return 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(svg)));
}

function getBinMarkerIcon(status, fillLevel, opacity = 1, scale = 1.5) {
    // Determine colors based on fill level
    let fillColor, strokeColor;

    if (fillLevel >= 90) {
        fillColor = '#EF4444'; // Red
        strokeColor = '#B91C1C';
    } else if (fillLevel >= 70) {
        fillColor = '#F97316'; // Orange
        strokeColor = '#C2410C';
    } else if (fillLevel >= 50) {
        fillColor = '#F59E0B'; // Amber
        strokeColor = '#B45309';
    } else if (fillLevel >= 30) {
        fillColor = PRIMARY_COLOR; // Blue
        strokeColor = '#1D4ED8';
    } else {
        fillColor = '#10B981'; // Green
        strokeColor = '#059669';
    }

    // Adjust for status
    if (status === 'Inactive') {
        fillColor = '#9CA3AF';
        strokeColor = '#6B7280';
    } else if (status === 'Maintenance') {
        fillColor = '#FBBF24';
        strokeColor = '#D97706';
    } else if (status === 'Faulty') {
        fillColor = '#F87171';
        strokeColor = '#DC2626';
    }

    const size = 32 * scale;
    const iconSize = 16 * scale;

    // Enhanced SVG with better design and animations
    const svg = `
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 ${size + 16} ${size + 20}" width="${size + 16}" height="${size + 20}">
                <defs>
                    <filter id="shadow-bin-${status}-${fillLevel}-${scale}" x="-50%" y="-50%" width="200%" height="200%">
                        <feDropShadow dx="0" dy="2" stdDeviation="3" flood-opacity="0.3"/>
                    </filter>
                    <radialGradient id="grad-bin-${status}-${fillLevel}-${scale}" cx="50%" cy="30%" r="70%">
                        <stop offset="0%" style="stop-color:${fillColor};stop-opacity:1" />
                        <stop offset="100%" style="stop-color:${strokeColor};stop-opacity:1" />
                    </radialGradient>
                    ${fillLevel >= 90 ? `
                    <animate attributeName="r" values="0;12;0" dur="2s" repeatCount="indefinite" begin="0s"/>
                    ` : ''}
                </defs>

                <!-- Pulse ring for critical bins -->
                ${fillLevel >= 90 ? `
                <circle cx="${(size + 16) / 2}" cy="${size / 2 + 8}" r="0" fill="${fillColor}" opacity="0.3">
                    <animate attributeName="r" values="0;${size / 2 + 8};0" dur="2s" repeatCount="indefinite"/>
                    <animate attributeName="opacity" values="0.3;0;0.3" dur="2s" repeatCount="indefinite"/>
                </circle>
                ` : ''}

                <!-- Main pin circle -->
                <circle cx="${(size + 16) / 2}" cy="${size / 2 + 8}" r="${size / 2}"
                        fill="url(#grad-bin-${status}-${fillLevel}-${scale})"
                        stroke="white"
                        stroke-width="3"
                        opacity="${opacity}"
                        filter="url(#shadow-bin-${status}-${fillLevel}-${scale})"/>

                <!-- Pin point -->
                <path d="M${(size + 16) / 2},${size + 8} L${(size + 16) / 2 + 6},${size + 18} L${(size + 16) / 2 - 6},${size + 18} Z"
                      fill="url(#grad-bin-${status}-${fillLevel}-${scale})"
                      stroke="white"
                      stroke-width="2"
                      opacity="${opacity}"/>

                <!-- Trash bin icon -->
                <g transform="translate(${(size + 16) / 2 - iconSize / 2}, ${size / 2 + 8 - iconSize / 2}) scale(${iconSize / 16})">
                    <!-- Bin body -->
                    <rect x="2" y="5" width="12" height="9" rx="1" fill="white" opacity="0.9"/>
                    <!-- Bin lid -->
                    <rect x="1" y="4" width="14" height="2" rx="1" fill="white" opacity="0.9"/>
                    <!-- Handle -->
                    <rect x="6" y="2" width="4" height="3" rx="1" fill="white" opacity="0.7"/>

                    <!-- Fill level indicator -->
                    <rect x="3" y="${14 - (fillLevel / 100) * 8}" width="10" height="${(fillLevel / 100) * 8}"
                          fill="${fillColor}" opacity="0.3" rx="0.5"/>

                    <!-- Recycling symbol -->
                    <g transform="translate(8, 9) scale(0.4)" fill="white" opacity="0.8">
                        <path d="M0,-3 L-1.5,-1 L1.5,-1 Z M-3,1.5 L-4.5,0 L-1.5,0 Z M3,1.5 L1.5,0 L4.5,0 Z"/>
                    </g>
                </g>

                <!-- Status indicator -->
                ${status === 'Faulty' ? `<circle cx="${size + 8}" cy="8" r="6" fill="#DC2626" stroke="white" stroke-width="2"/>
                                        <path d="M${size + 5},5 L${size + 11},11 M${size + 11},5 L${size + 5},11" stroke="white" stroke-width="2" stroke-linecap="round"/>` : ''}
                ${status === 'Maintenance' ? `<circle cx="${size + 8}" cy="8" r="6" fill="#D97706" stroke="white" stroke-width="2"/>
                                              <path d="M${size + 5},8 L${size + 11},8 M${size + 8},5 L${size + 8},11" stroke="white" stroke-width="2" stroke-linecap="round"/>` : ''}
                ${status === 'Inactive' ? `<circle cx="${size + 8}" cy="8" r="6" fill="#6B7280" stroke="white" stroke-width="2"/>
                                          <circle cx="${size + 8}" cy="8" r="2" fill="white"/>` : ''}
            </svg>
        `;

    const svgBase64 = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(svg)));

    return {
        url: svgBase64,
        scaledSize: new google.maps.Size(size + 16, size + 20),
        anchor: new google.maps.Point((size + 16) / 2, size + 18),
        labelOrigin: new google.maps.Point((size + 16) / 2, -8)
    };
}

function createClusterIcon(size, color) {
    const svg = `
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 ${size} ${size}" width="${size}" height="${size}">
                <defs>
                    <filter id="shadow-cluster-${size}" x="-50%" y="-50%" width="200%" height="200%">
                        <feDropShadow dx="0" dy="2" stdDeviation="2" flood-opacity="0.3"/>
                    </filter>
                    <radialGradient id="grad-cluster-${size}" cx="30%" cy="30%" r="70%">
                        <stop offset="0%" style="stop-color:${color};stop-opacity:1" />
                        <stop offset="100%" style="stop-color:${darkenColor(color, 20)};stop-opacity:1" />
                    </radialGradient>
                </defs>
                <circle cx="${size / 2}" cy="${size / 2}" r="${size / 2 - 2}"
                        fill="url(#grad-cluster-${size})"
                        stroke="white"
                        stroke-width="3"
                        opacity="0.95"
                        filter="url(#shadow-cluster-${size})"/>
                <circle cx="${size / 2}" cy="${size / 2}" r="${size / 2 - 8}" fill="white" opacity="0.2"/>
                <g transform="translate(${size / 2}, ${size / 2}) scale(${size / 50})">
                    <path d="M-8,-8 L-8,8 L8,8 L8,-8 Z" fill="white" opacity="0.9"/>
                    <path d="M-6,-6 L-6,6 L6,6 L6,-6 Z" fill="${color}"/>
                    <circle cx="0" cy="0" r="3" fill="white" opacity="0.8"/>
                </g>
            </svg>
        `;
    return 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(svg)));
}

// ===========================
// GLOBAL INITIALIZATION
// ===========================

/**
 * Initialize Google Maps integration
 * @param {DotNetReference} reference - .NET reference for calling Blazor methods
 */
window.initializeGoogleMaps = function (reference) {
    dotNetRef = reference;
    detectDarkMode();
    console.log("Google Maps initialization started");

    // Load MarkerClusterer library
    loadMarkerClusterer().catch(error => {
        console.warn("MarkerClusterer not available:", error);
    });
};

// ===========================
// MAIN GOOGLE MAPS INTEROP OBJECT
// ===========================

/**
 * Main object containing all Google Maps related functions
 */
window.googleMapsInterop = {

    // ===========================
    // COMMON MAP FUNCTIONS
    // ===========================

    /**
     * Initialize the main 3D map - Enhanced version
     * @param {string} mapElementId - HTML element ID for the map container
     */
    initializeMainMap: function (mapElementId = "admin-observer-map") {
        console.log("Initializing main map in element:", mapElementId);

        const mapElement = document.getElementById(mapElementId);
        if (!mapElement) {
            console.error("Main map element not found:", mapElementId);
            return false;
        }

        // Hide loading indicator when map is fully loaded
        const hideLoading = () => {
            const loadingIndicator = document.getElementById('map-loading-indicator');
            if (loadingIndicator) {
                loadingIndicator.style.opacity = '0';
                setTimeout(() => {
                    loadingIndicator.style.display = 'none';
                }, 500);
            }
        };

        try {
            // Create map centered on Çorlu with better default options
            mainMap = new google.maps.Map(mapElement, {
                center: DEFAULT_CENTER,
                zoom: DEFAULT_ZOOM,
                tilt: DEFAULT_TILT,
                heading: 0,
                mapTypeId: 'roadmap',
                mapId: MAP_ID,
                // Improved UI controls
                streetViewControl: false,
                mapTypeControl: false,
                rotateControl: true,
                zoomControl: true,
                fullscreenControl: true,
                // Enhanced interaction controls
                draggable: true,
                scrollwheel: true,
                disableDoubleClickZoom: false,
                gestureHandling: 'auto',
                clickableIcons: true,
                styles: getMapStyles()
            });

            // Initialize Directions Service for route optimization
            directionsService = new google.maps.DirectionsService();

            // Add custom controls
            this.add3DToggle(mainMap);
            this.addUserLocationButton(mainMap);

            // Map loaded event
            google.maps.event.addListenerOnce(mainMap, 'tilesloaded', () => {
                hideLoading();
                this.ensureMapInteractivity();

                if (dotNetRef) {
                    dotNetRef.invokeMethodAsync('OnMapInitialized');
                }
            });

            // Maintain map interactivity on resize
            google.maps.event.addListener(mainMap, 'resize', () => {
                this.ensureMapInteractivity();
            });

            return true;
        } catch (error) {
            console.error("Error initializing main map:", error);
            hideLoading();
            return false;
        }
    },

    /**
     * Ensure map interactivity remains active
     */
    ensureMapInteractivity: function () {
        if (mainMap) {
            mainMap.setOptions({
                draggable: true,
                scrollwheel: true,
                disableDoubleClickZoom: false,
                zoomControl: true,
                gestureHandling: 'auto',
                clickableIcons: true
            });
        }
    },

    /**
     * Add 3D/2D toggle button to map
     * @param {google.maps.Map} map - Map object
     */
    add3DToggle: function (map) {
        const toggleButton = document.createElement('button');
        toggleButton.className = 'custom-map-control-button toggle-view-button';
        toggleButton.innerHTML = '<i class="fas fa-cube"></i> 3D';
        toggleButton.title = 'Toggle 2D/3D view';

        let is3DMode = true; // Start in 3D mode

        toggleButton.addEventListener('click', () => {
            if (is3DMode) {
                // Switch to 2D
                map.setTilt(0);
                toggleButton.innerHTML = '<i class="fas fa-map"></i> 2D';
            } else {
                // Switch to 3D
                map.setTilt(DEFAULT_TILT);
                toggleButton.innerHTML = '<i class="fas fa-cube"></i> 3D';
            }
            is3DMode = !is3DMode;
        });

        map.controls[google.maps.ControlPosition.TOP_RIGHT].push(toggleButton);

        // Add styles for the toggle button
        const style = document.createElement('style');
        style.textContent = `
                .toggle-view-button {
                    background-color: ${isDarkMode ? '#1F2937' : '#fff'};
                    border: 2px solid ${isDarkMode ? '#374151' : '#fff'};
                    border-radius: 6px;
                    box-shadow: 0 3px 8px rgba(0,0,0,.2);
                    color: ${isDarkMode ? '#E5E7EB' : '#555'};
                    cursor: pointer;
                    font-family: 'Inter', -apple-system, sans-serif;
                    font-size: 14px;
                    font-weight: 500;
                    margin: 10px;
                    padding: 10px 16px;
                    text-align: center;
                    transition: all 0.3s ease;
                }
                .toggle-view-button:hover {
                    background-color: ${isDarkMode ? '#374151' : '#f8f9fa'};
                    transform: translateY(-1px);
                    box-shadow: 0 4px 12px rgba(0,0,0,.25);
                }
                .toggle-view-button i {
                    margin-right: 6px;
                }
            `;
        document.head.appendChild(style);
    },

    /**
     * Add user location button to map
     * @param {google.maps.Map} map - Map object
     */
    addUserLocationButton: function (map) {
        const locationButton = document.createElement('button');
        locationButton.className = 'custom-map-control-button location-button';
        locationButton.innerHTML = '<i class="fas fa-location-arrow"></i>';
        locationButton.title = 'Show my location';

        locationButton.addEventListener('click', () => {
            if (navigator.geolocation) {
                locationButton.innerHTML = '<i class="fas fa-spinner fa-spin"></i>';

                navigator.geolocation.getCurrentPosition(
                    (position) => {
                        const pos = {
                            lat: position.coords.latitude,
                            lng: position.coords.longitude,
                        };

                        // Remove existing user location marker if any
                        if (userLocationMarker) {
                            userLocationMarker.setMap(null);
                        }

                        // Create new marker at user location
                        userLocationMarker = new google.maps.Marker({
                            position: pos,
                            map: map,
                            icon: {
                                path: google.maps.SymbolPath.CIRCLE,
                                fillColor: PRIMARY_COLOR,
                                fillOpacity: 1,
                                strokeColor: '#FFFFFF',
                                strokeWeight: 3,
                                scale: 10,
                            },
                            title: 'Your Location',
                            animation: google.maps.Animation.DROP
                        });

                        // Add accuracy circle
                        const accuracyCircle = new google.maps.Circle({
                            center: pos,
                            radius: position.coords.accuracy,
                            map: map,
                            fillColor: PRIMARY_COLOR,
                            fillOpacity: 0.15,
                            strokeColor: PRIMARY_COLOR,
                            strokeOpacity: 0.3,
                            strokeWeight: 1,
                        });

                        // Pan to location with smooth animation
                        smoothPanTo(map, pos);

                        locationButton.innerHTML = '<i class="fas fa-location-arrow"></i>';
                    },
                    () => {
                        alert('Error: The Geolocation service failed.');
                        locationButton.innerHTML = '<i class="fas fa-location-arrow"></i>';
                    }
                );
            } else {
                alert('Error: Your browser doesn\'t support geolocation.');
            }
        });

        map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(locationButton);

        // Add styles for the location button
        const style = document.createElement('style');
        style.textContent = `
                .location-button {
                    background-color: ${isDarkMode ? '#1F2937' : '#fff'};
                    border: 2px solid ${isDarkMode ? '#374151' : '#fff'};
                    border-radius: 50%;
                    box-shadow: 0 3px 8px rgba(0,0,0,.2);
                    color: ${isDarkMode ? '#E5E7EB' : '#666'};
                    cursor: pointer;
                    height: 44px;
                    width: 44px;
                    margin: 10px;
                    padding: 0;
                    text-align: center;
                    transition: all 0.3s ease;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                }
                .location-button:hover {
                    background-color: ${isDarkMode ? '#374151' : '#f8f9fa'};
                    transform: translateY(-2px);
                    box-shadow: 0 4px 12px rgba(0,0,0,.25);
                }
                .location-button i {
                    font-size: 16px;
                }
            `;
        document.head.appendChild(style);
    },

    // ===========================
    // BIN MANAGEMENT FUNCTIONS
    // ===========================

    /**
     * Initialize the location picker map for bin creation/editing
     * @param {string} mapElementId - HTML element ID for the map container
     * @param {number} lat - Initial latitude
     * @param {number} lng - Initial longitude
     */
    initializeLocationPickerMap: function (mapElementId, lat, lng) {
        console.log("Initializing location picker map");

        const initialPosition = { lat: lat, lng: lng };

        try {
            locationPickerMap = new google.maps.Map(document.getElementById(mapElementId), {
                center: initialPosition,
                zoom: 15,
                mapTypeId: 'roadmap',
                streetViewControl: false,
                mapTypeControl: false,
                fullscreenControl: false,
                styles: getMapStyles()
            });

            // Place marker at initial position
            const marker = new google.maps.Marker({
                position: initialPosition,
                map: locationPickerMap,
                draggable: true,
                icon: getBinMarkerIcon('Active', 0, 1, 1.2),
                animation: google.maps.Animation.DROP
            });

            // Update coordinates when marker is dragged
            google.maps.event.addListener(marker, 'dragend', function () {
                const position = marker.getPosition();
                if (dotNetRef) {
                    dotNetRef.invokeMethodAsync('UpdateCoordinates', position.lat(), position.lng());

                    // Get address from coordinates (reverse geocoding)
                    const geocoder = new google.maps.Geocoder();
                    geocoder.geocode({ location: position }, function (results, status) {
                        if (status === 'OK' && results[0]) {
                            dotNetRef.invokeMethodAsync('UpdateAddress', results[0].formatted_address);
                        }
                    });
                }

                // Update displayed coordinates
                const coordsDisplay = document.getElementById('selected-coordinates');
                if (coordsDisplay) {
                    coordsDisplay.textContent = `Koordinat: ${position.lat().toFixed(6)}, ${position.lng().toFixed(6)}`;
                }
            });

            // Allow clicking on map to move marker
            google.maps.event.addListener(locationPickerMap, 'click', function (event) {
                marker.setPosition(event.latLng);

                // Trigger dragend to update coordinates
                google.maps.event.trigger(marker, 'dragend');
            });

            return true;
        } catch (error) {
            console.error("Error initializing location picker map:", error);
            return false;
        }
    },

    /**
     * Show all waste bins on the main map with clustering
     * @param {string} binsJson - JSON string of waste bin data
     */
    showAllBins: function (binsJson) {
        if (!mainMap) {
            console.error("Main map not initialized");
            return false;
        }

        // Clear existing markers and clusters
        this.clearMarkers();

        try {
            const bins = JSON.parse(binsJson);
            console.log(`Showing ${bins.length} bins on map`);

            // Store the bins data globally for later use
            window.binsData = bins;

            // Create bounds object to fit all markers
            const bounds = new google.maps.LatLngBounds();

            bins.forEach(bin => {
                if (bin.Latitude && bin.Longitude) {
                    const position = { lat: bin.Latitude, lng: bin.Longitude };

                    // Create enhanced marker
                    const marker = new google.maps.Marker({
                        position: position,
                        map: null, // Don't add to map yet, will be added via clusterer
                        title: bin.Label,
                        icon: getBinMarkerIcon(bin.DeviceStatus, bin.FillLevel, 1, 1.5),
                        binId: bin.Id,
                        binData: bin,
                        animation: google.maps.Animation.DROP
                    });

                    // Add marker to markers array
                    markers.push(marker);

                    // Add marker to bounds
                    bounds.extend(position);

                    // Add enhanced click listener
                    marker.addListener('click', () => {
                        this.onMarkerClick(marker, bin);
                    });

                    // Add hover effects
                    marker.addListener('mouseover', () => {
                        marker.setIcon(getBinMarkerIcon(bin.DeviceStatus, bin.FillLevel, 1, 1.7));
                    });

                    marker.addListener('mouseout', () => {
                        marker.setIcon(getBinMarkerIcon(bin.DeviceStatus, bin.FillLevel, 1, 1.5));
                    });
                }
            });

            // Initialize marker clustering
            this.initializeMarkerClustering();

            // Adjust map to fit all markers if there are any
            if (markers.length > 0) {
                mainMap.fitBounds(bounds);

                // If only one marker, zoom out a bit
                if (markers.length === 1) {
                    google.maps.event.addListenerOnce(mainMap, 'bounds_changed', () => {
                        mainMap.setZoom(Math.min(15, mainMap.getZoom()));
                    });
                }
            }

            return true;
        } catch (error) {
            console.error("Error showing bins on map:", error);
            return false;
        }
    },

    /**
     * Initialize marker clustering with multiple fallback methods
     */
    initializeMarkerClustering: function () {
        if (markerCluster) {
            markerCluster.clearMarkers();
            markerCluster = null;
        }

        if (markers.length === 0) {
            console.log("No markers to cluster");
            return;
        }

        // Custom cluster styles using recycling theme
        const clusterStyles = [
            {
                textColor: 'white',
                url: createClusterIcon(35, RECYCLING_GREEN),
                height: 35,
                width: 35,
                textSize: 11,
                fontFamily: 'Inter, sans-serif',
                fontWeight: 'bold'
            },
            {
                textColor: 'white',
                url: createClusterIcon(45, '#F59E0B'),
                height: 45,
                width: 45,
                textSize: 13,
                fontFamily: 'Inter, sans-serif',
                fontWeight: 'bold'
            },
            {
                textColor: 'white',
                url: createClusterIcon(55, '#EF4444'),
                height: 55,
                width: 55,
                textSize: 15,
                fontFamily: 'Inter, sans-serif',
                fontWeight: 'bold'
            }
        ];

        try {
            // Try multiple approaches to find the MarkerClusterer
            if (window.markerClusterer && window.markerClusterer.MarkerClusterer) {
                console.log("Using new MarkerClusterer (window.markerClusterer.MarkerClusterer)");
                markerCluster = new window.markerClusterer.MarkerClusterer({
                    map: mainMap,
                    markers: markers,
                    renderer: {
                        render: ({ count, position }) => {
                            const color = count < 10 ? RECYCLING_GREEN : count < 50 ? '#F59E0B' : '#EF4444';
                            const size = count < 10 ? 35 : count < 50 ? 45 : 55;

                            return new google.maps.Marker({
                                position,
                                icon: {
                                    url: createClusterIcon(size, color),
                                    scaledSize: new google.maps.Size(size, size),
                                    anchor: new google.maps.Point(size / 2, size / 2)
                                },
                                label: {
                                    text: String(count),
                                    color: 'white',
                                    fontSize: '12px',
                                    fontWeight: 'bold'
                                },
                                title: `${count} atık kutusu`,
                                zIndex: Number(google.maps.Marker.MAX_ZINDEX) + count,
                            });
                        }
                    }
                });
            }
            else if (window.MarkerClusterer) {
                console.log("Using global MarkerClusterer constructor");
                markerCluster = new window.MarkerClusterer(mainMap, markers, {
                    styles: clusterStyles,
                    gridSize: 50,
                    maxZoom: 15,
                    minimumClusterSize: 2,
                    zoomOnClick: true,
                    averageCenter: true
                });
            }
            else if (typeof MarkerClusterer === 'function') {
                console.log("Using legacy MarkerClusterer constructor");
                markerCluster = new MarkerClusterer(mainMap, markers, {
                    styles: clusterStyles,
                    gridSize: 50,
                    maxZoom: 15,
                    minimumClusterSize: 2,
                    zoomOnClick: true,
                    averageCenter: true
                });
            }
            else {
                console.warn("MarkerClusterer library not found, displaying individual markers");
                // Fallback: display individual markers
                markers.forEach(marker => marker.setMap(mainMap));
            }

            if (markerCluster) {
                console.log("MarkerClusterer initialized successfully with", markers.length, "markers");
            }
        } catch (error) {
            console.error("Error creating marker clusterer:", error);
            // Fallback: display individual markers
            markers.forEach(marker => marker.setMap(mainMap));
        }
    },

    /**
     * Handle marker click with enhanced animations
     * @param {google.maps.Marker} marker - Clicked marker
     * @param {object} bin - Bin data
     */
    onMarkerClick: function (marker, bin) {
        // Close currently open info window if any
        if (currentInfoWindow) {
            currentInfoWindow.close();
            currentInfoWindow = null;
        }

        // Get position from marker
        const position = marker.getPosition();

        // Smooth zoom with current level preservation
        const currentZoom = mainMap.getZoom();
        const targetZoom = currentZoom < 15 ? 15 : Math.min(currentZoom + 1, 17);

        smoothZoomTo(mainMap, targetZoom);

        // Set tilt
        mainMap.setTilt(DEFAULT_TILT);

        // Smooth pan
        smoothPanTo(mainMap, {
            lat: position.lat(),
            lng: position.lng()
        });

        // Brief marker animation
        marker.setAnimation(google.maps.Animation.BOUNCE);
        setTimeout(() => {
            marker.setAnimation(null);
        }, 800);

        // Open sidebar
        setTimeout(() => {
            this.openEnhancedBinSidebar(bin);
        }, 900);

        // Store selected bin ID
        selectedBinId = bin.Id;

        // Notify Blazor of bin selection
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('OpenBinDetail', bin.Id);
        }
    },

    /**
     * Open enhanced sidebar with better animations and content
     * @param {object} bin - Bin data
     */
    openEnhancedBinSidebar: function (bin) {
        // Get sidebar element
        const sidebar = document.getElementById('bin-sidebar');
        if (!sidebar) return;

        // Maintain map controls
        this.ensureMapInteractivity();

        // Calculate estimates with improved algorithm
        const getEstimatedFillDate = (fillLevel) => {
            if (!fillLevel || fillLevel >= 100) return "Bilinmiyor";

            const DAILY_FILL_RATE = 5.0;
            const remainingCapacity = 100 - fillLevel;
            const daysUntilFull = Math.ceil(remainingCapacity / DAILY_FILL_RATE);

            const estimatedDate = new Date();
            estimatedDate.setDate(estimatedDate.getDate() + daysUntilFull);

            return estimatedDate.toLocaleDateString('tr-TR');
        };

        const getEstimatedFillRate = (fillLevel, days) => {
            if (!fillLevel) return 0;
            const DAILY_FILL_RATE = 5.0;
            const estimatedFill = fillLevel + (DAILY_FILL_RATE * days);
            return Math.min(estimatedFill, 100).toFixed(0);
        };

        // Enhanced styling functions
        const getFillLevelColor = (level) => {
            if (!level) return '#6B7280';
            if (level >= 90) return '#EF4444';
            if (level >= 70) return '#F97316';
            if (level >= 50) return '#F59E0B';
            if (level >= 30) return '#3B82F6';
            return '#10B981';
        };

        // Get device status with icons
        const getDeviceStatusDisplay = (status) => {
            const statusMap = {
                'Active': { text: "Aktif", icon: "fa-check-circle", color: "#10B981" },
                'Inactive': { text: "Pasif", icon: "fa-ban", color: "#6B7280" },
                'Maintenance': { text: "Bakımda", icon: "fa-wrench", color: "#F59E0B" },
                'Faulty': { text: "Arızalı", icon: "fa-exclamation-triangle", color: "#EF4444" }
            };
            return statusMap[status] || { text: status, icon: "fa-question", color: "#6B7280" };
        };

        const deviceStatus = getDeviceStatusDisplay(bin.DeviceStatus);

        // Build enhanced sensor list
        const sensorsList = bin.Sensors && bin.Sensors.length > 0
            ? bin.Sensors.map(sensor => `
                    <div class="bg-gray-50 dark:bg-gray-700 p-3 rounded-lg border-l-4 ${sensor.IsActive ? 'border-green-500' : 'border-red-500'} mb-2 transition-transform duration-300 hover:translate-x-1">
                        <div class="flex justify-between items-center mb-1">
                            <span class="font-medium text-gray-800 dark:text-white">${sensor.Type}</span>
                            <span class="${sensor.IsActive ? 'text-green-500' : 'text-red-500'} text-xs flex items-center">
                                <i class="fas ${sensor.IsActive ? 'fa-check-circle' : 'fa-times-circle'} mr-1 ${sensor.IsActive ? 'animate-pulse' : ''}"></i>
                                ${sensor.IsActive ? 'Aktif' : 'Pasif'}
                            </span>
                        </div>
                        <div class="text-xs text-gray-500 dark:text-gray-400">
                            Son Güncelleme: ${sensor.LastUpdate ? new Date(sensor.LastUpdate).toLocaleDateString('tr-TR') : '-'}
                        </div>
                    </div>
                `).join('')
            : `<div class="bg-gray-50 dark:bg-gray-700 p-3 rounded text-center">
                      <p class="text-sm text-gray-500 dark:text-gray-400">Sensör bulunamadı</p>
                   </div>`;

        // Enhanced sidebar content with better UX
        sidebar.innerHTML = `
                <div class="h-full flex flex-col text-gray-700 dark:text-gray-200">
                    <div class="p-4 bg-primary-500 text-white flex justify-between items-center">
                        <h2 class="text-xl font-bold flex items-center">
                            <i class="fas fa-dumpster mr-2"></i> ${bin.Label}
                        </h2>
                        <button class="p-2 hover:bg-primary-600 rounded" onclick="googleMapsInterop.closeBinSidebar()">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>

                    <div class="p-4 bg-white dark:bg-gray-800 shadow-md">
                        <div class="bg-${deviceStatus.color.includes('10B981') ? 'green' : deviceStatus.color.includes('EF4444') ? 'red' : deviceStatus.color.includes('F59E0B') ? 'yellow' : 'gray'}-100 text-${deviceStatus.color.includes('10B981') ? 'green' : deviceStatus.color.includes('EF4444') ? 'red' : deviceStatus.color.includes('F59E0B') ? 'yellow' : 'gray'}-800 dark:bg-${deviceStatus.color.includes('10B981') ? 'green' : deviceStatus.color.includes('EF4444') ? 'red' : deviceStatus.color.includes('F59E0B') ? 'yellow' : 'gray'}-900/20 dark:text-${deviceStatus.color.includes('10B981') ? 'green' : deviceStatus.color.includes('EF4444') ? 'red' : deviceStatus.color.includes('F59E0B') ? 'yellow' : 'gray'}-300 px-3 py-1 inline-block rounded-full text-sm font-medium mb-2">
                            ${deviceStatus.text}
                        </div>
                        <p class="text-gray-700 dark:text-gray-300 mb-2 flex items-start">
                            <i class="fas fa-map-marker-alt text-red-500 mr-2 mt-1"></i>
                            <span>${bin.Address}</span>
                        </p>
                    </div>

                    <div class="flex-1 overflow-y-auto p-4">
                        <!-- Fill Level Status -->
                        <div class="bg-white dark:bg-gray-800 p-5 rounded-lg shadow-md transform transition-all duration-300 hover:scale-105 mb-4">
                            <div class="flex justify-between items-center mb-4">
                                <h3 class="text-lg font-semibold text-gray-800 dark:text-white flex items-center">
                                    <i class="fas fa-chart-line mr-2 text-amber-500"></i>
                                    Doluluk Durumu
                                </h3>
                            </div>

                            <div class="flex flex-col items-center justify-center space-y-3 py-2">
                                <div class="relative w-32 h-32">
                                    <svg class="w-full h-full" viewBox="0 0 36 36">
                                        <path class="stroke-current text-gray-200 dark:text-gray-700" fill="none" stroke-width="3.8" d="M18 2.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831" />
                                        <path style="stroke: ${getFillLevelColor(bin.FillLevel)}" fill="none" stroke-width="3.8" stroke-linecap="round"
                                              stroke-dasharray="${(bin.FillLevel || 0) * 100 / 100}, 100" d="M18 2.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831">
                                            <animate attributeName="stroke-dasharray"
                                                     from="0, 100"
                                                     to="${(bin.FillLevel || 0) * 100 / 100}, 100"
                                                     dur="1s"
                                                     fill="freeze"
                                                     calcMode="spline"
                                                     keyTimes="0; 1"
                                                     keySplines="0.42 0 0.58 1" />
                                        </path>
                                        <text x="18" y="20.5" class="fill-current font-bold text-gray-700 dark:text-gray-200" text-anchor="middle" font-size="${bin.FillLevel >= 100 ? '6' : '7'}">${bin.FillLevel || 0}%</text>
                                    </svg>
                                </div>

                                <div class="mt-2 space-y-2 w-full">
                                    <div class="flex justify-between items-center border-t border-gray-200 dark:border-gray-700 pt-2">
                                        <span class="text-sm text-gray-500 dark:text-gray-400">Tahmini dolma tarihi:</span>
                                        <span class="font-medium text-gray-700 dark:text-gray-300">${getEstimatedFillDate(bin.FillLevel)}</span>
                                    </div>

                                    <div class="flex justify-between items-center">
                                        <span class="text-sm text-gray-500 dark:text-gray-400">3 Gün Sonra Doluluk:</span>
                                        <span class="font-medium" style="color: ${getFillLevelColor(getEstimatedFillRate(bin.FillLevel, 3))}">
                                            %${getEstimatedFillRate(bin.FillLevel, 3)}
                                        </span>
                                    </div>

                                    <div class="flex justify-between items-center">
                                        <span class="text-sm text-gray-500 dark:text-gray-400">7 Gün Sonra Doluluk:</span>
                                        <span class="font-medium" style="color: ${getFillLevelColor(getEstimatedFillRate(bin.FillLevel, 7))}">
                                            %${getEstimatedFillRate(bin.FillLevel, 7)}
                                        </span>
                                    </div>
                                </div>
                                <div class="text-sm text-gray-500 dark:text-gray-400 mt-2">
                                    Son güncelleme: ${bin.UpdatedAt ? new Date(bin.UpdatedAt).toLocaleString('tr-TR') : '-'}
                                </div>
                            </div>
                        </div>

                        <!-- Sensors -->
                        <div class="bg-white dark:bg-gray-800 p-5 rounded-lg shadow-md transform transition-all duration-300 hover:scale-105 mb-4">
                            <div class="flex justify-between items-center mb-4">
                                <h3 class="text-lg font-semibold text-gray-800 dark:text-white flex items-center">
                                    <i class="fas fa-microchip mr-2 text-green-500"></i>
                                    Sensörler
                                </h3>
                                <span class="bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300 px-2 py-1 rounded-full text-xs">
                                    ${bin.Sensors ? bin.Sensors.length : 0} adet
                                </span>
                            </div>

                            <div class="space-y-3 max-h-60 overflow-y-auto">
                                ${sensorsList}
                            </div>
                        </div>

                        <!-- Nearby Points -->
                        <div class="bg-white dark:bg-gray-800 p-5 rounded-lg shadow-md transform transition-all duration-300 hover:scale-105">
                            <h3 class="text-lg font-semibold text-gray-800 dark:text-white mb-3 flex items-center">
                                <i class="fas fa-map-marked-alt mr-2 text-amber-500"></i> Yakındaki Atık Kutuları
                            </h3>

                            <div class="space-y-2 max-h-48 overflow-y-auto overflow-x-hidden">
                                ${this.getNearbyBins(bin)}
                            </div>
                        </div>
                    </div>

                    <!-- Bottom actions -->
                    <div class="p-4 bg-gray-100 dark:bg-gray-800 border-t border-gray-200 dark:border-gray-700 flex justify-between">
                        <button onclick="googleMapsInterop.editBin('${bin.Id}')" class="px-4 py-2 bg-blue-500 hover:bg-blue-600 text-white rounded flex items-center">
                            <i class="fas fa-edit mr-2"></i> Düzenle
                        </button>
                        <button onclick="googleMapsInterop.focusOnBin('${bin.Id}')" class="px-4 py-2 bg-primary-500 hover:bg-primary-600 text-white rounded flex items-center">
                            <i class="fas fa-search-location mr-2"></i> 3D Görünüm
                        </button>
                    </div>
                </div>
            `;

        // Show sidebar with better animation
        sidebar.classList.add('active');

        // Update map dimensions but maintain interactivity
        setTimeout(() => {
            google.maps.event.trigger(mainMap, 'resize');
            this.ensureMapInteractivity();
        }, 450);
    },

    /**
     * Get nearby bins sorted by distance with improved styling
     * @param {object} currentBin - Current bin data
     * @returns {string} - HTML for nearby bins list
     */
    getNearbyBins: function (currentBin) {
        // Find nearby bins based on actual distance from current bin
        if (!currentBin || !markers || markers.length === 0) {
            return `<div class="text-center text-gray-500 dark:text-gray-400 p-4">
                          <i class="fas fa-map-marked-alt text-2xl mb-2 opacity-50"></i>
                          <p>Yakında atık kutusu bulunamadı</p>
                        </div>`;
        }

        try {
            // Get current bin's position
            const currentPosition = { lat: currentBin.Latitude, lng: currentBin.Longitude };

            // Calculate distances for all other bins
            const nearbyBins = [];
            markers.forEach(marker => {
                // Skip the current bin
                if (marker.binId === currentBin.Id) return;

                const markerPosition = marker.getPosition();

                // Use the Google Maps geometry library to calculate distance
                const distance = google.maps.geometry.spherical.computeDistanceBetween(
                    new google.maps.LatLng(currentPosition),
                    markerPosition
                );

                // Find the bin data for this marker
                const bin = this.findBinById(marker.binId);
                if (bin && distance <= 2000) { // Only show bins within 2km
                    nearbyBins.push({
                        bin: bin,
                        distance: distance,
                        marker: marker
                    });
                }
            });

            // Sort by distance and take the closest 4
            nearbyBins.sort((a, b) => a.distance - b.distance);
            const closestBins = nearbyBins.slice(0, 4);

            if (closestBins.length === 0) {
                return `<div class="text-center text-gray-500 dark:text-gray-400 p-4">
                              <i class="fas fa-search-location text-2xl mb-2 opacity-50"></i>
                              <p>2km içinde atık kutusu bulunamadı</p>
                            </div>`;
            }

            // Create HTML for the closest bins with improved styling
            let html = '';
            closestBins.forEach((item, index) => {
                const bin = item.bin;
                const distanceInMeters = Math.round(item.distance);
                let distanceText = distanceInMeters < 1000 ?
                    `${distanceInMeters}m` :
                    `${(distanceInMeters / 1000).toFixed(1)}km`;

                // Get color and icon based on bin's fill level
                let fillColor = '#10B981';
                let fillIcon = 'fa-battery-quarter';
                let fillText = 'Az Dolu';

                if (bin.FillLevel >= 90) {
                    fillColor = '#EF4444';
                    fillIcon = 'fa-exclamation-triangle';
                    fillText = 'Kritik';
                } else if (bin.FillLevel >= 70) {
                    fillColor = '#F97316';
                    fillIcon = 'fa-fill';
                    fillText = 'Yüksek';
                } else if (bin.FillLevel >= 50) {
                    fillColor = '#F59E0B';
                    fillIcon = 'fa-fill-drip';
                    fillText = 'Orta';
                } else if (bin.FillLevel >= 30) {
                    fillColor = '#3B82F6';
                    fillIcon = 'fa-battery-half';
                    fillText = 'Az Dolu';
                }

                html += `
                        <div class="group flex items-center justify-between p-3 bg-white dark:bg-gray-700 rounded-xl hover:bg-gray-50 dark:hover:bg-gray-600 cursor-pointer mb-3
                                   transition-all duration-300 ease-out hover:scale-[1.02] hover:shadow-md border border-gray-100 dark:border-gray-600 hover:border-gray-200 dark:hover:border-gray-500"
                             onclick="googleMapsInterop.smoothFocusOnBin('${bin.Id}', ${index})"
                             style="animation-delay: ${index * 100}ms">
                            <div class="flex items-center min-w-0 flex-1">
                                <div class="relative">
                                    <div class="w-10 h-10 rounded-full flex items-center justify-center text-white shadow-sm transition-transform duration-300 group-hover:scale-110"
                                         style="background: linear-gradient(135deg, ${fillColor}, ${darkenColor(fillColor, 15)})">
                                        <i class="fas ${fillIcon} text-sm"></i>
                                    </div>
                                    ${bin.FillLevel >= 90 ? `
                                        <div class="absolute -top-1 -right-1 w-3 h-3 bg-red-500 rounded-full animate-pulse">
                                            <div class="absolute inset-0 bg-red-500 rounded-full animate-ping"></div>
                                        </div>
                                    ` : ''}
                                </div>
                                <div class="ml-3 min-w-0 flex-1">
                                    <div class="font-semibold text-gray-900 dark:text-white truncate text-sm group-hover:text-${fillColor.includes('10B981') ? 'green' : fillColor.includes('EF4444') ? 'red' : fillColor.includes('F97316') ? 'orange' : fillColor.includes('F59E0B') ? 'yellow' : 'blue'}-600 transition-colors duration-300">
                                        ${bin.Label}
                                    </div>
                                    <div class="flex items-center text-xs text-gray-500 dark:text-gray-400 mt-1 space-x-2">
                                        <span class="flex items-center">
                                            <i class="fas fa-route mr-1"></i>
                                            ${distanceText}
                                        </span>
                                        <span class="flex items-center" style="color: ${fillColor}">
                                            <i class="fas fa-percentage mr-1"></i>
                                            ${bin.FillLevel || 0}% ${fillText}
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="flex items-center text-gray-400 group-hover:text-gray-600 dark:group-hover:text-gray-300 transition-all duration-300 group-hover:translate-x-1">
                                <i class="fas fa-arrow-right text-sm"></i>
                            </div>
                        </div>
                    `;
            });

            return html;
        } catch (error) {
            console.error("Error finding nearby bins:", error);
            return `<div class="text-center text-gray-500 dark:text-gray-400 p-4">
                          <i class="fas fa-exclamation-triangle text-2xl mb-2 text-red-400"></i>
                          <p>Yakın atık kutuları hesaplanırken hata oluştu</p>
                        </div>`;
        }
    },

    /**
     * Smooth focus on nearby bin with enhanced animation and delay
     * @param {string} binId - ID of the bin to focus on
     * @param {number} animationDelay - Animation delay for staggered effects
     */
    smoothFocusOnBin: function (binId, animationDelay = 0) {
        const marker = markers.find(m => m.binId === binId);
        if (!marker) return;

        // Add click feedback animation
        const clickedElement = event.currentTarget;
        if (clickedElement) {
            clickedElement.style.transform = 'scale(0.98)';
            setTimeout(() => {
                clickedElement.style.transform = '';
            }, 150);
        }

        // Close current sidebar with smooth animation
        this.closeBinSidebar();

        setTimeout(() => {
            const position = marker.getPosition();

            // Enhanced smooth zoom and pan with easing
            const currentZoom = mainMap.getZoom();
            if (currentZoom < FOCUS_ZOOM) {
                smoothZoomTo(mainMap, FOCUS_ZOOM);
            }

            // Smooth pan with custom timing
            smoothPanTo(mainMap, {
                lat: position.lat(),
                lng: position.lng()
            });

            // Enhanced marker highlight with multiple effects
            setTimeout(() => {
                // Bounce animation
                marker.setAnimation(google.maps.Animation.BOUNCE);

                // Scale animation
                const originalIcon = marker.getIcon();
                marker.setIcon(getBinMarkerIcon(
                    marker.binData.DeviceStatus,
                    marker.binData.FillLevel,
                    1,
                    2.0
                ));

                setTimeout(() => {
                    marker.setAnimation(null);
                    marker.setIcon(originalIcon);

                    // Trigger click to open sidebar
                    google.maps.event.trigger(marker, 'click');
                }, 1000);
            }, 400);

        }, 300 + (animationDelay * 50)); // Staggered animation delay
    },

    /**
     * Focus on a specific bin in the main map - Enhanced version
     * @param {string} binId - ID of the bin to focus on
     */
    focusOnBin: function (binId) {
        if (!binId) {
            console.error("Invalid binId provided to focusOnBin");
            return false;
        }

        const marker = markers.find(m => m.binId === binId);

        if (!marker) {
            console.error("No marker found with binId:", binId);
            return false;
        }

        try {
            const position = marker.getPosition();

            if (!position || !isFinite(position.lat()) || !isFinite(position.lng())) {
                console.error("Invalid marker position:", position);
                return false;
            }

            // Current zoom level
            const currentZoom = mainMap.getZoom();

            // Smoother and user-friendly zoom
            const targetZoom = currentZoom < FOCUS_ZOOM ? FOCUS_ZOOM : Math.min(currentZoom + 1, MAX_FOCUS_ZOOM);

            // Move map smoothly
            smoothPanTo(mainMap, {
                lat: position.lat(),
                lng: position.lng()
            });

            // Gradual zoom
            setTimeout(() => {
                smoothZoomTo(mainMap, targetZoom);
            }, 500);

            // Set 3D mode but smoother
            setTimeout(() => {
                mainMap.setTilt(DEFAULT_TILT);
            }, 800);

            // Marker animation - shorter duration
            setTimeout(() => {
                marker.setAnimation(google.maps.Animation.BOUNCE);
                setTimeout(() => {
                    marker.setAnimation(null);
                    // Open sidebar but don't restrict map controls
                    this.openEnhancedBinSidebar(marker.binData);
                }, 1000); // 1 second instead of 1.5 seconds
            }, 1000);

            // Ensure map controls remain active
            this.ensureMapInteractivity();

            return true;
        } catch (e) {
            console.error("Error in focusOnBin:", e);
            return false;
        }
    },

    /**
     * Show waste bins on the location picker map (for reference)
     * @param {string} binsJson - JSON string of waste bin data
     */
    showBinsOnPickerMap: function (binsJson) {
        if (!locationPickerMap) {
            console.error("Location picker map not initialized");
            return false;
        }

        try {
            const bins = JSON.parse(binsJson);

            bins.forEach(bin => {
                if (bin.Latitude && bin.Longitude) {
                    const position = { lat: bin.Latitude, lng: bin.Longitude };

                    // Create marker with semi-transparent icon
                    const marker = new google.maps.Marker({
                        position: position,
                        map: locationPickerMap,
                        title: bin.Label,
                        icon: getBinMarkerIcon(bin.DeviceStatus, bin.FillLevel, 0.6, 1.0),
                        clickable: false
                    });
                }
            });

            return true;
        } catch (error) {
            console.error("Error showing bins on picker map:", error);
            return false;
        }
    },

    /**
     * Close the bin sidebar - Enhanced version
     */
    closeBinSidebar: function () {
        const sidebar = document.getElementById('bin-sidebar');
        if (sidebar) {
            sidebar.classList.remove('active');
        }

        // Update map dimensions and maintain controls
        setTimeout(() => {
            google.maps.event.trigger(mainMap, 'resize');
            this.ensureMapInteractivity();
        }, 450);
    },

    /**
     * Edit a bin (call .NET method)
     * @param {string} binId - ID of the bin to edit
     */
    editBin: function (binId) {
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('EditBin', binId);
        }
    },

    /**
     * Find bin data by ID
     * @param {string} binId - Bin ID to find
     * @returns {object|null} - Bin data or null if not found
     */
    findBinById: function (binId) {
        if (!window.binsData || !binId) return null;
        return window.binsData.find(bin => bin.Id === binId) || null;
    },

    // ===========================
    // ROUTE MANAGEMENT FUNCTIONS
    // ===========================

    /**
     * Initialize the start point mini map
     */
    initializeStartPointMap: function () {
        const mapElement = document.getElementById("start-point-mini-map");
        if (!mapElement) {
            console.log("Start point mini map element not found");
            return false;
        }

        try {
            const startPointMap = new google.maps.Map(mapElement, {
                center: DEFAULT_CENTER,
                zoom: 14,
                mapTypeId: 'roadmap',
                disableDefaultUI: true,
                draggable: false,
                scrollwheel: false,
                disableDoubleClickZoom: true,
                styles: getMapStyles()
            });

            new google.maps.Marker({
                position: DEFAULT_CENTER,
                map: startPointMap,
                icon: {
                    url: getDepotIcon(1.0),
                    scaledSize: new google.maps.Size(50, 60),
                    anchor: new google.maps.Point(25, 56)
                },
                title: 'Çorlu Belediyesi (Başlangıç Noktası)'
            });

            return true;
        } catch (error) {
            console.error("Error initializing start point map:", error);
            return false;
        }
    },

    /**
     * Show all routes on the map with waste bins
     * @param {string} routesJson - JSON string of routes data
     * @param {string} wasteBinsJson - JSON string of waste bins data
     */
    showAllRoutes: function (routesJson, wasteBinsJson) {
        if (!mainMap) {
            console.error("Main map not initialized");
            return false;
        }

        this.clearMarkers();
        this.clearPolylines();
        this.clearWasteBinMarkers();

        try {
            const routes = JSON.parse(routesJson);
            const wasteBins = wasteBinsJson ? JSON.parse(wasteBinsJson) : null;

            console.log(`Showing ${routes.length} routes on map`);

            if (wasteBins && wasteBins.length > 0) {
                this.showWasteBins(wasteBins);
            }

            const bounds = new google.maps.LatLngBounds();
            bounds.extend(DEFAULT_CENTER);

            // Fixed: Single consistent color per route
            const colors = ['#4285F4', '#EA4335', '#FBBC05', '#34A853', '#8E24AA', '#16A085', '#E67E22', '#E74C3C', '#2980B9', '#27AE60'];

            routes.forEach((route, routeIndex) => {
                if (route.status === 'Completed' || route.Status === 'Completed' ||
                    route.status === 2 || route.Status === 2) return;

                if (!route.steps || route.steps.length === 0) return;

                const routeColor = colors[routeIndex % colors.length]; // Fixed: Consistent color for entire route
                let path = null;

                if (route.overviewPolyline) {
                    try {
                        path = google.maps.geometry.encoding.decodePath(route.overviewPolyline);
                        if (!path || path.length === 0) {
                            path = null;
                        }
                    } catch (error) {
                        path = null;
                    }
                }

                if (!path || path.length === 0) {
                    if (route.steps && route.steps.length > 0) {
                        path = route.steps.filter(step =>
                            step.latitude && step.longitude &&
                            isFinite(step.latitude) && isFinite(step.longitude)
                        ).map(step => ({
                            lat: step.latitude,
                            lng: step.longitude
                        }));

                        if (path.length > 0) {
                            path.unshift(DEFAULT_CENTER);
                            path.push(DEFAULT_CENTER); // Fixed: Return to depot
                        } else {
                            path = [DEFAULT_CENTER];
                        }
                    } else {
                        return;
                    }
                }

                try {
                    const routePolyline = new google.maps.Polyline({
                        path: path,
                        geodesic: true,
                        strokeColor: routeColor, // Fixed: Use consistent color
                        strokeOpacity: 0.7,
                        strokeWeight: 4
                    });

                    routePolyline.setMap(mainMap);
                    routePolylines.push(routePolyline);

                    // Store route color for simulation consistency
                    route._simulationColor = routeColor;

                    // Add click event to polyline to show route info
                    routePolyline.addListener('click', (event) => {
                        this.focusRouteOnMap(route.id);
                    });

                    if (path && path.length > 0) {
                        const vehicleMarker = new google.maps.Marker({
                            position: path[0],
                            map: mainMap,
                            icon: {
                                url: getTruckIcon(routeColor, 1.0), // Fixed: Use consistent color
                                scaledSize: new google.maps.Size(36, 43),
                                anchor: new google.maps.Point(18, 39)
                            },
                            title: `Route ${route.id} - ${route.wasteType}`,
                            routeId: route.id,
                            routeData: route
                        });

                        truckMarkers.push(vehicleMarker);

                        vehicleMarker.addListener('click', () => {
                            this.focusRouteOnMap(route.id);
                        });
                    }

                    path.forEach(point => {
                        if (point && point.lat && point.lng &&
                            isFinite(point.lat) && isFinite(point.lng)) {
                            bounds.extend(point);
                        }
                    });
                } catch (error) {
                    console.error("Error creating polyline for route:", route.id, error);
                }
            });

            // Add depot marker
            const depotMarker = new google.maps.Marker({
                position: DEFAULT_CENTER,
                map: mainMap,
                icon: {
                    url: getDepotIcon(1.5),
                    scaledSize: new google.maps.Size(75, 90),
                    anchor: new google.maps.Point(37.5, 84)
                },
                title: 'Çorlu Belediyesi (Araç Deposu)',
                zIndex: 1000
            });

            markers.push(depotMarker);

            if (bounds.isEmpty()) {
                mainMap.setCenter(DEFAULT_CENTER);
                mainMap.setZoom(DEFAULT_ZOOM);
            } else {
                mainMap.fitBounds(bounds);
                google.maps.event.addListenerOnce(mainMap, 'bounds_changed', function () {
                    if (mainMap.getZoom() > 15) {
                        mainMap.setZoom(15);
                    }
                });
            }

            return true;
        } catch (error) {
            console.error("Error showing routes on map:", error);
            return false;
        }
    },

    /**
     * Generate real road path using Google Directions API
     * @param {Array} routeSteps - Array of route steps
     */
    generateRealRoutePath: async function (routeSteps) {
        if (!directionsService || !routeSteps || routeSteps.length === 0) {
            return null;
        }

        try {
            // Create waypoints from route steps
            const waypoints = routeSteps
                .filter(step => step.latitude && step.longitude &&
                    isFinite(step.latitude) && isFinite(step.longitude))
                .map(step => ({
                    location: new google.maps.LatLng(step.latitude, step.longitude),
                    stopover: true
                }));

            if (waypoints.length === 0) {
                return null;
            }

            const origin = DEFAULT_CENTER;
            const destination = DEFAULT_CENTER; // Fixed: Return to depot
            const intermediateWaypoints = waypoints;

            return new Promise((resolve, reject) => {
                directionsService.route({
                    origin: origin,
                    destination: destination,
                    waypoints: intermediateWaypoints.slice(0, 23), // Google limits to 23 waypoints
                    travelMode: google.maps.TravelMode.DRIVING,
                    avoidTolls: false,
                    avoidHighways: false,
                    optimizeWaypoints: true // Fixed: Route optimization
                }, (result, status) => {
                    if (status === google.maps.DirectionsStatus.OK) {
                        // Extract detailed path from directions result
                        const detailedPath = [];
                        const route = result.routes[0];

                        route.legs.forEach(leg => {
                            leg.steps.forEach(step => {
                                const stepPath = google.maps.geometry.encoding.decodePath(step.polyline.points);
                                detailedPath.push(...stepPath);
                            });
                        });

                        resolve({
                            path: detailedPath,
                            waypoints: waypoints,
                            directionsResult: result
                        });
                    } else {
                        console.warn('Directions request failed:', status);
                        resolve(null);
                    }
                });
            });
        } catch (error) {
            console.error('Error generating real route path:', error);
            return null;
        }
    },

    /**
     * Start route simulation with enhanced features
     * @param {string} routeId - Route ID
     * @param {string} routeData - JSON string of route data
     */
    startRouteSimulation: async function (routeId, routeData) {
        if (activeSimulations[routeId]) {
            console.log(`Simulation already running for route ${routeId}`);
            return false;
        }

        try {
            const route = JSON.parse(routeData);
            if (!route.steps || route.steps.length === 0) {
                console.error('Route has no steps for simulation');
                return false;
            }

            // Generate real road path
            console.log('Generating optimized route path for:', routeId);
            const routePathData = await this.generateRealRoutePath(route.steps);

            let path = [];
            let waypoints = [];

            if (routePathData && routePathData.path && routePathData.path.length > 1) {
                path = routePathData.path.map(p => safeCoordinate(p)).filter(p => p);
                waypoints = routePathData.waypoints;
                console.log(`✅ Optimized route generated with ${path.length} points`);
            } else {
                // Fallback path
                console.warn('⚠️ Using fallback path');
                path = route.steps
                    .filter(step => step.latitude && step.longitude &&
                        isFinite(step.latitude) && isFinite(step.longitude))
                    .map(step => ({
                        lat: step.latitude,
                        lng: step.longitude
                    }));

                if (path.length > 0) {
                    path.unshift(DEFAULT_CENTER);
                    path.push(DEFAULT_CENTER); // Fixed: Return to depot
                }
            }

            if (path.length < 2) {
                console.error('Insufficient coordinates for simulation');
                return false;
            }

            // Use consistent route color
            const routeColor = route._simulationColor || '#FF6B35';

            // Create simulation vehicle marker
            const simulationMarker = new google.maps.Marker({
                position: path[0],
                map: mainMap,
                icon: {
                    url: getTruckIcon(routeColor, 1.6),
                    scaledSize: new google.maps.Size(65, 78),
                    anchor: new google.maps.Point(32.5, 74)
                },
                title: `🚛 Simülasyon - ${route.routeName || 'Rota'}`,
                zIndex: 2000
            });

            // Create polylines with consistent color
            const traveledPath = new google.maps.Polyline({
                path: [path[0]],
                geodesic: true,
                strokeColor: '#28a745',
                strokeOpacity: 0.9,
                strokeWeight: 8,
                zIndex: 100
            });
            traveledPath.setMap(mainMap);

            const remainingPath = new google.maps.Polyline({
                path: path,
                geodesic: true,
                strokeColor: routeColor,
                strokeOpacity: 0.6,
                strokeWeight: 6,
                zIndex: 50
            });
            remainingPath.setMap(mainMap);

            // Fixed: Calculate smooth progress parameters
            const totalDistance = route.totalDistanceKm || 10;
            const baseSpeed = SIMULATION_SPEED_KMH;
            let currentSpeed = 1;

            const intervalMs = 500; // Fixed: Longer interval for better performance
            const totalSimulationTime = Math.max(30000, totalDistance * 3000); // Min 30 seconds
            const totalSteps = Math.floor(totalSimulationTime / intervalMs);

            let currentStep = 0;
            let currentPathIndex = 0;
            let completedRouteSteps = 0;

            // Store simulation data
            activeSimulations[routeId] = {
                marker: simulationMarker,
                traveledPath: traveledPath,
                remainingPath: remainingPath,
                route: route,
                path: path,
                waypoints: waypoints,
                currentStep: currentStep,
                totalSteps: totalSteps,
                isRunning: true,
                speed: currentSpeed,
                completedRouteSteps: completedRouteSteps,
                lastStepTime: 0,
                currentProgress: 0 // Fixed: Track smooth progress
            };

            // 3D focus and camera settings
            mainMap.setTilt(67.5);
            mainMap.setHeading(0);
            mainMap.setZoom(17);
            mainMap.panTo(path[0]);

            setTimeout(() => {
                mainMap.setTilt(67.5);
                mainMap.setZoom(16);
            }, 500);

            // Show simulation start notification
            if (dotNetRef) {
                dotNetRef.invokeMethodAsync('ShowToastFromJs', '🚀 Rota simülasyonu başlatıldı!');
                dotNetRef.invokeMethodAsync('OnSimulationStarted', routeId);
            }

            // Fixed: Debounced progress update to prevent spam
            const debouncedProgressUpdate = debounce((routeId, progress, steps) => {
                this.updateSimulationProgress(routeId, progress, steps);
            }, 100);

            // Start simulation interval
            simulationIntervals[routeId] = setInterval(async () => {
                const simulation = activeSimulations[routeId];
                if (!simulation || !simulation.isRunning) {
                    return;
                }

                // Update current step
                simulation.currentStep += simulation.speed;
                const pathProgress = (simulation.currentStep / simulation.totalSteps) * path.length;
                currentPathIndex = Math.min(Math.floor(pathProgress), path.length - 1);

                if (currentPathIndex >= path.length - 1) {
                    this.completeSimulation(routeId);
                    return;
                }

                // Calculate current position with safe interpolation
                let currentPosition = path[currentPathIndex];

                if (currentPathIndex < path.length - 1) {
                    const segmentProgress = pathProgress - currentPathIndex;
                    currentPosition = interpolateCoordinates(
                        path[currentPathIndex],
                        path[currentPathIndex + 1],
                        segmentProgress
                    );
                }

                // Validate and set marker position
                const safePosition = safeCoordinate(currentPosition);
                if (safePosition) {
                    try {
                        simulationMarker.setPosition(safePosition);
                    } catch (error) {
                        console.error('Error setting marker position:', error);
                        return;
                    }
                }

                // Update polylines
                const traveledSegment = path.slice(0, currentPathIndex + 1);
                if (currentPathIndex < path.length - 1 && safePosition) {
                    traveledSegment.push(safePosition);
                }

                try {
                    traveledPath.setPath(traveledSegment);
                    remainingPath.setPath(path.slice(currentPathIndex));
                } catch (error) {
                    console.error('Error updating polylines:', error);
                }

                // Fixed: Smooth progress calculation (0-100%)
                const currentProgress = Math.min((simulation.currentStep / simulation.totalSteps) * 100, 100);
                simulation.currentProgress = currentProgress;

                // Step completion logic based on route steps
                const totalRouteSteps = route.steps.length;
                const expectedCompletedSteps = Math.floor((currentProgress / 100) * totalRouteSteps);

                if (expectedCompletedSteps > simulation.completedRouteSteps &&
                    (Date.now() - simulation.lastStepTime) > 3000) {

                    simulation.completedRouteSteps = expectedCompletedSteps;
                    simulation.lastStepTime = Date.now();

                    try {
                        await dotNetRef.invokeMethodAsync('CompleteNextSimulationStep', routeId);

                        // Step completion notification
                        const stepNumber = expectedCompletedSteps;
                        if (stepNumber === 1) {
                            dotNetRef.invokeMethodAsync('ShowToastFromJs', `🚀 ${stepNumber}. adım: Başlangıç noktasından ayrıldı`);
                        } else if (stepNumber === totalRouteSteps) {
                            dotNetRef.invokeMethodAsync('ShowToastFromJs', `🏁 ${stepNumber}. adım: Başlangıç noktasına döndü!`);
                        } else {
                            const stepInfo = route.steps[stepNumber - 1];
                            const address = stepInfo?.address || `${stepNumber}. Durak`;
                            dotNetRef.invokeMethodAsync('ShowToastFromJs', `📍 ${stepNumber}. adım: ${address} tamamlandı`);
                        }
                    } catch (error) {
                        console.error('Error completing simulation step:', error);
                    }
                }

                // Fixed: Update progress smoothly
                debouncedProgressUpdate(routeId, currentProgress, simulation.completedRouteSteps);

                // Fixed: Optimized camera tracking (less frequent updates)
                if (simulation.currentStep % 15 === 0 && safePosition) {
                    // Calculate heading based on movement
                    if (currentPathIndex > 0 && currentPathIndex < path.length - 1) {
                        try {
                            const prevPoint = safeCoordinate(path[currentPathIndex - 1]);
                            const nextPoint = safeCoordinate(path[currentPathIndex + 1]);
                            if (prevPoint && nextPoint) {
                                const heading = google.maps.geometry.spherical.computeHeading(prevPoint, nextPoint);
                                if (isFinite(heading)) {
                                    mainMap.setHeading(heading);
                                }
                            }
                        } catch (error) {
                            console.error('Error calculating heading:', error);
                        }
                    }

                    // Smooth camera follow
                    try {
                        mainMap.panTo(safePosition);

                        // Maintain 3D view
                        if (mainMap.getTilt() < 60) {
                            mainMap.setTilt(67.5);
                        }
                    } catch (error) {
                        console.error('Error updating camera:', error);
                    }
                }

            }, intervalMs);

            return true;
        } catch (error) {
            console.error('Error starting route simulation:', error);
            return false;
        }
    },

    /**
     * Update simulation speed
     * @param {string} routeId - Route ID
     * @param {number} newSpeed - New speed multiplier
     */
    updateSimulationSpeed: function (routeId, newSpeed) {
        if (activeSimulations[routeId]) {
            activeSimulations[routeId].speed = Math.max(0.1, Math.min(10, newSpeed));

            // Update speed display
            const speedDisplay = document.getElementById(`speed-display-${routeId}`);
            if (speedDisplay) {
                speedDisplay.textContent = `${newSpeed}x`;
            }

            // Show speed change notification
            if (dotNetRef) {
                dotNetRef.invokeMethodAsync('ShowToastFromJs', `⚡ Simülasyon hızı ${newSpeed}x olarak ayarlandı`);
            }
        }
    },

    /**
     * Complete simulation
     * @param {string} routeId - Route ID
     */
    completeSimulation: function (routeId) {
        if (!activeSimulations[routeId]) return;

        const simulation = activeSimulations[routeId];
        simulation.isRunning = false;

        // Clear interval
        if (simulationIntervals[routeId]) {
            clearInterval(simulationIntervals[routeId]);
            delete simulationIntervals[routeId];
        }

        // Complete all remaining steps in database
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('CompleteRouteSimulation', routeId);
        }

        // Update marker to show completion
        simulation.marker.setIcon({
            url: getTruckIcon('#28a745', 1.4),
            scaledSize: new google.maps.Size(55, 66),
            anchor: new google.maps.Point(27.5, 62)
        });

        // Fixed: Final progress update
        this.updateSimulationProgress(routeId, 100, simulation.route.steps.length);

        // Show completion notification
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('ShowToastFromJs', '🎉 Rota simülasyonu başarıyla tamamlandı!');
            dotNetRef.invokeMethodAsync('OnSimulationCompleted', routeId);
        }

        // Clean up after 5 seconds
        setTimeout(() => {
            this.cleanupSimulation(routeId);
        }, 5000);
    },

    /**
     * Stop simulation
     * @param {string} routeId - Route ID
     */
    stopSimulation: function (routeId) {
        if (!activeSimulations[routeId]) return false;

        const simulation = activeSimulations[routeId];
        simulation.isRunning = false;

        if (simulationIntervals[routeId]) {
            clearInterval(simulationIntervals[routeId]);
            delete simulationIntervals[routeId];
        }

        // Show stop notification
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('ShowToastFromJs', '⏹️ Rota simülasyonu durduruldu');
            dotNetRef.invokeMethodAsync('OnSimulationStopped', routeId);
        }

        this.cleanupSimulation(routeId);
        return true;
    },

    /**
     * Cleanup simulation resources
     * @param {string} routeId - Route ID
     */
    cleanupSimulation: function (routeId) {
        if (!activeSimulations[routeId]) return;

        const simulation = activeSimulations[routeId];

        // Remove markers and polylines safely
        try {
            if (simulation.marker) simulation.marker.setMap(null);
            if (simulation.traveledPath) simulation.traveledPath.setMap(null);
            if (simulation.remainingPath) simulation.remainingPath.setMap(null);
        } catch (error) {
            console.error('Error cleaning up simulation:', error);
        }

        // Clear from active simulations
        delete activeSimulations[routeId];

        // Refresh the route data
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('RefreshData');
        }
    },

    /**
     * Update simulation progress with optimized error handling
     * @param {string} routeId - Route ID
     * @param {number} progress - Progress percentage
     * @param {number} completedSteps - Completed steps count
     */
    updateSimulationProgress: function (routeId, progress, completedSteps) {
        const safeProgress = Math.max(0, Math.min(100, progress || 0));

        try {
            // Update progress circle
            const progressCircle = document.getElementById(`progress-circle-${routeId}`);
            if (progressCircle) {
                progressCircle.setAttribute('stroke-dasharray', `${safeProgress}, 100`);
            }

            // Update progress text
            const progressText = document.getElementById(`progress-text-${routeId}`);
            if (progressText) {
                progressText.textContent = `${Math.round(safeProgress)}%`;
            }

            // Update progress bar
            const progressBar = document.getElementById(`progress-bar-${routeId}`);
            if (progressBar) {
                progressBar.style.width = `${safeProgress}%`;

                // Update color based on progress
                const colorClass = safeProgress >= 100 ? 'bg-green-500' :
                    safeProgress >= 75 ? 'bg-blue-500' :
                        safeProgress >= 50 ? 'bg-yellow-500' : 'bg-red-500';
                progressBar.className = `${colorClass} h-2.5 rounded-full transition-all duration-500 ease-in-out`;
            }

            // Update step counts
            const completedStepsEl = document.getElementById(`completed-steps-${routeId}`);
            if (completedStepsEl) {
                completedStepsEl.textContent = `${completedSteps || 0} adet`;
            }

            const remainingStepsEl = document.getElementById(`remaining-steps-${routeId}`);
            if (remainingStepsEl) {
                const totalSteps = activeSimulations[routeId]?.route?.steps?.length || 0;
                remainingStepsEl.textContent = `${Math.max(0, totalSteps - (completedSteps || 0))} adet`;
            }

            // Update simulation status
            const statusEl = document.getElementById(`simulation-status-${routeId}`);
            if (statusEl) {
                if (safeProgress >= 100) {
                    statusEl.textContent = 'Simülasyon tamamlandı!';
                    statusEl.className = 'text-green-600 font-semibold';
                } else {
                    statusEl.textContent = `${Math.round(safeProgress)}% tamamlandı - ${completedSteps || 0} adım`;
                    statusEl.className = 'text-blue-600 font-medium';
                }
            }

            // Fixed: CSS class based selectors
            const progressElements = document.querySelectorAll(`[data-route-id="${routeId}"]`);
            progressElements.forEach(element => {
                if (element.classList.contains('progress-bar')) {
                    element.style.width = `${safeProgress}%`;
                } else if (element.classList.contains('progress-text')) {
                    element.textContent = `${Math.round(safeProgress)}%`;
                } else if (element.classList.contains('step-counter')) {
                    element.textContent = `${completedSteps} / ${activeSimulations[routeId]?.route?.steps?.length || 0}`;
                }
            });

            // Fixed: Window event dispatch for C# to catch
            window.dispatchEvent(new CustomEvent('simulationProgressUpdate', {
                detail: {
                    routeId: routeId,
                    progress: safeProgress,
                    completedSteps: completedSteps,
                    totalSteps: activeSimulations[routeId]?.route?.steps?.length || 0
                }
            }));
        } catch (error) {
            console.error('Error updating simulation progress:', error);
        }
    },

    /**
     * Show waste bins for route display
     * @param {Array} wasteBins - Array of waste bin data
     */
    showWasteBins: function (wasteBins) {
        if (!mainMap || !wasteBins || wasteBins.length === 0) return false;

        const binMarkers = [];
        wasteBins.forEach(bin => {
            if (bin.latitude && bin.longitude &&
                isFinite(bin.latitude) && isFinite(bin.longitude)) {
                const position = { lat: bin.latitude, lng: bin.longitude };

                const marker = new google.maps.Marker({
                    position: position,
                    icon: {
                        url: getBinMarkerIcon(bin.deviceStatus || 'Active', bin.fillLevel || 0, 0.8, 1.0).url,
                        scaledSize: new google.maps.Size(40, 46),
                        anchor: new google.maps.Point(20, 44)
                    },
                    title: `${bin.label} - ${bin.fillLevel || 0}% dolu`,
                    opacity: 0.8
                });

                const infoWindow = new google.maps.InfoWindow({
                    content: this.createBinInfoContent(bin)
                });

                marker.addListener('click', () => {
                    if (currentInfoWindow) {
                        currentInfoWindow.close();
                    }
                    infoWindow.open(mainMap, marker);
                    currentInfoWindow = infoWindow;
                });

                binMarkers.push(marker);
                wasteBinMarkers.push(marker);
            }
        });

        this.createMarkerClusterer(binMarkers);
        return true;
    },

    /**
     * Create marker clusterer for waste bins
     * @param {Array} markers - Array of markers to cluster
     */
    createMarkerClusterer: function (binMarkers) {
        try {
            if (markerCluster) {
                markerCluster.clearMarkers();
                markerCluster = null;
            }

            if (binMarkers.length > 0) {
                const clusterStyles = [
                    {
                        textColor: 'white',
                        url: createClusterIcon(35, RECYCLING_GREEN),
                        height: 35,
                        width: 35,
                        textSize: 11
                    },
                    {
                        textColor: 'white',
                        url: createClusterIcon(45, '#F59E0B'),
                        height: 45,
                        width: 45,
                        textSize: 13
                    },
                    {
                        textColor: 'white',
                        url: createClusterIcon(55, '#EF4444'),
                        height: 55,
                        width: 55,
                        textSize: 15
                    }
                ];

                if (window.markerClusterer && window.markerClusterer.MarkerClusterer) {
                    markerCluster = new window.markerClusterer.MarkerClusterer({
                        map: mainMap,
                        markers: binMarkers
                    });
                } else if (window.MarkerClusterer) {
                    markerCluster = new window.MarkerClusterer(mainMap, binMarkers, {
                        styles: clusterStyles
                    });
                } else {
                    binMarkers.forEach(marker => marker.setMap(mainMap));
                }
            }
        } catch (error) {
            console.warn("Error creating marker clusterer:", error);
            binMarkers.forEach(marker => marker.setMap(mainMap));
        }
    },

    /**
     * Create bin info window content
     * @param {object} bin - Bin data
     * @returns {string} - HTML content for info window
     */
    createBinInfoContent: function (bin) {
        const getFillLevelColor = (level) => {
            if (!level) return '#6B7280';
            if (level >= 90) return '#EF4444';
            if (level >= 70) return '#F97316';
            if (level >= 50) return '#F59E0B';
            return '#10B981';
        };

        return `
                <div style="padding: 12px; max-width: 280px; font-family: Arial, sans-serif;">
                    <div style="font-weight: bold; font-size: 16px; margin-bottom: 8px; color: #1F2937;">
                        ${bin.label}
                    </div>
                    <div style="margin-bottom: 6px; font-size: 14px; color: #6B7280;">
                        ${bin.address}
                    </div>
                    <div style="display: flex; align-items: center; margin-bottom: 8px;">
                        <span style="margin-right: 8px; font-size: 14px; color: #6B7280;">Doluluk:</span>
                        <span style="font-weight: bold; color: ${getFillLevelColor(bin.fillLevel)};">
                            ${bin.fillLevel || 0}%
                        </span>
                    </div>
                    <div style="width: 100%; height: 8px; background-color: #E5E7EB; border-radius: 4px; overflow: hidden;">
                        <div style="height: 100%; background-color: ${getFillLevelColor(bin.fillLevel)}; width: ${bin.fillLevel || 0}%; transition: width 0.3s ease;"></div>
                    </div>
                </div>
            `;
    },

    /**
     * Focus route on map
     * @param {string} routeId - Route ID to focus on
     */
    focusRouteOnMap: function (routeId) {
        const marker = truckMarkers.find(m => m.routeId === routeId);
        if (!marker) return false;

        try {
            const position = marker.getPosition();
            const safePos = safeCoordinate(position);
            if (safePos) {
                mainMap.setCenter(safePos);
                mainMap.setZoom(FOCUS_ZOOM);

                marker.setAnimation(google.maps.Animation.BOUNCE);
                setTimeout(() => {
                    marker.setAnimation(null);
                }, 1500);
            }

            // Trigger route expansion in table
            if (dotNetRef) {
                dotNetRef.invokeMethodAsync('FocusRouteOnMap', routeId);
            }

            return true;
        } catch (error) {
            console.error("Error focusing route on map:", error);
            return false;
        }
    },

    // ===========================
    // CLEANUP AND UTILITY FUNCTIONS
    // ===========================

    /**
     * Clear all markers from the map
     */
    clearMarkers: function () {
        markers.forEach(marker => {
            try {
                marker.setMap(null);
            } catch (e) { }
        });
        markers = [];
        truckMarkers.forEach(marker => {
            try {
                marker.setMap(null);
            } catch (e) { }
        });
        truckMarkers = [];
    },

    /**
     * Clear all polylines from the map
     */
    clearPolylines: function () {
        routePolylines.forEach(polyline => {
            try {
                polyline.setMap(null);
            } catch (e) { }
        });
        routePolylines = [];
    },

    /**
     * Clear waste bin markers
     */
    clearWasteBinMarkers: function () {
        if (markerCluster) {
            try {
                markerCluster.clearMarkers();
                markerCluster = null;
            } catch (e) { }
        }
        wasteBinMarkers.forEach(marker => {
            try {
                marker.setMap(null);
            } catch (e) { }
        });
        wasteBinMarkers = [];
    },

    /**
     * Reset the map state - Enhanced version
     */
    resetMapState: function () {
        if (currentInfoWindow) {
            currentInfoWindow.close();
            currentInfoWindow = null;
        }

        this.closeBinSidebar();
        selectedBinId = null;
        selectedRouteId = null;

        // Stop all active simulations
        Object.keys(activeSimulations).forEach(routeId => {
            this.stopSimulation(routeId);
        });

        // Return map to default state
        if (mainMap) {
            // Smooth zoom-out
            const currentZoom = mainMap.getZoom();
            if (currentZoom > DEFAULT_ZOOM + 2) {
                smoothZoomTo(mainMap, DEFAULT_ZOOM + 1);
            }

            // Ensure map controls
            this.ensureMapInteractivity();
        }
    },

    /**
     * Dispose all resources
     */
    disposeResources: function () {
        this.clearMarkers();
        this.clearPolylines();
        this.clearWasteBinMarkers();
        this.resetMapState();
    }
};

// ===========================
// GLOBAL UTILITY FUNCTIONS
// ===========================

/**
 * Reset filter dropdowns (called from Blazor)
 */
window.resetFilterDropdowns = function () {
    const statusDropdown = document.querySelector('select[onchange*="FilterByStatus"]');
    const fillLevelDropdown = document.querySelector('select[onchange*="FilterByFillLevel"]');

    if (statusDropdown) statusDropdown.value = '';
    if (fillLevelDropdown) fillLevelDropdown.value = '';
};

// ===========================
// INITIALIZATION
// ===========================

// Initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function () {
        detectDarkMode();
        console.log('Google Maps Integration: DOM ready');
    });
} else {
    detectDarkMode();
    console.log('Google Maps Integration: Script loaded');
}// JavaScript helper functions for Bin Management

    