angular.module('app')
    .directive('demoFileModel', function ($parse) {
        return {
            restrict: 'A', 

            link: function (scope, element, attrs) {
                var model = $parse(attrs.demoFileModel),
                    modelSetter = model.assign; //define a setter for demoFileModel

                //Bind change event on the element
                element.bind('change',
                    function () {
                        //Call apply on scope, it checks for value changes and reflect them on UI
                        scope.$apply(function () {
                            //set the model value
                            modelSetter(scope, element[0].files[0]);
                        });
                    });
            }
        };
    })
    .directive('ngConfirmClick', [
        function () {
            return {
                link: function (scope, element, attr) {
                    var msg = attr.ngConfirmClick || "Are you sure?";
                    var clickAction = attr.confirmedClick;
                    element.bind('click', function (event) {
                        if (window.confirm(msg)) {
                            scope.$eval(clickAction)
                        }
                    });
                }
            };
        }]);