angular.module('app')
    .controller('SingleItemController', SingleItemController);

function SingleItemController($http, $routeParams, $scope, $window, $location) {
    var vm = this;
    vm.currentCategoryId = $routeParams.id;
    vm.currentItemId = $routeParams.itemId;
    vm.getItemInfo = getItemInfo;
    vm.pressModify = pressModify;
    vm.saveItem = saveItem;
    vm.item = null;
    vm.isModifyPressed = false;

    vm.token = $window.localStorage.getItem('token');
    vm.config = { "headers": { "Authorization": "Bearer " + vm.token } }

    function getItemInfo() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/GetItems/" + vm.currentCategoryId + "/" + vm.currentItemId, vm.config)
            .then(function (response) {
                vm.testItem = response.data;
                $scope.fetchedItem = response.data;
                vm.item = response.data;
                vm.loading = false;
            }, function (response) {
                displayResponseMessage(response);
            });
    };

    function saveItem(item) {
        vm.error = null;
        vm.loading = true;
        var model = { name: item.Name, price: item.Price, description: item.Description, parentCategoryId: vm.currentCategoryId, itemId: vm.currentItemId, oemNumber: item.Oem, partNumber: item.Part}
        console.log(model);
        $http.put("https://localhost:44376/api/UpdateCategoryItem", model, vm.config)
            .then(function (response) {
                vm.loading = false;
                vm.categories = getItemInfo();
                vm.isModifyPressed = false;
            }, function (response) {
                displayResponseMessage(response);
            });
    };

    function pressModify() {
        vm.isModifyPressed = !vm.isModifyPressed;
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