using Core;
using Core.Flight;
using Core.RP;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using MojoIndia.Models;
using Newtonsoft.Json;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MojoIndia.Controllers
{
    public class FlightController : Controller
    {
        private static string apiKey = ConfigurationManager.AppSettings["apiKeyTextLocal"];
        private static string sender = ConfigurationManager.AppSettings["senderTextLocal"];
        private static string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();
        public Stopwatch stopwatch = new Stopwatch();
        public static string[] LccAirline = new string[] { "IX", "QP", "SG", "I5", "6E" };
        // GET: Flight
        [HttpPost]
        public ActionResult SearchingFlight(FormCollection FormColl)
        {
            try
            {
                StringBuilder sbLogger = new StringBuilder();
                FlightSearchRequest fsr = null;
                fsr = new FlightSearchRequest();
                fsr.segment = new List<SearchSegment>();
                if (FormColl["hfTripType"] != null)
                    fsr.tripType = (TripType)Convert.ToByte(FormColl["hfTripType"]);
                SearchSegment sseg = new SearchSegment();
                sseg.originAirport = GetAirportCode(FormColl["hfCity_from"]);
                sseg.orgArp = FlightUtility.GetAirport(sseg.originAirport);
                sseg.destinationAirport = GetAirportCode(FormColl["hfCity_to"]);
                sseg.travelDate = DateTime.ParseExact(FormColl["departure_date"].ToString(), "dd MMM yy", new CultureInfo("en-US"));//Convert.ToDateTime(FormColl["departDate"].ToString());
                sseg.destArp = FlightUtility.GetAirport(sseg.destinationAirport);
                fsr.segment.Add(sseg);
                if (sseg.orgArp.countryCode.ToUpper() == "IN" && sseg.destArp.countryCode.ToUpper() == "IN")
                    fsr.travelType = TravelType.Domestic;
                else
                    fsr.travelType = TravelType.International;
                if (fsr.tripType == TripType.RoundTrip)
                {
                    SearchSegment sseg1 = new SearchSegment();
                    sseg1.originAirport = GetAirportCode(FormColl["hfCity_to"]);
                    sseg1.orgArp = FlightUtility.GetAirport(sseg1.originAirport);
                    sseg1.destinationAirport = GetAirportCode(FormColl["hfCity_from"]);
                    sseg1.travelDate = DateTime.ParseExact(FormColl["return_date"].ToString(), "dd MMM yy", new CultureInfo("en-US"));//Convert.ToDateTime(FormColl["returnDate"].ToString());

                    sseg1.destArp = FlightUtility.GetAirport(sseg1.destinationAirport);
                    fsr.segment.Add(sseg1);
                }
                if (FormColl["Cabin"] != null)
                    fsr.cabinType = (CabinType)Convert.ToByte(FormColl["Cabin"].ToString());

                if (FormColl["airlines"] != null)
                    fsr.airline = FormColl["airlines"].ToString().ToUpper();
                else
                    fsr.airline = "ALL";

                fsr.adults = FormColl["Adult"] != null ? Convert.ToByte(FormColl["Adult"].ToString()) : (byte)0;
                fsr.child = FormColl["Child"] != null ? Convert.ToByte(FormColl["Child"].ToString()) : (byte)0;
                fsr.infants = FormColl["Infant"] != null ? Convert.ToByte(FormColl["Infant"].ToString()) : (byte)0;
                fsr.searchDirectFlight = FormColl["NonStop"] != null ? true : false;
                fsr.flexibleSearch = FormColl["FlexDate"] != null ? true : false;
                fsr.isNearBy = FormColl["NearByAirport"] != null ? true : false;
                fsr.currencyCode = "INR";
                fsr.siteId = SiteId.FlightsMojoIN;
                fsr.sourceMedia = GetCookie();
                fsr.utm_campaign = "";
                fsr.utm_medium = "";
                fsr.userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                fsr.tgy_Request_id = DateTime.Now.ToString("ddMMyyyHHmmsss");
                fsr.device = GetDevice();
                #region set kayakClickID for S2S Pixel
                if (Session["SearchDetails"] != null)
                {
                    AirContext airContextTemp = Session["SearchDetails"] as AirContext;
                    if (airContextTemp.flightSearchRequest != null && !string.IsNullOrEmpty(airContextTemp.flightSearchRequest.redirectID))
                    {
                        fsr.redirectID = airContextTemp.flightSearchRequest.redirectID;
                    }
                }
                #endregion

                fsr.deepLink = "/flight/searchFlightResult?org=" + fsr.segment[0].originAirport + "&dest=" + fsr.segment[0].destinationAirport + "&depdate=" + fsr.segment[0].travelDate.ToString("dd-MM-yyyy") + "&retdate=" + (fsr.segment.Count > 1 ? fsr.segment[1].travelDate.ToString("dd-MM-yyyy") : "") + "&tripType=" + (fsr.segment.Count > 1 ? "R" : "O") + "&adults=" + fsr.adults + "&child=" + fsr.child + "&infants=" + fsr.infants + "&cabin=" + ((int)fsr.cabinType) + "&utm_source=" + fsr.sourceMedia + "&currency=inr";

                AirContext airContext = new AirContext(fsr.userIP);
                airContext.isDomestic = (Core.FlightUtility.GetAirport(fsr.segment[0].originAirport).countryCode.ToUpper() == "IN" && Core.FlightUtility.GetAirport(fsr.segment[0].destinationAirport).countryCode.ToUpper() == "IN");
                airContext.flightSearchRequest = fsr;
                airContext.IsSearchCompleted = false;
                airContext.flightRef = new List<string>();
                fsr.userSessionID = Session.SessionID;
                fsr.userLogID = fsr.userSearchID = getSearchID();
                FlightOperation.SetAirContext(airContext);
                bookingLog(ref sbLogger, "Original Request", JsonConvert.SerializeObject(fsr));
                return Redirect("/Flight/Result/" + fsr.userSearchID);
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "SearchRequestException" + DateTime.Today.ToString("ddMMyy"), "Error");
                return Redirect("/");
            }
        }
        [HttpGet]
        public ActionResult itinerary()
        {
            StringBuilder sbLogger = new StringBuilder();
            try
            {
                string strUrl = Request.ServerVariables["SERVER_NAME"] + Request.RawUrl.ToString();
                sbLogger.Append(Environment.NewLine + strUrl + Environment.NewLine);
                new Bal.LogWriter(sbLogger.ToString(), ("DeepLink_itinerary_" + DateTime.Today.ToString("ddMMMyy")));
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error2_" + DateTime.Today.ToString("ddMMyy"), "Error");
            }
            if (Request.QueryString["sec1"] == null || Request.QueryString["adults"] == null || Request.QueryString["cabin"] == null ||
                Request.QueryString["TranId"] == null || Request.QueryString["Id"] == null)
            {
                return Redirect("/");
            }
            #region make Request
            FlightSearchRequest fsr = new FlightSearchRequest();
            fsr.adults = string.IsNullOrEmpty(Request.QueryString.Get("adults")) ? 1 : Convert.ToInt32(Request.QueryString.Get("adults"));
            fsr.child = string.IsNullOrEmpty(Request.QueryString.Get("child")) ? 0 : Convert.ToInt32(Request.QueryString.Get("child"));
            fsr.infants = string.IsNullOrEmpty(Request.QueryString.Get("infants")) ? 0 : Convert.ToInt32(Request.QueryString.Get("infants"));


            fsr.segment = new List<SearchSegment>();
            if (!string.IsNullOrEmpty(Request.QueryString.Get("sec1")))
            {
                SearchSegment sseg = new SearchSegment();
                string[] arrStr = Request.QueryString.Get("sec1").Split('|');
                sseg.originAirport = arrStr[0].ToUpper();
                sseg.orgArp = Core.FlightUtility.GetAirport(sseg.originAirport);

                sseg.destinationAirport = arrStr[1].ToUpper();
                sseg.destArp = Core.FlightUtility.GetAirport(sseg.destinationAirport);

                sseg.travelDate = Convert.ToDateTime(arrStr[2]);// DateTime.Today.AddDays(150);
                fsr.segment.Add(sseg);
                fsr.tripType = TripType.OneWay;

                if (sseg.orgArp.countryCode.ToUpper() == "IN" && sseg.destArp.countryCode.ToUpper() == "IN")
                    fsr.travelType = TravelType.Domestic;
                else
                    fsr.travelType = TravelType.International;

            }

            fsr.travelType = getTravelType(fsr.segment[0].orgArp.countryCode, fsr.segment[0].destArp.countryCode);
            if (!string.IsNullOrEmpty(Request.QueryString.Get("sec1")) && Request.QueryString["Sec2"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("sec2")))
            {
                SearchSegment sseg = new SearchSegment();
                string[] arrStr = Request.QueryString.Get("sec2").Split('|');
                sseg.originAirport = arrStr[0].ToUpper();
                sseg.orgArp = Core.FlightUtility.GetAirport(sseg.originAirport);

                sseg.destinationAirport = arrStr[1].ToUpper();
                sseg.destArp = Core.FlightUtility.GetAirport(sseg.destinationAirport);

                sseg.travelDate = Convert.ToDateTime(arrStr[2]);// DateTime.Today.AddDays(150);
                fsr.segment.Add(sseg);

                if (fsr.segment[0].originAirport == fsr.segment[1].destinationAirport && fsr.segment[0].destinationAirport == fsr.segment[1].originAirport)
                    fsr.tripType = TripType.RoundTrip;
                else
                    fsr.tripType = TripType.MultiCity;
            }

            fsr.siteId = SiteId.FlightsMojoIN;
            fsr.cabinType = (CabinType)Convert.ToInt32((Request.QueryString["cabin"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("cabin"))) ? Request.QueryString.Get("cabin") : "1");
            fsr.airline = (Request.QueryString["airline"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("airline"))) ? Request.QueryString.Get("airline").ToUpper() : "ALL";

            fsr.searchDirectFlight = false;
            fsr.flexibleSearch = false;
            fsr.isNearBy = false;

            fsr.sourceMedia = Request.QueryString.Get("campain");
            if ((fsr.sourceMedia.Equals("1001") || fsr.sourceMedia.Equals("1002")) && Request.QueryString["utm_source"] != null)
            {
                fsr.sourceMedia = Request.QueryString["utm_source"] != null ? Request.QueryString.Get("utm_source").ToString() : "";
            }
            int intSmedia;
            bool bNum = int.TryParse(fsr.sourceMedia, out intSmedia);
            fsr.sourceMedia = bNum ? intSmedia.ToString() : "1000";
            setCookie(fsr.sourceMedia);
            fsr.sID = Request.QueryString.Get("TranId");
            fsr.rID = Request.QueryString.Get("Id");
            fsr.utm_campaign = "";
            fsr.utm_medium = "";
            fsr.currencyCode = (Request.QueryString["currency"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("currency"))) ? Request.QueryString.Get("currency").ToUpper() : "INR";
            if (fsr.sourceMedia.Equals("1015"))
            {
                fsr.redirectID = (Request.QueryString["skyscanner_redirectid"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("skyscanner_redirectid"))) ? Request.QueryString.Get("skyscanner_redirectid") : "";
            }
            else if (fsr.sourceMedia.Equals("1001") || fsr.sourceMedia.Equals("1002"))
            {
                fsr.redirectID = Request.QueryString["wego_click_id"] != null ? Request.QueryString.Get("wego_click_id").ToString() : "";
            }
            else if (fsr.sourceMedia.Equals("1013"))
            {
                fsr.redirectID = Request.QueryString["kayakclickid"] != null ? Request.QueryString.Get("kayakclickid").ToString() : "";
            }
            else
            {
                fsr.redirectID = (Request.QueryString["redirectID"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("redirectID"))) ? Request.QueryString.Get("redirectID") : "";
            }

            #endregion
            fsr.userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            fsr.device = GetDevice();
            fsr.deepLink = "/flight/searchFlightResult?org=" + fsr.segment[0].originAirport + "&dest=" + fsr.segment[0].destinationAirport + "&depdate=" + fsr.segment[0].travelDate.ToString("dd-MM-yyyy") + "&retdate=" + (fsr.segment.Count > 1 ? fsr.segment[1].travelDate.ToString("dd-MM-yyyy") : "") + "&tripType=" + (fsr.segment.Count > 1 ? "R" : "O") + "&adults=" + fsr.adults + "&child=" + fsr.child + "&infants=" + fsr.infants + "&cabin=" + ((int)fsr.cabinType) + "&utm_source=" + fsr.sourceMedia + "&currency=inr";
            fsr.deepLink += (string.IsNullOrEmpty(fsr.redirectID) ? "" : ("&redirectID=" + fsr.redirectID));

            fsr.deepLink += (string.IsNullOrEmpty(fsr.sID) ? "" : ("&TranId=" + fsr.sID));
            fsr.deepLink += (string.IsNullOrEmpty(fsr.rID) ? "" : ("&Id=" + fsr.rID));


            fsr.userLogID = string.IsNullOrEmpty(fsr.sID) ? getSearchID() : fsr.sID;
            fsr.userSearchID = getSearchID();
            fsr.userSessionID = Session.SessionID;
            AirContext airContext = new AirContext(fsr.userIP);
            airContext.flightSearchRequest = fsr;
            airContext.IsSearchCompleted = false;
            airContext.flightRef = new List<string>();

            FlightOperation.SetAirContext(airContext);
            fsr.tgy_Request_id = DateTime.Now.ToString("ddMMyyyyHHmmsss");
            stopwatch.Stop();

            new Bal.FlightDetails().SearchFlight(ref airContext, GlobalData.isDummyResult);

            if (airContext.flightSearchResponse.response.status == TransactionStatus.Success && checkIsFareExist(ref airContext))
            {
                return Redirect("/Flight/Passenger/" + fsr.userSearchID + "/" + airContext.flightSearchResponse.result1Index + (string.IsNullOrEmpty(airContext.flightSearchResponse.result2Index) ? "" : ("/" + airContext.flightSearchResponse.result2Index)));
            }
            else
            {
                return Redirect("/Flight/Result/" + fsr.userSearchID);
            }
        }
        [HttpGet]
        public ActionResult itinerarygf()
        {
            ///flight/itinerarygf?org=DEL&dest=BOM&depdate=26-09-2024&retdate=&tripType=O&adults=3&child=2&infants=1&cabin=1&utm_source=1037&currency=inr&price=20485.50&rdtl=UK_999_240926|2155_240927|0015
            StringBuilder sbLogger = new StringBuilder();
            try
            {
                string strUrl = Request.ServerVariables["SERVER_NAME"] + Request.RawUrl.ToString();
                sbLogger.Append(Environment.NewLine + strUrl + Environment.NewLine);
                new Bal.LogWriter(sbLogger.ToString(), ("DeepLink_itinerarygf_" + DateTime.Today.ToString("ddMMMyy")));
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error3_" + DateTime.Today.ToString("ddMMyy"), "Error");
            }

            if (Request.QueryString["org"] == null || Request.QueryString["dest"] == null || Request.QueryString["depdate"] == null || Request.QueryString["adults"] == null || Request.QueryString["cabin"] == null)
            {
                return Redirect("/");
            }
            #region make Request
            FlightSearchRequest fsr = new FlightSearchRequest();

            fsr.adults = string.IsNullOrEmpty(Request.QueryString.Get("adults")) ? 1 : Convert.ToInt32(Request.QueryString.Get("adults"));
            fsr.child = string.IsNullOrEmpty(Request.QueryString.Get("child")) ? 0 : Convert.ToInt32(Request.QueryString.Get("child"));
            fsr.infants = string.IsNullOrEmpty(Request.QueryString.Get("infants")) ? 0 : Convert.ToInt32(Request.QueryString.Get("infants"));

            #region Set Setgment
            fsr.segment = new List<SearchSegment>();
            if (!string.IsNullOrEmpty(Request.QueryString.Get("org")) && !string.IsNullOrEmpty(Request.QueryString.Get("dest")))
            {
                SearchSegment sseg = new SearchSegment();
                sseg.originAirport = Request.QueryString["org"].ToUpper();
                sseg.orgArp = Core.FlightUtility.GetAirport(sseg.originAirport);

                sseg.destinationAirport = Request.QueryString["dest"].ToUpper();
                sseg.destArp = Core.FlightUtility.GetAirport(sseg.destinationAirport);

                sseg.travelDate = DateTime.ParseExact(Request.QueryString["depdate"], "dd-MM-yyyy", new CultureInfo("en-US"));
                if (!string.IsNullOrEmpty(Request.QueryString.Get("NewDate")))
                {
                    string dt = Request.QueryString.Get("NewDate");
                    sseg.travelDate = Convert.ToDateTime(dt + "-" + sseg.travelDate.Year);
                }

                fsr.segment.Add(sseg);
                fsr.tripType = TripType.OneWay;
                if (sseg.orgArp.countryCode.ToUpper() == "IN" && sseg.destArp.countryCode.ToUpper() == "IN")
                    fsr.travelType = TravelType.Domestic;
                else
                    fsr.travelType = TravelType.International;

                if (!string.IsNullOrEmpty(Request.QueryString.Get("tripType")) && !string.IsNullOrEmpty(Request.QueryString.Get("retdate")) && Request.QueryString.Get("tripType").Equals("R", StringComparison.OrdinalIgnoreCase))
                {
                    SearchSegment sseg1 = new SearchSegment();
                    sseg1.originAirport = Request.QueryString["dest"].ToUpper();
                    sseg1.orgArp = Core.FlightUtility.GetAirport(sseg1.originAirport);

                    sseg1.destinationAirport = Request.QueryString["org"].ToUpper();
                    sseg1.destArp = Core.FlightUtility.GetAirport(sseg1.destinationAirport);

                    sseg1.travelDate = DateTime.ParseExact(Request.QueryString["retdate"], "dd-MM-yyyy", new CultureInfo("en-US"));
                    fsr.tripType = TripType.RoundTrip;
                    fsr.segment.Add(sseg1);
                }
            }
            #endregion

            fsr.siteId = SiteId.FlightsMojoIN;
            fsr.cabinType = (CabinType)Convert.ToInt32((Request.QueryString["cabin"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("cabin"))) ? Request.QueryString.Get("cabin") : "1");
            fsr.airline = (Request.QueryString["airline"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("airline"))) ? Request.QueryString.Get("airline").ToUpper() : "ALL";


            if (Request.QueryString["utm_source"] != null)
            {
                fsr.sourceMedia = Request.QueryString.Get("utm_source");
            }
            else
            {
                fsr.sourceMedia = "1037";
            }
            fsr.sourceMedia = fsr.sourceMedia.Split(',')[0];
            int intSmedia;
            bool bNum = int.TryParse(fsr.sourceMedia, out intSmedia);
            fsr.sourceMedia = bNum ? intSmedia.ToString() : "1000";
            setCookie(fsr.sourceMedia);
            fsr.utm_campaign = "";
            fsr.utm_medium = "";


            fsr.currencyCode = (Request.QueryString["currency"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("currency"))) ? Request.QueryString.Get("currency").ToUpper() : "INR";
            fsr.googleFlightRequest = new GoogleFlightDeepLink();
            fsr.googleFlightRequest.DisplayedPrice = Convert.ToDecimal(Request.QueryString.Get("price"));
            #endregion
            fsr.userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            fsr.device = GetDevice();
            fsr.deepLink = "/flight/searchFlightResult?org=" + fsr.segment[0].originAirport + "&dest=" + fsr.segment[0].destinationAirport + "&depdate=" + fsr.segment[0].travelDate.ToString("dd-MM-yyyy") + "&retdate=" + (fsr.segment.Count > 1 ? fsr.segment[1].travelDate.ToString("dd-MM-yyyy") : "") + "&tripType=" + (fsr.segment.Count > 1 ? "R" : "O") + "&adults=" + fsr.adults + "&child=" + fsr.child + "&infants=" + fsr.infants + "&cabin=" + ((int)fsr.cabinType) + "&utm_source=" + fsr.sourceMedia + "&currency=inr";


            AirContext airContext = new AirContext(fsr.userIP);
            airContext.flightSearchRequest = fsr;
            airContext.IsSearchCompleted = false;
            airContext.flightRef = new List<string>();
            fsr.userSessionID = Guid.NewGuid().ToString();
            fsr.userSearchID = getSearchID();

            FlightOperation.SetAirContext(airContext);
            fsr.tgy_Request_id = DateTime.Now.ToString("ddMMyyyyHHmmsss");
            stopwatch.Stop();

            airContext.flightSearchResponse = new Bal.FlightDetails().SearchFlightGF(fsr);
            FlightResult result = null,resutlReturn=null;
            if (true)
            {
                //var resultResponse = new Bal.FlightDetails().SearchFlightGF(airContext.flightSearchRequest);

                if (airContext.flightSearchResponse != null && airContext.flightSearchResponse.Results.Count > 0)
                {
                    string gfItinComb = Request.QueryString.Get("rdtl"); 

                    foreach (var flightResult in airContext.flightSearchResponse.Results[0])
                    {
                        if (result == null)
                        {
                            string ItinComb = string.Empty;
                            foreach (var fs in flightResult.FlightSegments)
                            {
                                foreach (var seg in fs.Segments)
                                {
                                    ItinComb += (seg.Airline + "_" + seg.FlightNumber + "_" + seg.DepTime.ToString("yyMMdd|HHmm") + "_" + seg.ArrTime.ToString("yyMMdd|HHmm"));
                                }
                            }
                            if (gfItinComb.Equals(ItinComb, StringComparison.OrdinalIgnoreCase))
                            {
                                result = flightResult;
                            }
                        }
                    }
                    if (Request.QueryString["r2dtl"] != null)
                    {
                        string gfItin2Comb = Request.QueryString.Get("r2dtl");

                        foreach (var returnFlightResult in airContext.flightSearchResponse.Results[1])
                        {
                            if (resutlReturn == null)
                            {
                                string ItinComb = string.Empty;
                                foreach (var fs in returnFlightResult.FlightSegments)
                                {
                                    foreach (var seg in fs.Segments)
                                    {
                                        ItinComb += (seg.Airline + "_" + seg.FlightNumber + "_" + seg.DepTime.ToString("yyMMdd|HHmm") + "_" + seg.ArrTime.ToString("yyMMdd|HHmm"));
                                    }
                                }
                                if (gfItin2Comb.Equals(ItinComb, StringComparison.OrdinalIgnoreCase))
                                {
                                    resutlReturn = returnFlightResult;
                                }
                            }
                        }
                        if ((result.Fare.grandTotal + resutlReturn.Fare.grandTotal) < fsr.googleFlightRequest.DisplayedPrice)
                        {
                            decimal fare= (fsr.googleFlightRequest.DisplayedPrice - (result.Fare.grandTotal + resutlReturn.Fare.grandTotal));
                            result.Fare.Markup += fare/2;
                            result.Fare.grandTotal += fare / 2;

                            resutlReturn.Fare.Markup += fare / 2;
                            resutlReturn.Fare.grandTotal += fare / 2;
                        }
                    }
                    else
                    {
                        if (result.Fare.grandTotal < fsr.googleFlightRequest.DisplayedPrice)
                        {
                            result.Fare.Markup += (fsr.googleFlightRequest.DisplayedPrice - result.Fare.grandTotal);
                            result.Fare.grandTotal += (fsr.googleFlightRequest.DisplayedPrice - result.Fare.grandTotal);
                        }
                    }
                }
            }
            if (result != null && resutlReturn !=null)
            {

                //airContext.flightSearchResponse.Results[0][0] = result;
                airContext.IsGFMatch = true;
                return Redirect("/Flight/passengerGF/" + fsr.userSearchID + "/" + result.ResultID+"/"+resutlReturn.ResultID);
            }
            if (result != null )
            {

                //airContext.flightSearchResponse.Results[0][0] = result;
                airContext.IsGFMatch = true;
                return Redirect("/Flight/passengerGF/" + fsr.userSearchID + "/" + result.ResultID);
            }
            else
            {
                airContext.IsGFMatch = false;
                result = airContext.flightSearchResponse.Results[0][0];
                return Redirect("/Flight/flightnotmatch/" + fsr.userSearchID);
            }
        }
        [HttpGet]
        public ActionResult getFlightResult()
        {
            //flight/getFlightResult?sec1=DEL|DXB|2021-06-14&sec2=DXB|DEL|2021-06-20&adults=1&child=0&infants=0&cabin=1&&utm_source=1000&currency=inr
            //flight/getFlightResult?sec1=DEL|BOM|2023-06-14&sec2=BOM|DEL|2023-06-20&adults=1&child=0&infants=0&cabin=1&&utm_source=1013&currency=inr
            StringBuilder sbLogger = new StringBuilder();
            try
            {
                string strUrl = Request.ServerVariables["SERVER_NAME"] + Request.RawUrl.ToString();
                sbLogger.Append(Environment.NewLine + strUrl + Environment.NewLine);
                new Bal.LogWriter(sbLogger.ToString(), ("DeepLink_getFlightResult_" + DateTime.Today.ToString("ddMMMyy")));
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error3_" + DateTime.Today.ToString("ddMMyy"), "Error");
            }

            if (Request.QueryString["sec1"] == null || Request.QueryString["adults"] == null || Request.QueryString["cabin"] == null)
            {
                return Redirect("/");
            }
            #region make Request
            FlightSearchRequest fsr = new FlightSearchRequest();

            fsr.adults = string.IsNullOrEmpty(Request.QueryString.Get("adults")) ? 1 : Convert.ToInt32(Request.QueryString.Get("adults"));
            fsr.child = string.IsNullOrEmpty(Request.QueryString.Get("child")) ? 0 : Convert.ToInt32(Request.QueryString.Get("child"));
            fsr.infants = string.IsNullOrEmpty(Request.QueryString.Get("infants")) ? 0 : Convert.ToInt32(Request.QueryString.Get("infants"));

            #region Set Setgment
            fsr.segment = new List<SearchSegment>();
            if (!string.IsNullOrEmpty(Request.QueryString.Get("sec1")))
            {
                SearchSegment sseg = new SearchSegment();
                string[] arrStr = Request.QueryString.Get("sec1").Split('|');
                sseg.originAirport = arrStr[0].ToUpper();
                sseg.orgArp = Core.FlightUtility.GetAirport(sseg.originAirport);

                sseg.destinationAirport = arrStr[1].ToUpper();
                sseg.destArp = Core.FlightUtility.GetAirport(sseg.destinationAirport);

                sseg.travelDate = Convert.ToDateTime(arrStr[2]);
                fsr.segment.Add(sseg);
                fsr.tripType = TripType.OneWay;
                if (sseg.orgArp.countryCode.ToUpper() == "IN" && sseg.destArp.countryCode.ToUpper() == "IN")
                    fsr.travelType = TravelType.Domestic;
                else
                    fsr.travelType = TravelType.International;
            }
            if (!string.IsNullOrEmpty(Request.QueryString.Get("sec1")) && Request.QueryString["Sec2"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("sec2")))
            {
                SearchSegment sseg = new SearchSegment();
                string[] arrStr = Request.QueryString.Get("sec2").Split('|');
                sseg.originAirport = arrStr[0].ToUpper();
                sseg.orgArp = Core.FlightUtility.GetAirport(sseg.originAirport);

                sseg.destinationAirport = arrStr[1].ToUpper();
                sseg.destArp = Core.FlightUtility.GetAirport(sseg.destinationAirport);

                sseg.travelDate = Convert.ToDateTime(arrStr[2]);
                fsr.segment.Add(sseg);

                if (fsr.segment[0].originAirport == fsr.segment[1].destinationAirport && fsr.segment[0].destinationAirport == fsr.segment[1].originAirport)
                    fsr.tripType = TripType.RoundTrip;
                else
                    fsr.tripType = TripType.MultiCity;
            }


            #endregion

            fsr.siteId = SiteId.FlightsMojoIN;
            fsr.cabinType = (CabinType)Convert.ToInt32((Request.QueryString["cabin"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("cabin"))) ? Request.QueryString.Get("cabin") : "1");
            fsr.airline = (Request.QueryString["airline"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("airline"))) ? Request.QueryString.Get("airline").ToUpper() : "ALL";


            fsr.searchDirectFlight = false;
            fsr.isNearBy = Request.QueryString["isNearBy"] != null ? Convert.ToBoolean(Request.QueryString.Get("isNearBy")) : false;
            fsr.flexibleSearch = Request.QueryString["isflex"] != null ? Convert.ToBoolean(Request.QueryString.Get("isflex")) : false;

            if (Request.QueryString["utm_source"] != null)
            {
                fsr.sourceMedia = Request.QueryString.Get("utm_source");
            }
            else if (Request.QueryString["campain"] != null)
            {
                fsr.sourceMedia = Request.QueryString.Get("campain");
            }
            else
            {
                fsr.sourceMedia = "1000";
            }
            fsr.sourceMedia = fsr.sourceMedia.Split(',')[0];
            int intSmedia;
            bool bNum = int.TryParse(fsr.sourceMedia, out intSmedia);
            fsr.sourceMedia = bNum ? intSmedia.ToString() : "1000";
            setCookie(fsr.sourceMedia);
            fsr.utm_campaign = "";
            fsr.utm_medium = "";
            fsr.currencyCode = (Request.QueryString["currency"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("currency"))) ? Request.QueryString.Get("currency").ToUpper() : "USD";
            fsr.redirectID = (Request.QueryString["redirectID"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("redirectID"))) ? Request.QueryString.Get("redirectID") : "";

            if (Request.QueryString["wego_click_id"] != null)
            {
                fsr.redirectID = Request.QueryString.Get("wego_click_id").ToString();
            }


            #endregion
            fsr.userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            fsr.device = GetDevice();
            fsr.deepLink = "/flight/searchFlightResult?org=" + fsr.segment[0].originAirport + "&dest=" + fsr.segment[0].destinationAirport + "&depdate=" + fsr.segment[0].travelDate.ToString("dd-MM-yyyy") + "&retdate=" + (fsr.segment.Count > 1 ? fsr.segment[1].travelDate.ToString("dd-MM-yyyy") : "") + "&tripType=" + (fsr.segment.Count > 1 ? "R" : "O") + "&adults=" + fsr.adults + "&child=" + fsr.child + "&infants=" + fsr.infants + "&cabin=" + ((int)fsr.cabinType) + "&utm_source=" + fsr.sourceMedia + "&currency=inr";
            fsr.deepLink += (string.IsNullOrEmpty(fsr.redirectID) ? "" : ("&redirectID=" + fsr.redirectID));
            fsr.deepLink += (string.IsNullOrEmpty(fsr.sID) ? "" : ("&TranId=" + fsr.sID));
            fsr.deepLink += (string.IsNullOrEmpty(fsr.rID) ? "" : ("&Id=" + fsr.rID));

            AirContext airContext = new AirContext(fsr.userIP);
            airContext.flightSearchRequest = fsr;
            airContext.IsSearchCompleted = false;
            airContext.flightRef = new List<string>();

            fsr.userSessionID = Session.SessionID;
            fsr.userLogID = fsr.userSearchID = getSearchID();
            FlightOperation.SetAirContext(airContext);

            return Redirect("/Flight/Result/" + fsr.userSearchID);
        }
        [HttpGet]
        public ActionResult searchFlightResult()
        {
            //flight/searchFlightResult?org=DEL&dest=BOM&depdate=20-10-2023&retdate=27-10-2023&tripType=R&adults=1&child=0&infants=0&cabin=1&&utm_source=1000&currency=inr

            StringBuilder sbLogger = new StringBuilder();
            try
            {
                string strUrl = Request.ServerVariables["SERVER_NAME"] + Request.RawUrl.ToString();
                sbLogger.Append(Environment.NewLine + strUrl + Environment.NewLine);
                new Bal.LogWriter(sbLogger.ToString(), ("DeepLink_getFlightResult_" + DateTime.Today.ToString("ddMMMyy")));
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error3_" + DateTime.Today.ToString("ddMMyy"), "Error");
            }

            if (Request.QueryString["org"] == null || Request.QueryString["dest"] == null || Request.QueryString["depdate"] == null || Request.QueryString["adults"] == null || Request.QueryString["cabin"] == null)
            {
                return Redirect("/");
            }
            #region make Request
            FlightSearchRequest fsr = new FlightSearchRequest();

            fsr.adults = string.IsNullOrEmpty(Request.QueryString.Get("adults")) ? 1 : Convert.ToInt32(Request.QueryString.Get("adults"));
            fsr.child = string.IsNullOrEmpty(Request.QueryString.Get("child")) ? 0 : Convert.ToInt32(Request.QueryString.Get("child"));
            fsr.infants = string.IsNullOrEmpty(Request.QueryString.Get("infants")) ? 0 : Convert.ToInt32(Request.QueryString.Get("infants"));

            #region Set Setgment
            fsr.segment = new List<SearchSegment>();
            if (!string.IsNullOrEmpty(Request.QueryString.Get("org")) && !string.IsNullOrEmpty(Request.QueryString.Get("dest")))
            {
                SearchSegment sseg = new SearchSegment();
                sseg.originAirport = Request.QueryString["org"].ToUpper();
                sseg.orgArp = Core.FlightUtility.GetAirport(sseg.originAirport);

                sseg.destinationAirport = Request.QueryString["dest"].ToUpper();
                sseg.destArp = Core.FlightUtility.GetAirport(sseg.destinationAirport);

                sseg.travelDate = DateTime.ParseExact(Request.QueryString["depdate"], "dd-MM-yyyy", new CultureInfo("en-US"));
                if (!string.IsNullOrEmpty(Request.QueryString.Get("NewDate")))
                {
                    string dt = Request.QueryString.Get("NewDate");
                    sseg.travelDate = Convert.ToDateTime(dt + "-" + sseg.travelDate.Year);
                }

                fsr.segment.Add(sseg);
                fsr.tripType = TripType.OneWay;
                if (sseg.orgArp.countryCode.ToUpper() == "IN" && sseg.destArp.countryCode.ToUpper() == "IN")
                    fsr.travelType = TravelType.Domestic;
                else
                    fsr.travelType = TravelType.International;

                if (!string.IsNullOrEmpty(Request.QueryString.Get("tripType")) && !string.IsNullOrEmpty(Request.QueryString.Get("retdate")) && Request.QueryString.Get("tripType").Equals("R", StringComparison.OrdinalIgnoreCase))
                {
                    SearchSegment sseg1 = new SearchSegment();
                    sseg1.originAirport = Request.QueryString["dest"].ToUpper();
                    sseg1.orgArp = Core.FlightUtility.GetAirport(sseg1.originAirport);

                    sseg1.destinationAirport = Request.QueryString["org"].ToUpper();
                    sseg1.destArp = Core.FlightUtility.GetAirport(sseg1.destinationAirport);

                    sseg1.travelDate = DateTime.ParseExact(Request.QueryString["retdate"], "dd-MM-yyyy", new CultureInfo("en-US"));
                    fsr.tripType = TripType.RoundTrip;
                    fsr.segment.Add(sseg1);
                }
            }
            #endregion

            fsr.siteId = SiteId.FlightsMojoIN;
            fsr.cabinType = (CabinType)Convert.ToInt32((Request.QueryString["cabin"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("cabin"))) ? Request.QueryString.Get("cabin") : "1");
            fsr.airline = (Request.QueryString["airline"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("airline"))) ? Request.QueryString.Get("airline").ToUpper() : "ALL";


            if (Request.QueryString["utm_source"] != null)
            {
                fsr.sourceMedia = Request.QueryString.Get("utm_source");
            }
            else if (Request.QueryString["campain"] != null)
            {
                fsr.sourceMedia = Request.QueryString.Get("campain");
            }
            else
            {
                fsr.sourceMedia = "1000";
            }
            fsr.sourceMedia = fsr.sourceMedia.Split(',')[0];
            int intSmedia;
            bool bNum = int.TryParse(fsr.sourceMedia, out intSmedia);
            fsr.sourceMedia = bNum ? intSmedia.ToString() : "1000";
            setCookie(fsr.sourceMedia);
            fsr.utm_campaign = "";
            fsr.utm_medium = "";
            fsr.sID = Request.QueryString.Get("TranId");
            fsr.rID = Request.QueryString.Get("Id");

            fsr.currencyCode = (Request.QueryString["currency"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("currency"))) ? Request.QueryString.Get("currency").ToUpper() : "INR";
            fsr.redirectID = (Request.QueryString["redirectID"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("redirectID"))) ? Request.QueryString.Get("redirectID") : "";

            fsr.currencyCode = (Request.QueryString["currency"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("currency"))) ? Request.QueryString.Get("currency").ToUpper() : "USD";
            #endregion
            fsr.userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            fsr.device = GetDevice();
            fsr.deepLink = "/flight/searchFlightResult?org=" + fsr.segment[0].originAirport + "&dest=" + fsr.segment[0].destinationAirport + "&depdate=" + fsr.segment[0].travelDate.ToString("dd-MM-yyyy") + "&retdate=" + (fsr.segment.Count > 1 ? fsr.segment[1].travelDate.ToString("dd-MM-yyyy") : "") + "&tripType=" + (fsr.segment.Count > 1 ? "R" : "O") + "&adults=" + fsr.adults + "&child=" + fsr.child + "&infants=" + fsr.infants + "&cabin=" + ((int)fsr.cabinType) + "&utm_source=" + fsr.sourceMedia + "&currency=inr";
            fsr.deepLink += (string.IsNullOrEmpty(fsr.redirectID) ? "" : ("&redirectID=" + fsr.redirectID));
            fsr.deepLink += (string.IsNullOrEmpty(fsr.sID) ? "" : ("&TranId=" + fsr.sID));
            fsr.deepLink += (string.IsNullOrEmpty(fsr.rID) ? "" : ("&Id=" + fsr.rID));

            AirContext airContext = new AirContext(fsr.userIP);
            airContext.flightSearchRequest = fsr;
            airContext.IsSearchCompleted = false;
            airContext.flightRef = new List<string>();

            fsr.userSessionID = Session.SessionID;
            fsr.userLogID = fsr.userSearchID = getSearchID();
            FlightOperation.SetAirContext(airContext);

            return Redirect("/Flight/Result/" + fsr.userSearchID);
        }

        [HttpGet]
        public ActionResult GF_FlightResult()
        {


            StringBuilder sbLogger = new StringBuilder();
            try
            {
                string strUrl = Request.ServerVariables["SERVER_NAME"] + Request.RawUrl.ToString();
                sbLogger.Append(Environment.NewLine + strUrl + Environment.NewLine);
                new Bal.LogWriter(sbLogger.ToString(), ("DeepLink_GF_FlightResult_" + DateTime.Today.ToString("ddMMMyy")));

            }
            catch (Exception ex)
            {

            }
            #region set GoogleFlightDeepLink
            Core.Flight.GoogleFlightDeepLink gfd = new Core.Flight.GoogleFlightDeepLink()
            {
                //&=1
                //&=IN
                //&=16986
                //&=INR
                //&=en
                //&=INR
                //&=RoundTrip
                //&=1,2
                //&=3,4
                //&Cabin1=Economy
                //&Carrier1=SG
                //&DepartureDate1=2023-01-19
                //&Origin1=SXR
                //&Destination1=DEL
                //&FlightNumber1=958
                //&Cabin2=Economy
                //&Carrier2=SG
                //&DepartureDate2=2023-01-20
                //&Origin2=DEL
                //&Destination2=DBR
                //&FlightNumber2=751
                //&Cabin3=Economy
                //&Carrier3=SG
                //&DepartureDate3=2023-01-26
                //&Origin3=DBR
                //&Destination3=DEL
                //&FlightNumber3=8496
                //&Cabin4=Economy
                //&Carrier4=SG
                //&DepartureDate4=2023-01-27
                //&Origin4=DEL
                //&Destination4=SXR
                //&FlightNumber4=8473
                //&=GOOG_GFS_712
                Adult = string.IsNullOrEmpty(Request.QueryString.Get("Adult")) ? 1 : Convert.ToInt32(Request.QueryString.Get("Adult")),
                child = string.IsNullOrEmpty(Request.QueryString.Get("child")) ? 0 : Convert.ToInt32(Request.QueryString.Get("child")),
                infant = string.IsNullOrEmpty(Request.QueryString.Get("infant")) ? 0 : Convert.ToInt32(Request.QueryString.Get("infant")),

                PointOfSaleCountry = string.IsNullOrEmpty(Request.QueryString.Get("PointOfSaleCountry")) ? "" : Request.QueryString.Get("PointOfSaleCountry"),
                DisplayedPrice = string.IsNullOrEmpty(Request.QueryString.Get("DisplayedPrice")) ? 0 : Convert.ToDecimal(Request.QueryString.Get("DisplayedPrice")),
                DisplayedPriceCurrency = string.IsNullOrEmpty(Request.QueryString.Get("DisplayedPriceCurrency")) ? "" : Request.QueryString.Get("DisplayedPriceCurrency"),
                UserLanguage = string.IsNullOrEmpty(Request.QueryString.Get("UserLanguage")) ? "" : Request.QueryString.Get("UserLanguage"),
                UserCurrency = string.IsNullOrEmpty(Request.QueryString.Get("UserCurrency")) ? "" : Request.QueryString.Get("UserCurrency"),
                TripType = string.IsNullOrEmpty(Request.QueryString.Get("TripType")) ? "" : Request.QueryString.Get("TripType"),

                ReferralId = string.IsNullOrEmpty(Request.QueryString.Get("ReferralId")) ? "" : Request.QueryString.Get("ReferralId"),
                flightSlice = new List<FlightSlice>(),
                slice = new List<Slice>()
            };
            if (gfd.infant == 0)
            {
                gfd.infant = string.IsNullOrEmpty(Request.QueryString.Get("InfantLap")) ? 0 : Convert.ToInt32(Request.QueryString.Get("InfantLap"));
            }
            for (int i = 1; i <= 2; i++)
            {
                if (!string.IsNullOrEmpty(Request.QueryString.Get("Slice" + i)))
                {
                    Slice slc = new Slice() { id = Request.QueryString.Get("Slice" + i), sliceId = new List<int>() };
                    if (!string.IsNullOrEmpty(slc.id))
                    {
                        foreach (string item in slc.id.Split(','))
                        {
                            slc.sliceId.Add(Convert.ToInt32(item.Trim()));
                        }
                    }
                    gfd.slice.Add(slc);
                }
            }
            foreach (var slc in gfd.slice)
            {
                if (!string.IsNullOrEmpty(slc.id) && slc.sliceId.Count > 0)
                {
                    foreach (int sliceID in slc.sliceId)
                    {
                        FlightSlice fs = new FlightSlice()
                        {
                            id = sliceID,
                            Origin = string.IsNullOrEmpty(Request.QueryString.Get("Origin" + sliceID)) ? "" : Request.QueryString.Get("Origin" + sliceID),
                            Destination = string.IsNullOrEmpty(Request.QueryString.Get("Destination" + sliceID)) ? "" : Request.QueryString.Get("Destination" + sliceID),
                            Cabin = string.IsNullOrEmpty(Request.QueryString.Get("Cabin" + sliceID)) ? "" : Request.QueryString.Get("Cabin" + sliceID),
                            Carrier = string.IsNullOrEmpty(Request.QueryString.Get("Carrier" + sliceID)) ? "" : Request.QueryString.Get("Carrier" + sliceID),
                            depDate = string.IsNullOrEmpty(Request.QueryString.Get("DepartureDate" + sliceID)) ? "" : Request.QueryString.Get("DepartureDate" + sliceID),
                            depTime = string.IsNullOrEmpty(Request.QueryString.Get("DepartureTime" + sliceID)) ? "" : Request.QueryString.Get("DepartureTime" + sliceID),
                            arrDate = string.IsNullOrEmpty(Request.QueryString.Get("ArrivalDate" + sliceID)) ? "" : Request.QueryString.Get("ArrivalDate" + sliceID),
                            arrTime = string.IsNullOrEmpty(Request.QueryString.Get("ArrivalTime" + sliceID)) ? "" : Request.QueryString.Get("ArrivalTime" + sliceID),
                            FlightNumber = string.IsNullOrEmpty(Request.QueryString.Get("FlightNumber" + sliceID)) ? "" : Request.QueryString.Get("FlightNumber" + sliceID),
                            BookingCode = string.IsNullOrEmpty(Request.QueryString.Get("BookingCode" + sliceID)) ? "" : Request.QueryString.Get("BookingCode" + sliceID),
                        };
                        gfd.flightSlice.Add(fs);
                    }
                }
            }
            #endregion

            #region make Request
            FlightSearchRequest fsr = new FlightSearchRequest();

            fsr.adults = gfd.Adult;
            fsr.child = gfd.child;
            fsr.infants = gfd.infant;

            #region Set Setgment

            fsr.segment = new List<SearchSegment>();
            foreach (Slice slc in gfd.slice)
            {
                FlightSlice fsorg = gfd.flightSlice.Where(k => k.id == slc.sliceId.First()).FirstOrDefault();
                FlightSlice fsdest = gfd.flightSlice.Where(k => k.id == slc.sliceId.Last()).FirstOrDefault();
                SearchSegment seg = new SearchSegment()
                {
                    originAirport = fsorg.Origin,
                    orgArp = Core.FlightUtility.GetAirport(fsorg.Origin),
                    travelDate = DateTime.ParseExact(fsorg.depDate, "yyyy-MM-dd", new CultureInfo("en-US")),
                    destinationAirport = fsdest.Destination,
                    destArp = Core.FlightUtility.GetAirport(fsdest.Destination),
                };
                fsr.segment.Add(seg);

            }

            fsr.tripType = fsr.segment.Count == 1 ? TripType.OneWay : TripType.RoundTrip;
            fsr.travelType = getTravelType(fsr.segment[0].orgArp.countryCode, fsr.segment[0].destArp.countryCode);
            #endregion

            fsr.siteId = SiteId.FlightsMojoIN;
            if (gfd.flightSlice[0].Cabin.Equals("PremiumEconomy", StringComparison.OrdinalIgnoreCase))
            {
                fsr.cabinType = CabinType.PremiumEconomy;
            }
            else if (gfd.flightSlice[0].Cabin.Equals("Business", StringComparison.OrdinalIgnoreCase))
            {
                fsr.cabinType = CabinType.Business;
            }
            else if (gfd.flightSlice[0].Cabin.Equals("First", StringComparison.OrdinalIgnoreCase))
            {
                fsr.cabinType = CabinType.First;
            }
            else
            {
                fsr.cabinType = CabinType.Economy;
            }

            fsr.siteId = SiteId.FlightsMojoIN;
            fsr.sourceMedia = "1037";
            setCookie(fsr.sourceMedia);
            fsr.utm_campaign = "";
            fsr.utm_medium = "";
            fsr.currencyCode = "INR";
            #endregion

            fsr.userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            #region set deep link
            fsr.deepLink = "/flight/getFlightResult?sec1=" + fsr.segment[0].originAirport + "|" + fsr.segment[0].destinationAirport + "|" + fsr.segment[0].travelDate.ToString("yyyy-MM-dd");
            for (int i = 1; i < 4; i++)
            {
                if (i < fsr.segment.Count)
                {
                    fsr.deepLink += "&sec" + (i + 1) + "=" + fsr.segment[i].originAirport + "|" + fsr.segment[i].destinationAirport + "|" + fsr.segment[i].travelDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    fsr.deepLink += "&sec" + (i + 1) + "=";
                }
            }
            fsr.deepLink += "&adults=" + fsr.adults + "&child=" + fsr.child + "&infants=" + fsr.infants + "&cabin=" + ((int)fsr.cabinType).ToString() + "&airline=&siteid=" + ((int)fsr.siteId).ToString() + "&campain=" + fsr.sourceMedia + "&currency=" + fsr.currencyCode;
            #endregion

            AirContext airContext = new AirContext(fsr.userIP);
            airContext.googleFlightDeepLink = gfd;
            fsr.googleFlightRequest = gfd;
            airContext.flightSearchRequest = fsr;
            airContext.IsSearchCompleted = false;
            airContext.flightRef = new List<string>();

            fsr.userSessionID = Guid.NewGuid().ToString();
            fsr.userSearchID = getSearchID();
            string aa = JsonConvert.SerializeObject(fsr);
            FlightOperation.SetAirContext(airContext);

            string strAirLine = string.Empty;

            var feeRule = new Bal.MakeFlightItinerary().GetFlightResult(airContext, ref strAirLine);
            FlightResult result = null;

            GfPriceVerifyResponse priceVfResponse = null;
            if (!LccAirline.Contains(strAirLine))
            {
                GfPriceVerifyRequest pvReq = new GfPriceVerifyRequest()
                {
                    adults = fsr.adults,
                    child = fsr.child,
                    infants = fsr.infants,

                    flightResult = airContext.flightSearchResponse.Results[0]
                };


                priceVfResponse = new Bal.FlightDetails().PriceVerify(pvReq);
            }
            bool isSerchFlight = true;

            if (priceVfResponse != null && priceVfResponse.responseStatus.status == TransactionStatus.Success)
            {
                Fare fare = new Fare();
                fare.BaseFare = priceVfResponse.fare.BaseFare;
                fare.Tax = priceVfResponse.fare.Tax;
                fare.PublishedFare = priceVfResponse.fare.PublishedFare;
                fare.NetFare = priceVfResponse.fare.NetFare;
                fare.grandTotal = priceVfResponse.fare.grandTotal;
                fare.fareBreakdown = priceVfResponse.fare.fareBreakdown;
                fare.gdsType = GdsType.Amadeus;
                Core.Flight.FareBreakdown fb = fare.fareBreakdown.Where(o => o.PassengerType == PassengerType.Adult).FirstOrDefault();


                feeRule = new Bal.MakeFlightItinerary().getFareRule(airContext, (fb.BaseFare + fb.Tax));
                if (feeRule != null)
                {
                    if (feeRule.FeeAmount != 0)
                    {
                        fare.Markup += feeRule.FeeAmount;
                        if (fsr.child > 0)
                        {
                            fare.Markup += feeRule.FeeAmount;
                        }
                        if (fsr.infants > 0)
                        {
                            fare.Markup += feeRule.FeeAmount;
                        }
                    }
                    if (feeRule.FeePercent != 0)
                    {
                        Core.Flight.FareBreakdown fbAdt = priceVfResponse.fare.fareBreakdown.Where(o => o.PassengerType == PassengerType.Adult).FirstOrDefault();
                        fare.Markup += ((priceVfResponse.fare.BaseFare + priceVfResponse.fare.Tax) * (feeRule.FeePercent / 100));

                    }
                    fare.grandTotal = (priceVfResponse.fare.BaseFare + priceVfResponse.fare.Tax + fare.Markup);
                }
                result = airContext.flightSearchResponse.Results[0][0];
                if (fare.grandTotal > result.Fare.grandTotal)
                {
                    result.Fare.BaseFare = fare.BaseFare;
                    result.Fare.Tax = fare.Tax;
                    result.Fare.PublishedFare = fare.PublishedFare;
                    result.Fare.NetFare = fare.NetFare;
                    result.Fare.grandTotal = fare.grandTotal;
                    result.Fare.Currency = fare.Currency;
                    result.Fare.Markup = fare.Markup;
                    result.Fare.fareBreakdown = new List<FareBreakdown>();
                    foreach (var item in fare.fareBreakdown)
                    {
                        FareBreakdown fbreak = new FareBreakdown() { BaseFare = item.BaseFare, Tax = item.Tax, Markup = item.Markup, PassengerType = item.PassengerType };
                        result.Fare.fareBreakdown.Add(fbreak);
                    }
                }
                isSerchFlight = false;
            }

            if (isSerchFlight)
            {
                var resultResponse = new Bal.FlightDetails().SearchFlightGF(airContext.flightSearchRequest);

                if (resultResponse != null && resultResponse.Results.Count > 0)
                {
                    string gfItinComb = string.Empty;
                    foreach (var fs in gfd.flightSlice)
                    {
                        gfItinComb += fs.Origin + fs.Destination + fs.FlightNumber + fs.Carrier + fs.depDate;
                    }
                    foreach (var flightResult in resultResponse.Results[0])
                    {
                        if (result == null)
                        {
                            string ItinComb = string.Empty;
                            foreach (var fs in flightResult.FlightSegments)
                            {
                                foreach (var seg in fs.Segments)
                                {
                                    ItinComb += seg.Origin + seg.Destination + seg.FlightNumber + seg.Airline + seg.DepTime.ToString("yyyy-MM-dd");
                                }
                            }
                            if (gfItinComb.Equals(ItinComb, StringComparison.OrdinalIgnoreCase))
                            {
                                result = flightResult;

                                feeRule = new Bal.MakeFlightItinerary().getFareRule(airContext, (result.Fare.grandTotal));
                                if (feeRule != null)
                                {
                                    if (feeRule.FeeAmount != 0)
                                    {
                                        result.Fare.Markup += feeRule.FeeAmount;
                                        if (fsr.child > 0)
                                        {
                                            result.Fare.Markup += feeRule.FeeAmount;
                                        }
                                        if (fsr.infants > 0)
                                        {
                                            result.Fare.Markup += feeRule.FeeAmount;
                                        }
                                    }
                                    if (feeRule.FeePercent != 0)
                                    {
                                        Core.Flight.FareBreakdown fbAdt = priceVfResponse.fare.fareBreakdown.Where(o => o.PassengerType == PassengerType.Adult).FirstOrDefault();
                                        result.Fare.Markup += ((priceVfResponse.fare.BaseFare + priceVfResponse.fare.Tax) * (feeRule.FeePercent / 100));

                                    }
                                    result.Fare.grandTotal = (result.Fare.BaseFare + result.Fare.Tax + result.Fare.Markup);
                                }
                                if (result.Fare.grandTotal < fsr.googleFlightRequest.DisplayedPrice)
                                {
                                    result.Fare.Markup += (fsr.googleFlightRequest.DisplayedPrice - result.Fare.grandTotal);
                                    result.Fare.grandTotal = (result.Fare.BaseFare + result.Fare.Tax + result.Fare.Markup);
                                }
                            }
                        }
                    }
                }
            }
            if (result != null)
            {
                airContext.flightSearchResponse.Results[0][0] = result;
                airContext.IsGFMatch = true;
                return Redirect("/Flight/passengerGF/" + fsr.userSearchID + "/" + result.ResultID);
            }
            else
            {
                airContext.IsGFMatch = false;
                result = airContext.flightSearchResponse.Results[0][0];
                return Redirect("/Flight/flightnotmatch/" + fsr.userSearchID);
            }
        }



        [HttpGet]
        public ActionResult Get_GF_FlightResult()
        {

            StringBuilder sbLogger = new StringBuilder();
            try
            {
                string strUrl = Request.ServerVariables["SERVER_NAME"] + Request.RawUrl.ToString();
                sbLogger.Append(Environment.NewLine + strUrl + Environment.NewLine);
                new Bal.LogWriter(sbLogger.ToString(), ("DeepLink_itinerary_" + DateTime.Today.ToString("ddMMMyy")));
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error2_" + DateTime.Today.ToString("ddMMyy"), "Error");
            }
            if (Request.QueryString["sec1"] == null || Request.QueryString["adults"] == null || Request.QueryString["cabin"] == null ||
                Request.QueryString["TranId"] == null || Request.QueryString["Id"] == null)
            {
                return Redirect("/");
            }
            #region make Request
            FlightSearchRequest fsr = new FlightSearchRequest();
            fsr.adults = string.IsNullOrEmpty(Request.QueryString.Get("adults")) ? 1 : Convert.ToInt32(Request.QueryString.Get("adults"));
            fsr.child = string.IsNullOrEmpty(Request.QueryString.Get("child")) ? 0 : Convert.ToInt32(Request.QueryString.Get("child"));
            fsr.infants = string.IsNullOrEmpty(Request.QueryString.Get("infants")) ? 0 : Convert.ToInt32(Request.QueryString.Get("infants"));


            fsr.segment = new List<SearchSegment>();
            if (!string.IsNullOrEmpty(Request.QueryString.Get("sec1")))
            {
                SearchSegment sseg = new SearchSegment();
                string[] arrStr = Request.QueryString.Get("sec1").Split('|');
                sseg.originAirport = arrStr[0].ToUpper();
                sseg.orgArp = Core.FlightUtility.GetAirport(sseg.originAirport);

                sseg.destinationAirport = arrStr[1].ToUpper();
                sseg.destArp = Core.FlightUtility.GetAirport(sseg.destinationAirport);

                sseg.travelDate = Convert.ToDateTime(arrStr[2]);// DateTime.Today.AddDays(150);
                fsr.segment.Add(sseg);
                fsr.tripType = TripType.OneWay;

                if (sseg.orgArp.countryCode.ToUpper() == "IN" && sseg.destArp.countryCode.ToUpper() == "IN")
                    fsr.travelType = TravelType.Domestic;
                else
                    fsr.travelType = TravelType.International;

            }

            fsr.travelType = getTravelType(fsr.segment[0].orgArp.countryCode, fsr.segment[0].destArp.countryCode);
            if (!string.IsNullOrEmpty(Request.QueryString.Get("sec1")) && Request.QueryString["Sec2"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("sec2")))
            {
                SearchSegment sseg = new SearchSegment();
                string[] arrStr = Request.QueryString.Get("sec2").Split('|');
                sseg.originAirport = arrStr[0].ToUpper();
                sseg.orgArp = Core.FlightUtility.GetAirport(sseg.originAirport);

                sseg.destinationAirport = arrStr[1].ToUpper();
                sseg.destArp = Core.FlightUtility.GetAirport(sseg.destinationAirport);

                sseg.travelDate = Convert.ToDateTime(arrStr[2]);// DateTime.Today.AddDays(150);
                fsr.segment.Add(sseg);

                if (fsr.segment[0].originAirport == fsr.segment[1].destinationAirport && fsr.segment[0].destinationAirport == fsr.segment[1].originAirport)
                    fsr.tripType = TripType.RoundTrip;
                else
                    fsr.tripType = TripType.MultiCity;
            }

            fsr.siteId = SiteId.FlightsMojoIN;
            fsr.cabinType = (CabinType)Convert.ToInt32((Request.QueryString["cabin"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("cabin"))) ? Request.QueryString.Get("cabin") : "1");
            fsr.airline = (Request.QueryString["airline"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("airline"))) ? Request.QueryString.Get("airline").ToUpper() : "ALL";

            fsr.searchDirectFlight = false;
            fsr.flexibleSearch = false;
            fsr.isNearBy = false;

            fsr.sourceMedia = Request.QueryString.Get("campain");
            if ((fsr.sourceMedia.Equals("1001") || fsr.sourceMedia.Equals("1002")) && Request.QueryString["utm_source"] != null)
            {
                fsr.sourceMedia = Request.QueryString["utm_source"] != null ? Request.QueryString.Get("utm_source").ToString() : "";
            }
            int intSmedia;
            bool bNum = int.TryParse(fsr.sourceMedia, out intSmedia);
            fsr.sourceMedia = bNum ? intSmedia.ToString() : "1000";
            setCookie(fsr.sourceMedia);
            fsr.sID = Request.QueryString.Get("TranId");
            fsr.rID = Request.QueryString.Get("Id");
            fsr.utm_campaign = "";
            fsr.utm_medium = "";
            fsr.currencyCode = (Request.QueryString["currency"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("currency"))) ? Request.QueryString.Get("currency").ToUpper() : "INR";
            if (fsr.sourceMedia.Equals("1015"))
            {
                fsr.redirectID = (Request.QueryString["skyscanner_redirectid"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("skyscanner_redirectid"))) ? Request.QueryString.Get("skyscanner_redirectid") : "";
            }
            else if (fsr.sourceMedia.Equals("1001") || fsr.sourceMedia.Equals("1002"))
            {
                fsr.redirectID = Request.QueryString["wego_click_id"] != null ? Request.QueryString.Get("wego_click_id").ToString() : "";
            }
            else if (fsr.sourceMedia.Equals("1013"))
            {
                fsr.redirectID = Request.QueryString["kayakclickid"] != null ? Request.QueryString.Get("kayakclickid").ToString() : "";
            }
            else
            {
                fsr.redirectID = (Request.QueryString["redirectID"] != null && !string.IsNullOrEmpty(Request.QueryString.Get("redirectID"))) ? Request.QueryString.Get("redirectID") : "";
            }

            #endregion
            fsr.userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            fsr.device = GetDevice();
            fsr.deepLink = "/flight/searchFlightResult?org=" + fsr.segment[0].originAirport + "&dest=" + fsr.segment[0].destinationAirport + "&depdate=" + fsr.segment[0].travelDate.ToString("dd-MM-yyyy") + "&retdate=" + (fsr.segment.Count > 1 ? fsr.segment[1].travelDate.ToString("dd-MM-yyyy") : "") + "&tripType=" + (fsr.segment.Count > 1 ? "R" : "O") + "&adults=" + fsr.adults + "&child=" + fsr.child + "&infants=" + fsr.infants + "&cabin=" + ((int)fsr.cabinType) + "&utm_source=" + fsr.sourceMedia + "&currency=inr";
            fsr.deepLink += (string.IsNullOrEmpty(fsr.redirectID) ? "" : ("&redirectID=" + fsr.redirectID));

            fsr.deepLink += (string.IsNullOrEmpty(fsr.sID) ? "" : ("&TranId=" + fsr.sID));
            fsr.deepLink += (string.IsNullOrEmpty(fsr.rID) ? "" : ("&Id=" + fsr.rID));


            fsr.userLogID = string.IsNullOrEmpty(fsr.sID) ? getSearchID() : fsr.sID;
            fsr.userSearchID = getSearchID();
            fsr.userSessionID = Session.SessionID;
            AirContext airContext = new AirContext(fsr.userIP);
            airContext.flightSearchRequest = fsr;
            airContext.IsSearchCompleted = false;
            airContext.flightRef = new List<string>();

            FlightOperation.SetAirContext(airContext);
            fsr.tgy_Request_id = DateTime.Now.ToString("ddMMyyyyHHmmsss");
            stopwatch.Stop();

            new Bal.FlightDetails().SearchFlight(ref airContext, GlobalData.isDummyResult);

            if (airContext.flightSearchResponse.response.status == TransactionStatus.Success && checkIsFareExist(ref airContext))
            {
                return Redirect("/Flight/Passenger/" + fsr.userSearchID + "/" + airContext.flightSearchResponse.result1Index + (string.IsNullOrEmpty(airContext.flightSearchResponse.result2Index) ? "" : ("/" + airContext.flightSearchResponse.result2Index)));
            }
            else
            {
                return Redirect("/Flight/Result/" + fsr.userSearchID);
            }
        }

        [HttpGet]
        public ActionResult GetResearch(string ID)
        {

            DAL.Deal.WebsiteDeal objResearch = new DAL.Deal.WebsiteDeal();
            var kk = objResearch.GetResearch(ID);

            FlightSearchRequest fsr = null;
            fsr = new FlightSearchRequest();
            fsr.segment = new List<SearchSegment>();



            fsr.tripType = (TripType)Convert.ToByte(kk.TripType);
            SearchSegment sseg = new SearchSegment();
            sseg.originAirport = GetAirportCode(kk.Origin);
            sseg.orgArp = FlightUtility.GetAirport(sseg.originAirport);
            sseg.destinationAirport = GetAirportCode(kk.Destination);
            sseg.travelDate = kk.TravelDate;
            // sseg.travelDate = DateTime.ParseExact(kk.TravelDate.ToString(), "dd MMM yy", new CultureInfo("en-US"));
            sseg.destArp = FlightUtility.GetAirport(sseg.destinationAirport);
            fsr.segment.Add(sseg);
            if (sseg.orgArp.countryCode.ToUpper() == "IN" && sseg.destArp.countryCode.ToUpper() == "IN")
                fsr.travelType = TravelType.Domestic;
            else
                fsr.travelType = TravelType.International;
            if (fsr.tripType == TripType.RoundTrip)
            {
                SearchSegment sseg1 = new SearchSegment();
                sseg1.originAirport = GetAirportCode(kk.Destination);
                sseg1.orgArp = FlightUtility.GetAirport(sseg1.originAirport);
                sseg1.destinationAirport = GetAirportCode(kk.Origin);
                sseg1.travelDate = DateTime.ParseExact(kk.ReturnDate.ToString(), "dd MMM yy", new CultureInfo("en-US"));

                sseg1.destArp = FlightUtility.GetAirport(sseg1.destinationAirport);
                fsr.segment.Add(sseg1);
            }

            fsr.cabinType = (CabinType)Convert.ToByte(kk.CabinType);


            fsr.airline = "ALL";

            fsr.adults = kk.adults != 0 ? Convert.ToByte(kk.adults.ToString()) : (byte)0;
            fsr.child = kk.child != 0 ? Convert.ToByte(kk.child.ToString()) : (byte)0;
            fsr.infants = kk.infants != 0 ? Convert.ToByte(kk.infants.ToString()) : (byte)0;
            //fsr.searchDirectFlight = FormColl["NonStop"] != null ? true : false;
            //fsr.flexibleSearch = FormColl["FlexDate"] != null ? true : false;
            //fsr.isNearBy = FormColl["NearByAirport"] != null ? true : false;
            fsr.currencyCode = "INR";
            fsr.siteId = SiteId.FlightsMojoIN;
            fsr.sourceMedia = GetCookie();
            fsr.utm_campaign = "webengage";
            fsr.utm_medium = "retargeting";
            fsr.userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            fsr.tgy_Request_id = DateTime.Now.ToString("ddMMyyyHHmmsss");
            fsr.device = GetDevice();
            #region set kayakClickID for S2S Pixel
            if (Session["SearchDetails"] != null)
            {
                AirContext airContextTemp = Session["SearchDetails"] as AirContext;
                if (airContextTemp.flightSearchRequest != null && !string.IsNullOrEmpty(airContextTemp.flightSearchRequest.redirectID))
                {
                    fsr.redirectID = airContextTemp.flightSearchRequest.redirectID;
                }
            }
            #endregion

            fsr.deepLink = "/flight/searchFlightResult?org=" + fsr.segment[0].originAirport + "&dest=" + fsr.segment[0].destinationAirport + "&depdate=" + fsr.segment[0].travelDate.ToString("dd-MM-yyyy") + "&retdate=" + (fsr.segment.Count > 1 ? fsr.segment[1].travelDate.ToString("dd-MM-yyyy") : "") + "&tripType=" + (fsr.segment.Count > 1 ? "R" : "O") + "&adults=" + fsr.adults + "&child=" + fsr.child + "&infants=" + fsr.infants + "&cabin=" + ((int)fsr.cabinType) + "&utm_source=" + fsr.sourceMedia + "&currency=inr";

            AirContext airContext = new AirContext(fsr.userIP);
            airContext.isDomestic = (Core.FlightUtility.GetAirport(fsr.segment[0].originAirport).countryCode.ToUpper() == "IN" && Core.FlightUtility.GetAirport(fsr.segment[0].destinationAirport).countryCode.ToUpper() == "IN");
            airContext.flightSearchRequest = fsr;
            airContext.IsSearchCompleted = false;
            airContext.flightRef = new List<string>();
            fsr.userSessionID = Session.SessionID;
            fsr.userLogID = fsr.userSearchID = getSearchID();
            FlightOperation.SetAirContext(airContext);
            return Redirect("/Flight/Result/" + fsr.userSearchID);
        }



        public ActionResult Result(string ID)
        {
            try
            {
                AirContext airContext = FlightOperation.GetAirContext(ID);
                if (airContext != null)
                {
                    if (airContext.IsBookingCompleted)
                    {
                        return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                    }

                    ViewBag.isShowResult = false;
                    airContext.flightSearchRequest.matrixData = FlightOperation.GetMatrixFare(airContext.flightSearchRequest.segment[0].originAirport, airContext.flightSearchRequest.segment[0].destinationAirport, airContext.flightSearchRequest.segment[0].travelDate);
                    airContext.flightSearchRequest.matrixPos = (int)(airContext.flightSearchRequest.segment[0].travelDate - DateTime.Today).TotalDays;
                    if (airContext.flightSearchRequest.matrixPos <= 3)
                    {
                        airContext.flightSearchRequest.matrixPos = 0;
                    }
                    else if (airContext.flightSearchRequest.matrixPos <= 15)
                    {
                        airContext.flightSearchRequest.matrixPos -= 3;
                    }
                    else
                    {
                        airContext.flightSearchRequest.matrixPos = 12;
                    }
                    return View("Result", airContext.flightSearchRequest);
                }
                else { return Redirect("/"); }
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "ResultException" + DateTime.Today.ToString("ddMMyy"), "Error");
                return Redirect("/");
            }
        }
        public ActionResult NoResult(string ID)
        {
            try
            {
                AirContext airContext = FlightOperation.GetAirContext(ID);
                if (airContext != null)
                {
                    ViewBag.isShowResult = true;
                    if (airContext.IsBookingCompleted)
                    {
                        return Redirect("/flight/FlightConfirmation");
                    }
                    return View(airContext.flightSearchRequest);
                }
                else { return Redirect("/"); }
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "NoResultException" + DateTime.Today.ToString("ddMMyy"), "Error");
                return Redirect("/");
            }
        }

        public ActionResult flightnotmatch(string ID)
        {
            try
            {
                AirContext airContext = FlightOperation.GetAirContext(ID);
                if (airContext != null)
                {
                    ViewBag.isShowResult = true;
                    if (airContext.IsBookingCompleted)
                    {
                        return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                    }
                    return View(airContext.flightSearchRequest);
                }
                else { return Redirect("/"); }
            }
            catch (Exception ex)
            {
                return Redirect("/");
            }
        }
        public ActionResult PrePax(FormCollection fc)
        {
            string sMedia = fc["sMedia"];

            if (sMedia == "1037")
            {
                return Redirect("/flight/PassengerGF/" + fc["sID"].ToString() + "/" + fc["oID"].ToString() + (string.IsNullOrEmpty(fc["iID"].ToString()) ? "" : ("/" + fc["iID"].ToString())));
            }
            else
            {
                return Redirect("/flight/Passenger/" + fc["sID"].ToString() + "/" + fc["oID"].ToString() + (string.IsNullOrEmpty(fc["iID"].ToString()) ? "" : ("/" + fc["iID"].ToString())));
            }
        }
        [HttpGet]
        public ActionResult Passenger(string ID, string lid, string rid)
        {
            ViewBag.isShowResult = true;
            StringBuilder sbLogger = new StringBuilder();
            AirContext airContext = FlightOperation.GetAirContext(ID);
            try
            {
                if (airContext != null || (!string.IsNullOrEmpty(lid)))
                {
                    if (airContext.IsBookingCompleted)
                    {
                        return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                    }
                    if (airContext.flightSearchResponse != null && airContext.flightSearchResponse.Results != null && airContext.flightSearchResponse.Results.Count > 0)
                    {
                        airContext.flightBookingRequest = new FlightBookingRequest()
                        {
                            airline = airContext.flightSearchResponse.airline,
                            airport = airContext.flightSearchResponse.airport,
                            aircraftDetail = airContext.flightSearchResponse.aircraftDetail,
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID,
                            flightResult = new List<FlightResult>(),
                            currencyCode = airContext.flightSearchRequest.currencyCode,
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            TvoTraceId = airContext.flightSearchResponse.TraceId,
                            CouponAmount = 0,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            BrowserDetails = System.Web.HttpContext.Current.Request.Headers["User-Agent"],
                            siteID = airContext.flightSearchRequest.siteId,
                            paymentDetails = new PaymentDetails(),
                            tgy_Request_id = airContext.flightSearchRequest.tgy_Request_id,
                            tgy_Search_Key = airContext.flightSearchResponse.tgy_Search_Key,
                            FB_booking_token_id = airContext.flightSearchResponse.FB_booking_token_id,
                            responseStatus = new ResponseStatus(),
                            affiliate = airContext.flightSearchResponse.affiliate,
                            redirectID = airContext.flightSearchRequest.redirectID,
                            isBuyCancellaionPolicy = false,
                            device = GetDevice(),
                            utm_medium = airContext.flightSearchRequest.utm_medium,
                            utm_campaign = airContext.flightSearchRequest.utm_campaign,
                        };
                        try
                        {
                            int searID = new DAL.ShortLink.DalShortLinkOperation().SaveSearchDetails(StringHelper.CompressString(JsonConvert.SerializeObject(airContext.flightSearchRequest)));
                            airContext.flightBookingRequest.tinyUrlID = Base64Encode(searID.ToString());
                            //airContext.flightBookingRequest.utm_medium = "retrageting";
                            //airContext.flightBookingRequest.utm_campaign = "webengage";
                        }
                        catch { }
                        bookingLog(ref sbLogger, "Flight Booking Request", JsonConvert.SerializeObject(airContext.flightBookingRequest));
                        airContext.flightBookingRequest.deepLink = airContext.flightSearchRequest.deepLink;
                        airContext.flightBookingRequest.device = airContext.flightSearchRequest.device;
                        #region set View bag for session Time out
                        if (airContext.flightSearchRequest.tripType != TripType.MultiCity)
                        {
                            ViewBag.org = airContext.flightSearchRequest.segment[0].originAirport;
                            ViewBag.dest = airContext.flightSearchRequest.segment[0].destinationAirport;
                            ViewBag.depDate = airContext.flightSearchRequest.segment[0].travelDate.ToString("yyyy-MM-dd");
                            ViewBag.retDate = airContext.flightSearchRequest.segment.Count == 1 ? airContext.flightSearchRequest.segment[0].travelDate.AddDays(7).ToString("yyyy-MM-dd") : airContext.flightSearchRequest.segment[1].travelDate.ToString("yyyy-MM-dd");
                            ViewBag.tripType = ((int)airContext.flightSearchRequest.tripType).ToString();
                        }
                        else
                        {
                            ViewBag.org1 = airContext.flightSearchRequest.segment[0].originAirport;
                            ViewBag.dest1 = airContext.flightSearchRequest.segment[0].destinationAirport;
                            ViewBag.depDate1 = airContext.flightSearchRequest.segment[0].travelDate.ToString("yyyy-MM-dd");
                            ViewBag.org2 = airContext.flightSearchRequest.segment[1].originAirport;
                            ViewBag.dest2 = airContext.flightSearchRequest.segment[1].destinationAirport;
                            ViewBag.depDate2 = airContext.flightSearchRequest.segment[1].travelDate.ToString("yyyy-MM-dd");

                            if (airContext.flightSearchRequest.segment.Count >= 3)
                            {
                                ViewBag.org3 = airContext.flightSearchRequest.segment[2].originAirport;
                                ViewBag.dest3 = airContext.flightSearchRequest.segment[2].destinationAirport;
                                ViewBag.depDate3 = airContext.flightSearchRequest.segment[2].travelDate.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                ViewBag.org3 = "";
                                ViewBag.dest3 = "";
                                ViewBag.depDate3 = "";
                            }
                            if (airContext.flightSearchRequest.segment.Count >= 4)
                            {
                                ViewBag.org4 = airContext.flightSearchRequest.segment[3].originAirport;
                                ViewBag.dest4 = airContext.flightSearchRequest.segment[3].destinationAirport;
                                ViewBag.depDate4 = airContext.flightSearchRequest.segment[3].travelDate.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                ViewBag.org4 = "";
                                ViewBag.dest4 = "";
                                ViewBag.depDate4 = "";
                            }
                            ViewBag.tripType = ((int)airContext.flightSearchRequest.tripType).ToString();
                            ViewBag.totTrip = airContext.flightSearchRequest.segment.Count.ToString();
                        }
                        #endregion

                        #region Set Selected Flights

                        FlightResult outBound = airContext.flightSearchResponse.Results.FirstOrDefault().Where(o => o.ResultID == lid).ToList()[0];
                        airContext.flightBookingRequest.flightResult.Add(Clone<FlightResult>(outBound));
                        if (!string.IsNullOrEmpty(rid))
                        {
                            FlightResult inBound = airContext.flightSearchResponse.Results.LastOrDefault().Where(o => o.ResultID == rid).ToList()[0];
                            airContext.flightBookingRequest.flightResult.Add(Clone<FlightResult>(inBound));
                            TimeSpan ts = airContext.flightBookingRequest.flightResult[1].FlightSegments[0].Segments[0].DepTime - airContext.flightBookingRequest.flightResult[0].FlightSegments[0].Segments.Last().ArrTime;
                            if (ts.TotalMinutes < 240)
                            {
                                return Redirect("/Flight/Result/" + airContext.flightSearchRequest.userSearchID + "?error=1");
                            }
                        }
                        #endregion

                        #region Set Sum fare
                        airContext.flightBookingRequest.sumFare = new Fare() { fareBreakdown = new List<FareBreakdown>() };
                        airContext.flightBookingRequest.PriceID = new List<string>();
                        foreach (var item in airContext.flightBookingRequest.flightResult)
                        {
                            Fare fare = item.Fare;
                            if (item.Fare.gdsType == GdsType.Travelogy)
                            {
                                airContext.flightBookingRequest.PriceID.Add(fare.Tgy_FareID);
                            }
                            else
                            {
                                airContext.flightBookingRequest.PriceID.Add(fare.tjID);
                            }

                            airContext.flightBookingRequest.sumFare.subProvider = fare.subProvider;

                            airContext.flightBookingRequest.sumFare.Currency = fare.Currency;
                            airContext.flightBookingRequest.sumFare.NetFare += fare.NetFare;
                            airContext.flightBookingRequest.sumFare.PublishedFare += fare.PublishedFare;
                            airContext.flightBookingRequest.sumFare.grandTotal += fare.grandTotal;
                            airContext.flightBookingRequest.sumFare.ffDiscount += fare.ffDiscount;
                            airContext.flightBookingRequest.sumFare.BaseFare += fare.BaseFare;
                            airContext.flightBookingRequest.sumFare.Tax += fare.Tax;
                            airContext.flightBookingRequest.sumFare.AdditionalTxnFeeOfrd += fare.AdditionalTxnFeeOfrd;
                            airContext.flightBookingRequest.sumFare.AdditionalTxnFeePub += fare.AdditionalTxnFeePub;
                            airContext.flightBookingRequest.sumFare.Discount += fare.Discount;
                            airContext.flightBookingRequest.sumFare.Markup += fare.Markup;
                            airContext.flightBookingRequest.sumFare.OfferedFare += fare.OfferedFare;
                            airContext.flightBookingRequest.sumFare.OtherCharges += fare.OtherCharges;
                            airContext.flightBookingRequest.sumFare.ServiceFee += fare.ServiceFee;
                            airContext.flightBookingRequest.sumFare.CommissionEarned += fare.CommissionEarned;
                            airContext.flightBookingRequest.sumFare.TdsOnCommission += fare.TdsOnCommission;
                            airContext.flightBookingRequest.sumFare.TdsOnIncentive += fare.TdsOnIncentive;
                            airContext.flightBookingRequest.sumFare.TdsOnPLB += fare.TdsOnPLB;
                            airContext.flightBookingRequest.sumFare.YQTax += fare.YQTax;
                            airContext.flightBookingRequest.sumFare.pLBEarned += fare.pLBEarned;

                            if (airContext.flightBookingRequest.sumFare.fareBreakdown.Count == 0)
                            {
                                foreach (FareBreakdown fb in fare.fareBreakdown)
                                {
                                    airContext.flightBookingRequest.sumFare.fareBreakdown.Add(new FareBreakdown());
                                }
                            }
                            for (int i = 0; i < fare.fareBreakdown.Count; i++)
                            {
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].BaseFare += fare.fareBreakdown[i].BaseFare;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].Tax += fare.fareBreakdown[i].Tax;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].Markup += fare.fareBreakdown[i].Markup;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].PassengerType = item.Fare.fareBreakdown[i].PassengerType;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].YQTax += fare.fareBreakdown[i].YQTax;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeeOfrd += fare.fareBreakdown[i].AdditionalTxnFeeOfrd;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeePub += fare.fareBreakdown[i].AdditionalTxnFeePub;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].PGCharge += fare.fareBreakdown[i].PGCharge;
                            }
                        }
                        #endregion
                        int TotPax = (airContext.flightBookingRequest.adults + airContext.flightBookingRequest.child + airContext.flightBookingRequest.infants);
                        if (airContext.flightSearchRequest.travelType == TravelType.Domestic)
                        {
                            airContext.flightBookingRequest.RefundPolicyAmt = (airContext.flightBookingRequest.flightResult.Count * TotPax) * (Convert.ToDecimal(ConfigurationManager.AppSettings["RefundPolicyAmt"]));
                            airContext.flightBookingRequest.CancellaionPolicyAmt = (airContext.flightBookingRequest.flightResult.Count * TotPax) * (Convert.ToDecimal(ConfigurationManager.AppSettings["CancellaionPolicyAmt"]));
                        }
                        else
                        {
                            airContext.flightBookingRequest.RefundPolicyAmt = airContext.flightBookingRequest.sumFare.grandTotal * .1m; //(airContext.flightBookingRequest.flightResult.Count * TotPax) * (Convert.ToDecimal(ConfigurationManager.AppSettings["RefundPolicyAmt"]));
                            airContext.flightBookingRequest.CancellaionPolicyAmt = airContext.flightBookingRequest.sumFare.grandTotal * .1m;//(airContext.flightBookingRequest.flightResult.Count * TotPax) * (Convert.ToDecimal(ConfigurationManager.AppSettings["CancellaionPolicyAmt"]));
                        }
                        airContext.flightBookingRequest.currencyCode = airContext.flightSearchRequest.currencyCode;
                        airContext.flightBookingRequest.adults = airContext.flightSearchResponse.adults;
                        airContext.flightBookingRequest.child = airContext.flightSearchResponse.child;
                        airContext.flightBookingRequest.infants = airContext.flightSearchResponse.infants;
                        airContext.flightBookingRequest.travelType = airContext.flightSearchRequest.travelType;
                        airContext.flightBookingRequest.passengerDetails = GetPassengerDefault(airContext.flightSearchRequest);
                        airContext.flightBookingRequest.LastCheckInDate = airContext.flightBookingRequest.flightResult.LastOrDefault().FlightSegments.LastOrDefault().Segments.FirstOrDefault().DepTime;
                        //   string jsonString = JsonConvert.SerializeObject(airContext.flightBookingRequest);
                        return View(airContext.flightBookingRequest);
                    }
                    else { return Redirect("/"); }
                }
                else { return Redirect("/"); }
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "GetPassengerException" + DateTime.Today.ToString("ddMMyy"), "Error");
                return Redirect("/");
            }
        }
        [HttpGet]
        public ActionResult PassengerGF(string ID, string lid, string rid)
        {
            ViewBag.isShowResult = true;
            StringBuilder sbLogger = new StringBuilder();
            try
            {
                AirContext airContext = FlightOperation.GetAirContext(ID);
                if (airContext != null || (!string.IsNullOrEmpty(lid)))
                {
                    if (airContext.IsBookingCompleted)
                    {
                        return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                    }
                    if (airContext.flightSearchResponse != null && airContext.flightSearchResponse.Results != null && airContext.flightSearchResponse.Results.Count > 0)
                    {
                        airContext.flightBookingRequest = new FlightBookingRequest()
                        {
                            airline = airContext.flightSearchResponse.airline,
                            airport = airContext.flightSearchResponse.airport,
                            aircraftDetail = airContext.flightSearchResponse.aircraftDetail,
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID,
                            flightResult = new List<FlightResult>(),
                            currencyCode = airContext.flightSearchRequest.currencyCode,
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            TvoTraceId = airContext.flightSearchResponse.TraceId,
                            CouponAmount = 0,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            BrowserDetails = System.Web.HttpContext.Current.Request.Headers["User-Agent"],
                            siteID = airContext.flightSearchRequest.siteId,
                            paymentDetails = new PaymentDetails(),
                            tgy_Request_id = airContext.flightSearchRequest.tgy_Request_id,
                            tgy_Search_Key = airContext.flightSearchResponse.tgy_Search_Key,
                            FB_booking_token_id = airContext.flightSearchResponse.FB_booking_token_id,
                            responseStatus = new ResponseStatus(),
                            affiliate = airContext.flightSearchResponse.affiliate,
                            redirectID = airContext.flightSearchRequest.redirectID,
                            isBuyCancellaionPolicy = false
                        };
                        bookingLog(ref sbLogger, "Flight Booking Request", JsonConvert.SerializeObject(airContext.flightBookingRequest));
                        airContext.flightBookingRequest.deepLink = airContext.flightSearchRequest.deepLink;
                        #region set View bag for session Time out
                        if (airContext.flightSearchRequest.tripType != TripType.MultiCity)
                        {
                            ViewBag.org = airContext.flightSearchRequest.segment[0].originAirport;
                            ViewBag.dest = airContext.flightSearchRequest.segment[0].destinationAirport;
                            ViewBag.depDate = airContext.flightSearchRequest.segment[0].travelDate.ToString("yyyy-MM-dd");
                            ViewBag.retDate = airContext.flightSearchRequest.segment.Count == 1 ? airContext.flightSearchRequest.segment[0].travelDate.AddDays(7).ToString("yyyy-MM-dd") : airContext.flightSearchRequest.segment[1].travelDate.ToString("yyyy-MM-dd");
                            ViewBag.tripType = ((int)airContext.flightSearchRequest.tripType).ToString();
                        }
                        else
                        {
                            ViewBag.org1 = airContext.flightSearchRequest.segment[0].originAirport;
                            ViewBag.dest1 = airContext.flightSearchRequest.segment[0].destinationAirport;
                            ViewBag.depDate1 = airContext.flightSearchRequest.segment[0].travelDate.ToString("yyyy-MM-dd");
                            ViewBag.org2 = airContext.flightSearchRequest.segment[1].originAirport;
                            ViewBag.dest2 = airContext.flightSearchRequest.segment[1].destinationAirport;
                            ViewBag.depDate2 = airContext.flightSearchRequest.segment[1].travelDate.ToString("yyyy-MM-dd");

                            if (airContext.flightSearchRequest.segment.Count >= 3)
                            {
                                ViewBag.org3 = airContext.flightSearchRequest.segment[2].originAirport;
                                ViewBag.dest3 = airContext.flightSearchRequest.segment[2].destinationAirport;
                                ViewBag.depDate3 = airContext.flightSearchRequest.segment[2].travelDate.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                ViewBag.org3 = "";
                                ViewBag.dest3 = "";
                                ViewBag.depDate3 = "";
                            }
                            if (airContext.flightSearchRequest.segment.Count >= 4)
                            {
                                ViewBag.org4 = airContext.flightSearchRequest.segment[3].originAirport;
                                ViewBag.dest4 = airContext.flightSearchRequest.segment[3].destinationAirport;
                                ViewBag.depDate4 = airContext.flightSearchRequest.segment[3].travelDate.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                ViewBag.org4 = "";
                                ViewBag.dest4 = "";
                                ViewBag.depDate4 = "";
                            }
                            ViewBag.tripType = ((int)airContext.flightSearchRequest.tripType).ToString();
                            ViewBag.totTrip = airContext.flightSearchRequest.segment.Count.ToString();
                        }
                        #endregion

                        #region Set Selected Flights

                        FlightResult outBound = airContext.flightSearchResponse.Results.FirstOrDefault().Where(o => o.ResultID == lid).ToList()[0];
                        airContext.flightBookingRequest.flightResult.Add(Clone<FlightResult>(outBound));
                        if (!string.IsNullOrEmpty(rid))
                        {
                            FlightResult inBound = airContext.flightSearchResponse.Results.LastOrDefault().Where(o => o.ResultID == rid).ToList()[0];
                            airContext.flightBookingRequest.flightResult.Add(Clone<FlightResult>(inBound));
                            TimeSpan ts = airContext.flightBookingRequest.flightResult[1].FlightSegments[0].Segments[0].DepTime - airContext.flightBookingRequest.flightResult[0].FlightSegments[0].Segments.Last().ArrTime;
                            if (ts.TotalMinutes < 240)
                            {
                                return Redirect("/Flight/Result/" + airContext.flightSearchRequest.userSearchID + "?error=1");
                            }
                        }
                        #endregion

                        #region Set Sum fare
                        airContext.flightBookingRequest.sumFare = new Fare() { fareBreakdown = new List<FareBreakdown>() };
                        airContext.flightBookingRequest.PriceID = new List<string>();
                        foreach (var item in airContext.flightBookingRequest.flightResult)
                        {
                            Fare fare = item.Fare;
                            if (item.Fare.gdsType == GdsType.Travelogy)
                            {
                                airContext.flightBookingRequest.PriceID.Add(fare.Tgy_FareID);
                            }
                            else
                            {
                                airContext.flightBookingRequest.PriceID.Add(fare.tjID);
                            }

                            airContext.flightBookingRequest.sumFare.subProvider = fare.subProvider;

                            airContext.flightBookingRequest.sumFare.Currency = fare.Currency;
                            airContext.flightBookingRequest.sumFare.NetFare += fare.NetFare;
                            airContext.flightBookingRequest.sumFare.PublishedFare += fare.PublishedFare;
                            airContext.flightBookingRequest.sumFare.grandTotal += fare.grandTotal;
                            airContext.flightBookingRequest.sumFare.ffDiscount += fare.ffDiscount;
                            airContext.flightBookingRequest.sumFare.BaseFare += fare.BaseFare;
                            airContext.flightBookingRequest.sumFare.Tax += fare.Tax;
                            airContext.flightBookingRequest.sumFare.AdditionalTxnFeeOfrd += fare.AdditionalTxnFeeOfrd;
                            airContext.flightBookingRequest.sumFare.AdditionalTxnFeePub += fare.AdditionalTxnFeePub;
                            airContext.flightBookingRequest.sumFare.Discount += fare.Discount;
                            airContext.flightBookingRequest.sumFare.Markup += fare.Markup;
                            airContext.flightBookingRequest.sumFare.OfferedFare += fare.OfferedFare;
                            airContext.flightBookingRequest.sumFare.OtherCharges += fare.OtherCharges;
                            airContext.flightBookingRequest.sumFare.ServiceFee += fare.ServiceFee;
                            airContext.flightBookingRequest.sumFare.CommissionEarned += fare.CommissionEarned;
                            airContext.flightBookingRequest.sumFare.TdsOnCommission += fare.TdsOnCommission;
                            airContext.flightBookingRequest.sumFare.TdsOnIncentive += fare.TdsOnIncentive;
                            airContext.flightBookingRequest.sumFare.TdsOnPLB += fare.TdsOnPLB;
                            airContext.flightBookingRequest.sumFare.YQTax += fare.YQTax;

                            if (airContext.flightBookingRequest.sumFare.fareBreakdown.Count == 0)
                            {
                                foreach (FareBreakdown fb in fare.fareBreakdown)
                                {
                                    airContext.flightBookingRequest.sumFare.fareBreakdown.Add(new FareBreakdown());
                                }
                            }
                            for (int i = 0; i < fare.fareBreakdown.Count; i++)
                            {
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].BaseFare += fare.fareBreakdown[i].BaseFare;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].Tax += fare.fareBreakdown[i].Tax;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].Markup += fare.fareBreakdown[i].Markup;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].PassengerType = item.Fare.fareBreakdown[i].PassengerType;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].YQTax += fare.fareBreakdown[i].YQTax;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeeOfrd += fare.fareBreakdown[i].AdditionalTxnFeeOfrd;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeePub += fare.fareBreakdown[i].AdditionalTxnFeePub;
                                airContext.flightBookingRequest.sumFare.fareBreakdown[i].PGCharge += fare.fareBreakdown[i].PGCharge;
                            }
                        }
                        #endregion

                        airContext.flightBookingRequest.RefundPolicyAmt = (airContext.flightBookingRequest.flightResult.Count) * (Convert.ToDecimal(ConfigurationManager.AppSettings["RefundPolicyAmt"]));
                        airContext.flightBookingRequest.CancellaionPolicyAmt = (airContext.flightBookingRequest.flightResult.Count) * (Convert.ToDecimal(ConfigurationManager.AppSettings["CancellaionPolicyAmt"]));


                        airContext.flightBookingRequest.currencyCode = airContext.flightSearchRequest.currencyCode;
                        airContext.flightBookingRequest.adults = airContext.flightSearchResponse.adults;
                        airContext.flightBookingRequest.child = airContext.flightSearchResponse.child;
                        airContext.flightBookingRequest.infants = airContext.flightSearchResponse.infants;
                        airContext.flightBookingRequest.travelType = airContext.flightSearchRequest.travelType;
                        airContext.flightBookingRequest.passengerDetails = GetPassengerDefault(airContext.flightSearchRequest);
                        airContext.flightBookingRequest.LastCheckInDate = airContext.flightBookingRequest.flightResult.LastOrDefault().FlightSegments.LastOrDefault().Segments.FirstOrDefault().DepTime;
                        //   string jsonString = JsonConvert.SerializeObject(airContext.flightBookingRequest);
                        return View(airContext.flightBookingRequest);
                    }
                    else { return Redirect("/"); }
                }
                else { return Redirect("/"); }
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "GetPassengerException" + DateTime.Today.ToString("ddMMyy"), "Error");
                return Redirect("/");
            }
        }
        public JsonResult VerifyPrice(string ID)
        {
            VerifyPriceResponse objVps = new VerifyPriceResponse();
            objVps.RedirectUrl = "";
            StringBuilder sbLogger = new StringBuilder();
            AirContext airContext = FlightOperation.GetAirContext(ID);

            if (airContext != null)
            {
                VerifyFareDetails vfd = new VerifyFareDetails()
                {
                    userSearchID = airContext.flightSearchRequest.userSearchID,
                    FirstSearchDate = airContext.flightSearchResponse.searchDate,
                    SecondSearchDate = airContext.flightSearchResponse.searchDate,
                    PreviousAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount),
                    FirstSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID),
                    Origin = airContext.flightSearchRequest.segment[0].originAirport,
                    Destination = airContext.flightSearchRequest.segment[0].destinationAirport,
                };

                if (airContext.IsBookingCompleted)
                {
                    objVps.RedirectUrl = "/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID;
                }
                else
                {
                    bookingLog(ref sbLogger, "Session Data", JsonConvert.SerializeObject(airContext));
                    bookingLog(ref sbLogger, "Old Fare ID", airContext.flightBookingRequest.flightResult[0].Fare.tjID);
                    if (false && airContext.flightSearchResponse.searchDate < DateTime.Now.AddMinutes(-10))
                    {
                        bookingLog(ref sbLogger, "Old Search", "Current Time:-" + DateTime.Now.ToLongDateString() + ",   Search Time:-" + airContext.flightSearchResponse.searchDate.ToLongDateString());
                        airContext.flightSearchRequest.isGetLiveFare = true;
                        int ctr = 0;
                        ReSearch:
                        FlightSearchResponse SearchResponse = new Bal.FlightDetails().ReSearchFlight(airContext.flightSearchRequest);

                        bookingLog(ref sbLogger, "New Search Data", JsonConvert.SerializeObject(SearchResponse));
                        if (SearchResponse != null && SearchResponse.Results != null && SearchResponse.Results.Count() > 0 && SearchResponse.Results[0].Count > 0 && SearchResponse.Results.LastOrDefault().Count > 0)
                        {
                            vfd.SecondSearchDate = SearchResponse.searchDate;
                            bookingLog(ref sbLogger, "Result not empty", "1");
                            List<FlightResult> resultNew = new List<FlightResult>();
                            for (int i = 0; i < airContext.flightBookingRequest.flightResult.Count; i++)
                            {
                                FlightResult fr = SearchResponse.Results[i].Where(k => k.ResultCombination == airContext.flightBookingRequest.flightResult[i].ResultCombination).FirstOrDefault();
                                if (fr != null)
                                {
                                    resultNew.Add(fr);
                                }
                            }
                            if (resultNew.Count == airContext.flightBookingRequest.flightResult.Count)
                            {
                                bookingLog(ref sbLogger, "New Fare ID", resultNew[0].Fare.tjID);
                                bookingLog(ref sbLogger, "all flight match", "1");
                                airContext.flightBookingRequest.flightResult = resultNew;
                                #region Set Sum fare
                                airContext.flightBookingRequest.sumFare = new Fare() { fareBreakdown = new List<FareBreakdown>() };
                                airContext.flightBookingRequest.PriceID = new List<string>();
                                foreach (var item in airContext.flightBookingRequest.flightResult)
                                {
                                    Fare fare = item.Fare;
                                    airContext.flightBookingRequest.PriceID.Add(fare.tjID);
                                    airContext.flightBookingRequest.sumFare.Currency = fare.Currency;
                                    airContext.flightBookingRequest.sumFare.NetFare += fare.NetFare;
                                    airContext.flightBookingRequest.sumFare.PublishedFare += fare.PublishedFare;
                                    airContext.flightBookingRequest.sumFare.grandTotal += fare.grandTotal;
                                    airContext.flightBookingRequest.sumFare.ffDiscount += fare.ffDiscount;
                                    airContext.flightBookingRequest.sumFare.BaseFare += fare.BaseFare;
                                    airContext.flightBookingRequest.sumFare.Tax += fare.Tax;
                                    airContext.flightBookingRequest.sumFare.AdditionalTxnFeeOfrd += fare.AdditionalTxnFeeOfrd;
                                    airContext.flightBookingRequest.sumFare.AdditionalTxnFeePub += fare.AdditionalTxnFeePub;
                                    airContext.flightBookingRequest.sumFare.Discount += fare.Discount;
                                    airContext.flightBookingRequest.sumFare.Markup += fare.Markup;
                                    airContext.flightBookingRequest.sumFare.OfferedFare += fare.OfferedFare;
                                    airContext.flightBookingRequest.sumFare.OtherCharges += fare.OtherCharges;
                                    airContext.flightBookingRequest.sumFare.ServiceFee += fare.ServiceFee;
                                    airContext.flightBookingRequest.sumFare.TdsOnCommission += fare.TdsOnCommission;
                                    airContext.flightBookingRequest.sumFare.TdsOnIncentive += fare.TdsOnIncentive;
                                    airContext.flightBookingRequest.sumFare.TdsOnPLB += fare.TdsOnPLB;
                                    airContext.flightBookingRequest.sumFare.YQTax += fare.YQTax;

                                    if (airContext.flightBookingRequest.sumFare.fareBreakdown.Count == 0)
                                    {
                                        foreach (FareBreakdown fb in fare.fareBreakdown)
                                        {
                                            airContext.flightBookingRequest.sumFare.fareBreakdown.Add(new FareBreakdown());
                                        }
                                    }
                                    for (int i = 0; i < fare.fareBreakdown.Count; i++)
                                    {
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].BaseFare += fare.fareBreakdown[i].BaseFare;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].Tax += fare.fareBreakdown[i].Tax;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].Markup += fare.fareBreakdown[i].Markup;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].PassengerType = item.Fare.fareBreakdown[i].PassengerType;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].YQTax += fare.fareBreakdown[i].YQTax;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeeOfrd += fare.fareBreakdown[i].AdditionalTxnFeeOfrd;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeePub += fare.fareBreakdown[i].AdditionalTxnFeePub;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].PGCharge += fare.fareBreakdown[i].PGCharge;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                bookingLog(ref sbLogger, "all not flight match", "1");
                                airContext.flightSearchResponse = SearchResponse;
                                objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                            }
                        }
                        else
                        {
                            if (ctr < 2)
                            {
                                bookingLog(ref sbLogger, "re search", ctr.ToString());
                                System.Threading.Thread.Sleep(1000);
                                ctr++;
                                goto ReSearch;
                            }
                        }
                    }

                    if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelogy)
                    {
                        #region Check Price Verification Travelogy
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID,
                            tgy_Search_Key = airContext.flightSearchResponse.tgy_Search_Key,
                            PhoneNo = "9876543210",
                            tgy_Request_id = airContext.flightSearchRequest.tgy_Request_id
                        };


                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount += airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice - airContext.flightBookingRequest.sumFare.NetFare;

                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount < 1000M)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }


                            airContext.flightBookingRequest.tgy_Flight_Key = airContext.priceVerificationResponse.fareQuoteResponse.tgy_Flight_Key;
                            airContext.flightBookingRequest.tgy_Block_Ticket_Allowed = airContext.priceVerificationResponse.fareQuoteResponse.tgy_Block_Ticket_Allowed;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }

                        else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelogy && airContext.priceVerificationResponse.fareQuoteResponse == null)
                        {
                            bookingLog(ref sbLogger, "TGY", JsonConvert.SerializeObject(airContext.priceVerificationResponse.fareQuoteResponse));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelogy && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "0028")
                        {
                            bookingLog(ref sbLogger, "TGY", JsonConvert.SerializeObject(airContext.priceVerificationResponse.fareQuoteResponse));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        if (airContext.flightBookingRequest.tgy_Flight_Key == null && airContext.flightBookingRequest.tgy_Block_Ticket_Allowed == null)
                        {
                            bookingLog(ref sbLogger, "TGY", JsonConvert.SerializeObject(airContext.flightBookingRequest.tgy_Block_Ticket_Allowed));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;

                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }

                        objVps.TaxWithMakrup = (((airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.sumFare.ServiceFee + airContext.flightBookingRequest.fareIncreaseAmount)));

                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.TripJack)
                    {
                        #region Check Price Verification Tripjack
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice - airContext.flightBookingRequest.sumFare.NetFare;

                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }

                        if (airContext.priceVerificationResponse.fareQuoteResponse.isRunFareQuoteFalse == true || airContext.priceVerificationResponse.fareQuoteResponse == null)
                        {
                            #region Research Flight
                            airContext.flightSearchRequest.isGetLiveFare = true;
                            int ctr = 0;
                            ReSearch:
                            FlightSearchResponse SearchResponse = new Bal.FlightDetails().ReSearchFlight(airContext.flightSearchRequest);

                            if (SearchResponse != null && SearchResponse.Results != null && SearchResponse.Results.Count() > 0 && SearchResponse.Results[0].Count > 0 && SearchResponse.Results.LastOrDefault().Count > 0)
                            {
                                vfd.SecondSearchDate = SearchResponse.searchDate;
                                List<FlightResult> resultNew = new List<FlightResult>();
                                for (int i = 0; i < airContext.flightBookingRequest.flightResult.Count; i++)
                                {
                                    FlightResult fr = SearchResponse.Results[i].Where(k => k.ResultCombination == airContext.flightBookingRequest.flightResult[i].ResultCombination).FirstOrDefault();
                                    if (fr != null)
                                    {
                                        resultNew.Add(fr);
                                    }
                                }
                                if (resultNew.Count == airContext.flightBookingRequest.flightResult.Count)
                                {
                                    airContext.flightBookingRequest.flightResult = resultNew;
                                    #region Set Sum fare
                                    airContext.flightBookingRequest.sumFare = new Fare() { fareBreakdown = new List<FareBreakdown>() };
                                    airContext.flightBookingRequest.PriceID = new List<string>();
                                    foreach (var item in airContext.flightBookingRequest.flightResult)
                                    {
                                        Fare fare = item.Fare;
                                        airContext.flightBookingRequest.PriceID.Add(fare.tjID);
                                        airContext.flightBookingRequest.sumFare.Currency = fare.Currency;
                                        airContext.flightBookingRequest.sumFare.NetFare += fare.NetFare;
                                        airContext.flightBookingRequest.sumFare.PublishedFare += fare.PublishedFare;
                                        airContext.flightBookingRequest.sumFare.grandTotal += fare.grandTotal;
                                        airContext.flightBookingRequest.sumFare.ffDiscount += fare.ffDiscount;
                                        airContext.flightBookingRequest.sumFare.BaseFare += fare.BaseFare;
                                        airContext.flightBookingRequest.sumFare.Tax += fare.Tax;
                                        airContext.flightBookingRequest.sumFare.AdditionalTxnFeeOfrd += fare.AdditionalTxnFeeOfrd;
                                        airContext.flightBookingRequest.sumFare.AdditionalTxnFeePub += fare.AdditionalTxnFeePub;
                                        airContext.flightBookingRequest.sumFare.Discount += fare.Discount;
                                        airContext.flightBookingRequest.sumFare.Markup += fare.Markup;
                                        airContext.flightBookingRequest.sumFare.OfferedFare += fare.OfferedFare;
                                        airContext.flightBookingRequest.sumFare.OtherCharges += fare.OtherCharges;
                                        airContext.flightBookingRequest.sumFare.ServiceFee += fare.ServiceFee;
                                        airContext.flightBookingRequest.sumFare.TdsOnCommission += fare.TdsOnCommission;
                                        airContext.flightBookingRequest.sumFare.TdsOnIncentive += fare.TdsOnIncentive;
                                        airContext.flightBookingRequest.sumFare.TdsOnPLB += fare.TdsOnPLB;
                                        airContext.flightBookingRequest.sumFare.YQTax += fare.YQTax;
                                        if (airContext.flightBookingRequest.sumFare.fareBreakdown.Count == 0)
                                        {
                                            foreach (FareBreakdown fb in fare.fareBreakdown)
                                            {
                                                airContext.flightBookingRequest.sumFare.fareBreakdown.Add(new FareBreakdown());
                                            }
                                        }
                                        for (int i = 0; i < fare.fareBreakdown.Count; i++)
                                        {
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].BaseFare += fare.fareBreakdown[i].BaseFare;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].Tax += fare.fareBreakdown[i].Tax;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].Markup += fare.fareBreakdown[i].Markup;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].PassengerType = item.Fare.fareBreakdown[i].PassengerType;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].YQTax += fare.fareBreakdown[i].YQTax;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeeOfrd += fare.fareBreakdown[i].AdditionalTxnFeeOfrd;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeePub += fare.fareBreakdown[i].AdditionalTxnFeePub;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].PGCharge += fare.fareBreakdown[i].PGCharge;
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    airContext.flightSearchResponse = SearchResponse;
                                    objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                                }
                            }
                            else
                            {
                                if (ctr < 2)
                                {
                                    System.Threading.Thread.Sleep(1000);
                                    ctr++;
                                    goto ReSearch;
                                }
                            }
                            #endregion

                            #region Re PriceVerification
                            airContext.priceVerificationRequest = new PriceVerificationRequest()
                            {
                                adults = airContext.flightSearchResponse.adults,
                                child = airContext.flightSearchResponse.child,
                                infants = airContext.flightSearchResponse.infants,
                                flightResult = airContext.flightBookingRequest.flightResult,
                                infantsWs = 0,
                                isFareQuote = true,
                                isFareRule = false,
                                isSSR = false,
                                PriceID = airContext.flightBookingRequest.PriceID,
                                siteID = airContext.flightSearchRequest.siteId,
                                sourceMedia = airContext.flightSearchRequest.sourceMedia,
                                TvoTraceId = "",
                                userIP = airContext.flightSearchRequest.userIP,
                                userSearchID = airContext.flightSearchRequest.userSearchID,
                                userLogID = airContext.flightSearchRequest.userLogID,
                                userSessionID = airContext.flightSearchRequest.userSessionID
                            };

                            new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                            if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                                airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                            {
                                airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice - airContext.flightBookingRequest.sumFare.NetFare;

                                if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                {
                                    if (airContext.flightBookingRequest.sourceMedia == "1015")
                                    {
                                        decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                        if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                        {
                                            airContext.flightBookingRequest.isFareChange = true;
                                            airContext.flightBookingRequest.isShowFareIncrease = true;
                                            airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                            bookingLog(ref sbLogger, "Re PriceVerification isFareChange", "true");
                                            bookingLog(ref sbLogger, "Re PriceVerification isShowFareIncrease", "true");
                                            bookingLog(ref sbLogger, "Re PriceVerification isMakeBookingInprogress", "false");
                                        }
                                        else
                                        {
                                            airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                            airContext.flightBookingRequest.isFareChange = true;
                                            airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                            bookingLog(ref sbLogger, "Re PriceVerification isFareChange else1", "0.00");
                                            bookingLog(ref sbLogger, "Re PriceVerification isShowFareIncrease else1", "true");
                                            bookingLog(ref sbLogger, "Re PriceVerification isMakeBookingInprogress else1", "true");
                                        }
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                        bookingLog(ref sbLogger, "Re PriceVerification isFareChange else2", "true");
                                        bookingLog(ref sbLogger, "Re PriceVerification isShowFareIncrease else2", "false");
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                    airContext.flightBookingRequest.isFareChange = false;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                                airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                                airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                                airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                            }
                            #endregion
                        }

                        if (airContext.priceVerificationResponse.fareQuoteResponse.isRunFareQuoteFalse == true || airContext.priceVerificationResponse.fareQuoteResponse == null || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            airContext.flightBookingRequest.isMakeBookingInprogress = true;
                        }

                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;

                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }

                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);

                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.FareBoutique)
                    {
                        #region Check Price Verification FareBoutique
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;

                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }

                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Tbo)
                    {
                        #region Check Price Verification TBO
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = true,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = airContext.flightSearchResponse.TraceId,
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };
                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                          airContext.priceVerificationResponse.fareQuoteResponse != null
                          && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }

                        if (airContext.priceVerificationResponse.fareQuoteResponse.isRunFareQuoteFalse == true || airContext.priceVerificationResponse.fareQuoteResponse == null)
                        {
                            airContext.flightBookingRequest.isMakeBookingInprogress = true;
                        }
                        bookingLog(ref sbLogger, "TBOBookingID", airContext.flightBookingRequest.TjBookingID);
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;
                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }

                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount + airContext.flightBookingRequest.sumFare.OtherCharges);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.OneDFare)
                    {
                        #region Check Price Verification OneDFare
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;

                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }
                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.SatkarTravel)
                    {
                        #region Check Price Verification SatkarTravel
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                            airContext.flightBookingRequest.STSessionID = airContext.priceVerificationResponse.fareQuoteResponse.STSessionID;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }
                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;
                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }
                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.AirIQ ||
                        airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Ease2Fly)
                    {
                        #region Check Price Verification AirIQ And Ease2Fly
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                            airContext.flightBookingRequest.STSessionID = airContext.priceVerificationResponse.fareQuoteResponse.STSessionID;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }
                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;
                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare) * (airContext.flightBookingRequest.adults);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare) * (airContext.flightBookingRequest.child);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare) * (airContext.flightBookingRequest.infants);
                        }
                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }

                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.GFS)
                    {
                        #region Check Price Verification GFS
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID,
                            origin = airContext.flightSearchRequest.segment[0].originAirport.ToLower(),
                            destination = airContext.flightSearchRequest.segment[0].destinationAirport.ToLower(),
                            depDate = airContext.flightSearchRequest.segment[0].travelDate.ToString("dd/MM/yyyy")
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                            airContext.flightBookingRequest.STSessionID = airContext.priceVerificationResponse.fareQuoteResponse.STSessionID;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }
                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;
                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare) * (airContext.flightBookingRequest.adults);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare) * (airContext.flightBookingRequest.child);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare) * (airContext.flightBookingRequest.infants);
                        }
                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Amadeus)
                    {
                        #region Check Price Verification Amadeus
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID,
                            origin = airContext.flightSearchRequest.segment[0].originAirport.ToLower(),
                            destination = airContext.flightSearchRequest.segment[0].destinationAirport.ToLower(),
                            depDate = airContext.flightSearchRequest.segment[0].travelDate.ToString("dd/MM/yyyy")
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                            airContext.flightBookingRequest.STSessionID = airContext.priceVerificationResponse.fareQuoteResponse.STSessionID;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }
                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;
                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare) * (airContext.flightBookingRequest.adults);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare) * (airContext.flightBookingRequest.child);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare) * (airContext.flightBookingRequest.infants);
                        }
                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelopedia)
                    {
                        #region Check Price Verification Travelopedia
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID,
                            tgy_Search_Key = airContext.flightSearchResponse.tgy_Search_Key,
                            PhoneNo = "9876543210",
                            tgy_Request_id = airContext.flightSearchRequest.tgy_Request_id
                        };


                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount += airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice - airContext.flightBookingRequest.sumFare.NetFare;

                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount < 1000M)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }


                            airContext.flightBookingRequest.tgy_Flight_Key = airContext.priceVerificationResponse.fareQuoteResponse.tgy_Flight_Key;
                            airContext.flightBookingRequest.tgy_Block_Ticket_Allowed = airContext.priceVerificationResponse.fareQuoteResponse.tgy_Block_Ticket_Allowed;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }

                        else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelopedia && airContext.priceVerificationResponse.fareQuoteResponse == null)
                        {
                            bookingLog(ref sbLogger, "TRP", JsonConvert.SerializeObject(airContext.priceVerificationResponse.fareQuoteResponse));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelopedia && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "0028")
                        {
                            bookingLog(ref sbLogger, "TRP", JsonConvert.SerializeObject(airContext.priceVerificationResponse.fareQuoteResponse));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        if (airContext.flightBookingRequest.tgy_Flight_Key == null && airContext.flightBookingRequest.tgy_Block_Ticket_Allowed == null)
                        {
                            bookingLog(ref sbLogger, "TRP", JsonConvert.SerializeObject(airContext.flightBookingRequest.tgy_Block_Ticket_Allowed));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;

                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }
                        objVps.TaxWithMakrup = (((airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.sumFare.ServiceFee + airContext.flightBookingRequest.fareIncreaseAmount)));
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }

                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.TripShope)
                    {
                        #region Check Price Verification TripShope
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID,
                            tgy_Search_Key = airContext.flightSearchResponse.tgy_Search_Key,
                            PhoneNo = "9876543210",
                            tgy_Request_id = airContext.flightSearchRequest.tgy_Request_id,
                            nextracustomstr = airContext.flightBookingRequest.flightResult[0].Fare.nextracustomstr,
                            nextraflightkey = airContext.flightBookingRequest.flightResult[0].Fare.nextraflightkey,
                            flightdeeplinkurl = airContext.flightBookingRequest.flightResult[0].Fare.flightdeeplinkurl,
                            TravelType = airContext.flightSearchRequest.travelType == TravelType.Domestic ? "domestic" : "international",
                            TripType = airContext.flightSearchRequest.tripType == TripType.OneWay ? "onward" : "return",
                            selectedflighttw = (airContext.flightSearchRequest.tripType == TripType.RoundTrip ? airContext.flightBookingRequest.flightResult[1].Fare.nextraflightkey : ""),
                        };


                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount += airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice - airContext.flightBookingRequest.sumFare.NetFare;

                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount < 1000M)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }

                            airContext.flightBookingRequest.BookingKey = airContext.priceVerificationResponse.fareQuoteResponse.BookingKey;
                            //  airContext.flightBookingRequest.tgy_Flight_Key = airContext.priceVerificationResponse.fareQuoteResponse.tgy_Flight_Key;
                            //   airContext.flightBookingRequest.tgy_Block_Ticket_Allowed = airContext.priceVerificationResponse.fareQuoteResponse.tgy_Block_Ticket_Allowed;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }




                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;

                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }
                        objVps.TaxWithMakrup = (((airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.sumFare.ServiceFee + airContext.flightBookingRequest.fareIncreaseAmount)));
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                }
                saveVerificationDetails(vfd);
                bookingLog(ref sbLogger, "Verification Response", JsonConvert.SerializeObject(objVps));
                CreateLogFile(sbLogger.ToString(), "NewLog\\Search", airContext.flightSearchRequest.userSearchID + ".txt");
                //   LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\VerifyPrice", "VerifyPrice" + DateTime.Today.ToString("ddMMMyy"), airContext.flightBookingRequest.userSearchID + ".txt");
            }
            else
            {
                objVps.RedirectUrl = "/";
            }
            return Json(objVps, JsonRequestBehavior.AllowGet);
        }
        public JsonResult VerifyPriceGF(string ID)
        {
            VerifyPriceResponse objVps = new VerifyPriceResponse();
            objVps.RedirectUrl = "";
            StringBuilder sbLogger = new StringBuilder();
            AirContext airContext = FlightOperation.GetAirContext(ID);

            if (airContext != null)
            {
                VerifyFareDetails vfd = new VerifyFareDetails()
                {
                    userSearchID = airContext.flightSearchRequest.userSearchID,
                    FirstSearchDate = airContext.flightSearchResponse.searchDate,
                    SecondSearchDate = airContext.flightSearchResponse.searchDate,
                    PreviousAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount),
                    FirstSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID),
                    Origin = airContext.flightSearchRequest.segment[0].originAirport,
                    Destination = airContext.flightSearchRequest.segment[0].destinationAirport,
                };

                if (airContext.IsBookingCompleted)
                {
                    objVps.RedirectUrl = "/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID;
                }
                else
                {
                    bookingLog(ref sbLogger, "Session Data", JsonConvert.SerializeObject(airContext));
                    bookingLog(ref sbLogger, "Old Fare ID", airContext.flightBookingRequest.flightResult[0].Fare.tjID);
                    if (false && airContext.flightSearchResponse.searchDate < DateTime.Now.AddMinutes(-10))
                    {
                        bookingLog(ref sbLogger, "Old Search", "Current Time:-" + DateTime.Now.ToLongDateString() + ",   Search Time:-" + airContext.flightSearchResponse.searchDate.ToLongDateString());
                        airContext.flightSearchRequest.isGetLiveFare = true;
                        int ctr = 0;
                        ReSearch:
                        FlightSearchResponse SearchResponse = new Bal.FlightDetails().ReSearchFlight(airContext.flightSearchRequest);

                        bookingLog(ref sbLogger, "New Search Data", JsonConvert.SerializeObject(SearchResponse));
                        if (SearchResponse != null && SearchResponse.Results != null && SearchResponse.Results.Count() > 0 && SearchResponse.Results[0].Count > 0 && SearchResponse.Results.LastOrDefault().Count > 0)
                        {
                            vfd.SecondSearchDate = SearchResponse.searchDate;
                            bookingLog(ref sbLogger, "Result not empty", "1");
                            List<FlightResult> resultNew = new List<FlightResult>();
                            for (int i = 0; i < airContext.flightBookingRequest.flightResult.Count; i++)
                            {
                                FlightResult fr = SearchResponse.Results[i].Where(k => k.ResultCombination == airContext.flightBookingRequest.flightResult[i].ResultCombination).FirstOrDefault();
                                if (fr != null)
                                {
                                    resultNew.Add(fr);
                                }
                            }
                            if (resultNew.Count == airContext.flightBookingRequest.flightResult.Count)
                            {
                                bookingLog(ref sbLogger, "New Fare ID", resultNew[0].Fare.tjID);
                                bookingLog(ref sbLogger, "all flight match", "1");
                                airContext.flightBookingRequest.flightResult = resultNew;
                                #region Set Sum fare
                                airContext.flightBookingRequest.sumFare = new Fare() { fareBreakdown = new List<FareBreakdown>() };
                                airContext.flightBookingRequest.PriceID = new List<string>();
                                foreach (var item in airContext.flightBookingRequest.flightResult)
                                {
                                    Fare fare = item.Fare;
                                    airContext.flightBookingRequest.PriceID.Add(fare.tjID);
                                    airContext.flightBookingRequest.sumFare.Currency = fare.Currency;
                                    airContext.flightBookingRequest.sumFare.NetFare += fare.NetFare;
                                    airContext.flightBookingRequest.sumFare.PublishedFare += fare.PublishedFare;
                                    airContext.flightBookingRequest.sumFare.grandTotal += fare.grandTotal;
                                    airContext.flightBookingRequest.sumFare.ffDiscount += fare.ffDiscount;
                                    airContext.flightBookingRequest.sumFare.BaseFare += fare.BaseFare;
                                    airContext.flightBookingRequest.sumFare.Tax += fare.Tax;
                                    airContext.flightBookingRequest.sumFare.AdditionalTxnFeeOfrd += fare.AdditionalTxnFeeOfrd;
                                    airContext.flightBookingRequest.sumFare.AdditionalTxnFeePub += fare.AdditionalTxnFeePub;
                                    airContext.flightBookingRequest.sumFare.Discount += fare.Discount;
                                    airContext.flightBookingRequest.sumFare.Markup += fare.Markup;
                                    airContext.flightBookingRequest.sumFare.OfferedFare += fare.OfferedFare;
                                    airContext.flightBookingRequest.sumFare.OtherCharges += fare.OtherCharges;
                                    airContext.flightBookingRequest.sumFare.ServiceFee += fare.ServiceFee;
                                    airContext.flightBookingRequest.sumFare.TdsOnCommission += fare.TdsOnCommission;
                                    airContext.flightBookingRequest.sumFare.TdsOnIncentive += fare.TdsOnIncentive;
                                    airContext.flightBookingRequest.sumFare.TdsOnPLB += fare.TdsOnPLB;
                                    airContext.flightBookingRequest.sumFare.YQTax += fare.YQTax;

                                    if (airContext.flightBookingRequest.sumFare.fareBreakdown.Count == 0)
                                    {
                                        foreach (FareBreakdown fb in fare.fareBreakdown)
                                        {
                                            airContext.flightBookingRequest.sumFare.fareBreakdown.Add(new FareBreakdown());
                                        }
                                    }
                                    for (int i = 0; i < fare.fareBreakdown.Count; i++)
                                    {
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].BaseFare += fare.fareBreakdown[i].BaseFare;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].Tax += fare.fareBreakdown[i].Tax;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].Markup += fare.fareBreakdown[i].Markup;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].PassengerType = item.Fare.fareBreakdown[i].PassengerType;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].YQTax += fare.fareBreakdown[i].YQTax;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeeOfrd += fare.fareBreakdown[i].AdditionalTxnFeeOfrd;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeePub += fare.fareBreakdown[i].AdditionalTxnFeePub;
                                        airContext.flightBookingRequest.sumFare.fareBreakdown[i].PGCharge += fare.fareBreakdown[i].PGCharge;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                bookingLog(ref sbLogger, "all not flight match", "1");
                                airContext.flightSearchResponse = SearchResponse;
                                objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                            }
                        }
                        else
                        {
                            if (ctr < 2)
                            {
                                bookingLog(ref sbLogger, "re search", ctr.ToString());
                                System.Threading.Thread.Sleep(1000);
                                ctr++;
                                goto ReSearch;
                            }
                        }
                    }

                    if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.TripJack)
                    {
                        #region Check Price Verification Tripjack
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };

                        new Bal.FlightDetails().FlightVerificationGF(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice - airContext.flightBookingRequest.sumFare.NetFare;

                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }

                        if (airContext.priceVerificationResponse.fareQuoteResponse.isRunFareQuoteFalse == true || airContext.priceVerificationResponse.fareQuoteResponse == null)
                        {
                            #region Research Flight
                            airContext.flightSearchRequest.isGetLiveFare = true;
                            int ctr = 0;
                            ReSearch:
                            FlightSearchResponse SearchResponse = new Bal.FlightDetails().ReSearchFlight(airContext.flightSearchRequest);

                            if (SearchResponse != null && SearchResponse.Results != null && SearchResponse.Results.Count() > 0 && SearchResponse.Results[0].Count > 0 && SearchResponse.Results.LastOrDefault().Count > 0)
                            {
                                vfd.SecondSearchDate = SearchResponse.searchDate;
                                List<FlightResult> resultNew = new List<FlightResult>();
                                for (int i = 0; i < airContext.flightBookingRequest.flightResult.Count; i++)
                                {
                                    FlightResult fr = SearchResponse.Results[i].Where(k => k.ResultCombination == airContext.flightBookingRequest.flightResult[i].ResultCombination).FirstOrDefault();
                                    if (fr != null)
                                    {
                                        resultNew.Add(fr);
                                    }
                                }
                                if (resultNew.Count == airContext.flightBookingRequest.flightResult.Count)
                                {
                                    airContext.flightBookingRequest.flightResult = resultNew;
                                    #region Set Sum fare
                                    airContext.flightBookingRequest.sumFare = new Fare() { fareBreakdown = new List<FareBreakdown>() };
                                    airContext.flightBookingRequest.PriceID = new List<string>();
                                    foreach (var item in airContext.flightBookingRequest.flightResult)
                                    {
                                        Fare fare = item.Fare;
                                        airContext.flightBookingRequest.PriceID.Add(fare.tjID);
                                        airContext.flightBookingRequest.sumFare.Currency = fare.Currency;
                                        airContext.flightBookingRequest.sumFare.NetFare += fare.NetFare;
                                        airContext.flightBookingRequest.sumFare.PublishedFare += fare.PublishedFare;
                                        airContext.flightBookingRequest.sumFare.grandTotal += fare.grandTotal;
                                        airContext.flightBookingRequest.sumFare.ffDiscount += fare.ffDiscount;
                                        airContext.flightBookingRequest.sumFare.BaseFare += fare.BaseFare;
                                        airContext.flightBookingRequest.sumFare.Tax += fare.Tax;
                                        airContext.flightBookingRequest.sumFare.AdditionalTxnFeeOfrd += fare.AdditionalTxnFeeOfrd;
                                        airContext.flightBookingRequest.sumFare.AdditionalTxnFeePub += fare.AdditionalTxnFeePub;
                                        airContext.flightBookingRequest.sumFare.Discount += fare.Discount;
                                        airContext.flightBookingRequest.sumFare.Markup += fare.Markup;
                                        airContext.flightBookingRequest.sumFare.OfferedFare += fare.OfferedFare;
                                        airContext.flightBookingRequest.sumFare.OtherCharges += fare.OtherCharges;
                                        airContext.flightBookingRequest.sumFare.ServiceFee += fare.ServiceFee;
                                        airContext.flightBookingRequest.sumFare.TdsOnCommission += fare.TdsOnCommission;
                                        airContext.flightBookingRequest.sumFare.TdsOnIncentive += fare.TdsOnIncentive;
                                        airContext.flightBookingRequest.sumFare.TdsOnPLB += fare.TdsOnPLB;
                                        airContext.flightBookingRequest.sumFare.YQTax += fare.YQTax;
                                        if (airContext.flightBookingRequest.sumFare.fareBreakdown.Count == 0)
                                        {
                                            foreach (FareBreakdown fb in fare.fareBreakdown)
                                            {
                                                airContext.flightBookingRequest.sumFare.fareBreakdown.Add(new FareBreakdown());
                                            }
                                        }
                                        for (int i = 0; i < fare.fareBreakdown.Count; i++)
                                        {
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].BaseFare += fare.fareBreakdown[i].BaseFare;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].Tax += fare.fareBreakdown[i].Tax;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].Markup += fare.fareBreakdown[i].Markup;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].PassengerType = item.Fare.fareBreakdown[i].PassengerType;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].YQTax += fare.fareBreakdown[i].YQTax;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeeOfrd += fare.fareBreakdown[i].AdditionalTxnFeeOfrd;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].AdditionalTxnFeePub += fare.fareBreakdown[i].AdditionalTxnFeePub;
                                            airContext.flightBookingRequest.sumFare.fareBreakdown[i].PGCharge += fare.fareBreakdown[i].PGCharge;
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    airContext.flightSearchResponse = SearchResponse;
                                    objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                                }
                            }
                            else
                            {
                                if (ctr < 2)
                                {
                                    System.Threading.Thread.Sleep(1000);
                                    ctr++;
                                    goto ReSearch;
                                }
                            }
                            #endregion

                            #region Re PriceVerification
                            airContext.priceVerificationRequest = new PriceVerificationRequest()
                            {
                                adults = airContext.flightSearchResponse.adults,
                                child = airContext.flightSearchResponse.child,
                                infants = airContext.flightSearchResponse.infants,
                                flightResult = airContext.flightBookingRequest.flightResult,
                                infantsWs = 0,
                                isFareQuote = true,
                                isFareRule = false,
                                isSSR = false,
                                PriceID = airContext.flightBookingRequest.PriceID,
                                siteID = airContext.flightSearchRequest.siteId,
                                sourceMedia = airContext.flightSearchRequest.sourceMedia,
                                TvoTraceId = "",
                                userIP = airContext.flightSearchRequest.userIP,
                                userSearchID = airContext.flightSearchRequest.userSearchID,
                                userLogID = airContext.flightSearchRequest.userLogID,
                                userSessionID = airContext.flightSearchRequest.userSessionID
                            };

                            new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                            if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                                airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                            {
                                airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice - airContext.flightBookingRequest.sumFare.NetFare;

                                if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                {
                                    if (airContext.flightBookingRequest.sourceMedia == "1015")
                                    {
                                        decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                        if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                        {
                                            airContext.flightBookingRequest.isFareChange = true;
                                            airContext.flightBookingRequest.isShowFareIncrease = true;
                                            airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                            bookingLog(ref sbLogger, "Re PriceVerification isFareChange", "true");
                                            bookingLog(ref sbLogger, "Re PriceVerification isShowFareIncrease", "true");
                                            bookingLog(ref sbLogger, "Re PriceVerification isMakeBookingInprogress", "false");
                                        }
                                        else
                                        {
                                            airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                            airContext.flightBookingRequest.isFareChange = true;
                                            airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                            bookingLog(ref sbLogger, "Re PriceVerification isFareChange else1", "0.00");
                                            bookingLog(ref sbLogger, "Re PriceVerification isShowFareIncrease else1", "true");
                                            bookingLog(ref sbLogger, "Re PriceVerification isMakeBookingInprogress else1", "true");
                                        }
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                        bookingLog(ref sbLogger, "Re PriceVerification isFareChange else2", "true");
                                        bookingLog(ref sbLogger, "Re PriceVerification isShowFareIncrease else2", "false");
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                    airContext.flightBookingRequest.isFareChange = false;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                                airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                                airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                                airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                            }
                            #endregion
                        }

                        if (airContext.priceVerificationResponse.fareQuoteResponse.isRunFareQuoteFalse == true || airContext.priceVerificationResponse.fareQuoteResponse == null || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            airContext.flightBookingRequest.isMakeBookingInprogress = true;
                        }

                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;

                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }

                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);

                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Tbo)
                    {
                        #region Check Price Verification TBO
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = true,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = airContext.flightSearchResponse.TraceId,
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };
                        new Bal.FlightDetails().FlightVerificationGF(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                          airContext.priceVerificationResponse.fareQuoteResponse != null
                          && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }




                        if (airContext.priceVerificationResponse.fareQuoteResponse.isRunFareQuoteFalse == true || airContext.priceVerificationResponse.fareQuoteResponse == null)
                        {
                            airContext.flightBookingRequest.isMakeBookingInprogress = true;
                        }


                        bookingLog(ref sbLogger, "TBOBookingID", airContext.flightBookingRequest.TjBookingID);
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;
                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }

                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount + airContext.flightBookingRequest.sumFare.OtherCharges);

                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.OneDFare)
                    {
                        #region Check Price Verification OneDFare
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;

                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }

                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);

                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelogy)
                    {
                        #region Check Price Verification Travelogy
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID,
                            tgy_Search_Key = airContext.flightSearchResponse.tgy_Search_Key,
                            PhoneNo = "9876543210",
                            tgy_Request_id = airContext.flightSearchRequest.tgy_Request_id
                        };


                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount += airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice - airContext.flightBookingRequest.sumFare.NetFare;

                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount < 1000M)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }


                            airContext.flightBookingRequest.tgy_Flight_Key = airContext.priceVerificationResponse.fareQuoteResponse.tgy_Flight_Key;
                            airContext.flightBookingRequest.tgy_Block_Ticket_Allowed = airContext.priceVerificationResponse.fareQuoteResponse.tgy_Block_Ticket_Allowed;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }

                        else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelogy && airContext.priceVerificationResponse.fareQuoteResponse == null)
                        {
                            bookingLog(ref sbLogger, "TGY", JsonConvert.SerializeObject(airContext.priceVerificationResponse.fareQuoteResponse));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelogy && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "0028")
                        {
                            bookingLog(ref sbLogger, "TGY", JsonConvert.SerializeObject(airContext.priceVerificationResponse.fareQuoteResponse));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        if (airContext.flightBookingRequest.tgy_Flight_Key == null && airContext.flightBookingRequest.tgy_Block_Ticket_Allowed == null)
                        {
                            bookingLog(ref sbLogger, "TGY", JsonConvert.SerializeObject(airContext.flightBookingRequest.tgy_Block_Ticket_Allowed));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;

                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }

                        objVps.TaxWithMakrup = (((airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.sumFare.ServiceFee + airContext.flightBookingRequest.fareIncreaseAmount)));

                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.FareBoutique)
                    {
                        #region Check Price Verification FareBoutique
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;

                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }

                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.SatkarTravel)
                    {
                        #region Check Price Verification SatkarTravel
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                            airContext.flightBookingRequest.STSessionID = airContext.priceVerificationResponse.fareQuoteResponse.STSessionID;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }
                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;
                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }
                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.AirIQ ||
                        airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Ease2Fly)
                    {
                        #region Check Price Verification AirIQ And Ease2Fly
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                            airContext.flightBookingRequest.STSessionID = airContext.priceVerificationResponse.fareQuoteResponse.STSessionID;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }
                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;
                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare) * (airContext.flightBookingRequest.adults);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare) * (airContext.flightBookingRequest.child);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare) * (airContext.flightBookingRequest.infants);
                        }
                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }

                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.GFS)
                    {
                        #region Check Price Verification GFS
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID,
                            origin = airContext.flightSearchRequest.segment[0].originAirport.ToLower(),
                            destination = airContext.flightSearchRequest.segment[0].destinationAirport.ToLower(),
                            depDate = airContext.flightSearchRequest.segment[0].travelDate.ToString("dd/MM/yyyy")
                        };

                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                            airContext.flightBookingRequest.STSessionID = airContext.priceVerificationResponse.fareQuoteResponse.STSessionID;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }
                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;
                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare) * (airContext.flightBookingRequest.adults);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare) * (airContext.flightBookingRequest.child);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare) * (airContext.flightBookingRequest.infants);
                        }
                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }

                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelopedia)
                    {
                        #region Check Price Verification Travelopedia
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID,
                            tgy_Search_Key = airContext.flightSearchResponse.tgy_Search_Key,
                            PhoneNo = "9876543210",
                            tgy_Request_id = airContext.flightSearchRequest.tgy_Request_id
                        };


                        new Bal.FlightDetails().FlightVerification(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount += airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice - airContext.flightBookingRequest.sumFare.NetFare;

                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount < 1000M)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }


                            airContext.flightBookingRequest.tgy_Flight_Key = airContext.priceVerificationResponse.fareQuoteResponse.tgy_Flight_Key;
                            airContext.flightBookingRequest.tgy_Block_Ticket_Allowed = airContext.priceVerificationResponse.fareQuoteResponse.tgy_Block_Ticket_Allowed;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                        }

                        else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelopedia && airContext.priceVerificationResponse.fareQuoteResponse == null)
                        {
                            bookingLog(ref sbLogger, "TRP", JsonConvert.SerializeObject(airContext.priceVerificationResponse.fareQuoteResponse));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Travelopedia && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "0028")
                        {
                            bookingLog(ref sbLogger, "TRP", JsonConvert.SerializeObject(airContext.priceVerificationResponse.fareQuoteResponse));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        if (airContext.flightBookingRequest.tgy_Flight_Key == null && airContext.flightBookingRequest.tgy_Block_Ticket_Allowed == null)
                        {
                            bookingLog(ref sbLogger, "TRP", JsonConvert.SerializeObject(airContext.flightBookingRequest.tgy_Block_Ticket_Allowed));
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }

                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;

                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare);
                        }
                        objVps.TaxWithMakrup = (((airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.sumFare.ServiceFee + airContext.flightBookingRequest.fareIncreaseAmount)));
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }

                    else if (airContext.flightBookingRequest.flightResult[0].Fare.gdsType == GdsType.Amadeus)
                    {
                        #region Check Price Verification Amadeus
                        airContext.priceVerificationRequest = new PriceVerificationRequest()
                        {
                            adults = airContext.flightSearchResponse.adults,
                            child = airContext.flightSearchResponse.child,
                            infants = airContext.flightSearchResponse.infants,
                            flightResult = airContext.flightBookingRequest.flightResult,
                            infantsWs = 0,
                            isFareQuote = true,
                            isFareRule = false,
                            isSSR = false,
                            PriceID = airContext.flightBookingRequest.PriceID,
                            siteID = airContext.flightSearchRequest.siteId,
                            sourceMedia = airContext.flightSearchRequest.sourceMedia,
                            TvoTraceId = "",
                            userIP = airContext.flightSearchRequest.userIP,
                            userSearchID = airContext.flightSearchRequest.userSearchID,
                            userLogID = airContext.flightSearchRequest.userLogID,
                            userSessionID = airContext.flightSearchRequest.userSessionID,
                            origin = airContext.flightSearchRequest.segment[0].originAirport.ToLower(),
                            destination = airContext.flightSearchRequest.segment[0].destinationAirport.ToLower(),
                            depDate = airContext.flightSearchRequest.segment[0].travelDate.ToString("dd/MM/yyyy")
                        };

                        new Bal.FlightDetails().FlightVerificationGF(ref airContext, ref sbLogger);

                        if (airContext.priceVerificationResponse != null && airContext.priceVerificationResponse.responseStatus.status == TransactionStatus.Success &&
                            airContext.priceVerificationResponse.fareQuoteResponse != null && airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.status == TransactionStatus.Success)
                        {
                            airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                            if (airContext.flightBookingRequest.fareIncreaseAmount > 0)
                            {
                                if (airContext.flightBookingRequest.sourceMedia == "1015")
                                {
                                    decimal FarePercentage = (airContext.flightBookingRequest.fareIncreaseAmount / airContext.flightBookingRequest.sumFare.NetFare) * 100;
                                    if (FarePercentage >= 10 || airContext.flightBookingRequest.fareIncreaseAmount > 0)
                                    {
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isShowFareIncrease = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                    }
                                    else
                                    {
                                        airContext.flightBookingRequest.fareIncreaseAmount = 0.00M;
                                        airContext.flightBookingRequest.isFareChange = true;
                                        airContext.flightBookingRequest.isMakeBookingInprogress = true;
                                    }
                                }
                                else
                                {
                                    airContext.flightBookingRequest.isFareChange = true;
                                    airContext.flightBookingRequest.isMakeBookingInprogress = false;
                                }
                            }
                            else
                            {
                                airContext.flightBookingRequest.isFareChange = false;
                                airContext.flightBookingRequest.isMakeBookingInprogress = false;
                            }
                            airContext.flightBookingRequest.TjBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjBookingID;
                            airContext.flightBookingRequest.TjReturnBookingID = airContext.priceVerificationResponse.fareQuoteResponse.TjReturnBookingID;
                            airContext.flightBookingRequest.VerifiedTotalPrice = airContext.priceVerificationResponse.fareQuoteResponse.VerifiedTotalPrice;
                            airContext.flightBookingRequest.STSessionID = airContext.priceVerificationResponse.fareQuoteResponse.STSessionID;
                        }
                        else if (airContext.priceVerificationResponse.fareQuoteResponse.responseStatus.message == "Error" || airContext.flightBookingRequest.TjBookingID == "" || airContext.flightBookingRequest.TjBookingID == null)
                        {
                            objVps.RedirectUrl = "/Flight/Result/" + airContext.flightSearchRequest.userSearchID;
                        }
                        bookingLog(ref sbLogger, "TjBookingID", airContext.flightBookingRequest.TjBookingID);
                        airContext.flightBookingRequest.fareIncreaseAmount = airContext.priceVerificationResponse.fareQuoteResponse.fareIncreaseAmount;
                        objVps.isFareChange = airContext.flightBookingRequest.isFareChange;
                        objVps.fareIncreaseAmount = airContext.flightBookingRequest.fareIncreaseAmount;
                        vfd.newAmt = ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - airContext.flightBookingRequest.CouponAmount);
                        vfd.SeconSearchFareID = String.Join(",", airContext.flightBookingRequest.PriceID);
                        vfd.TripjackBookingID = airContext.flightBookingRequest.TjBookingID;
                        objVps.adultFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Adult).First().BaseFare) * (airContext.flightBookingRequest.adults);
                        if (airContext.flightSearchRequest.child > 0)
                        {
                            objVps.childFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Child).First().BaseFare) * (airContext.flightBookingRequest.child);
                        }
                        if (airContext.flightSearchRequest.infants > 0)
                        {
                            objVps.infantFare = (airContext.flightBookingRequest.sumFare.fareBreakdown.Where(k => k.PassengerType == PassengerType.Infant).First().BaseFare) * (airContext.flightBookingRequest.infants);
                        }
                        objVps.TaxWithMakrup = (airContext.flightBookingRequest.sumFare.Tax + airContext.flightBookingRequest.sumFare.Markup + airContext.flightBookingRequest.fareIncreaseAmount);
                        objVps.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal;
                        #endregion
                    }
                }
                saveVerificationDetails(vfd);
                bookingLog(ref sbLogger, "Verification Response", JsonConvert.SerializeObject(objVps));
                LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\VerifyPrice", "VerifyPrice" + DateTime.Today.ToString("ddMMMyy"), airContext.flightBookingRequest.userSearchID + ".txt");
            }
            else
            {
                objVps.RedirectUrl = "/";
            }
            return Json(objVps, JsonRequestBehavior.AllowGet);
        }

        #region Razorpay
        [HttpGet]
        public ActionResult Payment(string ID)
        {
            AirContext airContext = FlightOperation.GetAirContext(ID);
            if (airContext != null)
            {
                if (airContext.IsBookingCompleted)
                {
                    return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                }
                ViewBag.IsBookingCompleted = airContext.IsBookingCompleted;
                return View(airContext.flightBookingRequest);
            }
            else
            {
                return Redirect("/");
            }
        }
        string key = ConfigurationManager.AppSettings["key"];
        string secret = ConfigurationManager.AppSettings["secret"];
        public JsonResult details(string mode, string ID)
        {
            StringBuilder sbLogger = new StringBuilder();
            PaymentRP RP = new PaymentRP();
            AirContext airContext = FlightOperation.GetAirContext(ID);
            if (airContext.IsBookingCompleted == true)
            {
                RP.RedirectUrl = "/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID;
            }
            else
            {
                if (airContext != null)
                {
                    string orderid = string.Empty;
                    string amt = string.Empty;

                    if (airContext.flightBookingRequest.isBuyCancellaionPolicy == false)
                    {
                        airContext.flightBookingRequest.CancellaionPolicyAmt = 0;
                    }
                    if (airContext.flightBookingRequest.isBuyRefundPolicy == false)
                    {
                        airContext.flightBookingRequest.RefundPolicyAmt = 0;
                    }

                    decimal totAmt = (((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount + airContext.flightBookingRequest.RefundPolicyAmt + airContext.flightBookingRequest.CancellaionPolicyAmt) - airContext.flightBookingRequest.CouponAmount));
                    decimal convFee = 0;
                    airContext.flightBookingRequest.paymentMode = getPayementMode(mode, ref convFee, airContext.flightBookingRequest.affiliate, airContext.flightBookingRequest.flightResult.Count, totAmt, airContext.flightBookingRequest.passengerDetails.Count);
                    airContext.flightBookingRequest.convenienceFee = convFee;
                    amt = Math.Round((totAmt + convFee) * 100).ToString("g29");
                    string Currency = airContext.flightBookingRequest.currencyCode;
                    airContext.flightBookingResponse.gatewayType = GetWayType.Razorpay;
                    try
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                        String orderId = airContext.flightBookingRequest.bookingID.ToString();
                        RazorpayClient client = new RazorpayClient(key, secret);
                        Dictionary<string, object> options = new Dictionary<string, object>();
                        options.Add("amount", amt);
                        options.Add("receipt", orderId);
                        options.Add("currency", Currency);
                        options.Add("payment_capture", "1");
                        Order order = client.Order.Create(options);
                        RP.orderid = order.Attributes["id"];
                        RP.name = " ";
                        RP.email = airContext.flightBookingRequest.emailID;
                        RP.phone = airContext.flightBookingRequest.phoneNo;
                        RP.amount = amt;
                        RP.status = "1";
                    }
                    catch (Exception ex)
                    {
                        RP.status = "2";
                        RP.name = ex.Message;
                        new LogWriter(ex.ToString(), "PaymentRequetException" + DateTime.Today.ToString("ddMMyy"), "Error");
                    }
                    finally
                    {

                    }
                    //   new DAL.TransactionDetailsCallBackData().SaveTransactonData(airContext.flightBookingResponse.bookingID, airContext.flightBookingRequest.transactionID, JsonConvert.SerializeObject(airContext));
                    //new DAL.LogWriter_New(sbLogger.ToString(), airContext.flightBookingResponse.bookingID.ToString(), "PaymentRequest");
                }
                else
                {
                    new DAL.LogWriter_New("Null", airContext.flightBookingRequest.bookingID.ToString(), "PaymentRequest");
                }
            }
            bookingLog(ref sbLogger, "Original Payment Request", JsonConvert.SerializeObject(RP));
            CreateLogFile(sbLogger.ToString(), "NewLog\\Booking", airContext.flightBookingRequest.bookingID.ToString() + ".txt");
            //  new DAL.LogWriter_New(JsonConvert.SerializeObject(airContext.flightBookingRequest), airContext.flightBookingRequest.bookingID.ToString() + "Request", "PaymentRequest");
            return Json(RP, JsonRequestBehavior.AllowGet);
        }
        public Core.PaymentMode getPayementMode(string PMode, ref decimal ConvenceFee, Affiliate aff, int tripCount, decimal totalAmt, int totPax)
        {
            if (PMode.Equals("upi", StringComparison.OrdinalIgnoreCase))
            {
                ConvenceFee = ((aff.UPIConFee.IndexOf("%") != -1 ? ((Convert.ToDecimal(aff.UPIConFee.Replace("%", "")) * totalAmt) / 100) : (Convert.ToDecimal(aff.UPIConFee) * tripCount * totPax)));
                return PaymentMode.upi;
            }
            else if (PMode.Equals("card", StringComparison.OrdinalIgnoreCase))
            {
                ConvenceFee = ((aff.CardConFee.IndexOf("%") != -1 ? ((Convert.ToDecimal(aff.CardConFee.Replace("%", "")) * totalAmt) / 100) : (Convert.ToDecimal(aff.CardConFee) * tripCount * totPax)));
                return PaymentMode.card;
            }
            else if (PMode.Equals("netbanking", StringComparison.OrdinalIgnoreCase))
            {
                ConvenceFee = ((aff.NetBankingConFee.IndexOf("%") != -1 ? ((Convert.ToDecimal(aff.NetBankingConFee.Replace("%", "")) * totalAmt) / 100) : (Convert.ToDecimal(aff.NetBankingConFee) * tripCount * totPax)));
                return PaymentMode.netbanking;
            }
            else if (PMode.Equals("wallet", StringComparison.OrdinalIgnoreCase))
            {
                ConvenceFee = ((aff.WalletConFee.IndexOf("%") != -1 ? ((Convert.ToDecimal(aff.WalletConFee.Replace("%", "")) * totalAmt) / 100) : (Convert.ToDecimal(aff.WalletConFee) * tripCount * totPax)));
                return PaymentMode.wallet;
            }
            else if (PMode.Equals("emi", StringComparison.OrdinalIgnoreCase))
            {
                ConvenceFee = ((aff.EmiConFee.IndexOf("%") != -1 ? ((Convert.ToDecimal(aff.EmiConFee.Replace("%", "")) * totalAmt) / 100) : (Convert.ToDecimal(aff.EmiConFee) * tripCount * totPax)));
                return PaymentMode.emi;
            }
            else if (PMode.Equals("paylater", StringComparison.OrdinalIgnoreCase))
            {
                ConvenceFee = ((aff.PayLaterConFee.IndexOf("%") != -1 ? ((Convert.ToDecimal(aff.PayLaterConFee.Replace("%", "")) * totalAmt) / 100) : (Convert.ToDecimal(aff.PayLaterConFee) * tripCount * totPax)));
                return PaymentMode.paylater;
            }
            else if (PMode.Equals("Paytm", StringComparison.OrdinalIgnoreCase))
            {
                ConvenceFee = ((aff.UPIConFee.IndexOf("%") != -1 ? ((Convert.ToDecimal(aff.UPIConFee.Replace("%", "")) * totalAmt) / 100) : (Convert.ToDecimal(aff.UPIConFee) * tripCount * totPax)));
                return PaymentMode.Paytm;
            }
            else if (PMode.Equals("GooglePay", StringComparison.OrdinalIgnoreCase))
            {
                ConvenceFee = ((aff.UPIConFee.IndexOf("%") != -1 ? ((Convert.ToDecimal(aff.UPIConFee.Replace("%", "")) * totalAmt) / 100) : (Convert.ToDecimal(aff.UPIConFee) * tripCount * totPax)));
                return PaymentMode.GooglePay;
            }
            else
            {
                ConvenceFee = ((aff.UPIConFee.IndexOf("%") != -1 ? ((Convert.ToDecimal(aff.UPIConFee.Replace("%", "")) * totalAmt) / 100) : (Convert.ToDecimal(aff.UPIConFee) * tripCount * totPax)));
                return PaymentMode.upi;
            }

        }
        public static string HmacSha256Digest(string message, string secret)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            System.Security.Cryptography.HMACSHA256 cryptographer = new System.Security.Cryptography.HMACSHA256(keyBytes);
            byte[] bytes = cryptographer.ComputeHash(messageBytes);
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        [HttpPost]
        public ActionResult Payment(FormCollection FC)
        {
            StringBuilder sbLogger = new StringBuilder();
            string paymentid = FC["paymentid"], signature = FC["signature"], orderID = FC["orderID"], searchID = FC["searchID"], BookingID = FC["BookingID"];

            bookingLog(ref sbLogger, "Original Final Payment Request BookingID", BookingID);

            PaymentCaptured pc = new PaymentCaptured();
            Core.RefineResult.ResultResponse obj = new Core.RefineResult.ResultResponse();
            AirContext airContext = FlightOperation.GetAirContext(searchID);

            if (airContext.IsBookingRequestSend == false)
            {
                airContext.IsBookingRequestSend = true;
                string status = "", capture = "", createdat = "", contact = "", email = "", amount = "", amtount = "";
                ViewBag.isShowResult = true;
                try
                {
                    airContext.IsBookingCompleted = true;
                    decimal CouponAmount = 0;
                    amtount = Math.Round(((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount + airContext.flightBookingRequest.convenienceFee + airContext.flightBookingRequest.RefundPolicyAmt + airContext.flightBookingRequest.CancellaionPolicyAmt) - airContext.flightBookingRequest.CouponAmount) * 100).ToString("g29");
                    bookingLog(ref sbLogger, "Original Final Payment Amount", amtount);
                    string newsignatue = HmacSha256Digest(orderID + "|" + paymentid, secret);
                    if (signature == newsignatue)
                    {
                        RazorpayClient client = new RazorpayClient(key, secret);
                        Payment payment = client.Payment.Fetch(paymentid);
                        Dictionary<string, object> options = new Dictionary<string, object>();
                        options.Add("amount", amtount);
                        Payment payment1 = client.Payment.Fetch(paymentid);
                        status = payment1.Attributes["status"];
                        capture = payment1.Attributes["captured"];
                        createdat = payment1.Attributes["created_at"];
                        contact = payment1.Attributes["contact"];
                        email = payment1.Attributes["email"];
                        amount = payment1.Attributes["amount"];
                        bookingLog(ref sbLogger, "Original Final Payment amount1", amount);
                        airContext.flightBookingRequest.paymentDetails.IsReturnHashMatched = true;
                        airContext.flightBookingRequest.paymentDetails.IsAmountMatch = (amount == amtount);
                    }
                    else
                    {
                        airContext.flightBookingRequest.paymentDetails.IsReturnHashMatched = false;
                        return Redirect("/flight/PaymentFail");
                    }
                    if (status.Equals("captured", StringComparison.OrdinalIgnoreCase))
                    {
                        pc.status = "1";
                        pc.orderid = orderID;

                    }
                    else if (status.Equals("Authorized", StringComparison.OrdinalIgnoreCase))
                    {
                        RazorpayClient client = new RazorpayClient(key, secret);
                        Payment payment = client.Payment.Fetch(paymentid);
                        Dictionary<string, object> options = new Dictionary<string, object>();
                        options.Add("amount", amtount);
                        Payment paymentCaptured = payment.Capture(options);
                        Payment payment1 = client.Payment.Fetch(paymentid);
                        status = payment1.Attributes["status"];
                        capture = payment1.Attributes["captured"];
                        createdat = payment1.Attributes["created_at"];
                        contact = payment1.Attributes["contact"];
                        email = payment1.Attributes["email"];
                        amount = payment1.Attributes["amount"];
                        bookingLog(ref sbLogger, "Original Final Payment amount2", amount);
                        if (status.Equals("captured", StringComparison.OrdinalIgnoreCase))
                        {
                            pc.status = "1";
                            pc.orderid = orderID;
                        }
                        else
                        {
                            pc.status = "0";
                            pc.orderid = orderID;

                        }
                    }
                    else
                    {
                        pc.status = "0";
                    }
                    airContext.flightBookingRequest.razorpayOrderID = pc.orderid;
                    bookingLog(ref sbLogger, "Original Final Payment razorpayOrderID", airContext.flightBookingRequest.razorpayOrderID);
                    airContext.flightBookingRequest.razorpayTransectionID = paymentid;
                    bookingLog(ref sbLogger, "Original Final Payment paymentid", airContext.flightBookingRequest.razorpayTransectionID);
                    airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts = status;
                    bookingLog(ref sbLogger, "Original Final Payment OnlinePaymentStauts", airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts);
                    bookingLog(ref sbLogger, "Original Final Payment IsAmountMatch", airContext.flightBookingRequest.paymentDetails.IsAmountMatch.ToString());

                    if ((status.Equals("captured", StringComparison.OrdinalIgnoreCase) || status.Equals("Authorized", StringComparison.OrdinalIgnoreCase))
                        && airContext.flightBookingRequest.paymentDetails.IsAmountMatch)
                    {
                        #region GetWebHookDetails
                        int wCtr = 0;
                        StartAgainWebHook:
                        Core.RP.Webhook.RazorPay_WebhooksDetails wsd = new DAL.PayU.RazorpayWebhook().GetRazorPay_WebhooksDetails(paymentid);
                        bookingLog(ref sbLogger, "Original Final Payment Webhook Details", JsonConvert.SerializeObject(wsd));
                        if (wsd == null && wCtr < 3)
                        {
                            System.Threading.Thread.Sleep(1000);
                            wCtr++;
                            goto StartAgainWebHook;
                        }
                        if (wsd != null)
                        {
                            airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts = wsd.status;
                            bookingLog(ref sbLogger, "Original Final Payment OnlinePaymentStauts", airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts);
                        }
                        else
                        {
                            wsd = new Core.RP.Webhook.RazorPay_WebhooksDetails() { amount = Convert.ToDecimal(amtount), status = airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts, };
                            bookingLog(ref sbLogger, "Original Final Payment Webhook wsd", JsonConvert.SerializeObject(wsd));
                        }

                        airContext.flightBookingRequest.paymentDetails.Rz_Amt = Math.Round(((wsd.amount) / 100)).ToString("f0");
                        bookingLog(ref sbLogger, "Original Final Payment Rz_Amt", airContext.flightBookingRequest.paymentDetails.Rz_Amt);
                        #endregion
                        if ((airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts.Equals("captured", StringComparison.OrdinalIgnoreCase) ||
                            airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts.Equals("Authorized", StringComparison.OrdinalIgnoreCase)) &&
                            (wsd.amount).ToString("f0") == amtount)
                        {
                            #region MakeBooking


                            Bal.FlightDetails objFlightDetails = new Bal.FlightDetails();
                            objFlightDetails.bookFlight(ref airContext, ref sbLogger);

                            airContext.flightBookingRequest.PNR = airContext.flightBookingResponse.PNR;
                            airContext.flightBookingRequest.ReturnPNR = airContext.flightBookingResponse.ReturnPNR;
                            airContext.flightBookingRequest.paymentStatus = airContext.flightBookingResponse.paymentStatus;
                            airContext.flightBookingRequest.bookingStatus = airContext.flightBookingResponse.bookingStatus;
                            airContext.flightBookingResponse.airline = airContext.flightBookingRequest.airline;
                            airContext.flightBookingResponse.airport = airContext.flightBookingRequest.airport;
                            airContext.flightBookingResponse.aircraftDetail = airContext.flightBookingRequest.aircraftDetail;
                            try
                            {
                                sendAttachment(airContext.flightBookingResponse);
                                //sendsms(airContext.flightBookingResponse, airContext.flightBookingResponse.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination,
                                //    airContext.flightBookingResponse.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime,
                                //    airContext.flightBookingResponse.PNR, airContext.flightBookingResponse.phoneNo);
                                //sendsms(airContext.flightBookingResponse, airContext.flightBookingResponse.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination,
                                //    airContext.flightBookingResponse.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime,
                                //    airContext.flightBookingResponse.PNR, "9310313497");
                                SendElectronicMail(airContext.flightBookingResponse);
                                if (airContext.flightBookingResponse.isWhatsapp == true)
                                {
                                    sendwhatsapp(airContext.flightBookingResponse);
                                    if (airContext.flightBookingResponse.flightResult.FirstOrDefault().Fare.mojoFareType == Core.MojoFareType.SeriesFareWithPNR || airContext.flightBookingResponse.flightResult.FirstOrDefault().Fare.mojoFareType == Core.MojoFareType.SeriesFareWithoutPNR)
                                    {
                                        if (airContext.flightBookingResponse.bookingStatus == Core.BookingStatus.Confirmed || airContext.flightBookingResponse.bookingStatus == Core.BookingStatus.InProgress
                                            || airContext.flightBookingResponse.bookingStatus == Core.BookingStatus.Ticketed || airContext.flightBookingResponse.bookingStatus == Core.BookingStatus.Pending
                                            || airContext.flightBookingResponse.bookingStatus == Core.BookingStatus.Incomplete)
                                        {
                                            groupbookingmsg(airContext.flightBookingResponse);
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(airContext.flightBookingResponse.ReturnPNR))
                                {
                                    // sendsms(airContext.flightBookingResponse, airContext.flightBookingResponse.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination,
                                    //airContext.flightBookingResponse.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime,
                                    //airContext.flightBookingResponse.ReturnPNR, airContext.flightBookingResponse.phoneNo);

                                    // sendsms(airContext.flightBookingResponse, airContext.flightBookingResponse.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination,
                                    //     airContext.flightBookingResponse.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime,
                                    //     airContext.flightBookingResponse.ReturnPNR, "9310313497");
                                }
                                #region S2S pixel for Kayak
                                if (airContext.flightSearchRequest.sourceMedia == "1013")
                                {
                                    if (airContext.flightBookingResponse.CouponAmount > 0 && !string.IsNullOrEmpty(airContext.flightBookingResponse.CouponCode))
                                    {
                                        CouponAmount = airContext.flightBookingResponse.CouponAmount;
                                    }
                                    string LogString = "";
                                    try
                                    {
                                        System.Net.ServicePointManager.Expect100Continue = true;
                                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls
                                               | System.Net.SecurityProtocolType.Tls11
                                               | System.Net.SecurityProtocolType.Tls12
                                               | System.Net.SecurityProtocolType.Ssl3;

                                        string s2sURL = "https://www.kayak.com/s/s2s/confirm?" +
                                        "partnercode=FLIGHTSMOJO&" +
                                        "bookingid=" + airContext.flightBookingResponse.bookingID.ToString() +
                                        "&bookedon=" + DateTime.Now.ToString("yyyy-MM-ddThh:mm:sszzz") +
                                         "&price=" + ((airContext.flightBookingResponse.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount + airContext.flightBookingRequest.convenienceFee) - CouponAmount) +
                                         "&currency=" + airContext.flightBookingResponse.sumFare.Currency +
                                         "&kayakcommission=0.22&commissioncurrency=" + airContext.flightBookingResponse.sumFare.Currency +
                                         "&kayakclickid=" + airContext.flightSearchRequest.redirectID + "&bookingtype=flight";
                                        LogString = s2sURL;
                                        var kk = new System.Net.WebClient().DownloadString(new Uri(s2sURL));

                                    }
                                    catch (Exception ex)
                                    {
                                        LogString += (Environment.NewLine + ex.ToString());
                                    }
                                    try
                                    {
                                        new Bal.LogWriter(LogString, ("S2S_" + DateTime.Today.ToString("ddMMMyy")));
                                    }
                                    catch { }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                bookingLog(ref sbLogger, "Original Final Payment Exception", ex.ToString());
                                CreateLogFile(sbLogger.ToString(), "NewLog\\Booking", airContext.flightBookingRequest.bookingID.ToString() + ".txt");
                                //  LogCreater.CreateLogFile(ex.ToString(), "Log\\PaymentException\\", airContext.flightBookingRequest.bookingID.ToString(), "_Payment3.txt");
                            }
                            CreateLogFile(sbLogger.ToString(), "NewLog\\Booking", airContext.flightBookingRequest.bookingID.ToString() + ".txt");
                            //    LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\Payment\\", airContext.flightBookingRequest.bookingID.ToString(), "Payment.txt");
                            #endregion

                            airContext.IsBookingCompleted = true;
                        }
                        else
                        {
                            bookingLog(ref sbLogger, "Original Final Payment Exception Else", JsonConvert.SerializeObject(airContext.flightBookingRequest));
                            CreateLogFile(sbLogger.ToString(), "NewLog\\Booking", airContext.flightBookingRequest.bookingID.ToString() + ".txt");
                            // LogCreater.CreateLogFile(JsonConvert.SerializeObject(airContext.flightBookingRequest), "Log\\PaymentException\\", airContext.flightBookingRequest.bookingID.ToString(), "_Payment3.txt");
                            return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                        }
                    }
                    else
                    {
                        bookingLog(ref sbLogger, "Original Final Payment Exception Else 2", JsonConvert.SerializeObject(airContext.flightBookingRequest));
                        CreateLogFile(sbLogger.ToString(), "NewLog\\Booking", airContext.flightBookingRequest.bookingID.ToString() + ".txt");
                        //  LogCreater.CreateLogFile(JsonConvert.SerializeObject(airContext.flightBookingRequest), "Log\\PaymentException\\", airContext.flightBookingRequest.bookingID.ToString(), "_Payment4.txt");
                        return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                    }
                }
                catch (Exception ex)
                {
                    bookingLog(ref sbLogger, "Original Final Payment Exception Else 2", ex.ToString());
                    CreateLogFile(sbLogger.ToString(), "NewLog\\Booking", airContext.flightBookingRequest.bookingID.ToString() + ".txt");
                    //  new LogWriter(ex.ToString(), "CompletePaymentException" + airContext.flightBookingResponse.bookingID, "Error");
                    return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                }
                bookingLog(ref sbLogger, "Original Final Payment FlightBookingFinalResponse", JsonConvert.SerializeObject(airContext.flightBookingResponse));
                CreateLogFile(sbLogger.ToString(), "NewLog\\Booking", airContext.flightBookingRequest.bookingID.ToString() + ".txt");
                //bookingLog(ref sbLogger, "FlightBookingFinalResponse", JsonConvert.SerializeObject(airContext.flightBookingResponse));
                //CreateLogFile(JsonConvert.SerializeObject(airContext.flightBookingResponse), "Log\\PaymentException", airContext.flightBookingRequest.bookingID.ToString() + ".txt");
            }
            else
            {
                System.Threading.Thread.Sleep(10000);
            }

            return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);

        }

        public void groupbookingmsg(string fsr)
        {
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            //var url = "https://api.imiconnect.in/resources/v1/messaging";
            //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            //var result = "";
            //httpRequest.Method = "POST";
            //httpRequest.ContentType = "application/json";
            //httpRequest.Headers["key"] = "30127004-37da-11ed-baaa-02e28ff40276";
            //Core.Whatsapp.WA whatsappGB = new Core.Whatsapp.WA();
            //whatsappGB.appid = "a_167149593199252900";
            //whatsappGB.deliverychannel = "whatsapp";
            //whatsappGB.message = new Core.Whatsapp.Message();
            //whatsappGB.message.template = "241117392398076";
            //whatsappGB.message.parameters = new Core.Whatsapp.Parameters();
            //whatsappGB.destination = new List<Core.Whatsapp.Destination>();
            //Core.Whatsapp.Destination dsGB = new Core.Whatsapp.Destination();
            //whatsappGB.destination.Add(dsGB);
            //dsGB.waid = new List<string>();
            //dsGB.waid.Add(fsr);
            //string output = JsonConvert.SerializeObject(whatsappGB);
            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(output);
            //}
            //var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    result = streamReader.ReadToEnd();
            //}
            //var statuscode = httpResponse.StatusCode;
        }

        public void groupbookingmsgOLD(FlightBookingResponse fsr)
        {
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            //var url = "https://api.imiconnect.in/resources/v1/messaging";
            //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            //var result = "";
            //httpRequest.Method = "POST";
            //httpRequest.ContentType = "application/json";
            //httpRequest.Headers["key"] = "30127004-37da-11ed-baaa-02e28ff40276";
            //Core.Whatsapp.WA whatsappGB = new Core.Whatsapp.WA();
            //whatsappGB.appid = "a_167149593199252900";
            //whatsappGB.deliverychannel = "whatsapp";
            //whatsappGB.message = new Core.Whatsapp.Message();
            //whatsappGB.message.template = "241117392398076";
            //whatsappGB.message.parameters = new Core.Whatsapp.Parameters();
            //whatsappGB.destination = new List<Core.Whatsapp.Destination>();
            //Core.Whatsapp.Destination dsGB = new Core.Whatsapp.Destination();
            //whatsappGB.destination.Add(dsGB);
            //dsGB.waid = new List<string>();
            //dsGB.waid.Add(fsr.phoneNo);
            //string output = JsonConvert.SerializeObject(whatsappGB);
            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(output);
            //}
            //var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    result = streamReader.ReadToEnd();
            //}
            //var statuscode = httpResponse.StatusCode;
            //CreateLogFile(result.ToString(), "Log\\whatsapp", "GB" + fsr.bookingID.ToString() + ".txt");
        }

        public ActionResult sendAttachment(FlightBookingResponse fsr)
        {
            try
            {
                fsr.FareTypeList = new List<FareType>();
                for (int i = 0; i < fsr.PriceID.Count; i++)
                {
                    fsr.FareTypeList.Add(fsr.flightResult[i].Fare.FareType);
                }

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                var templatePath = MailbodyViaView(this.ControllerContext, "~/Views/Shared/_EticketPdf.cshtml", fsr);
                var output = new MemoryStream();
                var document = new Document(PageSize.A4);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();
                var htmlContext = new HtmlPipelineContext(null);
                htmlContext.SetTagFactory(iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory());
                ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                var pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(document, writer)));
                var worker = new XMLWorker(pipeline, true);
                var stringReader = new StringReader(templatePath);
                var xmlParser = new XMLParser(worker);
                xmlParser.Parse(stringReader);
                document.Close();
                writer.Close();
                stringReader.Close();
                worker.Close();
                output.Position = 0;
                byte[] content = output.ToArray();
                MailMessage mm = new MailMessage(GlobalData.SendEmail, fsr.emailID);
                mm.Bcc.Add("kundan@flightsmojo.com");
                mm.Subject = "Your Booking is " + (fsr.bookingStatus == Core.BookingStatus.NONE ? "InProgress" : fsr.bookingStatus.ToString()) + " with Flightsmojo.in :- " + fsr.bookingID.ToString();
                new LogWriter(mm.Subject.ToString(), "Mail" + fsr.bookingID.ToString(), "Mail");
                mm.Body = MailbodyViaView(this.ControllerContext, "~/Views/Shared/_Ticketed.cshtml", fsr);
                if (fsr.bookingStatus == Core.BookingStatus.Ticketed)
                {
                    mm.Attachments.Add(new Attachment(new MemoryStream(content), "E-Ticket.pdf"));
                }
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Host = "smtp.office365.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = "res@flightsmojo.in";
                NetworkCred.Password = "Gux@123477";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
                return Json(new { isSuccess = true, message = "Send to mail customer sucessfully." });
            }
            catch (Exception ex)
            {
                ex.ToString();
                return Json(new { isSuccess = false, message = "Authorization form failed. Something going wrong." });
                new LogWriter(ex.ToString(), "MailException" + DateTime.Today.ToString("ddMMyy"), "Error");
            }
        }


        public ActionResult SendInvoice(FlightBookingResponse fsr)
        {
            try
            {
                fsr.FareTypeList = new List<FareType>();
                for (int i = 0; i < fsr.PriceID.Count; i++)
                {
                    fsr.FareTypeList.Add(fsr.flightResult[i].Fare.FareType);
                }
                var templatePath = MailbodyViaView(this.ControllerContext, "~/Views/Shared/_Invoice.cshtml", fsr.bookingID);
                var output = new MemoryStream();
                var document = new Document(PageSize.A4);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();
                var htmlContext = new HtmlPipelineContext(null);
                htmlContext.SetTagFactory(iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory());
                ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                var pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new iTextSharp.tool.xml.pipeline.end.PdfWriterPipeline(document, writer)));
                var worker = new XMLWorker(pipeline, true);
                var stringReader = new StringReader(templatePath);
                var xmlParser = new XMLParser(worker);
                xmlParser.Parse(stringReader);
                document.Close();
                writer.Close();
                stringReader.Close();
                worker.Close();
                output.Position = 0;
                byte[] content = output.ToArray();
                MailMessage mm = new MailMessage(GlobalData.SendEmail, fsr.emailID);
                mm.Subject = "Tax Invoice for your Flight Booking id" + " : " + fsr.bookingStatus.ToString() + "";
                mm.Body = MailbodyViaView(this.ControllerContext, "~/Views/Shared/_InvoiceMailBody.cshtml", fsr);
                mm.Attachments.Add(new Attachment(new MemoryStream(content), "Invoice.pdf"));
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Host = "smtp.office365.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = "res@flightsmojo.in";
                NetworkCred.Password = "Gux@123477";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);


                return Json(new { isSuccess = true, message = "Send to mail customer sucessfully." });
            }
            catch (Exception ex)
            {
                ex.ToString();
                return Json(new { isSuccess = false, message = "Authorization form failed. Something going wrong." });
                new LogWriter(ex.ToString(), "MailException" + DateTime.Today.ToString("ddMMyy"), "Error");
            }
        }


        private void sendsms(FlightBookingResponse fsr, string dest, DateTime depDate, string PNR, string phoneNo)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            String result;
            string message = string.Empty;
            if (fsr.bookingStatus == BookingStatus.Ticketed || fsr.bookingStatus == BookingStatus.Confirmed)
            {
                message = "Dear " + fsr.passengerDetails.FirstOrDefault().firstName + ",\n\nThank you for choosing FlightsMojo.in. You flight to " + dest + " is confirmed and ticketed.\nPlease find the details below:\n\nFM Booking ID: " + fsr.bookingID + "\nAirline PNR: " + PNR + "\nTravel Date: " + depDate.ToString("dd-MMM") + "\n\nIn case of any queries, please reach out to us at 0124-445-2000 or email us at care@flightsmojo.in";
            }
            else if (fsr.bookingStatus == BookingStatus.Failed)
            {
                message = "Dear " + fsr.passengerDetails.FirstOrDefault().firstName + ",\nYou booking is Failed with booking id- " + fsr.bookingID + ". We are trying to confirm your booking and can take Upto 30 minutes.\nPlease do not create new booking. If unable to confirm, full refund will be processed.\nFor any queries, please call us at 0124-445-2000. \nFlights Mojo.in";
            }
            else if (fsr.bookingStatus == BookingStatus.NONE || fsr.bookingStatus == BookingStatus.InProgress)
            {
                message = "Dear " + fsr.passengerDetails.FirstOrDefault().firstName + ",\nYou booking is Processing in the airline's system with your booking id- " + fsr.bookingID + " .In some cases, it can take Upto 1 hour to confirm the final status. \nPlease do not create another booking. For any queries, please call us at 01244452000. \nFlights Mojo.in";
            }
            String url = "https://api.textlocal.in/send/?apikey=" + apiKey + "&numbers=" + phoneNo + "&message=" + message + "&sender=" + sender;
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(url);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }
            CreateLogFile(message.ToString() + " " + result.ToString(), "Log\\MSG", fsr.bookingID.ToString() + ".txt");
        }

        public void SendElectronicMail(FlightBookingResponse fsr)

        {
            try
            {
                AirContext airContext = FlightOperation.GetAirContext(fsr.userSearchID);
                if (airContext != null)
                {
                    airContext.flightBookingResponse.airline = airContext.flightBookingRequest.airline;
                    airContext.flightBookingResponse.airport = airContext.flightBookingRequest.airport;
                    airContext.flightBookingResponse.aircraftDetail = airContext.flightBookingRequest.aircraftDetail;
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    var templatePath = MailbodyViaView(this.ControllerContext, "~/Views/Shared/_EticketPdf.cshtml", fsr);
                    var output = new MemoryStream();
                    var document = new Document(PageSize.A4);
                    var writer = PdfWriter.GetInstance(document, output);
                    writer.CloseStream = false;
                    document.Open();
                    var htmlContext = new HtmlPipelineContext(null);
                    htmlContext.SetTagFactory(iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory());
                    ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                    var pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(document, writer)));
                    var worker = new XMLWorker(pipeline, true);
                    var stringReader = new StringReader(templatePath);
                    var xmlParser = new XMLParser(worker);
                    xmlParser.Parse(stringReader);
                    document.Close();
                    writer.Close();
                    stringReader.Close();
                    worker.Close();
                    output.Position = 0;
                    byte[] content = output.ToArray();
                    string fname = fsr.bookingID + ".pdf";
                    fname = Path.Combine(Server.MapPath("~/Uploadedpdf/"), fname);
                    System.IO.File.WriteAllBytes(fname, content);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        public string MailbodyViaView(ControllerContext context, string viewName, object bookingdetails)
        {
            try
            {
                if (string.IsNullOrEmpty(viewName))
                    viewName = context.RouteData.GetRequiredString("action");
                var viewData = new ViewDataDictionary(bookingdetails);
                using (var sw = new System.IO.StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                    var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                    viewResult.View.Render(viewContext, sw);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private void sendwhatsappOLD(FlightBookingResponse fsr)
        {
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            //var url = "https://api.imiconnect.in/resources/v1/messaging";
            //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            //httpRequest.Method = "POST";
            //httpRequest.ContentType = "application/json";
            //httpRequest.Headers["key"] = "30127004-37da-11ed-baaa-02e28ff40276";
            //string output = string.Empty;

            //if (!string.IsNullOrEmpty(fsr.ReturnPNR))
            //{
            //    Core.Whatsapp.WA whatsapp = new Core.Whatsapp.WA();
            //    whatsapp.appid = "a_167149593199252900";
            //    whatsapp.deliverychannel = "whatsapp";
            //    whatsapp.message = new Core.Whatsapp.Message();
            //    whatsapp.message.template = "1494430417783268";
            //    whatsapp.message.parameters = new Core.Whatsapp.Parameters();
            //    whatsapp.message.parameters.variable1 = fsr.passengerDetails.FirstOrDefault().firstName;
            //    whatsapp.message.parameters.variable2 = fsr.bookingID.ToString();
            //    whatsapp.message.parameters.variable3 = fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination;
            //    whatsapp.message.parameters.variable4 = fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination;
            //    whatsapp.message.parameters.variable5 = fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("dd MMM yy");
            //    whatsapp.message.parameters.variable6 = fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("hh:mm");
            //    whatsapp.message.parameters.variable7 = fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().Airline + "-" + fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().FlightNumber;
            //    whatsapp.message.parameters.variable8 = fsr.PNR;
            //    whatsapp.message.parameters.variable9 = fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination;
            //    whatsapp.message.parameters.variable10 = fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination;
            //    whatsapp.message.parameters.variable11 = fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("dd MMM yy");
            //    whatsapp.message.parameters.variable12 = fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("hh:mm");
            //    whatsapp.message.parameters.variable13 = fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().Airline + "-" + fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().FlightNumber;
            //    whatsapp.message.parameters.variable14 = fsr.ReturnPNR;
            //    whatsapp.message.parameters.document = new Core.Whatsapp.document();
            //    whatsapp.message.parameters.document.link = GlobalData.URL + "/Uploadedpdf/" + fsr.bookingID + ".pdf";
            //    whatsapp.message.parameters.document.filename = "e-Ticket";
            //    whatsapp.destination = new List<Core.Whatsapp.Destination>();
            //    Core.Whatsapp.Destination ds = new Core.Whatsapp.Destination();
            //    whatsapp.destination.Add(ds);
            //    ds.waid = new List<string>();
            //    ds.waid.Add(fsr.phoneNo);
            //    output = JsonConvert.SerializeObject(whatsapp);
            //}
            //else if (!string.IsNullOrEmpty(fsr.PNR))
            //{
            //    Core.Whatsapp.WA whatsapp = new Core.Whatsapp.WA();
            //    whatsapp.appid = "a_167149593199252900";
            //    whatsapp.deliverychannel = "whatsapp";
            //    whatsapp.message = new Core.Whatsapp.Message();
            //    whatsapp.message.template = "1075029163764892";
            //    whatsapp.message.parameters = new Core.Whatsapp.Parameters();
            //    whatsapp.message.parameters.variable1 = fsr.passengerDetails.FirstOrDefault().firstName;
            //    whatsapp.message.parameters.variable2 = fsr.bookingID.ToString();
            //    whatsapp.message.parameters.variable3 = fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().Origin;
            //    whatsapp.message.parameters.variable4 = fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination;
            //    whatsapp.message.parameters.variable5 = fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("dd MMM yy");
            //    whatsapp.message.parameters.variable6 = fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("hh:mm");
            //    whatsapp.message.parameters.variable7 = fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().Airline + "-" + fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().FlightNumber;
            //    whatsapp.message.parameters.variable8 = fsr.PNR;
            //    whatsapp.message.parameters.document = new Core.Whatsapp.document();
            //    whatsapp.message.parameters.document.link = GlobalData.URL + "/Uploadedpdf/" + fsr.bookingID + ".pdf";
            //    whatsapp.message.parameters.document.filename = "e-Ticket";
            //    whatsapp.destination = new List<Core.Whatsapp.Destination>();
            //    Core.Whatsapp.Destination ds = new Core.Whatsapp.Destination();
            //    whatsapp.destination.Add(ds);
            //    ds.waid = new List<string>();
            //    ds.waid.Add(fsr.phoneNo);
            //    output = JsonConvert.SerializeObject(whatsapp);
            //}
            //else if (fsr.bookingStatus == Core.BookingStatus.Failed)
            //{
            //    Core.Whatsapp.WA whatsapp = new Core.Whatsapp.WA();
            //    whatsapp.appid = "a_167149593199252900";
            //    whatsapp.deliverychannel = "whatsapp";
            //    whatsapp.message = new Core.Whatsapp.Message();
            //    whatsapp.message.template = "3381846288779813";
            //    whatsapp.message.parameters = new Core.Whatsapp.Parameters();
            //    whatsapp.message.parameters.variable1 = fsr.passengerDetails.FirstOrDefault().firstName;
            //    whatsapp.message.parameters.variable2 = fsr.bookingID.ToString();
            //    whatsapp.destination = new List<Core.Whatsapp.Destination>();
            //    Core.Whatsapp.Destination ds = new Core.Whatsapp.Destination();
            //    whatsapp.destination.Add(ds);
            //    ds.waid = new List<string>();
            //    ds.waid.Add(fsr.phoneNo);
            //    output = JsonConvert.SerializeObject(whatsapp);
            //}
            //else if (fsr.bookingStatus == Core.BookingStatus.InProgress)
            //{
            //    Core.Whatsapp.WA whatsapp = new Core.Whatsapp.WA();
            //    whatsapp.appid = "a_167149593199252900";
            //    whatsapp.deliverychannel = "whatsapp";
            //    whatsapp.message = new Core.Whatsapp.Message();
            //    whatsapp.message.template = "317661550864529";
            //    whatsapp.message.parameters = new Core.Whatsapp.Parameters();
            //    whatsapp.message.parameters.variable1 = fsr.passengerDetails.FirstOrDefault().firstName;
            //    whatsapp.message.parameters.variable2 = fsr.bookingID.ToString();
            //    whatsapp.destination = new List<Core.Whatsapp.Destination>();
            //    Core.Whatsapp.Destination ds = new Core.Whatsapp.Destination();
            //    whatsapp.destination.Add(ds);
            //    ds.waid = new List<string>();
            //    ds.waid.Add(fsr.phoneNo);
            //    output = JsonConvert.SerializeObject(whatsapp);
            //}
            //else if (fsr.bookingStatus == Core.BookingStatus.NONE)
            //{
            //    Core.Whatsapp.WA whatsapp = new Core.Whatsapp.WA();
            //    whatsapp.appid = "a_167149593199252900";
            //    whatsapp.deliverychannel = "whatsapp";
            //    whatsapp.message = new Core.Whatsapp.Message();
            //    whatsapp.message.template = "317661550864529";
            //    whatsapp.message.parameters = new Core.Whatsapp.Parameters();
            //    whatsapp.message.parameters.variable1 = fsr.passengerDetails.FirstOrDefault().firstName;
            //    whatsapp.message.parameters.variable2 = fsr.bookingID.ToString();
            //    whatsapp.destination = new List<Core.Whatsapp.Destination>();
            //    Core.Whatsapp.Destination ds = new Core.Whatsapp.Destination();
            //    whatsapp.destination.Add(ds);
            //    ds.waid = new List<string>();
            //    ds.waid.Add(fsr.phoneNo);
            //    output = JsonConvert.SerializeObject(whatsapp);
            //}
            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(output);
            //}
            //var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //}
            //var statuscode = httpResponse.StatusCode;
        }


        #endregion
        [HttpGet]
        public ActionResult PaymentOnline(string ID)
        {
            StringBuilder sbLogger = new StringBuilder();
            ViewBag.isShowResult = true;
            try
            {
                AirContext airContext = FlightOperation.GetAirContext(ID);
                if (airContext != null)
                {
                    airContext.flightSearchResponse = null;
                    airContext.flightBookingRequest.transactionID = DAL.IdGenrator.Get("TransactionID");

                    new DAL.TransactionDetailsCallBackData().SaveTransactonData(airContext.flightBookingResponse.bookingID, airContext.flightBookingRequest.transactionID, JsonConvert.SerializeObject(airContext));

                    airContext.IsGoToPaymentPage = true;
                    if (airContext.IsBookingCompleted)
                    {
                        return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                    }

                    #region PayU
                    try
                    {
                        MojoIndia.Models.PayUMoney objPayU = new MojoIndia.Models.PayUMoney();
                        string[] hashVarsSeq;
                        string hash_string = string.Empty;
                        string hash1 = string.Empty;

                        decimal CouponAmount = 0;
                        if (airContext.flightBookingRequest.CouponAmount > 0 && !string.IsNullOrEmpty(airContext.flightBookingRequest.CouponCode))
                        {
                            CouponAmount = airContext.flightBookingRequest.CouponAmount;
                        }
                        string pinfo = objPayU.GetJson(((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - CouponAmount),
                            ConfigurationManager.AppSettings["MERCHANT_ID"], "2");
                        //if (airContext.flightBookingRequest.emailID.ToLower() == "kundan@flightsmojo.com")
                        //{
                        //    pinfo = objPayU.GetJson(1, ConfigurationManager.AppSettings["MERCHANT_ID"], "2");
                        //}

                        hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|'); // spliting hash sequence from config
                        hash_string = "";
                        foreach (string hash_var in hashVarsSeq)
                        {
                            if (hash_var == "key")
                            {
                                hash_string += ConfigurationManager.AppSettings["MERCHANT_KEY"];
                                hash_string += '|';
                            }
                            else if (hash_var == "txnid")
                            {
                                hash_string += ("TRN" + airContext.flightBookingRequest.transactionID);//txnid1;
                                hash_string += '|';
                            }
                            else if (hash_var == "amount")
                            {
                                //if (airContext.flightBookingRequest.emailID.ToLower() == "kundan@flightsmojo.com")
                                //{
                                //    hash_string += Convert.ToDecimal("1").ToString("g29");
                                //}
                                //else
                                //{
                                hash_string += ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - CouponAmount).ToString("g29");
                                //}
                                hash_string += '|';
                            }
                            else if (hash_var == "productinfo")
                            {
                                hash_string += ("{\"paymentParts\":" + pinfo + "}");//pinfo;
                                hash_string += '|';
                            }
                            else if (hash_var == "firstname")
                            {
                                hash_string += airContext.flightBookingRequest.passengerDetails[0].firstName;
                                hash_string += '|';
                            }
                            else if (hash_var == "email")
                            {
                                hash_string += airContext.flightBookingRequest.emailID;
                                hash_string += '|';
                            }
                            else
                            {
                                hash_string += (Request.Form[hash_var] != null ? Request.Form[hash_var] : "");// isset if else
                                hash_string += '|';
                            }
                        }

                        hash_string += ConfigurationManager.AppSettings["SALT"];// appending SALT

                        hash1 = objPayU.Generatehash512(hash_string).ToLower();         //generating hash
                        string action1 = (ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment");



                        if (!string.IsNullOrEmpty(hash1))
                        {

                            System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                            data.Add("hash", hash1);
                            data.Add("txnid", ("TRN" + airContext.flightBookingRequest.transactionID));//txnid1);
                            data.Add("key", ConfigurationManager.AppSettings["MERCHANT_KEY"]);

                            //if (airContext.flightBookingRequest.emailID.ToLower() == "kundan@flightsmojo.com")
                            //{
                            //    data.Add("amount", Convert.ToDecimal("1").ToString("g29"));
                            //}
                            //else
                            //{
                            data.Add("amount", ((airContext.flightBookingRequest.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - CouponAmount).ToString("g29"));
                            //}

                            data.Add("firstname", airContext.flightBookingRequest.passengerDetails[0].firstName);
                            data.Add("email", airContext.flightBookingRequest.emailID.Trim());
                            data.Add("phone", airContext.flightBookingRequest.phoneNo.Trim());
                            data.Add("productinfo", HttpUtility.HtmlEncode("{\"paymentParts\":" + pinfo + "}"));
                            data.Add("surl", GlobalData.URL + "/Flight/PaymentSuccess");
                            data.Add("furl", GlobalData.URL + "/Flight/PaymentFail");
                            data.Add("lastname", airContext.flightBookingRequest.passengerDetails[0].lastName);
                            data.Add("curl", GlobalData.URL + "/Flight/PaymentFail");
                            //data.Add("address1", airContext.flightBookingRequest.paymentDetails.address1);
                            //data.Add("address2", airContext.flightBookingRequest.paymentDetails.address2);
                            //data.Add("city", airContext.flightBookingRequest.paymentDetails.city);
                            //data.Add("state", airContext.flightBookingRequest.paymentDetails.state);
                            //data.Add("country", airContext.flightBookingRequest.paymentDetails.country);
                            //data.Add("zipcode", airContext.flightBookingRequest.paymentDetails.postalCode);
                            data.Add("udf1", "");
                            data.Add("udf2", "");
                            data.Add("udf3", "");
                            data.Add("udf4", "");
                            data.Add("udf5", "");
                            data.Add("pg", "");
                            data.Add("service_provider", "payu_paisa");


                            string strForm = objPayU.PreparePOSTForm(action1, data);
                            ViewBag.Form = strForm;

                        }
                        else
                        {
                            //no hash
                        }
                    }
                    catch (Exception ex)
                    {
                        //Response.Write("<span style='color:red'>" + ex.Message + "</span>");
                    }
                    #endregion
                    return View();
                }
                else { return Redirect("/"); }
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error9_" + DateTime.Today.ToString("ddMMyy"), "Error");
                return Redirect("/");
            }
        }
        [HttpPost]
        public ActionResult PaymentSuccess(string ID)
        {
            AirContext airContext = FlightOperation.GetAirContext(ID);
            if (airContext != null)
            {
                airContext.IsGoToPaymentPage = true;
                if (airContext.IsBookingCompleted)
                {
                    return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                }
            }
            StringBuilder sbLogger = new StringBuilder();
            try
            {
                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                string order_id = string.Empty;
                string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
                MojoIndia.Models.PayUMoney objPayU = new MojoIndia.Models.PayUMoney();
                if (Request.Form["status"].Equals("success", StringComparison.OrdinalIgnoreCase))
                {
                    merc_hash_vars_seq = hash_seq.Split('|');
                    Array.Reverse(merc_hash_vars_seq);
                    merc_hash_string = ConfigurationManager.AppSettings["SALT"] + "|" + Request.Form["status"];
                    foreach (string merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (Request.Form[merc_hash_var] != null ? Request.Form[merc_hash_var] : "");
                    }
                    Response.Write(merc_hash_string); //return;
                    merc_hash = objPayU.Generatehash512(merc_hash_string).ToLower();
                    order_id = Request.Form["txnid"];
                    //Response.Write("value matched");
                    string ss = new DAL.TransactionDetailsCallBackData().getTransactonData(Convert.ToInt64(order_id.Replace("TRN", "")));
                    airContext = JsonConvert.DeserializeObject<AirContext>(ss.ToString());
                    FlightOperation.SetAirContext(airContext);
                    if (airContext.flightBookingRequest.paymentDetails == null)
                    {
                        airContext.flightBookingRequest.paymentDetails = new PaymentDetails();
                    }
                    airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts = Request.Form["status"];
                    airContext.flightBookingRequest.paymentDetails.Hash = Request.Form["hash"];
                    if (merc_hash != Request.Form["hash"])
                    {
                        //Response.Write("Hash value did not matched");
                        airContext.flightBookingRequest.paymentDetails.IsReturnHashMatched = false;
                    }
                    else
                    {
                        airContext.flightBookingRequest.paymentDetails.IsReturnHashMatched = true;
                    }
                }
                else
                {
                    //Response.Write("Hash value did not matched");
                    order_id = Request.Form["txnid"];
                    string ss = new DAL.TransactionDetailsCallBackData().getTransactonData(Convert.ToInt64(order_id.Replace("TRN", "")));
                    airContext = JsonConvert.DeserializeObject<AirContext>(ss.ToString());
                    FlightOperation.SetAirContext(airContext);
                    if (airContext.flightBookingRequest.paymentDetails == null)
                    {
                        airContext.flightBookingRequest.paymentDetails = new PaymentDetails();
                    }
                    airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts = Request.Form["status"];
                    airContext.flightBookingRequest.paymentDetails.Hash = Request.Form["hash"];
                }
                #region GetWebHookDetails
                if (!ConfigurationManager.AppSettings["PAYU_BASE_URL"].Equals("https://sandboxsecure.payu.in"))
                {
                    int wCtr = 0;
                    StartAgainWebHook:
                    Core.PayU.WebhookSuccessDetails wsd = new DAL.PayU.DalWebHook().GetPayU_WebhooksDetails("Success", order_id.Replace("TRN", ""));

                    if (wsd == null && wCtr < 3)
                    {
                        System.Threading.Thread.Sleep(1000);
                        wCtr++;
                        goto StartAgainWebHook;
                    }
                    if (wsd != null)
                    {
                        airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts = wsd.status;
                    }
                    else
                    {
                        airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts = "fail";
                    }
                }
                #endregion

                #region check booking is Process or not

                System.Data.DataSet ds = new DAL.TransactionDetailsCallBackData().getTransactionDetails(Convert.ToInt64(order_id.Replace("TRN", "")));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Redirect("/");
                }

                #endregion

                #region MakeBooking
                Bal.FlightDetails objFlightDetails = new Bal.FlightDetails();
                objFlightDetails.bookFlight(ref airContext, ref sbLogger);

                airContext.flightBookingRequest.PNR = airContext.flightBookingResponse.PNR;
                airContext.flightBookingRequest.ReturnPNR = airContext.flightBookingResponse.ReturnPNR;
                airContext.flightBookingRequest.TvoBookingID = airContext.flightBookingResponse.TvoBookingID;
                airContext.flightBookingRequest.TvoReturnBookingID = airContext.flightBookingResponse.TvoReturnBookingID;
                airContext.flightBookingRequest.isTickted = airContext.flightBookingResponse.isTickted;
                airContext.flightBookingRequest.paymentStatus = airContext.flightBookingResponse.paymentStatus;
                airContext.flightBookingRequest.bookingStatus = airContext.flightBookingResponse.bookingStatus;
                if (airContext.flightBookingResponse.isTickted.Where(k => k == false).ToList().Count > 0)
                {
                    objFlightDetails.TicketFlight(ref airContext, ref sbLogger);
                }
                airContext.flightBookingResponse.airline = airContext.flightBookingRequest.airline;
                airContext.flightBookingResponse.airport = airContext.flightBookingRequest.airport;
                airContext.flightBookingResponse.aircraftDetail = airContext.flightBookingRequest.aircraftDetail;
                try
                {
                    SendEmailRequest objSendEmailRequest = new SendEmailRequest();
                    //string _BookingStaus = string.Empty;
                    objSendEmailRequest.FromEmail = GlobalData.SendEmail;
                    objSendEmailRequest.ToEmail = airContext.flightBookingRequest.emailID;
                    objSendEmailRequest.CcEmail = "";
                    objSendEmailRequest.BccEmail = "kundan@flightsmojo.com";
                    objSendEmailRequest.MailBody = GetMailBody(airContext.flightBookingResponse);
                    objSendEmailRequest.MailSubject = "Your Booking is " + airContext.flightBookingResponse.bookingStatus.ToString() + " with Flightsmojo.in :- " + airContext.flightBookingResponse.bookingID.ToString();
                    objSendEmailRequest.BookingID = airContext.flightBookingResponse.bookingID;
                    objSendEmailRequest.prodID = airContext.flightBookingResponse.prodID;
                    objSendEmailRequest.MailType = "confirmation";


                    new Bal.SMTP().SendEMail(objSendEmailRequest);

                }
                catch (Exception ex)
                {

                }
                #region S2S pixel for Kayak
                if (airContext.flightSearchRequest.sourceMedia == "1013")
                {
                    decimal CouponAmount = 0;
                    if (airContext.flightBookingResponse.CouponAmount > 0 && !string.IsNullOrEmpty(airContext.flightBookingResponse.CouponCode))
                    {
                        CouponAmount = airContext.flightBookingResponse.CouponAmount;
                    }
                    string LogString = "";
                    try
                    {
                        System.Net.ServicePointManager.Expect100Continue = true;
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls
                               | System.Net.SecurityProtocolType.Tls11
                               | System.Net.SecurityProtocolType.Tls12
                               | System.Net.SecurityProtocolType.Ssl3;

                        string s2sURL = "https://www.kayak.com/s/s2s/confirm?" +
                        "partnercode=FLIGHTSMOJO&" +
                        "bookingid=" + airContext.flightBookingResponse.bookingID.ToString() +
                        "&bookedon=" + DateTime.Now.ToString("yyyy-MM-ddThh:mm:sszzz") +
                         "&price=" + ((airContext.flightBookingResponse.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - CouponAmount) +
                         "&currency=" + airContext.flightBookingResponse.sumFare.Currency +
                         "&kayakcommission=0.22&commissioncurrency=" + airContext.flightBookingResponse.sumFare.Currency +
                         "&kayakclickid=" + airContext.flightSearchRequest.redirectID + "&bookingtype=flight";
                        LogString = s2sURL;
                        var kk = new System.Net.WebClient().DownloadString(new Uri(s2sURL));

                    }
                    catch (Exception ex)
                    {
                        LogString += (Environment.NewLine + ex.ToString());
                    }
                    try
                    {
                        new Bal.LogWriter(LogString, ("S2S_" + DateTime.Today.ToString("ddMMMyy")));
                    }
                    catch { }
                }
                #endregion

                LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\Booking", airContext.flightBookingRequest.userSearchID, "PaymentSuccess.txt");
                return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                #endregion
            }
            catch (Exception ex)
            {
                sbLogger.Append(Environment.NewLine + "--------------------------------------------- Payment Success Error---------------------------------------------");
                sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                sbLogger.Append(Environment.NewLine + ex.ToString());
                sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\Error", "Error" + DateTime.Today.ToString("ddMMMyy"), "PaymentSuccess.txt");
                new LogWriter(ex.ToString(), "Error10_" + DateTime.Today.ToString("ddMMyy"), "Error");
                return Redirect("/flight/PaymentFail");
            }

        }
        [HttpGet]
        public ActionResult PaymentFail(string ID)
        {
            AirContext airContext = FlightOperation.GetAirContext(ID);
            if (airContext != null)
            {
                airContext.IsGoToPaymentPage = true;
                if (airContext.IsBookingCompleted)
                {
                    return Redirect("/Flight/PaymentFail/" + airContext.flightSearchRequest.userSearchID);
                }
            }
            return View();
        }
        //[HttpPost]
        //public ActionResult PaymentFail(string ID)
        //{
        //    AirContext airContext = FlightOperation.GetAirContext(ID);
        //    if (airContext != null)
        //    {
        //        airContext.IsGoToPaymentPage = true;
        //        if (airContext.IsBookingCompleted)
        //        {
        //            return Redirect("/Flight/PaymentFail/" + airContext.flightSearchRequest.userSearchID);
        //        }
        //    }

        //    StringBuilder sbLogger = new StringBuilder();
        //    try
        //    {
        //        string[] merc_hash_vars_seq;
        //        string merc_hash_string = string.Empty;
        //        string merc_hash = string.Empty;
        //        string order_id = string.Empty;
        //        string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
        //        MojoIndia.Models.PayUMoney objPayU = new MojoIndia.Models.PayUMoney();
        //        if (Request.Form["status"] != null && Request.Form["status"].Equals("success", StringComparison.OrdinalIgnoreCase))
        //        {
        //            merc_hash_vars_seq = hash_seq.Split('|');
        //            Array.Reverse(merc_hash_vars_seq);
        //            merc_hash_string = ConfigurationManager.AppSettings["SALT"] + "|" + Request.Form["status"];
        //            foreach (string merc_hash_var in merc_hash_vars_seq)
        //            {
        //                merc_hash_string += "|";
        //                merc_hash_string = merc_hash_string + (Request.Form[merc_hash_var] != null ? Request.Form[merc_hash_var] : "");
        //            }
        //            Response.Write(merc_hash_string); //return;
        //            merc_hash = objPayU.Generatehash512(merc_hash_string).ToLower();

        //            order_id = Request.Form["txnid"];
        //            //Response.Write("value matched");                  
        //            string ss = new DAL.TransactionDetailsCallBackData().getTransactonData(Convert.ToInt64(order_id.Replace("TRN", "")));
        //            airContext = JsonConvert.DeserializeObject<AirContext>(ss.ToString());
        //            FlightOperation.SetAirContext(airContext);
        //            if (airContext.flightBookingRequest.paymentDetails == null)
        //            {
        //                airContext.flightBookingRequest.paymentDetails = new PaymentDetails();
        //            }
        //            airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts = Request.Form["status"];
        //            airContext.flightBookingRequest.paymentDetails.Hash = Request.Form["hash"];
        //            if (merc_hash != Request.Form["hash"])
        //            {
        //                //Response.Write("Hash value did not matched");
        //                airContext.flightBookingRequest.paymentDetails.IsReturnHashMatched = false;
        //            }
        //            else
        //            {
        //                airContext.flightBookingRequest.paymentDetails.IsReturnHashMatched = true;
        //            }
        //        }
        //        else
        //        {
        //            order_id = Request.Form["txnid"];
        //            //Response.Write("Hash value did not matched");
        //            string ss = new DAL.TransactionDetailsCallBackData().getTransactonData(Convert.ToInt64(order_id.Replace("TRN", "")));
        //            airContext = JsonConvert.DeserializeObject<AirContext>(ss.ToString());
        //            FlightOperation.SetAirContext(airContext);
        //            if (airContext.flightBookingRequest.paymentDetails == null)
        //            {
        //                airContext.flightBookingRequest.paymentDetails = new PaymentDetails();
        //            }
        //            airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts = Request.Form["status"];
        //            airContext.flightBookingRequest.paymentDetails.Hash = Request.Form["hash"];
        //        }
        //        #region check booking is Process or not

        //        System.Data.DataSet ds = new DAL.TransactionDetailsCallBackData().getTransactionDetails(Convert.ToInt64(order_id.Replace("TRN", "")));
        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            return Redirect("/");
        //        }

        //        #endregion
        //        #region MakeBooking
        //        Bal.FlightDetails objFlightDetails = new Bal.FlightDetails();
        //        objFlightDetails.bookFlight(ref airContext, ref sbLogger);

        //        airContext.flightBookingRequest.PNR = airContext.flightBookingResponse.PNR;
        //        airContext.flightBookingRequest.ReturnPNR = airContext.flightBookingResponse.ReturnPNR;
        //        airContext.flightBookingRequest.TvoBookingID = airContext.flightBookingResponse.TvoBookingID;
        //        airContext.flightBookingRequest.TvoReturnBookingID = airContext.flightBookingResponse.TvoReturnBookingID;
        //        airContext.flightBookingRequest.isTickted = airContext.flightBookingResponse.isTickted;

        //        if (airContext.flightBookingResponse.isTickted.Where(k => k == false).ToList().Count > 0)
        //        {
        //            objFlightDetails.TicketFlight(ref airContext, ref sbLogger);
        //        }
        //        airContext.flightBookingResponse.airline = airContext.flightBookingRequest.airline;
        //        airContext.flightBookingResponse.airport = airContext.flightBookingRequest.airport;
        //        airContext.flightBookingResponse.aircraftDetail = airContext.flightBookingRequest.aircraftDetail;
        //        try
        //        {
        //            //SendEmailRequest objSendEmailRequest = new SendEmailRequest();
        //            ////string _BookingStaus = string.Empty;
        //            //objSendEmailRequest.FromEmail = GlobalData.SendEmail;
        //            //objSendEmailRequest.ToEmail ="kundan@flightsmojo.com"; //airContext.flightBookingRequest.emailID;
        //            //objSendEmailRequest.CcEmail = "";
        //            //objSendEmailRequest.BccEmail = "";
        //            //objSendEmailRequest.MailBody = GetMailBody(airContext.flightBookingResponse);
        //            //objSendEmailRequest.MailSubject = "Your Booking is " + airContext.flightBookingResponse.bookingStatus.ToString() + " with Flightsmojo.in :- " + airContext.flightBookingResponse.bookingID.ToString();
        //            //objSendEmailRequest.BookingID = airContext.flightBookingResponse.bookingID;
        //            //objSendEmailRequest.prodID = airContext.flightBookingResponse.prodID;
        //            //objSendEmailRequest.MailType = "confirmation";
        //            //new Bal.SMTP().SendEMail(objSendEmailRequest);
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        #region S2S pixel for Kayak
        //        if (airContext.flightSearchRequest.sourceMedia == "1013")
        //        {
        //            decimal CouponAmount = 0;
        //            if (airContext.flightBookingResponse.CouponAmount > 0 && !string.IsNullOrEmpty(airContext.flightBookingResponse.CouponCode))
        //            {
        //                CouponAmount = airContext.flightBookingResponse.CouponAmount;
        //            }
        //            string LogString = "";
        //            try
        //            {
        //                System.Net.ServicePointManager.Expect100Continue = true;
        //                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls
        //                       | System.Net.SecurityProtocolType.Tls11
        //                       | System.Net.SecurityProtocolType.Tls12
        //                       | System.Net.SecurityProtocolType.Ssl3;

        //                string s2sURL = "https://www.kayak.com/s/s2s/confirm?" +
        //                "partnercode=FLIGHTSMOJO&" +
        //                "bookingid=" + airContext.flightBookingResponse.bookingID.ToString() +
        //                "&bookedon=" + DateTime.Now.ToString("yyyy-MM-ddThh:mm:sszzz") +
        //                 "&price=" + ((airContext.flightBookingResponse.sumFare.grandTotal + airContext.flightBookingRequest.fareIncreaseAmount) - CouponAmount) +
        //                 "&currency=" + airContext.flightBookingResponse.sumFare.Currency +
        //                 "&kayakcommission=0.22&commissioncurrency=" + airContext.flightBookingResponse.sumFare.Currency +
        //                 "&kayakclickid=" + airContext.flightSearchRequest.kayakClickId + "&bookingtype=flight";
        //                LogString = s2sURL;
        //                var kk = new System.Net.WebClient().DownloadString(new Uri(s2sURL));

        //            }
        //            catch (Exception ex)
        //            {
        //                LogString += (Environment.NewLine + ex.ToString());
        //            }
        //            try
        //            {
        //                new Bal.LogWriter(LogString, ("S2S_" + DateTime.Today.ToString("ddMMMyy")));
        //            }
        //            catch { }
        //        }
        //        #endregion
        //        LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\Booking", airContext.flightBookingRequest.userSearchID, "PaymentFail.txt");
        //        return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        sbLogger.Append(Environment.NewLine + "--------------------------------------------- Payment Success Error---------------------------------------------");
        //        sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
        //        sbLogger.Append(Environment.NewLine + ex.ToString());
        //        sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
        //        LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\Error", "Error" + DateTime.Today.ToString("ddMMMyy"), "PaymentFail.txt");
        //        new LogWriter(ex.ToString(), "Error11_" + DateTime.Today.ToString("ddMMyy"), "Error");
        //        return Redirect("/flight/PaymentFail");
        //    }
        //}
        [HttpGet]
        public ActionResult DemoPaymentSuccess(string ID)
        {
            StringBuilder sbLogger = new StringBuilder();
            ViewBag.isShowResult = true;
            AirContext airContext = FlightOperation.GetAirContext(ID);
            if (airContext != null)
            {
                airContext.IsGoToPaymentPage = true;
                airContext.flightSearchResponse = null;
                airContext.flightBookingRequest.transactionID = DAL.IdGenrator.Get("TransactionID");

                new DAL.TransactionDetailsCallBackData().SaveTransactonData(airContext.flightBookingResponse.bookingID, airContext.flightBookingRequest.transactionID, JsonConvert.SerializeObject(airContext));

                string ss = new DAL.TransactionDetailsCallBackData().getTransactonData(airContext.flightBookingRequest.transactionID);
                airContext = JsonConvert.DeserializeObject<AirContext>(ss.ToString());
                if (airContext.flightBookingRequest.paymentDetails == null)
                {
                    airContext.flightBookingRequest.paymentDetails = new PaymentDetails();
                }
                airContext.flightBookingRequest.paymentDetails.OnlinePaymentStauts = "success";
                airContext.flightBookingRequest.paymentDetails.Hash = "";
                airContext.flightBookingRequest.paymentDetails.IsReturnHashMatched = true;
                #region MakeBooking
                Bal.FlightDetails objFlightDetails = new Bal.FlightDetails();
                objFlightDetails.bookFlight(ref airContext, ref sbLogger);

                airContext.flightBookingRequest.PNR = airContext.flightBookingResponse.PNR;
                airContext.flightBookingRequest.ReturnPNR = airContext.flightBookingResponse.ReturnPNR;
                airContext.flightBookingRequest.TvoBookingID = airContext.flightBookingResponse.TvoBookingID;
                airContext.flightBookingRequest.TvoReturnBookingID = airContext.flightBookingResponse.TvoReturnBookingID;
                airContext.flightBookingRequest.isTickted = airContext.flightBookingResponse.isTickted;
                if (airContext.flightBookingResponse.isTickted.Where(k => k == false).ToList().Count > 0)
                {
                    objFlightDetails.TicketFlight(ref airContext, ref sbLogger);
                }
                airContext.flightBookingResponse.airline = airContext.flightBookingRequest.airline;
                airContext.flightBookingResponse.airport = airContext.flightBookingRequest.airport;
                airContext.flightBookingResponse.aircraftDetail = airContext.flightBookingRequest.aircraftDetail;
                try
                {
                    //SendEmailRequest objSendEmailRequest = new SendEmailRequest();
                    //string _BookingStaus = string.Empty;
                    //objSendEmailRequest.FromEmail = GlobalData.SendEmail;
                    //objSendEmailRequest.ToEmail = airContext.flightBookingRequest.emailID;
                    //objSendEmailRequest.CcEmail = "";
                    //objSendEmailRequest.BccEmail = "brij@flightsmojo.com";
                    //objSendEmailRequest.MailBody = GetMailBody(airContext.flightBookingResponse);
                    //objSendEmailRequest.MailSubject = "Your Booking is " + airContext.flightBookingResponse.bookingStatus.ToString() + " with Flightsmojo.in :- " + airContext.flightBookingResponse.bookingID.ToString();
                    //objSendEmailRequest.BookingID = airContext.flightBookingResponse.bookingID;
                    //objSendEmailRequest.prodID = airContext.flightBookingResponse.prodID;
                    //objSendEmailRequest.MailType = "confirmation";

                    //new Bal.SMTP().SendEMail(objSendEmailRequest);
                }
                catch (Exception ex)
                {
                    new LogWriter(ex.ToString(), "Error12_" + DateTime.Today.ToString("ddMMyy"), "Error");
                }

                return Redirect("/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID);
                #endregion
            }
            else
            {
                return Redirect("/");
            }
        }
        [HttpGet]
        public ActionResult FlightConfirmation(string ID)
        {
            ViewBag.isShowResult = true;
            try
            {
                AirContext airContext = FlightOperation.GetAirContext(ID);
                StringBuilder sbLogger = new StringBuilder();
                if (airContext != null)
                {
                    airContext.flightBookingResponse.FareTypeList = new List<FareType>();
                    for (int i = 0; i < airContext.flightBookingResponse.PriceID.Count; i++)
                    {
                        airContext.flightBookingResponse.FareTypeList.Add(airContext.flightBookingResponse.flightResult[i].Fare.FareType);
                    }
                    airContext.flightBookingResponse.redirectID = airContext.flightSearchRequest.redirectID;
                    airContext.IsBookingCompleted = true;
                    airContext.flightBookingResponse.airline = airContext.flightBookingRequest.airline;
                    airContext.flightBookingResponse.airport = airContext.flightBookingRequest.airport;
                    airContext.flightBookingResponse.aircraftDetail = airContext.flightBookingRequest.aircraftDetail;
                    airContext.flightBookingResponse.sumFare = airContext.flightBookingRequest.sumFare;
                    airContext.flightBookingResponse.redirectID = airContext.flightBookingRequest.redirectID;
                    airContext.flightBookingResponse.webengCtr++;
                    return View(airContext.flightBookingResponse);
                }
                else { return Redirect("/"); }
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error13_" + DateTime.Today.ToString("ddMMyy"), "Error");
                return Redirect("/");
            }
        }
        public JsonResult getResult(string ID)
        {
            Core.RefineResult.ResultResponse obj = new Core.RefineResult.ResultResponse();
            StringBuilder sbLogger = new StringBuilder();
            AirContext airContext = FlightOperation.GetAirContext(ID);
            if (airContext != null)
            {
                if (airContext.IsSearchCompleted == false)
                    new Bal.FlightDetails().SearchFlight(ref airContext, GlobalData.isDummyResult);
                if (airContext.flightSearchResponse != null && airContext.flightSearchResponse.Results != null && airContext.flightSearchResponse.Results.Count() > 0 &&
                    airContext.flightSearchResponse.Results[0].Count > 0 && airContext.flightSearchResponse.Results.LastOrDefault().Count > 0)
                {
                    // string output = JsonConvert.SerializeObject(airContext.flightSearchResponse);
                    obj.depCity = airContext.flightSearchResponse.airport.Where(k => k.airportCode == airContext.flightSearchResponse.Results[0][0].FlightSegments[0].Segments[0].Origin).FirstOrDefault().cityName;
                    obj.depDate = airContext.flightSearchResponse.Results[0][0].FlightSegments[0].Segments[0].DepTime.ToString();
                    obj.Currency = airContext.flightSearchResponse.Results.FirstOrDefault().FirstOrDefault().Fare.Currency.ToUpper();
                    obj.isShowDetails = airContext.isShowDetails;
                    obj.isReturn = airContext.flightSearchRequest.tripType == TripType.RoundTrip ? true : false;
                    obj.minPrice = Convert.ToInt32(airContext.flightSearchResponse.Results.FirstOrDefault().FirstOrDefault().Fare.grandTotal);
                    obj.totpax = airContext.flightSearchResponse.adults + airContext.flightSearchResponse.child + airContext.flightSearchResponse.infants;
                    if (obj.minPrice > (airContext.flightSearchResponse.Results.FirstOrDefault().FirstOrDefault().Fare.grandTotal))
                    {
                        obj.minPrice = obj.minPrice - 1;
                    }
                    if (airContext.flightSearchResponse.Results.Count > 1)
                    {
                        if (obj.minPrice > (airContext.flightSearchResponse.Results[1].FirstOrDefault().Fare.grandTotal))
                        {
                            obj.minPrice = Convert.ToInt32(airContext.flightSearchResponse.Results[1].FirstOrDefault().Fare.grandTotal) - 1;
                        }
                    }
                    obj.maxPrice = Convert.ToInt32(Convert.ToInt32(airContext.flightSearchResponse.Results.FirstOrDefault().LastOrDefault().Fare.grandTotal));
                    if (obj.maxPrice < Convert.ToInt32(airContext.flightSearchResponse.Results.FirstOrDefault().LastOrDefault().Fare.grandTotal))
                    {
                        obj.maxPrice = obj.maxPrice + 1;
                    }
                    if (airContext.flightSearchResponse.Results.Count > 1)
                    {
                        if (obj.maxPrice < (airContext.flightSearchResponse.Results[1].LastOrDefault().Fare.grandTotal))
                        {
                            obj.maxPrice = Convert.ToInt32(airContext.flightSearchResponse.Results[1].LastOrDefault().Fare.grandTotal) + 1;
                        }
                    }
                    int kk = 1;
                    try
                    {
                        foreach (var item in airContext.flightSearchResponse.Results.FirstOrDefault())
                        {
                            kk++;
                            Core.RefineResult.Result result = new Core.RefineResult.Result() { markupID = new List<string>() };
                            result.subProvider = item.Fare.subProvider;
                            result.GDS = item.Fare.gdsType.ToString();
                            result.airlineCode = item.FlightSegments[0].Segments[0].Airline;
                            Fare cutPrice = item.FareList.Where(o => o.FareType == FareType.PUBLISH).FirstOrDefault();
                            if (cutPrice == null)
                            {
                                cutPrice = item.FareList.OrderBy(k => k.grandTotal).FirstOrDefault();
                            }
                            result.price = System.Math.Round(item.Fare.grandTotal, 2);
                            result.dPrice = System.Math.Round((result.price / obj.totpax), 0);
                            result.bFare = System.Math.Round(item.Fare.fareBreakdown[0].BaseFare, 2).ToString();
                            result.tax = System.Math.Round((item.Fare.fareBreakdown[0].Tax), 2).ToString();
                            result.CutPrice = Math.Round((cutPrice.grandTotal / obj.totpax)).ToString();
                            result.sPrice = Math.Round((item.Fare.grandTotal / obj.totpax)).ToString();
                            int a = int.Parse(result.CutPrice);
                            int b = int.Parse(result.sPrice);
                            int c = a - b;
                            result.CPrice = c.ToString("0,0", new CultureInfo("HI-in"));
                            if (result.CPrice == "00" || c <= 0)
                            {
                                int d = b + 300;
                                result.CutPrice = d.ToString();
                                result.CPrice = "300";
                            }
                            result.maxSeat = 9;
                            result.resultID = item.ResultID;
                            result.fareType = item.Fare.FareType.ToString();
                            result.mojofare = item.Fare.mojoFareType.ToString();
                            result.SeatAvailable = item.Fare.SeatAvailable;


                            if (result.mojofare == "SeriesFareWithPNR" || result.mojofare == "SeriesFareWithoutPNR")
                            {
                                result.dealType = "Exclusive Offer";
                            }
                            else
                            {
                                result.dealType = "Saver Fare";
                            }

                            if (!string.IsNullOrEmpty(item.Fare.markupID))
                            {
                                string rowData = item.Fare.markupID.Replace("=>", "ᦍ");
                                string[] mName = rowData.Split('ᦍ');
                                foreach (string kks in mName[0].Split('~'))
                                {
                                    result.markupID.Add(kks);
                                }
                                if (mName.Length > 1)
                                {
                                    result.markupID.Add("Markup=>" + mName[mName.Length - 1]);
                                }
                            }
                            #region set Dep/Arr Time Duration
                            int depTime = Convert.ToInt32(item.FlightSegments[0].Segments[0].DepTime.ToString("HHmm"));
                            if (depTime >= 1 && depTime <= 500)
                                result.depTimeDur = 1;
                            else if (depTime >= 501 && depTime <= 1200)
                                result.depTimeDur = 2;
                            else if (depTime >= 1201 && depTime <= 1800)
                                result.depTimeDur = 3;
                            else if (depTime >= 1801 && depTime <= 2359)
                                result.depTimeDur = 4;
                            if (item.FlightSegments.Count > 1)
                            {
                                int arrTime = Convert.ToInt32(item.FlightSegments[1].Segments[0].ArrTime.ToString("HHmm"));
                                if (arrTime >= 1 && arrTime <= 500)
                                    result.arrTimeDur = 1;
                                else if (arrTime >= 501 && arrTime <= 1200)
                                    result.arrTimeDur = 2;
                                else if (arrTime >= 1201 && arrTime <= 1800)
                                    result.arrTimeDur = 3;
                                else if (arrTime >= 1801 && arrTime <= 2359)
                                    result.arrTimeDur = 4;
                            }
                            #endregion
                            result.flightSegments = new List<Core.RefineResult.FlightSegment>();
                            foreach (var fSeg in item.FlightSegments)
                            {
                                Core.RefineResult.FlightSegment fSegment = new Core.RefineResult.FlightSegment() { segments = new List<Core.RefineResult.Segment>() };
                                fSegment.segName = fSeg.SegName;
                                fSegment.Eft = fSeg.Duration;
                                fSegment.TotalTime = (fSeg.Duration / 60) + "h " + (fSeg.Duration % 60) + "m";
                                foreach (var seg in fSeg.Segments)
                                {
                                    #region Set out Bound flight
                                    Airline airline = null;
                                    Airline OptAirline = null;

                                    airline = airContext.flightSearchResponse.airline.Where(x => x.code == seg.Airline).SingleOrDefault();
                                    OptAirline = airContext.flightSearchResponse.airline.Where(x => x.code == seg.OperatingCarrier).SingleOrDefault();
                                    var fromAirPort = airContext.flightSearchResponse.airport.Where(x => x.airportCode == seg.Origin).SingleOrDefault();
                                    var toAirPort = airContext.flightSearchResponse.airport.Where(x => x.airportCode == seg.Destination).SingleOrDefault();
                                    var equip = airContext.flightSearchResponse.aircraftDetail.Where(x => x.aircraftCode == seg.equipmentType).SingleOrDefault();
                                    Core.RefineResult.Segment outSeg = new Core.RefineResult.Segment();
                                    outSeg.airline = new Core.RefineResult.Airline();
                                    outSeg.url = seg.url;

                                    outSeg.airline.code = airline.code;
                                    outSeg.airline.name = airline.name;

                                    outSeg.arrDate = seg.ArrTime.ToString("ddd, dd MMM yy");
                                    outSeg.arrTime = seg.ArrTime.ToString("HH:mm");

                                    outSeg.depDate = seg.DepTime.ToString("ddd, dd MMM yy");
                                    outSeg.depTime = seg.DepTime.ToString("HH:mm");

                                    outSeg.cabinType = seg.CabinClass.ToString();

                                    outSeg.isNearByOrg = false;
                                    outSeg.isNearByDest = false;
                                    outSeg.isFlexiFare = false;

                                    outSeg.dest = new Core.RefineResult.Airport();
                                    outSeg.dest.airportCode = toAirPort.airportCode;
                                    outSeg.dest.airportName = toAirPort.airportName;
                                    outSeg.dest.cityName = toAirPort.cityName;
                                    outSeg.tTerminal = string.IsNullOrEmpty(seg.ToTerminal) ? "" : (toAirPort.airportName + " T-" + seg.ToTerminal);
                                    //outSeg.tTerShort = string .IsNullOrEmpty(seg.ToTerminal)?"": seg.ToTerminal.ToLower().Replace("terminal","");
                                    outSeg.tFullAprName = toAirPort.cityName + " • " + toAirPort.airportName + ", " + (string.IsNullOrEmpty(seg.ToTerminal) ? "" : ("Terminal " + seg.ToTerminal.ToLower().Replace("terminal", ""))) + " (" + toAirPort.airportCode + ")";

                                    outSeg.eft = seg.Duration;
                                    outSeg.equipType = seg.equipmentType;
                                    outSeg.flightNo = seg.FlightNumber;
                                    result.airlineClass += (string.IsNullOrEmpty(result.airlineClass) ? seg.FareClass : ("-" + seg.FareClass));
                                    outSeg.operatedBy = new Core.RefineResult.Airline();
                                    outSeg.operatedBy.code = OptAirline.code;
                                    outSeg.operatedBy.name = OptAirline.name;
                                    outSeg.baggage = "";
                                    outSeg.org = new Core.RefineResult.Airport();
                                    outSeg.org.airportCode = fromAirPort.airportCode;
                                    outSeg.org.airportName = fromAirPort.airportName;
                                    outSeg.fTerminal = string.IsNullOrEmpty(seg.FromTerminal) ? "" : (fromAirPort.airportName + " T-" + seg.FromTerminal);
                                    //outSeg.fTerShort = string.IsNullOrEmpty(seg.FromTerminal) ? "" : seg.FromTerminal.ToLower().Replace("terminal", "");
                                    outSeg.fFullAprName = fromAirPort.cityName + " • " + fromAirPort.airportName + ", " + (string.IsNullOrEmpty(seg.FromTerminal) ? "" : ("Terminal " + seg.FromTerminal.ToLower().Replace("terminal", ""))) + " (" + fromAirPort.airportCode + ")";

                                    outSeg.org.cityName = fromAirPort.cityName;
                                    if (seg.layOverTime > 0)
                                        outSeg.layOverTime = (seg.layOverTime / 60) + "hr " + (seg.layOverTime % 60) + "min at(" + outSeg.dest.airportCode + ")";
                                    else
                                        outSeg.layOverTime = "";
                                    outSeg.stop = 0;

                                    fSegment.segments.Add(outSeg);
                                    #endregion
                                }
                                result.flightSegments.Add(fSegment);
                            }

                            result.stop = item.FlightSegments.Max(o => o.stop);
                            result.stop = result.stop >= 3 ? 3 : result.stop;
                            if (!obj.stop.Exists(o => o.stop == result.stop))
                            {
                                Core.RefineResult.Stop stop = new Core.RefineResult.Stop();
                                stop.stop = result.stop;
                                stop.price = Convert.ToInt32(result.price) / (airContext.flightSearchResponse.adults + airContext.flightSearchResponse.child + airContext.flightSearchResponse.infants);
                                obj.stop.Add(stop);
                            }
                            if (!obj.airlineList.Exists(o => o.code == result.airlineCode))
                            {
                                Core.RefineResult.AirlineList airList = new Core.RefineResult.AirlineList();
                                airList.code = result.flightSegments[0].segments[0].airline.code;
                                airList.url = result.flightSegments[0].segments[0].url;
                                airList.name = result.flightSegments[0].segments[0].airline.name;
                                airList.fare = Convert.ToInt32(result.price) / (airContext.flightSearchResponse.adults + airContext.flightSearchResponse.child + airContext.flightSearchResponse.infants);
                                obj.airlineList.Add(airList);
                            }

                            obj.result.Add(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        CreateLogFile(JsonConvert.SerializeObject(airContext), "Log\\SearchN", airContext.flightSearchRequest.userSearchID + "__1.txt");

                    }
                    kk = 1;
                    try
                    {
                        if (airContext.flightSearchResponse.Results.Count > 1)
                        {
                            obj.resultReturn = new List<Core.RefineResult.Result>();
                            foreach (var item in airContext.flightSearchResponse.Results[1])
                            {
                                kk++;
                                Core.RefineResult.Result result = new Core.RefineResult.Result() { markupID = new List<string>() };
                                result.GDS = item.Fare.gdsType.ToString();
                                result.airlineCode = item.FlightSegments[0].Segments[0].Airline;
                                Fare cutPrice = item.FareList.Where(o => o.FareType == FareType.PUBLISH).FirstOrDefault();
                                if (cutPrice == null)
                                {
                                    cutPrice = item.FareList.OrderBy(k => k.grandTotal).FirstOrDefault();
                                }
                                result.price = System.Math.Round(item.Fare.grandTotal, 2);
                                result.dPrice = System.Math.Round((result.price / obj.totpax), 0);
                                result.bFare = System.Math.Round(item.Fare.fareBreakdown[0].BaseFare, 2).ToString();
                                result.tax = System.Math.Round((item.Fare.fareBreakdown[0].Tax), 2).ToString();
                                result.CutPrice = Math.Round((cutPrice.grandTotal / obj.totpax)).ToString();
                                result.sPrice = Math.Round((item.Fare.grandTotal / obj.totpax)).ToString();
                                int a = int.Parse(result.CutPrice);
                                int b = int.Parse(result.sPrice);
                                int c = a - b;
                                result.CPrice = c.ToString("0,0", new CultureInfo("HI-in"));
                                if (result.CPrice == "00")
                                {
                                    int d = a + 300;
                                    result.CutPrice = d.ToString();
                                    result.CPrice = "300";
                                }

                                result.maxSeat = 9;
                                result.resultID = item.ResultID;

                                result.fareType = item.Fare.FareType.ToString();
                                result.mojofare = item.Fare.mojoFareType.ToString();
                                #region set Dep/Arr Time Duration
                                int depTime = Convert.ToInt32(item.FlightSegments[0].Segments[0].DepTime.ToString("HHmm"));

                                if (depTime >= 1 && depTime <= 500)
                                    result.arrTimeDur = 1;
                                else if (depTime >= 501 && depTime <= 1200)
                                    result.arrTimeDur = 2;
                                else if (depTime >= 1201 && depTime <= 1800)
                                    result.arrTimeDur = 3;
                                else if (depTime >= 1801 && depTime <= 2359)
                                    result.arrTimeDur = 4;

                                #endregion
                                if (!string.IsNullOrEmpty(item.Fare.markupID))
                                {
                                    string rowData = item.Fare.markupID.Replace("=>", "ᦍ");
                                    string[] mName = rowData.Split('ᦍ');
                                    foreach (string kks in mName[0].Split('~'))
                                    {
                                        result.markupID.Add(kks);
                                    }
                                    if (mName.Length > 1)
                                    {
                                        result.markupID.Add("Markup=>" + mName[mName.Length - 1]);
                                    }
                                }



                                result.flightSegments = new List<Core.RefineResult.FlightSegment>();
                                foreach (var fSeg in item.FlightSegments)
                                {
                                    Core.RefineResult.FlightSegment fSegment = new Core.RefineResult.FlightSegment() { segments = new List<Core.RefineResult.Segment>() };
                                    fSegment.segName = fSeg.SegName;
                                    fSegment.Eft = fSeg.Duration;
                                    fSegment.TotalTime = (fSeg.Duration / 60) + "h " + (fSeg.Duration % 60) + "m";
                                    foreach (var seg in fSeg.Segments)
                                    {
                                        #region Set out Bound flight
                                        Airline airline = null;
                                        Airline OptAirline = null;

                                        airline = airContext.flightSearchResponse.airline.Where(x => x.code == seg.Airline).SingleOrDefault();
                                        OptAirline = airContext.flightSearchResponse.airline.Where(x => x.code == seg.OperatingCarrier).SingleOrDefault();


                                        var fromAirPort = airContext.flightSearchResponse.airport.Where(x => x.airportCode == seg.Origin).SingleOrDefault();
                                        var toAirPort = airContext.flightSearchResponse.airport.Where(x => x.airportCode == seg.Destination).SingleOrDefault();
                                        var equip = airContext.flightSearchResponse.aircraftDetail.Where(x => x.aircraftCode == seg.equipmentType).SingleOrDefault();
                                        Core.RefineResult.Segment outSeg = new Core.RefineResult.Segment();

                                        outSeg.airline = new Core.RefineResult.Airline();
                                        outSeg.airline.code = airline.code;
                                        outSeg.airline.name = airline.name;
                                        outSeg.url = seg.url;

                                        outSeg.arrDate = seg.ArrTime.ToString("ddd, dd MMM yy");
                                        outSeg.arrTime = seg.ArrTime.ToString("HH:mm");

                                        outSeg.depDate = seg.DepTime.ToString("ddd, dd MMM yy");
                                        outSeg.depTime = seg.DepTime.ToString("HH:mm");

                                        outSeg.cabinType = seg.CabinClass.ToString();

                                        outSeg.isNearByOrg = false;
                                        outSeg.isNearByDest = false;
                                        outSeg.isFlexiFare = false;

                                        outSeg.dest = new Core.RefineResult.Airport();
                                        outSeg.dest.airportCode = toAirPort.airportCode;
                                        outSeg.dest.airportName = toAirPort.airportName;
                                        outSeg.tTerminal = string.IsNullOrEmpty(seg.ToTerminal) ? "" : (toAirPort.airportName + " T-" + seg.ToTerminal);
                                        // outSeg.tTerShort = string.IsNullOrEmpty(seg.ToTerminal) ? "" : seg.ToTerminal.ToLower().Replace("terminal", "");
                                        outSeg.tFullAprName = toAirPort.cityName + " • " + toAirPort.airportName + ", " + (string.IsNullOrEmpty(seg.ToTerminal) ? "" : ("Terminal " + seg.ToTerminal.ToLower().Replace("terminal", ""))) + " (" + toAirPort.airportCode + ")";

                                        outSeg.dest.cityName = toAirPort.cityName;

                                        outSeg.eft = seg.Duration;
                                        outSeg.equipType = seg.equipmentType;
                                        outSeg.flightNo = seg.FlightNumber;

                                        result.airlineClass += (string.IsNullOrEmpty(result.airlineClass) ? seg.FareClass : ("-" + seg.FareClass));

                                        outSeg.operatedBy = new Core.RefineResult.Airline();
                                        outSeg.operatedBy.code = OptAirline.code;
                                        outSeg.operatedBy.name = OptAirline.name;
                                        outSeg.baggage = "";

                                        outSeg.org = new Core.RefineResult.Airport();
                                        outSeg.org.airportCode = fromAirPort.airportCode;
                                        outSeg.org.airportName = fromAirPort.airportName;
                                        outSeg.fTerminal = string.IsNullOrEmpty(seg.FromTerminal) ? "" : (fromAirPort.airportName + " T-" + seg.FromTerminal);
                                        //outSeg.fTerShort = string.IsNullOrEmpty(seg.FromTerminal) ? "" : seg.FromTerminal.ToLower().Replace("terminal", "");
                                        outSeg.fFullAprName = fromAirPort.cityName + " • " + fromAirPort.airportName + ", " + (string.IsNullOrEmpty(seg.FromTerminal) ? "" : ("Terminal " + seg.FromTerminal.ToLower().Replace("terminal", ""))) + " (" + fromAirPort.airportCode + ")";

                                        outSeg.org.cityName = fromAirPort.cityName;
                                        if (seg.layOverTime > 0)
                                            outSeg.layOverTime = (seg.layOverTime / 60) + "hr " + (seg.layOverTime % 60) + "min at(" + outSeg.dest.airportCode + ")";
                                        else
                                            outSeg.layOverTime = "";
                                        outSeg.stop = 0;

                                        fSegment.segments.Add(outSeg);
                                        #endregion
                                    }
                                    result.flightSegments.Add(fSegment);
                                }

                                result.stop = item.FlightSegments.Max(o => o.stop);
                                result.stop = result.stop >= 3 ? 3 : result.stop;
                                if (!obj.stop.Exists(o => o.stop == result.stop))
                                {
                                    Core.RefineResult.Stop stop = new Core.RefineResult.Stop();
                                    stop.stop = result.stop;
                                    stop.price = Convert.ToInt32(result.price) / (airContext.flightSearchResponse.adults + airContext.flightSearchResponse.child + airContext.flightSearchResponse.infants);
                                    obj.stop.Add(stop);
                                }
                                if (!obj.airlineList.Exists(o => o.code == result.airlineCode))
                                {
                                    Core.RefineResult.AirlineList airList = new Core.RefineResult.AirlineList();
                                    airList.code = result.flightSegments[0].segments[0].airline.code;
                                    airList.url = result.flightSegments[0].segments[0].url;
                                    airList.name = result.flightSegments[0].segments[0].airline.name;
                                    airList.fare = Convert.ToInt32(result.price) / (airContext.flightSearchResponse.adults + airContext.flightSearchResponse.child + airContext.flightSearchResponse.infants);
                                    obj.airlineList.Add(airList);
                                }
                                obj.resultReturn.Add(result);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CreateLogFile(JsonConvert.SerializeObject(airContext), "Log\\SearchN", airContext.flightSearchRequest.userSearchID + "__2.txt");
                    }
                    obj.stop = obj.stop.OrderBy(o => o.stop).ToList();
                }
                else if (airContext.flightSearchRequest.segment[0].travelDate < DateTime.Today)
                {
                    LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\NoResult", "PastDate_" + DateTime.Today.ToString("ddMMMyy"), airContext.flightSearchRequest.userSearchID + ".txt");
                    obj.redirectURl = "/";
                }
                else
                {
                    LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\NoResult", "NoResult_" + DateTime.Today.ToString("ddMMMyy"), airContext.flightSearchRequest.userSearchID + ".txt");
                    obj.redirectURl = "/flight/NoResult/" + airContext.flightSearchRequest.userSearchID;
                }
                bookingLog(ref sbLogger, "Original Request", JsonConvert.SerializeObject(airContext.flightSearchRequest));
                bookingLog(ref sbLogger, "Original Response", JsonConvert.SerializeObject(obj));
                CreateLogFile(sbLogger.ToString(), "NewLog\\Search", airContext.flightSearchRequest.userSearchID + ".txt");
            }
            else
            {
                obj.redirectURl = "/";
            }
            //string output = JsonConvert.SerializeObject(obj);

            JsonResult jsonR = Json(obj, JsonRequestBehavior.AllowGet);
            jsonR.MaxJsonLength = Int32.MaxValue;
            return jsonR;
        }
        public static void CreateLogFile(string logMessage, string PathPrefix, string fileName)
        {
            try
            {
                using (StreamWriter w = System.IO.File.AppendText(AppDomain.CurrentDomain.BaseDirectory + PathPrefix + "\\" + fileName))
                {
                    w.WriteLine("  :{0}", logMessage);
                }
            }
            catch
            {
            }
        }

        public JsonResult SaveBookingDetails(string ID, string EmailID, string phoneNo, string GSTNo, string GSTCompany, bool mode)
        {
            CouponStatusResponse objResponse = new CouponStatusResponse();
            try
            {
                AirContext airContext = FlightOperation.GetAirContext(ID);
                StringBuilder sbLogger = new StringBuilder();
                string pathFlightSearch = System.IO.Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "NewLog\\Search", airContext.flightSearchRequest.userSearchID + ".txt");
                if (System.IO.Directory.Exists(pathFlightSearch))
                {
                    string strFlightSearch = "";
                    using (System.IO.StreamReader r = new System.IO.StreamReader(pathFlightSearch))
                    {
                        strFlightSearch = r.ReadToEnd();
                    }
                    sbLogger.Append(strFlightSearch);
                    sbLogger.Append(Environment.NewLine);
                }
                else
                {
                    try
                    {
                        string strFlightSearch = "";
                        using (System.IO.StreamReader r = new System.IO.StreamReader(pathFlightSearch))
                        {
                            strFlightSearch = r.ReadToEnd();
                        }
                        sbLogger.Append(strFlightSearch);
                        sbLogger.Append(Environment.NewLine);
                    }
                    catch
                    {


                    }
                }
                CreateLogFile(sbLogger.ToString(), "NewLog\\Search", airContext.flightSearchRequest.userSearchID + ".txt");

                if (airContext.IsBookingCompleted == true)
                {
                    objResponse.RedirectUrl = "/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID;
                }
                else
                {
                    if (airContext != null)
                    {
                        airContext.flightBookingRequest.emailID = EmailID;
                        airContext.flightBookingRequest.phoneNo = phoneNo;
                        airContext.flightBookingRequest.GSTNo = GSTNo;
                        airContext.flightBookingRequest.GSTCompany = GSTCompany;
                        airContext.flightBookingRequest.isWhatsapp = mode;

                        airContext.flightBookingRequest.bookingID = airContext.bookingID;
                        var kk = new Bal.FlightDetails().SaveBookingDetails_WithOutPax(airContext.flightBookingRequest, ref sbLogger);

                        if (airContext.bookingID == 0)
                        {
                            airContext.bookingID = kk.bookingID;
                        }
                        airContext.flightBookingRequest.bookingID = kk.bookingID;
                        airContext.flightBookingRequest.prodID = kk.prodID;
                        objResponse.BookingID = kk.bookingID;
                        // groupbooking(phoneNo);
                        //sendwhatsappAT(phoneNo);
                        CallDeleteFiles();
                        //CreateLogFile(sbLogger.ToString(), "Log\\Passenger", kk.bookingID.ToString() + ".txt");
                        CreateLogFile(sbLogger.ToString(), "NewLog\\Booking", kk.bookingID.ToString() + ".txt");
                    }
                    else
                    {
                        objResponse.responseStatus.status = TransactionStatus.Error;
                        objResponse.responseStatus.message = "Invalide coupon, Please try another coupon.";
                    }
                }
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error15_" + DateTime.Today.ToString("ddMMyy"), "Error");
                objResponse.responseStatus.status = TransactionStatus.Error;
                objResponse.responseStatus.message = "Invalide coupon, Please try another coupon.";

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SavePassengerDetails(string ID, List<PassengerDetails> Paxlst)
        {
            StringBuilder sbLogger = new StringBuilder();
            Core.CouponStatusResponse objResponse = new CouponStatusResponse();
            ViewBag.isShowResult = true;
            try
            {
                AirContext airContext = FlightOperation.GetAirContext(ID);
                if (airContext.IsBookingCompleted == true)
                {
                    objResponse.RedirectUrl = "/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID;
                }
                else
                {
                    if (airContext != null)
                    {
                        airContext.flightBookingRequest.passengerDetails = Paxlst;

                        List<string> pname = new List<string>();
                        bool ErrorList1 = false;
                        foreach (PassengerDetails pax in airContext.flightBookingRequest.passengerDetails)
                        {
                            if (pax.passengerType == PassengerType.Adult)
                            {
                                if (airContext.flightBookingRequest.travelType == Core.TravelType.International)
                                {
                                    pax.dateOfBirth = Convert.ToDateTime(pax.year + "-" + pax.month + "-" + pax.day);
                                }
                                else
                                {
                                    pax.dateOfBirth = DateTime.Today.AddYears(-24);
                                }
                            }
                            else
                            {
                                pax.dateOfBirth = Convert.ToDateTime(pax.year + "-" + pax.month + "-" + pax.day);
                            }
                            if (pax.title.Equals("Mr", StringComparison.OrdinalIgnoreCase) || pax.title.Equals("Master", StringComparison.OrdinalIgnoreCase) || pax.title.Equals("MSTR", StringComparison.OrdinalIgnoreCase))
                            {
                                pax.gender = Gender.Male;
                            }
                            else
                            {
                                pax.gender = Gender.Female;
                            }
                            if (!string.IsNullOrEmpty(pax.exYear) && !string.IsNullOrEmpty(pax.exMonth) && !string.IsNullOrEmpty(pax.exDay))
                            {
                                pax.expiryDate = Convert.ToDateTime(pax.exYear + "-" + pax.exMonth + "-" + pax.exDay);
                            }
                            if (!string.IsNullOrEmpty(pax.issueYear) && !string.IsNullOrEmpty(pax.issueMonth) && !string.IsNullOrEmpty(pax.issueDay))
                            {
                                pax.passportIssueDate = Convert.ToDateTime(pax.issueYear + "-" + pax.issueMonth + "-" + pax.issueDay);
                            }
                            if (!string.IsNullOrEmpty(pax.passportNumber))
                            {
                                pax.passportNumber = pax.passportNumber.Replace(" ", "");
                            }
                            string plName = pax.firstName.Trim() + pax.middleName + pax.lastName.Trim();
                            if (pname.Contains(plName))
                            {
                                ErrorList1 = true;
                            }
                            if ((airContext.flightBookingRequest.flightResult[0].FlightSegments[0].Segments[0].Airline == "2T" || airContext.flightBookingRequest.flightResult[0].FlightSegments[0].Segments[0].Airline == "ZO"))
                            {
                                if (pax.lastName.Trim().IndexOf(" ") != -1)
                                {
                                    string[] strLame = pax.lastName.Trim().Split(' ');
                                    pax.lastName = strLame[strLame.Length - 1];
                                    for (int i = 0; i < strLame.Length - 1; i++)
                                    {
                                        pax.firstName += (" " + strLame[i]);
                                    }
                                }
                            }
                            if (airContext.flightBookingRequest.flightResult.Count > 1)
                            {
                                if ((airContext.flightBookingRequest.flightResult[1].FlightSegments[0].Segments[0].Airline == "2T" || airContext.flightBookingRequest.flightResult[1].FlightSegments[0].Segments[0].Airline == "ZO"))
                                {
                                    if (pax.lastName.Trim().IndexOf(" ") != -1)
                                    {
                                        string[] strLame = pax.lastName.Trim().Split(' ');
                                        pax.lastName = strLame[strLame.Length - 1];
                                        for (int i = 0; i < strLame.Length - 1; i++)
                                        {
                                            pax.firstName += (" " + strLame[i]);
                                        }
                                    }
                                }
                            }
                        }
                        Bal.FlightDetails objFlightDetails = new Bal.FlightDetails();
                        airContext.flightBookingResponse = objFlightDetails.Update_BookingPaxDetail(airContext.flightBookingRequest, ref sbLogger);
                        airContext.flightBookingRequest.bookingID = airContext.flightBookingResponse.bookingID;

                        // new DAL.LogWriter_New(sbLogger.ToString(), airContext.flightBookingResponse.bookingID.ToString(), "UpdatePassengerDetail");
                        CreateLogFile(sbLogger.ToString(), "NewLog\\Booking", airContext.flightBookingResponse.bookingID.ToString() + ".txt");
                    }
                }
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "PostPassengerException" + DateTime.Today.ToString("ddMMyy"), "Error");
            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveAddones(string ID, string CPolicy, string RPolicy)
        {

            StringBuilder sbLogger = new StringBuilder();
            Core.CouponStatusResponse objResponse = new CouponStatusResponse();
            try
            {
                AirContext airContext = FlightOperation.GetAirContext(ID);

                if (airContext.IsBookingCompleted == true)
                {
                    objResponse.RedirectUrl = "/flight/FlightConfirmation/" + airContext.flightSearchRequest.userSearchID;
                }
                else
                {
                    if (airContext != null)
                    {
                        airContext.flightBookingRequest.isBuyCancellaionPolicy = Convert.ToBoolean(CPolicy);
                        airContext.flightBookingRequest.isBuyRefundPolicy = Convert.ToBoolean(RPolicy);
                    }
                    else
                    {
                        objResponse.responseStatus.status = TransactionStatus.Error;
                        objResponse.responseStatus.message = "Invalide coupon, Please try another coupon.";
                    }
                }
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error15_" + DateTime.Today.ToString("ddMMyy"), "Error");
                objResponse.responseStatus.status = TransactionStatus.Error;
                objResponse.responseStatus.message = "Invalide coupon, Please try another coupon.";

            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        public string GetAirportCode(string FullString)
        {
            string[] arrs = FullString.Split('-');
            if (arrs.Length > 1)
            {
                return arrs[0].Trim().ToUpper();
            }
            else
            {
                return arrs[0].Trim().Substring(0, 3).ToUpper();

            }
        }
        private string getSearchID()
        {
            return (DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid().ToString("N"));
        }
        public void setCookie(string sourceMedia)
        {
            int i;
            bool bNum = int.TryParse(sourceMedia, out i);
            sourceMedia = bNum ? i.ToString() : "1000";

            HttpCookie FMsMedia = new HttpCookie("FMsMediaIndia");
            FMsMedia["sMediaIndia"] = sourceMedia;
            // FMsMedia.Expires = DateTime.Now.AddHours(1);
            FMsMedia.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(FMsMedia);
        }
        public string GetCookie()
        {
            string sMedia = "";
            HttpCookie FMsMedia = Request.Cookies["FMsMediaIndia"];
            if (FMsMedia != null)
            {
                sMedia = FMsMedia["sMediaIndia"].ToString();
            }
            if (string.IsNullOrEmpty(sMedia))
                sMedia = "1000";
            return sMedia;
        }
        private static List<PassengerDetails> GetPassengerDefault(FlightSearchRequest search)
        {
            List<PassengerDetails> passengers = new List<PassengerDetails>();
            int Ctr = 0;
            for (int i = 0; i < search.adults; i++)
            {
                passengers.Add(new PassengerDetails()
                {
                    travelerNo = Ctr,
                    passengerType = PassengerType.Adult,
                    gender = Gender.None
                });
                Ctr++;
            }
            for (int i = 0; i < search.child; i++)
            {
                passengers.Add(new PassengerDetails()
                {
                    travelerNo = Ctr,
                    passengerType = PassengerType.Child,
                    gender = Gender.None
                });
                Ctr++;
            }
            for (int i = 0; i < search.infants; i++)
            {
                passengers.Add(new PassengerDetails()
                {
                    travelerNo = Ctr,
                    passengerType = PassengerType.Infant,
                    gender = Gender.None
                });
                Ctr++;
            }
            return passengers;
        }
        private string creditCardTypeFromNumber(string num)
        {
            if (num.Substring(0, 1) == "4")
            {
                return "VI";
            }
            else if (num.Length >= 2 && (num.Substring(0, 2) == "34" || num.Substring(0, 2) == "37"))
            {
                return "AX";
            }
            else if (num.Substring(0, 1) == "5")
            {
                return "CA";
            }
            else if (num.Substring(0, 1) == "6")
            {
                return "DS";
            }
            else if (num.Length >= 2 && (num.Substring(0, 2) == "35"))
            {
                return "JC";
            }
            else if (num.Length >= 2 && (num.Substring(0, 2) == "36" || num.Substring(0, 2) == "38"))
            {
                return "DC";
            }
            else
            {
                return "None";
            }
        }
        public void SetDummyResult(AirContext ac)
        {
            ac.flightBookingResponse = new FlightBookingResponse()
            {
                adults = ac.flightBookingRequest.adults,
                child = ac.flightBookingRequest.child,
                infants = ac.flightBookingRequest.infants,
                infantsWs = ac.flightBookingRequest.infantsWs,
                bookingID = 10001,
                emailID = ac.flightBookingRequest.emailID,
                flightResult = ac.flightBookingRequest.flightResult,
                mobileNo = ac.flightBookingRequest.mobileNo,
                passengerDetails = ac.flightBookingRequest.passengerDetails,
                paymentDetails = ac.flightBookingRequest.paymentDetails,
                phoneNo = ac.flightBookingRequest.phoneNo,
                PNR = "FLMOJO",
                prodID = 1,
                responseStatus = new ResponseStatus(),
                siteID = ac.flightBookingRequest.siteID,
                sourceMedia = ac.flightBookingRequest.sourceMedia,
                transactionID = 00236,
                updatedBookingAmount = 0,
                userSearchID = ac.flightBookingRequest.userSearchID,
                userSessionID = ac.flightBookingRequest.userSessionID,
            };
        }
        public bool checkIsFareExist(ref AirContext ac)
        {
            bool retVal = false;
            if (ac.flightSearchRequest.travelType == TravelType.Domestic)
            {
                if (ac.flightSearchRequest.tripType == TripType.RoundTrip)
                {
                    retVal = (!string.IsNullOrEmpty(ac.flightSearchResponse.result1Index) && !string.IsNullOrEmpty(ac.flightSearchResponse.result2Index));
                }
                else
                {
                    retVal = !string.IsNullOrEmpty(ac.flightSearchResponse.result1Index);
                }
            }
            else
            {
                retVal = !string.IsNullOrEmpty(ac.flightSearchResponse.result1Index);
            }
            return retVal;
        }
        public bool checkResultIsExist(ref AirContext airContest, string resultReference)
        {
            bool retVal = false;
            if (airContest.flightSearchResponse.Results != null && airContest.flightSearchResponse.Results.Count > 0)
            {
                foreach (FlightResult result in airContest.flightSearchResponse.Results[0])
                {
                    if (!retVal)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(result.Fare.grandTotal.ToString("f2") + "_");

                        if (result.FlightSegments != null && result.FlightSegments.Count > 0)
                        {
                            foreach (var fSeg in result.FlightSegments)
                            {
                                foreach (Segment seg in fSeg.Segments)
                                {
                                    sb.Append(seg.FlightNumber + "_");
                                }
                            }
                        }
                        if (resultReference.Equals(sb.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            airContest.flightSearchResponse.result1Index = result.ResultIndex;
                            retVal = true;
                        }
                    }
                }
            }
            return retVal;
        }
        public TravelType getTravelType(string FromCountry, string ToCountry)
        {

            if (FromCountry.Equals("IN", StringComparison.OrdinalIgnoreCase) && ToCountry.Equals("IN", StringComparison.OrdinalIgnoreCase))
            {
                return TravelType.Domestic;
            }
            else
                return TravelType.International;
        }
        public string GetMailBody(FlightBookingResponse fsr)
        {
            fsr.FareTypeList = new List<FareType>();
            for (int i = 0; i < fsr.PriceID.Count; i++)
            {
                var p = fsr.flightResult[i].FareList.Where(k => k.Tgy_FareID.Equals(fsr.PriceID[i], StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (p != null)
                {
                    fsr.FareTypeList.Add(p.FareType);
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<div style='font-family:Arial, Helvetica, sans-serif; width:100%; margin:0 auto;'>");
            sb.Append("    <table cellpadding='0' cellspacing='0' align='center' border='0' style='width:800px; background-color:#ffffff; text-align:center; font-size:12px;padding:10px 10px 10px 10px ; line-height:30px; font-family:Arial, Helvetica, sans-serif; box-sizing: border-box;'>");
            sb.Append("        <tr>");
            sb.Append("            <td align='center' valign='top'>");
            sb.Append("                <table cellpadding='0' cellspacing='0' border='0' style='background-color:#49494a; border: 1px solid #ccc; border-collapse: collapse; width:100%; font-family:Arial, Helvetica, sans-serif;'>");
            sb.Append("                    <tr>");
            sb.Append("                        <td style='text-align:left; padding-left:20px; background-color: #fa7760; border:1px solid #fa7760; width:50%;'><img style='width:150px;' src='" + GlobalData.URL + "/images/logo_white.png' alt='logo' /></td>");
            sb.Append("                        <td style='text-align:right; padding-right: 20px;background-color: #fa7760; border:1px solid #fa7760; width:50%;'><p style='color: #ffffff; font-size: 13px; margin: 0; padding: 0px; font-weight: bold;'>CALL US ON</p><a href='tel: " + GlobalData.Phone + "' target='_blank' style='color: #ffffff; font-size: 20px; text-decoration: none; font-weight: bold;'> " + GlobalData.Phone + "</a></td>");
            sb.Append("                    </tr>");
            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='width:100%; vertical-align: middle; border-bottom: 1px solid #ffffff;'>");
            sb.Append("                            <table align='center' width='60%' style='padding:10px 0'>");
            sb.Append("                                <tr>");
            sb.Append("                                    <td style='width: 10%;'>&nbsp;</td>");
            sb.Append("                                    <td style='font-size: 25px; color: #ffffff; padding-left: 20px;'><span style='border-bottom: 1px solid #ffffff;'> Booking Confirmation</span></td>");
            sb.Append("                                </tr>");
            sb.Append("                                <tr>");
            sb.Append("                                    <td style='width: 10%;'>&nbsp;</td>");
            sb.Append("                                    <td style='font-size: 14px; color: #ffffff;'>Booking Reference Number - <b>" + fsr.bookingID + "</b></td>");
            sb.Append("                                </tr>");
            sb.Append("                            </table>");
            sb.Append("                        </td>");
            sb.Append("                    </tr>");
            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='padding:10px 20px; font-size: 25px; color: #ffffff;'>Booking Status - <span>" + (fsr.bookingStatus == Core.BookingStatus.NONE ? "InProgress" : fsr.bookingStatus.ToString()) + "</span></td>");
            sb.Append("                    </tr>");
            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='padding:10px 20px; font-size: 25px; color: #ffffff;'>Payment Status - <span>" + fsr.paymentStatus.ToString() + "</span></td>");
            sb.Append("                    </tr>");

            if (fsr.bookingStatus == BookingStatus.Ticketed)
            {
                sb.Append("                     <tr>");
                sb.Append("                        <td colspan='2' style='font-size: 13px;padding:6px 20px; color: #ffffff; line-height: 22px;'>Your booking has been successfully confirmed and ticketed. Please carry the printout of your e - ticket along with passport to the airline check -in counter.</td>");
                sb.Append("                     </tr>");
            }
            else if (fsr.bookingStatus == BookingStatus.Confirmed)
            {
                sb.Append("                     <tr>");
                sb.Append("                        <td colspan='2' style='font-size: 13px;padding:6px 20px; color: #ffffff; line-height: 22px;'>Your booking is confirmed and the e-ticket will be emailed to you shortly.</td>");
                sb.Append("                     </tr>");
            }
            else if (fsr.bookingStatus == BookingStatus.Failed)
            {
                sb.Append("                     <tr>");
                sb.Append("                        <td colspan='2' style='font-size: 13px;padding:6px 20px; color: #ffffff; line-height: 22px;'>We’re sorry. There’s been a problem with your booking. We know it’s annoying, but it happens. There’s no need to worry, we are trying to resolve the issue and will update you shortly.</td>");
                sb.Append("                     </tr>");
            }
            else if (fsr.bookingStatus == BookingStatus.Cancelled)
            {
                sb.Append("                     <tr>");
                sb.Append("                        <td colspan='2' style='font-size: 13px;padding:6px 20px; color: #ffffff; line-height: 22px;'>Your booking has been successfully cancelled. Your financial institution will credit any applicable refund, using the original form of payment within next 72 hours.</td>");
                sb.Append("                     </tr>");
            }
            else if (fsr.bookingStatus == BookingStatus.Pending)
            {
                sb.Append("                     <tr>");
                sb.Append("                        <td colspan='2' style='font-size: 13px;padding:6px 20px; color: #ffffff; line-height: 22px;'>We’re sorry. There’s been a problem with your booking. We know it’s annoying, but it happens. There’s no need to worry, we are trying to resolve the issue and will update you shortly.</td>");
                sb.Append("                     </tr>");
            }
            else
            {
                sb.Append("                     <tr>");
                sb.Append("                        <td colspan='2' style='font-size: 13px;padding:6px 20px; color: #ffffff; line-height: 22px;'>Your booking has one or more flights in progress, you will receive an email as each flight is confirmed. Please wait for the e-ticket(s) to be generated.</td>");
                sb.Append("                     </tr>");
            }

            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='font-size: 13px;padding:6px 20px; color: #ffffff; line-height: 22px;'> All details sent to " + fsr.emailID + "</td>");
            sb.Append("                    </tr>");
            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='padding: 0 20px; color: #ffffff;  font-weight: bold;'>");
            sb.Append("                            <h2 style='background: #fa7760;padding:0;margin: 0;font-size:18px;text-align: center;'>Itinerary Detail</h2>");
            sb.Append("                        </td>");
            sb.Append("                    </tr>");

            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='padding: 0 20px;'>");
            sb.Append("                            <table cellspacing='0' cellpadding='0' style='width: 100%; border: 1px solid #ffffff;border-top:0px;'>");
            int ctr = 0;
            foreach (var result in fsr.flightResult)
            {
                foreach (var fSeg in result.FlightSegments)
                {
                    sb.Append("                                <tr>");
                    sb.Append("                                    <td colspan='4' style='padding:10px 20px 0px 20px; border-bottom: 0.5px dotted #ffffff;'>");
                    if ((fSeg.SegName.ToUpper() == "DEPART"))
                    {
                        sb.Append("                                    <h2 style='color: #ffffff; font-size: 14px;text-align: left; padding :0; margin: 0;'>Onward Flight</h2>");
                    }
                    else
                    {
                        sb.Append("                                    <h2 style='color: #ffffff; font-size: 14px;text-align: left; padding :0; margin: 0;'>Return Flight</h2>");
                    }
                    sb.Append("                                        <br /><span style='color: #ffffff;'> " + fSeg.Segments[0].Origin + " to " + fSeg.Segments[fSeg.Segments.Count - 1].Destination + "</span>");
                    sb.Append("                                    </td>");
                    sb.Append("                                    <td style='padding:10px 20px 0px 20px; border-bottom: 0.5px dotted #ffffff; font-size: 14px;'>");
                    sb.Append("                                        <span style='color: #ffffff; font-size: 14px;'>");
                    sb.Append("                                            PNR");
                    sb.Append("                                        </span><br />");
                    sb.Append("                                        <h2 style='color: #ffffff; font-size: 14px;text-align: left; padding :0; margin: 0;'>" + (ctr == 0 ? (string.IsNullOrEmpty(fsr.PNR) ? "Pending" : fsr.PNR) : (string.IsNullOrEmpty(fsr.ReturnPNR) ? "Pending" : fsr.ReturnPNR)) + "</h2>");
                    sb.Append("                                    </td>");
                    sb.Append("                                </tr>");
                    sb.Append("                                <tr>");
                    sb.Append("                                    <td colspan='2' style='padding: 0 20px;'></td>");
                    sb.Append("                                </tr>");
                    foreach (var item in fSeg.Segments)
                    {
                        var airline = fsr.airline.Where(x => x.code == item.Airline).SingleOrDefault();
                        var fromAirport = fsr.airport.Where(x => x.airportCode == item.Origin).SingleOrDefault();
                        var toAirport = fsr.airport.Where(x => x.airportCode == item.Destination).SingleOrDefault();
                        var operatedAirline = fsr.airline.Where(x => x.code == item.OperatingCarrier).SingleOrDefault();
                        sb.Append("                                <tr>");
                        sb.Append("                                    <td style='font: normal 12px Arial, Helvetica, sans-serif;padding:5px 5px 5px 10px;'><img src='" + GlobalData.URL + "/images/AirlineLogo/" + airline.code + ".gif'></td>");
                        sb.Append("                                    <td style='font: normal 12px Arial, Helvetica, sans-serif;padding:5px 5px 15px 10px;line-height:20px; color: #ffffff;'>" + airline.name + "<br />" + airline.code + " " + item.FlightNumber);
                        if (item.Airline != operatedAirline.code && !string.IsNullOrEmpty(operatedAirline.name))
                        {
                            sb.Append("                                <br />Operated by : " + operatedAirline.name + "");
                        }
                        sb.Append("                                    </td>");
                        sb.Append("                                    <td style='font: normal 12px Arial, Helvetica, sans-serif;padding:5px 5px 15px 10px;line-height:20px;color: #ffffff;'><b>" + item.DepTime.ToString("hh:mm tt") + "</b> " + item.DepTime.ToString("ddd, dd MMM yyyy") + "<br /><b>" + item.ArrTime.ToString("hh:mm tt") + "</b> " + item.ArrTime.ToString("ddd, dd MMM yyyy") + "</td>");
                        sb.Append("                                    <td style='font: normal 12px Arial, Helvetica, sans-serif;padding:5px 5px 15px 10px;line-height:20px;color: #ffffff;'>" + fromAirport.cityName + "(" + fromAirport.airportCode + ")<br />" + toAirport.cityName + "(" + toAirport.airportCode + ")</td>");
                        sb.Append("                                    <td style='font: normal 12px Arial, Helvetica, sans-serif;padding:5px 5px 15px 10px;line-height:20px;color: #ffffff;'>" + ((!string.IsNullOrEmpty(item.FromTerminal)) ? ("Terminal: " + item.FromTerminal) : item.CabinClass.ToString()) + "<br /> " + ((!string.IsNullOrEmpty(item.ToTerminal)) ? ("Terminal: " + item.ToTerminal) : (item.Duration > 0 ? ((item.Duration / 60) + "hr " + (item.Duration % 60) + "min") : "Non Stop")) + "</td>");
                        sb.Append("                                </tr>");
                        sb.Append("                                <tr>");
                        sb.Append("                                    <td colspan='5' style='height:2px; border-top: 1px solid #ffffff;'>&nbsp;</td>");
                        sb.Append("                                </tr>");
                    }
                }
                ctr++;
            }
            sb.Append("                            </table>");
            sb.Append("                        </td>");
            sb.Append("                    </tr>");
            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='padding: 15px 15px 0 20px; color: #ffffff; font-size:18px; font-weight: bold;'>");
            sb.Append("                            <h2 style='background:#fa7760; margin: 0; padding: 0;font-size: 18px;text-align: center;'>Passenger Details</h2>");
            sb.Append("                        </td>");
            sb.Append("                    </tr>");
            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='font-size: 13px; color: #ffffff;'>");
            sb.Append("                            <table cellspacing='0' cellpadding='0' style='width: 100%; color: #ffffff;font-size: 13px;padding:0px 20px;'>");
            sb.Append("                                <tr>");
            sb.Append("                                    <td style='padding: 0 6px; border: 1px solid #ffffff;'>Sr.No.</td>");
            sb.Append("                                    <td style='padding: 0 6px; border: 1px solid #ffffff;'>Name</td>");

            if (fsr.child + fsr.infants > 0)
            {
                sb.Append("                                    <td style='padding: 0 6px; border: 1px solid #ffffff;'>Date Of Birth</td>");
            }
            sb.Append("                                    <td style='padding: 0 6px; border: 1px solid #ffffff;'>PNR</td>");
            if (fsr.flightResult[0].IsLCC == false)
            {
                sb.Append("                                 <td style='padding: 0 6px; border: 1px solid #ffffff;'>Ticket Number</td>");
            }
            if (!string.IsNullOrEmpty(fsr.passengerDetails[0].ticketNo))
            {
                sb.Append("                                 <td style='padding: 0 6px; border: 1px solid #ffffff;'>Ticket Number</td>");
            }
            sb.Append("                                 <td style='padding: 0 6px; border: 1px solid #ffffff;'>Status</td>");
            sb.Append("                                </tr>");
            int paxCtr = 1;
            foreach (var pax in fsr.passengerDetails)
            {
                sb.Append("                                <tr>");
                sb.Append("                                    <td style='padding: 0 6px; border: 1px solid #ffffff;'>" + (paxCtr++).ToString() + "</td>");
                sb.Append("                                    <td style='padding: 0 6px; border: 1px solid #ffffff;'>" + pax.title + " " + pax.firstName + " " + pax.lastName + "</td>");
                if (fsr.child + fsr.infants > 0)
                {
                    sb.Append("                                    <td style='padding: 0 6px; border: 1px solid #ffffff;'>" + (pax.passengerType == PassengerType.Adult ? "" : pax.dateOfBirth.ToString("dd MMM yy")) + "</td>");
                }
                sb.Append("                                 <td style='padding: 0 6px; border: 1px solid #ffffff;'>" + (string.IsNullOrEmpty(fsr.PNR) ? "Pending" : (fsr.PNR + (string.IsNullOrEmpty(fsr.ReturnPNR) ? "" : ("/" + fsr.ReturnPNR)))) + "</td>");
                if (fsr.flightResult[0].IsLCC == false)
                {
                    sb.Append("                                 <td style='padding: 0 6px; border: 1px solid #ffffff;'>Pending</td>");
                }
                if (!string.IsNullOrEmpty(pax.ticketNo))
                {
                    sb.Append("                                 <td style='padding: 0 6px; border: 1px solid #ffffff;'>" + pax.ticketNo + "</td>");
                }
                sb.Append("                                 <td style='padding: 0 6px; border: 1px solid #ffffff;'>" + fsr.bookingStatus.ToString() + "</td>");
                sb.Append("                                </tr>");
            }

            sb.Append("                            </table>");
            sb.Append("                        </td>");
            sb.Append("                    </tr>");
            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='padding:10px 20px 0 20px;'>");
            sb.Append("                            <h2 style='background-color: #fa7760; color: #ffffff; font-size: 18px;text-align: center; padding: 0; margin: 0;'>Travellers Charges Details</h2>");
            sb.Append("                        </td>");
            sb.Append("                    </tr>");
            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='padding: 0 20px;'>");
            sb.Append("                            <table cellspacing='0' cellpadding='0' style='width: 100%; '>");
            sb.Append("                                <tr>");
            sb.Append("                                    <td style='font: normal 14px Arial, Helvetica, sans-serif;padding:5px 5px 5px 10px;line-height:20px;border: 1px solid #ffffff;color: #ffffff;'>Basic Fare</td>");
            sb.Append("                                    <td style='font :normal 14px Arial, Helvetica, sans-serif;padding:5px 5px 5px 10px;line-height:20px;border: 1px solid #ffffff;color: #ffffff;'>₹" + (fsr.sumFare.BaseFare).ToString("f2") + "</td>");
            sb.Append("                                </tr>");
            sb.Append("                                <tr>");
            sb.Append("                                    <td style='font: normal 14px Arial, Helvetica, sans-serif;padding:5px 5px 5px 10px;line-height:20px;border: 1px solid #ffffff;color: #ffffff;'>Taxes</td>");
            sb.Append("                                    <td style='font: normal 14px Arial, Helvetica, sans-serif;padding:5px 5px 5px 10px;line-height:20px;border: 1px solid #ffffff;color:#ffffff;'>₹" + ((fsr.sumFare.Tax + fsr.sumFare.Markup + fsr.fareIncreaseAmount) - fsr.sumFare.CommissionEarned).ToString("f2") + "</td>");
            sb.Append("                                </tr>");
            decimal CouponAmount = 0;
            if (fsr.CouponAmount > 0 && !string.IsNullOrEmpty(fsr.CouponCode))
            {
                CouponAmount = fsr.CouponAmount;
                sb.Append("                                <tr>");
                sb.Append("                                    <td style='font: normal 14px Arial, Helvetica, sans-serif;padding:5px 5px 5px 10px;line-height:20px;border: 1px solid #ffffff;color: #ffffff;'>Coupon Discount</td>");
                sb.Append("                                    <td style='font: normal 14px Arial, Helvetica, sans-serif;padding:5px 5px 5px 10px;line-height:20px;border: 1px solid #ffffff;color:#ffffff;'>₹" + CouponAmount.ToString("f2") + "</td>");
                sb.Append("                                </tr>");
            }


            decimal CFee = 0;
            if (fsr.conveniencefee > 0)
            {
                CFee = fsr.conveniencefee;
                sb.Append("                                <tr>");
                sb.Append("                                    <td style='font: normal 14px Arial, Helvetica, sans-serif;padding:5px 5px 5px 10px;line-height:20px;border: 1px solid #ffffff;color: #ffffff;'>Convenience Fee</td>");
                sb.Append("                                    <td style='font: normal 14px Arial, Helvetica, sans-serif;padding:5px 5px 5px 10px;line-height:20px;border: 1px solid #ffffff;color:#ffffff;'>₹" + CFee.ToString("f2") + "</td>");
                sb.Append("                                </tr>");
            }



            sb.Append("                                <tr>");
            sb.Append("                                    <td style='font: bold 14px Arial, Helvetica, sans-serif;padding:5px 5px 5px 10px;line-height:20px;border: 1px solid #ffffff;color: #ffffff;'>Total</td>");
            sb.Append("                                    <td style='font: bold 14px Arial, Helvetica, sans-serif;padding:5px 5px 5px 10px;line-height:20px;border: 1px solid #ffffff;color: #ffffff;'>₹" + ((fsr.sumFare.grandTotal + fsr.fareIncreaseAmount + fsr.conveniencefee) - CouponAmount).ToString("f2") + "</td>");
            sb.Append("                                </tr>");
            sb.Append("                            </table>");
            sb.Append("                        </td>");
            sb.Append("                    </tr>");
            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='padding:15px 20px 0 20px;'>");
            sb.Append("                            <table style='width: 100%; border: 1px solid #ffffff;'>");
            sb.Append("                                <tr>");
            sb.Append("                                    <td colspan='2' style='background: #fa7760; text-align: center;'>");
            sb.Append("                                        <h2 style='font-size:18px; color:#ffffff; padding: 0;margin: 0;'>Terms & Conditions</h2>");
            sb.Append("                                    </td>");
            sb.Append("                                </tr>");
            if (fsr.FareTypeList.Contains(FareType.INSTANTPUR) || fsr.FareTypeList.Contains(FareType.INST_SERIESPUR) || fsr.FareTypeList.Contains(FareType.OFFER_FARE_WITHOUT_PNR) || fsr.FareTypeList.Contains(FareType.OFFER_FARE_WITH_PNR))
            {
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  The Fare Selected by You is a Special Category Group Promotional Fare.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  This Fare, Once Booked, CAN NOT be canceled or changed. This is a highly restricted fare, and any request to cancel or change the ticket can not be entertained.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  This is a 100% Confirmed Ticket; Your name will reflect on Airline Website only 12-24 hrs before departure time.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  Web Check-in for this ticket Can be done a day before Journey Date after 7 Pm.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  If there is Any Change in Flight Timing, we will notify you on your registered mobile/email ID.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  You must ensure to reach Boarding Gate at least 30 minutes before Departure Time.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  Please reconfirm the Terminal Information with the boarding pass.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
            }
            else
            {
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  All passengers, including children and infants, have to present their valid ID proof at the time of check in. </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  We recommend you check in at least 3 hours prior to departure of your domestic flight and 4 hours prior to your international flight. </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  Carriage and other facilities provided by the carrier are subject to their Terms and Condition. We are not liable for missing any facility of the carrier. </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  To cancel tickets in less than 6 hours prior to departure, please contact the airlines directly. We are not at all responsible for any losses on receiving the request in such cases. </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;   Recheck your baggage with your respective airline before travelling for hassle free travel experience.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  Due to the security, reasons and Government regulations, passengers flying on destination like Jammu and Srinagar are not allow to carry any Hand Baggage. </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&nbsp;&nbsp;Free baggage allowance : 15KG checked baggage and 7 KG cabin baggage OutBound, </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&nbsp;&nbsp;15KG checked baggage and cabin baggage InBound</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&nbsp;&nbsp;Baggage more than specified units is subject to a charge to be paid at the airport during check in. (Baggage allowance differ for infants) </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  Baggage more than specified units is subject to a charge to be paid at the airport during check in. (Baggage allowance differ for infants) </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  Partial cancellation is not allow for Round trip fares.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  Partial cancellation is not allow for tickets booked under Friends &amp; Family fares. </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  The No Show refund* should be collected within 90 days from date of departure. Group Booking Rules will be applicable if passengers are 9 or more in numbers. Company is not responsible for any delay or cancellation of flights from airline's end. </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  Kindly contact the airline at least 24 Hours before to reconfirm your flight details giving reference of Airline PNR Number.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  All reservations done through our website are as per the terms and conditions of the concerned airlines. Any modification, cancellation and refund of the airline tickets shall be strictly as per the policy of the concerned airlines and we deny all liability in connection thereof.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  Unaccompanied Child: Children below the age of 12 will not be accepted for carriage unless they are accompanied by a person of at least 18 years of age. Such child/children must be seated next to the accompanying adult. The accompanying adult is solely responsible for the well being of the child/children traveling together with him/her. This also includes ensuring that seats are booked to ensure child/children and an accompanying adult are seated together. </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  To review your baggage allowance, please contact Airline well before your flight. </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  All time indicated are the local times (in 24 hrs. format) at the relevant airport. </p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
                sb.Append("                                <tr><td colspan='2' style='padding:5px 10px;'><p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  Cancellation and date change fees are applicable before departure and per passenger basis. In case of amendment, along with the airline and FMB fees, you will also have to pay fare difference, if any </p>");
                sb.Append("                                <tr>");
                sb.Append("                                    <td colspan='2' style='padding:5px 10px;'>");
                sb.Append("                                        <p style='padding: 0; margin: 0; color: #ffffff; font-size: 13px; line-height: 20px;'>&bull;  Some airports have multiple terminals catering to domestic/international flights, please check the departure/arrival terminal of your flight with the airlines before reaching airport.</p>");
                sb.Append("                                    </td>");
                sb.Append("                                </tr>");
            }
            sb.Append("                            </table>");
            sb.Append("                        </td>");
            sb.Append("                    </tr>");
            sb.Append("                    <tr>");
            sb.Append("                        <td colspan='2' style='padding:10px; text-align:center; font-size:15px; font-weight: bold; color: #ffffff;line-height: 20px;'>For any further assistance, please contact us at <a href='tel:" + GlobalData.Phone + "' style='color:#fa7760' target='_blank'>" + GlobalData.Phone + "</a> or you can also reach us at<a mailto:'" + GlobalData.Email + "' style='color:#fa7760' target='_blank'> " + GlobalData.Email + "</a></td>");
            sb.Append("                    </tr>");
            sb.Append("                </table>");
            sb.Append("            </td>");
            sb.Append("        </tr>");
            sb.Append("    </table>");
            sb.Append("</div>");

            return sb.ToString();
        }
        [HttpPost]
        public ActionResult webhookCallBack(string ID)
        {
            StringBuilder sbLogger = new StringBuilder();
            try
            {
                Stream req = Request.InputStream;
                req.Seek(0, System.IO.SeekOrigin.Begin);
                string jsonData = new StreamReader(req).ReadToEnd();
                sbLogger.Append("json:-" + jsonData + Environment.NewLine);
                Core.PayU.WebhookSuccessDetails objWHD = JsonConvert.DeserializeObject<Core.PayU.WebhookSuccessDetails>(jsonData);
                new DAL.PayU.DalWebHook().SavePayU_WebhooksDetails("Success", objWHD);
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error16_" + DateTime.Today.ToString("ddMMyy"), "Error");
                sbLogger.Append(ex.ToString() + Environment.NewLine);
            }
            LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\WebHook", "WebHook" + DateTime.Today.ToString("ddMMMyy"), "WebHook.txt");
            return View();
        }
        [HttpPost]
        public ActionResult webhookCallBack_tj(string ID)
        {
            StringBuilder sbLogger = new StringBuilder();
            try
            {
                var bodyStream = new StreamReader(Request.InputStream);
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                var bodyText = bodyStream.ReadToEnd();
                sbLogger.Append("json:-" + bodyText.ToString() + Environment.NewLine);
                Core.RP.Webhook.WebhookSuccess objWHD = JsonConvert.DeserializeObject<Core.RP.Webhook.WebhookSuccess>(bodyText);
                new DAL.PayU.RazorpayWebhook().SaveRazorPay_WebhooksDetails("captured", objWHD);
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error17_" + DateTime.Today.ToString("ddMMyy") + "_" + ID.ToString(), "Error");
                sbLogger.Append(ex.ToString() + Environment.NewLine);
            }
            try
            {
                LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\WebHook", "WebHook" + DateTime.Today.ToString("ddMMMyy"), "WebHook.txt");
            }
            catch { }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        //[HttpPost]
        //public ActionResult webhookCallBack_tj1(string ID)
        //{
        //    StringBuilder sbLogger = new StringBuilder();
        //    try
        //    {
        //        var RazorpaySignature = Request.Headers["X-Razorpay-Signature"];
        //        //var RazorpaySignature = Request.Headers["X-Razorpay-Signature"];
        //        sbLogger.Append("RazorpaySignature:-" + RazorpaySignature + Environment.NewLine);
        //        sbLogger.Append("RazorpayHeader:-" + Request.Headers.ToString() + Environment.NewLine);
        //        sbLogger.Append("Request1:-" + Request.ToString() + Environment.NewLine);
        //        //sbLogger.Append("Request2:-" + JsonConvert.SerializeObject(Request) + Environment.NewLine);
        //        string documentContents;
        //        //To Capture RAW JSON posted
        //        //using (Stream receiveStream = Request.InputStream)
        //        //{
        //        //    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
        //        //    {
        //        //        documentContents = readStream.ReadToEnd();
        //        //    }
        //        //}
        //        var bodyStream = new StreamReader(Request.InputStream);
        //        bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
        //        var bodyText = bodyStream.ReadToEnd();
        //        sbLogger.Append("json:-" + bodyText.ToString() + Environment.NewLine);
        //        //sbLogger.Append("Request:-" + JsonConvert.SerializeObject(Request)+Environment.NewLine);
        //        //Stream req = Request.InputStream;

        //        //req.Seek(0, System.IO.SeekOrigin.Begin);
        //        //string jsonData = new StreamReader(req).ReadToEnd();
        //        //sbLogger.Append("json:-" + jsonData + Environment.NewLine);
        //        //Core.PayU.WebhookSuccessDetails objWHD = JsonConvert.DeserializeObject<Core.PayU.WebhookSuccessDetails>(jsonData);
        //        //new DAL.PayU.DalWebHook().SavePayU_WebhooksDetails("Success", objWHD);
        //    }
        //    catch (Exception ex)
        //    {
        //        sbLogger.Append(ex.ToString() + Environment.NewLine);
        //    }
        //    LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\WebHook", "WebHook" + DateTime.Today.ToString("ddMMMyy"), "WebHook.txt");
        //    return View();
        //}
        public JsonResult CheckCoupon(string ID, string CouponCode)
        {
            Core.CouponStatusResponse objResponse = new CouponStatusResponse();
            try
            {
                //AirContext airContext = FlightOperation.GetAirContext(ID);
                //if (airContext != null)
                //{
                //    if (CouponCode.Equals("INSTAMOJO") || CouponCode.Equals("INSTAMOJOMob"))
                //    {
                //        if (airContext.flightBookingRequest.flightResult.Where(k => k.isPreCuponAvailable).ToList().Count > 0)
                //        {
                //            airContext.flightBookingRequest.CouponAmount = 0;
                //            airContext.flightBookingRequest.CouponCode = CouponCode;
                //            foreach (var item in airContext.flightBookingRequest.flightResult.Where(k => k.isPreCuponAvailable).ToList())
                //            {
                //                //      airContext.flightBookingRequest.CouponAmount += airContext.flightBookingRequest.CouponIncreaseAmount <= 0 ? 300m : airContext.flightBookingRequest.CouponIncreaseAmount;
                //            }
                //            objResponse.responseStatus.status = TransactionStatus.Success;
                //            objResponse.CouponAmount = airContext.flightBookingRequest.CouponAmount;
                //        }
                //        else
                //        {
                //            objResponse.responseStatus.status = TransactionStatus.Error;
                //            objResponse.responseStatus.message = "Invalide coupon, Please try another coupon.";
                //        }
                //    }



                //    else
                //    {
                //        airContext.flightBookingRequest.CouponAmount = 0;
                //        //airContext.flightBookingRequest.sumFare.grandTotal = airContext.flightBookingRequest.sumFare.grandTotal+ airContext.flightBookingRequest.CouponIncreaseAmount;
                //        objResponse.responseStatus.status = TransactionStatus.Error;
                //        objResponse.responseStatus.message = "Invalide coupon, Please try another coupon.";
                //    }
                //}
                //else
                //{
                //    airContext.flightBookingRequest.CouponAmount = 0;
                //    objResponse.responseStatus.status = TransactionStatus.Error;
                //    objResponse.responseStatus.message = "Invalide coupon, Please try another coupon.";
                //}



                AirContext airContext = FlightOperation.GetAirContext(ID);
                if (airContext != null)
                {
                    Core.CouponStatusRequest csr = new Core.CouponStatusRequest
                    {
                        CabinClass = airContext.flightSearchRequest.cabinType,
                        clientType = Core.ClientType.Web,
                        CouponCode = CouponCode,
                        //MCOAmount = (airContext.flightBookingRequest.flightResult.fare.adultMarkup * airContext.flightBookingRequest.adults) +
                        //    (airContext.flightBookingRequest.flightResult.fare.childMarkup * airContext.flightBookingRequest.child) +
                        //    (airContext.flightBookingRequest.flightResult.fare.infantMarkup * airContext.flightBookingRequest.infants) +
                        //    (airContext.flightBookingRequest.flightResult.fare.infantWsMarkup * airContext.flightBookingRequest.infantsWs),
                        TotalUnit = (airContext.flightBookingRequest.adults + airContext.flightBookingRequest.child + airContext.flightBookingRequest.infants + airContext.flightBookingRequest.infantsWs),

                        SourceMedia = airContext.flightBookingRequest.sourceMedia,
                        TotalAmount = airContext.flightBookingRequest.sumFare.grandTotal,
                        TravelDate = airContext.flightBookingRequest.flightResult[0].FlightSegments[0].Segments[0].DepTime
                    };
                    new Bal.BalCoupon().ValidateCoupon(csr, ref objResponse);
                    if (objResponse.responseStatus.status == TransactionStatus.Success)
                    {
                        airContext.flightBookingRequest.CouponAmount = objResponse.CouponAmount;
                        airContext.flightBookingRequest.CouponCode = CouponCode;
                    }
                    //    new Bal.FlightDetails().SaveFlightSessionData(airContext.flightSearchRequest.userSearchID, airContext);
                }
                else
                {
                    objResponse.responseStatus.status = TransactionStatus.Error;
                    objResponse.responseStatus.message = "Invalide coupon, Please try another coupon.";
                }


            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error18_" + DateTime.Today.ToString("ddMMyy"), "Error");
                objResponse.responseStatus.status = TransactionStatus.Error;
                objResponse.responseStatus.message = "Invalide coupon, Please try another coupon.";
            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckCouponNew(string ID, string CouponCode)
        {
            Core.CouponStatusResponse objResponse = new CouponStatusResponse();
            objResponse.responseStatus.status = TransactionStatus.Error;
            objResponse.responseStatus.message = "Invalid Coupon!!";
            try
            {

                AirContext airContext = FlightOperation.GetAirContext(ID);
                if (airContext != null)
                {
                    CouponDetail cd = new DAL.dalCoupon().GetCouponData(CouponCode);
                    if (cd != null && cd.isActive)
                    {
                        ResponseStatus responseStatus = new ResponseStatus();
                        if (cd.isValidateSourceMedia)
                        {
                            if (!cd.sourceMedia.Equals(airContext.flightBookingRequest.sourceMedia, StringComparison.OrdinalIgnoreCase))
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Invalid Coupon!!";
                            }
                        }
                        if (responseStatus.status == TransactionStatus.Success && cd.isValidateByEmail)
                        {
                            if (!cd.emailID.Equals(airContext.flightBookingRequest.emailID, StringComparison.OrdinalIgnoreCase))
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Invalid Coupon!!";
                            }
                        }
                        if (responseStatus.status == TransactionStatus.Success && cd.isValidateByCount)
                        {
                            if (cd.noOfCoupon <= cd.totalConsume)
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Invalid Coupon!!";
                            }
                        }
                        if (responseStatus.status == TransactionStatus.Success && cd.isValidateByNoOfPax)
                        {
                            if (cd.noOfPax > (airContext.flightBookingRequest.adults + airContext.flightBookingRequest.child + airContext.flightBookingRequest.infants))
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Coupon is valid for " + cd.noOfPax + " or more passengers!!";
                            }
                        }
                        if (responseStatus.status == TransactionStatus.Success && cd.isValidateByTotalAmt)
                        {
                            if (airContext.flightBookingRequest.sumFare.grandTotal < cd.minAmount)
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Coupon is valid for minimum booking Amount" + cd.minAmount + "!!";
                            }
                        }
                        if (responseStatus.status == TransactionStatus.Success && cd.isValidateByCabinClass)
                        {
                            if (airContext.flightSearchRequest.cabinType != cd.cabinClass)
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Invalid Coupon!!";
                            }
                        }
                        if (responseStatus.status == TransactionStatus.Success && cd.isValidateByAirline)
                        {
                            if (!airContext.flightBookingRequest.flightResult[0].FlightSegments[0].Segments[0].Airline.Equals(cd.airline))
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Invalid Coupon!!";
                            }
                        }
                        if (responseStatus.status == TransactionStatus.Success && cd.isValidateByBookingDate)
                        {
                            if (cd.bookingDateFrom < DateTime.Today || cd.bookingDateTo > DateTime.Today)
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Invalid Coupon!!";
                            }
                        }
                        if (responseStatus.status == TransactionStatus.Success && cd.isValidateByTravelDate)
                        {
                            if (cd.travelDateFrom < airContext.flightBookingRequest.flightResult[0].FlightSegments[0].Segments[0].DepTime || cd.travelDateTo > airContext.flightBookingRequest.flightResult[0].FlightSegments[0].Segments[0].DepTime)
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Invalid Coupon!!";
                            }
                        }
                        if (responseStatus.status == TransactionStatus.Success && cd.isValidationByUTFCampaign)
                        {
                            if (!cd.UTFCampaign.Equals(airContext.flightBookingRequest.utm_campaign, StringComparison.OrdinalIgnoreCase))
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Invalid Coupon!!";
                            }
                        }

                        if (responseStatus.status == TransactionStatus.Success && cd.isValidTravelType)
                        {
                            if (airContext.flightSearchRequest.travelType != cd.TravelType)
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Invalid Coupon!!";
                            }
                        }

                        if (responseStatus.status == TransactionStatus.Success && cd.isValidTripType)
                        {
                            if (airContext.flightSearchRequest.tripType != cd.TripType)
                            {
                                responseStatus.status = TransactionStatus.Error;
                                responseStatus.message = "Invalid Coupon!!";
                            }
                        }

                        objResponse.responseStatus = responseStatus;
                    }

                    if (objResponse.responseStatus.status == TransactionStatus.Success)
                    {
                        if (cd.isLessConvenceFee)
                        {
                            objResponse.isLessConvenceFee = true;
                        }
                        if (cd.amountType == CouponAmountType.Flat)
                        {
                            objResponse.CouponAmount = cd.couponAmount;
                        }
                        else
                        {
                            objResponse.CouponAmount = airContext.flightBookingRequest.sumFare.grandTotal * (cd.couponAmount / 100);
                        }
                        airContext.flightBookingRequest.CouponAmount = objResponse.CouponAmount;
                        airContext.flightBookingRequest.CouponCode = CouponCode;

                    }
                }
                else
                {
                    objResponse.responseStatus.status = TransactionStatus.Error;
                    objResponse.responseStatus.message = "Invalide coupon, Please try another coupon.";
                    airContext.flightBookingRequest.responseStatus.message = objResponse.responseStatus.message;
                }


            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error18_" + DateTime.Today.ToString("ddMMyy"), "Error");
                objResponse.responseStatus.status = TransactionStatus.Error;
                objResponse.responseStatus.message = "Invalide coupon, Please try another coupon.";
                //airContext.flightBookingRequest.responseStatus.message = objResponse.responseStatus.message;
            }
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
        public T Clone<T>(T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
        public void bookingLog(ref StringBuilder sbLogger, string requestTitle, string logText)
        {
            sbLogger.Append(Environment.NewLine + "---------------------------------------------" + requestTitle + "" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------------------------------");
            sbLogger.Append(Environment.NewLine + logText);
            sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
        }
        private void saveVerificationDetails(VerifyFareDetails vfd)
        {
            //string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            var save = System.Threading.Tasks.Task.Run(async () =>
            {
                await new DAL.Deal.UserSearchHistory().SaveSearchData(vfd);
            });
        }
        [HttpPost]
        public ActionResult SearchingFlight_se(FormCollection FormColl)
        {
            try
            {
                FlightSearchRequest fsr = null;
                fsr = new FlightSearchRequest();
                fsr.segment = new List<SearchSegment>();
                SearchSegment sseg = new SearchSegment();
                sseg.originAirport = GetAirportCode(FormColl["se_fromCity"]);
                sseg.orgArp = Core.FlightUtility.GetAirport(sseg.originAirport);
                sseg.destinationAirport = GetAirportCode(FormColl["se_toCity"]);
                sseg.destArp = Core.FlightUtility.GetAirport(sseg.destinationAirport);
                sseg.travelDate = DateTime.ParseExact(FormColl["se_departure_date"].ToString(), "yyyy-MM-dd", new CultureInfo("en-US"));//Convert.ToDateTime(FormColl["departDate"].ToString());
                fsr.segment.Add(sseg);

                if (FormColl["se_hfTripType"] != null)
                    fsr.tripType = (TripType)Convert.ToByte(FormColl["se_hfTripType"]);
                if (fsr.tripType == TripType.RoundTrip)
                {
                    SearchSegment sseg1 = new SearchSegment();
                    sseg1.originAirport = GetAirportCode(FormColl["se_toCity"]);
                    sseg1.orgArp = Core.FlightUtility.GetAirport(sseg1.originAirport);
                    sseg1.destinationAirport = GetAirportCode(FormColl["se_fromCity"]);
                    sseg1.destArp = Core.FlightUtility.GetAirport(sseg1.destinationAirport);
                    sseg1.travelDate = DateTime.ParseExact(FormColl["se_return_date"].ToString(), "yyyy-MM-dd", new CultureInfo("en-US"));//Convert.ToDateTime(FormColl["returnDate"].ToString());
                    fsr.segment.Add(sseg1);
                }

                if (FormColl["se_Cabin"] != null)
                    fsr.cabinType = (CabinType)Convert.ToByte(FormColl["se_Cabin"].ToString());
                fsr.airline = "ALL";

                if (sseg.orgArp.countryCode.ToUpper() == "IN" && sseg.destArp.countryCode.ToUpper() == "IN")
                    fsr.travelType = TravelType.Domestic;
                else
                    fsr.travelType = TravelType.International;

                fsr.adults = FormColl["se_Adult"] != null ? Convert.ToByte(FormColl["se_Adult"].ToString()) : (byte)0;
                fsr.child = FormColl["se_Child"] != null ? Convert.ToByte(FormColl["se_Child"].ToString()) : (byte)0;
                fsr.infants = FormColl["se_Infant"] != null ? Convert.ToByte(FormColl["se_Infant"].ToString()) : (byte)0;
                fsr.searchDirectFlight = false;
                fsr.flexibleSearch = false;
                fsr.isNearBy = false;
                fsr.currencyCode = "USD";
                fsr.siteId = SiteId.FlightsMojoIN;
                fsr.sourceMedia = GetCookie();
                fsr.userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                fsr.tgy_Request_id = DateTime.Now.ToString("ddMMyyyHHmmsss");
                #region set deep link
                fsr.deepLink = "/flight/getFlightResult?sec1=" + fsr.segment[0].originAirport + "|" + fsr.segment[0].destinationAirport + "|" + fsr.segment[0].travelDate.ToString("yyyy-MM-dd");
                for (int i = 1; i < 4; i++)
                {
                    if (i < fsr.segment.Count)
                    {
                        fsr.deepLink += "&sec" + (i + 1) + "=" + fsr.segment[i].originAirport + "|" + fsr.segment[i].destinationAirport + "|" + fsr.segment[i].travelDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        fsr.deepLink += "&sec" + (i + 1) + "=";
                    }
                }
                fsr.deepLink += "&adults=" + fsr.adults + "&child=" + fsr.child + "&infants=" + fsr.infants + "&cabin=" + ((int)fsr.cabinType).ToString() + "&airline=" + fsr.airline + "&siteid=" + ((int)fsr.siteId).ToString() + "&campain=" + fsr.sourceMedia + "&currency=" + fsr.currencyCode;
                #endregion

                AirContext airContext = new AirContext(fsr.userIP);
                airContext.flightSearchRequest = fsr;
                airContext.IsSearchCompleted = false;
                airContext.flightRef = new List<string>();

                fsr.userSessionID = Session.SessionID;
                fsr.userSearchID = getSearchID();
                FlightOperation.SetAirContext(airContext);
                stopwatch.Stop();
                return Redirect("/Flight/Result/" + fsr.userSearchID);
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "SearchingFlight_se_" + DateTime.Today.ToString("ddMMyy"), "Error");
                return Redirect("/");
            }
        }
        public async System.Threading.Tasks.Task deletefiles()
        {
            string pathFlightSearch = System.IO.Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "NewLog\\Search\\");
            string[] files = System.IO.Directory.GetFiles(pathFlightSearch);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.CreationTime < DateTime.Now.AddMinutes(-15))
                    fi.Delete();
            }
        }
        private void CallDeleteFiles()
        {
            var save = System.Threading.Tasks.Task.Run(async () =>
            {
                await deletefiles();
            });
        }
        private void sendInCompletewhatsapp(FlightBookingRequest fsr)
        {
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            //var url = "https://api.imiconnect.in/resources/v1/messaging";
            //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            //httpRequest.Method = "POST";
            //httpRequest.ContentType = "application/json";
            //httpRequest.Headers["key"] = "30127004-37da-11ed-baaa-02e28ff40276";
            //string output = string.Empty;

            //string urllink = "https://www.flightsmojo.in" + fsr.deepLink;
            //Core.Whatsapp.WA whatsapp = new Core.Whatsapp.WA();
            //whatsapp.appid = "a_167149593199252900";
            //whatsapp.deliverychannel = "whatsapp";
            //whatsapp.message = new Core.Whatsapp.Message();

            //whatsapp.message.template = "6878282908957621";
            //whatsapp.message.parameters = new Core.Whatsapp.Parameters();
            //whatsapp.message.parameters.variable1 = "Brij";
            //whatsapp.message.parameters.variable2 = fsr.bookingID.ToString();
            //whatsapp.destination = new List<Core.Whatsapp.Destination>();
            //Core.Whatsapp.Destination ds = new Core.Whatsapp.Destination();
            //whatsapp.destination.Add(ds);
            //ds.waid = new List<string>();
            //ds.waid.Add(fsr.phoneNo);
            //output = JsonConvert.SerializeObject(whatsapp);

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(output);
            //}
            //var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //}
            //var statuscode = httpResponse.StatusCode;
        }

        public Device GetDevice()
        {
            string u = System.Web.HttpContext.Current.Request.Headers["User-Agent"];
            System.Text.RegularExpressions.Regex b = new System.Text.RegularExpressions.Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
            System.Text.RegularExpressions.Regex v = new System.Text.RegularExpressions.Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                return Device.Mobile;
            }
            return Device.Desktop;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        #region AirtelWhatsApp

        public static string BasicAuth = "";
        private void sendwhatsapp(FlightBookingResponse fsr)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var url = ConfigurationManager.AppSettings["UrlWhatsapp"];
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers["Authorization"] = ConfigurationManager.AppSettings["WhatsappAuth"]; ;
            string output = string.Empty;

            if (!string.IsNullOrEmpty(fsr.ReturnPNR))
            {
                Core.Whatsapp.AirtelWhatsapp whatsapp = new Core.Whatsapp.AirtelWhatsapp()
                {
                    templateId = ConfigurationManager.AppSettings["TempIdRoundTrip"],
                    from = ConfigurationManager.AppSettings["FromNo"],
                    to = fsr.phoneNo,
                    message = new Core.Whatsapp.Message()
                    {
                        variables = new List<string>()
                    },
                    mediaAttachment = new Core.Whatsapp.MediaAttachment() { type = "DOCUMENT", url = GlobalData.URL + "/Uploadedpdf/" + fsr.bookingID + ".pdf", fileName = "e-Ticket" }
                };
                whatsapp.message.variables.Add(fsr.passengerDetails.FirstOrDefault().firstName);
                whatsapp.message.variables.Add(fsr.bookingID.ToString());
                whatsapp.message.variables.Add(fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination);
                whatsapp.message.variables.Add(fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination);
                whatsapp.message.variables.Add(fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("dd MMM yy"));
                whatsapp.message.variables.Add(fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("hh:mm"));
                whatsapp.message.variables.Add(fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().Airline + "-" + fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().FlightNumber);
                whatsapp.message.variables.Add(fsr.PNR);
                whatsapp.message.variables.Add(fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination);
                whatsapp.message.variables.Add(fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination);
                whatsapp.message.variables.Add(fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("dd MMM yy"));
                whatsapp.message.variables.Add(fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("hh:mm"));
                whatsapp.message.variables.Add(fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().Airline + "-" + fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().FlightNumber);
                whatsapp.message.variables.Add(fsr.ReturnPNR);
                output = JsonConvert.SerializeObject(whatsapp);
            }
            else if (!string.IsNullOrEmpty(fsr.PNR))
            {
                Core.Whatsapp.AirtelWhatsapp whatsapp = new Core.Whatsapp.AirtelWhatsapp()
                {
                    templateId = ConfigurationManager.AppSettings["TempIdOneWay"],
                    from = ConfigurationManager.AppSettings["FromNo"],
                    to = fsr.phoneNo,
                    message = new Core.Whatsapp.Message()
                    {
                        variables = new List<string>()
                    },
                    mediaAttachment = new Core.Whatsapp.MediaAttachment() { type = "DOCUMENT", url = GlobalData.URL + "/Uploadedpdf/" + fsr.bookingID + ".pdf", fileName = "e-Ticket" }
                };
                whatsapp.message.variables.Add(fsr.passengerDetails.FirstOrDefault().firstName);
                whatsapp.message.variables.Add(fsr.bookingID.ToString());
                whatsapp.message.variables.Add(fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().Origin);
                whatsapp.message.variables.Add(fsr.flightResult.LastOrDefault().FlightSegments.FirstOrDefault().Segments.LastOrDefault().Destination);
                whatsapp.message.variables.Add(fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("dd MMM yy"));
                whatsapp.message.variables.Add(fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().DepTime.ToString("hh:mm"));
                whatsapp.message.variables.Add(fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().Airline + "-" + fsr.flightResult.FirstOrDefault().FlightSegments.FirstOrDefault().Segments.FirstOrDefault().FlightNumber);
                whatsapp.message.variables.Add(fsr.PNR);
                output = JsonConvert.SerializeObject(whatsapp);
            }
            else if (fsr.bookingStatus == Core.BookingStatus.Failed)
            {
                Core.Whatsapp.AirtelWhatsapp whatsapp = new Core.Whatsapp.AirtelWhatsapp()
                {
                    templateId = ConfigurationManager.AppSettings["TempIdFailed"],
                    from = ConfigurationManager.AppSettings["FromNo"],
                    to = fsr.phoneNo,
                    message = new Core.Whatsapp.Message()
                    {
                        variables = new List<string>()
                    }
                };
                whatsapp.message.variables.Add(fsr.passengerDetails.FirstOrDefault().firstName);
                whatsapp.message.variables.Add(fsr.bookingID.ToString());
                output = JsonConvert.SerializeObject(whatsapp);
            }
            else if (fsr.bookingStatus == Core.BookingStatus.InProgress || fsr.bookingStatus == Core.BookingStatus.NONE)
            {
                Core.Whatsapp.AirtelWhatsapp whatsapp = new Core.Whatsapp.AirtelWhatsapp()
                {
                    templateId = ConfigurationManager.AppSettings["TempIdInProgress"],
                    from = ConfigurationManager.AppSettings["FromNo"],
                    to = fsr.phoneNo,
                    message = new Core.Whatsapp.Message()
                    {
                        variables = new List<string>()
                    }
                };
                whatsapp.message.variables.Add(fsr.passengerDetails.FirstOrDefault().firstName);
                whatsapp.message.variables.Add(fsr.bookingID.ToString());
                output = JsonConvert.SerializeObject(whatsapp);
            }
            else if (fsr.bookingStatus == Core.BookingStatus.NONE)
            {
                Core.Whatsapp.AirtelWhatsapp whatsapp = new Core.Whatsapp.AirtelWhatsapp()
                {
                    templateId = ConfigurationManager.AppSettings["TempIdInProgress"],
                    from = ConfigurationManager.AppSettings["FromNo"],
                    to = fsr.phoneNo,
                    message = new Core.Whatsapp.Message()
                    {
                        variables = new List<string>()
                    }
                };
                whatsapp.message.variables.Add(fsr.passengerDetails.FirstOrDefault().firstName);
                whatsapp.message.variables.Add(fsr.bookingID.ToString());
                output = JsonConvert.SerializeObject(whatsapp);
            }
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(output);
            }
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
            var statuscode = httpResponse.StatusCode;
            CreateLogFile(output, "Log\\whatsapp", fsr.bookingID.ToString() + ".txt");
        }
        public void groupbookingmsg(FlightBookingResponse fsr)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var url = ConfigurationManager.AppSettings["UrlWhatsapp"];

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers["Authorization"] = ConfigurationManager.AppSettings["WhatsappAuth"];
            string output = string.Empty;
            var result = "";

            Core.Whatsapp.AirtelWhatsapp whatsapp = new Core.Whatsapp.AirtelWhatsapp()
            {
                templateId = ConfigurationManager.AppSettings["TempIdGroupBooking"],
                from = ConfigurationManager.AppSettings["FromNo"],
                to = fsr.phoneNo//fsr
                                //    to = fsr
            };

            output = JsonConvert.SerializeObject(whatsapp);
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(output);
            }
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            var statuscode = httpResponse.StatusCode;
            CreateLogFile(output, "Log\\whatsapp", "GB" + fsr.bookingID.ToString() + ".txt");
        }
        public void GetBasicAuth()
        {
            //var strResponse = GetTokenResponse((ConfigurationManager.AppSettings["Url"]), new Core.Whatsapp.LoginDetails);
            //Ease2FlyClass.TokenResponse res = Newtonsoft.Json.JsonConvert.DeserializeObject<Ease2FlyClass.TokenResponse>(strResponse);
            //AuthToken = res.result.token;
        }
        private string GetTokenResponse(string url, string requestData)
        {
            string response = string.Empty;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                byte[] data = Encoding.UTF8.GetBytes(requestData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
                WebResponse webResponse = request.GetResponse();
                var rsp = webResponse.GetResponseStream();
                using (StreamReader reader = new StreamReader(rsp))
                {
                    response = reader.ReadToEnd();
                }
                return response;
            }
            catch (WebException webEx)
            {
                if (webEx != null && webEx.Response != null)
                {
                    //new ServicesHub.LogWriter_New(webEx.ToString(), "E2F GetTokenResponse" + DateTime.Today.ToString("ddMMyy"), "Exeption");
                    if (webEx.Message.Contains("timed out") == false && webEx.Response != null)
                    {
                        WebResponse errResp = webEx.Response;
                        Stream responseStream = null;
                        if (errResp.Headers.Get("Content-Encoding") == "gzip")
                        {
                            responseStream = new System.IO.Compression.GZipStream(errResp.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                        }
                        else
                        {
                            responseStream = errResp.GetResponseStream();
                        }
                        StreamReader reader = new StreamReader(responseStream);
                        response = reader.ReadToEnd();
                    }
                }
            }
            return response;
        }
        #endregion


        public ActionResult Coupon(string ID)
        {
            ViewBag.CouponCode = ID;
            return View();
        }
    }
}