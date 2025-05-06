// googlemaps.js - EcoRoute Atık Kutusu Yönetim Sistemi için Google Maps entegrasyonu

// Global değişkenler
let map = null;
let markers = [];
let infoWindow = null;
let createMarker = null;
let dotNetHelper = null;
let detailMaps = {};
let locationMap = null;
let mapInitialized = false;
let mapTries = {};

// Google Maps nesnesini başlat
function initializeGoogleMaps(dotNetRef) {
    dotNetHelper = dotNetRef;

    // Global fonksiyonlar
    window.googleMapsInterop = {
        initializeMap,
        updateMarkerPosition,
        showNearbyBins,
        loadLocationMap,
        loadDetailMap,
        showNearbyBinsOnLocationMap,
        showAllBins,
        focusOnBin,
        resetMapState,
        getMapTheme
    };

    // API callback
    window.initGoogleMapsCallback = function () {
        console.log("Google Maps API yüklendi");
        if (dotNetHelper) dotNetHelper.invokeMethodAsync('OnMapInitialized');
    };
}

function getMapTheme() {
    // Check if dark mode is active
    const isDarkMode = document.documentElement.classList.contains('dark');

    if (isDarkMode) {
        return [
            { elementType: "geometry", stylers: [{ color: "#242f3e" }] },
            { elementType: "labels.text.stroke", stylers: [{ color: "#242f3e" }] },
            { elementType: "labels.text.fill", stylers: [{ color: "#746855" }] },
            {
                featureType: "administrative.locality",
                elementType: "labels.text.fill",
                stylers: [{ color: "#d59563" }],
            },
            {
                featureType: "poi",
                elementType: "labels.text.fill",
                stylers: [{ color: "#d59563" }],
            },
            {
                featureType: "poi.park",
                elementType: "geometry",
                stylers: [{ color: "#263c3f" }],
            },
            {
                featureType: "poi.park",
                elementType: "labels.text.fill",
                stylers: [{ color: "#6b9a76" }],
            },
            {
                featureType: "road",
                elementType: "geometry",
                stylers: [{ color: "#38414e" }],
            },
            {
                featureType: "road",
                elementType: "geometry.stroke",
                stylers: [{ color: "#212a37" }],
            },
            {
                featureType: "road",
                elementType: "labels.text.fill",
                stylers: [{ color: "#9ca5b3" }],
            },
            {
                featureType: "road.highway",
                elementType: "geometry",
                stylers: [{ color: "#746855" }],
            },
            {
                featureType: "road.highway",
                elementType: "geometry.stroke",
                stylers: [{ color: "#1f2835" }],
            },
            {
                featureType: "road.highway",
                elementType: "labels.text.fill",
                stylers: [{ color: "#f3d19c" }],
            },
            {
                featureType: "transit",
                elementType: "geometry",
                stylers: [{ color: "#2f3948" }],
            },
            {
                featureType: "transit.station",
                elementType: "labels.text.fill",
                stylers: [{ color: "#d59563" }],
            },
            {
                featureType: "water",
                elementType: "geometry",
                stylers: [{ color: "#17263c" }],
            },
            {
                featureType: "water",
                elementType: "labels.text.fill",
                stylers: [{ color: "#515c6d" }],
            },
            {
                featureType: "water",
                elementType: "labels.text.stroke",
                stylers: [{ color: "#17263c" }],
            },
        ];
    } else {
        // Light mode theme - subtle and clean
        return [
            {
                featureType: "administrative",
                elementType: "labels.text.fill",
                stylers: [{ color: "#444444" }],
            },
            {
                featureType: "landscape",
                elementType: "all",
                stylers: [{ color: "#f2f2f2" }],
            },
            {
                featureType: "poi",
                elementType: "all",
                stylers: [{ visibility: "off" }],
            },
            {
                featureType: "road",
                elementType: "all",
                stylers: [{ saturation: -100 }, { lightness: 45 }],
            },
            {
                featureType: "road.highway",
                elementType: "all",
                stylers: [{ visibility: "simplified" }],
            },
            {
                featureType: "road.arterial",
                elementType: "labels.icon",
                stylers: [{ visibility: "off" }],
            },
            {
                featureType: "transit",
                elementType: "all",
                stylers: [{ visibility: "off" }],
            },
            {
                featureType: "water",
                elementType: "all",
                stylers: [{ color: "#d4e4eb" }, { visibility: "on" }],
            },
        ];
    }
}

