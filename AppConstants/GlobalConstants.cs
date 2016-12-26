using System;
using System.Configuration;
using System.Net.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.Mvc;

namespace ScottybonsMVC.AppConstants
{
    public static class GlobalConstants
    {
        private static string _language = string.Empty;


        public static string EmailTemplatePath
        {
            get
            {
                // Returns the title string.
                return HttpContext.Current.Server.MapPath("~/Views/Mail/EmailContent.htm");
            }
        }

        public static string EmailTemplatePath10
        {
            get
            {
                // Returns the title string.
                return HttpContext.Current.Server.MapPath("~/Views/Mail/EmailContent10.htm");
            }
        }

        public static string EmailTemplatePath30
        {
            get
            {
                // Returns the title string.
                return HttpContext.Current.Server.MapPath("~/Views/Mail/EmailContent30.htm");
            }
        }

        public static string ScottybonCustomerSupportAdminName
        {
            get
            {
                // Returns the title string.
                return WebConfigurationManager.AppSettings["CustomerSupportAdminName"];
            }
        }


        public static string ScottybonCustomerSupportAdminEmail
        {
            get
            {
                // Returns the title string.
                return WebConfigurationManager.AppSettings["CustomerSupportMail"];
            }
        }


        public static string ScottybonEmailAddress
        {
            get
            {
                // Returns the title string.
                return WebConfigurationManager.AppSettings["AdminEmail"];
            }
        }

        public static string ScottybonEmailToAddress
        {
            get
            {
                // Returns the title string.
                return WebConfigurationManager.AppSettings["AdminEmailTo"];
            }
        }

        public static SmtpSection SmtpSection
        {
            get
            {
                // Returns the title string.
                return (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            }
        }


        public static string Language
        {
            get
            {
                // Returns the title string.
                return _language;
            }
            set { _language = value; }
        }

        public static string LanguageUrl
        {
            get
            {
                // Returns the title string.
                return "/" + _language;
            }
            set { _language = value; }
        }

        public static string ErrorURL
        {
            get
            {
                // Returns the title string.
                return "/" + _language + "/ErrorPage?Error=";
            }
        }

        public static string NavigationKey
        {
            get
            {
                // Returns the title string.
                return "/" + Language;
            }

        }


        public static string SmtpHost
        {
            get
            {
                // Returns the title string.
                return SmtpSection.Network.Host;
            }
        }

        public static string SmtpUserName
        {
            get
            {
                // Returns the title string.
                return SmtpSection.Network.UserName;
            }
        }

        public static string SmtpUPassword
        {
            get
            {
                // Returns the title string.
                return SmtpSection.Network.Password;
            }
        }

        public static string LinkInEmail
        {
            get
            {
                var request = HttpContext.Current.Request;
                var appUrl = HttpRuntime.AppDomainAppVirtualPath;


                if (!string.IsNullOrWhiteSpace(appUrl)) appUrl += "/";


                var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
                //return baseUrl;

                return baseUrl + LanguageUrl;
                // Returns the title string.
                //return System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(System.Web.HttpContext.Current.Request.Url.AbsolutePath, string.Empty) + LanguageUrl;
            }
        }


        //ICE Pay Properties
        public static string IcePayTranService
        {
            get
            {

                return WebConfigurationManager.AppSettings["IcePayTranService"];
            }
        }
        public static string IcePayTranOperation
        {
            get
            {

                return WebConfigurationManager.AppSettings["IcePayTranOperation"];
            }
        }
        public static int IcePayTranMerchantId
        {
            get
            {
                int temp = 0;
                bool res = int.TryParse(WebConfigurationManager.AppSettings["IcePayTranMerchantId"], out temp);
                return temp;
            }
        }
        public static string IcePayTranMerchantSecret
        {
            get
            {

                return WebConfigurationManager.AppSettings["IcePayTranMerchantSecret"];
            }
        }
        public static string IcePayTranSuccess
        {
            get
            {

                return WebConfigurationManager.AppSettings["IcePayTranSuccess"];
            }
        }
        public static string IcePayTranError
        {
            get
            {

                return WebConfigurationManager.AppSettings["IcePayTranError"];
            }
        }
        public static string IcePayTranGiftCardDescription
        {
            get
            {

                return WebConfigurationManager.AppSettings["IcePayTranGiftCardDescription"];
            }
        }
        
        public static string IcePayTranDescription
        {
            get
            {

                return WebConfigurationManager.AppSettings["IcePayTranDescription"];
            }
        }
        public static string IcePayTranCurrency
        {
            get
            {

                return WebConfigurationManager.AppSettings["IcePayTranCurrency"];
            }
        }

        public static int IcePayTranInitialAmount
        {
            get
            {
                int temp = 0;
                bool res = int.TryParse(WebConfigurationManager.AppSettings["IcePayTranInitialAmount"], out temp);
                return temp;
            }
        }

        public static bool ActivateEmail
        {
            get
            {
                bool temp = false;
                bool res = bool.TryParse(WebConfigurationManager.AppSettings["ActivateEmail"], out temp);
                return temp;
            }
        }

        public static bool ShowActualErrorMessage
        {
            get
            {
                bool temp = false;
                bool res = bool.TryParse(WebConfigurationManager.AppSettings["ShowActualErrorMessage"], out temp);
                return temp;
            }
        }

        //ICE-Pay Gift Card Properties
        public static string IcePayGiftCardTranSuccess
        {
            get
            {

                return WebConfigurationManager.AppSettings["IcePayGiftCardTranSuccess"];
            }
        }
        public static string IcePayGiftCardTranError
        {
            get
            {

                return WebConfigurationManager.AppSettings["IcePayGiftCardTranError"];
            }
        }

        #region Gift Card Properties
        public static string CustomerGiftCardOrderNumberPrefixer
        {
            get
            {

                return WebConfigurationManager.AppSettings["CustomerGiftCardOrderNumberPrefixer"];
            }
        }

        public static string AdminGiftCardOrderNumberPrefixer
        {
            get
            {

                return WebConfigurationManager.AppSettings["AdminGiftCardOrderNumberPrefixer"];
            }
        }
        #endregion
    }

    public static class GetSession
    {
        public static int CustomerId
        {
            get
            {
                int temp = 0;
                try
                {
                    return Convert.ToInt32(HttpContext.Current.Session["CustomerID"]);
                }
                catch (Exception)
                {
                    return temp;
                }
            }
        }
        public static int OrderNumber
        {
            get
            {


                int temp = 0;
                try
                {
                    return Convert.ToInt32(HttpContext.Current.Session["OrderNumber"]);
                }
                catch (Exception)
                {
                    return temp;
                }


            }
        }

        public static int RequestOrderNumber
        {
            get
            {


                int temp = 0;
                try
                {
                    return Convert.ToInt32(HttpContext.Current.Request["OrderID"]);
                }
                catch (Exception)
                {
                    return temp;
                }


            }
        }
    }
}