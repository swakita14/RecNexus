var app = angular.module('myApp');
var sortController = function ($scope) {
    $scope.init = function(venues) {
        $scope.venues = venues;
    }

    var geocoder


    function showOnMap(address) {
        geocoder.geocode({ 'address': address },
            function(results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    map.setCenter(results[0].geometry.location);
                    var marker = new google.maps.Marker({
                        map: map,
                        position: results[0].geometry.location
                    });
                } else {
                    $scope.error = true;
                    $scope.info = 'Geocode was not successful for the following reason: ' + status;
                }
            });
    }

    function getCompleteAddress(venue) {
        return venue.Address1 + ' ' + venue.Address2 + ' ' + venue.City + ', ' + venue.State;
    }

    $http.get('/api/venues/')
        .success(function (data) {
            $scope.venues = data;
            geocoder = new google.maps.Geocoder();
            map = new google.maps.Map($('.map-canvas')[0], {
                zoom: 8,
                center: getMyLocation()
            });
            $scope.index = 0;
            var venue = $scope.venues[$scope.index];
            if (typeof venue !== 'undefined') {
                showOnMap(getCompleteAddress(venue));
            }
        });


  

    function getDistanceFromLatLonInKm(lat1, lon1, lat2, lon2) {
        var R = 6878; // Radius of the earth in km
        var dLat = deg2rad(lat2 - lat1);  // deg2rad below
        var dLon = deg2rad(lon2 - lon1);
        var a =
                Math.sin(dLat / 2) * Math.sin(dLat / 2) +
                    Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
                    Math.sin(dLon / 2) * Math.sin(dLon / 2)
            ;
        var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        var d = R * c; // Distance in km
        return d;
    };
    function deg2rad(deg) {
        return deg * (Math.PI / 180)
    };

    $scope.distance = function(venue) {
        return parseFloat(getDistanceFromLatLonInKm())
    }

    $scope.showMe = function () {
        var venue = $scope.venue
        if (typeof venue !== 'undefined') {
            showOnMap(geCompleteAddress(venue))
        }

    }


    function getMyLocation(){
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                $scope.$apply(function () {
                    $scope.position = position;
                });
            });
        }
    }
   
   
};

app.controller('SortController', ['$scope', sortController]);