// Ana harita başlatma
function initializeMap() {
    resetMapState();

    const mapElement = document.getElementById('google-map');
    if (!mapElement) return false;

    // Apply theme based on dark/light mode
    const mapStyles = getMapTheme();

    map = new google.maps.Map(mapElement, {
        center: { lat: 41.0082, lng: 28.9784 },
        zoom: 12,
        mapTypeControl: false,
        streetViewControl: false,
        fullscreenControl: false,
        styles: mapStyles,
        zoomControlOptions: {
            position: google.maps.ControlPosition.RIGHT_BOTTOM
        },
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.DROPDOWN_MENU
        }
    });

    // Add custom controls for better UX
    const customControlDiv = document.createElement('div');
    customControlDiv.className = 'custom-map-controls';
    customControlDiv.innerHTML = `
        <div class="control-container" style="margin: 10px; display: flex; flex-direction: column; gap: 8px;">
            <button id="refresh-map" class="map-button" title="Haritayı Yenile" 
                    style="background: white; border: none; border-radius: 4px; width: 32px; height: 32px; display: flex; align-items: center; justify-content: center; box-shadow: 0 2px 6px rgba(0,0,0,0.15);">
                <i class="fas fa-sync-alt" style="color: #3089d6;"></i>
            </button>
            <button id="center-map" class="map-button" title="Haritayı Merkeze Al" 
                    style="background: white; border: none; border-radius: 4px; width: 32px; height: 32px; display: flex; align-items: center; justify-content: center; box-shadow: 0 2px 6px rgba(0,0,0,0.15);">
                <i class="fas fa-crosshairs" style="color: #3089d6;"></i>
            </button>
        </div>
    `;
    map.controls[google.maps.ControlPosition.RIGHT_TOP].push(customControlDiv);

    // Short delay to ensure the map is fully loaded
    setTimeout(() => {
        infoWindow = new google.maps.InfoWindow({
            maxWidth: 350,
            pixelOffset: new google.maps.Size(0, -5)
        });

        // Map click event handler
        map.addListener('click', (e) => {
            setMarkerPosition(e.latLng.lat(), e.latLng.lng());
            if (dotNetHelper) {
                dotNetHelper.invokeMethodAsync('UpdateCoordinates', e.latLng.lat(), e.latLng.lng());

                // Get address using geocoding
                const geocoder = new google.maps.Geocoder();
                geocoder.geocode({ location: e.latLng }, (results, status) => {
                    if (status === 'OK' && results[0]) {
                        dotNetHelper.invokeMethodAsync('UpdateAddress', results[0].formatted_address);
                    }
                });
            }
        });

        // Add event listeners for custom controls
        document.getElementById('refresh-map').addEventListener('click', function () {
            if (dotNetHelper) {
                dotNetHelper.invokeMethodAsync('RefreshData');
            }
        });

        document.getElementById('center-map').addEventListener('click', function () {
            map.setCenter({ lat: 41.0082, lng: 28.9784 });
            map.setZoom(12);
        });

        mapInitialized = true;
        if (dotNetHelper) dotNetHelper.invokeMethodAsync('OnMapInitialized');
    }, 200);

    return true;
}

// Marker konumunu ayarla
function setMarkerPosition(lat, lng) {
    if (!map) return false;

    const position = new google.maps.LatLng(lat, lng);

    if (!createMarker) {
        createMarker = new google.maps.Marker({
            position: position,
            map: map,
            draggable: true,
            animation: google.maps.Animation.DROP,
            icon: {
                url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(createRecycleBinSVG('Active', 0)),
                scaledSize: new google.maps.Size(50, 50),
                anchor: new google.maps.Point(25, 50)
            },
            zIndex: 1000 // Ensure it's on top of other markers
        });

        // Add drag end event listener to update coordinates
        createMarker.addListener('dragend', () => {
            const newPos = createMarker.getPosition();
            if (dotNetHelper) {
                dotNetHelper.invokeMethodAsync('UpdateCoordinates', newPos.lat(), newPos.lng());

                // Get address using geocoding
                const geocoder = new google.maps.Geocoder();
                geocoder.geocode({ location: newPos }, (results, status) => {
                    if (status === 'OK' && results[0]) {
                        dotNetHelper.invokeMethodAsync('UpdateAddress', results[0].formatted_address);
                    }
                });
            }
        });

        // Add drag start animation
        createMarker.addListener('dragstart', () => {
            // Add a pulsing effect to indicate active dragging
            const icon = createMarker.getIcon();
            icon.url = 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(createRecycleBinSVG('Active', 0, true));
            createMarker.setIcon(icon);
        });

        // Show tooltip on hover
        const markerTooltip = new google.maps.InfoWindow({
            content: '<div style="padding: 5px; font-size: 12px;">Sürükleyerek konumu değiştirebilirsiniz</div>',
            disableAutoPan: true
        });

        createMarker.addListener('mouseover', () => {
            markerTooltip.open(map, createMarker);
        });

        createMarker.addListener('mouseout', () => {
            markerTooltip.close();
        });

    } else {
        // Add smooth animation for position change
        smoothlyAnimateMarker(createMarker, position);
    }

    // Pan map to new position with smooth animation
    map.panTo(position);
    return true;
}

function smoothlyAnimateMarker(marker, newPosition) {
    if (!marker) return;

    const startPosition = marker.getPosition();
    const startLat = startPosition.lat();
    const startLng = startPosition.lng();
    const endLat = newPosition.lat();
    const endLng = newPosition.lng();

    const frames = 20; // Number of animation frames
    let frame = 0;

    const animate = () => {
        if (frame >= frames) {
            marker.setPosition(newPosition);
            return;
        }

        const progress = frame / frames;
        // Use easing function for smoother animation
        const easeProgress = 1 - Math.pow(1 - progress, 3); // Cubic ease out

        const lat = startLat + (endLat - startLat) * easeProgress;
        const lng = startLng + (endLng - startLng) * easeProgress;

        marker.setPosition(new google.maps.LatLng(lat, lng));
        frame++;

        requestAnimationFrame(animate);
    };

    animate();
}

// Form değerleri değiştiğinde marker konumunu güncelle
function updateMarkerPosition(lat, lng) {
    if (!map || !mapInitialized) {
        const initialized = initializeMap();
        if (initialized) {
            setTimeout(() => setMarkerPosition(lat, lng), 500);
        }
        return;
    }

    setMarkerPosition(lat, lng);
}

// Yakındaki atık kutularını göster
function showNearbyBins(binsJson) {
    if (!map || !mapInitialized) {
        const initialized = initializeMap();
        if (initialized) {
            setTimeout(() => showNearbyBins(binsJson), 500);
        }
        return;
    }

    // Önceki işaretçileri temizle
    clearMarkers();

    // Dizeyi JSON olarak ayrıştır
    try {
        const bins = JSON.parse(binsJson);
        if (!bins || bins.length === 0) return;

        // Her kutu için işaretçi ekle
        bins.forEach(bin => addBinMarker(bin, map));
    } catch (error) {
        console.error("Binsler ayrıştırılırken hata:", error);
    }
}

