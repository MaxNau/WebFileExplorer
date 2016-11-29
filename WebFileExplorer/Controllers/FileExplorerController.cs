using System.Web.Mvc;

namespace WebFileExplorer.Controllers
{
    public class FileExplorerController : Controller
    {
        // GET: FileExplorer
        public ActionResult Index()
        {
            var filePath = Request.QueryString["path"];
            if (filePath != null)
            {
                try
                {
                    // read the file
                    var file = System.IO.File.ReadAllBytes(filePath);
                    Microsoft.Win32.RegistryKey classes = Microsoft.Win32.Registry.ClassesRoot;

                    // find the sub key based on the file extension
                    Microsoft.Win32.RegistryKey fileClass = classes.OpenSubKey(System.IO.Path.GetExtension(filePath));
                    string contentType = fileClass.GetValue("Content Type").ToString();
                    return File(file, contentType);
                }
                catch (System.Exception ex) { }
            }

            return View();
        }
    }
}