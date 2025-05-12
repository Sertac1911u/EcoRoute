///**
// * EcoRoute Google Maps Integration - Complete Fixed Implementation
// * 
// * This file contains the complete implementation with all required changes and bug fixes.
// * It addresses both the requirements and the errors found in the console logs.
// */

//// Global variables
//let mainMap;
//let locationPickerMap;
//let markers = [];
//let currentInfoWindow = null;
//let sidebarOpen = false;
//let dotNetRef = null;
//let selectedBinId = null;
//let userLocationMarker = null;

//// Constants
//const DEFAULT_CENTER = { lat: 41.0082, lng: 28.9784 }; // Istanbul
//const DEFAULT_ZOOM = 13; // Initial zoom level for overview
//const FOCUS_ZOOM = 18; // Zoom level when focusing on a bin (req #3)
//const DEFAULT_TILT = 67.7; // Default tilt for 3D view (req #3)
//const MAP_ID = '8b70db4a26fb9f4cd11929e3'; // Your custom map style ID

///**
// * Initialize Google Maps integration
// * @param {DotNetReference} reference - .NET reference for calling Blazor methods
// */
//window.initializeGoogleMaps = function (reference) {
//    dotNetRef = reference;
//    console.log("Google Maps initialization started");
//};

///**
// * Main object containing all Google Maps related functions
// */
//window.googleMapsInterop = {
//    /**
//     * Initialize the main 3D map
//     * @param {string} mapElementId - HTML element ID for the map container
//     */
//    initializeMainMap: function (mapElementId) {
//        console.log("Initializing main map in element:", mapElementId);

//        // Hide loading indicator when map is fully loaded
//        const hideLoading = () => {
//            const loadingIndicator = document.getElementById('map-loading-indicator');
//            if (loadingIndicator) {
//                loadingIndicator.style.opacity = '0';
//                setTimeout(() => {
//                    loadingIndicator.style.display = 'none';
//                }, 500);
//            }
//        };

//        try {
//            mainMap = new google.maps.Map(document.getElementById(mapElementId), {
//                center: DEFAULT_CENTER,
//                zoom: DEFAULT_ZOOM,
//                tilt: DEFAULT_TILT,
//                heading: 0,
//                mapTypeId: 'roadmap',
//                mapId: MAP_ID,
//                // Requirement #1: Removed all camera controls
//                streetViewControl: false,
//                mapTypeControl: false,
//                rotateControl: false,
//                fullscreenControl: false,
//                zoomControl: false
//                // Removed styles array since using mapId (console warning)
//            });

//            // Add 2D/3D toggle button
//            this.add2D3DToggle(mainMap);

//            // Add user location button
//            this.addUserLocationButton(mainMap);

//            // Add map loaded event
//            google.maps.event.addListenerOnce(mainMap, 'tilesloaded', () => {
//                hideLoading();
//                if (dotNetRef) {
//                    dotNetRef.invokeMethodAsync('OnMapInitialized');
//                }
//            });

//        } catch (error) {
//            console.error("Error initializing main map:", error);
//            hideLoading();
//        }
//    },

//    /**
//     * Add 2D/3D toggle button to map
//     * @param {google.maps.Map} map - Map object
//     */
//    add2D3DToggle: function (map) {
//        const toggleButton = document.createElement('button');
//        toggleButton.className = 'custom-map-control-button toggle-view-button';
//        toggleButton.innerHTML = '<i class="fas fa-cube"></i> 3D';
//        toggleButton.title = 'Toggle 2D/3D view';

//        let is3DMode = true; // Start in 3D mode

//        toggleButton.addEventListener('click', () => {
//            if (is3DMode) {
//                // Switch to 2D
//                map.setTilt(0);
//                toggleButton.innerHTML = '<i class="fas fa-map"></i> 2D';
//            } else {
//                // Switch to 3D
//                map.setTilt(DEFAULT_TILT);
//                toggleButton.innerHTML = '<i class="fas fa-cube"></i> 3D';
//            }
//            is3DMode = !is3DMode;
//        });

//        map.controls[google.maps.ControlPosition.TOP_RIGHT].push(toggleButton);

//        // Add styles for the toggle button
//        const style = document.createElement('style');
//        style.textContent = `
//            .toggle-view-button {
//                background-color: #fff;
//                border: 2px solid #fff;
//                border-radius: 3px;
//                box-shadow: 0 2px 6px rgba(0,0,0,.3);
//                color: #555;
//                cursor: pointer;
//                font-family: 'Inter', -apple-system, sans-serif;
//                font-size: 14px;
//                margin: 10px;
//                padding: 8px 16px;
//                text-align: center;
//                transition: background-color 0.3s, color 0.3s;
//            }
//            .toggle-view-button:hover {
//                background-color: #f1f1f1;
//            }
//            .toggle-view-button i {
//                margin-right: 5px;
//            }
//            @media (prefers-color-scheme: dark) {
//                .toggle-view-button {
//                    background-color: #1F2937;
//                    border-color: #374151;
//                    color: #E5E7EB;
//                }
//                .toggle-view-button:hover {
//                    background-color: #374151;
//                }
//            }
//        `;
//        document.head.appendChild(style);
//    },

//    /**
//     * Add user location button to map
//     * @param {google.maps.Map} map - Map object
//     */
//    addUserLocationButton: function (map) {
//        const locationButton = document.createElement('button');
//        locationButton.className = 'custom-map-control-button location-button';
//        locationButton.innerHTML = '<i class="fas fa-location-arrow"></i>';
//        locationButton.title = 'Show my location';

//        locationButton.addEventListener('click', () => {
//            if (navigator.geolocation) {
//                navigator.geolocation.getCurrentPosition(
//                    (position) => {
//                        const pos = {
//                            lat: position.coords.latitude,
//                            lng: position.coords.longitude,
//                        };

//                        // Remove existing user location marker if any
//                        if (userLocationMarker) {
//                            userLocationMarker.setMap(null);
//                        }

//                        // Create new marker at user location
//                        userLocationMarker = new google.maps.Marker({
//                            position: pos,
//                            map: map,
//                            icon: {
//                                path: google.maps.SymbolPath.CIRCLE,
//                                fillColor: '#4285F4',
//                                fillOpacity: 1,
//                                strokeColor: '#FFFFFF',
//                                strokeWeight: 2,
//                                scale: 8,
//                            },
//                            title: 'Your Location',
//                        });

