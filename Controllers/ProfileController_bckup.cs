using System.Linq;
using System.Web;
using System.Web.Security;
using Resources.Scottybons;
using ScottybonsMVC.AppConstants;
using ScottybonsMVC.Infrastructure;
using ScottybonsMVC.Models.Entities;
using System;
using System.Web.Mvc;
using ScottybonsMVC.Models.ViewModels;
using ScottybonsMVC.Models.ViewModels.Customer;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace ScottybonsMVC.Controllers
{

    public class ProfileController : SurfaceController
    {
        readonly ScottybonsEComEntities _scottybonsEComEntities;
        const string ROLE_NAME = "Member";
        private const string MEMBERS_LOGIN_URL = "/login/";
        const string ACCOUNT_PROFILEQUESTIONS = "/Account/ProfileQuestions/";
        const string ACCOUNT_STYLEPROFILE = "/account/styleprofile/";
        const string ACCOUNT_MYACCOUNT = "/account/myaccount/";
        const string ACCOUNT_FORGETPASSWORDCONFIRMATION = "/account/forgetpasswordconfirmation/";
        const string ACCOUNT_RESETPASSWORD_RESETGUID = "/Account/resetpassword?resetGUID=";
        const string RESETPASSWORDCONFIRMATION = "/ResetPasswordConfirmation/";

        #region Constructors

        public ProfileController()
        {
            if (ReferenceEquals(_scottybonsEComEntities, null))
            {
                _scottybonsEComEntities = new ScottybonsEComEntities();
            }


        }
        #endregion

        #region Action Methods
        [HttpPost]
        public ActionResult Register(RegisterVm model)
        {
            ViewBag.ConfirmPassword = model.ConfirmPassword;
            ViewBag.Password = model.Password;
            if (ModelState.IsValid)
            {
                ViewBag.EmailExisted =
                    Resources.Scottybons.ScottybonsDataStrings.Success;
                var customer = new Customer
                {
                    Email = model.Emailaddress,
                    FirstName = model.Firstname,
                    LastName = model.Surname,
                    Password = "Stored In Member Table",
                    PrePosition = "position",
                    SaltKey = 1,
                    LocaleSetting = model.Language,
                    RegisterDate = DateTime.UtcNow ,
                    UpdatedOn = DateTime.UtcNow,
                    CreatedOn = DateTime.UtcNow,
                    Active = 1
                };

                try
                {
                    //var memberService = Services.MemberService;

                    var memberService = this.UmbracoContext.Application.Services.MemberService;
                    if (null != memberService.GetByEmail(model.Emailaddress))
                    {
                        ModelState.AddModelError("",
                            ScottybonsDataStrings.EmailExisted);
                        ViewBag.EmailExisted = Resources.Scottybons.ScottybonsDataStrings.EmailExisted;
                        //"Email is already in use";
                        return CurrentUmbracoPage();
                    }


                    var memberGroupService = this.UmbracoContext.Application.Services.MemberGroupService;

                    var newMember = memberService.CreateMemberWithIdentity(model.Emailaddress, model.Emailaddress, model.Firstname, ROLE_NAME);

                    newMember.IsApproved = true;

                    memberService.SavePassword(newMember, model.Password);

                    //Save to Member
                    memberService.Save(newMember);


                    // create a GUID for the email validation
                    string newUserGuid = System.Guid.NewGuid().ToString("N");

                    #region "For Conrimation Email : Phase 2"

                    //// create the member using the MemberService
                    //var member = memberService.CreateMember(model.Email, model.Email, model.FirstName + " " + model.LastName, "Member");

                    //// set custom variables and set to NOT approved - careful with the alias case on your custom
                    //// properties! e.g. firstName 

                    //// Set custom properties - we should check our custom properties exist first
                    //// if (member.HasProperty("firstname"))  - we'll let it bomb instead for learning / setup.
                    //member.SetValue("firstname", model.FirstName);
                    //member.SetValue("lastname", model.LastName);
                    //member.SetValue("validateguid", newUserGuid);
                    //member.SetValue("mailinglistinclude", model.MailingListInclude);
                    //member.IsApproved = false;

                    #endregion


                    customer.MemberId = newMember.Id;
                    _scottybonsEComEntities.Customers.Add(customer);
                    _scottybonsEComEntities.SaveChanges();

                    Session["CustomerID"] = customer.CustomerID;


                    //Send Welcome Email to Customer 
                    SendWelcomeEmail(newMember);

                    if (Members.Login(model.Emailaddress, model.Password))
                    {
                        return
                            Redirect(Request["Nav"] == "IsForOrder"
                                ? Url.Content(GlobalConstants.LanguageUrl + "/order/PlanScottyBox")
                                : Url.Content(GlobalConstants.LanguageUrl + "/account/myaccount/"));
                    }
                }
                catch (Exception ex)
                {

                    return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
                }



            }

            ViewBag.ErrorMsg = Resources.Scottybons.ScottybonsDataStrings.EmailExisted;  //  "Problem while logging in.";
            return CurrentUmbracoPage();
        }

        /// <summary>
        /// Send Welcome Email to newly Registered Member
        /// </summary>
        /// <param name="member"></param>
        private void SendWelcomeEmail(IMember member)
        {
           
            var urlToProfile = GlobalConstants.LinkInEmail + ACCOUNT_PROFILEQUESTIONS;
            
            urlToProfile = string.Format("<a href='{0}'>" + Resources.Scottybons.ScottybonsDataStrings.LinkHere + "</a>", urlToProfile);
            var emailServices = new EmailServices(_scottybonsEComEntities);
            emailServices.SendEmailWithLink((int)EmailReason.RegistersAndCreatesAnAccount, member.Name, member.Email, urlToProfile);
        }

        [HttpGet]
        public ActionResult MemberLogout()
        {
            TempData.Clear();
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect(GlobalConstants.LanguageUrl);
        }

        public ActionResult Login(LoginVm model)
        {
            try
            {
                var urlToProfile = GlobalConstants.LinkInEmail + ACCOUNT_PROFILEQUESTIONS;

                //Get the querystring ReturnUrl
                var returnUrl = Request.QueryString["ReturnUrl"];

                if (!ModelState.IsValid)
                {
                    ViewBag.errMsg = Resources.Scottybons.ScottybonsDataStrings.ProblemWhileLoggingIn;
                    model.ErrorMessage = ViewBag.errMsg;
                    return CurrentUmbracoPage();
                }

                var isValid = (Membership.ValidateUser(model.Email, model.Password));
                if (isValid)
                {
                    //Populate CustomerId for Sessions
                    var user = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.Email.Trim().Equals(model.Email));
                    var customerId = !ReferenceEquals(user, null) ? user.CustomerID : 0;

                    if (customerId > 0)
                    {
                        Session["CustomerID"] = customerId;

                        FormsAuthentication.SetAuthCookie(model.Email, false); // revisit, put true

                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        if (Request["Nav"] == "IsForOrder")
                        {
                            return Redirect(GlobalConstants.LanguageUrl + ACCOUNT_STYLEPROFILE);
                        }
                        return Redirect(GlobalConstants.LanguageUrl + ACCOUNT_MYACCOUNT);
                    }
                }
                ViewBag.errMsg = Resources.Scottybons.ScottybonsDataStrings.ProblemWhileLoggingIn;
                model.ErrorMessage = ViewBag.errMsg;
                return CurrentUmbracoPage();
            }
            catch (Exception ex)
            {
                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }

        }

        /// <summary>
        /// Forgot Password : Post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(ForgotPasswordPageModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_scottybonsEComEntities != null)
                    {

                        var user =
                            _scottybonsEComEntities.Customers.FirstOrDefault(
                                c => c.Email.Trim().Equals(model.ForgotPasswordViewModel.Email));


                        if (user == null)
                        {
                            // Don't reveal that the user does not exist or is not confirmed

                           if( Session["ForgotPasswordSuccess"] == null)
                               Session["ForgotPasswordSuccess"] = Resources.Scottybons.ScottybonsDataStrings.Forgot_EmailUnknown;
                           return RedirectToCurrentUmbracoPage("Error=Error");
                        }

                        //Set expiry date to 
                        var expiryTime = DateTime.UtcNow.AddMinutes(15);


                        var forgotPassword = new ForgotPassword
                        {
                            CustomerID = user.CustomerID,
                            ResetGuid = expiryTime.ToString("ddMMyyyyHHmmssFFFF")
                        };

                        _scottybonsEComEntities.ForgotPasswords.Add(forgotPassword);
                        _scottybonsEComEntities.SaveChanges();

                        //Reset link
                        var baseUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(System.Web.HttpContext.Current.Request.Url.AbsolutePath, string.Empty);

                        var callbackUrl = GlobalConstants.LinkInEmail + ACCOUNT_RESETPASSWORD_RESETGUID + expiryTime.ToString("ddMMyyyyHHmmssFFFF");

                        var emailServices = new EmailServices(_scottybonsEComEntities);
                        emailServices.SendEmailForForgotPassword((int)EmailReason.ForgotPassword, user.FirstName, user.Email, callbackUrl);

                    }

                    return Redirect(GlobalConstants.LanguageUrl + ACCOUNT_FORGETPASSWORDCONFIRMATION);
                }

                // If we got this far, something failed, redisplay form
                return CurrentUmbracoPage();

            }
            catch (Exception ex)
            {
                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }

        }


        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            //return View();
            return CurrentUmbracoPage();
        }



        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var customer = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.Email.Equals(model.Email));

                    //Update Umbraco MemberShip Provider
                    var member = Services.MemberService.GetByUsername(model.Email);
                    Services.MemberService.SavePassword(member, model.Password);

                    if (TempData["ResetPasswordError"] == null || string.IsNullOrEmpty(ViewBag.ResetPasswordError))
                    {
                        TempData["ResetPasswordError"] = null;
                        ViewBag.ResetPasswordError = "";
                    }
                    return Redirect(GlobalConstants.LanguageUrl + RESETPASSWORDCONFIRMATION);

                }

                return CurrentUmbracoPage();
            }
            catch (Exception ex)
            {

                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }

        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return CurrentUmbracoPage();
        }

        #endregion

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                ViewBag.NewPassword = model.NewPassword;
                ViewBag.ConfirmNewPassword = model.ConfirmNewPassword;
                if (ModelState.IsValid)
                {
                    //var newPassword = model.NewPassword;
                    var customer = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID == GetSession.CustomerId);
                    var member = Services.MemberService.GetByUsername(customer.Email);
                    Services.MemberService.SavePassword(member, model.NewPassword);
                    ViewBag.Status = "Your Password Updated Successfully";
                    ViewBag.NewPassword = "";
                    ViewBag.ConfirmNewPassword = "";
                }
                return CurrentUmbracoPage();
                //return Redirect(GlobalConstants.LanguageUrl + CHANGEPASSWORDCONFIRMATION);

            }
            catch (Exception ex)
            {
                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }
        }

        #region "Helper Methods"
        // Handles the login
        // you might wonder why this can't be just called MvcMemberRegister
        // http://our.umbraco.org/forum/developers/api-questions/37547-SurfaceController-Form-Post-with-feedback
        [ChildActionOnly]
        [ActionName("MvcMemberLoginRenderForm")]
        public ActionResult MvcMemberLoginRenderForm(MemberModel model)
        {
            string checkUrl = HttpContext.Request.Url.AbsolutePath.ToString();

            // add a trailing / if there isn't one (you can access the same page via http://mydomain.com/login or http://mydomain.com/login/)
            if (checkUrl[checkUrl.Length - 1] != '/')
            {
                checkUrl = checkUrl + "/";
            }

            // if we don't have a session variable and have a request URL then store it
            // we have to store it because if user tries an incorrect login then Current.Request.Url will show /umbraco/RenderMvc 
            // in MVC we won't have "/umbraco/RenderMvc" but I leave this in here just in case
            if (HttpContext.Request.Url != null && HttpContext.Request.Url.AbsolutePath.ToString() != "/umbraco/RenderMvc" && HttpContext.Session["redirectURL"] == null)
            {
                if (checkUrl.ToLower() != MEMBERS_LOGIN_URL)
                {
                    HttpContext.Session["redirectURL"] = HttpContext.Request.Url.ToString();
                }
            }

            // set this to be checked by default - wish you could just pass checked=checked
            model.RememberMe = true;
            return PartialView("Login", model);
        }
        #endregion







    }
}
