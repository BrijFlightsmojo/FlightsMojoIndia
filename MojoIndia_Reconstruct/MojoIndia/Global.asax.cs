using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MojoIndia
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(@System.Configuration.ConfigurationManager.AppSettings["isSSL"]) && isLocalhost() && Request.Url.ToString().IndexOf("test.") == -1)
            {
                if (Request.Url.Authority.StartsWith("www") == false)
                {
                    var url = string.Format("{0}://www.{1}{2}", Request.Url.Scheme, Request.Url.Authority, Request.Url.PathAndQuery);
                    url = url.Replace("http:", "https:");
                    Response.RedirectPermanent(url, true);
                }
                if (HttpContext.Current.Request.IsSecureConnection == false)
                {
                    Response.RedirectPermanent("https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl);
                }
                if (Request.Url.Authority.EndsWith("index", StringComparison.OrdinalIgnoreCase) || Request.Url.PathAndQuery.EndsWith("index", StringComparison.OrdinalIgnoreCase))
                {
                    var url = string.Format("https://www.{0}", Request.Url.Authority);

                    Response.RedirectPermanent("/", true);
                }

            }
            string lowercaseURL = (Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.AbsolutePath);
            if (Regex.IsMatch(lowercaseURL, @"[A-Z]") && lowercaseURL.IndexOf("/flight/", StringComparison.OrdinalIgnoreCase) == -1 && lowercaseURL.IndexOf("/service/", StringComparison.OrdinalIgnoreCase) == -1)
            {
                System.Web.HttpContext.Current.Response.RedirectPermanent
                (
                     lowercaseURL.ToLower() + HttpContext.Current.Request.Url.Query
                );
            }
        }
        protected bool isLocalhost()
        {
            string VisitorsIPAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
            }
            if (VisitorsIPAddr == "" || VisitorsIPAddr == "127.0.0.1" || VisitorsIPAddr == "::1")
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Core.FlightUtility.LoadMasterData();
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            new Bal.LogWriter(ex.ToString(), ("GlobalError_" + DateTime.Today.ToString("ddMMMyy")));
        }
    }
}
