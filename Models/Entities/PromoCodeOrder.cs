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
    using System.Collections.Generic;
    
    public partial class PromoCodeOrder
    {
        public int PromoCodeOrderId { get; set; }
        public string PromoCode { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<double> PromoCodeValue { get; set; }
        public Nullable<bool> IsPercentage { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