// Function to add a waste bin marker to the map
function addBinMarker(bin, targetMap) {
    if (!targetMap) return null;

    try {
        // Pascal case (Blazor) to camelCase (JS) conversion
        const position = new google.maps.LatLng(
            bin.Latitude || bin.latitude,
            bin.Longitude || bin.longitude
        );

        // Get fill level and device status
        const fillLevel = bin.FillLevel || bin.fillLevel || 0;
        const deviceStatus = bin.DeviceStatus || bin.deviceStatus || 'Active';
        const wasteBinId = bin.WasteBinId || bin.wasteBinId;

        // Create an enhanced SVG icon with animation attributes
        const svgIcon = createRecycleBinSVG(deviceStatus, fillLevel);

        // Create marker with improved animation and appearance
        const marker = new google.maps.Marker({
            position: position,
            map: targetMap,
            icon: {
                url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(svgIcon),
                scaledSize: new google.maps.Size(40, 40),
                anchor: new google.maps.Point(20, 40), // Center bottom alignment
                labelOrigin: new google.maps.Point(20, -10) // Label position above icon
            },
            title: bin.Label || bin.label || 'Atık Kutusu',
            binId: wasteBinId,
            animation: google.maps.Animation.DROP,
            optimized: false // Required for animations in some browsers
        });

        // Add label for critical bins
        if (fillLevel >= 90 || deviceStatus === 'Faulty') {
            marker.setLabel({
                text: fillLevel >= 90 ? '!' : 'X',
                color: 'white',
                fontSize: '12px',
                fontWeight: 'bold',
                className: 'marker-label-alert'
            });
        }

        // Add click event listener
        marker.addListener('click', () => {
            if (infoWindow) infoWindow.close();

            // Create enhanced info window content
            const infoContent = createInfoWindowContent(bin);
            infoWindow.setContent(infoContent);
            infoWindow.open(targetMap, marker);

            // Add bounce animation on click for better focus
            marker.setAnimation(google.maps.Animation.BOUNCE);
            setTimeout(() => marker.setAnimation(null), 750);

            // Add event listener for the "View Details" button
            setTimeout(() => {
                const detailButton = document.getElementById(`view-detail-${wasteBinId}`);
                if (detailButton) {
                    detailButton.addEventListener('click', (e) => {
                        e.preventDefault();

                        // Add button click animation
                        detailButton.style.transform = 'scale(0.95)';
                        setTimeout(() => {
                            detailButton.style.transform = 'scale(1)';
                        }, 100);

                        if (dotNetHelper) {
                            dotNetHelper.invokeMethodAsync('OpenBinDetail', wasteBinId);
                            infoWindow.close();
                        }
                    });

                    // Add hover effect for better interaction feedback
                    detailButton.addEventListener('mouseover', () => {
                        detailButton.style.boxShadow = '0 4px 8px rgba(0,0,0,0.2)';
                    });

                    detailButton.addEventListener('mouseout', () => {
                        detailButton.style.boxShadow = '0 2px 4px rgba(0,0,0,0.1)';
                    });
                }
            }, 100);
        });

        // Add hover effect for markers
        marker.addListener('mouseover', () => {
            marker.setIcon({
                url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(createRecycleBinSVG(deviceStatus, fillLevel, true)),
                scaledSize: new google.maps.Size(48, 48),
                anchor: new google.maps.Point(24, 48),
                labelOrigin: new google.maps.Point(24, -10)
            });
        });

        marker.addListener('mouseout', () => {
            marker.setIcon({
                url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(createRecycleBinSVG(deviceStatus, fillLevel)),
                scaledSize: new google.maps.Size(40, 40),
                anchor: new google.maps.Point(20, 40),
                labelOrigin: new google.maps.Point(20, -10)
            });
        });

        // Add marker to list for tracking
        markers.push(marker);
        return marker;
    } catch (error) {
        console.error("İşaretçi eklenirken hata:", error);
        return null;
    }
}

