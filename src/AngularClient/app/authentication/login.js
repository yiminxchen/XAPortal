(function () {
    'use strict';

    angular
        .module('app')
        .controller('Login', Login);

    Login.$inject = ['securityService'];

    function Login(securityService) {
        securityService.login();
    }

})();
