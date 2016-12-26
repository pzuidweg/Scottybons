using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Resources.Scottybons;
using ScottybonsMVC.AppConstants;
using ScottybonsMVC.Infrastructure;
using ScottybonsMVC.Models.Entities;
using ScottybonsMVC.Models.ViewModels;
using ScottybonsMVC.Models.ViewModels.Customer;
using Umbraco.Core;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using System;
using ScottybonsMVC.Models.PageViewModels;

namespace ScottybonsMVC.Controllers
{

    public class AccountController : RenderMvcController
    {
        ScottybonsEComEntities _scottybonsEComEntities;

        const string ORDER_PLANSCOTTYBOX = "/order/PlanScottyBox/";
        private const string LOGIN_URL = "/account/login/";
        private const string PROFILEQUESTION_URL = "/account/ProfileQuestions/";
        private const string ACCOUNTFORGOTPASSWORD_URL = "/account/forgetpasswordconfirmation/";

        //private const string RESET_TOKEN_ERROR =
        //    Resources.Scottybons.ScottybonsDataStrings.ForgotPasswordTokenExpired;	

        private const string EDITPROFILEQUESTION_URL = "/account/EditProfileQuestions/";


        public AccountController()
        {
            if (ReferenceEquals(_scottybonsEComEntities, null))
            {
                _scottybonsEComEntities = new ScottybonsEComEntities();
            }

        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(RenderModel model)
        {

            try
            {
                string errorMessage = string.Empty;

                //Get the querystring GUID
                var resetQs = Request.QueryString["resetGUID"];

                if (resetQs != null)
                {
                    var user =
                        _scottybonsEComEntities.ForgotPasswords.FirstOrDefault(c => c.ResetGuid.Trim().Equals(resetQs));
                    if (user != null)
                    {
                        var customer =
                            _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID.Equals(user.CustomerID));


                        DateTime expiryTime = DateTime.ParseExact(resetQs, "ddMMyyyyHHmmssFFFF", null);
                        //Check if date has NOT expired (been and gone) - Removed Expiry conditions
                        //if ((DateTime.Now).CompareTo(expiryTime) < 0)
                        //{
                        ViewBag.Email = customer.Email;
                        return CurrentTemplate(model);
                        //}
                    }
                }
                if (TempData["ResetPasswordError"] == null)
                {
                    TempData["ResetPasswordError"] = Resources.Scottybons.ScottybonsDataStrings.ForgotPasswordTokenExpired;
                }

                return Redirect(GlobalConstants.LanguageUrl + ACCOUNTFORGOTPASSWORD_URL);
            }
            catch (Exception ex)
            {

                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }

        }

