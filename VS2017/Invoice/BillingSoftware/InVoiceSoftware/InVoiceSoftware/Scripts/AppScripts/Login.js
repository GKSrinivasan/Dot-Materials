(function () {
    var myApp = angular.module("myApp", []);
})()

angular.module('myApp').controller('AccountController', function ($scope, $http, $window) {
    $scope.login = function (e) {
        $http({
            method: "POST",
            url: '../Account/Login',
            data: $scope.logUser
        }).then(function (d) {
            if (d.data == "Success") {
                $window.location.href = '../Dashboard/Dashboard';
                }
            else { $("#errorMsg").show(); $("#errorMsg")[0].innerText = d.data; }
        });
    }
    $scope.forgotPwd = function (e) {
        if ($scope.logUser.userID == undefined)
            alert("Please provide UserID");
    }
})
