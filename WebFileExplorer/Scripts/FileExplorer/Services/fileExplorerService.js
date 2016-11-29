app.factory("fileExplorerService", function($http){
    return {
        GetVolumes: function (fileExplorer) {
            debugger;
            $http.get('/api/fileexplorerapi/getvolumesrequest').
            success(fileExplorer).
            error(function () {
                alert("sa");
            });
        },

        GetDirectory: function (fileExplorer, currentPath) {
            debugger;
            fileExplorer.CurrentPath = currentPath;
            return $http.post('/api/fileexplorerapi/getcurrentdirectoryrequest', fileExplorer).then(function (response) {
                    return response.data;
            });
        }
    }
})