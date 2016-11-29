app.controller("fileExplorerController", function ($scope, $http, fileExplorerService) {
    fileExplorerService.GetVolumes(function(fileExplorer){
            $scope.fileExplorer = fileExplorer;   
    });

    $scope.open = function (fileExplorer, currentPath) {
        $scope.loading = true;
        fileExplorerService.GetDirectory(fileExplorer, currentPath).then(function (fileExplorer) {
            $scope.loading = false;
            $scope.fileExplorer = fileExplorer;
        });
    }
});