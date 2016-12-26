using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ScottybonsMVC.AppConstants;
using Umbraco.Forms.Web.App_Start;
using Umbraco.Web;

namespace ScottybonsMVC
{
    public class ScottyBonsGlobal :UmbracoApplication
    {
        
            public void Init(HttpApplication application)
            {
                application.PreRequestHandlerExecute += application_PreRequestHandlerExecute;
                application.BeginRequest += this.Application_BeginRequest;
                application.EndRequest += this.Application_EndRequest;
               // application.re += this.Application_EndRequest;
                application.Error += Application_Error;
            }

            protected void Application_Start()
            {
                System.Diagnostics.Debugger.Break();
                AreaRegistration.RegisterAllAreas();
                RouteConfig.RegisterRoutes(RouteTable.Routes);
            }

            protected override void OnApplicationStarted(object sender, EventArgs e)
            {
                base.OnApplicationStarted(sender, e);

                // Your code here
            }

            private void application_PreRequestHandlerExecute(object sender, EventArgs e)
            {
                try
                {
                   
                }
                catch (Exception ex) { }
            }

            private void Application_BeginRequest(object sender, EventArgs e)
            {
                try
                {
                    var url = Request.Url.AbsoluteUri;
                    var currentLanguage = string.Empty;
                    if (url.Length >= 3)
                    {
                        currentLanguage = url.Split('/')[3];
                    }

                    if (string.IsNullOrEmpty(currentLanguage))
                    {
                        currentLanguage = "en";
                    }

                    if (currentLanguage.ToLower().Contains("en") || currentLanguage.ToLower().Contains(("nl")))
                    {
                        GlobalConstants.Language = currentLanguage;

                        var culture ="nl-"+ currentLanguage ?? "nl-NL";
                        var ci = CultureInfo.GetCultureInfo(culture);

                        Thread.CurrentThread.CurrentCulture = ci;
                        Thread.CurrentThread.CurrentUICulture = ci;

                        var cookie = new HttpCookie("culture", ci.Name);
                        Response.Cookies.Add(cookie);
                    }
                }
                catch { }
            }

            private void Application_EndRequest(object sender, EventArgs e)
            {
                // Your code here
            }

            protected new void Application_Error(object sender, EventArgs e)
            {
                // Your error handling here
            }
        
    }
}