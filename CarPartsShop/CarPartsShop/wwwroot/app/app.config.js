
// configure our routes
angular.module('app')
    .config(function ($routeProvider) {
    $routeProvider
        .when('/',
            {
                templateUrl: 'pages/viewCategories.html',
                controller: 'CategoriesController'
            })
        .when('/manageItems/:id',
            {
                templateUrl: 'pages/manageItems.html',
                controller: 'ItemsController'
            })
        .when('/manageItems/:id/:itemId',
            {
                templateUrl: 'pages/viewItem.html',
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
        .otherwise('/');
});