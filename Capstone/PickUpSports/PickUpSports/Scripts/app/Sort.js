(function ($, angular, window, undefined) {

    angular.module('myApp', [])
        .controller('SortController', ['$scope', '$filter',
            function ($scope, $filter) {

                $scope.init = function (venues) {
                    $scope.venues = venues;
                    console.log(venues);

//                    angular.forEach(venues, function() {
//                        var 
//                    }


                    var options = {
                        enableHighAccuracy: true,
                        timeout: 5000,
                        maximumAge: 0
                    };

                    function success(pos) {
                        var crd = pos.coords;

                       
                    }
                    console.log('Your current position is:');
                    console.log(`Latitude : ${crd.latitude}`);
                    console.log(`Longitude: ${crd.longitude}`);
                    console.log(`More or less ${crd.accuracy} meters.`);

                    function error(err) {
                        console.warn(`ERROR(${err.code}): ${err.message}`);
                    }

                    navigator.geolocation.getCurrentPosition(success, error, options);

                }

                

                //



            }])
})(jQuery, angular, window);