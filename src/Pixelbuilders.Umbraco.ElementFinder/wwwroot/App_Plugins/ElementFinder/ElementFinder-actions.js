angular.module("umbraco").run([
    "$compile",
    "appState",
    "eventsService",
    "$http",
    function ($compile, appState, eventsService, $http) {
        eventsService.on("app.ready", function (e, args) {
        });
    }
]);