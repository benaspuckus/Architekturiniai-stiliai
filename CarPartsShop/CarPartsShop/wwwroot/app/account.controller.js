angular.module('app')
    .controller('AccountController', AccountController);


function AccountController($location, $http, $window, $rootScope) {
    var vm = this;
    vm.createAccount = createAccount;
    vm.login = login;


    function createAccount(account) {
        vm.error = null;
        vm.loading = true;
        $window.localStorage.removeItem('token');
        $window.localStorage.removeItem('cart');
        var model = { email: account.Email, password: account.Password, confirmPassword: account.Confirm }
        $http.post("https://localhost:44376/api/Account/Register", model)
            .then(function (response) {
                $window.localStorage.setItem('token', response.data);
                vm.loading = false;
                $rootScope.shouldReload = true;
                $window.location.href = "/";

            }, function (response) {
                console.log(response);
                displayResponseMessage(response);
            });
    };

    function login(account) {
        vm.error = null;
        vm.loading = true;
        $window.localStorage.removeItem('token');
        $window.localStorage.removeItem('cart');
        var model = { email: account.Email, password: account.Password }
        $http.post("https://localhost:44376/api/Account/Login", model)
            .then(function (response) {
                $window.localStorage.setItem('token', response.data);
                vm.loading = false;
                $window.location.href = "/";
            }, function (response) {
                displayResponseMessage(response);
            });

    };

    function displayResponseMessage(response) {
        if (response.status === 401 || response.status === 403) {
            $window.location.href = "/";
        }

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