
// configure our routes
angular.module('app')
    .config(function ($routeProvider) {
    $routeProvider
        .when('/',
            {
                templateUrl: 'pages/browseItems.html',
                controller: 'BrowseItemsController'
            })
        .when('/:categoryId',
            {
                templateUrl: 'pages/browseItems.html',
                controller: 'BrowseItemsController'
            })
        .when('/:searchCategoryId/:categoryId/:itemId',
            {
                templateUrl: 'pages/viewItem.html',
                controller: 'ViewItemController'
            })
        .when('/manageItems',
            {
                templateUrl: 'pages/editCategories.html',
                controller: 'CategoriesController'
            })
        .when('/cart',
            {
                templateUrl: 'pages/shoppingCart.html',
                controller: 'BrowseItemsController'
            })
        .when('/manageItems/:id',
            {
                templateUrl: 'pages/editCategoryItems.html',
                controller: 'ItemsController'
            })
        .when('/manageItems/:id/:itemId',
            {
                templateUrl: 'pages/editItem.html',
                controller: 'SingleItemController'
            })
        .when('/register',
            {
                templateUrl: 'pages/register.html',
                controller: 'AccountController'
            })
        .when('/login',
            {
                templateUrl: 'pages/login.html',
                controller: 'AccountController'
            })
        .when('/aboutUs',
            {
                templateUrl: 'pages/aboutUs.html',
                controller: 'BrowseItemsController'
            })
        .otherwise('/');
});