//                        // Add accuracy circle
//                        const accuracyCircle = new google.maps.Circle({
//                            center: pos,
//                            radius: position.coords.accuracy,
//                            map: map,
//                            fillColor: '#4285F4',
//                            fillOpacity: 0.15,
//                            strokeColor: '#4285F4',
//                            strokeOpacity: 0.3,
//                            strokeWeight: 1,
//                        });

//                        // Pan to location with smooth animation
//                        this.smoothPanTo(map, pos);

//                        // Don't change zoom or tilt to preserve user's view settings
//                    },
//                    () => {
//                        alert('Error: The Geolocation service failed.');
//                    }
//                );
//            } else {
//                alert('Error: Your browser doesn\'t support geolocation.');
//            }
//        });

//        map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(locationButton);

//        // Add styles for the location button
//        const style = document.createElement('style');
//        style.textContent = `
//            .location-button {
//                background-color: #fff;
//                border: 2px solid #fff;
//                border-radius: 50%;
//                box-shadow: 0 2px 6px rgba(0,0,0,.3);
//                color: #666;
//                cursor: pointer;
//                height: 40px;
//                width: 40px;
//                margin: 10px;
//                padding: 0;
//                text-align: center;
//                transition: background-color 0.3s;
//                display: flex;
//                align-items: center;
//                justify-content: center;
//            }
//            .location-button:hover {
//                background-color: #f1f1f1;
//            }
//            .location-button i {
//                font-size: 16px;
//            }
//            @media (prefers-color-scheme: dark) {
//                .location-button {
//                    background-color: #1F2937;
//                    border-color: #374151;
//                    color: #E5E7EB;
//                }
//                .location-button:hover {
//                    background-color: #374151;
//                }
//            }
//        `;
//        document.head.appendChild(style);
//    },

//    /**
//     * Initialize the location picker map for bin creation/editing
//     * @param {string} mapElementId - HTML element ID for the map container
//     * @param {number} lat - Initial latitude 
//     * @param {number} lng - Initial longitude
//     */
//    initializeLocationPickerMap: function (mapElementId, lat, lng) {
//        console.log("Initializing location picker map");

//        const initialPosition = { lat: lat, lng: lng };

//        try {
//            locationPickerMap = new google.maps.Map(document.getElementById(mapElementId), {
//                center: initialPosition,
//                zoom: 15,
//                mapTypeId: 'roadmap',
//                streetViewControl: false,
//                mapTypeControl: false,
//                fullscreenControl: false
//            });

//            // Place marker at initial position
//            const marker = new google.maps.Marker({
//                position: initialPosition,
//                map: locationPickerMap,
//                draggable: true,
//                icon: this.getBinMarkerIcon('Active', 0),
//                animation: google.maps.Animation.DROP
//            });

//            // Update coordinates when marker is dragged
//            google.maps.event.addListener(marker, 'dragend', function () {
//                const position = marker.getPosition();
//                if (dotNetRef) {
//                    dotNetRef.invokeMethodAsync('UpdateCoordinates', position.lat(), position.lng());

//                    // Get address from coordinates (reverse geocoding)
//                    const geocoder = new google.maps.Geocoder();
//                    geocoder.geocode({ location: position }, function (results, status) {
//                        if (status === 'OK' && results[0]) {
//                            dotNetRef.invokeMethodAsync('UpdateAddress', results[0].formatted_address);
//                        }
//                    });
//                }

//                // Update displayed coordinates
//                const coordsDisplay = document.getElementById('selected-coordinates');
//                if (coordsDisplay) {
//                    coordsDisplay.textContent = `Koordinat: ${position.lat().toFixed(6)}, ${position.lng().toFixed(6)}`;
//                }
//            });

//            // Allow clicking on map to move marker
//            google.maps.event.addListener(locationPickerMap, 'click', function (event) {
//                marker.setPosition(event.latLng);

//                // Trigger dragend to update coordinates
//                google.maps.event.trigger(marker, 'dragend');
//            });

//        } catch (error) {
//            console.error("Error initializing location picker map:", error);
//        }
//    },

//    /**
//     * Show all waste bins on the main map
//     * @param {string} binsJson - JSON string of waste bin data
//     */
//    showAllBins: function (binsJson) {
//        if (!mainMap) {
//            console.error("Main map not initialized");
//            return;
//        }

//        // Clear existing markers
//        this.clearMarkers();

//        try {
//            const bins = JSON.parse(binsJson);
//            console.log(`Showing ${bins.length} bins on map`);

//            // Create bounds object to fit all markers
//            const bounds = new google.maps.LatLngBounds();

//            bins.forEach(bin => {
//                if (bin.Latitude && bin.Longitude) {
//                    const position = { lat: bin.Latitude, lng: bin.Longitude };

//                    // Create marker
//                    const marker = new google.maps.Marker({
//                        position: position,
//                        map: mainMap,
//                        title: bin.Label,
//                        icon: this.getBinMarkerIcon(bin.DeviceStatus, bin.FillLevel),
//                        binId: bin.WasteBinId
//                    });

//                    // Add marker to markers array
//                    markers.push(marker);

//                    // Add marker to bounds
//                    bounds.extend(position);

//                    // Add click listener to marker - Requirement #2: Smooth zoom to bin location
//                    marker.addListener('click', () => {
//                        // Close currently open info window if any
//                        if (currentInfoWindow) {
//                            currentInfoWindow.close();
//                            currentInfoWindow = null;
//                        }

//                        // Requirement #3: Set zoom level to FOCUS_ZOOM (18)
//                        mainMap.setZoom(FOCUS_ZOOM);

//                        // Ensure 3D mode with correct tilt (req #3)
//                        mainMap.setTilt(DEFAULT_TILT);

//                        // Requirement #2: Smooth pan to marker position 
//                        this.smoothPanTo(mainMap, position);

//                        // Open sidebar with detailed information
//                        this.openBinSidebar(bin);

//                        // Store selected bin ID
//                        selectedBinId = bin.WasteBinId;

//                        // Notify Blazor of bin selection
//                        if (dotNetRef) {
//                            dotNetRef.invokeMethodAsync('OpenBinDetail', bin.WasteBinId);
//                        }
//                    });
//                }
//            });

//            // Adjust map to fit all markers if there are any
//            if (markers.length > 0) {
//                mainMap.fitBounds(bounds);

//                // If only one marker, zoom out a bit
//                if (markers.length === 1) {
//                    google.maps.event.addListenerOnce(mainMap, 'bounds_changed', () => {
//                        mainMap.setZoom(Math.min(15, mainMap.getZoom()));
//                    });
//                }
//            }

