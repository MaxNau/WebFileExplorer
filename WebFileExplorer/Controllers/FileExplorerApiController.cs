using System.IO;
using System.Linq;
using System.Web.Http;
using WebFileExplorer.Models;

namespace WebFileExplorer.Controllers
{
    public class FileExplorerApiController : ApiController
    {
        public FileExplorer GetVolumesRequest()
        {
            return GetVolumes();
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

        private FileExplorer GetVolumes()
        {
            FileExplorer fileExplorer = new FileExplorer();
            fileExplorer.Volumes = (from drive in DriveInfo.GetDrives()
                      select new Volume(drive.Name, drive.Name)).ToList();

            return fileExplorer;
        }
    }
}
