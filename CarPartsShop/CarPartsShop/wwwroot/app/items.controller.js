angular.module('app')
    .controller('ItemsController', ItemsController);

function ItemsController($http) {
    var vm = this;

    vm.getCategories = getCategories;

    function getCategories() {
        vm.error = null;
        vm.loading = true;
        $http.get("https://localhost:44376/api/GetCategories")
            .then(function (response) {
                vm.myData = response;
                vm.loading = false;
                console.log(response);
            }, function (response) {
                displayResponseMessage(response);
            });
    };

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