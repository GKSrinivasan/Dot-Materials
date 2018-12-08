(function () {
    var myApp = angular.module("myApp", []);
})()

angular.module('myApp').controller('EmployeeDetailsController', function ($scope, $http, $window) {
    //$scope.AddUser = function (e) {$('#myUserModal').modal('show');}
    $scope.User.Location = $('#workLocation option:selected').text();
    $scope.User.UserRoll = $('#userRoll option:selected').text();
    $scope.User.Gender = $('#employeeGender option:selected').text();
    $scope.User.Department = $('#employeeDepartment option:selected').text();
    $scope.User.IDType = $('#employeeIDType option:selected').text();
    $scope.User.UserStatus = $('#employeeUserStatus option:selected').text();
    $scope.Employeedetails = function (e) {
        $http({
            method: "POST",
            url: '../UserDetail/Employeedetails',
            data: $scope.User
        }).then(function (d) {
        });
    }
    $scope.gender = [];
    $scope.iDType = [];
    $scope.department = [];
    $scope.userStatus = [];
    $scope.userRoll = [];
    $scope.location = null;
    $scope.fillList = function () {
        $http({
            method: 'POST',
            url: '../UserDetail/GetEmployeeCommonCodes',
        }).then(function (result) {
            for (var i = 0; i < result.data.length; i++) {
                if (result.data[i].CodeType == "GENDER")
                    $scope.gender.push(result.data[i].Code);
                else if (result.data[i].CodeType == "IDPROOFTYPE")
                    $scope.iDType.push(result.data[i].Code);
                else if (result.data[i].CodeType == "DEPARTMENT")
                    $scope.department.push(result.data[i].Code);
                else if (result.data[i].CodeType == "USERSTATUS")
                    $scope.userStatus.push(result.data[i].Code);
                else if (result.data[i].CodeType == "USERROLL")
                    $scope.userRoll.push(result.data[i].Code);
            }
        });
        $http({
            method: 'POST',
            url: '../UserDetail/GetCompanyLocation',
        }).then(function (result) {
            $scope.location = result.data;
        });
    }
    $scope.fillList();
    $scope.userChange = function (e) {
        if ($scope.User.IsUser) { $("#userControl1").show(); $("#userControl2").show(); $("#userControl3").show(); }
        else { $("#userControl1").hide(); $("#userControl2").hide(); $("#userControl3").hide(); }
    }
})

angular.module('myApp').controller('VendorDetailsController', function ($scope, $http, $window) {
    $scope.Vendor.Currency = $('#currency option:selected').text();
    $scope.Vendor.BusinessType = $('#businessType option:selected').text();
    $scope.Vendordetails = function (e) {
        $http({
            method: "POST",
            url: '../UserDetail/Vendordetails',
            data: $scope.Vendor
        }).then(function (d) {
        });
    }
    $scope.currency = null;
    $scope.businessType = null;
    $scope.fillList = function () {
        $http({
            method: 'POST',
            url: '../UserDetail/GetVendorCommonCodes',
        }).then(function (result) {
            $scope.businessType = result.data;
        });
        $http({
            method: 'POST',
            url: '../UserDetail/GetCurrency',
        }).then(function (result) {
            $scope.currency = result.data;
        });
    }
    $scope.fillList();
})

angular.module('myApp').controller('CustomerDetailsController', function ($scope, $http, $window) {
    $scope.Customerdetails = function (e) {
        $scope.Customer.Gender = $('#customerGender option:selected').text();
        $scope.Customer.IDType = $('#customerIdType option:selected').text();
        $scope.Customer.StoreLocation = $('#storelocation option:selected').text();
        $http({
            method: "POST",
            url: '../UserDetail/Customerdetails',
            data: $scope.Customer
        }).then(function (d) {
        });
    }
    $scope.gender = [];
    $scope.idType = [];
    $scope.storeLocation = null;
    $scope.fillList = function () {
        $http({
            method: 'POST',
            url: '../UserDetail/GetEmployeeCommonCodes',
        }).then(function (result) {
            for (var i = 0; i < result.data.length; i++) {
                if (result.data[i].CodeType == "GENDER")
                    $scope.gender.push(result.data[i].Code);
                else if (result.data[i].CodeType == "IDPROOFTYPE")
                    $scope.idType.push(result.data[i].Code);
            }
        });
        $http({
            method: 'POST',
            url: '../UserDetail/GetCompanyLocation',
        }).then(function (result) {
            $scope.storeLocation = result.data;
        });
    }
    $scope.fillList();
})

angular.module('myApp').controller('AccountGroupController', function ($scope, $http, $window) {
    $scope.group.Statement = $('#statement option:selected').text();
    $scope.AccountGroup = function (e) {
        $scope.group.Statement = $('#statement option:selected').text();
        $http({
            method: "POST",
            url: '../UserDetail/AccountGroup',
            data: $scope.group
        }).then(function (d) {
        });
    }
    $scope.statement = null;
    $scope.fillList = function () {
        $http({
            method: 'POST',
            url: '../UserDetail/GetGroupCommonCodes',
        }).then(function (result) {
            $scope.statement = result.data;
        });
    }
    $scope.fillList();
})