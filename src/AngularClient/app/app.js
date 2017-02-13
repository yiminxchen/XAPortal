(function () {
    'use strict';

    var app = angular.module('app', [
        'ui.router',
        'LocalStorageModule',
        'angularSpinner'
    ]);

    app.$inject = ['$rootScope'];

    app.run(main);

    function main($rootScope) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams, options) {
            console.log('from ' + fromState.url + ' to ' + toState.url);
        });

        $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            console.log('from ' + fromState.url + ' to ' + toState.url);
        });

        $rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, error) {
            console.log(event);
            console.log(toState);
            console.log(toParams);
            console.log(fromState);
            console.log(fromParams);
            console.log(error);
        });

        $rootScope.$on('$stateNotFound', function (event, unfoundState, fromState, fromParams) {
            console.log(event);
            console.log(unfoundState);
            console.log(fromState);
            console.log(fromParams);
        });
    }
})();