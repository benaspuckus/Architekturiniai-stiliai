﻿angular.module('app')
    .controller('BrowseItemsController', BrowseItemsController)
    .config(theming);

theming.$inject = ['$mdThemingProvider'];
function theming($mdThemingProvider) {
}


function BrowseItemsController($window, $http, $location, $rootScope, $mdDialog, $routeParams, $scope) {
    var vm = this;
    vm.currentItems = null;
    vm.currentPage = 0;
    vm.pageSize = 6;
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
    vm.confirmPayment = confirmPayment;
    vm.showSearch = showSearch;
    vm.addItemPrice = addItemPrice;
    vm.minusItemPrice = minusItemPrice;
    vm.addCartItem = addCartItem;
    vm.goToOrders = goToOrders;
    vm.hideSearch = hideSearch;
    vm.confirmSearch = confirmSearch;
    vm.numberOfPages = numberOfPages;
    vm.goToItemInfo = goToItemInfo;
    vm.deleteCartItem = deleteCartItem;
    vm.getTotalSum = getTotalSum;
    vm.cart = null;
    vm.toggle = toggle;
    vm.toggledItem = null;
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
        if (item.count) {
            for (var i = 0; i < item.count; i++) {
                localCart.push(item);
            }
        }
        else {
            localCart.push(item);
        }
        
        $window.localStorage.setItem('cart', JSON.stringify(localCart));
        $window.location.reload();
    }

    function addItemPrice(item) {
        if (item.count) {
            item.count ++;
        } else {
            item.count = 2;
        }
    }

    function minusItemPrice(item) {
        if (item.count && item.count >1) {
            item.count--;
        }
    }

    
    function toggle(index) {
        if (vm.toggledItem == index) {
            vm.toggledItem = null;
        } else {
            vm.toggledItem = index;
        }
    }

    function goToOrders() {
        $location.path("/manageOrders");
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

        var cart = JSON.parse($window.localStorage.getItem('cart'));
        var cartList = [];
        var counts = {};

       
            cart.forEach(function (x) {
                var key = x.itemId;
                counts[key] = (counts[key] || 0) + 1;
            });

        const entries = Object.entries(counts);

        for (const [item, count] of entries) {

            var singleItem = cart.find(x => x.itemId === item);
            singleItem.count = count;
            cartList.push(singleItem);
        }

        vm.cart = cartList;
    };

    function getOrders() {
        $http.get("https://localhost:44376/api/Cart/Orders", vm.config)
            .then(function (response) {
                vm.orders = response.data;
                vm.loading = false;
            }, function (response) {
                if (response.status === 401 || response.status === 403) {
                    $window.location.href = "/";
                }
                displayResponseMessage(response);
            });
    };

    function confirmPayment(ev) {
        var boolValue = ($scope.delivery == "true");
        var model = { items: JSON.parse($window.localStorage.getItem('cart')), needsDelivery: boolValue, deliveryAddress: $scope.address }
        $http.post("https://localhost:44376/api/Cart/Confirm", model, vm.config)
            .then(function (response) {
                vm.loading = false;
                var id = response.data;
                var slicedId = id.slice(31, 36);
                console.log(id);
                vm.confirmedCart = { id: slicedId, sum: getTotalSum()};
                $window.localStorage.removeItem('cart');
                $mdDialog.show({
                    controller: function() {
                        return vm;
                    },
                    controllerAs: 'controller',
                    templateUrl: 'confirmPayment.dialog.html',
                    parent: angular.element(document.body),
                    targetEvent: ev,
                    clickOutsideToClose: false
                }).then(function () {
                    $window.location.href = "/";
                });;
            }, function (response) {
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