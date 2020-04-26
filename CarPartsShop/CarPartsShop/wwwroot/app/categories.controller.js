angular.module('app')
    .controller('CategoriesController', CategoriesController);

function CategoriesController($http, $location, $window) {
    var vm = this;

    vm.isCreateInitialCategoryPressed = false;
    vm.getCategories = getCategories;
    vm.addCategory = addCategory;
    vm.addInitialCategory = addInitialCategory;
    vm.expandCategory = expandCategory;
    vm.removeCategory = removeCategory;
    vm.goToManageItems = goToManageItems;
    vm.pressCreateInitialCategory = pressCreateInitialCategory;
    vm.categories = null;
    vm.token = $window.localStorage.getItem('token');
    vm.config = { "headers": { "Authorization": "Bearer " + vm.token} }

   

    function getCategories() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/GetCategories", vm.config)
            .then(function (response) {
                vm.categories = response.data;
                vm.loading = false;
            }, function (response) {
                displayResponseMessage(response);
            });
    };

    function addCategory(categoryName, category) {
        vm.error = null;
        vm.loading = true;
        var model = {name: categoryName, parentId: category.categoryId}
        $http.post("https://localhost:44376/api/AddCategory", model, vm.config)
            .then(function (response) {
                vm.loading = false;
                vm.categories = getCategories();
            }, function (response) {
                displayResponseMessage(response);
            });
    }

    function pressCreateInitialCategory() {
        vm.isCreateInitialCategoryPressed = !vm.isCreateInitialCategoryPressed;
    };

    function addInitialCategory(categoryName) {
        vm.error = null;
        vm.loading = true;
        var model = { name: categoryName}
        $http.post("https://localhost:44376/api/AddCategory", model, vm.config)
            .then(function (response) {
                vm.loading = false;
                vm.categories = getCategories();
            }, function (response) {
                displayResponseMessage(response);
            });
    }

    function removeCategory(categoryId) {
        vm.error = null;
        vm.loading = true;
        $http.delete("https://localhost:44376/api/RemoveCategory/" + categoryId, vm.config)
            .then(function (response) {
                vm.loading = false;
                vm.categories = getCategories();
            }, function (response) {
                displayResponseMessage(response);
            });
    }

    function expandCategory(id) {
        findNestedCategory(vm.categories, id, "childCategories");
    };

    function findNestedCategory(arr, itemId, nestingKey) {
        arr.reduce((a, item) => {
                if (a) return a;
            if (item.categoryId === itemId) {
                item.isExpanded = !item.isExpanded; return item;}
                if (item[nestingKey]) return findNestedCategory(item[nestingKey], itemId, nestingKey);
            },
            null);

    };

    function goToManageItems(categoryId) {
        $location.path("/manageItems/" + categoryId);
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
