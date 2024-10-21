(function () {
    'use strict';

    angular.module('umbraco.services')
        .factory('elementFinderService', function ($http) {
            var service = {};

            service.getElements = function () {
                return $http.get('/umbraco/ElementFinder/data/getElements')
                    .then(function (response) {
                        return response.data;
                    });
            };

            service.getElementUsedLocations = function (key, currentPage, totalPages) {
                return $http.post(`/umbraco/ElementFinder/data/GetLocationsOfElement?elementGuid=${key}&currentPage=${currentPage}&totalPages=${totalPages}`)
                    .then(function (response) {
                        return response.data;
                    });
            };

            return service;
        });
})();