// Create a recycling bin SVG icon with animation and improved visuals
function createRecycleBinSVG(status, fillLevel, isHovered = false) {
    // Determine main color based on status and fill level
    let mainColor;
    switch (status) {
        case 'Active':
            // Active status - color changes based on fill level
            if (fillLevel >= 90) mainColor = "#dc3545"; // Red
            else if (fillLevel >= 70) mainColor = "#fd7e14"; // Orange
            else if (fillLevel >= 50) mainColor = "#ffc107"; // Yellow
            else if (fillLevel >= 30) mainColor = "#3089d6"; // Blue
            else mainColor = "#28a745"; // Green
            break;
        case 'Inactive': mainColor = "#6c757d"; break; // Gray
        case 'Maintenance': mainColor = "#ffc107"; break; // Yellow
        case 'Faulty': mainColor = "#dc3545"; break; // Red
        default: mainColor = "#6c757d"; // Default gray
    }

    // Calculate fill height based on level (0-100%)
    const fillHeight = Math.min(Math.max(fillLevel, 0), 100) * 0.7 / 100;

    // Shadow for hover effect
    const shadowEffect = isHovered ?
        '<filter id="shadow" x="-20%" y="-20%" width="140%" height="140%">' +
        '  <feDropShadow dx="0" dy="2" stdDeviation="3" flood-color="#000" flood-opacity="0.3" />' +
        '</filter>' : '';

    // Animation for critical bins
    const criticalAnimation = (fillLevel >= 90 || status === 'Faulty') ?
        'class="bin-pulse"' : '';

    // Enhanced recycling bin SVG with animations and better styling
    return `
    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 64 64" width="40" height="40" ${criticalAnimation}>
      <defs>
        ${shadowEffect}
        <linearGradient id="binGradient" x1="0%" y1="0%" x2="100%" y2="100%">
          <stop offset="0%" style="stop-color:${mainColor}"/>
          <stop offset="100%" style="stop-color:${getGradientEndColor(mainColor)}"/>
        </linearGradient>
        <style>
          .bin-pulse {
            animation: pulse 1.5s infinite;
          }
          @keyframes pulse {
            0% { transform: scale(1); opacity: 1; }
            50% { transform: scale(1.05); opacity: 0.9; }
            100% { transform: scale(1); opacity: 1; }
          }
          .maintenance-icon {
            animation: spin 4s linear infinite;
          }
          @keyframes spin {
            0% { transform: rotate(0deg); transform-origin: center; }
            100% { transform: rotate(360deg); transform-origin: center; }
          }
        </style>
      </defs>
      
      <!-- Background circle with gradient -->
      <circle cx="32" cy="32" r="30" fill="#ffffff" stroke="url(#binGradient)" stroke-width="2.5" ${isHovered ? 'filter="url(#shadow)"' : ''} />
      
      <!-- Waste bin body with gradient fill -->
      <rect x="17" y="18" width="30" height="34" rx="3" fill="url(#binGradient)" ${isHovered ? 'filter="url(#shadow)"' : ''} />
      
      <!-- Waste bin lid -->
      <rect x="15" y="16" width="34" height="4" rx="1" fill="url(#binGradient)" ${isHovered ? 'filter="url(#shadow)"' : ''} />
      
      <!-- Handle -->
      <rect x="28" y="12" width="8" height="4" rx="1" fill="url(#binGradient)" ${isHovered ? 'filter="url(#shadow)"' : ''} />
      
      <!-- Recycling symbol - enhanced with animation for maintenance status -->
      <g ${status === 'Maintenance' ? 'class="maintenance-icon"' : ''}>
        <path d="M32,25 L36,32 L28,32 Z M28,30 L24,37 L32,37 Z M36,30 L40,37 L32,37 Z" 
              fill="#ffffff" stroke="#ffffff" stroke-width="0.8" />
      </g>
      
      <!-- Fill level indicator with subtle gradient -->
      <rect x="19" y="${44 - fillHeight * 20}" width="26" height="${fillHeight * 20}" 
            fill="rgba(255,255,255,${isHovered ? 0.4 : 0.3})" rx="1" />
      
      <!-- Status indicators for different states -->
      ${status === 'Faulty' ?
            '<circle cx="50" cy="16" r="6" fill="#dc3545" stroke="#ffffff" stroke-width="1">' +
            '<animate attributeName="opacity" values="1;0.3;1" dur="1s" repeatCount="indefinite" />' +
            '</circle>' +
            '<text x="50" y="19" font-size="8" text-anchor="middle" fill="white" font-weight="bold">!</text>' : ''}
        
      ${status === 'Inactive' ?
            '<circle cx="50" cy="16" r="6" fill="#6c757d" stroke="#ffffff" stroke-width="1"></circle>' +
            '<rect x="47" y="15" width="6" height="2" fill="white" rx="1"></rect>' : ''}
        
      ${fillLevel >= 90 ?
            '<circle cx="50" cy="16" r="6" fill="#dc3545" stroke="#ffffff" stroke-width="1">' +
            '<animate attributeName="opacity" values="1;0.5;1" dur="1s" repeatCount="indefinite" />' +
            '</circle>' +
            '<text x="50" y="19" font-size="8" text-anchor="middle" fill="white" font-weight="bold">!</text>' : ''}
      
      <!-- Bin shadow -->
      <ellipse cx="32" cy="54" rx="18" ry="2" fill="rgba(0,0,0,0.15)" />
    </svg>
    `;
}

// Add custom styles to the page to improve map markers and infowindows
function addCustomStyles() {
    // Create a style element if it doesn't exist
    if (!document.getElementById('map-custom-styles')) {
        const styleEl = document.createElement('style');
        styleEl.id = 'map-custom-styles';
        styleEl.innerHTML = `
            /* Info Window Styling */
            .gm-style .gm-style-iw-c {
                padding: 0 !important;
                border-radius: 8px !important;
                box-shadow: 0 6px 16px rgba(0,0,0,0.15) !important;
                max-width: 350px !important;
                min-width: 320px !important;
            }
            
            .gm-style .gm-style-iw-d {
                overflow: hidden !important;
            }
            
            .gm-style .gm-style-iw-t::after {
                box-shadow: 0 0 10px 1px rgba(0,0,0,0.1) !important;
            }
            
            .gm-ui-hover-effect {
                top: 0 !important;
                right: 0 !important;
                background: #ffffff33 !important;
                border-radius: 50% !important;
                margin: 8px !important;
            }
            
            .gm-ui-hover-effect img {
                width: 20px !important;
                height: 20px !important;
                margin: 4px !important;
                filter: brightness(0) invert(1) !important;
            }
            
            /* Marker Label Styling */
            .marker-label-alert {
                background-color: #dc3545;
                border-radius: 50%;
                padding: 3px;
                font-size: 10px !important;
                font-weight: bold !important;
                min-width: 18px;
                min-height: 18px;
                display: flex;
                align-items: center;
                justify-content: center;
                box-shadow: 0 2px 4px rgba(0,0,0,0.2);
            }
            
            /* Animated Markers */
            @keyframes markerPulse {
                0% { transform: scale(1); opacity: 1; }
                50% { transform: scale(1.1); opacity: 0.9; }
                100% { transform: scale(1); opacity: 1; }
            }
            
            .marker-pulse {
                animation: markerPulse 1.5s infinite;
            }
        `;
        document.head.appendChild(styleEl);
    }
}