//        } catch (error) {
//            console.error("Error showing bins on map:", error);
//        }
//    },

//    /**
//     * Smoothly pan to a location - Enhanced for requirements #2, #4, and #5
//     * With added validation to fix NaN coordinates error
//     * @param {google.maps.Map} map - Map object
//     * @param {google.maps.LatLng} position - Position to pan to
//     */
//    smoothPanTo: function (map, position) {
//        // Validate position coordinates
//        if (!position || !isFinite(position.lat) || !isFinite(position.lng)) {
//            console.error("Invalid coordinates for smoothPanTo:", position);
//            return; // Exit early if coordinates are invalid
//        }

//        const currentCenter = map.getCenter();
//        const currentLat = currentCenter.lat();
//        const currentLng = currentCenter.lng();
//        const targetLat = position.lat;
//        const targetLng = position.lng;

//        // Validate calculated coordinates
//        if (!isFinite(currentLat) || !isFinite(currentLng) ||
//            !isFinite(targetLat) || !isFinite(targetLng)) {
//            console.error("Invalid coordinate calculation in smoothPanTo");
//            return; // Exit if any coordinates are invalid
//        }

//        // Animation parameters - improved for smoother motion
//        const frames = 40; // Increased number of frames for smoother animation
//        const duration = 850; // Slightly longer duration for more natural motion
//        let frame = 0;

//        const animate = () => {
//            if (frame >= frames) {
//                // End of animation
//                try {
//                    map.panTo(position);
//                } catch (e) {
//                    console.error("Error in final panTo:", e);
//                }
//                return;
//            }

//            // Calculate progress (ease-out cubic function)
//            // This provides a more natural deceleration at the end
//            const progress = 1 - Math.pow(1 - frame / frames, 3);

//            // Calculate intermediate position
//            const lat = currentLat + (targetLat - currentLat) * progress;
//            const lng = currentLng + (targetLng - currentLng) * progress;

//            // Validate the calculated position
//            if (!isFinite(lat) || !isFinite(lng)) {
//                console.error("Animation calculated invalid coordinates:", { lat, lng });
//                return; // Stop animation if invalid coordinates
//            }

//            // Pan to intermediate position
//            try {
//                map.panTo({ lat, lng });
//            } catch (e) {
//                console.error("Error during animation panTo:", e);
//                return; // Stop animation on error
//            }

//            // Next frame
//            frame++;
//            setTimeout(animate, duration / frames);
//        };

//        // Start animation
//        animate();
//    },

//    /**
//     * Show waste bins on the location picker map (for reference)
//     * @param {string} binsJson - JSON string of waste bin data
//     */
//    showBinsOnPickerMap: function (binsJson) {
//        if (!locationPickerMap) {
//            console.error("Location picker map not initialized");
//            return;
//        }

//        try {
//            const bins = JSON.parse(binsJson);

//            bins.forEach(bin => {
//                if (bin.Latitude && bin.Longitude) {
//                    const position = { lat: bin.Latitude, lng: bin.Longitude };

//                    // Create marker with non-interactive icon
//                    const marker = new google.maps.Marker({
//                        position: position,
//                        map: locationPickerMap,
//                        title: bin.Label,
//                        icon: this.getBinMarkerIcon(bin.DeviceStatus, bin.FillLevel, 0.7), // 70% opacity
//                        clickable: false // Make non-interactive
//                    });
//                }
//            });

//        } catch (error) {
//            console.error("Error showing bins on picker map:", error);
//        }
//    },

//    /**
//     * Focus on a specific bin in the main map - Table "Haritada Göster" button handler
//     * Enhanced for requirement #4 with added validation
//     * @param {string} binId - ID of the bin to focus on
//     */
//    focusOnBin: function (binId) {
//        if (!binId) {
//            console.error("Invalid binId provided to focusOnBin");
//            return;
//        }

//        const marker = markers.find(m => m.binId === binId);

//        if (!marker) {
//            console.error("No marker found with binId:", binId);
//            return;
//        }

//        try {
//            // Get position from marker
//            const position = marker.getPosition();

//            // Validate position
//            if (!position || !isFinite(position.lat()) || !isFinite(position.lng())) {
//                console.error("Invalid marker position:", position);
//                return;
//            }

//            // Requirement #3: Set zoom level to FOCUS_ZOOM (18)
//            mainMap.setZoom(FOCUS_ZOOM);

//            // Requirement #3: Ensure map stays in 3D mode with the correct tilt
//            mainMap.setTilt(DEFAULT_TILT);

//            // Requirement #4: Enhanced smooth pan animation
//            this.smoothPanTo(mainMap, {
//                lat: position.lat(),
//                lng: position.lng()
//            });

//            // Add temporary bounce animation
//            marker.setAnimation(google.maps.Animation.BOUNCE);
//            setTimeout(() => {
//                marker.setAnimation(null);
//            }, 1500);

//            // Trigger click event to show info
//            google.maps.event.trigger(marker, 'click');
//        } catch (e) {
//            console.error("Error in focusOnBin:", e);
//        }
//    },

//    /**
//     * Reset the map state (close info windows, etc.)
//     */
//    resetMapState: function () {
//        // Close any open info windows
//        if (currentInfoWindow) {
//            currentInfoWindow.close();
//            currentInfoWindow = null;
//        }

//        // Close sidebar if open
//        this.closeBinSidebar();

//        // Clear selected bin
//        selectedBinId = null;
//    },

//    /**
//     * Clear all markers from the map
//     */
//    clearMarkers: function () {
//        markers.forEach(marker => {
//            marker.setMap(null);
//        });
//        markers = [];
//    },

//    /**
//     * Create an info window with bin details
//     * @param {object} bin - Bin data
//     * @returns {google.maps.InfoWindow} - Info window object
//     */
//    createBinInfoWindow: function (bin) {
//        // Create fill level bar color
//        const getFillLevelColor = (level) => {
//            if (!level) return '#6B7280'; // Gray for null/undefined

//            if (level >= 90) return '#EF4444'; // Red for 90-100%
//            if (level >= 70) return '#F97316'; // Orange for 70-90%
//            if (level >= 50) return '#F59E0B'; // Amber for 50-70%
//            if (level >= 30) return '#3B82F6'; // Blue for 30-50%
//            return '#10B981'; // Green for 0-30%
//        };

