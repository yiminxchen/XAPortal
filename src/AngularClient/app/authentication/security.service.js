(function () {
    'use strict';

    angular
        .module('app')
        .factory('securityService', securityService);

    securityService.$inject = ['$http', '$q', '$rootScope', '$window', '$state', 'localStorageService'];

    function securityService($http, $q, $rootScope, $window, $state, localStorageService) {
        // store in $rootScope is not good, page refresh will kill it
        $rootScope.isAuthenticated = false;

        var service = {
            clearAuthenticationData: clearAuthenticationData,
            getProfile: getProfile,
            login: login,
            logoff: logoff
        }

        return service;

        function clearAuthenticationData() {
            localStorageService.set('accessToken', '');
            localStorageService.set('idToken', '');
            $rootScope.isAuthenticated = false;
        }

        function getProfile() {
            var profile = {
                status: 'Not Authenticated',
                email: '',
                roles: []
            };

            // userinfo Endpoint example
            // GET /connect/userinfo
            return $http.get('http://localhost:5000/connect/userinfo')
                .then(handleComplete, function () {
                    return profile;
                })
                .catch(function (message) {
                    console.log('/connect/userinfo failed: ' + message);
                    return profile;
                });

            function handleComplete(response) {
                profile.status = 'Authenticated';
                JSON.parse(JSON.stringify(response.data), function (key, value) {
                    if (key === 'email') {
                        profile.email = value;
                    }
                    if (key === 'role') {
                        profile.roles.push(value);
                    }
                });

                return profile;
            }
        }

        function login() {
            clearAuthenticationData();

            if ($window.location.hash) {
                authenticateCallback();
            }
            else {
                authenticate();
            }
        }

        function logoff() {
            // endsession Endpoint example
            // GET /connect/endsession?
            // id_token_hint=id_token&
            // post_logout_redirect_uri=https://myapp/callback&
            // state=abc&
            var id_token = localStorageService.get('idToken');     
            var authorizationUrl = 'http://localhost:5000/connect/endsession';
            var id_token_hint = id_token;
            var post_logout_redirect_uri = 'http://localhost:5004';
            var state = Date.now() + '' + Math.random();

            var url =
                authorizationUrl + '?' +
                'id_token_hint=' + id_token_hint + '&' +
                'post_logout_redirect_uri=' + encodeURI(post_logout_redirect_uri) + '&' +
                'state=' + encodeURI(state);

            clearAuthenticationData();
            $window.location = url;
        }

        function setAuthenticationData(token, id_token) {
            localStorageService.set('accessToken', token);
            localStorageService.set('idToken', id_token);
            $rootScope.isAuthenticated = true;
        }

        function urlBase64Decode(str) {
            var output = str.replace('-', '+').replace('_', '/');
            switch (output.length % 4) {
                case 0:
                    break;
                case 2:
                    output += '==';
                    break;
                case 3:
                    output += '=';
                    break;
                default:
                    throw 'Illegal base64url string!';
            }
            return window.atob(output);
        }

        function getPayloadFromToken(token) {
            var data = {};
            if (typeof token !== 'undefined') {
                var encoded = token.split('.')[1];
                if (typeof encoded !== 'undefined') {
                    data = JSON.parse(urlBase64Decode(encoded));
                    console.log('data = ' + data);
                }
            }
            return data;
        }

        function authenticate() {
            // Authorize Endpoint example
            // GET /connect/authorize?
            // client_id=client1&
            // scope=openid email api1&
            // response_type=id_token token&
            // redirect_uri=https://myapp/callback&
            // state=abc&
            // nonce=xyz
            var authorizationUrl = 'http://localhost:5000/connect/authorize';
            var client_id = 'angularPortal';
            var redirect_uri = 'http://localhost:5004/login';
            var response_type = 'id_token token';
            var scope = 'webdatascope openid profile';
            var nonce = 'N' + Math.random() + '' + Date.now();
            var state = Date.now() + '' + Math.random();

            localStorageService.set('authNonce', nonce);
            localStorageService.set('authState', state);

            var url =
                authorizationUrl + '?' +
                'response_type=' + encodeURI(response_type) +'&' +
                'client_id=' + encodeURI(client_id) + '&' +
                'redirect_uri=' + encodeURI(redirect_uri) + '&' +
                'scope=' + encodeURI(scope) + '&' +
                'nonce=' + encodeURI(nonce) + '&' +
                'state=' + encodeURI(state);

            console.log("authorization request url is " + url);

            $window.location = url;
        }

        function authenticateCallback() {
            var hash = window.location.hash.substr(1);

            var result = hash.split('&').reduce(function (result, item) {
                var parts = item.split('=');
                result[parts[0]] = parts[1];
                return result;
            }, {});

            var token = '';
            var id_token = '';

            if (!result.error) {

                if (result.state === localStorageService.get('authState')) {
                    token = result.access_token;
                    id_token = result.id_token;

                    console.log('idToken = ' + id_token);
                    console.log('accessToken = ' + token);

                    var dataIdToken = getPayloadFromToken(id_token);

                    if (dataIdToken.nonce === localStorageService.get('authNonce')) {
                        setAuthenticationData(token, id_token);
                        $state.go('authenticated');
                        return;
                    }
                }
            }
            clearAuthenticationData();
            $state.go('home');
        }
    }
})();