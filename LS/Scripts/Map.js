angular.module('way-map', [])
    .directive('mapElement', ['$window', function ($window) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var win = angular.element($window);

                var myOptions = {
                    zoom: 15,
                    center: new google.maps.LatLng(37.767471, -122.431077),
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                var map = new google.maps.Map(element[0], myOptions);
                var infowindow = new google.maps.InfoWindow();

                var setElementHeight = function () {
                    element.height(win.height());
                };

                setElementHeight();
                win.bind('resize', setElementHeight);
            }
        };
    } ]);