//        // Format device status
//        const getStatusText = (status) => {
//            switch (status) {
//                case 'Active': return '<span class="status-badge status-active">Aktif</span>';
//                case 'Inactive': return '<span class="status-badge status-inactive">Pasif</span>';
//                case 'Maintenance': return '<span class="status-badge status-maintenance">Bakımda</span>';
//                case 'Faulty': return '<span class="status-badge status-faulty">Arızalı</span>';
//                default: return '<span class="status-badge">Bilinmiyor</span>';
//            }
//        };

//        // Build info window content
//        const content = `
//            <div class="bin-info-window">
//                <div class="bin-info-header">
//                    <h3>${bin.Label}</h3>
//                    ${getStatusText(bin.DeviceStatus)}
//                </div>
                
//                <div class="bin-info-address">
//                    <i class="fas fa-map-marker-alt"></i> ${bin.Address}
//                </div>
                
//                <div class="bin-info-fill">
//                    <div class="bin-info-fill-label">
//                        <span>Doluluk:</span>
//                        <span>${bin.FillLevel || 0}%</span>
//                    </div>
//                    <div class="bin-info-fill-bar-bg">
//                        <div class="bin-info-fill-bar" style="width: ${bin.FillLevel || 0}%; background-color: ${getFillLevelColor(bin.FillLevel)}"></div>
//                    </div>
//                </div>
                
//                <div class="bin-info-date">
//                    <i class="fas fa-clock"></i> Son güncelleme: ${new Date(bin.UpdatedAt).toLocaleString('tr-TR')}
//                </div>
                
//                <div class="bin-info-actions">
//                    <button class="bin-info-button bin-info-details" onclick="googleMapsInterop.openBinSidebar(${JSON.stringify(bin)})">
//                        <i class="fas fa-info-circle"></i> Detaylar
//                    </button>
                    
//                    <button class="bin-info-button bin-info-edit" onclick="googleMapsInterop.editBin('${bin.WasteBinId}')">
//                        <i class="fas fa-edit"></i> Düzenle
//                    </button>
//                </div>
//            </div>
            
//            <style>
//                .bin-info-window {
//                    padding: 10px;
//                    max-width: 300px;
//                    font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
//                }
                
//                .bin-info-header {
//                    display: flex;
//                    justify-content: space-between;
//                    align-items: center;
//                    margin-bottom: 10px;
//                    border-bottom: 1px solid #E5E7EB;
//                    padding-bottom: 8px;
//                }
                
//                .bin-info-header h3 {
//                    margin: 0;
//                    font-size: 16px;
//                    font-weight: 600;
//                    color: #111827;
//                }
                
//                .status-badge {
//                    font-size: 12px;
//                    padding: 2px 8px;
//                    border-radius: 9999px;
//                    font-weight: 500;
//                }
                
//                .status-active {
//                    background-color: #D1FAE5;
//                    color: #065F46;
//                }
                
//                .status-inactive {
//                    background-color: #E5E7EB;
//                    color: #374151;
//                }
                
//                .status-maintenance {
//                    background-color: #FEF3C7;
//                    color: #92400E;
//                }
                
//                .status-faulty {
//                    background-color: #FEE2E2;
//                    color: #B91C1C;
//                }
                
//                .bin-info-address {
//                    margin-bottom: 12px;
//                    font-size: 14px;
//                    color: #4B5563;
//                }
                
//                .bin-info-fill {
//                    margin-bottom: 12px;
//                }
                
//                .bin-info-fill-label {
//                    display: flex;
//                    justify-content: space-between;
//                    font-size: 13px;
//                    margin-bottom: 4px;
//                }
                
//                .bin-info-fill-bar-bg {
//                    height: 8px;
//                    background-color: #E5E7EB;
//                    border-radius: 4px;
//                    overflow: hidden;
//                }
                
//                .bin-info-fill-bar {
//                    height: 100%;
//                    transition: width 0.5s ease-in-out;
//                }
                
//                .bin-info-date {
//                    font-size: 12px;
//                    color: #6B7280;
//                    margin-bottom: 12px;
//                }
                
//                .bin-info-actions {
//                    display: flex;
//                    justify-content: space-between;
//                    gap: 8px;
//                }
                
//                .bin-info-button {
//                    flex: 1;
//                    padding: 6px 12px;
//                    border-radius: 6px;
//                    border: none;
//                    font-size: 13px;
//                    font-weight: 500;
//                    cursor: pointer;
//                    display: flex;
//                    align-items: center;
//                    justify-content: center;
//                    gap: 4px;
//                    transition: background-color 0.2s;
//                }
                
//                .bin-info-details {
//                    background-color: #EFF6FF;
//                    color: #1D4ED8;
//                }
                
//                .bin-info-details:hover {
//                    background-color: #DBEAFE;
//                }
                
//                .bin-info-edit {
//                    background-color: #F3F4F6;
//                    color: #4B5563;
//                }
                
//                .bin-info-edit:hover {
//                    background-color: #E5E7EB;
//                }
//            </style>
//        `;

//        return new google.maps.InfoWindow({
//            content: content,
//            maxWidth: 320
//        });
//    },

//    /**
//     * Open sidebar with detailed bin information
//     * @param {object} bin - Bin data
//     */
//    openBinSidebar: function (bin) {
//        // Check if sidebar exists, create if not
//        let sidebar = document.getElementById('bin-sidebar');

//        if (!sidebar) {
//            sidebar = document.createElement('div');
//            sidebar.id = 'bin-sidebar';
//            document.body.appendChild(sidebar);
//        }

//        // Calculate estimated fill date
//        const getEstimatedFillDate = (fillLevel) => {
//            if (!fillLevel || fillLevel >= 100) return "Bilinmiyor";

//            const DAILY_FILL_RATE = 5.0; // Daily average fill rate percentage
//            const remainingCapacity = 100 - fillLevel;
//            const daysUntilFull = Math.ceil(remainingCapacity / DAILY_FILL_RATE);

//            const estimatedDate = new Date();
//            estimatedDate.setDate(estimatedDate.getDate() + daysUntilFull);

//            return estimatedDate.toLocaleDateString('tr-TR');
//        };

//        // Get estimated fill rate for a given number of days
//        const getEstimatedFillRate = (fillLevel, days) => {
//            if (!fillLevel) return 0;

//            const DAILY_FILL_RATE = 5.0;
//            const estimatedFill = fillLevel + (DAILY_FILL_RATE * days);

//            return Math.min(estimatedFill, 100);
//        };

//        // Get fill level color
//        const getFillLevelColor = (level) => {
//            if (!level) return '#6B7280'; // Gray for null/undefined

