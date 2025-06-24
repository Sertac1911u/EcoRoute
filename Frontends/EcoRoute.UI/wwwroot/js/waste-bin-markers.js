
window.wasteBinMarkers = {
    calculateDistance: function (lat1, lon1, lat2, lon2) {
        const R = 6371; 
        const dLat = this.deg2rad(lat2 - lat1);
        const dLon = this.deg2rad(lon2 - lon1);
        const a =
            Math.sin(dLat / 2) * Math.sin(dLat / 2) +
            Math.cos(this.deg2rad(lat1)) * Math.cos(this.deg2rad(lat2)) *
            Math.sin(dLon / 2) * Math.sin(dLon / 2);
        const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        const distance = R * c; 
        return distance;
    },

    deg2rad: function (deg) {
        return deg * (Math.PI / 180);
    },

    findNearestBins: function (bins, lat, lng, maxDistance = 2) {
        if (!bins || !bins.length) return [];

        return bins.filter(bin => {
            const distance = this.calculateDistance(
                lat, lng,
                bin.Latitude, bin.Longitude
            );
            return distance <= maxDistance;
        }).sort((a, b) => {
            const distA = this.calculateDistance(lat, lng, a.Latitude, a.Longitude);
            const distB = this.calculateDistance(lat, lng, b.Latitude, b.Longitude);
            return distA - distB;
        });
    },

    getMarkerIcon: function (bin) {
        let markerColor = 'blue';

        switch (bin.DeviceStatus) {
            case 'Active':
                markerColor = bin.FillLevel >= 90 ? 'red' :
                    bin.FillLevel >= 70 ? 'orange' :
                        bin.FillLevel >= 50 ? 'yellow' : 'green';
                break;
            case 'Inactive':
                markerColor = 'gray';
                break;
            case 'Maintenance':
                markerColor = 'yellow';
                break;
            case 'Faulty':
                markerColor = 'red';
                break;
            default:
                markerColor = 'blue';
        }

        return {
            url: `https://maps.google.com/mapfiles/ms/icons/${markerColor}-dot.png`,
            scaledSize: new google.maps.Size(35, 35)
        };
    },

    formatBinInfo: function (bin) {
        const lastUpdate = new Date(bin.UpdatedAt).toLocaleString('tr-TR', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });

        const statusText = bin.DeviceStatus === 'Active' ? 'Aktif' :
            bin.DeviceStatus === 'Inactive' ? 'Pasif' :
                bin.DeviceStatus === 'Maintenance' ? 'Bakımda' : 'Arızalı';

        const fillLevelText = bin.FillLevel >= 90 ? 'Acil Boşaltılmalı' :
            bin.FillLevel >= 70 ? 'Yakında Boşaltılmalı' :
                bin.FillLevel >= 50 ? 'Orta Doluluk' : 'Boşaltma Gerekmiyor';

        return {
            label: bin.Label,
            address: bin.Address,
            coordinates: `${bin.Latitude.toFixed(6)}, ${bin.Longitude.toFixed(6)}`,
            status: statusText,
            fillLevel: bin.FillLevel || 0,
            fillLevelText: fillLevelText,
            lastUpdate: lastUpdate
        };
    }
};

document.addEventListener('DOMContentLoaded', function () {
    console.log('Waste bin markers module loaded');

    if (window.googleMapsInterop) {
        Object.assign(window.googleMapsInterop, {
            calculateDistance: window.wasteBinMarkers.calculateDistance.bind(window.wasteBinMarkers),

            findNearestBins: window.wasteBinMarkers.findNearestBins.bind(window.wasteBinMarkers),

            getMarkerIcon: window.wasteBinMarkers.getMarkerIcon.bind(window.wasteBinMarkers),

            formatBinInfo: window.wasteBinMarkers.formatBinInfo.bind(window.wasteBinMarkers)
        });

        console.log('Waste bin markers methods integrated with googleMapsInterop');
    }
});

