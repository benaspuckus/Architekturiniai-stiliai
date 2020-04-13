angular.module('app')
    .controller('MainController', MainController);

function MainController($window, $http, $location) {
    var vm = this;
    vm.loggedUserName = null;
    vm.checkLoginStatus = checkLoginStatus;
    vm.logOut = logOut;

    function checkLoginStatus() {
        vm.error = null;
        vm.loading = true;
        var token = $window.localStorage.getItem('token');
        $http({
            method: "GET",
            url: "https://localhost:44376/api/Account/CheckStatus",
            headers: {
                'Authorization': 'Bearer ' + token
            }
        }).then(function (response) {
            $location.path("/");
            console.log(response);
            vm.loading = false;
            vm.loggedUserName = response.data;
        }, function (response) {
            vm.loggedUserName = null;
            displayResponseMessage(response);
        });
    };

    function logOut() {
        vm.error = null;
        vm.loading = true;
        $window.localStorage.removeItem('token');
        checkLoginStatus();
        vm.loading = false;
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