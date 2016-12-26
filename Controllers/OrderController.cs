using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Resources.Scottybons;
using ScottybonsMVC.AppConstants;
using ScottybonsMVC.Infrastructure;
using ScottybonsMVC.Models.Entities;
using ScottybonsMVC.Models.ViewModels;
using ScottybonsMVC.Models.ViewModels.Customer;
using Umbraco.Core;
using Umbraco.Web.Mvc;
using Umbraco.Web.Models;
using ScottybonsMVC.IcepayRestClient.Classes;
using ScottybonsMVC.IcepayRestClient.Classes.Payment;
using ScottybonsMVC.Models;

namespace ScottybonsMVC.Controllers
{

    public class OrderController : RenderMvcController
    {
        const string ORDER_PLANSCOTTYBOX = "/order/PlanScottyBox";
        const string ACCOUNT_LOGIN_NAV_ISFORORDER = "/Account/Login?Nav=IsForOrder&type=register";
        const string ORDER_THANKYOUFORYOURORDER = "/order/thankyouforyourorder/";
        const string ORDER_PAYMENT_ERROR = "/order/paymentError/";
        const string ACCOUNT_STYLEPROFILE = "/account/styleprofile/";
        private const string CREATEPROFILEQUESTION_URL = "/account/ProfileQuestions??Nav=IsForOrder";
        const string ORDER_CONFIRMYOURORDER = "/order/ConfirmYourOrder";

        ScottybonsEComEntities _scottybonsEComEntities;
        public OrderController()
        {

            if (ReferenceEquals(_scottybonsEComEntities, null))
            {
                _scottybonsEComEntities = new ScottybonsEComEntities();
            }


        }


        /// <summary>
        /// Redirect to the Login Or PlanScottybox
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                if (!Members.IsLoggedIn())
                {

                    return Redirect(GlobalConstants.LanguageUrl + ACCOUNT_LOGIN_NAV_ISFORORDER);
                }

