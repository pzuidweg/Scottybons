using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScottybonsMVC.Models.Entities
{
    public class GiftCardDetailsModal
    {
        public bool Expired { get; set; }
        public bool Exists { get; set; }
        public double OrginalAmount { get; set; }
        public double CurrentBalance { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}