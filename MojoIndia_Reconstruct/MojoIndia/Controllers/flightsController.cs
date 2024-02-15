using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dal;
using Core.ContentPage;

namespace MojoIndia.Controllers
{
    public class flightsController : Controller
    {
        // GET: flights
        public ActionResult Index(string id, string extra)
        {
            //flights/delhi-del-Varanasi-vns-cheap-airtickets
            if (string.IsNullOrEmpty(id))
            {
                DAL.DALPageContent DPC = new DAL.DALPageContent();
                PageContent PC = DPC.PageContentForWebsite(Core.SiteId.FlightsMojoIN, Core.PageType.Flights, true, "SelectDetailForSite");
                if (Request.QueryString["utm_source"] != null)
                {
                    setCookie(Request.QueryString.Get("utm_source"));
                }
                return View("test", PC);
            }
            else if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(extra))
            {
                if (id.Equals("varanasi-vns-delhi-del-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/varanasi-vns-new-delhi-del-cheap-airtickets");
                }
                else if (id.Equals("delhi-del-mumbai-bom-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/new-delhi-del-mumbai-bom-cheap-airtickets");
                }
                else if (id.Equals("kolkata-ccu-new delhi-del-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/kolkata-ccu-new-delhi-del-cheap-airtickets");
                }

                else if (id.Equals("newdelhi-del-bagdogra-ixb-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/new-delhi-del-bagdogra-ixb-cheap-airtickets");
                }

                else if (id.Equals("delhi-del-simla-slv-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/new-delhi-del-shimla-slv-cheap-airtickets");
                }

                else if (id.Equals("agartala-ixa-delhi-del-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/agartala-ixa-new-delhi-del-cheap-airtickets");
                }
                else if (id.Equals("kushinagar-kbk-new delhi-del-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/kushinagar-kbk-new-delhi-del-cheap-airtickets");
                }
                else if (id.Equals("delhi-del-bengaluru-blr-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/new-delhi-del-bengaluru-blr-cheap-airtickets");
                }
                else if (id.Equals("new delhi-del-goa-goi-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/new-delhi-del-goa-goi-cheap-airtickets");
                }
                else if (id.Equals("delhi-del-chennai-maa-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/new-delhi-del-chennai-maa-cheap-airtickets");
                }
                else if (id.Equals("new delhi-del-gaya-gay-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/new-delhi-del-gaya-gay-cheap-airtickets");
                }
                else if (id.Equals("delhi-del-chandigarh-ixc-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/new-delhi-del-chandigarh-ixc-cheap-airtickets");
                }
                else if (id.Equals("delhi-del-patna-pat-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/new-delhi-del-patna-pat-cheap-airtickets");
                }
                else if (id.Equals("delhi-del-varanasi-vns-cheap-airtickets", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectPermanent("/flights/new-delhi-del-varanasi-vns-cheap-airtickets");
                }
                id = id.ToLower();
                id = id.Replace("-cheap-airtickets", "").Trim();
                string[] str = id.Split('-');
                string code = string.Empty, name = string.Empty;
                if (str.Length >= 4)
                {
                    string Org = string.Empty, Dest = string.Empty;
                    Dest = str[str.Length - 1];
                    for (int i = 1; i < str.Length - 1 && string.IsNullOrEmpty(Org); i++)
                    {
                        if (str[i].Length == 3)
                            Org = str[i];
                    }
                    Dal.DALOriginDestinationContent ODC = new DALOriginDestinationContent();
                    OriginDestinationContent acd = ODC.OriginDestinationWithDeal(Core.SiteId.FlightsMojoIN, Org, Dest, true, "SelectDetailForSite");
                    if ((acd.ResponseStatus.status == Core.TransactionStatus.Success) && (acd.OriginName.Replace(" ", "-") + "-" + acd.OriginCode + "-" + acd.DestinationName.Replace(" ", "-") + "-" + acd.DestinationCode).Equals(id, StringComparison.OrdinalIgnoreCase))
                    {
                        ViewBag.CityCode = acd.OriginCode.ToUpper();
                        ViewBag.org = acd.DestinationCode.ToUpper();
                        ViewBag.orgcity = Core.FlightUtility.GetAirport(acd.OriginCode).cityName;
                        ViewBag.orgArp = Core.FlightUtility.GetAirport(acd.OriginCode).airportName;
                        ViewBag.orgCode = Core.FlightUtility.GetAirport(acd.OriginCode).countryName;

                        ViewBag.descity = Core.FlightUtility.GetAirport(acd.DestinationCode).cityName;
                        ViewBag.desArp = Core.FlightUtility.GetAirport(acd.DestinationCode).airportName;
                        ViewBag.desCode = Core.FlightUtility.GetAirport(acd.DestinationCode).countryName;


                        if (Request.QueryString["utm_source"] != null)
                        {
                            setCookie(Request.QueryString.Get("utm_source"));
                        }

                        return View(acd);
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