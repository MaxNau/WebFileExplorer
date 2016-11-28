using System.Web.Mvc;

namespace WebFileExplorer.Controllers
{
    public class FileExplorerController : Controller
    {
        // GET: FileExplorer
        public ActionResult Index()
        {
            return View();
        }
    }
}