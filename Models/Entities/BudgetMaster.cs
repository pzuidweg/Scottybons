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
    
    public partial class BudgetMaster
    {
        public int BudgetID { get; set; }
        public string BudgetName { get; set; }
        public long DisplayOrder { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public System.DateTime CreatedOn { get; set; }
    }
}
