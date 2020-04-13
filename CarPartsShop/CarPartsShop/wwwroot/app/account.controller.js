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
                console.log(response.data);
                $location.path("/");
                vm.loading = false;
                $window.location.reload();
            }, function (response) {
                displayResponseMessage(response);
            });
    };

    function login(account) {
        vm.error = null;
        vm.loading = true;
        var model = { email: account.Email, password: account.Password}
        $http.post("https://localhost:44376/api/Account/Login", model)
            .then(function (response) {
                console.log(response);
                $window.localStorage.setItem('token', response.data);
                $location.path("/");
                vm.loading = false;
                $window.location.reload();
            }, function (response) {
                console.log(response);
                displayResponseMessage(response);
            });
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