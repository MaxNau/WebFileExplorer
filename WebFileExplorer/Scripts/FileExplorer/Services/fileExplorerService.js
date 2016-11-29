app.factory("fileExplorerService", function($http){
    return {
        GetVolumes: function (fileExplorer) {
            $http.get('/api/fileexplorerapi/getvolumesrequest').
            success(fileExplorer).
            error(function () {
            });
        },

        GetDirectory: function (fileExplorer, currentPath) {
            fileExplorer.CurrentPath = currentPath;
            return $http.post('/api/fileexplorerapi/getcurrentdirectoryrequest', fileExplorer).then(function (response) {
                    return response.data;
            });
        }
    }
})