// This function creates the content for the popup that appears when clicking on a waste bin marker
function createInfoWindowContent(bin) {
    try {
        // Get values from either Pascal case or camelCase properties
        const label = bin.Label || bin.label || 'Bilinmeyen';
        const address = bin.Address || bin.address || 'Konum bilgisi yok';
        const fillLevel = bin.FillLevel || bin.fillLevel || 0;
        const deviceStatus = bin.DeviceStatus || bin.deviceStatus || 'Active';
        const updatedAt = bin.UpdatedAt || bin.updatedAt || new Date().toISOString();
        const wasteBinId = bin.WasteBinId || bin.wasteBinId;

        // Determine status and fill level colors
        const statusColor = getStatusColor(deviceStatus);
        const fillLevelColor = getFillLevelColor(fillLevel);

        // Create animated fill level indicator class
        const animationClass = fillLevel >= 90 ? 'pulse-animation' : '';

        // Enhanced HTML content for the info window with animations and better styling
        return `
            <div class="info-window-container" style="font-family: 'Segoe UI', Arial, sans-serif; width: 320px; box-shadow: 0 4px 12px rgba(0,0,0,0.1); border-radius: 8px; overflow: hidden; animation: fadeIn 0.3s ease-out;">
                <div style="background: linear-gradient(to right, ${statusColor}, ${getGradientEndColor(statusColor)}); padding: 12px 16px; position: relative;">
                    <div style="position: absolute; top: 0; right: 0; width: 40px; height: 40px; overflow: hidden;">
                        <div style="position: absolute; top: -20px; right: -20px; width: 40px; height: 40px; background: rgba(255,255,255,0.2); transform: rotate(45deg);"></div>
                    </div>
                    <h3 style="font-size: 18px; font-weight: 600; margin: 0; color: white; text-shadow: 0 1px 2px rgba(0,0,0,0.1);">
                        ${label}
                    </h3>
                    <div style="font-size: 12px; margin-top: 4px; color: rgba(255,255,255,0.9); display: flex; align-items: center;">
                        <i class="fas fa-map-marker-alt" style="margin-right: 6px;"></i>${address}
                    </div>
                </div>
                
                <div style="padding: 16px; background: white;">
                    <div style="display: flex; justify-content: space-between; margin-bottom: 16px;">
                        <div style="text-align: center; flex: 1; padding: 8px 4px; border-radius: 6px; background-color: ${getLighterColor(statusColor)}; color: ${getDarkerColor(statusColor)};">
                            <i class="fas ${getStatusIcon(deviceStatus)} ${getStatusClass(deviceStatus)}" style="margin-right: 4px;"></i>
                            ${getStatusText(deviceStatus)}
                        </div>
                        <div style="width: 8px;"></div>
                        <div style="text-align: center; flex: 1; padding: 8px 4px; border-radius: 6px; background-color: ${getLighterColor(fillLevelColor)}; color: ${getDarkerColor(fillLevelColor)};">
                            <i class="fas ${getFillLevelIcon(fillLevel)}" style="margin-right: 4px;"></i>
                            ${getFillLevelText(fillLevel)}
                        </div>
                    </div>
                    
                    <div style="margin-bottom: 16px;">
                        <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 6px;">
                            <div style="font-size: 13px; color: #555; font-weight: 500;">Doluluk Seviyesi</div>
                            <div style="font-size: 13px; font-weight: 600; color: ${fillLevelColor};" class="${animationClass}">${fillLevel}%</div>
                        </div>
                        <div style="height: 12px; background-color: #f0f0f0; border-radius: 6px; overflow: hidden; box-shadow: inset 0 1px 2px rgba(0,0,0,0.1);">
                            <div style="height: 100%; width: ${fillLevel}%; background: linear-gradient(to right, ${fillLevelColor}, ${getGradientEndColor(fillLevelColor)}); border-radius: 6px; transition: width 0.5s ease-out;" class="${animationClass}"></div>
                        </div>
                    </div>
                    
                    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px;">
                        <div style="font-size: 12px; color: #777;">
                            <i class="far fa-clock" style="margin-right: 4px;"></i> 
                            ${formatDate(updatedAt)}
                        </div>
                        <div style="font-size: 12px; color: #777;">
                            <i class="fas fa-history" style="margin-right: 4px;"></i>
                            ${getRelativeTime(updatedAt)}
                        </div>
                    </div>
                    
                    <button id="view-detail-${wasteBinId}" style="width: 100%; background: linear-gradient(to right, #3089d6, #1a6cb7); color: white; border: none; padding: 10px; border-radius: 6px; font-size: 14px; font-weight: 500; cursor: pointer; display: flex; justify-content: center; align-items: center; box-shadow: 0 2px 4px rgba(0,0,0,0.1); transition: all 0.2s ease;">
                        <i class="fas fa-info-circle" style="margin-right: 8px;"></i>
                        Detayları Görüntüle
                        <i class="fas fa-chevron-right" style="margin-left: 8px; font-size: 12px;"></i>
                    </button>
                </div>
            </div>
            
            <style>
                .info-window-container button:hover {
                    transform: translateY(-2px);
                    box-shadow: 0 4px 8px rgba(0,0,0,0.15);
                }
                
                .info-window-container button:active {
                    transform: translateY(0);
                    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                }
                
                @keyframes fadeIn {
                    from {
                        opacity: 0;
                        transform: translateY(10px);
                    }
                    to {
                        opacity: 1;
                        transform: translateY(0);
                    }
                }
                
                @keyframes bounce {
                    0%, 100% { transform: translateY(0); }
                    50% { transform: translateY(-5px); }
                }
                
                .bounce-animation {
                    animation: bounce 2s infinite;
                }
                
                @keyframes pulse {
                    0% { opacity: 1; }
                    50% { opacity: 0.7; }
                    100% { opacity: 1; }
                }
                
                .pulse-animation {
                    animation: pulse 1.5s infinite;
                }
                
                .blink-animation {
                    animation: pulse 0.5s infinite;
                }
                
                .spin-animation {
                    animation: spin 2s linear infinite;
                }
                
                @keyframes spin {
                    0% { transform: rotate(0deg); }
                    100% { transform: rotate(360deg); }
                }
            </style>
        `;
    } catch (error) {
        console.error("Bilgi penceresi oluşturulurken hata:", error);
        return "<div>Bilgi yüklenirken hata oluştu</div>";
    }
}

