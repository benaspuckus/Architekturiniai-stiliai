angular.module('app')
    .controller('BrowseItemsController', BrowseItemsController);

function BrowseItemsController($window, $http, $location, $rootScope) {
    var vm = this;
    vm.goToEditCategories = goToEditCategories;
    vm.adminStatus = false;
    vm.test = false;
    vm.checkAdminStatus = checkAdminStatus;
    vm.getShoppingCart = getShoppingCart;
    vm.goToShoppingCart = goToShoppingCart;
    vm.isSimpleUserLoggedIn = isSimpleUserLoggedIn;
    vm.token = $window.localStorage.getItem('token');
    vm.config = { "headers": { "Authorization": "Bearer " + vm.token } }

    function goToEditCategories() {
        $location.path("/manageItems");
    }

    function checkAdminStatus() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/Account/CheckAdminStatus", vm.config)
            .then(function (response) {
                vm.loading = false;
                vm.adminStatus = true;
            }, function (response) {
                vm.adminStatus = false;
                displayResponseMessage(response);
            });
    };

    function isSimpleUserLoggedIn() {
        return ($rootScope.isUserLoggedIn && !vm.adminStatus);
    }

    function goToShoppingCart() {
        $location.path("/cart");
    }

    function getShoppingCart() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/Cart", vm.config)
            .then(function (response) {
                vm.loading = false;
            }, function (response) {
                displayResponseMessage(response);
            });
    };
    function displayResponseMessage(response) {
        if (response.status === 401 || response.status === 403) {
            $location.path("/");
        }

        if (response.data.errors) {
            vm.error = response.data.errors.Name.join();
        }
        else {
            vm.error = response.data;
        }
        vm.loading = false;
    };
}