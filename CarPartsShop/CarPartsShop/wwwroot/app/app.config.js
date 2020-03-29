﻿
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
        .otherwise('/');
});