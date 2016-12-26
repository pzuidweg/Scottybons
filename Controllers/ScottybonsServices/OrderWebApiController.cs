using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ScottybonsMVC.AppConstants;
using ScottybonsMVC.Models.Entities;
using ScottybonsMVC.Models.ViewModels;
using ScottybonsMVC.Models.ViewModels.Admin;
using ScottybonsMVC.Models.ViewModels.Customer;

namespace ScottybonsMVC.Controllers.ScottybonsServices
{
    public class OrderWebApiController : Umbraco.Web.WebApi.UmbracoApiController
    {
        readonly ScottybonsEComEntities _scottybonsEComEntities;



        public OrderWebApiController()
        {
            if (ReferenceEquals(_scottybonsEComEntities, null))
            {
                _scottybonsEComEntities = new ScottybonsEComEntities();
            }


        }

        /// <summary>
        /// Order Streets
        /// </summary>
        /// <param name="streetNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetTestValue(string streetNumber)
        {
            return streetNumber + "123";
        }


        


        /// <summary>
        /// Order Streets
        /// </summary>
        /// <param name="streetNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> GetOrderStreets(string streetNumber)
        {
            var user = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.Email.Trim().Equals(Members.CurrentUserName));
            var customerId = !ReferenceEquals(user, null) ? user.CustomerID : 0;

            IEnumerable<string> orderStreets;

            try
            {
                orderStreets =
                    _scottybonsEComEntities.Orders.Where(
                        hpp => hpp.CustomerID == customerId && hpp.OrderStreet.StartsWith(streetNumber))
                        .Select(hpp => hpp.OrderStreet)
                        .Distinct()
                        .OrderByDescending(hpp => hpp)
                        .AsEnumerable();

            }
            catch (Exception ex)
            {
                //Log Exception
                throw ex;
            }
            return orderStreets;
        }

        /// <summary>
        /// Order House Numbers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetOrderHouseNumber(string streetNumber)
        {
            var user = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.Email.Trim().Equals(Members.CurrentUserName));
            var customerId = !ReferenceEquals(user, null) ? user.CustomerID : 0;

            IEnumerable<string> orderHouseNumber;

            try
            {
                orderHouseNumber =
                    _scottybonsEComEntities.Orders.Where(
                        hpp => hpp.CustomerID == customerId && hpp.OrderStreet.ToLower().Equals(streetNumber.ToLower()))
                        .Select(hpp => hpp.OrderHouseNumber)
                        .Distinct()
                        .OrderByDescending(hpp => hpp)
                        .AsEnumerable();

            }
            catch (Exception ex)
            {
                //Log Exception
                throw ex;
            }
            return orderHouseNumber;
        }

        /// <summary>
        /// Get Order List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderListInfo> GetOrderList()
        {
            try
            {

                if (_scottybonsEComEntities != null)
                {
                    return _scottybonsEComEntities.GetOrders().AsEnumerable();
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                throw ex;
            }
            return null;

        }


        // GET api/<controller>/5
        public IEnumerable<ProfileQuestionAnswerViewModel> GetOrderDetailsById(int id)
        {
            int orderId = id;

            var profileQuestionList = new List<ProfileQuestionAnswerViewModel>();
            IEnumerable<OrderDetailsInfo> orderCustomer = _scottybonsEComEntities.GetCustomerDetailsByOrderID(orderId).ToList();


            //Add Order Object o Customer Profile Question Object
            if ((orderCustomer.Any()))
            {
                var orderObj = orderCustomer.FirstOrDefault();

                var custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "ORDER",
                    SecondColumnValue = string.Empty
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Order Number",
                    SecondColumnValue = orderObj.OrderId.ToString(CultureInfo.InvariantCulture)
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Order Date",
                    SecondColumnValue = orderObj.CreatedOn
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Customer ID",
                    SecondColumnValue = orderObj.CustomerID.ToString()
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "First Name",
                    SecondColumnValue = orderObj.FirstName
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Last Name",
                    SecondColumnValue = orderObj.LastName
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Email",
                    SecondColumnValue = orderObj.Email
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Occasion",
                    SecondColumnValue = orderObj.OccasionForScottyBoxName
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Street Name",
                    SecondColumnValue = orderObj.OrderStreet
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Street Number",
                    SecondColumnValue = orderObj.OrderHouseNumber
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Postal Code",
                    SecondColumnValue = orderObj.OrderPostalCode
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Town",
                    SecondColumnValue = orderObj.OrderTown
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Country",
                    SecondColumnValue = orderObj.CountryName
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Permission to deliver at neighbours?",
                    SecondColumnValue = orderObj.PermissionToDeliverAtNeighbours
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Permission to call?",
                    SecondColumnValue = orderObj.PermissionToCall
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Phonenumber",
                    SecondColumnValue = orderObj.ContactNumber
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Subscription",
                    SecondColumnValue = orderObj.PeriodicalScottyBox
                };
                profileQuestionList.Add(custObj);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "Period for Subscripion",
                    SecondColumnValue = orderObj.PerodicalMonths
                };
                profileQuestionList.Add(custObj);

                //Add customer Question and Answer Object 
                var customerObject = PopulateQuestions(orderObj.CustomerID ?? 0);

                custObj = new ProfileQuestionAnswerViewModel
                {
                    FirstColumnValue = "CUSTOMER",
                    SecondColumnValue = string.Empty
                };
                profileQuestionList.Add(custObj);

                foreach (var objprofileQAnswer in customerObject)
                {
                    custObj = new ProfileQuestionAnswerViewModel();
                    custObj.FirstColumnValue = objprofileQAnswer.QuestionNumber + " " + objprofileQAnswer.Question;
                    if (objprofileQAnswer.AnswerControlType == ProfileQuestionAnswerControlTypes.UploadControl)
                    {
                        custObj.SecondColumnValue = "img";
                        custObj.AnswerImage = objprofileQAnswer.SelectedAnswerImage;
                    }
                    else
                    {
                        custObj.SecondColumnValue = objprofileQAnswer.SelectedAnswerDescription;
                    }
                    profileQuestionList.Add(custObj);

                }


            }

            return profileQuestionList;
        }

