using System;
using System.Data.Entity.Migrations;
using System.Web;
using Resources.Scottybons;
using ScottybonsMVC.AppConstants;
using ScottybonsMVC.IcepayRestClient.Classes;
using ScottybonsMVC.IcepayRestClient.Classes.Payment;
using ScottybonsMVC.Infrastructure;
using System.Web.Mvc;
using ScottybonsMVC.Models.ViewModels.Customer;
using Umbraco.Web.Mvc;
using System.Linq;
using ScottybonsMVC.Models.Entities;
using ScottybonsMVC.Models.ViewModels;
using ScottybonsMVC.Models;
using ScottybonsMVC.Models.PageViewModels;
using System.Collections.Generic;

namespace ScottybonsMVC.Controllers
{
    [Authorize]
    public class OrderPageController : SurfaceController
    {
        const string ORDER_CONFIRMYOURORDER = "/order/ConfirmYourOrder";
        const string ORDER_THANKYOUFORYOURORDER = "/order/thankyouforyourorder/";

        ScottybonsEComEntities _scottybonsEComEntities;
        public OrderPageController()
        {

            if (ReferenceEquals(_scottybonsEComEntities, null))
            {
                _scottybonsEComEntities = new ScottybonsEComEntities();
            }


        }

        /// <summary>
        /// Order Creation : Post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HandleScottyBoxAction(OrderVm model)
        {
            try
            {
                if (model.CustomerOrder.IsStylistContactYou != true)
                {
                    this.ModelState.Remove("ContactNumber");
                }

                if (ModelState.IsValid)
                {
                    var user = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.Email.Trim().Equals(Members.CurrentUserName));
                    var customerId = !ReferenceEquals(user, null) ? user.CustomerID : 0;
                    var occation = 0;
                    var temp = int.TryParse(model.PlanScottyboxdropdownOccesions, out occation);
                    var order = new Order
                    {
                        CustomerID = customerId,
                        OrderOccasion = occation,
                        OccasionComments = "",
                        WishList = "",
                        DateOrder = DateTime.UtcNow,
                        RequiredDate = DateTime.UtcNow,
                        PaymentMethodID = 1, //Default value; Will Replace
                        AgreeGeneralConditions = true,
                        OrderComanyName = model.DeliveryAddress.Name,
                        OrderStreet = model.DeliveryAddress.Street,
                        OrderHouseNumber = model.DeliveryAddress.Number,
                        OrderPostalCode = model.DeliveryAddress.Zip,
                        OrderTown = model.DeliveryAddress.Town,
                        OrderCountryID = model.CustomerOrder.OrderCountryID,
                        OrderComments = "",
                        DeliverNeighbours = model.CustomerOrder.DeliverNeighbours,
                        OrderStatusID = (int)OrderStatus.OrderCreated,
                        PaymentDate = DateTime.Today,
                        OrderCommentStylist = "",
                        OrderStyleAdvice = "",
                        NumberOfArticles = 0,
                        ReviewOverall = "",
                        PeriodicalScottyBoxID = model.PeriodicalScottyBoxSelected ? model.CustomerOrder.PeriodicalScottyBoxID : 0,
                        NextPerodicalScottyBoxDate = DateTime.UtcNow.AddMonths(model.CustomerOrder.PeriodicalScottyBoxID),
                        IsStylistContactYou = model.CustomerOrder.IsStylistContactYou,
                        ContactNumber = model.ContactNumber,
                        UpdatedOn = DateTime.UtcNow,
                        CreatedOn = DateTime.UtcNow
                    };
                    _scottybonsEComEntities.Orders.Add(order);
                    _scottybonsEComEntities.SaveChanges();

                    if (!(order.OrderID > 0))
                    {
                        return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode("Order Id created with zero, please contact administrator"));
                    }

                    Session["OrderNumber"] = order.OrderID.ToString();
                    //Persist Order Number
                    if (!TempData.Keys.Any(t => t.Equals("OrderNumber")))
                    {
                        TempData.Add("OrderNumber", order.OrderID);
                    }

                    //Send Email to User
                    SendOrderConfirmationEmail(user, order.OrderID.ToString());
                    //Send Email to Admin for Notification
                    SendOrderNotificationToAdmin(user, order);

                    var confirmationUrl = GlobalConstants.LanguageUrl + ORDER_CONFIRMYOURORDER;
                    return Redirect(confirmationUrl);
                }
                else
                    return CurrentUmbracoPage();
            }
            catch (Exception ex)
            {

                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }


        }

        [HttpPost]
        public ActionResult HandleSubscription(SubscriptionPageViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    var customer = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID == GetSession.CustomerId);
                    var selectedPeriodId = Convert.ToInt32(model.SelectedPeriod);
                    var selectedPeriodMonths = _scottybonsEComEntities.PeriodicalScottyBoxMasters.FirstOrDefault(p => p.PeriodicalScottyBoxID.Equals(selectedPeriodId));
                    var customerOrder =
                            _scottybonsEComEntities.Orders.Where(m => m.CustomerID == customer.CustomerID)
                                .OrderByDescending(c => c.OrderID)
                                .FirstOrDefault();

                    var periodicOrderSubscription = new OrderPeriodicSubscriptionDetail
                    {
                        CustomerID = GetSession.CustomerId,
                        CreatedDate = DateTime.Now.Date,
                        OrderId = customerOrder.OrderID,
                        PeriodicalScottyBoxID = customerOrder.PeriodicalScottyBoxID,
                        NewPeriodicalScottyBoxID = model.IsSubscription ? selectedPeriodId : 0,
                        ChangedByAdmin = false
                    };

                    customerOrder.PeriodicalScottyBoxID = model.IsSubscription ? selectedPeriodId : 0;
                    customerOrder.OrderOccasion = Convert.ToInt32(model.OrderContent.PlanScottyboxdropdownOccesions);
                    customerOrder.ContactNumber = model.OrderContent.ContactNumber;
                    customerOrder.OrderComanyName = model.OrderContent.DeliveryAddress.Name;
                    customerOrder.OrderStreet = model.OrderContent.DeliveryAddress.Street;
                    customerOrder.OrderHouseNumber = model.OrderContent.DeliveryAddress.Number;
                    customerOrder.OrderPostalCode = model.OrderContent.DeliveryAddress.Zip;
                    customerOrder.OrderTown = model.OrderContent.DeliveryAddress.Town;
                    customerOrder.OrderCountryID = model.OrderContent.CustomerOrder.OrderCountryID;
                    customerOrder.DeliverNeighbours = model.StylistContactType.ToLower().Equals("true") ? true : false;
                    customerOrder.NextPerodicalScottyBoxDate = model.IsSubscription
                        ? (customerOrder.NextPerodicalScottyBoxDate.Value.Date > DateTime.Now.Date)
                            ? customerOrder.NextPerodicalScottyBoxDate.Value.AddMonths(selectedPeriodMonths.PerodicalMonths.Value)
                            : DateTime.Now.Date.AddMonths(selectedPeriodMonths.PerodicalMonths.Value)
                        : customerOrder.NextPerodicalScottyBoxDate;

                    _scottybonsEComEntities.SaveChanges();

                    _scottybonsEComEntities.OrderPeriodicSubscriptionDetails.Add(periodicOrderSubscription);
                    _scottybonsEComEntities.SaveChanges();
                }
                return CurrentUmbracoPage();
            }
            catch (Exception ex)
            {

                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }
        }

        [HttpPost]
        public ActionResult ConfirmYourOrder(ConfirmYourOrderVm model)
        {
            try
            {
                bool icePayPaymentError = !ReferenceEquals(Request["PaymentId"], null);
                var ordNumber = GetSession.OrderNumber;
                //Navigation from Order Failure conditions
                if (!(ordNumber > 0) && !(ReferenceEquals(Request["OrderID"], null)))
                {
                    var res = int.TryParse(Request["OrderID"], out ordNumber);
                }
                else if ((!(ordNumber > 0) && !(ReferenceEquals(Request["OrderNumber"], null))))
                {
                    var res = int.TryParse(Request["OrderNumber"], out ordNumber);
                }
                if (ordNumber == 0 || !(ModelState.IsValid))
                {
                    ModelState.AddModelError("", ScottybonsDataStrings.OrderPageController_ConfirmYourOrder_Invalid_OrderNumber);
                    return Redirect(GlobalConstants.ErrorURL + ScottybonsDataStrings.OrderPageController_ConfirmYourOrder_Invalid_OrderNumber);
                }
                var orderString = "F";
                if (icePayPaymentError)
                {
                    int temp;
                    var paymentRequest = new GetPaymentRequest();
                    var res = int.TryParse(Request["PaymentId"], out temp);
                    paymentRequest.PaymentID = temp;

                    GetPaymentResponse paymentResponse;
                    paymentResponse =
                        RestClient.SendAndReceive<GetPaymentRequest, GetPaymentResponse>(
                            GlobalConstants.IcePayTranOperation, "GetPayment", paymentRequest,
                            GlobalConstants.IcePayTranMerchantId, GlobalConstants.IcePayTranMerchantSecret);

                    var uniqueString = 0;

                    if (paymentResponse.OrderID.Contains('F'))
                    {
                        //var OrderId = paymentResponse.OrderID.Split('F')[0];
                        int temp1 = 0;
                        var res1 = int.TryParse(paymentResponse.OrderID.Split('F')[1], out temp1);
                        uniqueString = temp1 + 1;
                    }
                    else
                    {
                        uniqueString = 1;
                    }
                    orderString = orderString + uniqueString;
                }

                var orderModel = _scottybonsEComEntities.Orders.FirstOrDefault(c => c.OrderID == (int)ordNumber);
                orderModel.PaymentMethodID = model.PaymentMethodId;
                orderModel.AgreeGeneralConditions = true;
                orderModel.OrderStatusID = (int)OrderStatus.PaymentProcessInitiated;
                orderModel.PermissionToCollectFutureInvoices = model.PermissionToCollectFutureInvoices;


                _scottybonsEComEntities.Orders.AddOrUpdate(orderModel);
                _scottybonsEComEntities.SaveChanges();

                //Place holder to Payment Module
                // model.PaymentMethodId
                var transactionAmount = Convert.ToDouble(GlobalConstants.IcePayTranInitialAmount);

                var outPutUrl = string.Format("http://{0}?giftcard={1}&promocode={2}&orderNumber={3}",
                            (HttpContext.Request.Url.Host + GlobalConstants.LanguageUrl + GlobalConstants.IcePayTranSuccess), model.GiftCardRedeemed, model.PromoCodeRedeemed, orderModel.OrderID);

                if (model.PaymentMethodId.Equals((int)OrderPaymentTypeEnum.GiftCard) || model.PaymentMethodId.Equals((int)OrderPaymentTypeEnum.PromoCode))
                {
                    if (Convert.ToBoolean(model.PromoCodeRedeemed))
                    {
                        if (!string.IsNullOrEmpty(model.PromoCodes))
                        {
                            orderModel.PromoCode =string.Join(",", model.PromoCodes.Split(',').Select(s => s.Split(':').FirstOrDefault()));
                            orderModel.OrderStatusID = (int)OrderStatus.PaidWithPromoCode;

                            _scottybonsEComEntities.Orders.AddOrUpdate(orderModel);
                            _scottybonsEComEntities.SaveChanges();

                            foreach (var promocode in model.PromoCodes.Split(','))
                            {
                                var splitPromoCodeNameValue = promocode.Split(':');
                                var promoCodeObject = ((PromoCodeModel)ValidatePromoCode(splitPromoCodeNameValue[0]).Data);

                                var promoCodeOrder = new PromoCodeOrder
                                {
                                    PromoCode = splitPromoCodeNameValue[0],
                                    CreatedDate = DateTime.Now.Date,
                                    OrderId = orderModel.OrderID,
                                    CustomerID = GetSession.CustomerId,
                                    PromoCodeValue =Convert.ToDouble(splitPromoCodeNameValue[1]),
                                    IsPercentage = promoCodeObject.IsPercentage,
                                    Status = true
                                };
                                _scottybonsEComEntities.PromoCodeOrders.Add(promoCodeOrder);
                                _scottybonsEComEntities.SaveChanges();

                                transactionAmount = (transactionAmount - (Math.Round(Convert.ToDouble(splitPromoCodeNameValue[1]) * 100) / 100));
                            }
                        }
                    }

                    if (model.GiftCardRedeemed.ToLower().Equals("true"))
                    {
                        var gcRedeemedValue = (Math.Round(Convert.ToDouble(model.GiftCardRedeemedValue) * 100) / 100);

                        var giftCard = (from gc in _scottybonsEComEntities.GiftCards.ToList()
                                        join gco in _scottybonsEComEntities.GIftCardsOrders.ToList()
                                        on gc.OrderNumber equals gco.OrderNumber
                                        where gco.Status.Value.Equals(2) && gc.GiftCardCode.Trim().ToLower().Equals(model.GiftCardCode.ToLower().Trim())
                                        select gc).FirstOrDefault();

                        if (!ReferenceEquals(giftCard, null))
                        {
                            orderModel.OrderStatusID = (int)OrderStatus.PaidWithGiftCard;

                            var giftCardRedemption = new GiftCardRedemption
                            {
                                AmounOfRedemption = gcRedeemedValue,
                                CustomerId = orderModel.CustomerID,
                                GiftCardCode = model.GiftCardCode,
                                OrderNumber = orderModel.OrderID.ToString(),
                                RedemptionDateTime = DateTime.Now,
                                Status = (int)GiftCardRedemptionStatusEnum.Success,
                                StatusText = GiftCardRedemptionStatusEnum.Success.ToString()
                            };

                            giftCard.CurrentGiftCardValue = (Math.Round((giftCard.CurrentGiftCardValue- gcRedeemedValue) * 100) / 100);
                            _scottybonsEComEntities.GiftCardRedemptions.Add(giftCardRedemption);
                            _scottybonsEComEntities.SaveChanges();

                            //return Redirect(outPutUrl);
                        }
                    }

                    orderModel.OrderStatusID = (Convert.ToBoolean(model.PromoCodeRedeemed) && Convert.ToBoolean(model.GiftCardRedeemed))
                        ? (int)OrderStatus.PaidWithPromoCodeGiftCard
                        : orderModel.OrderStatusID;
                    _scottybonsEComEntities.SaveChanges();

                    return Redirect(outPutUrl);
                }
                else
                {
                    if (model.GiftCardRedeemed.ToLower().Equals("true") || model.PromoCodeRedeemed.ToLower().Equals("true"))
                    {
                        //gift card functionality
                        if (model.GiftCardRedeemed.ToLower().Equals("true"))
                        {
                            var giftCard = (from gc in _scottybonsEComEntities.GiftCards.ToList()
                                            join gco in _scottybonsEComEntities.GIftCardsOrders.ToList()
                                            on gc.OrderNumber equals gco.OrderNumber
                                            where gco.Status.Value.Equals(2) && gc.GiftCardCode.Trim().ToLower().Equals(model.GiftCardCode.ToLower().Trim())
                                            select gc).FirstOrDefault();
                            //todo
                            //create GiftCardRedemption record (use if error occurs to roll back the amount redeemed for the order)
                            if (!ReferenceEquals(giftCard, null))
                            {
                                //transactionAmount = (transactionAmount - giftCard.CurrentGiftCardValue);
                                var gcRedeemedValue = (Math.Round(Convert.ToDouble(model.GiftCardRedeemedValue) * 100) / 100);

                                transactionAmount = (transactionAmount - gcRedeemedValue);
                                var giftCardRedemption = new GiftCardRedemption
                                {
                                    AmounOfRedemption = gcRedeemedValue,
                                    CustomerId = orderModel.CustomerID,
                                    GiftCardCode = model.GiftCardCode,
                                    OrderNumber = orderModel.OrderID.ToString(),
                                    RedemptionDateTime = DateTime.Now,
                                    Status = (int)GiftCardRedemptionStatusEnum.Success,
                                    StatusText = GiftCardRedemptionStatusEnum.Success.ToString()
                                };
                                _scottybonsEComEntities.GiftCardRedemptions.Add(giftCardRedemption);

                                giftCard.CurrentGiftCardValue = (Math.Round((giftCard.CurrentGiftCardValue - gcRedeemedValue) * 100) / 100); 
                                _scottybonsEComEntities.SaveChanges();
                            }
                        }

                        //promo code functionality
                        if (model.PromoCodeRedeemed.ToLower().Equals("true"))
                        {
                            if (!string.IsNullOrEmpty(model.PromoCodes))
                            {
                                orderModel.PromoCode = model.PromoCodes;

                                _scottybonsEComEntities.Orders.AddOrUpdate(orderModel);
                                _scottybonsEComEntities.SaveChanges();

                                foreach (var promocode in model.PromoCodes.Split(','))
                                {
                                    var splitPromoCodeNameValue = promocode.Split(':');
                                    var promoCodeObject = ((PromoCodeModel)ValidatePromoCode(splitPromoCodeNameValue[0]).Data);

                                    //need to check percentage and offline type
                                    //transactionAmount = (transactionAmount - promoCodeObject.PromoCodeValue);
                                    transactionAmount = (transactionAmount - (Math.Round(Convert.ToDouble(splitPromoCodeNameValue[1]) * 100) / 100));

                                    var promoCodeOrder = new PromoCodeOrder
                                    {
                                        PromoCode = splitPromoCodeNameValue[0],
                                        CreatedDate = DateTime.Now.Date,
                                        OrderId = orderModel.OrderID,
                                        CustomerID = GetSession.CustomerId,
                                        PromoCodeValue =Convert.ToDouble(splitPromoCodeNameValue[1]),
                                        IsPercentage = promoCodeObject.IsPercentage,
                                        Status=true
                                    };
                                    _scottybonsEComEntities.PromoCodeOrders.Add(promoCodeOrder);
                                    _scottybonsEComEntities.SaveChanges();
                                }
                            }
                        }
                    }


                    var checkOutRequest = new ScottybonsMVC.IcepayRestClient.Classes.Payment.CheckoutRequest
                    {
                        Amount = Convert.ToInt32(transactionAmount * 100),
                        Country = orderModel.CountryMaster.TwoLetterISOCode.Trim(),
                        Description =
                        GlobalConstants.IcePayTranDescription + "" + orderModel.Customer.FirstName + "" +
                        orderModel.Customer.LastName + "" + orderModel.Customer.Email + "",
                        EndUserIP = HttpContext.Request.UserHostAddress,
                        Issuer = model.SelectIssuer,
                        Language = GlobalConstants.Language.ToUpper().Trim(),
                        OrderID = icePayPaymentError ? orderModel.OrderID.ToString() + orderString : orderModel.OrderID.ToString(),
                        PaymentMethod = orderModel.PaymentTypeMaster.PaymentTypeName.ToUpper().Trim(),
                        Reference =
                        GlobalConstants.IcePayTranDescription + ": " + orderModel.Customer.FirstName + " " +
                        orderModel.Customer.LastName,
                        URLCompleted =
                        "http://" + HttpContext.Request.Url.Host + GlobalConstants.LanguageUrl +
                        GlobalConstants.IcePayTranSuccess,
                        URLError =
                        "http://" + HttpContext.Request.Url.Host + GlobalConstants.LanguageUrl +
                        GlobalConstants.IcePayTranError + (model.GiftCardRedeemed.ToLower().Equals("true") ? ("?giftcard=" + model.GiftCardRedeemed) : string.Empty)
                        + (model.PromoCodeRedeemed.ToLower().Equals("true") ? ("&promocode=" + model.PromoCodeRedeemed) : string.Empty),
                        Currency = GlobalConstants.IcePayTranCurrency
                    };

                    //Call RestClient
                    CheckoutResponse iceResponse =
                        RestClient.SendAndReceive<CheckoutRequest, CheckoutResponse>(
                            GlobalConstants.IcePayTranOperation, GlobalConstants.IcePayTranService,
                            checkOutRequest, GlobalConstants.IcePayTranMerchantId,
                            GlobalConstants.IcePayTranMerchantSecret);

                    int paymentId = iceResponse.PaymentID;
                    return Redirect(iceResponse.PaymentScreenURL);
                }
            }
            catch (Exception ex)
            {

                return Redirect(GlobalConstants.ErrorURL + HttpUtility.UrlEncode(ex.Message));
            }



        }

        public JsonResult CheckGiftCardCode(string giftCardCode)
        {
            try
            {
                var giftCardObject = (from gc in _scottybonsEComEntities.GiftCards.ToList()
                                      join gco in _scottybonsEComEntities.GIftCardsOrders.ToList()
                                      on gc.OrderNumber equals gco.OrderNumber
                                      where gco.Status.Value.Equals(2) && gc.GiftCardCode.Trim().ToLower().Equals(giftCardCode.ToLower().Trim())
                                      select new GiftCardDetailsModal
                                      {
                                          OrginalAmount = gc.GiftCardValue,
                                          CurrentBalance = gc.CurrentGiftCardValue,
                                          ExpireDate = gc.ExpirationDate,
                                          Expired = gc.ExpirationDate < DateTime.Today,
                                          Exists = true
                                      }).FirstOrDefault();
                var modal = ReferenceEquals(giftCardObject, null) ? new GiftCardDetailsModal() : giftCardObject;
                return Json(modal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new GiftCardDetailsModal(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ValidatePromoCode(string promoCode)
        {
            try
            {
                var currentNode = Umbraco.TypedContent(2454);
                var promoCodeObject = currentNode.Children.FirstOrDefault(a => a.Name.ToLower().Trim().Equals(promoCode.ToLower().Trim()));

                if (ReferenceEquals(promoCodeObject, null))
                {
                    return Json(new PromoCodeModel(), JsonRequestBehavior.AllowGet);
                }
                else if (!Convert.ToBoolean(promoCodeObject.GetProperty("isEnable").Value))
                {
                    return Json(new PromoCodeModel(), JsonRequestBehavior.AllowGet);
                }

                var promoCodeModel = new PromoCodeModel
                {
                    PromoCode = promoCodeObject.GetProperty("promoCodeName").Value.ToString(),
                    PromoCodeValue = Convert.ToInt32(promoCodeObject.GetProperty("promoCodeValue").Value),
                    AllowAllusers = Convert.ToBoolean(promoCodeObject.GetProperty("allowAllUsers").Value),
                    OnlyOnce = Convert.ToBoolean(promoCodeObject.GetProperty("onlyOnce").Value),
                    IsOnline = Convert.ToBoolean(promoCodeObject.GetProperty("isOnline").Value),
                    IsPercentage = Convert.ToBoolean(promoCodeObject.GetProperty("isPercentage").Value),
                    IsEnable = Convert.ToBoolean(promoCodeObject.GetProperty("isEnable").Value),
                    StartDate = Convert.ToDateTime(promoCodeObject.GetProperty("startDate").Value),
                    EndDate = Convert.ToDateTime(promoCodeObject.GetProperty("endDate").Value),
                    Expired = Convert.ToDateTime(promoCodeObject.GetProperty("endDate").Value).Date < DateTime.Now.Date,
                    Valid = true
                };

                var successStatusIds = new List<int>();
                successStatusIds.Add((int)OrderStatus.PaymentSucess);
                successStatusIds.Add((int)OrderStatus.AutoCreated);
                successStatusIds.Add((int)OrderStatus.PaidWithGiftCard);
                successStatusIds.Add((int)OrderStatus.PaidWithPromoCode);

                var orderSuccessId = (int)OrderStatus.PaymentSucess;

                var promoCodeExistsStatus = false;

                if (!promoCodeModel.AllowAllusers)
                {
                    // promoCodeExistsStatus = _scottybonsEComEntities.Orders.Any(po => po.PromoCode!=null 
                    // && po.PromoCode.Trim().ToLower().Split(',').Any(s=>s.Split(':')[0].Equals(promoCode.Trim().ToLower()))
                    //&& po.OrderStatusID.Equals(orderSuccessId));

                    promoCodeExistsStatus = _scottybonsEComEntities.Orders.ToList().Any(po => po.PromoCode != null
                        && PromoCodeExists(po.PromoCode, promoCode)
                       && po.OrderStatusID.Equals(orderSuccessId));

                    //if (promoCodeExistsStatus)
                    //{
                    //    promoCodeModel.Valid = false;
                    //    promoCodeModel.Error = true;
                    //    promoCodeModel.Message = "Promo Code is Already used, Please use other promo code.";
                    //}

                    //return Json(promoCodeModel, JsonRequestBehavior.AllowGet);
                }

                if (!promoCodeExistsStatus && promoCodeModel.OnlyOnce)
                {
                    promoCodeExistsStatus = _scottybonsEComEntities.Orders.ToList()
                        .Any(po => po.PromoCode != null && PromoCodeExists(po.PromoCode, promoCode)
                        && po.CustomerID != null
                        && po.CustomerID.Value.Equals(GetSession.CustomerId)
                        && po.OrderStatusID != null && !successStatusIds.Any(id => id.Equals(po.OrderStatusID.Value)));

                    //if (promoCodeExistsStatus)
                    //{
                    //    promoCodeModel.Valid = false;
                    //    promoCodeModel.Error = true;
                    //    promoCodeModel.Message = "Promo Code is Already used, Please use other promo code.";
                    //}
                }

                if (promoCodeExistsStatus)
                {
                    promoCodeModel.Valid = false;
                    promoCodeModel.Error = true;
                    promoCodeModel.Message = "Promo Code is Already used, Please use other promo code.";
                }
                return Json(promoCodeModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new PromoCodeModel { Error = true, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool PromoCodeExists(string promoCodes,string promoCode)
        {
            return promoCodes.Split(',').Any(s => s.ToLower().Trim().Equals(promoCode.ToLower().Trim()));            
        }

        /// <summary>
        /// Send Welcome Email to newly Registered Member
        /// </summary>
        private void SendOrderConfirmationEmail(Customer user, string ordNumber)
        {
            var emailServices = new EmailServices(_scottybonsEComEntities);
            emailServices.SendEmailWithOutLink((int)EmailReason.PlacesAnOrder, user.FirstName, user.Email, ordNumber);

        }


        /// <summary>
        /// Send Welcome Email to newly Registered Member
        /// </summary>
        private void SendOrderNotificationToAdmin(Customer user, Order order)
        {
            var emailServices = new EmailServices(_scottybonsEComEntities);
            emailServices.SendOrderNotificationToAdmin((int)EmailReason.OrderNotificationMail, user, order, GlobalConstants.ScottybonCustomerSupportAdminEmail);

        }
    }
}
