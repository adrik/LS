angular.module('way-devices', ['way-services'])
    .directive('deviceList', ['$interval', 'web', function ($interval, webService) {
        return {
            restrict: 'E',
            scope: {
                //                devices: '=',
                //                makeTooltip: '='
            },
            templateUrl: '/home/devicetemplate',
            link: function (scope, element, attrs) {
                var ws = new webService();

                var all = function (method) {
                    return function (list) {
                        angular.forEach(list, method);
                    }
                };

                var add = function (x) {
                    var d = new Device(x, null, null);
                    scope.devices[d.id] = d;
                };
                var update = function (x) {
                    scope.devices[x.id].update(x);
                };

                ws.getUserInfo(all(add));
                ws.getContactsInfo(all(add));

                //                $.when(
                //                //ws.getUserInfo(addDevices),
                //                //ws.getContactsInfo(addDevices)
                //                ).done(function () {
                //                    var bounds = new google.maps.LatLngBounds();
                //                    for (i in devd) bounds.extend(devd[i].marker.getPosition());
                //                    //map.fitBounds(bounds);
                //                });

                var timeoutId = $interval(function () {
                    ws.getAllLocations(all(update));
                }, 1000);
                element.on('$destroy', function () {
                    $interval.cancel(timeoutId);
                });

                scope.devices = {};
                scope.tooltip = function (device) {
                    return device.uname;
                };
            }
        };
    } ]);