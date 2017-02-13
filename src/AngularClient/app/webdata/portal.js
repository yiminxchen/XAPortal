(function () {
    'use strict';

    angular
        .module('app')
        .controller('PortalAdmin', PortalAdmin);

    PortalAdmin.$inject = ['$scope', 'webdata'];

    function PortalAdmin($scope, webdata) {
        /* jshint validthis:true */
        $scope.webdata = webdata;
    }
})();
