angular.module('app')
    .controller('AccountController', AccountController);


function AccountController($location, $http, $window) {
    var vm = this;
    vm.createAccount = createAccount;
    vm.login = login;


    function createAccount(account) {
        vm.error = null;
        vm.loading = true;
        var model = { email: account.Email, password: account.Password, confirmPassword: account.Confirm }
        $http.post("https://localhost:44376/api/Account/Register", model)
            .then(function (response) {
                $window.localStorage.setItem('token', response.data);
                vm.loading = false;
                $window.location.reload();
            }, function (response) {
                displayResponseMessage(response);
            });
        $location.path("/");
    };

    function login(account) {
        vm.error = null;
        vm.loading = true;
        var model = { email: account.Email, password: account.Password}
        $http.post("https://localhost:44376/api/Account/Login", model)
            .then(function (response) {
                $window.localStorage.setItem('token', response.data);
                vm.loading = false;
               $window.location.reload();
            }, function (response) {
                displayResponseMessage(response);
            });

        $location.path("/");

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