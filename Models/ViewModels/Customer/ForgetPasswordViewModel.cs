using System.ComponentModel.DataAnnotations;
using System.Web;
using Resources.Scottybons;
using ScottybonsMVC.Models.BaseModels;
using Umbraco.Web.Models;

namespace ScottybonsMVC.Models.ViewModels.Customer
{

    #region Forgot Password

    public class ForgotViewModel
    {
        [Required]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterViewModel1_Email_Email")]
        public string Email { get; set; }
    }


    public class ForgotPasswordPageModel : BasePageViewModel
    {
        public IHtmlString BodyText { get; set; }
        public ForgotPwdViewModel ForgotPasswordViewModel { get; set; }
    }

    public class ForgotPwdViewModel : PostRedirectModel
    {
        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "EmailNotRegistred")]
        [EmailAddress(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "RegisterVm_Emailaddress_The_EmailadresFieldIsNotValidEmailAddress")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterViewModel1_Email_Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ForgotPwdViewModel_ResetYourPassword_Reset_Your_Password_")]
        public string ResetYourPassword { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ForgotPwdViewModel_EmailandPassword_Email_and_Password_")]
        public string EmailAndPassword { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "WhatisYourEmailAdderss")]
        public string EmailAddress { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ForgotPwdViewModel_Paragraph_Please_check_your_Email_to_reset_your_password")]
        public string Paragraph { get; set; }

    }

    public class ForgetPasswordConfirmationModel : PostRedirectModel
    {
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ForgotPasswordConfirmationModel_Forget_Password_Confirmation_ForgetPasswordConFirmation")]
        public string ForgotPasswordConfirmation { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ForgetPasswordConfirmationModel_Somethingwentwrong_Oops__Something_went_wrong__Please")]
        public string Somethingwentwrong { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ForgetPasswordConfirmationModel_clickhere_Click_here")]
        public string clickhere { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ForgetPasswordConfirmationModel_Tonaviagateforgotpassword_to_navigate_forgot_password_screen_")]
        public string Tonaviagateforgotpassword { get; set; }
    }

    #endregion

    #region Reset Password
    public class ResetPasswordConfirmationModel : PostRedirectModel
    {
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ResetPasswordConfirmationModel_Reset_Password_Confirmation_ResetPasswordConFirmation")]
        public string ResetPasswordConfirmation { get; set; }
    }

    //public class ResetPasswordPageModel : BasePageViewModel
    //{
    //    public IHtmlString BodyText { get; set; }
    //    public ResetPasswordViewModel ResetPasswordViewModel { get; set; }
    //}



    #endregion

    #region Change Password
    public class ChangePasswordModel : PostRedirectModel
    {
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "Change_Password")]
        public string NewPasswordLabel { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "Change_Password_Confirm")]
        public string ConfirmNewPasswordLabel { get; set; }


        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PasswordInvalidFormat")]
        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "ChangePassword_Required")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "Change_Password")]
        [StringLength(100, ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "RegisterViewModel1_Password_AtLeast8CharLong", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "Confirm_ChangePassword_Required")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "Change_Password_Confirm")]
        //[StringLength(100, ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "RegisterViewModel1_Password_AtLeast8CharLong", MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PasswordsNotMatched")]
        public string ConfirmNewPassword { get; set; }

        public string Message { get; set; }
        public bool Status { get; set; }
    }
    #endregion
}