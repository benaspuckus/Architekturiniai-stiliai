angular.module('app')
    .controller('ItemsController', ItemsController);

function ItemsController($scope, $http, $routeParams, $location, $window) {
    var vm = this;
    vm.currentCategoryId = $routeParams.id;
    vm.isAddItemPressed = false;
    vm.imageSrc = "";
    vm.currentPage = 0;
    vm.pageSize = 4;
    vm.getItems = getItems;
    vm.addItem = addItem;
    vm.removeItem = removeItem;
    vm.expandAddItem = expandAddItem;
    vm.goToItemInfo = goToItemInfo;
    vm.numberOfPages = numberOfPages;
    vm.token = $window.localStorage.getItem('token');
    vm.config = { "headers": { "Authorization": "Bearer " + vm.token } }

    function getItems() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/GetItems/" + vm.currentCategoryId, vm.config)
            .then(function (response) {
                vm.items = response.data;
                vm.loading = false;
            }, function (response) {
                displayResponseMessage(response);
            });
    };

    function numberOfPages() {
        return Math.ceil(vm.items.length / vm.pageSize);
    }

    function addItem(item) {
        var file = $scope.myFile;
        if (file) {
            getBase64(file)
                .then(
                    data => {
                        var model = {
                            name: item.Name,
                            parentCategoryId: vm.currentCategoryId,
                            description: item.Description,
                            price: item.Price,
                            oemNumber: item.OemNumber,
                            partNumber: item.PartNumber,
                            imageData: data
                        };

                        $http.post("https://localhost:44376/api/AddCategoryItem", model, vm.config)
                            .then(function (response) {
                                vm.loading = false;
                                vm.categories = getItems();
                            },
                                function (response) {
                                    displayResponseMessage(response);
                                });
                    }
                );
        } else {
            var model = {
                name: item.Name,
                parentCategoryId: vm.currentCategoryId,
                description: item.Description,
                price: item.Price,
                oemNumber: item.OemNumber,
                partNumber: item.PartNumber,
            };

            $http.post("https://localhost:44376/api/AddCategoryItem", model, vm.config)
                .then(function (response) {
                    vm.loading = false;
                    vm.categories = getItems();
                },
                    function (response) {
                        displayResponseMessage(response);
                    });
        }
    };

    function removeItem(item) {
        vm.error = null;
        vm.loading = true;

        var model = { categoryId: item.parentCategoryId, itemId: item.itemId }
        $http.post("https://localhost:44376/api/RemoveCategoryItem", model, vm.config)
            .then(function (response) {
                vm.loading = false;
                vm.categories = getItems();
            }, function (response) {
                displayResponseMessage(response);
            });


    };

    function goToItemInfo(item) {
        console.log("test");
        $location.path("/manageItems/" + vm.currentCategoryId + "/" + item.itemId);
    }

    function getBase64(file) {
        return new Promise((resolve, reject) => {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result);
            reader.onerror = error => reject(error);
        });
    }

    function expandAddItem() {
        vm.isAddItemPressed = !vm.isAddItemPressed;
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