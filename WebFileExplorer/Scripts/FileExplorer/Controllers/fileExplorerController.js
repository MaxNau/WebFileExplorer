app.controller("fileExplorerController", function ($scope, $http) {
    $http.get('/api/fileexplorerapi/getvolumesrequest').
        success(function (data) {
            debugger;
            $scope.fileExplorer = data;
        }).
        error(function (data) {
            alert("sa");
        });
    debugger;
    $scope.open = function (fe, curPath) {
        debugger;
        fe.CurrentPath = curPath;
        $http.post('/api/fileexplorerapi/getcurrentdirectoryrequest', fe).
            success(function (data) {
                debugger;
                $scope.fileExplorer = data;
            })
    };
});