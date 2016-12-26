using ScottybonsMVC.AppConstants;
using ScottybonsMVC.IcepayRestClient.Classes;
using ScottybonsMVC.IcepayRestClient.Classes.Payment;
using ScottybonsMVC.Models.Entities;
using ScottybonsMVC.Models.PageViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.IO;

namespace ScottybonsMVC.Models.Services
{
    public class GiftCardServices
    {
        ScottybonsEComEntities _scottybonsEComEntities;
        public GiftCardServices()
        {
            if (ReferenceEquals(_scottybonsEComEntities, null))
            {
                _scottybonsEComEntities = new ScottybonsEComEntities();
            }
        }

        public GiftCardModel HandleGiftCard(GiftCardModel model)
        {
            try
            {
                var orderNumber = GetCustomerGiftCardOrderNumberSequence();
                var customerType = model.IsGuest ? (long)GiftCardCustonerType.Guest : (long)GiftCardCustonerType.Member;
                var customerId = model.IsGuest ? 0 : Convert.ToInt32(HttpContext.Current.Session["CustomerID"]);
                if (!model.IsGuest)//by ram
                {
                    var customerDtlForEmail = _scottybonsEComEntities.Customers.FirstOrDefault(c => c.CustomerID == GetSession.CustomerId);
                    if (model.Email == null)
                    {
                        model.Email = customerDtlForEmail.Email;
                    }
                }//end
                Random r = new Random();
                model.RandomOrderSuffixer = r.Next().ToString();
                //string path = HttpContext.Current.Server.MapPath("~/ErrorLog/errorlog.txt");
                //var message = string.Empty;

                if (!string.IsNullOrEmpty(orderNumber))
                {
                    //Create Gift Card Order
                    model.OrderNumber = orderNumber;
                    //logging START
                    //message = "\n START SESSION *******************************\n=== Total at HandelGiftcard service === \n";
                    //message += "=== " + model.OrderTotal + " === \n";

                    //using (StreamWriter writer = new StreamWriter(path, true))
                    //{
                    //    writer.WriteLine(message);
                    //    writer.Close();
                    //}
                    //logging END
                    var giftCardOrder = new GIftCardsOrder
                    {
                        CustomerId = customerId,
                        CustomerType = customerType,
                        Status = (int)GiftCardOrderStatus.Created,
                        Email = model.Email,
                        PayerFirstName = model.ShipmentDetails.PayersFirstName,
                        PayerLastName = model.ShipmentDetails.PayersLastName,
                        ShipmentFirstName = model.ShipmentDetails.FirstName,
                        ShipmentLastName = model.ShipmentDetails.LastName,
                        StreetName = model.ShipmentDetails.StreetName,
                        HouseNumber = model.ShipmentDetails.HouseNumber,
                        OrderNumber = orderNumber,
                        PostalCode = model.ShipmentDetails.PostalCode,
                        Town = model.ShipmentDetails.City,
                        Country = model.ShipmentDetails.Country,
                        IssuerType = model.IssuerType,
                        VendorId = model.VendorId,
                        OrderBy = (int)GiftCardOrderByTypeEnum.Customer,
                        OrderTotal = model.GiftCards.Sum(gc => Convert.ToDouble(gc.Value)),
                        RandomOrderSuffixer = model.RandomOrderSuffixer
                    };

                    _scottybonsEComEntities.GIftCardsOrders.Add(giftCardOrder);
                    _scottybonsEComEntities.SaveChanges();

                    //message += "=== Giftcards Save Section Loop Start === \n";

                    foreach (var giftcard in model.GiftCards)
                    {
                        var giftCode = GenerateGiftCardCode(giftcard.Value.ToString()).Trim();

                        //message += "=== Giftcard Item Start === \n";
                        //message += "=== Giftcard Item Value Before Convertion === \n";
                        //message += "=== " + giftcard.Value + " === \n";

                        var giftCardDb = new GiftCard
                        {

                            CreatedDate = DateTime.Now,
                            ExpirationDate = DateTime.Now.AddYears(2),
                            GiftCardCode = giftCode,
                            Name = giftcard.Name,
                            GiftCardValue = Convert.ToDouble(giftcard.Value),
                            CurrentGiftCardValue = Convert.ToDouble(giftcard.Value),
                            PersonalMessage = giftcard.PersonalMessage,
                            Email = model.Email,
                            OrderNumber = orderNumber
                        };
                        //message += "=== Giftcard Item Value After Convertion === \n";
                        //message += "=== " + giftCardDb.GiftCardValue.ToString() + " === \n";

                        _scottybonsEComEntities.GiftCards.Add(giftCardDb);
                        _scottybonsEComEntities.SaveChanges();

                        //message += "=== Giftcard Item End === \n";
                    }
                   //message += "=== Giftcards Save Section Loop End === \n";
                    model.Status = true;
                }
                else
                {

                    model.ErrorMsg = "Error Occured. Please contact Administrator.";
                }
                //path = HttpContext.Current.Server.MapPath("~/ErrorLog/errorlog.txt");
                //using (StreamWriter writer = new StreamWriter(path, true))
                //{
                //    writer.WriteLine(message);
                //    writer.Close();
                //}
                //logging END
                return model;
            }
            catch (Exception ex)
            {
                model.ErrorMsg = "Error Occured. Please contact Administrator.";
                return model;
            }
        }


