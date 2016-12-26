using ScottybonsMVC.Models.Entities;
using ScottybonsMVC.Models.ViewModels;
using System.Linq;
using System.Web.Mvc;
using ScottybonsMVC.Models.ViewModels.Customer;


namespace ScottybonsMVC.Controllers
{
    public class StyleProfileQaController : Controller
    {
        readonly ScottybonsEComEntities _scottybonsEComEntities = new ScottybonsEComEntities();

        // GET: StyleProfile
        public ActionResult Index()
        {
            var mymodel = new StyleProfileViewModel
            {
                ProfileQuestion = _scottybonsEComEntities.StyleProfileQuestions.ToList(),
                ProfileAnswers = _scottybonsEComEntities.StyleProfileAnswers.ToList(),
                AnswerTypeControls = _scottybonsEComEntities.AnswerTypeofControls.ToList()
            };
            return View(mymodel);
        }
    }
}