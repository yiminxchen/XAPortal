(function () {
    'use strict';

    angular.module('app').config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];

    function config($stateProvider, $urlRouterProvider, $locationProvider) {
        //$urlRouterProvider.otherwise('/');
        $urlRouterProvider.otherwise(function ($injector, $location) {
            $location.path('/');
        });

        $stateProvider
            .state('home', { url: '/', templateUrl: 'template/home/home.html' })
            .state('login', {
                url: '/login',
                templateUrl: 'template/authentication/login.html',
                controller: 'Login'
            })
            .state("logoff", { url: "/logoff", templateUrl: "template/authentication/logoff.html", controller: "Logoff" })
            .state('authenticated', {
                url: '/authenticated',
                templateUrl: 'template/authentication/authenticated.html',
                controller: 'Profile',
                resolve: {
                    securityService: 'securityService',
                    profile: function (securityService) {
                        return securityService.getProfile();
                    }
                }
            })
            .state('public', {
                url: '/public',
                templateUrl: 'template/webdata/webdata.html',
                controller: 'PublicData',
                resolve: {
                    webservice: 'webDataService',
                    webdata: function (webservice) {
                        return webservice.getPublic();
                    }
                }
            })
            .state('client', {
                url: '/client',
                templateUrl: 'template/webdata/webdata.html',
                controller: 'ClientAdmin',
                resolve: {
                    webservice: 'webDataService',
                    webdata: function (webservice) {
                        return webservice.getClient();
                    }
                }
            })
            .state('portal', {
                url: '/portal',
                templateUrl: 'template/webdata/webdata.html',
                controller: 'PortalAdmin',
                resolve: {
                    webservice: 'webDataService',
                    webdata: function (webservice) {
                        return webservice.getPortal();
                    }
                }
            });

        $locationProvider.html5Mode(true);
    }
})();