// Helper function to get a lighter version of a color for backgrounds
function getLighterColor(color) {
    // Convert hex color to lighter version for background
    if (color.startsWith('#')) {
        return color.replace('#', 'rgba(') + ', 0.15)';
    }
    return color;
}

// Helper function to get a darker version of a color for text
function getDarkerColor(color) {
    // Return a darker version of the same color for text
    if (color.startsWith('#')) {
        // Simple approach to darken color - you could implement a more sophisticated conversion
        return color;
    }
    return color;
}

// Helper function to create gradient end colors
function getGradientEndColor(startColor) {
    // Create slightly darker end color for gradients
    if (startColor === '#28a745') return '#1e8535'; // Green
    if (startColor === '#dc3545') return '#c01d2e'; // Red
    if (startColor === '#ffc107') return '#e0a800'; // Yellow
    if (startColor === '#6c757d') return '#565e64'; // Gray
    if (startColor === '#3089d6') return '#1a6cb7'; // Blue
    return startColor;
}

// Get icon based on device status
function getStatusIcon(status) {
    switch (status) {
        case 'Active': return 'fa-check-circle';
        case 'Inactive': return 'fa-power-off';
        case 'Maintenance': return 'fa-wrench';
        case 'Faulty': return 'fa-exclamation-triangle';
        default: return 'fa-question-circle';
    }
}

// Get animation class based on device status
function getStatusClass(status) {
    switch (status) {
        case 'Active': return '';
        case 'Inactive': return '';
        case 'Maintenance': return 'spin-animation';
        case 'Faulty': return 'blink-animation';
        default: return '';
    }
}

// Get icon based on fill level
function getFillLevelIcon(fillLevel) {
    if (fillLevel >= 90) return 'fa-exclamation-circle';
    if (fillLevel >= 70) return 'fa-exclamation';
    if (fillLevel >= 50) return 'fa-arrows-alt-v';
    if (fillLevel >= 30) return 'fa-level-down-alt';
    return 'fa-check';
}

// Get relative time string
function getRelativeTime(dateString) {
    try {
        const date = new Date(dateString);
        if (isNaN(date.getTime())) return '';

        const now = new Date();
        const diffMs = now - date;
        const diffSecs = Math.floor(diffMs / 1000);
        const diffMins = Math.floor(diffSecs / 60);
        const diffHours = Math.floor(diffMins / 60);
        const diffDays = Math.floor(diffHours / 24);

        if (diffDays > 0) {
            return diffDays === 1 ? 'Dün' : `${diffDays} gün önce`;
        }
        if (diffHours > 0) {
            return `${diffHours} saat önce`;
        }
        if (diffMins > 0) {
            return `${diffMins} dakika önce`;
        }
        return 'Az önce';
    } catch (error) {
        return '';
    }
}

