using System.Net.Mime;
using System.Web.Mvc;
using ScottybonsMVC.Models.Export;

namespace ScottybonsMVC.Controllers
{
    public class ExportController : Controller
    {
        public ActionResult Index()
        {
            return View(new ExportViewModel());
        }

        [HttpPost]
        public ActionResult Export(ExportViewModel model)
        {
            var cd = new ContentDisposition
            {
                FileName = "StyleIntake.csv",
                Inline = false
            };
            Response.AddHeader("Content-Disposition", cd.ToString());
            return Content(model.Csv, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}