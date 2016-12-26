using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Resources.Scottybons;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace ScottybonsMVC.Models.ViewModels.Customer
{
    //public class LoginViewModel : PostRedirectModel
    //{
    //    public LoginViewModel()
    //    {
    //        //Needs to populate thsese values from Database/Resource Files
    //        AccountTypes = new List<AccountType>
    //        {
    //            new AccountType
    //            {
    //                Id = "1",
    //                Type = "No, i'm making a new account",
    //                Checked = "checked"
    //            },
    //            new AccountType
    //            {
    //                Id = "2",
    //                Type = "Yes, my password is",
    //                Checked = string.Empty
    //            }
    //        };
    //    }

    //    //public IHtmlString BodyText { get; set; }


    //    //[Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "EmailNotRegistred")]
    //    //[Display(ResourceType = typeof(ScottybonsDataStrings), Name = "LoginViewModel_EmailAddress_What_is_your_e_mail_address_")]
    //    //public string EmailAddress { get; set; }

    //    //[Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "EnterPassword")]
    //    //[Display(ResourceType = typeof(ScottybonsDataStrings), Name = "LoginViewModel_Password_Do_you_have_a_Scottybons_password_")]
    //    //public string Password { get; set; }

    //    [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "LoginViewModel_AccountType_select_type")]
    //    public int AccountType { get; set; }

    //    public List<AccountType> AccountTypes { get; set; }

    //    //public bool Status { get; set; }
    //}

    public class LoginVm : RenderModel
    {
        public LoginVm(IPublishedContent content, CultureInfo culture)
            : base(content, culture)
        {
        }
        public LoginVm(IPublishedContent content)
            : base(content)
        {
        }
        public LoginVm() : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent) { }


        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "EmailLogin")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "LoginViewModel_EmailAddress_What_is_your_e_mail_address_")]
        [EmailAddress(ErrorMessageResourceType = typeof (ScottybonsDataStrings), ErrorMessageResourceName = "EmailLogin")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "LoginViewModel_Password_Do_you_have_a_Scottybons_password_")]

        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "EnterPassword")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "PasswordInvalid")]
        public string ErrorMessage { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "LoginVm_loginheader_YOUR_SCOTTYBONS_ACCOUNT")]
        public string Loginheader { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "LoginVm_loginsubheader_Log_in_and_manage_your_information__your_styleprofile_and_your_orders")]
        public string Loginsubheader { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "LoginVm_Register_REGISTER")]
        public string Register { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "LoginVm_Forgotpasssword_Forgot_your_Password_")]
        public string Forgotpasssword { get; set; }

        public bool Status { get; set; }
    }
}