//            if (level >= 90) return '#EF4444'; // Red for 90-100%
//            if (level >= 70) return '#F97316'; // Orange for 70-90%
//            if (level >= 50) return '#F59E0B'; // Amber for 50-70%
//            if (level >= 30) return '#3B82F6'; // Blue for 30-50%
//            return '#10B981'; // Green for 0-30%
//        };

//        // Get fill status text
//        const getFillStatusText = (level) => {
//            if (!level) return "Durum Bilinmiyor";

//            if (level >= 90) return "Acil Boşaltılmalı";
//            if (level >= 70) return "Yakında Boşaltılmalı";
//            if (level >= 50) return "Orta Doluluk";
//            return "Boşaltma Gerekmiyor";
//        };

//        // Get fill status badge class
//        const getFillStatusBadge = (level) => {
//            if (!level) return "bg-gray-100 text-gray-800";

//            if (level >= 90) return "bg-red-100 text-red-800";
//            if (level >= 70) return "bg-orange-100 text-orange-800";
//            if (level >= 50) return "bg-yellow-100 text-yellow-800";
//            return "bg-green-100 text-green-800";
//        };

//        // Get device status
//        const getDeviceStatusText = (status) => {
//            switch (status) {
//                case 'Active': return "Aktif";
//                case 'Inactive': return "Pasif";
//                case 'Maintenance': return "Bakımda";
//                case 'Faulty': return "Arızalı";
//                default: return status;
//            }
//        };

//        // Get device status color
//        const getDeviceStatusColor = (status) => {
//            switch (status) {
//                case 'Active': return "bg-green-100 text-green-800";
//                case 'Inactive': return "bg-gray-100 text-gray-800";
//                case 'Maintenance': return "bg-yellow-100 text-yellow-800";
//                case 'Faulty': return "bg-red-100 text-red-800";
//                default: return "bg-gray-100 text-gray-800";
//            }
//        };

//        // Sample image for demo
//        const sampleImage = 'https://via.placeholder.com/300x200/10B981/FFFFFF?text=Çöp+Kutusu+Görüntüsü';

//        // Build sidebar content
//        const content = `
//            <div class="bin-sidebar-content">
//                <div class="bin-sidebar-header">
//                    <h2>
//                        <i class="fas fa-dumpster"></i>
//                        ${bin.Label}
//                    </h2>
//                    <button class="bin-sidebar-close" onclick="googleMapsInterop.closeBinSidebar()">
//                        <i class="fas fa-times"></i>
//                    </button>
//                </div>
                
//                <div class="bin-sidebar-body">
//                    <!-- Status and Address -->
//                    <div class="bin-sidebar-section">
//                        <div class="bin-status-badge ${getDeviceStatusColor(bin.DeviceStatus)}">
//                            ${getDeviceStatusText(bin.DeviceStatus)}
//                        </div>
                        
//                        <div class="bin-sidebar-address">
//                            <i class="fas fa-map-marker-alt"></i>
//                            ${bin.Address}
//                        </div>
//                    </div>
                    
//                    <!-- Bin Image -->
//                    <div class="bin-sidebar-section">
//                        <h3>Son Görüntü</h3>
//                        <div class="bin-image-container">
//                            <img src="${sampleImage}" alt="Çöp Kutusu Görüntüsü" class="bin-image" />
//                            <div class="bin-image-date">
//                                <i class="fas fa-camera"></i> Çekilme Tarihi: ${new Date().toLocaleDateString('tr-TR')}
//                            </div>
//                        </div>
//                    </div>
                    
//                    <!-- Fill Level Visualization -->
//                    <div class="bin-sidebar-section">
//                        <h3>Doluluk Durumu</h3>
                        
//                        <div class="bin-sidebar-fill-circle">
//                            <svg viewBox="0 0 36 36" class="bin-circle-progress">
//                                <path class="bin-circle-bg"
//                                      d="M18 2.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831" />
//                                <path class="bin-circle-fill" style="stroke: ${getFillLevelColor(bin.FillLevel)};"
//                                      stroke-dasharray="${bin.FillLevel}, 100"
//                                      d="M18 2.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831" />
//                                <text x="18" y="20.5" class="bin-circle-text">
//                                    ${bin.FillLevel || 0}%
//                                </text>
//                            </svg>
//                        </div>
                        
//                        <div class="bin-fill-status ${getFillStatusBadge(bin.FillLevel)}">
//                            ${getFillStatusText(bin.FillLevel)}
//                        </div>
                        
//                        <!-- Fill Rate Predictions -->
//                        <div class="bin-fill-predictions">
//                            <div class="bin-prediction-item">
//                                <span>Tahmini dolma tarihi:</span>
//                                <strong>${getEstimatedFillDate(bin.FillLevel)}</strong>
//                            </div>
                            
//                            <div class="bin-prediction-item">
//                                <span>3 Gün Sonra Doluluk:</span>
//                                <strong style="color: ${getFillLevelColor(getEstimatedFillRate(bin.FillLevel, 3))}">
//                                    %${getEstimatedFillRate(bin.FillLevel, 3).toFixed(0)}
//                                </strong>
//                            </div>
                            
//                            <div class="bin-prediction-item">
//                                <span>7 Gün Sonra Doluluk:</span>
//                                <strong style="color: ${getFillLevelColor(getEstimatedFillRate(bin.FillLevel, 7))}">
//                                    %${getEstimatedFillRate(bin.FillLevel, 7).toFixed(0)}
//                                </strong>
//                            </div>
//                        </div>
//                    </div>
                    
//                    <!-- Details -->
//                    <div class="bin-sidebar-section">
//                        <h3>Genel Bilgiler</h3>
                        
//                        <div class="bin-details-grid">
//                            <div class="bin-detail-item">
//                                <span>Oluşturulma Tarihi:</span>
//                                <strong>${new Date(bin.CreatedAt).toLocaleString('tr-TR')}</strong>
//                            </div>
                            
//                            <div class="bin-detail-item">
//                                <span>Güncellenme Tarihi:</span>
//                                <strong>${new Date(bin.UpdatedAt).toLocaleString('tr-TR')}</strong>
//                            </div>
                            
//                            <div class="bin-detail-item">
//                                <span>Doluluk Seviyesi:</span>
//                                <strong style="color: ${getFillLevelColor(bin.FillLevel)}">
//                                    ${bin.FillLevel || 0}%
//                                </strong>
//                            </div>
                            
