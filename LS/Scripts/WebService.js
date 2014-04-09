angular.module('way-services', [])
    .factory('web', ['$http', function ($http) {
        WebServiceInvoker = function (url) {
            this.url = url;
        };
        WebServiceInvoker.prototype = {
            invokeMethod: function (method, data, callback) {
                var promise = $http({
                    method: 'GET',
                    url: this.url + '/' + method,
                    data: data,
                    responseType: 'json'
                });
                promise.success(function (data) {
                    if (callback) {
                        callback(data);
                    }
                });
                promise.error(this._invocationFailed);
                return promise;
            },
            _invocationFailed: function (data, status) {
                console.log('Request failed with code: ' + status);
            }
        };

        var WebService = function () {
            this.ws = new WebServiceInvoker('/Services/Web.svc');
        };
        WebService.prototype = {
            getLocation: function (deviceId, callback) {
                return this.ws.invokeMethod('GetLocation', { deviceId: deviceId }, callback);
            },
            setLocation: function (deviceId, x, y, callback) {
                return this.ws.invokeMethod('SetLocation', { deviceId: deviceId, x: x, y: y }, callback);
            },
            getUserInfo: function (callback) {
                return this.ws.invokeMethod('GetUserInfo', {}, callback);
            },
            getContactsInfo: function (callback) {
                return this.ws.invokeMethod('GetContactsInfo', {}, callback);
            },
            getAllLocations: function (callback) {
                return this.ws.invokeMethod('GetAllLocations', {}, callback);
            },
            getMyLocations: function (callback) {
                return this.ws.invokeMethod('GetMyLocations', {}, callback);
            }
        };

        return WebService;
    } ]);
