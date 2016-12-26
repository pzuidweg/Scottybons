using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources.Scottybons;

namespace ScottybonsMVC.Models.ViewModels.Customer
{
    public class DeliveryAddressViewModel
    {
        //[Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "NameRequired")]
      public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "StreetNameRequired")]
        public string Street { get; set; }
        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "HouseNumberRequired")]
        public string Number { get; set; }

        [RegularExpression(@"^\d{4}[a-zA-Z]{2}$", ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PostalCodeRequired")]
        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PostalCodeRequired")]
        public string Zip { get; set; }



        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "CityRequired")]
        public string Town { get; set; }
        public IEnumerable<SelectListItem> Country { get; set; }
        public Nullable<int> CountryId { get; set; }
    }
}