//                            <div class="bin-detail-item">
//                                <span>Durum:</span>
//                                <strong class="bin-status-text-${bin.DeviceStatus.toLowerCase()}">
//                                    ${getDeviceStatusText(bin.DeviceStatus)}
//                                </strong>
//                            </div>
//                        </div>
//                    </div>
                    
//                    <!-- Sensors -->
//                    <div class="bin-sidebar-section">
//                        <h3>
//                            <i class="fas fa-microchip"></i>
//                            Sensörler
//                            <span class="bin-sensor-count">
//                                ${bin.Sensors ? bin.Sensors.length : 0} adet
//                            </span>
//                        </h3>
                        
//                        <div class="bin-sensors-list">
//                            ${bin.Sensors && bin.Sensors.length > 0
//                ? bin.Sensors.map(sensor => `
//                                    <div class="bin-sensor-item ${sensor.IsActive ? 'sensor-active' : 'sensor-inactive'}">
//                                        <div class="bin-sensor-header">
//                                            <span>${sensor.Type}</span>
//                                            <span class="bin-sensor-status">
//                                                <i class="fas ${sensor.IsActive ? 'fa-check-circle' : 'fa-times-circle'}"></i>
//                                                ${sensor.IsActive ? 'Aktif' : 'Pasif'}
//                                            </span>
//                                        </div>
//                                        <div class="bin-sensor-date">
//                                            Son Güncelleme: ${sensor.LastUpdate ? new Date(sensor.LastUpdate).toLocaleDateString('tr-TR') : '-'}
//                                        </div>
//                                    </div>
//                                `).join('')
//                : `<div class="bin-no-sensors">Sensör bulunamadı</div>`
//            }
//                        </div>
//                    </div>
                    
//                    <!-- Actions -->
//                    <div class="bin-sidebar-actions">
//                        <button class="bin-action-button bin-action-edit" onclick="googleMapsInterop.editBin('${bin.WasteBinId}')">
//                            <i class="fas fa-edit"></i> Düzenle
//                        </button>
                        
//                        <!-- Requirement #5: Enhanced zoom button for smooth animation -->
//                        <button class="bin-action-button bin-action-focus" onclick="googleMapsInterop.focusOnBin('${bin.WasteBinId}')">
//                            <i class="fas fa-search"></i> Yakınlaştır
//                        </button>
//                    </div>
//                </div>
//            </div>
//        `;

//        // Set sidebar content
//        sidebar.innerHTML = content;

//        // Show sidebar
//        sidebar.classList.add('bin-sidebar-visible');
//        sidebarOpen = true;

//        // Add styles if not already added
//        if (!document.getElementById('bin-sidebar-styles')) {
//            const style = document.createElement('style');
//            style.id = 'bin-sidebar-styles';
//            style.textContent = `
//                #bin-sidebar {
//                    position: fixed;
//                    top: 0;
//                    right: -400px;
//                    width: 380px;
//                    height: 100vh;
//                    background-color: white;
//                    box-shadow: -5px 0 15px rgba(0, 0, 0, 0.1);
//                    z-index: 1000;
//                    overflow-y: auto;
//                    transition: right 0.3s ease-in-out;
//                }
                
//                #bin-sidebar.bin-sidebar-visible {
//                    right: 0;
//                }
                
//                .bin-sidebar-content {
//                    display: flex;
//                    flex-direction: column;
//                    height: 100%;
//                }
                
//                .bin-sidebar-header {
//                    display: flex;
//                    justify-content: space-between;
//                    align-items: center;
//                    padding: 16px;
//                    border-bottom: 1px solid #E5E7EB;
//                    background-color: #F9FAFB;
//                }
                
//                .bin-sidebar-header h2 {
//                    margin: 0;
//                    font-size: 1.25rem;
//                    font-weight: 600;
//                    color: #111827;
//                    display: flex;
//                    align-items: center;
//                    gap: 8px;
//                }
                
//                .bin-sidebar-close {
//                    background: none;
//                    border: none;
//                    color: #6B7280;
//                    cursor: pointer;
//                    font-size: 1.25rem;
//                    padding: 4px;
//                    border-radius: 9999px;
//                    display: flex;
//                    align-items: center;
//                    justify-content: center;
//                    transition: background-color 0.2s;
//                }
                
//                .bin-sidebar-close:hover {
//                    background-color: #F3F4F6;
//                    color: #1F2937;
//                }
                
//                .bin-sidebar-body {
//                    flex: 1;
//                    padding: 16px;
//                    display: flex;
//                    flex-direction: column;
//                    gap: 24px;
//                    overflow-y: auto;
//                }
                
//                .bin-sidebar-section {
//                    background-color: #F9FAFB;
//                    border-radius: 8px;
//                    padding: 16px;
//                    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
//                }
                
//                .bin-sidebar-section h3 {
//                    margin-top: 0;
//                    margin-bottom: 16px;
//                    font-size: 1rem;
//                    font-weight: 600;
//                    color: #374151;
//                    border-bottom: 1px solid #E5E7EB;
//                    padding-bottom: 8px;
//                    display: flex;
//                    align-items: center;
//                    gap: 8px;
//                }
                
//                .bin-status-badge {
//                    display: inline-block;
//                    padding: 4px 12px;
//                    border-radius: 9999px;
//                    font-size: 0.875rem;
//                    font-weight: 500;
//                    margin-bottom: 12px;
//                }
                
//                .bin-sidebar-address {
//                    font-size: 0.938rem;
//                    color: #4B5563;
//                    display: flex;
//                    align-items: flex-start;
//                    gap: 8px;
//                }
                
//                .bin-sidebar-address i {
//                    color: #EF4444;
//                    margin-top: 3px;
//                }
                
//                .bin-image-container {
//                    width: 100%;
//                    position: relative;
//                    border-radius: 8px;
//                    overflow: hidden;
//                    margin-bottom: 8px;
//                }
                
//                .bin-image {
//                    width: 100%;
//                    height: auto;
//                    display: block;
//                    border-radius: 6px;
//                }
                
//                .bin-image-date {
//                    font-size: 0.75rem;
//                    color: #6B7280;
//                    margin-top: 8px;
//                    text-align: right;
//                }
                
//                .bin-sidebar-fill-circle {
//                    display: flex;
//                    justify-content: center;
//                    margin: 16px 0;
//                }
                
//                .bin-circle-progress {
//                    width: 120px;
//                    height: 120px;
//                }
                
