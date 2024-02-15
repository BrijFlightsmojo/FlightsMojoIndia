using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MojoIndia
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            routes.MapRoute(name: "service", url: "service/{action}/{id}", defaults: new { controller = "service", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute(name: "Flight", url: "Flight/{action}/{id}/{lid}/{rid}", defaults: new { controller = "Flight", action = "Index", id = UrlParameter.Optional, lid = UrlParameter.Optional, rid = UrlParameter.Optional });
            routes.MapRoute(name: "flights", url: "flights/{id}", defaults: new { controller = "flights", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute(name: "Payment", url: "Payment/{id}", defaults: new { controller = "Payment", action = "Payment", id = UrlParameter.Optional });
            routes.MapRoute(name: "city", url: "city/{id}", defaults: new { controller = "city", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute(name: "airline", url: "airline/{id}", defaults: new { controller = "airline", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute(name: "deals", url: "deals/{id}", defaults: new { controller = "deals", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute(name: "MyBooking", url: "MyBooking/{action}/{id}", defaults: new { controller = "MyBooking", action = "index", id = UrlParameter.Optional });
            routes.MapRoute(name: "Default1", url: "{action}/{id}", defaults: new { controller = "Home", action = "index", id = UrlParameter.Optional });
            routes.MapRoute(name: "Default", url: "{controller}/{action}/{id}", defaults: new { controller = "Home", action = "index", id = UrlParameter.Optional });
        }
    }
}