window.createCustomBinMarker = function (map, bin, position) {
    const fillLevel = bin.FillLevel || 0;

    let statusColor = '#6c757d'; // Default gray
    let fillColor = '#28a745'; // Default green

    switch (bin.DeviceStatus) {
        case 'Active': statusColor = '#28a745'; break; // green
        case 'Inactive': statusColor = '#6c757d'; break; // gray
        case 'Maintenance': statusColor = '#ffc107'; break; // yellow
        case 'Faulty': statusColor = '#dc3545'; break; // red
    }

    if (fillLevel >= 90) {
        fillColor = '#dc3545'; // red
    } else if (fillLevel >= 70) {
        fillColor = '#fd7e14'; // orange
    } else if (fillLevel >= 50) {
        fillColor = '#ffc107'; // yellow
    } else if (fillLevel >= 30) {
        fillColor = '#17a2b8'; // blue
    } else {
        fillColor = '#28a745'; // green
    }

    const customMarker = new google.maps.OverlayView();

    customMarker.onAdd = function () {
        const div = document.createElement('div');
        div.className = 'custom-bin-marker';
        div.style.position = 'absolute';
        div.style.width = '40px';
        div.style.height = '40px';
        div.style.backgroundColor = statusColor;
        div.style.borderRadius = '50%';
        div.style.border = '2px solid white';
        div.style.boxShadow = '0 2px 6px rgba(0,0,0,0.3)';
        div.style.cursor = 'pointer';
        div.style.zIndex = '1';
        div.style.display = 'flex';
        div.style.justifyContent = 'center';
        div.style.alignItems = 'center';

        const innerDiv = document.createElement('div');
        innerDiv.style.width = '30px';
        innerDiv.style.height = '30px';
        innerDiv.style.borderRadius = '50%';
        innerDiv.style.backgroundColor = 'white';
        innerDiv.style.display = 'flex';
        innerDiv.style.justifyContent = 'center';
        innerDiv.style.alignItems = 'center';

        const fillDiv = document.createElement('div');
        fillDiv.style.width = '24px';
        fillDiv.style.height = '24px';
        fillDiv.style.borderRadius = '50%';
        fillDiv.style.backgroundColor = fillColor;
        fillDiv.style.display = 'flex';
        fillDiv.style.justifyContent = 'center';
        fillDiv.style.alignItems = 'center';
        fillDiv.style.color = 'white';
        fillDiv.style.fontSize = '10px';
        fillDiv.style.fontWeight = 'bold';
        fillDiv.style.fontFamily = 'Arial, sans-serif';
        fillDiv.textContent = `${fillLevel}%`;

        innerDiv.appendChild(fillDiv);
        div.appendChild(innerDiv);

        div.addEventListener('mouseover', function () {
            div.style.transform = 'scale(1.1)';
            div.style.transition = 'transform 0.2s ease-in-out';
        });

        div.addEventListener('mouseout', function () {
            div.style.transform = 'scale(1)';
        });

        div.addEventListener('click', function () {
            if (window.googleMapsInterop && window.googleMapsInterop.infoWindow) {
                const infoContent = window.googleMapsInterop.createInfoWindowContent(bin);
                window.googleMapsInterop.infoWindow.setContent(infoContent);
                window.googleMapsInterop.infoWindow.setPosition(position);
                window.googleMapsInterop.infoWindow.open(map);

                if (window.googleMapsInterop.dotNetRef) {
                    window.googleMapsInterop.dotNetRef.invokeMethodAsync('OpenBinDetail', bin.WasteBinId);
                }
            }
        });

        this.div_ = div;
        const panes = this.getPanes();
        panes.overlayMouseTarget.appendChild(div);
    };

    customMarker.draw = function () {
        const overlayProjection = this.getProjection();
        const point = overlayProjection.fromLatLngToDivPixel(position);

        const div = this.div_;
        div.style.left = (point.x - 20) + 'px';
        div.style.top = (point.y - 20) + 'px';
    };

    customMarker.onRemove = function () {
        this.div_.parentNode.removeChild(this.div_);
        this.div_ = null;
    };

    customMarker.setMap(map);
    return customMarker;
};