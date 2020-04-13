angular.module('app')
    .controller('MainController', MainController);

function MainController($window, $http, $location, $rootScope) {
    var vm = this;
    vm.loggedUserName = null;
    vm.checkLoginStatus = checkLoginStatus;
    vm.logOut = logOut;
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

    function logOut() {
        vm.error = null;
        vm.loading = true;
        $window.localStorage.removeItem('token');
        checkLoginStatus();
        $rootScope.isUserLoggedIn = false;
        vm.loading = false;
        $window.location.reload();
        $location.path("/");
    };

    function displayResponseMessage(response) {

        if (response.data.errors) {
            vm.error = response.data.errors.Name.join();
        }
        else {
            vm.error = response.data;
        }
        vm.loading = false;
    };
}