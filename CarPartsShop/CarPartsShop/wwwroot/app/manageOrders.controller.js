angular.module('app')
    .controller('ManageOrdersController', ManageOrdersController);

function ManageOrdersController($routeParams, $window, $http, $location) {
    var vm = this;
    vm.toggle = toggle;
    vm.getOrders = getOrders;
    vm.moveToProgress = moveToProgress;
    vm.moveToFinished = moveToFinished;
    vm.toggledItem = null;
    vm.token = $window.localStorage.getItem('token');
    vm.config = { "headers": { "Authorization": "Bearer " + vm.token } }

    function getOrders() {
        $http.get("https://localhost:44376/api/Cart/AdminOrders", vm.config)
            .then(function (response) {
                console.log(response.data);

                vm.requestedOrders = response.data.requestedOrders;
                vm.acceptedOrders = response.data.acceptedOrders;
                vm.finishedOrders = response.data.finishedOrders;

                vm.loading = false;
            }, function (response) {
                if (response.status === 401 || response.status === 403) {
                    $window.location.href = "/";
                }
                displayResponseMessage(response);
            });
    }

    function moveToFinished(cart) {
        var model = { cartId: cart.cartId, status: 2 };
        $http.post("https://localhost:44376/api/Cart/ChangeStatus", model, vm.config)
            .then(function (response) {
                vm.loading = false;
                $window.location.reload();
            }, function (response) {
                if (response.status === 401 || response.status === 403) {
                    $window.location.href = "/";
                }
                displayResponseMessage(response);
            });
    }
    function moveToProgress(cart) {
        var model = { cartId: cart.cartId, status: 1 };
        $http.post("https://localhost:44376/api/Cart/ChangeStatus",model, vm.config)
            .then(function (response) {
                vm.loading = false;
                $window.location.reload();
            }, function (response) {
                if (response.status === 401 || response.status === 403) {
                    $window.location.href = "/";
                }
                displayResponseMessage(response);
            });
    }
  
    function toggle(index) {
        if (vm.toggledItem == index) {
            vm.toggledItem = null;
        } else {
            vm.toggledItem = index;
        }
    }

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