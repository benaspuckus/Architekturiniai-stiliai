angular.module('app')
    .controller('BrowseItemsController', BrowseItemsController)
    .config(theming);

theming.$inject = ['$mdThemingProvider'];
function theming($mdThemingProvider) {
}


function BrowseItemsController($window, $http, $location, $rootScope, $mdDialog, $routeParams) {
    var vm = this;
    vm.currentItems = null;
    vm.currentPage = 0;
    vm.pageSize = 4;
    vm.currentPage = 0;
    vm.adminStatus = $rootScope.adminStatus;
    vm.currentCategoryId = $routeParams.categoryId;
    vm.goToEditCategories = goToEditCategories;
    vm.getShoppingCart = getShoppingCart;
    vm.getOrders = getOrders;
    vm.getCurrentItems = getCurrentItems;
    vm.getAllItems = getAllItems;
    vm.isSimpleUserLoggedIn = isSimpleUserLoggedIn;
    vm.getCategoriesForSearch = getCategoriesForSearch;
    vm.showSearch = showSearch;
    vm.addCartItem = addCartItem;
    vm.hideSearch = hideSearch;
    vm.confirmSearch = confirmSearch;
    vm.numberOfPages = numberOfPages;
    vm.goToItemInfo = goToItemInfo;
    vm.deleteCartItem = deleteCartItem;
    vm.getTotalSum = getTotalSum;
    vm.cart = null;
    vm.token = $window.localStorage.getItem('token');
    vm.config = { "headers": { "Authorization": "Bearer " + vm.token } }

    function showSearch(ev) {
        $mdDialog.show({
            controller: function () {
                return vm;
            },
            controllerAs: 'controller',
            templateUrl: 'searchItems.dialog.html',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true
        })
            .then(function (search) {
                if (!search) { return }

                var id = null;
                if (search.make) {
                    id = search.make.categoryId;
                }
                if (search.model) {
                    id = search.model.categoryId;
                }
                if (search.category) {
                    id = search.category.categoryId;
                }
                if (search.subCategory) {
                    id = search.subCategory.categoryId;
                }
                getCurrentItems(id);
                $location.path("/search/" + id);
            });
    };

    function addCartItem(item) {

        var localCart = JSON.parse($window.localStorage.getItem('cart'));

        if (!localCart) {
            localCart = [];
        }
        localCart.push(item);
        $window.localStorage.setItem('cart', JSON.stringify(localCart));
    }

    function getTotalSum() {
        var sum = 0;
        var localCart = JSON.parse($window.localStorage.getItem('cart'));
        if (localCart) {
            for (var i = 0; i < localCart.length; i++) {
                sum = localCart[i].price + sum;
            }
        }
        return sum;
    }

    function deleteCartItem(index) {

        var localCart = JSON.parse($window.localStorage.getItem('cart'));
        localCart.splice(index,1);

        $window.localStorage.setItem('cart', JSON.stringify(localCart));
        $window.location.reload();
    }

    function goToItemInfo(item) {
        $location.path("/items/" + vm.currentCategoryId + "/" + item.parentCategoryId + "/" + item.itemId);
    }

    function confirmSearch(search) {
        $mdDialog.hide(search);
    }

    function hideSearch() {
        $mdDialog.hide();
    };

    function goToEditCategories() {
        $location.path("/manageItems");
    }

    function isSimpleUserLoggedIn() {
        return ($rootScope.isUserLoggedIn && !$rootScope.adminStatus);
    }

    function getShoppingCart() {
        if (!isSimpleUserLoggedIn()) {
            $window.location.href = "/";
        }

        vm.cart = JSON.parse($window.localStorage.getItem('cart'));
    };

    function getOrders() {
        $http.get("https://localhost:44376/api/Cart", vm.config)
            .then(function (response) {
                vm.orders = response.data;
                console.log(response.data);
                vm.loading = false;
            }, function (response) {
                if (response.status === 401 || response.status === 403) {
                    $window.location.href = "/";
                }
                displayResponseMessage(response);
            });
    };

    function getCurrentItems(id) {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/GetItemsForSearch/" + id, vm.config)
            .then(function (response) {
                vm.loading = false;
                vm.currentItems = response.data;
            }, function (response) {
                displayResponseMessage(response);
            });
    };

    function getAllItems() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/GetAllItems", vm.config)
            .then(function (response) {
                vm.loading = false;
                vm.currentItems = response.data;
            }, function (response) {
                displayResponseMessage(response);
            });
    };

    function getCategoriesForSearch() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/GetCategoryNames", vm.config)
            .then(function (response) {
                vm.searchCategories = response.data;
                vm.loading = false;
            }, function (response) {
                displayResponseMessage(response);
            });
    };
    function numberOfPages() {
        return Math.ceil(vm.currentItems.length / vm.pageSize);
    }

    function displayResponseMessage(response) {

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