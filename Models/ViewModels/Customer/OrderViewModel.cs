using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using Resources.Scottybons;
using ScottybonsMVC.Models.Entities;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace ScottybonsMVC.Models.ViewModels.Customer
{
    public class OrderVm : RenderModel
    {
        public OrderVm(IPublishedContent content, CultureInfo culture)
            : base(content, culture)
        {
        }

        public OrderVm(IPublishedContent content)
            : base(content)
        {
        }

        public OrderVm()
            : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent)
        {


        }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "OrderVm_OrderOccesions_For_which_occasion_is_your_next_Scottybox_")]
        public IEnumerable<SelectListItem> OrderOccesions { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "OrderVm_PeriodicalScottyBox_Send_me_automatically_periodically_a_Scottybox")]
        public IEnumerable<SelectListItem> PeriodicalScottyBox { get; set; }


        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "OrderVm_StylistContactTypes___May_the_stylist_contact_you_")]
        public IEnumerable<SelectListItem> StylistContactTypes { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "OrderVm_DeliveryNeighbours_Can_we_leave_your_order_with_the_neighbours__if_you_are_not_there_")]
        public IEnumerable<SelectListItem> DeliveryNeighbours { get; set; }


        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "OrderVm_DeliveryAddress_Where_can_we_deliver_your_Scottybox_")]
        public DeliveryAddressViewModel DeliveryAddress { get; set; }
       [Required]
        public Order CustomerOrder { get; set; }

        public bool PeriodicalScottyBoxSelected { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "OrderVm_ContactNumber_If_yes__on_what_number_can_we_best_reach_you_")]
        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PhoneNumberInValid")]
        [StringLength(10, ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PhoneNumberInValid", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PhoneNumberInValid")]
        public string ContactNumber { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "OrderVm_PlanScottyboxHeader_PLAN_YOUR_NEXT_SCOTTYBOX")]
        public string PlanScottyboxHeader { get; set; }

        [Required(ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PlanScottyboxdropdown_validation")]
        public string PlanScottyboxdropdownOccesions { get; set; }        
    }

    public class DynamicEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Checked { get; set; }
    }

    public class ConfirmYourOrderVm : RenderModel
    {
        public ConfirmYourOrderVm(IPublishedContent content, CultureInfo culture) : base(content, culture) { }
        public ConfirmYourOrderVm(IPublishedContent content) : base(content) { }
        public ConfirmYourOrderVm() : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent) { }
        public ConfirmYourOrderViewModel ConfirmYourOrderViewModel { get; set; }
        public int PaymentMethodId { get; set; }
        public bool PermissionToCollectFutureInvoices { get; set; }
        
        [Range(typeof(bool), "true", "true", ErrorMessageResourceType = typeof(ScottybonsDataStrings), ErrorMessageResourceName = "PleaseAcceptTermsAndConditionsToCompleteTheRegistration")]
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "RegisterVM_Terms_and_Conditions_TermsandConditions")]
        public bool AgreeGeneralConditions { get; set; }
        public string SelectIssuer { get; set; }
        public int OrderNumber { get; set; }
        public int CustomerId { get; set; }
        public string GiftCardRedeemed { get; set; }
        public string GiftCardCode { get; set; }
        public string PromoCode { get; set; }
        public string PromoCodes { get; set; }
        public string PromoCodeRedeemed { get; set; }
        public int FailedTransactionNumber { get; set; }
        public string RedeemType { get; set; }
        public string GiftCardRedeemedValue { get; set; }

    }
}
