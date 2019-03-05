var app = angular.module('myApp', ['ngRoute', 'ngResource']);

app
    .config(['$routeProvider',
        function ($routeProvider) {
            $routeProvider
                .when('/start', { templateUrl: './venue/map', controller: 'SortController' })
                .otherwise({ redirectTo: '/start' });
        }])
    .controller('SortController', ['$scope', '$route', '$routeParams', '$location',
        function($scope, $route, $routeParams, $location) {
        }]);