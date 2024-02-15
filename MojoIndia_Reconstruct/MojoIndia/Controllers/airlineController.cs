using Core.ContentPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MojoIndia.Controllers
{
    public class airlineController : Controller
    {
        // GET: airline

         //   /airline/united-airlines-ua-flight-tickets
        public ActionResult Index(string id, string extra)
        {
            if (string.IsNullOrEmpty(id))

            {
                DAL.DALPageContent DPC = new DAL.DALPageContent();
                PageContent PC = DPC.PageContentForWebsite(Core.SiteId.FlightsMojoIN, Core.PageType.Airline, true, "SelectDetailForSite");
                if (Request.QueryString["utm_source"] != null)
                {
                    setCookie(Request.QueryString.Get("utm_source"));
                }
                return View("airline_new", PC);
            }

            else if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(extra))
            {
                id = id.ToLower();
                id = id.Replace("-flight-tickets", "").Trim();
                string[] str = id.Split('-');
                string code = string.Empty, name = string.Empty;
                if (str.Length >= 1)
                {
                    code = str[str.Length - 1];
                    for (int i = 0; i < str.Length - 1; i++)
                    {
                        name = str[i];
                    }
                    Dal.DALAirlineContent DALAC = new Dal.DALAirlineContent();
                    Core.ContentPage.AirlineContent AC = DALAC.GetAirlineContent(code, 2, true, "Select");
                    if ((AC.ResponseStatus.status == Core.TransactionStatus.Success) && ((AC.AirlineName.Replace(" ", "-") + "-" + AC.AirlineCode).Equals(id, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (Request.QueryString["utm_source"] != null)
                        {
                            setCookie(Request.QueryString.Get("utm_source"));
                        }
                        return View(AC);
                    }
                    else
                    {
                        return Redirect("/404");
                    }
                }
                else
                {
                    return Redirect("/404");
                }
            }

            else
            {
                return Redirect("/404");
            }
        }

        public void setCookie(string sourceMedia)
        {
            int intSmedia;
            bool bNum = int.TryParse(sourceMedia, out intSmedia);
            sourceMedia = bNum ? intSmedia.ToString() : "1000";
            HttpCookie FMsMedia = new HttpCookie("FMsMediaIndia");
            FMsMedia["sMediaIndia"] = sourceMedia;
            FMsMedia.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(FMsMedia);
        }
    }
}