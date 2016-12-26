using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Resources.Scottybons;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace ScottybonsMVC.Models.ViewModels.Customer
{
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class AccountType
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Checked { get; set; }
    }
    
    public class MyAccountViewModel : PostRedirectModel
    {

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "MyAccountViewModel_My_Account_MyAccount")]
        public string MyAccount { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "MyAccountViewModel_My_Account_Title_MyAccountTitle_")]
        public string MyAccountTitle { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "MyAccountViewModel_My_Account_Profile_MyAccountProfile")]
        public string MyAccountProfile { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "MyAccountViewModel_My_Account_PLaning_Next_Scotty_Box_Planing")]
        public string Planing { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "MyAccountViewModel_My_Account_PLaning_Next_Scotty_Box_Next")]
        public string Next { get; set; }
        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "MyAccountViewModel_My_Account_PLaning_Next_Scotty_Box_ScottyBox")]
        public string ScottyBox { get; set; }
    }


    public class MyAccountVM : RenderModel
    {
        public MyAccountVM(IPublishedContent content, CultureInfo culture)
            : base(content, culture)
        {
        }
        public MyAccountVM(IPublishedContent content): base(content)
        {
        }
        public MyAccountVM() : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent) { }


        public MyAccountViewModel MyAccountViewModel { get; set; }
    }
    

   

    public class MemberModel
    {
        // for login
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

   
}