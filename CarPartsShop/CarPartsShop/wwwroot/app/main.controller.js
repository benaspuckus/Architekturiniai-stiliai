angular.module('app')
    .controller('MainController', MainController);

function MainController($window, $http, $location, $rootScope, $route) {
    var vm = this;
    vm.loggedUserName = null;
    vm.checkLoginStatus = checkLoginStatus;
    vm.logOut = logOut;
    vm.checkAdminStatus = checkAdminStatus;
    vm.goToShoppingCart = goToShoppingCart;
    vm.getCartSize = getCartSize;
    vm.token = $window.localStorage.getItem('token');
    vm.config = { "headers": { "Authorization": "Bearer " + vm.token } }

    function checkLoginStatus() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/Account/CheckStatus", vm.config)
            .then(function (response) {
                vm.loading = false;
                vm.loggedUserName = response.data;
                $rootScope.isUserLoggedIn = true;
            }, function (response) {
                vm.loggedUserName = null;
                $rootScope.isUserLoggedIn = false;
                displayResponseMessage(response);
            });
    };

    function getCartSize() {
        var numberAsString = $window.localStorage.getItem('cart');
        var number = JSON.parse(numberAsString);
            return number.length;
    }

    function goToShoppingCart() {
        $location.path("/cart");
    }

    function checkAdminStatus() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/Account/CheckAdminStatus", vm.config)
            .then(function (response) {
                vm.loading = false;
                $rootScope.adminStatus = true;
            }, function (response) {
                $rootScope.adminStatus = false;
                displayResponseMessage(response);
            });
    };

    function logOut() {
        vm.error = null;
        vm.loading = true;
        $window.localStorage.removeItem('token');
        $window.localStorage.removeItem('cart');
        checkLoginStatus();
        $rootScope.isUserLoggedIn = false;
        $rootScope.adminStatus = false;
        vm.loading = false;
        $window.location.href = "/";
    };

    function displayResponseMessage(response) {

        if (response.data.errors) {
            vm.error = response.data.errors.Name.join();
        }
        else if (typeof response.data === 'string' || response.data instanceof String) {
            vm.error = response.data;
        }
        else {
            var errorMessages = [];
            for (var key in response.data) {
                var value = response.data[key];
                var isArray = Array.isArray(value);
                if (!isArray) {
                    errorMessages.push(value);
                } else if (value.length) {
                    for (var message in value) {
                        errorMessages.push(value[message]);
                    }
                }
            }
            vm.error = errorMessages.join();
        }
        vm.loading = false;
    };
}