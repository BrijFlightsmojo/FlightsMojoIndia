using Core;
using Core.ContentPage;
using Core.Flight;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MojoIndia.Controllers
{
    public class cityController : Controller
    {
        // GET: city
        public ActionResult Index(string id, string extra)
        {
            //city/cheap-flights-to-lucknow-lko
            if (string.IsNullOrEmpty(id))

            {
                DAL.DALPageContent DPC = new DAL.DALPageContent();
                PageContent PC = DPC.PageContentForWebsite(Core.SiteId.FlightsMojoIN, Core.PageType.City, true, "SelectDetailForSite");
                if (Request.QueryString["utm_source"] != null)
                {
                    setCookie(Request.QueryString.Get("utm_source"));
                }
                return View("city_new", PC);
            }

            else if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(extra))
            {
                id = id.ToLower();
                id = id.Replace("cheap-flights-to-", "").Trim();
                string[] str = id.Split('-');
                string code = string.Empty, name = string.Empty;
                if (str.Length >= 1)
                {
                    code = str[str.Length - 1];
                    for (int i = 0; i < str.Length - 1; i++)
                    {
                        name = str[i];
                    }
                    DALCityContent DALCC = new DALCityContent();
                    CityContent CC = DALCC.GetCityContent(code, 2, true, "Select");
                    if ((CC.ResponseStatus.status == Core.TransactionStatus.Success) && ((CC.CityName.Replace(" ", "-") + "-" + CC.CityCode).Equals(id, StringComparison.OrdinalIgnoreCase)))
                    {
                        ViewBag.CityCode = CC.CityCode.ToUpper();
                        ViewBag.org = "";

                        ViewBag.orgcity = Core.FlightUtility.GetAirport(CC.CityCode).cityName;
                        ViewBag.orgArp = Core.FlightUtility.GetAirport(CC.CityCode).airportName;
                        ViewBag.orgCode = Core.FlightUtility.GetAirport(CC.CityCode).countryName;

                        ViewBag.descity = "";
                        ViewBag.desArp = "";
                        if (Request.QueryString["utm_source"] != null)
                        {
                            setCookie(Request.QueryString.Get("utm_source"));
                        }
                        return View(CC);
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