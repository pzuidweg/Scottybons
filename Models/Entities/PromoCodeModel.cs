using System;

namespace ScottybonsMVC.Models.Entities
{
    public class PromoCodeModel
    {
        public string PromoCode { get; set; }

        public int PromoCodeValue { get; set; }
        public bool IsOnline { get; set; }
        public bool IsPercentage { get; set; }
        public bool IsEnable { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Expired { get; set; }
        public bool AllowAllusers { get; set; }
        public bool OnlyOnce { get; set; }
        public bool Valid { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
    }
}