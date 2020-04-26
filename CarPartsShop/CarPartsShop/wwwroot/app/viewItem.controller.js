angular.module('app')
    .controller('ViewItemController', ViewItemController);

function ViewItemController($routeParams, $window, $http, $location) {
    var vm = this;
    vm.searchCategoryId = $routeParams.searchCategoryId;
    vm.categoryId = $routeParams.categoryId;
    vm.currentItemId = $routeParams.itemId;
    vm.getItemInfo = getItemInfo;
    vm.goBack = goBack;
    vm.token = $window.localStorage.getItem('token');
    vm.config = { "headers": { "Authorization": "Bearer " + vm.token } }

    function getItemInfo() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/GetItems/" + vm.categoryId + "/" + vm.currentItemId, vm.config)
            .then(function (response) {
                vm.item = response.data;
                vm.loading = false;
            }, function (response) {
                displayResponseMessage(response);
            });
    };

    function goBack() {
        var id = vm.searchCategoryId;
        if (vm.searchCategoryId === 'undefined') {
            id = "";
        }

        $location.path("/" + id);
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