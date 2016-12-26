using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Forms.Web.App_Start;
using Umbraco.Web;

namespace ScottybonsMVC
{
    public class Global : UmbracoApplication
    {
        protected void Application_Start()
        {
            System.Diagnostics.Debugger.Break();
            //AreaRegistration.RegisterAllAreas();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    var error = Server.GetLastError();
        //    Server.ClearError();
        //    Response.ContentType = "text/plain";
        //    Response.Write(error ?? (object)"unknown");
        //    Response.End();
        //}

        protected void Application_BeginRequest()
        {
            System.Diagnostics.Debugger.Break();

            var url = Request.Url.AbsoluteUri;
            if (url.Length >= 3)
            {
                url = url.Substring(3);
            }

            var culture = Request["culture"] ?? "en-US";
            var ci = CultureInfo.GetCultureInfo(culture);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            var cookie = new HttpCookie("culture", ci.Name);
            Response.Cookies.Add(cookie);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Session start logic here
        }
    }



   
}