//TODO: Below methods are duplicate with Account Controller, Needs to create a common class 

        /// <summary>
        /// Populate List of Questions for Edit/View
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ProfileQuestionsViewModel> PopulateQuestions(int customerId)
        {

            var profQuestions =
                _scottybonsEComEntities.StyleProfileQuestions.Where(c => c.Language == "en")
                    .OrderBy(c => c.DisplayOrder);
            var allProfileQuestionsViewModel = new List<ProfileQuestionsViewModel>();

            foreach (var objQuestions in profQuestions)
            {
                var profileQuestionsViewModel = GenerateSingleQuestion(objQuestions.QuestionNumber, customerId);
                allProfileQuestionsViewModel.Add(profileQuestionsViewModel);
            }
            return allProfileQuestionsViewModel;

        }


        /// <summary>
        /// Generate Single Questions
        /// </summary>
        /// <param name="questionNumber"></param>
        /// <param name="customerNumber"></param>
        /// <returns></returns>
        private ProfileQuestionsViewModel GenerateSingleQuestion(int? questionNumber, int customerNumber)
        {
            var profileQuestionsViewModel = new ProfileQuestionsViewModel();
            StyleProfileQuestion objQuestion =
                _scottybonsEComEntities.StyleProfileQuestions.FirstOrDefault(
                    q => q.QuestionNumber == questionNumber && q.Language == "nl");

            if (ReferenceEquals(objQuestion, null)) return profileQuestionsViewModel;

            //Navigational Properties
            profileQuestionsViewModel.NumberOfQuestions =
                _scottybonsEComEntities.StyleProfileQuestions.Where(q => q.Language == "nl")
                    .ToList()
                    .Count;

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
                _scottybonsEComEntities.StyleProfileAnswers.Where(a => a.QuestionID == objQuestion.QuestionID)
                    .ToList();


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
                    profileQuestionsViewModel.AnswerControlType.Equals(
                        ProfileQuestionAnswerControlTypes.UploadControl) &&
                    (!ReferenceEquals(profileQuestionsViewModel.CustomerAnswer.CustomerImage, null)))
                    profileQuestionsViewModel.SelectedAnswerImage =
                        profileQuestionsViewModel.CustomerAnswer.CustomerImage;
                else if (
                    profileQuestionsViewModel.AnswerControlType.Equals(ProfileQuestionAnswerControlTypes.CheckBox) &&
                    (!ReferenceEquals(profileQuestionsViewModel.CustomerAnswer.Answer, null)))
                    profileQuestionsViewModel.SelectedAnswer = profileQuestionsViewModel.CustomerAnswer.Answer;

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

        

    }



    public class ProfileQuestionAnswerViewModel
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string AnswerDescription { get; set; }

        public byte[] AnswerImage { get; set; }

        public int SerialNumber { get; set; }

        public string FirstColumnValue { get; set; }
        public object SecondColumnValue { get; set; }
    }
}