angular.module('way-map', [])
    .directive('mapElement', ['$window', function ($window) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var win = angular.element($window);
                var optimalZoom = 15;

                var myOptions = {
                    zoom: optimalZoom,
                    center: new google.maps.LatLng(37.767471, -122.431077),
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                var map = new google.maps.Map(element[0], myOptions);
                var infoWindow = new google.maps.InfoWindow();

                var setElementHeight = function () {
                    element.height(win.height());
                };

                setElementHeight();
                win.bind('resize', setElementHeight);

                var markers = {};
                var markerTemplate = '<div class="marker"><div class="face"></div><div class="arrow"></div></div>';

                scope.$on('device-focus-all', function (e) {
                    console.log('focusing-all');
                    var bounds = new google.maps.LatLngBounds();

                    for (deviceId in markers) {
                        if (markers.hasOwnProperty(deviceId)) {
                            bounds.extend(markers[deviceId].getPosition());
                            //console.log(markers[deviceId].getPosition());
                        }
                    }

                    map.fitBounds(bounds);
                    if (map.getZoom() > optimalZoom)
                        map.setZoom(optimalZoom);
                });
                scope.$on('device-focus', function (e, device) {
                    console.log('focusing ', device);
                    var marker = markers[device.id];
                    focus(marker, true);
                });
                scope.$on('device-toggle', function (e, device) {
                    console.log('toggling ', device);
                    var marker = markers[device.id];
                    if (device.hidden)
                        marker.setMap(null);
                    else {
                        marker.setMap(map);
                        //focus(marker);
                        showInfo(device);
                    }
                });
                scope.$on('device-update', function (e, device) {
                    console.log('updating ', device);
                    var marker = markers[device.id];
                    marker.setPosition(new google.maps.LatLng(device.lat, device.lng));
                });
                scope.$on('device-add', function (e, device) {
                    console.log('adding ', device);
                    var marker = new google.maps.Marker({
                        map: map,
                        icon: '/thumbnail/load/' + device.uid,
                        position: new google.maps.LatLng(device.lat, device.lng),
                        title: device.uname
                    });
                    google.maps.event.addListener(marker, 'click', function () {
                        showInfo(device);
                    });
                    markers[device.id] = marker;
                });


                scope.$on('device-path', function (e, dots) {
                    console.log('making path');
                    var pathDots = [];

                    for (var i = 0; i < dots.length; i++) {
                        pathDots.push(new google.maps.LatLng(dots[i].lat, dots[i].lng));
                    }

                    var path = new google.maps.Polyline({
                        path: pathDots,
                        strokeColor: "#FF0000",
                        strokeOpacity: 1.0,
                        strokeWeight: 2
                    });

                    path.setMap(map);
                    console.log('made path');

                });


                var topMarker, topIndex;

                var focus = function (marker, zoom) {
                    if (topMarker) {
                        topMarker.setZIndex(topIndex);
                    }
                    topMarker = marker;
                    topIndex = marker.getZIndex();
                    marker.setZIndex(99999);

                    map.panTo(marker.getPosition());
                    if (zoom) {
                        map.setZoom(optimalZoom);
                    }
                };

                var showInfo = function (device) {
                    var lat = Math.round(device.lat * 10000) / 10000;
                    var lng = Math.round(device.lng * 10000) / 10000;

                    var mapURL = 'http://maps.google.com/maps?z=' + map.getZoom() + '&q=' + device.lat + '+' + device.lng;

                    var content = '<b>' + device.uname + '</b> is at: ' + lat + ', ' + lng;
                    var a = '<a class="mapsLink" target="_blank" href="' + mapURL + '">';
                    var img = '<img src="/images/map_icon.jpg" alt="Link to Google Maps" title="Make a link to Google Maps" />';
                    content = content + a + img + '</a>';

                    infoWindow.setContent(content);
                    infoWindow.open(map, markers[device.id]);
                };
            }
        };
    } ]);
