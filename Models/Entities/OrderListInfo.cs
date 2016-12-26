//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScottybonsMVC.Models.Entities
{
    using System;
    
    public partial class OrderListInfo
    {
        public int OrderId { get; set; }
        public string CreatedOn { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nullable<int> OrderOccasion { get; set; }
        public string OccasionForScottyBoxName { get; set; }
        public string OrderStreet { get; set; }
        public string OrderHouseNumber { get; set; }
        public string OrderPostalCode { get; set; }
        public string OrderTown { get; set; }
        public Nullable<int> OrderCountryID { get; set; }
        public string CountryName { get; set; }
        public string CollectFutureInvoices { get; set; }
        public Nullable<bool> PermissionToCollectFutureInvoices { get; set; }
        public Nullable<bool> DeliverNeighbours { get; set; }
        public string PermissionToDeliverAtNeighbours { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<bool> IsStylistContactYou { get; set; }
        public string PermissionToCall { get; set; }
        public Nullable<System.DateTime> NextPerodicalScottyBoxDate { get; set; }
        public int PeriodicalScottyBoxID { get; set; }
        public string PeriodicalScottyBox { get; set; }
        public Nullable<int> PerodicalMonths { get; set; }
        public string OrderStatusName { get; set; }
    }
}
