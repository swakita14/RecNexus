var app = angular.module('myApp');
var mainController = function($scope) {
    $scope.name = 'hello';
};

app.controller('MainController',['$scope', mainController]);