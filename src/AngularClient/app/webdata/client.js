(function () {
    'use strict';

    angular
        .module('app')
        .controller('ClientAdmin', ClientAdmin);

    ClientAdmin.$inject = ['$scope', 'webdata'];

    function ClientAdmin($scope, webdata) {
        /* jshint validthis:true */
        $scope.webdata = webdata;
    }
})();
