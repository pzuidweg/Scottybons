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
    
    public partial class Article
    {
        public Article()
        {
            this.ArticleColors = new HashSet<ArticleColor>();
            this.ArticleSKUs = new HashSet<ArticleSKU>();
        }
    
        public int ArticleID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> GenerID { get; set; }
        public Nullable<int> SubCategoryID { get; set; }
        public Nullable<int> SubCategoryPropertyID { get; set; }
        public Nullable<int> BrandID { get; set; }
        public int ArticleName { get; set; }
        public int ArticleDescription { get; set; }
        public int SizeConsistencey { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        public virtual GenderMaster GenderMaster { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual SubCategoryProperty SubCategoryProperty { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ArticleColor> ArticleColors { get; set; }
        public virtual ICollection<ArticleSKU> ArticleSKUs { get; set; }
    }
}
