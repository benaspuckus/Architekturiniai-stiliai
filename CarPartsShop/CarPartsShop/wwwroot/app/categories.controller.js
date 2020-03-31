angular.module('app')
    .controller('CategoriesController', CategoriesController);

function CategoriesController($http, $location) {
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

    function getCategories() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/GetCategories")
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
        $http.post("https://localhost:44376/api/AddCategory", model)
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
        $http.post("https://localhost:44376/api/AddCategory", model)
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
        $http.delete("https://localhost:44376/api/RemoveCategory/" + categoryId)
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
        if (response.data.errors) {
            vm.error = response.data.errors.Name.join();
        }
        else {
            vm.error = response.data;
        }
        vm.loading = false;
    };

}