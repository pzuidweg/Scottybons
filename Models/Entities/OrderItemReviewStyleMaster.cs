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
    
    public partial class OrderItemReviewStyleMaster
    {
        public OrderItemReviewStyleMaster()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public int OrderItemReviewStyleID { get; set; }
        public string OrderItemReviewStyleName { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public int DisplayOrder { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
    
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
