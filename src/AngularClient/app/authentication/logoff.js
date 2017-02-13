(function () {
    'use strict';

    angular
        .module('app')
        .controller('Logoff', Logoff);

    Logoff.$inject = ['securityService'];

    function Logoff(securityService) {
        securityService.logoff();
    }
})();