// Konum haritası yükleme
// Load location map with enhanced features
function loadLocationMap(containerId, lat, lng) {
    try {
        const container = document.getElementById(containerId);
        if (!container) return null;

        // Track loading attempts
        mapTries[containerId] = (mapTries[containerId] || 0) + 1;

        // Check if the element is visible
        if (container.offsetWidth === 0 || container.offsetHeight === 0) {
            if (mapTries[containerId] < 3) {
                setTimeout(() => loadLocationMap(containerId, lat, lng), 500);
                return null;
            }
        }

        // Clear any previous map
        if (locationMap) {
            locationMap = null;
        }

        // Create position
        const position = new google.maps.LatLng(lat, lng);

        // Apply theme based on dark/light mode
        const mapStyles = getMapTheme();

        // Create map with enhanced styling
        locationMap = new google.maps.Map(container, {
            center: position,
            zoom: 16,
            mapTypeControl: true,
            streetViewControl: true,
            fullscreenControl: true,
            styles: mapStyles,
            mapTypeControlOptions: {
                style: google.maps.MapTypeControlStyle.DROPDOWN_MENU,
                position: google.maps.ControlPosition.TOP_RIGHT
            },
            zoomControlOptions: {
                position: google.maps.ControlPosition.RIGHT_BOTTOM
            },
            streetViewControlOptions: {
                position: google.maps.ControlPosition.RIGHT_BOTTOM
            }
        });

        // Add custom controls for better UX
        const actionControls = document.createElement('div');
        actionControls.className = 'location-controls';
        actionControls.innerHTML = `
            <div class="control-container" style="margin: 10px; display: flex; gap: 10px;">
                <button id="location-nearby" class="map-button" title="Yakındaki Atık Kutuları" 
                        style="background: white; border: none; border-radius: 4px; padding: 8px 12px; display: flex; align-items: center; 
                        box-shadow: 0 2px 6px rgba(0,0,0,0.15); font-size: 13px; color: #3089d6;">
                    <i class="fas fa-search-location" style="margin-right: 5px;"></i> Yakındaki Kutular
                </button>
                <button id="location-directions" class="map-button" title="Yol Tarifi Al" 
                        style="background: white; border: none; border-radius: 4px; padding: 8px 12px; display: flex; align-items: center; 
                        box-shadow: 0 2px 6px rgba(0,0,0,0.15); font-size: 13px; color: #3089d6;">
                    <i class="fas fa-directions" style="margin-right: 5px;"></i> Yol Tarifi
                </button>
            </div>
        `;
        locationMap.controls[google.maps.ControlPosition.TOP_LEFT].push(actionControls);

        // Add marker and other elements with a slight delay
        setTimeout(() => {
            // Create main marker with animation
            const mainMarker = new google.maps.Marker({
                position: position,
                map: locationMap,
                animation: google.maps.Animation.DROP,
                icon: {
                    url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(createRecycleBinSVG('Active', 50, true)),
                    scaledSize: new google.maps.Size(50, 50),
                    anchor: new google.maps.Point(25, 50)
                },
                zIndex: 1000
            });

            // Bounce animation on load
            mainMarker.setAnimation(google.maps.Animation.BOUNCE);
            setTimeout(() => mainMarker.setAnimation(null), 1500);

            // Add circle around the marker with animation effect
            const circle = new google.maps.Circle({
                map: locationMap,
                center: position,
                radius: 30,
                fillColor: "#3089d6",
                fillOpacity: 0.15,
                strokeColor: "#3089d6",
                strokeWeight: 1,
                strokeOpacity: 0.8
            });

            // Animate the circle radius on load
            let radius = 10;
            const maxRadius = 30;
            const circleAnimation = setInterval(() => {
                radius += 2;
                circle.setRadius(radius);
                if (radius >= maxRadius) clearInterval(circleAnimation);
            }, 50);

            // Add info window with bin details
            const locationInfo = new google.maps.InfoWindow({
                content: `
                    <div style="padding: 12px; max-width: 300px; font-family: 'Segoe UI', Arial, sans-serif;">
                        <div style="font-weight: 600; font-size: 16px; margin-bottom: 8px; color: #3089d6; border-bottom: 2px solid #3089d6; padding-bottom: 5px;">
                            ${selectedBin?.Label || 'Atık Kutusu'}
                        </div>
                        <div style="font-size: 13px; color: #555; margin-bottom: 10px;">
                            <i class="fas fa-map-marker-alt" style="color: #dc3545; margin-right: 5px;"></i>
                            ${selectedBin?.Address || 'Konum bilgisi'}
                        </div>
                        <div style="display: flex; justify-content: space-between; margin-bottom: 10px;">
                            <div style="font-size: 13px; color: #555;">
                                <i class="fas fa-percentage" style="margin-right: 5px;"></i>
                                Doluluk: <span style="font-weight: 600; color: ${getFillLevelColor(selectedBin?.FillLevel || 0)}">${selectedBin?.FillLevel || 0}%</span>
                            </div>
                            <div style="font-size: 13px; color: #555;">
                                <i class="fas fa-sync-alt" style="margin-right: 5px;"></i>
                                Son Güncelleme: ${formatDate(selectedBin?.UpdatedAt || new Date())}
                            </div>
                        </div>
                    </div>
                `,
                pixelOffset: new google.maps.Size(0, -5)
            });

            // Show info window on marker load
            locationInfo.open(locationMap, mainMarker);

            // Add event listeners for custom controls
            setTimeout(() => {
                const nearbyButton = document.getElementById('location-nearby');
                if (nearbyButton) {
                    nearbyButton.addEventListener('click', () => {
                        // Show loading indicator
                        nearbyButton.innerHTML = '<i class="fas fa-spinner fa-spin" style="margin-right: 5px;"></i> Yükleniyor...';

                        // Request nearby bins from Blazor component
                        if (dotNetHelper) {
                            dotNetHelper.invokeMethodAsync('ShowNearbyBins', lat, lng)
                                .then(() => {
                                    // Restore button text
                                    setTimeout(() => {
                                        nearbyButton.innerHTML = '<i class="fas fa-search-location" style="margin-right: 5px;"></i> Yakındaki Kutular';
                                    }, 1000);
                                });
                        }
                    });
                }

                const directionsButton = document.getElementById('location-directions');
                if (directionsButton) {
                    directionsButton.addEventListener('click', () => {
                        // Open Google Maps directions in a new tab
                        const googleMapsUrl = `https://www.google.com/maps/dir/?api=1&destination=${lat},${lng}`;
                        window.open(googleMapsUrl, '_blank');
                    });
                }
            }, 100);

            if (dotNetHelper) dotNetHelper.invokeMethodAsync('OnMapLoaded', containerId);
        }, 300);

        return locationMap;
    } catch (error) {
        console.error("Konum haritası yüklenirken hata:", error);
        return null;
    }
}

// Yakındaki kutuları konum haritasında göster
function showNearbyBinsOnLocationMap(binsJson) {
    if (!locationMap) {
        setTimeout(() => showNearbyBinsOnLocationMap(binsJson), 1000);
        return;
    }

    try {
        const bins = JSON.parse(binsJson);
        if (!bins || bins.length === 0) return;

        bins.forEach(bin => addBinMarker(bin, locationMap));
    } catch (error) {
        console.error("Konum haritasında kutular gösterilirken hata:", error);
    }
}

