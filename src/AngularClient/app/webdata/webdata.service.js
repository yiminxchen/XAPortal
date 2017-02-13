(function () {
    'use strict';

    angular
        .module('app')
        .factory('webDataService', webDataService);

    webDataService.$inject = ['$http', '$q', 'securityService'];

    function webDataService($http, $q, securityService) {
        var service = {
            getPublic: getPublic,
            getClient: getClient,
            getPortal: getPortal
        };

        return service;

        function getPublic() {
            return $http.get('http://localhost:5001/Public')
                .then(handleComplete, handleError)
                .catch(function (message) {
                    console.err('/public failed: ' + message);
                    return '';
                });
        }

        function getClient() {
            return $http.get('http://localhost:5001/Client')
                .then(handleComplete, handleError)
                .catch(function (message) {
                    console.log('/Client failed: ' + message);
                    return '';
                });
        }

        function getPortal() {
            return $http.get('http://localhost:5001/Portal')
                .then(handleComplete, handleError)
                .catch(function (message, status) {
                    console.log('/Portal failed: ' + message);
                    return message;
                });
        }

        function handleComplete(response) {
            return response.data;
        }

        function handleError(response) {
            console.log('/Portal error status: ' + response.status);
            // unauthorized
            if (response.status === 401) {
                securityService.clearAuthenticationData();
                return 'You are not authenticated, please login first.';
            }
            // forbidden - user is authenticated, but does not have enough privilege
            if (response.status === 403) {
                return 'You do not have enough privilege to access.';
            }
            return 'Error status code: ' + response.status;
        }
    }
})();