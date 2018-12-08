(function () {
    var myApp = angular.module("myApp", []);
})()

angular.module('myApp').controller('AccountController', function ($scope, $http) {
    $scope.login = function (e) {
        debugger;
        $http({

            method: "POST",

            url: '../Account/Login',

            data: $scope.logUser

        }).then(function (d) {
        });
    }
})
