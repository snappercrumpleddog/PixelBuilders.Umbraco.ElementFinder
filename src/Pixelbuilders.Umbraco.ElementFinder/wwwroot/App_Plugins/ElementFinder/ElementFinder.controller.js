(function () {
    "use strict";

    function ElementFinderController($scope, elementFinderService, editorService) {

        var vm = this;
        vm.elementResults = [];
        vm.page = {};
        vm.currentPage = 1;
        vm.totalPages = 1;
        vm.itemsPerPage = 20;
        vm.totalItems = 0;
        vm.selectedBlock = "";


        // register functions.
        vm.submit = submit;

        elementFinderService.getElements().then(function (data) {
            if (data && data.length > 0) {
                var select = document.getElementById("js-block-stat-elements");
                for (let i = 0; i < data.length; i++) {
                    var element = data[i];
                    var option = document.createElement('option');
                    option.value = element.key;
                    option.innerHTML = element.name;
                    select.appendChild(option);
                }
            }
        });

        function submit() {
            var select = document.getElementById("js-block-stat-elements");
            vm.selectedBlock = select.value;
            vm.currentPage = 1;
            elementFinderService.getElementUsedLocations(select.value, vm.currentPage, vm.itemsPerPage).then(function (data) {
                vm.elementResults = data.pages;
                vm.totalPages = data.pageCount;
                vm.totalItems = data.totalCount;
            });
        }

        vm.openBlocklistDrawer = function (pageId) {
            var editor = {
                id: pageId,
                close: function () {
                    // Handle the close event if necessary
                    editorService.close();
                },
                submit: function () {
                    editorService.close();
                }
            };

            // Open the drawer
            editorService.contentEditor(editor);
        };

        // Clean up when the controller is destroyed
        $scope.$on('$destroy', function () {
            editorService.close();
        });
        
        vm.prevPage = function () {
            console.log("next page");
            if (vm.currentPage > 1) {
                vm.currentPage--;
                elementFinderService.getElementUsedLocations(vm.selectedBlock, vm.currentPage, vm.itemsPerPage).then(function (data) {
                    vm.elementResults = data.pages;
                    vm.totalPages = data.pageCount;
                    vm.totalItems = data.totalCount;
                });
            }
        };

        vm.nextPage = function () {
            console.log("next page");
            if (vm.currentPage < vm.totalPages) {
                vm.currentPage++;
                elementFinderService.getElementUsedLocations(vm.selectedBlock, vm.currentPage, vm.itemsPerPage).then(function (data) {
                    vm.elementResults = data.pages;
                    vm.totalPages = data.pageCount;
                    vm.totalItems = data.totalCount;
                });
            }
        };

        vm.setPage = function (pageNumber) {
            vm.currentPage = pageNumber;

            elementFinderService.getElementUsedLocations(vm.selectedBlock, vm.currentPage, vm.itemsPerPage).then(function (data) {
                vm.elementResults = data.pages;
                vm.totalPages = data.pageCount;
            });
        };

    }

    angular.module("umbraco").controller("ElementFinder.controller", ElementFinderController);
})();