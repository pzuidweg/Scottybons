using System.Linq;
using System.Text;
using System.Web.Mvc;
using ScottybonsMVC.Infrastructure;
using ScottybonsMVC.Models.Entities;
using ScottybonsMVC.Models.ViewModels;
using ScottybonsMVC.Models.ViewModels.Customer;
using Umbraco.Web.Mvc;
using ScottybonsMVC.AppConstants;

namespace ScottybonsMVC.Controllers
{
    public class HomeController : SurfaceController
    {

        readonly ScottybonsEComEntities _scottybonsEComEntities;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scottybonsEComEntities"></param>
        public HomeController(ScottybonsEComEntities scottybonsEComEntities)
        {
            _scottybonsEComEntities = scottybonsEComEntities;
            
        }

        public HomeController()
        {
            if (ReferenceEquals(_scottybonsEComEntities, null))
            {
                _scottybonsEComEntities = new ScottybonsEComEntities();
            }
        }

        // GET: Home
        public ActionResult Index()
        {
            return CurrentUmbracoPage(); 
        }

        /// <summary>
        /// Contact Us
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ContactUs(ContactUsVm pageModel)
        {
            if (ModelState.IsValid)
            {
                var model = pageModel.ContactViewModel;
               
                //Populate User Email String
                var sb = new StringBuilder();
                sb.Append("<b>" + Resources.Scottybons.ScottybonsDataStrings.ContactUsViewModel_Topic_Topic + "</b>");
                sb.Append(": " + model.Topic + "<br>");
                sb.Append("<b>" + Resources.Scottybons.ScottybonsDataStrings.ContactUsViewModel_Message_Message + "</b>");
                sb.Append(": " + model.Message + "<br>");
                sb.Append("<b>" + Resources.Scottybons.ScottybonsDataStrings.ContactUsViewModel_Name_Name + "</b>");
                sb.Append(": " + model.Name + "<br>");
                sb.Append("<b>" + Resources.Scottybons.ScottybonsDataStrings.ContactUsViewModel_EmailAddress_Email_Address + "</b>");
                sb.Append(": " + model.EmailAddress + "<br>");
               
                sb.Append("<br>");

                var emailServices = new EmailServices(_scottybonsEComEntities);
                emailServices.SendEmailForSupportAdmin((int)EmailReason.ContactUsdAdmin, sb.ToString(), model.EmailAddress);

                return Redirect(GlobalConstants.LanguageUrl + "/contactusconfirm");
            }
            // If we got this far, something failed, redisplay form
            return CurrentUmbracoPage();
        }

        /// <summary>
        /// Get: Contact Us
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContactUs()
        { 
            return CurrentUmbracoPage();
        }
    }
}