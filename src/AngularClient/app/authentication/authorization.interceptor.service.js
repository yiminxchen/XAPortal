(function () {
    'use strict';

    angular
        .module('app')
        .service('authorizationInterceptor', authorizationInterceptor);

    authorizationInterceptor.$inject = ['$q', 'localStorageService'];

    angular.module('app').config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('authorizationInterceptor');
    }]);

    function authorizationInterceptor($q, localStorageService) {

        console.log('authorizationInterceptor created');

        var service = {
            request: request,
        }

        return service;

        function request(requestSuccess) {
            console.log("accessToken = " + localStorageService.get('accessToken'));
            requestSuccess.headers = requestSuccess.headers || {};

            if (localStorageService.get('accessToken') !== '') {
                requestSuccess.headers.Authorization = 'Bearer ' + localStorageService.get('accessToken');
            }

            return requestSuccess || $q.when(requestSuccess);
        };
    }
})();