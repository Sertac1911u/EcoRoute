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
        resetMapState
    };

    // API callback
    window.initGoogleMapsCallback = function () {
        console.log("Google Maps API yüklendi");
        if (dotNetHelper) dotNetHelper.invokeMethodAsync('OnMapInitialized');
    };
}

// Ana harita başlatma
function initializeMap() {
    resetMapState();

    const mapElement = document.getElementById('google-map');
    if (!mapElement) return false;

    map = new google.maps.Map(mapElement, {
        center: { lat: 41.0082, lng: 28.9784 },
        zoom: 12,
        mapTypeControl: false,
        streetViewControl: false,
        fullscreenControl: false
    });

    // Kısa bir gecikme ekle
    setTimeout(() => {
        infoWindow = new google.maps.InfoWindow({ maxWidth: 300 });

        // Haritada tıklama olayı
        map.addListener('click', (e) => {
            setMarkerPosition(e.latLng.lat(), e.latLng.lng());
            if (dotNetHelper) {
                dotNetHelper.invokeMethodAsync('UpdateCoordinates', e.latLng.lat(), e.latLng.lng());

                // Adres bilgisini almak için geocoding
                const geocoder = new google.maps.Geocoder();
                geocoder.geocode({ location: e.latLng }, (results, status) => {
                    if (status === 'OK' && results[0]) {
                        dotNetHelper.invokeMethodAsync('UpdateAddress', results[0].formatted_address);
                    }
                });
            }
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
                scaledSize: new google.maps.Size(40, 40)
            }
        });

        // Sürükleme bittiğinde koordinatları güncelle
        createMarker.addListener('dragend', () => {
            const newPos = createMarker.getPosition();
            if (dotNetHelper) {
                dotNetHelper.invokeMethodAsync('UpdateCoordinates', newPos.lat(), newPos.lng());

                // Adres bilgisini almak için geocoding
                const geocoder = new google.maps.Geocoder();
                geocoder.geocode({ location: newPos }, (results, status) => {
                    if (status === 'OK' && results[0]) {
                        dotNetHelper.invokeMethodAsync('UpdateAddress', results[0].formatted_address);
                    }
                });
            }
        });
    } else {
        createMarker.setPosition(position);
    }

    map.panTo(position);
    return true;
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

// Atık kutusu işaretçisi ekle
function addBinMarker(bin, targetMap) {
    if (!targetMap) return null;

    try {
        // Pascal case (Blazor) to camelCase (JS) dönüşümü
        const position = new google.maps.LatLng(
            bin.Latitude || bin.latitude,
            bin.Longitude || bin.longitude
        );

        // Doluluk seviyesi ve cihaz durumunu alın
        const fillLevel = bin.FillLevel || bin.fillLevel || 0;
        const deviceStatus = bin.DeviceStatus || bin.deviceStatus || 'Active';
        const wasteBinId = bin.WasteBinId || bin.wasteBinId;

        // İşaretçi oluşturma
        const marker = new google.maps.Marker({
            position: position,
            map: targetMap,
            icon: {
                url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(createRecycleBinSVG(deviceStatus, fillLevel)),
                scaledSize: new google.maps.Size(32, 32)
            },
            title: bin.Label || bin.label || 'Atık Kutusu',
            binId: wasteBinId
        });

        // İşaretçi tıklama olayı
        marker.addListener('click', () => {
            if (infoWindow) infoWindow.close();

            const infoContent = createInfoWindowContent(bin);
            infoWindow.setContent(infoContent);
            infoWindow.open(targetMap, marker);

            // "Detayları Görüntüle" düğmesi için olay dinleyicisi
            setTimeout(() => {
                const detailButton = document.getElementById(`view-detail-${wasteBinId}`);
                if (detailButton) {
                    detailButton.addEventListener('click', (e) => {
                        e.preventDefault();
                        if (dotNetHelper) {
                            dotNetHelper.invokeMethodAsync('OpenBinDetail', wasteBinId);
                            infoWindow.close();
                        }
                    });
                }
            }, 100);
        });

        // İşaretçiyi listeye ekle
        markers.push(marker);
        return marker;
    } catch (error) {
        console.error("İşaretçi eklenirken hata:", error);
        return null;
    }
}

// Bilgi penceresi içeriği oluştur
function createInfoWindowContent(bin) {
    try {
        // Pascal case veya camelCase'den değerleri al
        const label = bin.Label || bin.label || 'Bilinmeyen';
        const address = bin.Address || bin.address || 'Konum bilgisi yok';
        const fillLevel = bin.FillLevel || bin.fillLevel || 0;
        const deviceStatus = bin.DeviceStatus || bin.deviceStatus || 'Active';
        const updatedAt = bin.UpdatedAt || bin.updatedAt || new Date().toISOString();
        const wasteBinId = bin.WasteBinId || bin.wasteBinId;

        // Durum ve doluluk renklerini belirle
        const statusColor = getStatusColor(deviceStatus);
        const fillLevelColor = getFillLevelColor(fillLevel);

        // Bilgi penceresi içeriği HTML
        return `
            <div style="font-family: Arial, sans-serif; padding: 8px; max-width: 280px;">
                <div style="font-size: 16px; font-weight: bold; margin-bottom: 5px; color: #333; border-bottom: 2px solid ${statusColor}; padding-bottom: 5px;">
                    ${label}
                </div>
                <div style="font-size: 12px; margin-bottom: 8px; color: #666;">
                    <i class="fas fa-map-marker-alt" style="color: #dc3545; margin-right: 5px;"></i>${address}
                </div>
                <div style="display: flex; margin-bottom: 8px;">
                    <div style="flex: 1; text-align: center; background-color: ${statusColor}; color: white; padding: 3px 5px; border-radius: 3px; font-size: 11px; margin-right: 5px;">
                        ${getStatusText(deviceStatus)}
                    </div>
                    <div style="flex: 1; text-align: center; background-color: ${fillLevelColor}; color: white; padding: 3px 5px; border-radius: 3px; font-size: 11px;">
                        ${getFillLevelText(fillLevel)}
                    </div>
                </div>
                <div style="margin-bottom: 8px;">
                    <div style="font-size: 11px; color: #666; margin-bottom: 3px;">Doluluk Seviyesi:</div>
                    <div style="background-color: #e9ecef; height: 10px; border-radius: 5px; overflow: hidden;">
                        <div style="background-color: ${fillLevelColor}; width: ${fillLevel}%; height: 100%;"></div>
                    </div>
                    <div style="text-align: right; font-size: 11px; color: #666; margin-top: 2px;">${fillLevel}%</div>
                </div>
                <div style="font-size: 11px; color: #666; margin-bottom: 8px;">
                    Son Güncelleme: ${formatDate(updatedAt)}
                </div>
                <div style="text-align: center;">
                    <button id="view-detail-${wasteBinId}" style="background-color: #3089d6; color: white; border: none; padding: 5px 10px; border-radius: 3px; font-size: 12px; cursor: pointer;">
                        Detayları Görüntüle
                    </button>
                </div>
            </div>
        `;
    } catch (error) {
        console.error("Bilgi penceresi oluşturulurken hata:", error);
        return "<div>Bilgi yüklenirken hata oluştu</div>";
    }
}

// Konum haritası yükleme
function loadLocationMap(containerId, lat, lng) {
    try {
        const container = document.getElementById(containerId);
        if (!container) return null;

        // Yükleme denemelerini kaydet
        mapTries[containerId] = (mapTries[containerId] || 0) + 1;

        // Element görünür mü kontrol et
        if (container.offsetWidth === 0 || container.offsetHeight === 0) {
            if (mapTries[containerId] < 3) {
                setTimeout(() => loadLocationMap(containerId, lat, lng), 500);
                return null;
            }
        }

        // Önceki haritayı temizle
        if (locationMap) {
            locationMap = null;
        }

        // Pozisyon oluştur
        const position = new google.maps.LatLng(lat, lng);

        // Haritayı oluştur  
        locationMap = new google.maps.Map(container, {
            center: position,
            zoom: 16,
            mapTypeControl: true,
            streetViewControl: true,
            fullscreenControl: true
        });

        // İşaretçi ekle
        setTimeout(() => {
            new google.maps.Marker({
                position: position,
                map: locationMap,
                animation: google.maps.Animation.DROP,
                icon: {
                    url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(createRecycleBinSVG('Active', 50)),
                    scaledSize: new google.maps.Size(40, 40)
                }
            });

            // Çember ekle
            new google.maps.Circle({
                map: locationMap,
                center: position,
                radius: 30,
                fillColor: "#3089d6",
                fillOpacity: 0.15,
                strokeColor: "#3089d6",
                strokeWeight: 1
            });

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
function loadDetailMap(mapId, lat, lng) {
    try {
        const container = document.getElementById(mapId);
        if (!container) return null;

        // Yükleme denemelerini kaydet
        mapTries[mapId] = (mapTries[mapId] || 0) + 1;

        // Element görünür mü kontrol et
        if (container.offsetWidth === 0 || container.offsetHeight === 0) {
            if (mapTries[mapId] < 3) {
                setTimeout(() => loadDetailMap(mapId, lat, lng), 500);
                return null;
            }
        }

        // Önceki haritayı temizle
        if (detailMaps[mapId]) {
            detailMaps[mapId] = null;
        }

        // Pozisyon oluştur
        const position = new google.maps.LatLng(lat, lng);

        // Haritayı oluştur
        const detailMap = new google.maps.Map(container, {
            center: position,
            zoom: 15,
            mapTypeControl: false,
            streetViewControl: false,
            zoomControl: false,
            fullscreenControl: false,
            draggable: false
        });

        detailMaps[mapId] = detailMap;

        // İşaretçi ekle
        setTimeout(() => {
            new google.maps.Marker({
                position: position,
                map: detailMap,
                icon: {
                    url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(createRecycleBinSVG('Active', 50)),
                    scaledSize: new google.maps.Size(32, 32)
                }
            });

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

// Geri dönüşüm kutusu SVG'si oluştur
function createRecycleBinSVG(status, fillLevel) {
    // Her durum için farklı bir ana renk belirle
    let mainColor;
    switch (status) {
        case 'Active':
            // Aktif durumda, doluluk seviyesine göre renk değişimi
            if (fillLevel >= 90) mainColor = "#dc3545"; // Kırmızı
            else if (fillLevel >= 70) mainColor = "#fd7e14"; // Turuncu
            else if (fillLevel >= 50) mainColor = "#ffc107"; // Sarı
            else if (fillLevel >= 30) mainColor = "#3089d6"; // Mavi
            else mainColor = "#28a745"; // Yeşil
            break;
        case 'Inactive': mainColor = "#6c757d"; break; // Gri
        case 'Maintenance': mainColor = "#ffc107"; break; // Sarı
        case 'Faulty': mainColor = "#dc3545"; break; // Kırmızı
        default: mainColor = "#6c757d"; // Varsayılan gri
    }

    // Doluluk göstergesinin yüksekliği
    const fillHeight = Math.min(Math.max(fillLevel, 0), 100) * 0.7 / 100;

    // Geri dönüşüm kutusu SVG'si
    return `
    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 64 64" width="32" height="32">
      <!-- Arkaplan daire -->
      <circle cx="32" cy="32" r="30" fill="white" stroke="${mainColor}" stroke-width="2"/>
      
      <!-- Çöp kutusu gövdesi -->
      <rect x="16" y="18" width="32" height="36" rx="3" fill="${mainColor}" />
      
      <!-- Çöp kutusu kapağı -->
      <rect x="14" y="16" width="36" height="4" rx="1" fill="${mainColor}" />
      
      <!-- Sap/tutacak bölümü -->
      <rect x="28" y="12" width="8" height="4" rx="1" fill="${mainColor}" />
      
      <!-- Geri dönüşüm sembolü -->
      <path d="M32,25 L36,32 L28,32 Z M28,30 L24,37 L32,37 Z M36,30 L40,37 L32,37 Z" fill="white" stroke="white" stroke-width="0.5" />
      
      <!-- Doluluk seviyesi göstergesi -->
      <rect x="18" y="${44 - fillHeight * 20}" width="28" height="${fillHeight * 20}" fill="rgba(255,255,255,0.3)" />
      
      <!-- Çöp kutusu gölgesi -->
      <rect x="16" y="52" width="32" height="2" rx="1" fill="rgba(0,0,0,0.2)" />
    </svg>
    `;
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