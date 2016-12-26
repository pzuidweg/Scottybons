using Resources.Scottybons;
using ScottybonsMVC.AppConstants;
using ScottybonsMVC.Models;
using ScottybonsMVC.Models.Entities;
using ScottybonsMVC.Models.PageViewModels;
using ScottybonsMVC.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using ScottybonsMVC.Models.ViewModels.Customer;

namespace ScottybonsMVC.Controllers
{
    public class PromotionsController : SurfaceController
    {

        ScottybonsEComEntities _scottybonsEComEntities;
        GiftCardServices _giftCardServices;

        public PromotionsController()
        {
            if (ReferenceEquals(_scottybonsEComEntities, null))
            {
                _scottybonsEComEntities = new ScottybonsEComEntities();
            }

            if (ReferenceEquals(_giftCardServices, null))
            {
                _giftCardServices = new GiftCardServices();
            }

        }
        // GET: Promotions
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult GiftCard()
        {

            try
            {
                var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                var model = new GiftCardPageViewModel();
                var currentPage = CurrentPage.Properties;
                model.ProcessTitle = CurrentPage.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "processTitle").Value.ToString();
                model.ProcessText = CurrentPage.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "processText").Value.ToString();
                model.ProcessTextOne = CurrentPage.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "processTextOne").Value.ToString();
                model.ProcessTextTwo = CurrentPage.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "processTextTwo").Value.ToString();
                var image = umbracoHelper.Media(Convert.ToInt32(CurrentPage.Properties.FirstOrDefault(p => p.PropertyTypeAlias == "processImage").Value));
                model.ProcessImage = image.url;
                model.IsLoggedIn = Umbraco.MemberIsLoggedOn();

                if (model.IsLoggedIn)
                {
                    var memberId = Members.GetCurrentMemberId();
                    var user = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.MemberId.Equals(memberId));
                    model.LoggedUserFirstName = user.FirstName;
                    model.LoggedUserLastName = user.LastName;
                }

                ViewBag.PaymentTypes =
                                     _scottybonsEComEntities.PaymentTypeMasters.Where(c => c.Active != false && c.PaymentTypeID != 3)
                                         .OrderBy(c => c.DisplayOrder)
                                         .Select(
                                             oc => new SelectListItem() { Text = oc.PaymentTypeName, Value = oc.PaymentTypeID.ToString(), Selected = oc.PaymentTypeID.Equals(2) })
                                         .ToList();

                var issuers = _giftCardServices.GetIssuers();

                if (!ReferenceEquals(issuers, null))
                {
                    ViewBag.IssuerTypes = issuers.Select(
                        oc => new SelectListItem() { Text = oc.Key, Value = oc.Value.ToString() })
                        .ToList();
                    ;
                }

                ViewBag.Country =
                  _scottybonsEComEntities.CountryMasters.Distinct().Where(c => c.CountryID == 1).Select(
                        oc => new SelectListItem() { Text = oc.CountryName, Value = oc.CountryID.ToString() }).ToList();

                var orderKey = "OrderID"; var status = "Status";
                var isOrder = Request.QueryString[orderKey] != null;
                var isStatus = Request.QueryString[status] != null;

                if (isOrder)
                {
                    model.IsOrder = isOrder;
                    var orderNumber = Request.QueryString[orderKey];
                    if (!string.IsNullOrEmpty(orderNumber))
                    {
                        var orderDetails = _scottybonsEComEntities.GIftCardsOrders.FirstOrDefault(gco => gco.OrderNumber.Trim().Equals(orderNumber.Trim()));

                        if (!ReferenceEquals(orderDetails, null))
                        {
                            model.OrderNumber = orderDetails.OrderNumber.Trim();
                            if (isStatus)
                            {
                                var statusValue = Request.QueryString[status];
                                if (!string.IsNullOrEmpty(statusValue))
                                {
                                    if (statusValue.ToLower().Trim().Equals("ok"))
                                    {
                                        //Update order with success details
                                        orderDetails.Status = (int)GiftCardOrderStatus.Completed;
                                        orderDetails.PaymentId = string.IsNullOrEmpty(Request.QueryString["PaymentID"]) ? string.Empty : Request.QueryString["PaymentID"];
                                        orderDetails.TransactionId = string.IsNullOrEmpty(Request.QueryString["TransactionID"]) ? string.Empty : Request.QueryString["TransactionID"];
                                        _scottybonsEComEntities.SaveChanges();
                                        model.OrderStatus = true;
                                        //Send Email to Customer
                                        var emailServices = new Infrastructure.EmailServices(_scottybonsEComEntities);
                                        emailServices.SendEmailWithOutLink((int)EmailReason.GiftCardCustomer, orderDetails.PayerFirstName, orderDetails.Email, model.OrderNumber);
                                    }
                                    else if (statusValue.ToLower().Trim().Equals("err"))
                                    {
                                        model.OrderExists = true;
                                        model.OrderTotal = orderDetails.OrderTotal.Value;
                                        model.OrderGiftCards = _giftCardServices.GetOrderGiftCards(orderDetails.OrderNumber);

                                        //Update order with error details
                                        orderDetails.Status = (int)GiftCardOrderStatus.Error;
                                        orderDetails.PaymentId = string.IsNullOrEmpty(Request.QueryString["PaymentID"]) ? string.Empty : Request.QueryString["PaymentID"];
                                        orderDetails.TransactionId = string.IsNullOrEmpty(Request.QueryString["TransactionID"]) ? string.Empty : Request.QueryString["TransactionID"];
                                        _scottybonsEComEntities.SaveChanges();
                                        model.OrderStatus = false;
                                    }
                                    else
                                    {
                                        model.OrderExists = true;
                                        model.OrderTotal = orderDetails.OrderTotal.Value;
                                        model.OrderGiftCards = _giftCardServices.GetOrderGiftCards(orderDetails.OrderNumber);

                                        //Update order with error details
                                        orderDetails.Status = (int)GiftCardOrderStatus.Open;
                                        orderDetails.PaymentId = string.IsNullOrEmpty(Request.QueryString["PaymentID"]) ? string.Empty : Request.QueryString["PaymentID"];
                                        orderDetails.TransactionId = string.IsNullOrEmpty(Request.QueryString["TransactionID"]) ? string.Empty : Request.QueryString["TransactionID"];
                                        _scottybonsEComEntities.SaveChanges();
                                        model.OrderStatusIsOpen = true;
                                    }
                                }
                            }
                        }
                    }
                }
                return PartialView("_GiftCard", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [ChildActionOnly]
        public ActionResult ManageSubscription()
        {
            try
            {
                var model = new SubscriptionPageViewModel();
                var contentNode = CurrentPage;

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
                    model.IsSubscription = customerOrder.PeriodicalScottyBoxID > 0;
                    model.NextScottyBoxDate = customerOrder.NextPerodicalScottyBoxDate==null?"N/A":customerOrder.NextPerodicalScottyBoxDate.Value.ToString("MM/dd/yyyy");
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
                //return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
                throw;
            }
        }
        public JsonResult Login(LoginModel model)
        {
            try
            {
                var isValid = (Membership.ValidateUser(model.Email, model.Password));
                if (isValid)
                {
                    //Populate CustomerId for Sessions
                    var user = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.Email.Trim().Equals(model.Email));
                    var customerId = !ReferenceEquals(user, null) ? user.CustomerID : 0;

                    if (customerId > 0)
                    {
                        Session["CustomerID"] = customerId;
                        FormsAuthentication.SetAuthCookie(model.Email, false);
                        model.FirstName = user.FirstName;
                        model.LastName = user.LastName;
                        model.Status = true;
                    }
                }

                model.ErrorMsg = model.Status ? "Success" : "Invalid Credentials";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                model.ErrorMsg = "Error Occured. Please contact Administrator.";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult HandleGiftCardsReOrderAction(GiftCardModel model)
        {

            try
            {
                model.OrderNumber = HttpUtility.ParseQueryString(Request.UrlReferrer.Query).GetValues("OrderID").FirstOrDefault().ToString();
                //Payment
                _giftCardServices.GiftCardReOrderPayment(model);

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                model.ErrorMsg = "Error Occured. Please contact Administrator.";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult HandleGiftCardsAction(GiftCardModel model)
        {
            try
            {
                model = _giftCardServices.HandleGiftCard(model);
                //Payment
                _giftCardServices.GiftCardPayment(model);

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                model.ErrorMsg = "Error Occured. Please contact Administrator.";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
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
    }
}