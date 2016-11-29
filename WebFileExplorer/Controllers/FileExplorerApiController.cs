using System.Web.Http;
using WebFileExplorer.Models;

namespace WebFileExplorer.Controllers
{
    public class FileExplorerApiController : ApiController
    {
        public FileExplorer GetVolumesRequest()
        {
            FileExplorer fileExplorer = new FileExplorer();
            return fileExplorer.GetVolumes();
        }

        [HttpPost]
        public FileExplorer GetCurrentDirectoryRequest(FileExplorer fileExplorer)
        {
            if (fileExplorer.CurrentPath == null)
            {
                return GetVolumesRequest();
            }

            fileExplorer.Volumes = null;

            fileExplorer.GetCurrentDirectory();

            return fileExplorer;

        }
    }
}
