angular.module('way-devices', ['way-services'])
    .directive('deviceList', ['$rootScope', '$interval', 'web', '$q', function ($root, $interval, webService, $q) {
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
                    var device = new Device(x, null, null);
                    scope.devices[device.id] = device;
                    $root.$broadcast('device-add', device);
                };
                var update = function (x) {
                    var device = scope.devices[x.id];
                    if (device) {
                        if (device.update(x)) {
                            $root.$broadcast('device-update', device);
                        }
                    } else {
                        console.log('device not found in the list: ', x);
                    }
                };



                $q.all([
                    //ws.getUserInfo(all(add)),
                    ws.getMasterContactsInfo('', all(add))
                ]).then(function () {
                    $root.$broadcast('device-focus-all');
                });

//                                ws.getMyLocations(function (list) {
//                                    $root.$broadcast('device-path', list);
//                                });

                var timeoutId = $interval(function () {
                    //ws.getAllLocations(all(update));
                }, 30000);
                element.on('$destroy', function () {
                    $interval.cancel(timeoutId);
                });

                scope.devices = {};
                scope.tooltip = function (device) {
                    return device.uname;
                };
                scope.focus = function (device) {
                    if (!device.hidden) {
                        $root.$broadcast('device-focus', device);
                    }
                };
                scope.toggle = function (device) {
                    device.hidden = !device.hidden;
                    $root.$broadcast('device-toggle', device);
                };
            }
        };
    } ]);