        /// <summary>
        /// Register - Get
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Register(RenderModel model)
        {
            try
            {
                if (GetSession.CustomerId > 0)
                {
                    return Redirect(GlobalConstants.ErrorURL);
                }
                ViewBag.email = Request["email"];
                ViewBag.Languages =
                    _scottybonsEComEntities.CountryMasters.Select(
                        l => new SelectListItem() { Text = l.LanguageName, Value = l.TwoLetterISOCode + "-" + l.TwoLetterLanguageCode }).ToList();
                var registermodel = new RegisterVm(model.Content);
                return CurrentTemplate(registermodel);
            }
            catch (Exception ex)
            {

                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }

        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(RenderModel model)
        {
            return CurrentTemplate(new LoginVm(model.Content));
        }


        [Authorize]
        // GET: Order/Create
        public ActionResult Account(RenderModel model)
        {
            try
            {
                if (GetSession.CustomerId == 0)
                {
                    TempData.Clear();
                    Session.Clear();
                    FormsAuthentication.SignOut();
                    var loginUrl = GlobalConstants.LanguageUrl + LOGIN_URL;
                    return Redirect(loginUrl);
                }
                var myAccountmodel = new MyAccountVM(model.Content);
                return CurrentTemplate(myAccountmodel);
            }
            catch (Exception ex)
            {

                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }

        }
        
        public ActionResult ManageSubscription()
        {
            try
            {
                var model = new SubscriptionPageViewModel();
                var contentNode = CurrentPage.Children.FirstOrDefault(c => c.Name.Equals("ManageSubscription"));

                model.PageTitle = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "pageTitle").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "pageTitle").DataValue.ToString() : string.Empty;
                model.PageSubTitle = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "pageSubtitle").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "pageSubtitle").DataValue.ToString() : string.Empty;
                model.SelectPeriod = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "selectPeriod").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "selectPeriod").DataValue.ToString() : string.Empty;
                model.DeliveryDurationNote = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "deliveryDurationNoteText").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "deliveryDurationNoteText").DataValue.ToString() : string.Empty;
                model.EditButton = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "editButtonText").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "editButtonText").DataValue.ToString() : string.Empty;
                model.SendMeScottybox = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "sendMeScottyboxText").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "sendMeScottyboxText").DataValue.ToString() : string.Empty;
                //var subscriptionTypeList = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "subscriptionTypeList");
                model.HowToChangeNote = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "howToChangeNote").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "howToChangeNote").DataValue.ToString() : string.Empty;
                model.OccasionText = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "occasionText").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "occasionText").DataValue.ToString() : string.Empty;
                model.PhoneNumberText = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "phoneNumberText").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "phoneNumberText").DataValue.ToString() : string.Empty;
                model.DeliveryAddressText = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "deliveryAddressText").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "deliveryAddressText").DataValue.ToString() : string.Empty;
                model.DeliveryAtNeighbourText = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "deliveryAtNeighbourText").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "deliveryAtNeighbourText").DataValue.ToString() : string.Empty;
                model.SaveButton = contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "saveButtonText").HasValue ? contentNode.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "saveButtonText").DataValue.ToString() : string.Empty;

                var customer = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID == GetSession.CustomerId);
                var customerOrder =
                        _scottybonsEComEntities.Orders.Where(m => m.CustomerID == customer.CustomerID)
                            .OrderByDescending(c => c.OrderID)
                            .FirstOrDefault();
                //Populate Model Items
                model.OrderContent = GenerateOrderModel();
                if (!ReferenceEquals(customerOrder, null))
                {
                    var deliveryAddress = new DeliveryAddressViewModel()
                    {
                        Name = customerOrder.OrderComanyName,
                        Street = customerOrder.OrderStreet,
                        Number = customerOrder.OrderHouseNumber,
                        Zip = customerOrder.OrderPostalCode,
                        Town = customerOrder.OrderTown,
                        CountryId =
                            !ReferenceEquals(customerOrder.OrderCountryID, null) ? customerOrder.OrderCountryID : 1
                    };

                    model.OrderContent.DeliveryAddress = deliveryAddress;

                    model.OrderContent.ContactNumber = customerOrder.ContactNumber;

                    model.OrderContent.CustomerOrder.DeliverNeighbours = true;
                    if (customerOrder.DeliverNeighbours.HasValue)
                        model.OrderContent.CustomerOrder.DeliverNeighbours = (customerOrder.DeliverNeighbours == true ? true : false);

                    model.OrderContent.CustomerOrder.ContactNumber = customerOrder.ContactNumber;
                    model.OrderContent.CustomerOrder.PeriodicalScottyBoxID = customerOrder.PeriodicalScottyBoxID;
                    model.OrderContent.CustomerOrder.PeriodicalScottyBoxMaster = customerOrder.PeriodicalScottyBoxMaster;
                    model.OrderContent.CustomerOrder.IsStylistContactYou = customerOrder.IsStylistContactYou;

                    ViewBag.PeriodicalScottyBoxID = customerOrder.PeriodicalScottyBoxID;
                }



                var results = _scottybonsEComEntities.CountryMasters.Distinct();

                ViewBag.Country =
                  results.Where(c => c.CountryID == 1).Select(
                        oc => new SelectListItem() { Text = oc.CountryName, Value = oc.CountryID.ToString() }).ToList();

                return PartialView("account/_mysubscription", model);
                //return CurrentTemplate(orderModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Order/Create
        [Authorize]
        public ActionResult ProfileQuestions(RenderModel model)
        {
            try
            {
                var profileQuestionsVm = new ProfileQuestionsVm();

                if (GetSession.CustomerId == 0)
                {
                    TempData.Clear();
                    Session.Clear();
                    FormsAuthentication.SignOut();
                    var loginUrl = GlobalConstants.LanguageUrl + LOGIN_URL;
                    return Redirect(loginUrl);
                }

                var isCustomerAnsweredQuestion = _scottybonsEComEntities.CustomerAnswers.Any(cs => cs.CustomerId == GetSession.CustomerId);

                profileQuestionsVm.IsNewCustomer = !isCustomerAnsweredQuestion;
                if (isCustomerAnsweredQuestion)
                {
                    return Redirect(GlobalConstants.LanguageUrl + EDITPROFILEQUESTION_URL);
                }

                profileQuestionsVm.ProfileQuestionsViewModel = GenerateSingleQuestion(1, GetSession.CustomerId);
                return CurrentTemplate(profileQuestionsVm);
            }
            catch (Exception ex)
            {

                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }

        }

        [Authorize]
        [OutputCache(Duration = 10)]
        public ActionResult EditProfileQuestions(RenderModel model)
        {
            try
            {
                if (GetSession.CustomerId == 0)
                {
                    TempData.Clear();
                    Session.Clear();
                    FormsAuthentication.SignOut();
                    var loginUrl = GlobalConstants.LanguageUrl + LOGIN_URL;
                    return Redirect(loginUrl);
                }

                var profileQuestionsVm = new ProfileQuestionsVm();
                profileQuestionsVm.ProfileQuestionsForEditAndView = PopulateQuestions();

                return CurrentTemplate(profileQuestionsVm);
            }
            catch (Exception ex)
            {

                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }


        }

        [HttpPost]
        [OutputCache(Duration = 10)]
        public ActionResult EditProfileQuestions(ProfileQuestionsVm model, IEnumerable<HttpPostedFileBase> files)
        {

            var mandatoryQuestionNotFilled = false;

            foreach (var objProfileQuestionsVm in model.ProfileQuestionsForEditAndView)
            {

                HttpPostedFileBase selectedFile = null;

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (!ReferenceEquals(file, null))
                        {
                            if (objProfileQuestionsVm.QuestionNumber == 45)
                            {
                                selectedFile = files.First();
                            }
                            else if (objProfileQuestionsVm.QuestionNumber == 46)
                            {
                                selectedFile = files.Last();
                            }
                        }
                    }


                }

                var status = SaveCustomerProfileAnswer(objProfileQuestionsVm, selectedFile);

                if ((objProfileQuestionsVm.IsRequired) && !status)
                {
                    mandatoryQuestionNotFilled = true;
                    ModelState.AddModelError("ValidationError", objProfileQuestionsVm.ValidationMessage);
                    objProfileQuestionsVm.ErrorMessage =
                        objProfileQuestionsVm.ValidationMessage;
                }
            }
            if (mandatoryQuestionNotFilled)
            {
                //To refresh the hiddent Variables
                Session["NoFilledAllQuestions"] = true;
                ModelState.Clear();
                return CurrentTemplate(model);
            }

            //Validate Session
            if (GetSession.CustomerId == 0)
            {
                TempData.Clear();
                Session.Clear();
                FormsAuthentication.SignOut();
                var loginUrl = GlobalConstants.LanguageUrl + LOGIN_URL;
                return Redirect(loginUrl);
            }
            else
            {
                var customer = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID == GetSession.CustomerId);
                var order = _scottybonsEComEntities.Orders.FirstOrDefault(c => c.OrderID == GetSession.OrderNumber);
                //Send Email to Customer with the subject:Modified Style Profile
                SendChangesStyleprofileEmail();

                //Send Email to Admin for answered questions
                SendFillsStyleProfileEmailToAdmin(customer);

                Session["NoFilledAllQuestions"] = false;
            }

            //Generate Order Page
            var confirmationUrl = GlobalConstants.LanguageUrl + ORDER_PLANSCOTTYBOX;
            //Redirect to Order Page
            return Redirect(confirmationUrl);

        }


        [HttpPost]
        public ActionResult ProfileQuestions(string button, ProfileQuestionsVm model, HttpPostedFileBase file)
        {
            try
            {
                int currentQuestionNumber = 0;

                currentQuestionNumber = button == "Previous" ? model.ProfileQuestionsViewModel.PreviousQuestionNumber : model.ProfileQuestionsViewModel.NextQuestionNumber;
                if ((button != "Previous"))
                {
                    if (ModelState.IsValid)
                    {
                        //Save Answer to Database
                        var status = SaveCustomerProfileAnswer(model.ProfileQuestionsViewModel, file);

                        if ((model.ProfileQuestionsViewModel.IsRequired) && !status)
                        {
                            currentQuestionNumber = model.ProfileQuestionsViewModel.NextQuestionNumber - 1;
                            ModelState.Clear();
                            ModelState.AddModelError("ValidationError", model.ProfileQuestionsViewModel.ValidationMessage);
                            return CurrentTemplate(model);
                        }
                    }
                    else
                    {
                        currentQuestionNumber = model.ProfileQuestionsViewModel.QuestionNumber;
                        ModelState.Clear();
                        ModelState.AddModelError("", model.ProfileQuestionsViewModel.ValidationMessage);
                    }
                }
                if (model.ProfileQuestionsViewModel.NumberOfQuestions == model.ProfileQuestionsViewModel.QuestionNumber)
                {
                    //Validate Session
                    if (GetSession.CustomerId == 0)
                    {
                        TempData.Clear();
                        Session.Clear();
                        FormsAuthentication.SignOut();
                        var loginUrl = GlobalConstants.LanguageUrl + LOGIN_URL;
                        return Redirect(loginUrl);
                    }
                    else
                    {
                        var customer = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID == GetSession.CustomerId);
                        var order = _scottybonsEComEntities.Orders.FirstOrDefault(c => c.OrderID == GetSession.OrderNumber);
                        //Send Email to Customer with the subject:So good to see you filled your style profile!
                        SendFillsStyleProfileEmail(customer, order);

                        //Send Email to Admin for answered questions
                        SendFillsStyleProfileEmailToAdmin(customer);
                    }


                    //Generate Order Page
                    var confirmationUrl = GlobalConstants.LanguageUrl + ORDER_PLANSCOTTYBOX;
                    //Redirect to Order Page
                    return Redirect(confirmationUrl);
                }

                var profileQuestionsViewModel = GenerateSingleQuestion(currentQuestionNumber, GetSession.CustomerId);
                var profileQuestionsVm = new ProfileQuestionsVm { ProfileQuestionsViewModel = profileQuestionsViewModel };

                //To refresh the hiddent Variables
                ModelState.Clear();
                return CurrentTemplate(profileQuestionsVm);
            }
            catch (Exception ex)
            {

                return Redirect(GlobalConstants.ErrorURL + ex.Message);
            }
        }

        [Authorize]
        public ActionResult StyleProfile(RenderModel model)
        {
            #region Authtication Varificastion
            if (GetSession.CustomerId == 0)
            {
                TempData.Clear();
                Session.Clear();
                FormsAuthentication.SignOut();
                var loginUrl = GlobalConstants.LanguageUrl + LOGIN_URL;
                return Redirect(loginUrl);
            }
            #endregion

            //Get Customer Status : is customer filling first time  then navigate to Create Profile Otherwise Style Profile
            var isCustomerPrviouslyFilledStyleProfile = _scottybonsEComEntities.CustomerAnswers.Any(cs => cs.CustomerId == GetSession.CustomerId);

            if (!isCustomerPrviouslyFilledStyleProfile)
            {
                var profileQuestionUrl = GlobalConstants.LanguageUrl + PROFILEQUESTION_URL;
                //Redirect to Create Profile
                return Redirect(profileQuestionUrl);
            }
            else
            {

                var myStylemodel = new StyleProfileDisplayVm(model.Content);
                return CurrentTemplate(myStylemodel);
            }
        }

        [HttpPost]
        public ActionResult StyleProfile(StyleProfileDisplayVm model)
        {


            if (model.SelectedStyleProfileId == (int)StyleProfileNavigation.EditProfile)
            {
                var profileQuestionUrl = GlobalConstants.LanguageUrl + EDITPROFILEQUESTION_URL;
                //Redirect to Create Profile
                return Redirect(profileQuestionUrl);
            }

            var orderUrl = GlobalConstants.LanguageUrl + ORDER_PLANSCOTTYBOX;
            return Redirect(orderUrl);

        }

        #region "Helper Methods"
        /// <summary>
        /// Save Customer Profile Answer
        /// </summary>
        /// <param name="model"></param>
        /// <param name="file"></param>
        private bool SaveCustomerProfileAnswer(ProfileQuestionsViewModel model, HttpPostedFileBase file)
        {
            var customerAnswer = new CustomerAnswer();
            bool isValid = false;
            if (
                model.AnswerControlType.Equals(
                    ProfileQuestionAnswerControlTypes.RadioButton))
            {
                int temp = 0;
                var res = int.TryParse(model.SelectedAnswer, out temp);
                if (res)
                {
                    if ((!ReferenceEquals(model.Answers.FirstOrDefault(m => m.AnswerID == temp), null)
                        && (!ReferenceEquals(model.Answers.FirstOrDefault(m => m.AnswerID == temp).FLAG, null))))
                    {
                        if (model.Answers.FirstOrDefault(m => m.AnswerID == temp).FLAG == "O")
                        {
                            customerAnswer.OtherAnswer = model.SelectedOtherAnswer;
                        }
                    }
                    customerAnswer.AnswerId = temp;
                    isValid = true;
                }

            }
            else if (model.AnswerControlType.Equals(
                ProfileQuestionAnswerControlTypes.UploadControl))
            {
                if (!ReferenceEquals(file, null))
                {
                    using (var ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        customerAnswer.CustomerImage = ms.GetBuffer();
                    }
                    isValid = true;
                }

            }
            else if (model.AnswerControlType.Equals(
                ProfileQuestionAnswerControlTypes.CheckBox))
            {

                var sb = new StringBuilder();
                var separator = String.Empty;
                //Generate Answer string with the comma seperated from list
                foreach (var selectedAnswerObj in model.CheckBoxAnswers)
                {
                    if (selectedAnswerObj.IsSelected)
                    {
                        sb.Append(separator).Append(selectedAnswerObj.AnswerId);
                        separator = ",";
                        isValid = true;
                        if (model.CheckBoxAnswers.FirstOrDefault(m => m.AnswerId == selectedAnswerObj.AnswerId).Flag == "O")
                        {
                            customerAnswer.OtherAnswer = model.SelectedOtherAnswer;
                        }
                    }
                }
                customerAnswer.Answer = sb.ToString();
            }
            else if (model.AnswerControlType.Equals(
                ProfileQuestionAnswerControlTypes.TextBox))
            {
                if ((!string.IsNullOrEmpty(model.SelectedAnswer)))
                {
                    customerAnswer.Answer = model.SelectedAnswer;
                    isValid = true;
                }

            }
            else if (model.AnswerControlType.Equals(
                ProfileQuestionAnswerControlTypes.Calendar))
            {
                if ((!string.IsNullOrEmpty(model.SelectedAnswer)))
                {
                    customerAnswer.Answer = model.SelectedAnswer;
                    isValid = true;
                }

            }

            if (isValid)
            {
                var customerId = !ReferenceEquals(Session["CustomerID"], null) ? (int)Session["CustomerID"] : 0;
                customerAnswer.CustomerId = customerId;
                customerAnswer.QuestionControlId = model.AnswerControlId;
                customerAnswer.QuestionId = model.QuestionId;
                customerAnswer.UpdatedOn = DateTime.UtcNow;
                customerAnswer.CreatedOn = DateTime.UtcNow;


                _scottybonsEComEntities.CustomerAnswers.Add(customerAnswer);
                _scottybonsEComEntities.SaveChanges();
            }
            else
            {
                model.ErrorMessage = model.ValidationMessage;

            }
            return isValid;
        }

        /// <summary>
        /// Generate Single Questions
        /// </summary>
        /// <param name="questionNumber"></param>
        /// <param name="customerNumber"></param>
        /// <returns></returns>
        public ProfileQuestionsViewModel GenerateSingleQuestion(int? questionNumber, int customerNumber)
        {
            var profileQuestionsViewModel = new ProfileQuestionsViewModel();
            StyleProfileQuestion objQuestion =
                _scottybonsEComEntities.StyleProfileQuestions.FirstOrDefault(
                    q => q.QuestionNumber == questionNumber && q.Language == GlobalConstants.Language);

            if (ReferenceEquals(objQuestion, null)) return profileQuestionsViewModel;

            if (ReferenceEquals( Session["NumberOfQuestions"],null))
            {
                Session["NumberOfQuestions"]=  _scottybonsEComEntities.StyleProfileQuestions.Where(q => q.Language == GlobalConstants.Language)
                    .ToList()
                    .Count;
            }

            //Navigational Properties
            profileQuestionsViewModel.NumberOfQuestions = (int) Session["NumberOfQuestions"];
              

            var previousQuestionNumber = objQuestion.QuestionNumber - 1;
            var nextQuestionNumber = objQuestion.QuestionNumber + 1;
            previousQuestionNumber = previousQuestionNumber < 1 ? 1 : previousQuestionNumber;
            nextQuestionNumber = (nextQuestionNumber <= profileQuestionsViewModel.NumberOfQuestions)
                ? nextQuestionNumber
                : profileQuestionsViewModel.NumberOfQuestions;

            profileQuestionsViewModel.PreviousQuestionNumber = previousQuestionNumber ?? 1;
            profileQuestionsViewModel.NextQuestionNumber = nextQuestionNumber ?? 1;

            //Question Information
            profileQuestionsViewModel.Question = objQuestion.Question;
            profileQuestionsViewModel.QuestionId = objQuestion.QuestionID;
            profileQuestionsViewModel.QuestionNumber = objQuestion.QuestionNumber ?? 0;
            profileQuestionsViewModel.QuestionSubDescription = objQuestion.QuestionDescription;
            profileQuestionsViewModel.IsRequired = objQuestion.isRequired ?? false;
            profileQuestionsViewModel.IsImageTobeShown = objQuestion.IsImageTobeShown ?? false;
            profileQuestionsViewModel.ImagePath = objQuestion.ImagePath ?? string.Empty;
            profileQuestionsViewModel.DisplayOrder = objQuestion.DisplayOrder ?? 0;
            profileQuestionsViewModel.LanguageOfQuestion = objQuestion.Language;
            profileQuestionsViewModel.ValidationMessage = objQuestion.ValidationMessage;

            //Answer Information
            profileQuestionsViewModel.AnswerControlId = objQuestion.AnswerControlID ?? 0;
            profileQuestionsViewModel.AnswerControlType =
                (ProfileQuestionAnswerControlTypes)profileQuestionsViewModel.AnswerControlId;
            profileQuestionsViewModel.Answers =
                _scottybonsEComEntities.StyleProfileAnswers.Where(a => a.QuestionID == objQuestion.QuestionID).OrderBy(a => a.AnswerNumber).ToList();


            profileQuestionsViewModel.Percentage = (profileQuestionsViewModel.QuestionNumber) * 100 /
                                                   (profileQuestionsViewModel.NumberOfQuestions) + "%";
            profileQuestionsViewModel.IsDisablePrevious = profileQuestionsViewModel.QuestionNumber == 1;

            if (ReferenceEquals(
                _scottybonsEComEntities.CustomerAnswers.Where(
                    c => c.QuestionId == profileQuestionsViewModel.QuestionId && c.CustomerId == customerNumber)
                    .OrderByDescending(c => c.CreatedOn),
                null))
                return profileQuestionsViewModel;
            profileQuestionsViewModel.CustomerAnswer =
                _scottybonsEComEntities.CustomerAnswers.Where(
                    c => c.QuestionId == profileQuestionsViewModel.QuestionId && c.CustomerId == customerNumber)
                    .OrderByDescending(c => c.CreatedOn)
                    .FirstOrDefault();

            if (!ReferenceEquals(profileQuestionsViewModel.CustomerAnswer, null))
            {

                if (
                    profileQuestionsViewModel.AnswerControlType.Equals(ProfileQuestionAnswerControlTypes.RadioButton) &&
                    (!ReferenceEquals(profileQuestionsViewModel.CustomerAnswer.AnswerId, null)))
                {
                    profileQuestionsViewModel.SelectedAnswer =
                        profileQuestionsViewModel.CustomerAnswer.AnswerId.ToString();
                    if (!string.IsNullOrEmpty(profileQuestionsViewModel.CustomerAnswer.Answer))
                    {
                        profileQuestionsViewModel.SelectedOtherAnswer =
                        profileQuestionsViewModel.CustomerAnswer.Answer;
                    }

                    profileQuestionsViewModel.SelectedAnswerDescription =
                        profileQuestionsViewModel.Answers.FirstOrDefault(
                            o => o.AnswerID == profileQuestionsViewModel.CustomerAnswer.AnswerId).Answer + " " +
                        profileQuestionsViewModel.Answers.FirstOrDefault(
                            o => o.AnswerID == profileQuestionsViewModel.CustomerAnswer.AnswerId).AnswerDescription;
                }
                else if (
                    profileQuestionsViewModel.AnswerControlType.Equals(ProfileQuestionAnswerControlTypes.TextBox) &&
                    (!ReferenceEquals(profileQuestionsViewModel.CustomerAnswer.Answer, null)))
                    profileQuestionsViewModel.SelectedAnswer = profileQuestionsViewModel.CustomerAnswer.Answer;
                else if (
                   profileQuestionsViewModel.AnswerControlType.Equals(ProfileQuestionAnswerControlTypes.Calendar) &&
                   (!ReferenceEquals(profileQuestionsViewModel.CustomerAnswer.Answer, null)))
                    profileQuestionsViewModel.SelectedAnswer = profileQuestionsViewModel.CustomerAnswer.Answer;
                else if (
                    profileQuestionsViewModel.AnswerControlType.Equals(
                        ProfileQuestionAnswerControlTypes.UploadControl) &&
                    (!ReferenceEquals(profileQuestionsViewModel.CustomerAnswer.CustomerImage, null)))
                    profileQuestionsViewModel.SelectedAnswerImage =
                        profileQuestionsViewModel.CustomerAnswer.CustomerImage;
                else if (
                    profileQuestionsViewModel.AnswerControlType.Equals(ProfileQuestionAnswerControlTypes.CheckBox) &&
                    (!ReferenceEquals(profileQuestionsViewModel.CustomerAnswer.Answer, null)))
                {
                    if (!string.IsNullOrEmpty(profileQuestionsViewModel.CustomerAnswer.OtherAnswer))
                    {
                        profileQuestionsViewModel.SelectedOtherAnswer = profileQuestionsViewModel.CustomerAnswer.OtherAnswer;
                    }
                    profileQuestionsViewModel.SelectedAnswer = profileQuestionsViewModel.CustomerAnswer.Answer;
                }
                   

            }

            var answerCheckBoxViewModel = profileQuestionsViewModel.Answers.Select(ans => new AnswerControlViewModel
            {
                IsSelected = !ReferenceEquals(profileQuestionsViewModel.SelectedAnswer, null) && profileQuestionsViewModel.SelectedAnswer.Contains(ans.AnswerID.ToString()),
                AnswerId = ans.AnswerID,
                QuestionId = ans.QuestionID ?? 1,
                Answer = ans.Answer,
                AnswerDescription = ans.AnswerDescription,
                AnswerImagePath = ans.AnswerImagePath,
                Flag = ans.FLAG,
                QuestionNumber = ans.QuestionNumber,
               
                DependentQuestion = ans.DependentQuestion ?? 1
            }).ToList();
            profileQuestionsViewModel.CheckBoxAnswers = answerCheckBoxViewModel;

            return profileQuestionsViewModel;
        }

        /// <summary>
        /// Populate List of Questions for Edit/View
        /// </summary>
        /// <returns></returns>
        private List<ProfileQuestionsViewModel> PopulateQuestions()
        {

            var profQuestions =
                _scottybonsEComEntities.StyleProfileQuestions.Where(c => c.Language == GlobalConstants.Language)
                    .OrderBy(c => c.DisplayOrder);
            var allProfileQuestionsViewModel = new List<ProfileQuestionsViewModel>();

            foreach (var objQuestions in profQuestions)
            {
                var profileQuestionsViewModel = GenerateSingleQuestion(objQuestions.QuestionNumber, GetSession.CustomerId);
                allProfileQuestionsViewModel.Add(profileQuestionsViewModel);
            }
            return allProfileQuestionsViewModel;

        }


        /// <summary>
        /// Send Welcome Email to newly Registered Member
        /// </summary>
        private void SendFillsStyleProfileEmail(Customer customer, Order order)
        {

            if (!ReferenceEquals(customer, null))
            {
                var urlToOrder = GlobalConstants.LinkInEmail + ORDER_PLANSCOTTYBOX;

                urlToOrder = string.Format("<a href='{0}'>" + Resources.Scottybons.ScottybonsDataStrings.LinkClickHere + "</a>", urlToOrder);
                var emailServices = new EmailServices(_scottybonsEComEntities);
                emailServices.SendEmailWithLink((int)EmailReason.FillsStyleProfile, customer.FirstName, customer.Email,
                    urlToOrder);
            }
        }

        /// <summary>
        /// SendFillsStyleProfileEmailToAdmin
        /// </summary>
        private void SendFillsStyleProfileEmailToAdmin(Customer customer)
        {


            var emailServices = new EmailServices(_scottybonsEComEntities);

            var profileQuestionsVm = new ProfileQuestionsVm { ProfileQuestionsForEditAndView = PopulateQuestions() };

            var sb = new StringBuilder();

            sb.Append("<div style='text-align: left'>");
            foreach (var profileQuestion in profileQuestionsVm.ProfileQuestionsForEditAndView)
            {

                sb.Append("<b>" + profileQuestion.QuestionNumber + "</b>  ");
                sb.Append(profileQuestion.Question);

                if (profileQuestion.AnswerControlType.Equals(ProfileQuestionAnswerControlTypes.CheckBox))
                {
                    sb.Append(": " + "<b> ");
                    foreach (var objCheckBox in profileQuestion.CheckBoxAnswers)
                    {
                        if (objCheckBox.IsSelected)
                            sb.Append(objCheckBox.Answer + ":" + objCheckBox.AnswerDescription + "  ,");
                    }
                    sb.Append("</b>");
                }
                if (profileQuestion.AnswerControlType.Equals(ProfileQuestionAnswerControlTypes.RadioButton))
                {
                    sb.Append(": " + "<b> " + profileQuestion.SelectedAnswerDescription + "</b>");
                }

                if (profileQuestion.AnswerControlType.Equals(ProfileQuestionAnswerControlTypes.TextBox))
                {
                    sb.Append(": " + "<b> " + profileQuestion.SelectedAnswer + "</b>");
                }
                if (profileQuestion.AnswerControlType.Equals(ProfileQuestionAnswerControlTypes.UploadControl))
                {
                    //if(!ReferenceEquals(null, profileQuestion.SelectedAnswerImage))
                    //{
                    //    var base64 = Convert.ToBase64String(profileQuestion.SelectedAnswerImage);
                    //    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    //    sb.Append(": " + "<img src=" + imgSrc + " />");
                    //}
                    
                }
                if (profileQuestion.AnswerControlType.Equals(ProfileQuestionAnswerControlTypes.Calendar))
                {
                    sb.Append(": " + "<b> " + profileQuestion.SelectedAnswer + "</b>");
                }

                sb.Append("<br>");
            }

            sb.Append("</div><br>");
            emailServices.SendEmailForIntake((int)EmailReason.StyleIntake, customer.FirstName, customer.LastName, customer.Email, GlobalConstants.ScottybonCustomerSupportAdminEmail, customer.CreatedOn.ToShortDateString(), customer.CreatedOn.ToShortTimeString(), sb.ToString());
        }


        /// <summary>
        /// Send Welcome Email to newly Registered Member
        /// </summary>
        private void SendChangesStyleprofileEmail()
        {

            var customer = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID == GetSession.CustomerId);

            if (!ReferenceEquals(customer, null))
            {
                var urlToOrder = GlobalConstants.LinkInEmail + ORDER_PLANSCOTTYBOX;
                urlToOrder = string.Format("<a href='{0}'>" + Resources.Scottybons.ScottybonsDataStrings.LinkHere + "</a>", urlToOrder);
                var emailServices = new EmailServices(_scottybonsEComEntities);
                emailServices.SendEmailWithLink((int)EmailReason.ChangesStyleProfile, customer.FirstName, customer.Email,
                    urlToOrder);

            }
        }

        #endregion

        private OrderVm GenerateOrderModel()
        {
            var orderModel = new OrderVm()
            {
                CustomerOrder = new Order
                {
                    IsStylistContactYou = true,
                },
                StylistContactTypes = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "true",
                        Text = ScottybonsDataStrings.OrderVm_OrderVm_Yes_Offcourse,
                        Selected = true
                    },
                    new SelectListItem
                    {
                        Value = "false",
                        Text = ScottybonsDataStrings.PlanScottyBoxViewModel_PlanScottyBoxViewModel_No,
                        Selected = false
                    }
                },
                DeliveryNeighbours = new List<SelectListItem>
                {

                    new SelectListItem
                    {
                        Value = "true",
                        Text = ScottybonsDataStrings.OrderVm_OrderVm_Yes__please,
                        Selected = true
                    },
                    new SelectListItem
                    {
                        Value = "false",
                        Text = ScottybonsDataStrings.OrderVm_OrderVm_No__please_come_back_later,
                        Selected = false
                    }
                }
            };

            //Populate Order Occessions
            var orderOccesions =
                _scottybonsEComEntities.OccasionForScottyBoxMasters.Where(
                    c => c.Active != false && c.Language.ToLower() == GlobalConstants.Language.ToLower())
                    .OrderBy(c => c.DisplayOrder)
                    .Select(
                        oc =>
                            new SelectListItem()
                            {
                                Text = oc.OccasionForScottyBoxName,
                                Value = oc.OccasionForScottyBoxID.ToString()
                            }).ToList();

            orderOccesions.Insert(0, new SelectListItem { Text = Resources.Scottybons.ScottybonsDataStrings.PlanScottybox_occasionPlaceHolder, Value = string.Empty });

            orderModel.OrderOccesions = orderOccesions;

            //Populate PeriodicalScottyBox
            var periodicalScottyBox =
                _scottybonsEComEntities.PeriodicalScottyBoxMasters.Where(
                    c => c.Active != false && c.Language.Equals(GlobalConstants.Language))
                    .OrderBy(c => c.DisplayOrder)
                    .Select(
                        oc =>
                            new SelectListItem()
                            {
                                Text = oc.PeriodicalScottyBox,
                                Value = oc.PeriodicalScottyBoxID.ToString()

                            }).ToList();

            orderModel.PeriodicalScottyBox = periodicalScottyBox;

            orderModel.PeriodicalScottyBoxSelected = true;

            return orderModel;
        }

        [HttpPost]
        public ActionResult MySubscription(SubscriptionPageViewModel model)
        {
            try
            {
                var customer = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID == GetSession.CustomerId);
                var customerOrder =
                        _scottybonsEComEntities.Orders.Where(m => m.CustomerID == customer.CustomerID)
                            .OrderByDescending(c => c.OrderID)
                            .FirstOrDefault();
                customerOrder.PeriodicalScottyBoxID = Convert.ToInt32(model.OrderContent.PeriodicalScottyBox);
                customerOrder.OrderOccasion = Convert.ToInt32(model.OrderContent.PlanScottyboxdropdownOccesions);
                customerOrder.ContactNumber = model.OrderContent.DeliveryAddress.Number;
                customerOrder.OrderComanyName = model.OrderContent.DeliveryAddress.Name;
                customerOrder.OrderStreet = model.OrderContent.DeliveryAddress.Street;
                customerOrder.OrderHouseNumber = model.OrderContent.DeliveryAddress.Number;
                customerOrder.OrderPostalCode = model.OrderContent.DeliveryAddress.Zip;
                customerOrder.OrderTown = model.OrderContent.DeliveryAddress.Town;
                customerOrder.OrderCountryID = model.OrderContent.CustomerOrder.OrderCountryID;
                customerOrder.DeliverNeighbours = model.StylistContactType.ToLower().Equals("true") ? true : false;
                _scottybonsEComEntities.SaveChanges();

                return Redirect("/");
            }
            catch (Exception ex)
            {

                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }
        }
    }
}