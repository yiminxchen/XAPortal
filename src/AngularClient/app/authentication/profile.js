(function () {
    'use strict';

    angular
        .module('app')
        .controller('Profile', Profile);

    Profile.$inject = ['$scope', 'profile'];

    function Profile($scope, profile) {
        /* jshint validthis:true */
        $scope.profile = profile;
    }
})();
