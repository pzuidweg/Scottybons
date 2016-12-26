using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web;
using Resources.Scottybons;
using ScottybonsMVC.Models.BaseModels;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace ScottybonsMVC.Models.ViewModels.Customer
{
    public class ContactUsPageModel : BasePageViewModel
    {
        public IHtmlString BodyText { get; set; }
        public ContactUsViewModel ContactViewModel { get; set; }
    }

    public class ContactUsViewModel : PostRedirectModel
    {

        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "TopicRequired")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ContactUsViewModel_Topic_Topic")]
        public string Topic { get; set; }
        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "QuestionRequired")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ContactUsViewModel_Message_Message")]
        public string Message { get; set; }
        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "NameRequired")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ContactUsViewModel_Name_Name")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "EmailInvalid")]

        [EmailAddress(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "RegisterVm_Emailaddress_The_EmailadresFieldIsNotValidEmailAddress")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ContactUsViewModel_EmailAddress_Email_Address")]
        public string EmailAddress { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ContactUsViewModel_Status_Status")]
        public string Status { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ContactUsViewModel_Contact_CONTACT")]
        public string Contact { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ContactUsViewModel_Remarkorquestion_Do_you_have_a_remark_or_question_")]
        public string Remarkorquestion { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ContactUsViewModel_Sendmessage_Send_us_a_message_")]
        public string Sendmessage { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ContactUsViewModel_Callus__Or_call_us_on_number__31_85_7500_490")]
        public string Callus { get; set; }

        [Display(ResourceType = typeof (ScottybonsDataStrings), Name = "ContactUsViewModel_CallusContactNumber")]
        public string CallusContactNumber { get; set; }
        
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ContactUsViewModel_Loginsuccess__Login_Success_")]
        public string Loginsuccess { get; set; }
    }

    public class ContactUsVm : RenderModel
    {
        public ContactUsVm(IPublishedContent content, CultureInfo culture)
            : base(content, culture)
        {
        }

        public ContactUsVm(IPublishedContent content)
            : base(content)
        {
        }

        public ContactUsVm()
            : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent)
        {
        }
        public ContactUsViewModel ContactViewModel { get; set; }

    }
}