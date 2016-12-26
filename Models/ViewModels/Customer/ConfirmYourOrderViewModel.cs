using System.ComponentModel.DataAnnotations;
using Resources.Scottybons;
using Umbraco.Web.Models;

namespace ScottybonsMVC.Models.ViewModels.Customer
{
    public class ConfirmYourOrderViewModel : PostRedirectModel
    {
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_Outtfit_")]
        public string CompleteOutfit { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_YourorderHeading_YOUR_ORDER_")]
        public string YourorderHeading { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_Overview_Overview_")]
        public string Overview { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_TBD_")]
        public string TBD { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_retrospectivebilling_")]
        public string Retrospectivebilling { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_Styleadvice_")]
        public string StyleAdvice { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_tobepaiddirectly_")]
        public string ToBePaidDirectly { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_Shippingcost_")]
        public string Shippingcost { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_Free_")]
        public string Free { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_Totaltobepaiddirectly_")]
        public string Totaltobepaiddirectly { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_Chooseapaymentmethod_")]
        public int Chooseapaymentmethod { get; set; }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_Ideal_")]
        public string Ideal { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_CreditCard_")]
        public string Creditcard { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_ScottyBonsPermissions_")]
        public string ScottyBonsPermissions { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_ScottyBonsPrivacyPolicy_")]
        public string ScottyBonsPrivacyPolicy { get; set; }


        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_ScottyBonsClickOnPayment_")]
        public string ClickOnPayment { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_ScottyBonsProcess_")]
        public string ScottyBonsProcess { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_ScottyBonsDescription1_")]
        public string Description1 { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "ConfirmYourOrderModel_CompleteOutfit_Complete_ScottyBonsDescription2_")]
        public string Description2 { get; set; }

        public int OrderNumber { get; set; }
        public int CustomerNumber { get; set; }


    }
}