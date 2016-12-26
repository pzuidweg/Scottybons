using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Resources.Scottybons;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace ScottybonsMVC.Models.ViewModels.Customer
{
    public class RegisterVm : RenderModel
    {
        public RegisterVm(IPublishedContent content, CultureInfo culture) : base(content, culture) { }

        public RegisterVm(IPublishedContent content) : base(content) { }

        public RegisterVm() : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent) { }

        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "FirstNameRequired")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterVM_First_Name_FirstName")]
        public string Firstname { get; set; }

        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "SurnameRequired")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterVM_SurName_SurName")]
        public string Surname { get; set; }

        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof (ScottybonsDataStrings), ErrorMessageResourceName = "RegisterVm_Emailaddress_The_EmailadresFieldIsNotValidEmailAddress")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterVM_Email_Email")]
        public string Emailaddress { get; set; }

       
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterVM_Language_Language")]
        public string Language { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterVm_RegisterHeader_CONFIRM_YOUR_REGISTRATION")]
        public string RegisterHeader { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterVm_RegistersubHeader_And_let_us_select_an_outfit_for_you__or_just_do_the_style_intake")]
        public string RegistersubHeader { get; set; }


        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "RegisterVm_Password_YourPassword")]
        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PasswordRequiredForRegistration")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterVM_Password_Password")]
        [StringLength(100, ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "RegisterViewModel1_Password_AtLeast8CharLong", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "EnterConfirmPassword")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterViewModel1_ConfirmPassword_Confirm_password")]
        [StringLength(100, ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "RegisterViewModel1_Password_AtLeast8CharLong", MinimumLength = 8)]
        [Compare("Password", ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PasswordsNotMatched")]
        public string ConfirmPassword { get; set; }


        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "EmailExisted")]
        public string EmailExisted { get; set; }


        //[Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PleaseAcceptTermsAndConditionsToCompleteTheRegistration")]

        [Range(typeof(bool), "true", "true", ErrorMessageResourceType = typeof (ScottybonsDataStrings), ErrorMessageResourceName = "PleaseAcceptTermsAndConditionsToCompleteTheRegistration")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterVM_Terms_and_Conditions_TermsandConditions")]
        public bool TermsAndConditions { get; set; }


        public bool Success { get; set; }
    }

    public class ResetPasswordViewModel : RenderModel
    {
        public ResetPasswordViewModel(IPublishedContent content, CultureInfo culture) : base(content, culture) { }

        public ResetPasswordViewModel(IPublishedContent content) : base(content) { }

        public ResetPasswordViewModel() : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent) { }

        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "RegisterVm_Password_YourPassword")]
        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "EnterPassword")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ResetPasswordViewModel_enterPassword_Enter_your_New_Password")]
        [StringLength(100, ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "RegisterViewModel1_Password_AtLeast8CharLong", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "EnterConfirmPassword")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ResetPasswordViewModel_ReenterPassword_Reenter_your_New_Password")]
        [StringLength(100, ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "RegisterViewModel1_Password_AtLeast8CharLong", MinimumLength = 8)]
        [Compare("Password", ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PasswordsNotMatched")]
        public string ConfirmPassword { get; set; }

       

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ResetPasswordViewModel_enterEmail_Enter_your_Email")]
        public string Email { get; set; }

        

    }
}