                if (Request["Nav"] == "IsForOrder")
                {
                    return Redirect(GlobalConstants.LanguageUrl + ORDER_PLANSCOTTYBOX);
                }
                return Redirect(GlobalConstants.LanguageUrl + ACCOUNT_STYLEPROFILE);
            }
            catch (Exception ex)
            {
                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }


        }


        public ActionResult OrderSubscriptions(RenderModel model)
        {
            try
            {
                return CurrentTemplate(new OrderVm());
            }
            catch (Exception ex)
            {
                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }
        }

        [Authorize]
        // GET: Order/Create
        public ActionResult PlanScottyBox(RenderModel model)
        {

            try
            {

                //Navigate to Style Prfile
                var customerId = GetSession.CustomerId;
                // var isCustomerAnsweredQuestion = _scottybonsEComEntities.CustomerAnswers.Any(cs => cs.CustomerId == customerId);

                var isCustomerAnsweredQuestion = _scottybonsEComEntities.IsCustomerAnsweredAllRequiredQuestions(customerId, GlobalConstants.Language);
                Session["NoFilledAllQuestions"] = false;
                //No Records availabe with this customer
                if (isCustomerAnsweredQuestion > 0)
                {
                    Session["NoFilledAllQuestions"] = true;
                    return Redirect(GlobalConstants.LanguageUrl + CREATEPROFILEQUESTION_URL);
                }


                //Populate Deliver Address

                var customerOrder =
                    _scottybonsEComEntities.Orders.Where(m => m.CustomerID == customerId)
                        .OrderByDescending(c => c.OrderID)
                        .FirstOrDefault();

                //Populate Model Items
                var orderModel = GenerateOrderModel(model);

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

                    orderModel.DeliveryAddress = deliveryAddress;

                    orderModel.ContactNumber = customerOrder.ContactNumber;

                    orderModel.CustomerOrder.DeliverNeighbours = true;
                    if (customerOrder.DeliverNeighbours.HasValue)
                        orderModel.CustomerOrder.DeliverNeighbours = (customerOrder.DeliverNeighbours == true ? true : false);

                    orderModel.CustomerOrder.ContactNumber = customerOrder.ContactNumber;
                    orderModel.CustomerOrder.PeriodicalScottyBoxID = customerOrder.PeriodicalScottyBoxID;
                    orderModel.CustomerOrder.PeriodicalScottyBoxMaster = customerOrder.PeriodicalScottyBoxMaster;
                    orderModel.CustomerOrder.IsStylistContactYou = customerOrder.IsStylistContactYou;

                    ViewBag.PeriodicalScottyBoxID = customerOrder.PeriodicalScottyBoxID;
                }



                var results = _scottybonsEComEntities.CountryMasters.Distinct();

                ViewBag.Country =
                  results.Where(c => c.CountryID == 1).Select(
                        oc => new SelectListItem() { Text = oc.CountryName, Value = oc.CountryID.ToString() }).ToList();

                return CurrentTemplate(orderModel);
            }
            catch (Exception ex)
            {
                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }

        }


        [Authorize]
        // GET: Order/Create
        public ActionResult ConfirmYourOrder(RenderModel model)
        {
            try
            {
                ViewBag.PaymentTypes =
                                     _scottybonsEComEntities.PaymentTypeMasters.Where(c => c.Active != false)
                                         .OrderBy(c => c.DisplayOrder)
                                         .Select(
                                             oc => new SelectListItem() { Text = oc.PaymentTypeName, Value = oc.PaymentTypeID.ToString(), Selected = oc.PaymentTypeID.Equals(2) })
                                         .ToList();
                try
                {
                    if (!ReferenceEquals(GetIssuers(), null))
                    {
                        ViewBag.IssuerTypes = GetIssuers().Select(
                            oc => new SelectListItem() { Text = oc.Key, Value = oc.Value.ToString() })
                            .ToList();
                        ;
                    }

                }
                catch (Exception)
                {

                }

                var order = new ConfirmYourOrderVm(model.Content) { OrderNumber = GetSession.OrderNumber };
                if (GetSession.OrderNumber == 0)
                {
                    order.OrderNumber = GetSession.RequestOrderNumber;
                }
                if (order.OrderNumber > 0)
                {
                    order.CustomerId = GetSession.CustomerId;
                    return CurrentTemplate(order);
                }
                else
                {
                    //Something went wrong, navigate to error page
                    return Redirect(GlobalConstants.ErrorURL);
                }
            }
            catch (Exception ex)
            {
                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }

        }

        [Authorize]

        public ActionResult PaymentStatus(RenderModel model)
        {
            try
            {
                var orderId = 0;
                string paymentId = Request["PaymentID"];
                var isGiftCard = (Request["giftcard"] != null);
                var isPromoCode = (Request["promocode"] != null);
                var orderIdOfGiftCard = Request["orderNumber"] == null ? 0 : Convert.ToInt32(Request["orderNumber"]);

                GetPaymentResponse paymentResponse = new GetPaymentResponse();
                if (!ReferenceEquals(paymentId, null))
                {
                    //string msg = iceresponse.message;
                    paymentResponse = SavePaymentResponse(model, paymentId, isGiftCard, isPromoCode);

                    if (paymentResponse.OrderID.Contains("F"))
                    {
                        paymentResponse.OrderID = paymentResponse.OrderID.Split('F')[0];
                    }

                    var res = int.TryParse(paymentResponse.OrderID, out orderId);
                    if (paymentResponse.Status == "OK")
                    {
                        SendPaysStyleadviceEmail(orderId);
                        //Send Email to Admin for Notification
                        SendOrderNotificationToAdminWithPaymentStatus(orderId);
                        return Redirect(GlobalConstants.LanguageUrl + ORDER_THANKYOUFORYOURORDER + "?OrderNumber=" + paymentResponse.OrderID);
                    }
                    else
                    {
                        SendPaymentFailureEmail(orderId, paymentId);
                    }
                }

                if (isGiftCard || isPromoCode)
                {
                    return Redirect(GlobalConstants.LanguageUrl + ORDER_THANKYOUFORYOURORDER + "?OrderNumber=" + orderIdOfGiftCard);
                }

                return Redirect(GlobalConstants.LanguageUrl + ORDER_PAYMENT_ERROR + "?PaymentID=" + paymentId + "&OrderNumber=" + paymentResponse.OrderID);
            }
            catch (Exception ex)
            {
                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }

        }




        [Authorize]
        public ActionResult PaymentError(RenderModel model)
        {
            try
            {
                var confirmOrderVm = new ConfirmYourOrderVm(model.Content);
                string paymentId = Request["PaymentID"];
                var isGiftCard = (Request["giftcard"] != null);
                var isPromoCode = (Request["promocode"] != null);
                var paymentRequest = new GetPaymentRequest();
                if (!ReferenceEquals(paymentId, null))
                {
                    //string msg = iceresponse.message;
                    GetPaymentResponse paymentResponse = SavePaymentResponse(model, paymentId, isGiftCard, isPromoCode);

                    //SEND Failure Messag/Email to Client 
                    HttpContext.Session["OrderNumber"] = paymentResponse.OrderID;
                    TempData["OrderNumber"] = paymentResponse.OrderID;
                    SendPaymentFailureEmail(GetSession.OrderNumber, paymentId);

                    //Send Email to Admin for Notification
                    SendOrderNotificationToAdminWithPaymentStatus(GetSession.OrderNumber);

                    confirmOrderVm.OrderNumber = GetSession.OrderNumber;
                }

                return CurrentTemplate(confirmOrderVm);
            }
            catch (Exception ex)
            {
                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }

        }

        #region "Helper Methods"

        /// <summary>
        /// Save Payment Transaction Information to Database
        /// </summary>
        /// <param name="paymentResponse"></param>
        private void SavePaymentTransactionInfo(GetPaymentResponse paymentResponse)
        {


            try
            {
                var orderTransaction = new OrderIcePayTransaction();
                orderTransaction.PaymentID = paymentResponse.PaymentID;
                orderTransaction.Amount = paymentResponse.Amount;
                orderTransaction.Currency = paymentResponse.Currency;
                orderTransaction.Description = paymentResponse.Description;
                orderTransaction.Duration = paymentResponse.Duration;
                orderTransaction.ConsumerName = paymentResponse.ConsumerName;
                orderTransaction.ConsumerAccountNumber = paymentResponse.ConsumerAccountNumber;
                orderTransaction.ConsumerAddress = paymentResponse.ConsumerAddress;
                orderTransaction.ConsumerHouseNumber = paymentResponse.ConsumerHouseNumber;
                orderTransaction.ConsumerCity = paymentResponse.ConsumerCity;
                orderTransaction.ConsumerAccountNumber = paymentResponse.ConsumerCountry;
                orderTransaction.ConsumerAddress = paymentResponse.ConsumerEmail;
                orderTransaction.ConsumerHouseNumber = paymentResponse.ConsumerPhoneNumber;
                orderTransaction.ConsumerCity = paymentResponse.ConsumerIPAddress;
                orderTransaction.Issuer = paymentResponse.Issuer;
                orderTransaction.OrderID = paymentResponse.OrderID;
                orderTransaction.OrderTime = paymentResponse.OrderTime;
                orderTransaction.PaymentMethod = paymentResponse.PaymentMethod;
                orderTransaction.PaymentTime = paymentResponse.PaymentTime;
                orderTransaction.Reference = paymentResponse.Reference;
                orderTransaction.Status = paymentResponse.Status;
                orderTransaction.StatusCode = paymentResponse.StatusCode;
                orderTransaction.TestMode = paymentResponse.TestMode;
                orderTransaction.Message = paymentResponse.Message;

                _scottybonsEComEntities.OrderIcePayTransactions.Add(orderTransaction);
                _scottybonsEComEntities.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Get Issuer List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, string>> GetIssuers()
        {
            try
            {
                var list = new List<KeyValuePair<string, string>>();
                GetMyPaymentMethodsResponse paymentMethodsResponse = RestClient.SendAndReceive<GetMyPaymentMethodsRequest, GetMyPaymentMethodsResponse>(GlobalConstants.IcePayTranOperation, "GetMyPaymentMethods", new GetMyPaymentMethodsRequest(), GlobalConstants.IcePayTranMerchantId, GlobalConstants.IcePayTranMerchantSecret);

                if (ReferenceEquals(paymentMethodsResponse, null))
                { return list; }

                PaymentMethod[] paymentMethods = paymentMethodsResponse.PaymentMethods;

                foreach (PaymentMethod pymtMtd in paymentMethods)
                {
                    if (pymtMtd.PaymentMethodCode.Equals("IDEAL")) //get from config
                    {
                        Issuer[] issuers = pymtMtd.Issuers;
                        list.AddRange(
                            issuers.Select(
                                issuer => new KeyValuePair<string, string>(issuer.Description, issuer.IssuerKeyword)));
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Send Welcome Email to newly Registered Member
        /// </summary>
        private void SendPaysStyleadviceEmail(int orderNumber)
        {
            try
            {
                var customer = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID == GetSession.CustomerId);
                if (!ReferenceEquals(customer, null))
                {
                    var urlToOrder = GlobalConstants.LinkInEmail + ORDER_PLANSCOTTYBOX;
                    var emailServices = new EmailServices(_scottybonsEComEntities);
                    emailServices.SendEmailWithOutLink((int)EmailReason.PaysForStyleAdvice, customer.FirstName, customer.Email,
                        orderNumber.ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Send Payment Failure Email
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="paymentId"></param>
        private void SendPaymentFailureEmail(int orderId, string paymentId)
        {
            try
            {
                var customer = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID == GetSession.CustomerId);
                if (!ReferenceEquals(customer, null))
                {
                    var urlToOrder = HttpContext.Request.Url.Host + GlobalConstants.LanguageUrl + ORDER_CONFIRMYOURORDER + "?OrderID=" + orderId + "&PaymentId=" + paymentId;

                    urlToOrder = string.Format("<a href='{0}'>" + Resources.Scottybons.ScottybonsDataStrings.LinkClickHere + "</a>", urlToOrder);
                    var emailServices = new EmailServices(_scottybonsEComEntities);
                    emailServices.SendEmailWithLink((int)EmailReason.PaymentFailure, customer.FirstName, customer.Email,
                        urlToOrder, orderId.ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Get Payment Response
        /// </summary>
        /// <param name="model"></param>
        /// <param name="paymentId"></param>
        /// <returns />
        private GetPaymentResponse SavePaymentResponse(RenderModel model, string paymentId, bool giftCardApplied, bool promoCodeApplied)
        {

            try
            {
                int temp;
                var orderId = 0;
                var paymentRequest = new GetPaymentRequest();
                var res = int.TryParse(paymentId, out temp);
                paymentRequest.PaymentID = temp;

                _scottybonsEComEntities = new ScottybonsEComEntities();

                GetPaymentResponse paymentResponse;
                paymentResponse =
                    RestClient.SendAndReceive<GetPaymentRequest, GetPaymentResponse>(
                        GlobalConstants.IcePayTranOperation, "GetPayment", paymentRequest,
                        GlobalConstants.IcePayTranMerchantId, GlobalConstants.IcePayTranMerchantSecret);

                if (!ReferenceEquals(paymentResponse, null))
                {
                    if (paymentResponse.OrderID.Contains("F"))
                    {
                        paymentResponse.OrderID = paymentResponse.OrderID.Split('F')[0];
                    }

                    //Save Payment Response to Database
                    SavePaymentTransactionInfo(paymentResponse);

                    res = int.TryParse(paymentResponse.OrderID, out orderId);

                    var customerOrder = _scottybonsEComEntities.Orders.Where(m => m.OrderID == orderId).OrderByDescending(c => c.OrderID).FirstOrDefault();

                    //Update Payment Status
                    if (paymentResponse.Status != "ERR" && paymentResponse.Status != "OPEN")
                    {
                        customerOrder.OrderStatusID = (int)OrderStatus.PaymentSucess;
                    }
                    else
                    {
                        if (promoCodeApplied && giftCardApplied)
                        {
                            customerOrder.OrderStatusID = (int)OrderStatus.PartiallyPaidWithPromoCodeGiftCard;
                        }
                        else if (promoCodeApplied)
                        {
                            customerOrder.OrderStatusID = (int)OrderStatus.PartiallyPaidWithPromoCode;
                        }
                        else if (giftCardApplied)
                        {
                            customerOrder.OrderStatusID = (int)OrderStatus.PartiallyPaidWithGiftCard;
                        }
                        else
                        {
                            customerOrder.OrderStatusID = (int)OrderStatus.PaymentFailure;
                        }

                        //customerOrder.OrderStatusID = giftCardApplied? (int)OrderStatus.PartiallyPaidWithGiftCard: (int)OrderStatus.PaymentFailure;

                        if (giftCardApplied)
                        {
                            var gcRedemtion = _scottybonsEComEntities.GiftCardRedemptions.OrderByDescending(o => o.GiftCardRedemptionId).FirstOrDefault(gcr => gcr.OrderNumber.Trim().ToLower().Equals(paymentResponse.OrderID.Trim().ToLower()));
                            if (!ReferenceEquals(gcRedemtion, null))
                            {
                                gcRedemtion.Status = (int)GiftCardRedemptionStatusEnum.Failure;
                                gcRedemtion.StatusText = GiftCardRedemptionStatusEnum.Failure.ToString();
                                var giftCardSuccessId = (int)GiftCardOrderStatus.Completed;
                                var giftcard = (from gc in _scottybonsEComEntities.GiftCards.ToList()
                                                join gco in _scottybonsEComEntities.GIftCardsOrders.ToList()
                                                on gc.OrderNumber equals gco.OrderNumber
                                                where gco.Status.Value.Equals(giftCardSuccessId) && gc.GiftCardCode.Trim().ToLower().Equals(gcRedemtion.GiftCardCode.ToLower().Trim())
                                                select gc).FirstOrDefault();

                                //var giftcard =_scottybonsEComEntities.GiftCards.FirstOrDefault(gcr => gcr.GiftCardCode.Trim().ToLower().Equals(gcRedemtion.GiftCardCode.Trim().ToLower()));

                                if (!ReferenceEquals(giftcard, null))
                                {
                                    giftcard.CurrentGiftCardValue = gcRedemtion.AmounOfRedemption.Value;
                                    _scottybonsEComEntities.SaveChanges();
                                }
                            }
                        }

                        if (promoCodeApplied)
                        {
                            var pcOrders = _scottybonsEComEntities.PromoCodeOrders.ToList().Where(o => o.OrderId.Equals(orderId)).ToList();
                            if (!ReferenceEquals(pcOrders, null) && pcOrders.Any())
                            {
                                foreach (var pcOrder in pcOrders)
                                {
                                    pcOrder.Status = false;
                                    _scottybonsEComEntities.SaveChanges();
                                }
                            }
                        }
                    }

                    _scottybonsEComEntities.Orders.AddOrUpdate(customerOrder);
                    _scottybonsEComEntities.SaveChanges();

                    var orderModel = GenerateOrderModel(model);
                    orderModel.CustomerOrder = customerOrder;
                }
                return paymentResponse;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Generate Model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private OrderVm GenerateOrderModel(RenderModel model)
        {
            var orderModel = new OrderVm(model.Content)
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

        /// <summary>
        /// Send Welcome Email to newly Registered Member
        /// </summary>
        private void SendOrderNotificationToAdminWithPaymentStatus(int orderId)
        {
            try
            {
                var user = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID == GetSession.CustomerId);
                var order = _scottybonsEComEntities.Orders.FirstOrDefault(c => c.OrderID == orderId);
                var emailServices = new EmailServices(_scottybonsEComEntities);
                emailServices.SendOrderNotificationToAdmin((int)EmailReason.OrderNotificationMail, user, order, GlobalConstants.ScottybonCustomerSupportAdminEmail);
            }
            catch (Exception)
            {

                //Log Exception
            }


        }
        #endregion

    }
}