// Detay haritası yükleme
// Load detail map with improved visuals
function loadDetailMap(mapId, lat, lng) {
    try {
        const container = document.getElementById(mapId);
        if (!container) return null;

        // Track loading attempts
        mapTries[mapId] = (mapTries[mapId] || 0) + 1;

        // Check if the element is visible
        if (container.offsetWidth === 0 || container.offsetHeight === 0) {
            if (mapTries[mapId] < 3) {
                setTimeout(() => loadDetailMap(mapId, lat, lng), 500);
                return null;
            }
        }

        // Clear any previous map
        if (detailMaps[mapId]) {
            detailMaps[mapId] = null;
        }

        // Create position
        const position = new google.maps.LatLng(lat, lng);

        // Apply theme based on dark/light mode
        const mapStyles = getMapTheme();

        // Create a more interactive detail map
        const detailMap = new google.maps.Map(container, {
            center: position,
            zoom: 15,
            mapTypeControl: false,
            streetViewControl: true,
            zoomControl: true,
            fullscreenControl: true,
            draggable: true,
            styles: mapStyles,
            zoomControlOptions: {
                position: google.maps.ControlPosition.RIGHT_BOTTOM,
                style: google.maps.ZoomControlStyle.SMALL
            },
            streetViewControlOptions: {
                position: google.maps.ControlPosition.RIGHT_BOTTOM
            }
        });

        detailMaps[mapId] = detailMap;

        // Add marker and visual elements with a slight delay
        setTimeout(() => {
            // Main marker with advanced styling
            const marker = new google.maps.Marker({
                position: position,
                map: detailMap,
                animation: google.maps.Animation.DROP,
                icon: {
                    url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(createRecycleBinSVG('Active', 50)),
                    scaledSize: new google.maps.Size(40, 40),
                    anchor: new google.maps.Point(20, 40)
                }
            });

            // Add small radius indicator
            const circle = new google.maps.Circle({
                map: detailMap,
                center: position,
                radius: 20,
                fillColor: "#3089d6",
                fillOpacity: 0.1,
                strokeColor: "#3089d6",
                strokeWeight: 1,
                strokeOpacity: 0.6
            });

            // Add "expand view" button for better UX
            const expandButton = document.createElement('div');
            expandButton.className = 'expand-map-button';
            expandButton.innerHTML = `
                <button title="Tam Ekran Görüntüle" 
                        style="background: white; border: none; border-radius: 4px; width: 28px; height: 28px; 
                               display: flex; align-items: center; justify-content: center; box-shadow: 0 2px 4px rgba(0,0,0,0.15);">
                    <i class="fas fa-expand-alt" style="color: #3089d6; font-size: 12px;"></i>
                </button>
            `;

            expandButton.addEventListener('click', () => {
                // Request to show location in full modal
                if (dotNetHelper) {
                    // Extract bin ID from map ID (detail-map-{binId})
                    const binId = mapId.replace('detail-map-', '');
                    dotNetHelper.invokeMethodAsync('OpenLocationModalFromMap', binId);
                }
            });

            detailMap.controls[google.maps.ControlPosition.TOP_RIGHT].push(expandButton);

            if (dotNetHelper) dotNetHelper.invokeMethodAsync('OnMapLoaded', mapId);
        }, 300);

        return detailMap;
    } catch (error) {
        console.error("Detay haritası yüklenirken hata:", error);
        return null;
    }
}

// Belirli bir atık kutusuna odaklan
function focusOnBin(binId) {
    try {
        const marker = markers.find(m => m.binId === binId);
        if (marker && map) {
            map.setCenter(marker.getPosition());
            map.setZoom(17);
            google.maps.event.trigger(marker, 'click');
        }
    } catch (error) {
        console.error("Atık kutusuna odaklanırken hata:", error);
    }
}

// Tüm atık kutularını haritada göster
function showAllBins(binsJson) {
    if (!map || !mapInitialized) {
        const initialized = initializeMap();
        if (initialized) {
            setTimeout(() => showAllBins(binsJson), 500);
        }
        return;
    }

    try {
        const bins = JSON.parse(binsJson);
        if (!bins || bins.length === 0) return;

        // Önceki işaretçileri temizle
        clearMarkers();

        // Her kutu için işaretçi ekle
        bins.forEach(bin => addBinMarker(bin, map));

        // Tüm işaretçileri gösterecek şekilde haritayı ayarla
        if (bins.length > 0) {
            const bounds = new google.maps.LatLngBounds();
            markers.forEach(marker => bounds.extend(marker.getPosition()));

            map.fitBounds(bounds);

            // Tek işaretçi varsa
            if (bins.length === 1) {
                setTimeout(() => map.setZoom(14), 300);
            }
        }
    } catch (error) {
        console.error("Tüm kutular gösterilirken hata:", error);
    }
}

// Mevcut işaretçileri temizle
function clearMarkers() {
    markers.forEach(marker => {
        if (marker) marker.setMap(null);
    });
    markers = [];
}

// Harita durumunu sıfırla
function resetMapState() {
    try {
        // Ana işaretçiyi temizle
        if (createMarker) {
            createMarker.setMap(null);
            createMarker = null;
        }

        // Tüm işaretçileri temizle
        clearMarkers();

        // Bilgi penceresini kapat
        if (infoWindow) {
            infoWindow.close();
        }

        // Detay haritalarını temizle
        for (const mapId in detailMaps) {
            if (detailMaps[mapId]) {
                detailMaps[mapId] = null;
            }
        }

        // Konum haritasını temizle
        if (locationMap) {
            locationMap = null;
        }

        mapInitialized = false;
        mapTries = {};
    } catch (error) {
        console.error("Harita durumu sıfırlanırken hata:", error);
    }
}

// Durum rengi al
function getStatusColor(status) {
    switch (status) {
        case 'Active': return '#28a745'; // Yeşil
        case 'Inactive': return '#6c757d'; // Gri
        case 'Maintenance': return '#ffc107'; // Sarı
        case 'Faulty': return '#dc3545'; // Kırmızı
        default: return '#6c757d'; // Varsayılan olarak gri
    }
}

// Durum metni al
function getStatusText(status) {
    switch (status) {
        case 'Active': return 'Aktif';
        case 'Inactive': return 'Pasif';
        case 'Maintenance': return 'Bakımda';
        case 'Faulty': return 'Arızalı';
        default: return status;
    }
}

// Doluluk seviyesi rengi al
function getFillLevelColor(fillLevel) {
    if (fillLevel >= 90) return '#dc3545'; // Kırmızı
    if (fillLevel >= 70) return '#fd7e14'; // Turuncu
    if (fillLevel >= 50) return '#ffc107'; // Sarı
    if (fillLevel >= 30) return '#3089d6'; // Mavi
    return '#28a745'; // Yeşil
}

// Doluluk seviyesi metni al
function getFillLevelText(fillLevel) {
    if (fillLevel >= 90) return 'Acil Boşaltılmalı';
    if (fillLevel >= 70) return 'Yakında Dolu';
    if (fillLevel >= 50) return 'Orta Doluluk';
    if (fillLevel >= 30) return 'Az Dolu';
    return 'Boş';
}

// Tarih biçimlendirme
function formatDate(dateString) {
    try {
        const date = new Date(dateString);
        if (isNaN(date.getTime())) return dateString || 'Bilinmiyor';

        return date.toLocaleDateString('tr-TR', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    } catch (error) {
        return dateString || 'Bilinmiyor';
    }
}