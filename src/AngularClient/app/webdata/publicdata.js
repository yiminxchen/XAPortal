(function () {
    'use strict';

    angular
        .module('app')
        .controller('PublicData', PublicData);

    PublicData.$inject = ['$scope', 'webdata'];

    function PublicData($scope, webdata) {
        /* jshint validthis:true */
        $scope.webdata = webdata;
    }
})();
