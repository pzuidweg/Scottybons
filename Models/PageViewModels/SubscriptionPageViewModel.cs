using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace ScottybonsMVC.Models.PageViewModels
{
    public class SubscriptionPageViewModel : RenderModel
    {
        public SubscriptionPageViewModel(IPublishedContent content, CultureInfo culture)
            : base(content, culture)
        {
        }

        public SubscriptionPageViewModel(IPublishedContent content)
            : base(content)
        {
        }

        public SubscriptionPageViewModel()
            : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent)
        {


        }
        public ViewModels.Customer.OrderVm OrderContent { get; set; }
        public string PageTitle { get; set; }
        public string PageSubTitle { get; set; }
        public string SelectPeriod { get; set; }
        public string SelectedPeriod { get; set; }
        public string DeliveryDurationNote { get; set; }
        public string EditButton { get; set; }
        public string SendMeScottybox { get; set; }
        public string HowToChangeNote { get; set; }
        public string OccasionText { get; set; }
        public string StylistContactType { get; set; }
        public string PhoneNumberText { get; set; }
        public bool IsSubscription { get; set; }
        public string DeliveryAddressText { get; set; }
        public string DeliveryAtNeighbourText { get; set; }
        public string SaveButton { get; set; }
        public string NextScottyBoxDate { get; set; }
    }
}