//                .bin-circle-bg {
//                    fill: none;
//                    stroke: #E5E7EB;
//                    stroke-width: 3.8;
//                }
                
//                .bin-circle-fill {
//                    fill: none;
//                    stroke-width: 3.8;
//                    stroke-linecap: round;
//                    transform: rotate(-90deg);
//                    transform-origin: 50% 50%;
//                    transition: stroke-dasharray 1s ease;
//                }
                
//                .bin-circle-text {
//                    font-family: sans-serif;
//                    font-size: 0.5rem;
//                    font-weight: bold;
//                    fill: #1F2937;
//                    text-anchor: middle;
//                    dominant-baseline: middle;
//                }
                
//                .bin-fill-status {
//                    display: flex;
//                    justify-content: center;
//                    padding: 6px 12px;
//                    border-radius: 9999px;
//                    font-size: 0.875rem;
//                    font-weight: 500;
//                    margin: 0 auto;
//                    width: fit-content;
//                }
                
//                .bin-fill-predictions {
//                    margin-top: 16px;
//                    display: flex;
//                    flex-direction: column;
//                    gap: 8px;
//                    border-top: 1px solid #E5E7EB;
//                    padding-top: 16px;
//                }
                
//                .bin-prediction-item {
//                    display: flex;
//                    justify-content: space-between;
//                    font-size: 0.875rem;
//                }
                
//                .bin-prediction-item span {
//                    color: #6B7280;
//                }
                
//                .bin-details-grid {
//                    display: grid;
//                    grid-template-columns: 1fr;
//                    gap: 12px;
//                }
                
//                .bin-detail-item {
//                    display: flex;
//                    justify-content: space-between;
//                    font-size: 0.875rem;
//                    padding-bottom: 8px;
//                    border-bottom: 1px solid #E5E7EB;
//                }
                
//                .bin-detail-item:last-child {
//                    border-bottom: none;
//                }
                
//                .bin-detail-item span {
//                    color: #6B7280;
//                }
                
//                .bin-status-text-active {
//                    color: #059669;
//                }
                
//                .bin-status-text-inactive {
//                    color: #6B7280;
//                }
                
//                .bin-status-text-maintenance {
//                    color: #D97706;
//                }
                
//                .bin-status-text-faulty {
//                    color: #DC2626;
//                }
                
//                .bin-sensor-count {
//                    font-size: 0.75rem;
//                    padding: 2px 8px;
//                    background-color: #E5E7EB;
//                    color: #4B5563;
//                    border-radius: 9999px;
//                    margin-left: 8px;
//                    font-weight: 400;
//                }
                
//                .bin-sensors-list {
//                    display: flex;
//                    flex-direction: column;
//                    gap: 8px;
//                    max-height: 200px;
//                    overflow-y: auto;
//                }
                
//                .bin-sensor-item {
//                    padding: 10px;
//                    border-radius: 6px;
//                    border-left-width: 4px;
//                    border-left-style: solid;
//                    transition: transform 0.2s;
//                }
                
//                .bin-sensor-item:hover {
//                    transform: translateX(4px);
//                }
                
//                .bin-sensor-item.sensor-active {
//                    background-color: #F0FDF4;
//                    border-left-color: #10B981;
//                }
                
//                .bin-sensor-item.sensor-inactive {
//                    background-color: #FEF2F2;
//                    border-left-color: #EF4444;
//                }
                
//                .bin-sensor-header {
//                    display: flex;
//                    justify-content: space-between;
//                    font-weight: 500;
//                    margin-bottom: 4px;
//                }
                
//                .bin-sensor-status {
//                    font-size: 0.75rem;
//                    display: flex;
//                    align-items: center;
//                    gap: 4px;
//                }
                
//                .bin-sensor-item.sensor-active .bin-sensor-status {
//                    color: #10B981;
//                }
                
//                .bin-sensor-item.sensor-inactive .bin-sensor-status {
//                    color: #EF4444;
//                }
                
//                .bin-sensor-date {
//                    font-size: 0.75rem;
//                    color: #6B7280;
//                }
                
//                .bin-no-sensors {
//                    text-align: center;
//                    padding: 12px;
//                    color: #6B7280;
//                    background-color: #F3F4F6;
//                    border-radius: 6px;
//                }
                
//                .bin-sidebar-actions {
//                    display: flex;
//                    gap: 12px;
//                    margin-top: 8px;
//                }
                
//                .bin-action-button {
//                    flex: 1;
//                    padding: 10px;
//                    border: none;
//                    border-radius: 6px;
//                    font-size: 0.938rem;
//                    font-weight: 500;
//                    cursor: pointer;
//                    display: flex;
//                    align-items: center;
//                    justify-content: center;
//                    gap: 8px;
//                    transition: background-color 0.2s, transform 0.1s;
//                }
                
//                .bin-action-button:hover {
//                    transform: translateY(-2px);
//                }
                
//                .bin-action-button:active {
//                    transform: translateY(0);
//                }
                
//                .bin-action-edit {
//                    background-color: #F3F4F6;
//                    color: #4B5563;
//                }
                
//                .bin-action-edit:hover {
//                    background-color: #E5E7EB;
//                }
                
//                .bin-action-focus {
//                    background-color: #EFF6FF;
//                    color: #2563EB;
//                }
                
//                .bin-action-focus:hover {
//                    background-color: #DBEAFE;
//                }
                
//                /* Dark mode support */
//                @media (prefers-color-scheme: dark) {
//                    #bin-sidebar {
//                        background-color: #1F2937;
//                        box-shadow: -5px 0 15px rgba(0, 0, 0, 0.3);
//                    }
                    
//                    .bin-sidebar-header {
//                        background-color: #111827;
//                        border-bottom-color: #374151;
//                    }
                    
//                    .bin-sidebar-header h2 {
//                        color: #F9FAFB;
//                    }
                    
//                    .bin-sidebar-close {
//                        color: #9CA3AF;
//                    }
                    
//                    .bin-sidebar-close:hover {
//                        background-color: #374151;
//                        color: #F3F4F6;
//                    }
                    
//                    .bin-sidebar-section {
//                        background-color: #111827;
//                    }
                    
//                    .bin-sidebar-section h3 {
//                        color: #E5E7EB;
//                        border-bottom-color: #4B5563;
//                    }
                    
//                    .bin-sidebar-address {
//                        color: #D1D5DB;
//                    }
                    
//                    .bin-circle-bg {
//                        stroke: #4B5563;
//                    }
                    
//                    .bin-circle-text {
//                        fill: #E5E7EB;
//                    }
                    
