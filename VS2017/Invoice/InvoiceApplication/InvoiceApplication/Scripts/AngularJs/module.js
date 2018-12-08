
(function  ()  {
        var  myApp  =  angular.module("myApp", []);
})()

angular.module('myApp').controller('SignUpController', function ($scope, $http) {
    $scope.checkNumbers = function (e) {
        if ((e.keyCode >= 48 && e.keyCode <= 57) || e.keyCode == 8 || e.keyCode == 9 || e.keyCode == 13 || e.keyCode == 37 || e.keyCode == 39 || e.keyCode ==46 ) { return true; }
        else { e.preventDefault(); return false; }
    }
    $scope.emailChange = function (e) { $("#emailError").show(); }
    $scope.signup = function (e) {
        $http({

            method: "POST",

            url: '/SignUp/SignUp',

            data: $scope.user

        }).then(function (d) {
            if (d.data == "Tenant Exists")
                $("#errorMsg").show();
            else {
                $("#errorMsg").hide();
                $("#emailError").hide();
                $scope.user = null;
                $http.post('http://localhost:50139/api/tenant/AddNewTenant/').then(function (e) { })
            }
        });
    }
})
         