        public GiftCardModel GiftCardReOrderPayment(GiftCardModel orderModal)
        {
            try
            {
                var orderDetails = _scottybonsEComEntities.GIftCardsOrders.FirstOrDefault(o => o.OrderNumber.Trim().Equals(orderModal.OrderNumber.Trim()));
                if (!ReferenceEquals(orderDetails, null))
                {
                    var newOrderNumber = GetCustomerGiftCardOrderNumberSequence();

                    //logging START
                    //var message = "\n*******************************\n=== Total at 'GiftCardReOrderPayment' service === \n";
                    //message += orderModal.OrderTotal;
                    //string path = HttpContext.Current.Server.MapPath("~/ErrorLog/errorlog.txt");
                    //using (StreamWriter writer = new StreamWriter(path, true))
                    //{
                    //    writer.WriteLine(message);
                    //    writer.Close();
                    //}
                    //logging END

                    var newOrder = new GIftCardsOrder
                    {
                        CustomerId = orderDetails.CustomerId,
                        CustomerType = orderDetails.CustomerType,
                        Status = (int)GiftCardOrderStatus.Created,
                        Email = orderDetails.Email,
                        PayerFirstName = orderDetails.PayerFirstName,
                        PayerLastName = orderDetails.PayerLastName,
                        ShipmentFirstName = orderDetails.ShipmentFirstName,
                        ShipmentLastName = orderDetails.ShipmentLastName,
                        StreetName = orderDetails.StreetName,
                        HouseNumber = orderDetails.HouseNumber,
                        PostalCode = orderDetails.PostalCode,
                        Town = orderDetails.Town,
                        Country = orderDetails.Country,
                        IssuerType = orderDetails.IssuerType,
                        VendorId = orderDetails.VendorId,
                        OrderBy = orderDetails.OrderBy,
                        OrderTotal = orderDetails.OrderTotal,
                        RandomOrderSuffixer = orderDetails.RandomOrderSuffixer
                    };

                    newOrder.OrderNumber = newOrderNumber;
                    _scottybonsEComEntities.GIftCardsOrders.Add(newOrder);
                    _scottybonsEComEntities.SaveChanges();

                    var orderGiftCards = _scottybonsEComEntities.GiftCards.Where(gc => gc.OrderNumber.ToLower().Trim().Equals(orderDetails.OrderNumber.ToLower().Trim())).ToList();
                    foreach (var orderGiftCard in orderGiftCards)
                    {
                        var newOrderGiftCard = new GiftCard
                        {
                            CreatedDate = DateTime.Now,
                            ExpirationDate = orderGiftCard.ExpirationDate,
                            GiftCardCode = orderGiftCard.GiftCardCode,
                            Name = orderGiftCard.Name,
                            GiftCardValue = (Math.Round(orderGiftCard.GiftCardValue * 100) / 100),
                            CurrentGiftCardValue = (Math.Round(orderGiftCard.CurrentGiftCardValue * 100) / 100),
                            PersonalMessage = orderGiftCard.PersonalMessage,
                            Email = orderGiftCard.Email,
                            OrderNumber = newOrderNumber

                        };

                        newOrderGiftCard.OrderNumber = newOrderNumber;
                        _scottybonsEComEntities.GiftCards.Add(newOrderGiftCard);
                        _scottybonsEComEntities.SaveChanges();

                        //logging START
                        //message += "\n=== Giftcards foreach 'GiftCardReOrderPayment'  === \n";
                        //message += orderGiftCard.GiftCardValue;
                        //path = HttpContext.Current.Server.MapPath("~/ErrorLog/errorlog.txt");
                        //using (StreamWriter writer = new StreamWriter(path, true))
                        //{
                        //    writer.WriteLine(message);
                        //    writer.Close();
                        //}
                        //logging END
                    }

                    orderModal.OrderNumber = newOrderNumber;
                    orderModal.OrderTotal = newOrder.OrderTotal.ToString();
                    orderModal.Email = orderDetails.Email;
                    orderModal.OrderNumber = newOrderNumber;

                    //logging START
                    //message += "\n=== Ordertotal after foreach 'GiftCardReOrderPayment'  === \n";
                    //message += orderModal.OrderTotal;
                    //path = HttpContext.Current.Server.MapPath("~/ErrorLog/errorlog.txt");
                    //using (StreamWriter writer = new StreamWriter(path, true))
                    //{
                    //    writer.WriteLine(message);
                    //    writer.Close();
                    //}
                    //logging END

                    orderModal.ShipmentDetails = new GiftCardShipmentObject
                    {
                        FirstName = newOrder.ShipmentFirstName,
                        LastName = newOrder.ShipmentLastName,
                        PayersFirstName = newOrder.PayerFirstName,
                        PayersLastName = newOrder.PayerLastName
                    };
                    orderModal = MakePayment(orderModal);
                }
                return orderModal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public GiftCardModel GiftCardPayment(GiftCardModel model)
        {
            return MakePayment(model);
        }

        private GiftCardModel MakePayment(GiftCardModel model)
        {
            try
            {
                var url = HttpContext.Current.Request.UrlReferrer.AbsoluteUri;
                var currentLanguage = string.Empty;
                if (url.Length >= 3)
                {
                    currentLanguage = url.Split('/')[3];
                }

                if (string.IsNullOrEmpty(currentLanguage))
                {
                    currentLanguage = "en";
                }

                if (currentLanguage.ToLower().Contains("en") || currentLanguage.ToLower().Contains(("nl")))
                {
                    GlobalConstants.Language = currentLanguage;

                    //var culture = "nl-" + currentLanguage ?? "nl-NL";
                    //var ci = CultureInfo.GetCultureInfo(culture);

                    //Thread.CurrentThread.CurrentCulture = ci;
                    //Thread.CurrentThread.CurrentUICulture = ci;

                    //var cookie = new HttpCookie("culture", ci.Name);
                    //Response.Cookies.Add(cookie);
                }

                var checkOutRequest = new ScottybonsMVC.IcepayRestClient.Classes.Payment.CheckoutRequest
                {
                    Amount = Convert.ToInt32(Convert.ToDecimal(model.OrderTotal) * 100),
                    Country = "NL",
                    Description =
                       GlobalConstants.IcePayTranGiftCardDescription + "" + model.ShipmentDetails.PayersFirstName + "" +
                      model.ShipmentDetails.PayersLastName + "" + model.Email + "",
                    EndUserIP = HttpContext.Current.Request.UserHostAddress,
                    Issuer = model.IssuerType,
                    Language = "NL", //GlobalConstants.Language.ToUpper().Trim(),
                    OrderID = model.OrderNumber,
                    PaymentMethod = _scottybonsEComEntities.PaymentTypeMasters.FirstOrDefault(p => p.PaymentTypeID.Equals(model.VendorId)).PaymentTypeName.ToUpper().Trim(),
                    Reference =
                       GlobalConstants.IcePayTranGiftCardDescription + ": " + model.ShipmentDetails.PayersFirstName + " " +
                       model.ShipmentDetails.PayersLastName,

                    //URLCompleted = "http://61.12.8.212/ScottyBonsPhase2Test/en/giftcard",
                    URLCompleted = "http://" + HttpContext.Current.Request.Url.Host + GlobalConstants.LanguageUrl + GlobalConstants.IcePayGiftCardTranSuccess,

                    //URLError = "http://61.12.8.212/ScottyBonsPhase2Test/en/giftcard",
                    URLError = "http://" + HttpContext.Current.Request.Url.Host + GlobalConstants.LanguageUrl + GlobalConstants.IcePayGiftCardTranError,

                    Currency = GlobalConstants.IcePayTranCurrency
                };

                //logging START
                //var message = "\n*******************************\n=== Total at 'MakePayment' after checkoutRequest === \n";
                //message += Convert.ToInt32(Convert.ToDecimal(model.OrderTotal) * 100);
                //string path = HttpContext.Current.Server.MapPath("~/ErrorLog/errorlog.txt");
                //using (StreamWriter writer = new StreamWriter(path, true))
                //{
                //    writer.WriteLine(message);
                //    writer.Close();
                //}
                //logging END

                //Call RestClient
                CheckoutResponse iceResponse =
                    RestClient.SendAndReceive<CheckoutRequest, CheckoutResponse>(
                        GlobalConstants.IcePayTranOperation, GlobalConstants.IcePayTranService,
                        checkOutRequest, GlobalConstants.IcePayTranMerchantId,
                        GlobalConstants.IcePayTranMerchantSecret);

                model.PaymentUrl = iceResponse.PaymentScreenURL;

                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GenerateOrderNumber()
        {
            try
            {
                var newOrderNumber = GenerateCode(GlobalConstants.CustomerGiftCardOrderNumberPrefixer, string.Empty).Trim();

                if (!ReferenceEquals(_scottybonsEComEntities.GIftCardsOrders.FirstOrDefault(gc => gc.OrderNumber.Trim().Equals(newOrderNumber)), null))
                {
                    newOrderNumber = GenerateOrderNumber();
                }
                return newOrderNumber;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string GenerateGiftCardCode(string giftCardCode)
        {
            try
            {
                var newGiftCardCode = GenerateCode("GC", giftCardCode).Trim();
                if (!ReferenceEquals(_scottybonsEComEntities.GiftCards.FirstOrDefault(gc => gc.GiftCardCode.Trim().Equals(newGiftCardCode)), null))
                {
                    newGiftCardCode = GenerateGiftCardCode(newGiftCardCode);
                }
                return newGiftCardCode;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GetCustomerGiftCardOrderNumberSequence()
        {
            var orderBy = (int)GiftCardOrderByTypeEnum.Customer;
            var gcLatestOrderObject = _scottybonsEComEntities.GIftCardsOrders.ToList().OrderBy(gc => gc.GiftCardOrderId).LastOrDefault(gcw => gcw.OrderBy.Value.Equals(orderBy));
            var orderNumber = string.Empty;

            if (ReferenceEquals(gcLatestOrderObject, null))
            {
                orderNumber = "1";
            }
            else
            {
                orderNumber = (Convert.ToInt32(gcLatestOrderObject.OrderNumber.Replace(GlobalConstants.CustomerGiftCardOrderNumberPrefixer, "").Replace(GlobalConstants.AdminGiftCardOrderNumberPrefixer, "")) + 1).ToString();
            }

            return string.Format("{0}{1}", GlobalConstants.CustomerGiftCardOrderNumberPrefixer, orderNumber.ToString().PadLeft(6, '0'));
        }

        protected string GenerateCode(string startPrefix, string value)
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            characters += alphabets + small_alphabets + numbers;

            int length = 5;
            string code = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (code.IndexOf(character) != -1);
                code += character;
            }

            //Check the database for duplicate
            return string.Format("{0}{1}{2}", startPrefix, code, string.Format("{0:0.00}", (Math.Round(Convert.ToDouble(value) * 100) / 100))).Replace(',','.');
        }

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

        public List<GiftCardObject> GetOrderGiftCards(string orderNumber)
        {
            try
            {
                return _scottybonsEComEntities.GiftCards.Where(gc => gc.OrderNumber.Trim().Equals(orderNumber.Trim())).Select(gc => new GiftCardObject
                {
                    Title = "Gift Card",
                    Name = gc.Name,
                    Value = gc.GiftCardValue
                }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}