//                    .bin-detail-item {
//                        border-bottom-color: #4B5563;
//                    }
                    
//                    .bin-detail-item span {
//                        color: #9CA3AF;
//                    }
                    
//                    .bin-sensor-item.sensor-active {
//                        background-color: rgba(16, 185, 129, 0.1);
//                    }
                    
//                    .bin-sensor-item.sensor-inactive {
//                        background-color: rgba(239, 68, 68, 0.1);
//                    }
                    
//                    .bin-sensor-date {
//                        color: #9CA3AF;
//                    }
                    
//                    .bin-no-sensors {
//                        color: #9CA3AF;
//                        background-color: #374151;
//                    }
                    
//                    .bin-action-edit {
//                        background-color: #374151;
//                        color: #E5E7EB;
//                    }
                    
//                    .bin-action-edit:hover {
//                        background-color: #4B5563;
//                    }
                    
//                    .bin-action-focus {
//                        background-color: rgba(37, 99, 235, 0.2);
//                        color: #93C5FD;
//                    }
                    
//                    .bin-action-focus:hover {
//                        background-color: rgba(37, 99, 235, 0.3);
//                    }
                    
//                    .bin-image-date {
//                        color: #9CA3AF;
//                    }
//                }
//            `;
//            document.head.appendChild(style);
//        }
//    },

//    /**
//     * Close the bin sidebar
//     */
//    closeBinSidebar: function () {
//        const sidebar = document.getElementById('bin-sidebar');
//        if (sidebar) {
//            sidebar.classList.remove('bin-sidebar-visible');
//            sidebarOpen = false;
//        }
//    },

//    /**
//     * Edit a bin (call .NET method)
//     * @param {string} binId - ID of the bin to edit
//     */
//    editBin: function (binId) {
//        if (dotNetRef) {
//            dotNetRef.invokeMethodAsync('EditBin', binId);
//        }
//    },

//    /**
//     * Show a specific bin in the sidebar
//     * @param {object} bin - Bin data
//     */
//    showBinSidebar: function (bin) {
//        this.openBinSidebar(bin);
//    },

//    /**
//     * Get marker icon based on bin status and fill level
//     * @param {string} status - Bin status
//     * @param {number} fillLevel - Fill level percentage
//     * @param {number} opacity - Icon opacity (0-1)
//     * @returns {object} - Marker icon configuration
//     */
//    getBinMarkerIcon: function (status, fillLevel, opacity = 1) {
//        // Define colors based on status and fill level
//        let fillColor;
//        let strokeColor;

//        // Determine colors based on fill level
//        if (fillLevel >= 90) {
//            fillColor = '#EF4444'; // Red for 90-100%
//            strokeColor = '#B91C1C';
//        } else if (fillLevel >= 70) {
//            fillColor = '#F97316'; // Orange for 70-90%
//            strokeColor = '#C2410C';
//        } else if (fillLevel >= 50) {
//            fillColor = '#F59E0B'; // Amber for 50-70%
//            strokeColor = '#B45309';
//        } else if (fillLevel >= 30) {
//            fillColor = '#3B82F6'; // Blue for 30-50%
//            strokeColor = '#1D4ED8';
//        } else {
//            fillColor = '#10B981'; // Green for 0-30%
//            strokeColor = '#059669';
//        }

//        // Adjust for status
//        if (status === 'Inactive') {
//            fillColor = '#9CA3AF'; // Gray
//            strokeColor = '#6B7280';
//        } else if (status === 'Maintenance') {
//            // Make the color more yellowish
//            fillColor = '#FBBF24';
//            strokeColor = '#D97706';
//        } else if (status === 'Faulty') {
//            // Keep red but apply a pattern
//            fillColor = '#F87171';
//            strokeColor = '#DC2626';
//        }

//        // Create SVG for pin-style trash bin icon - NEW DESIGN
//        const svg = `
//            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="32" height="48">
//                <!-- Pin Top -->
//                <circle cx="12" cy="10" r="10" fill="${fillColor}" stroke="${strokeColor}" stroke-width="1.5" opacity="${opacity}" />
                
//                <!-- Pin Bottom -->
//                <path d="M12,20 L16,32 L8,32 Z" fill="${fillColor}" stroke="${strokeColor}" stroke-width="1" opacity="${opacity}" />
                
//                <!-- Trash Bin Icon -->
//                <g transform="translate(6, 5) scale(0.6)">
//                    <path d="M4,6 L4,18 C4,19.1 4.9,20 6,20 L14,20 C15.1,20 16,19.1 16,18 L16,6" fill="white" stroke="white" stroke-width="1" />
//                    <path d="M3,6 L17,6" stroke="white" stroke-width="2" stroke-linecap="round" />
//                    <path d="M8,6 L8,4 C8,3.4 8.4,3 9,3 L11,3 C11.6,3 12,3.4 12,4 L12,6" fill="white" stroke="white" stroke-width="1" />
                    
//                    <!-- Fill level indicator -->
//                    <rect x="5" y="${20 - (fillLevel / 10)}" width="10" height="${fillLevel / 10}" fill="white" opacity="0.8" />
//                </g>
                
//                <!-- Status indicators -->
//                ${status === 'Faulty' ? '<circle cx="18" cy="5" r="3" fill="#DC2626" stroke="white" stroke-width="1" />' : ''}
//                ${status === 'Maintenance' ? '<circle cx="18" cy="5" r="3" fill="#D97706" stroke="white" stroke-width="1" />' : ''}
//                ${status === 'Inactive' ? '<circle cx="18" cy="5" r="3" fill="#6B7280" stroke="white" stroke-width="1" />' : ''}
//            </svg>
//        `;

//        // Convert SVG to base64 data URL
//        const svgBase64 = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(svg)));

//        return {
//            url: svgBase64,
//            scaledSize: new google.maps.Size(32, 48),
//            anchor: new google.maps.Point(16, 40),
//            labelOrigin: new google.maps.Point(12, -8)
//        };
//    }
//};

///**y
// * Reset filter dropdowns (called from Blazor)
// */
//window.resetFilterDropdowns = function () {
//    // Reset select elements for filters
//    const statusDropdown = document.querySelector('select[onchange*="FilterByStatus"]');
//    const fillLevelDropdown = document.querySelector('select[onchange*="FilterByFillLevel"]');

//    if (statusDropdown) statusDropdown.value = '';
//    if (fillLevelDropdown) fillLevelDropdown.value = '';
//};