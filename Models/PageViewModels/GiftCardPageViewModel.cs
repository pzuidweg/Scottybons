using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScottybonsMVC.Models.PageViewModels
{
    public class GiftCardPageViewModel
    {
        public bool IsLoggedIn { get; set; }
        public string LoggedUserEmaild { get; set; }
        public string LoggedUserFirstName { get; set; }
        public string LoggedUserLastName { get; set; }
        public bool OrderStatus { get; set; }
        public bool OrderStatusIsOpen { get; set; }
        public bool IsOrder { get; set; }
        public bool OrderExists { get; set; }
        public double OrderTotal { get; set; }
        public string OrderNumber { get; set; }
        public List<GiftCardObject> OrderGiftCards { get; set; }
        public string ProcessText { get; set; }
        public string ProcessTextOne { get; set; }
        public string ProcessTextTwo { get; set; }
        public string ProcessImage { get; set; }
        public string ProcessTitle { get; set; }
    }

    public class GiftCardModel
    {
        public bool IsGuest { get; set; }
        public string Email { get; set; }
        public List<GiftCardObject> GiftCards { get; set; }
        public GiftCardShipmentObject ShipmentDetails { get; set; }
        public int VendorId { get; set; }
        public string IssuerType { get; set; }
        public bool Status { get; set; }
        public bool PaymentError { get; set; }
        public string PaymentUrl { get; set; }
        public string ErrorMsg { get; set; }
        public string  OrderNumber { get; set; }
        public string OrderTotal { get; set; }

        public string RandomOrderSuffixer { get; set; }
    }

    public class GiftCardObject
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public double Value { get; set; }
        public string PersonalMessage { get; set; }
    }

    public class GiftCardShipmentObject
    {
        public string Email { get; set; }
        public string PayersFirstName { get; set; }
        public string PayersLastName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber{ get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public int Country { get; set; }        
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public string ErrorMsg { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}