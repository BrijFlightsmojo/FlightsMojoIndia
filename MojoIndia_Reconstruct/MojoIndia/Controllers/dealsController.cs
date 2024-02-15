using Core.ContentPage;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MojoIndia.Controllers
{
    public class dealsController : Controller
    {
        // GET: deals
        public ActionResult Index(string id, string extra)
        {
            //  /deals/Senior_Citizen_Flight_fares
            //  https://www.flightsmojo.in/deals/premium_economy_deals_fares

            if (string.IsNullOrEmpty(id))
            {
                DALPageContent DPC = new DALPageContent();
                PageContent PC = DPC.PageContentForWebsite(Core.SiteId.FlightsMojoIN, Core.PageType.Deals, true, "SelectDetailForSite");
                if (Request.QueryString["utm_source"] != null)
                {
                    setCookie(Request.QueryString.Get("utm_source"));
                }
                return View("deals_new", PC);
            }

            else if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(extra))
            {
                id = id.ToLower();
                string[] str = id.Split('-');
                string code = string.Empty, name = string.Empty;
                if (str.Length >= 1)
                {
                    code = str[str.Length - 1];
                    code = id.Replace("-", " ").Trim();
                    for (int i = 0; i < str.Length - 1; i++)
                    {
                        name = str[i];
                    }
                    DALDealsContent DALDC = new DALDealsContent();
                    DealsContent DC = DALDC.GetDealsContent(code, 2, true, "Select");
                    if ((DC.ResponseStatus.status == Core.TransactionStatus.Success) && ((DC.ThemeName.Replace(" ", "-")).Equals(id, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (Request.QueryString["utm_source"] != null)
                        {
                            setCookie(Request.QueryString.Get("utm_source"));
                        }
                        return View(DC);
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