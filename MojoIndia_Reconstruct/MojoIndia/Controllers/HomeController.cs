using Core;
using Core.ContentPage;
using Dal;
//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
using MojoIndia.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MojoIndia.Controllers
{
    public class HomeController : Controller
    {
        public static string[] strAllAirportCityCode = new string[] { "AE", "AF", "AM", "AS", "AZ", "BD", "BH", "BN", "BT", "CN", "CY", "GE", "HK", "ID", "IL", "IN", "IQ", "IR", "JO", "JP", "KG", "KH", "KP", "KR", "KW", "KZ", "LA", "LB", "LK", "MH", "MM", "MN", "MO", "MV", "MY", "NP", "OM", "PH", "PK", "QA", "SA", "SG", "SY", "TH", "TJ", "TM", "TW", "UZ", "VN", "YE" };
        public ActionResult Index(string id)
        {
            if (Request.QueryString["utm_source"] != null)
            {
                setCookie(Request.QueryString.Get("utm_source"));
            }
            //String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes("flight_mojo" + ":" + "I2a3WxM&[2W0"));

            Dal.DALOriginDestinationContent ODC = new DALOriginDestinationContent();
            //if (Request.QueryString["location"] == null   )
            //{
            //    if (GetIpTrackerCookie() == "")
            //    {
            //        IpDetails ipDtl = new DAL.IP_Details().GetIpDetails(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            //        if (ipDtl == null)
            //        {
            //            GetIpDetails(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            //        }
            //        if (ipDtl != null && !string.IsNullOrEmpty(ipDtl.country_code))
            //        {
            //            if (strAllAirportCityCode.Contains(ipDtl.country_code))
            //            {
            //                return View(new OriginDestinationContent());
            //            }
            //            else if (ipDtl.country_code == "CA")
            //            {
            //                return Redirect("https://www.flightsmojo.ca");
            //            }
            //            else
            //            {
            //                return Redirect("https://www.flightsmojo.com");
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (GetIpTrackerCookie() == "india")
            //        {
            //            return View(new OriginDestinationContent());
            //        }
            //        else if (GetIpTrackerCookie() == "canada")
            //        {
            //            return Redirect("https://www.flightsmojo.ca");
            //        }
            //        else if (GetIpTrackerCookie() == "usa")
            //        {
            //            return Redirect("https://www.flightsmojo.com");
            //        }
            //        else
            //        {
            //            return Redirect("https://www.flightsmojo.com");
            //        }
            //    }
            //}
            //else
            //{
            //    setIpTrackerCookie(Request.QueryString["location"].ToString());
            //}
            return View(new OriginDestinationContent());
        }
        private void sendwa()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var url = System.Configuration.ConfigurationManager.AppSettings["UrlWhatsapp"];
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers["Authorization"] = System.Configuration.ConfigurationManager.AppSettings["WhatsappAuth"]; ;
            string output = string.Empty;

            Core.Whatsapp.AirtelWhatsapp whatsapp = new Core.Whatsapp.AirtelWhatsapp()
            {
                templateId = System.Configuration.ConfigurationManager.AppSettings["TempIdRoundTrip"],
                from = System.Configuration.ConfigurationManager.AppSettings["FromNo"],
                to = "917668677843",
                message = new Core.Whatsapp.Message()
                {
                    variables = new List<string>()
                },
                mediaAttachment = new Core.Whatsapp.MediaAttachment() { type = "DOCUMENT", url = GlobalData.URL + "/Uploadedpdf/525043.pdf", fileName = "525043" }
            };
            whatsapp.message.variables.Add("Brij");
            whatsapp.message.variables.Add("524771");
            whatsapp.message.variables.Add("DEL");
            whatsapp.message.variables.Add("BOM");
            whatsapp.message.variables.Add("12-Nov");
            whatsapp.message.variables.Add("09:30 AM");
            whatsapp.message.variables.Add("UK-159");
            whatsapp.message.variables.Add("ASD123");
            whatsapp.message.variables.Add("BOM");
            whatsapp.message.variables.Add("DEL");
            whatsapp.message.variables.Add("20-Nov");
            whatsapp.message.variables.Add("04:45 PM");
            whatsapp.message.variables.Add("6E-1597");
            whatsapp.message.variables.Add("WER73F");
            output = Newtonsoft.Json.JsonConvert.SerializeObject(whatsapp);
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
        }

        private ICredentials GetCredential()
        {
            string url = @"https://iqwhatsapp.airtel.in/gateway/airtel-xchange/basic/whatsapp-manager/v1/template/send";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(new System.Uri(url), "Basic_", new NetworkCredential("flight_mojo", "I2a3WxM&[2W0"));
            return credentialCache;
        }

        private void sendMSG()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            String result;
            String url = "https://api.textlocal.in/send/?apikey=NDQ3MDY0NmY0NDRiNzAzMTdhNTczNjUyNjQzNzYxNTA=&numbers=7668677843&message=Dear BRIJ,\n\nThank you for choosing FlightsMojo.in. You flight to LKO is confirmed and ticketed.\nPlease find the details below:\n\nFM Booking ID: 100000\nAirline PNR: ABC123\nTravel Date: 24-JAN\n\nIn case of any queries, please reach out to us at 0124-445-2000 or email us at care@flightsmojo.in&sender=FLMOJO";
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
            catch (Exception e)
            {
                //return e.Message;
                e.ToString();
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
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        [ActionName("contact-us")]
        public ActionResult Contact()
        {
            ViewBag.Message = "contact-us";
            CaptchaModel obj = new CaptchaModel();
            return View("Contact", obj);
        }
        [HttpPost]
        [ActionName("contact-us")]
        public ActionResult Contact(CaptchaModel obj)
        {
            if (ModelState.IsValid)
            {
                obj.sendMsg = "";
                ViewBag.Message = "contact-us";

                StringBuilder sb = new StringBuilder();

                sb.Append("<table border='1' cellpadding='10' cellspacing='10' style='width:700PX;'>");
                sb.Append("    <tr><td style='width:30%'>Name</td><td style='width:70%'>" + obj.Name + "</td></tr>");
                sb.Append("    <tr><td>EmailID</td><td>" + obj.EmailID + "</td></tr>");
                sb.Append("    <tr><td>PhoneNo</td><td>" + obj.PhoneNo + "</td></tr>");
                sb.Append("    <tr><td>Massege</td><td>" + obj.Massege + "</td></tr>");
                sb.Append("</table>");

                Core.SendEmailRequest objSendEmailRequest = new Core.SendEmailRequest();
                objSendEmailRequest.FromEmail = GlobalData.SendEmail;
                objSendEmailRequest.ToEmail = GlobalData.Email;
                objSendEmailRequest.BccEmail = "kundan@flightsmojo.com";
                objSendEmailRequest.MailSubject = "Contact us mail for flightsmojo.in";
                objSendEmailRequest.MailBody = sb.ToString();
                objSendEmailRequest.BookingID = 0;
                objSendEmailRequest.prodID = 0;
                objSendEmailRequest.MailType = "ContactUs";

                bool isSend = new Bal.SMTP().SendEMail(objSendEmailRequest);

                obj.sendMsg = isSend ? "We have accepted your query. Our support team will contact you shortly. Thank you!" : "Due to some technical resion we are not accept your query, Please try again After some time, Thankyou!";
                obj.Name = "";
                obj.EmailID = "";
                obj.PhoneNo = "";
                obj.Massege = "";
                return View("Contact", obj);
            }
            else
            {
                obj.sendMsg = "";
                return View("Contact", obj);
            }
        }



        public JsonResult Subscribe(string EmailID)
        {
            Core.CouponStatusResponse objResponse = new CouponStatusResponse();
            if (ModelState.IsValid)
            {
                Core.SendEmailRequest objSendEmailRequest = new Core.SendEmailRequest();
                objSendEmailRequest.FromEmail = GlobalData.SendEmail;
                objSendEmailRequest.ToEmail = EmailID;
                objSendEmailRequest.BccEmail = "brij@flightsmojo.com";
                objSendEmailRequest.MailSubject = "Subscribe mail for flightsmojo.in";
                objSendEmailRequest.MailBody = "Subscribe";
                objSendEmailRequest.BookingID = 0;
                objSendEmailRequest.prodID = 0;
                objSendEmailRequest.MailType = "Subscribe";

                bool isSend = new Bal.SMTP().SendEMail(objSendEmailRequest);

                //obj.sendMsg = isSend ? "We have accepted your query. Our support team will contact you shortly. Thank you!" : "Due to some technical resion we are not accept your query, Please try again After some time, Thankyou!";
                //obj.Name = "";

            }
            //if (ModelState.IsValid)
            //{
            //    obj.sendMsg = "";
            //    ViewBag.Message = "contact-us";

            //    StringBuilder sb = new StringBuilder();

            //    sb.Append("<table border='1' cellpadding='10' cellspacing='10' style='width:700PX;'>");
            //    sb.Append("    <tr><td style='width:30%'>Name</td><td style='width:70%'>" + obj.Name + "</td></tr>");
            //    sb.Append("    <tr><td>EmailID</td><td>" + obj.EmailID + "</td></tr>");
            //    sb.Append("    <tr><td>PhoneNo</td><td>" + obj.PhoneNo + "</td></tr>");
            //    sb.Append("    <tr><td>Massege</td><td>" + obj.Massege + "</td></tr>");
            //    sb.Append("</table>");

            //    Core.SendEmailRequest objSendEmailRequest = new Core.SendEmailRequest();
            //    objSendEmailRequest.FromEmail = GlobalData.SendEmail;
            //    objSendEmailRequest.ToEmail = GlobalData.Email;
            //    objSendEmailRequest.BccEmail = "kundan@flightsmojo.com";
            //    objSendEmailRequest.MailSubject = "Contact us mail for flightsmojo.in";
            //    objSendEmailRequest.MailBody = sb.ToString();
            //    objSendEmailRequest.BookingID = 0;
            //    objSendEmailRequest.prodID = 0;
            //    objSendEmailRequest.MailType = "ContactUs";

            //    bool isSend = new Bal.SMTP().SendEMail(objSendEmailRequest);

            //    obj.sendMsg = isSend ? "We have accepted your query. Our support team will contact you shortly. Thank you!" : "Due to some technical resion we are not accept your query, Please try again After some time, Thankyou!";
            //    obj.Name = "";
            //    obj.EmailID = "";
            //    obj.PhoneNo = "";
            //    obj.Massege = "";
            //    return View("Contact", obj);
            //}
            //else
            //{
            //    obj.sendMsg = "";
            //  return View("Subscribe", obj);
            return Json(objResponse, JsonRequestBehavior.AllowGet);
            //}
        }


        [ActionName("about-us")]
        public ActionResult about_us()
        {
            ViewBag.Message = "Your contact page.";

            return View("about_us");
        }
        [ActionName("terms-condition")]
        public ActionResult TermsCondition()
        {
            ViewBag.Message = "terms-condition";

            return View("TermsCondition");
        }
        [ActionName("cancelation-policy")]
        public ActionResult cancelationPolicy()
        {
            ViewBag.Message = "cancelation-policy";

            return View("cancelationPolicy");
        }
        [ActionName("user-agreement")]
        public ActionResult useragreement()
        {
            ViewBag.Message = "user-agreement";

            return View("useragreement");
        }

        [ActionName("privacy-policy")]
        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = "privacy-policy";

            return View("PrivacyPolicy");
        }
        [ActionName("pay-now")]
        public ActionResult paynow()
        {
            ViewBag.Message = "pay-now";

            return View("paynow");
        }
        [ActionName("cancellation-change")]
        public ActionResult cancellationchange()
        {
            ViewBag.Message = "cancellation-change";

            return View("cancellationchange");
        }
        [ActionName("credit-card-verification")]
        public ActionResult creditcardverification()
        {
            ViewBag.Message = "credit-card-cerification";

            return View("creditcardverification");
        }
        [ActionName("disclaimer")]
        public ActionResult Disclaimer()
        {
            ViewBag.Message = "Disclaimer";

            return View("Disclaimer");
        }
        [ActionName("taxes-fees")]
        public ActionResult taxesfees()
        {
            ViewBag.Message = "Taxes-fees";

            return View("Taxesfees");
        }
        [ActionName("booking-guide-lines")]
        public ActionResult bookingguidelines()
        {
            ViewBag.Message = "booking-guide-lines";

            return View("bookingguidelines");
        }
        [ActionName("flight-changes")]
        public ActionResult FlightChanges()
        {
            ViewBag.Message = "Flight-Changes";

            return View("FlightChanges");
        }

        [ActionName("travel-advisory")]
        public ActionResult traveladvisory()
        {
            ViewBag.Message = "travel-advisory";

            return View("traveladvisory");
        }
        [ActionName("visa")]
        public ActionResult visa()
        {
            ViewBag.Message = "visa";

            return View("visa");
        }
        [ActionName("cookies")]
        public ActionResult cookies()
        {
            ViewBag.Message = "cookies";

            return View("cookies");
        }
        [ActionName("sitemap")]
        public ActionResult sitemap()
        {
            Dal.DALOriginDestinationContent ODC = new DALOriginDestinationContent();
            Sitemap SM = ODC.GetSitemap();
            return View(SM);
        }

        public ActionResult genrateSiteMap()
        {
            SitemapXML();
            return View();
        }


        public void SitemapXML()
        {
            try
            {
                Dal.DALOriginDestinationContent ODC = new DALOriginDestinationContent();
                Core.ContentPage.Sitemap SM = ODC.GetSitemap();

                System.Xml.XmlDocument XMLDoc = new System.Xml.XmlDocument();
                System.Xml.XmlNode DocMode = XMLDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XMLDoc.AppendChild(DocMode);

                System.Xml.XmlElement DataNode = XMLDoc.CreateElement("urlset");

                DataNode.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
                DataNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                DataNode.SetAttribute("schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

                XMLDoc.AppendChild(DataNode);
                System.Xml.XmlNode url1 = XMLDoc.CreateElement("url");
                XMLDoc.DocumentElement.AppendChild(url1);

                System.Xml.XmlNode loc1 = XMLDoc.CreateElement("loc");
                loc1.AppendChild(XMLDoc.CreateTextNode(GlobalData.URL.ToLower()));
                url1.AppendChild(loc1);


                System.Xml.XmlNode changefreq = XMLDoc.CreateElement("changefreq");
                changefreq.AppendChild(XMLDoc.CreateTextNode("Daily"));
                url1.AppendChild(changefreq);


                System.Xml.XmlNode lastmod1 = XMLDoc.CreateElement("lastmod");
                lastmod1.AppendChild(XMLDoc.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd")));
                url1.AppendChild(lastmod1);

                for (int i = 0; i < SM.OnD.Count; i++)
                {
                    XMLDoc.AppendChild(DataNode);
                    System.Xml.XmlNode url = XMLDoc.CreateElement("url");
                    XMLDoc.DocumentElement.AppendChild(url);

                    System.Xml.XmlNode loc = XMLDoc.CreateElement("loc");
                    string URL = SM.OnD[i].OriginName.Trim().Replace(" ", "-") + "-" + SM.OnD[i].OriginCode + "-" + SM.OnD[i].DestinationName.Trim().Replace(" ", "-") + "-" + SM.OnD[i].DestinationCode;
                    loc.AppendChild(XMLDoc.CreateTextNode(GlobalData.URL.ToLower() + "/flights/" + URL + "-cheap-airtickets"));
                    url.AppendChild(loc);

                    System.Xml.XmlNode changefreq2 = XMLDoc.CreateElement("changefreq");
                    changefreq2.AppendChild(XMLDoc.CreateTextNode("Daily"));
                    url.AppendChild(changefreq2);

                    System.Xml.XmlNode lastmod = XMLDoc.CreateElement("lastmod");
                    DateTime dt = SM.OnD[i].Created;
                    lastmod.AppendChild(XMLDoc.CreateTextNode(dt.ToString("yyyy-MM-dd")));
                    url.AppendChild(lastmod);

                }

                for (int i = 0; i < SM.cityContent.Count; i++)
                {
                    XMLDoc.AppendChild(DataNode);
                    System.Xml.XmlNode url = XMLDoc.CreateElement("url");
                    XMLDoc.DocumentElement.AppendChild(url);

                    System.Xml.XmlNode loc = XMLDoc.CreateElement("loc");
                    string URL = SM.cityContent[i].CityName.Trim().Replace(" ", "-") + "-" + SM.cityContent[i].CityCode.Replace(" ", "-");
                    loc.AppendChild(XMLDoc.CreateTextNode(GlobalData.URL.ToLower() + "/city/cheap-flights-to-" + URL));
                    url.AppendChild(loc);

                    System.Xml.XmlNode changefreq3 = XMLDoc.CreateElement("changefreq");
                    changefreq3.AppendChild(XMLDoc.CreateTextNode("Daily"));
                    url.AppendChild(changefreq3);

                    System.Xml.XmlNode lastmod = XMLDoc.CreateElement("lastmod");
                    DateTime dt = SM.cityContent[i].InsertedOn;
                    lastmod.AppendChild(XMLDoc.CreateTextNode(dt.ToString("yyyy-MM-dd")));
                    url.AppendChild(lastmod);

                }


                for (int i = 0; i < SM.AirlineContent.Count; i++)
                {
                    XMLDoc.AppendChild(DataNode);
                    System.Xml.XmlNode url = XMLDoc.CreateElement("url");
                    XMLDoc.DocumentElement.AppendChild(url);

                    System.Xml.XmlNode loc = XMLDoc.CreateElement("loc");

                    string URL = SM.AirlineContent[i].AirlineName.Trim().Replace(" ", "-") + "-" + SM.AirlineContent[i].AirlineCode.Replace(" ", "-") + "-flight-tickets";
                    loc.AppendChild(XMLDoc.CreateTextNode(GlobalData.URL.ToLower() + "/airline/" + URL));
                    url.AppendChild(loc);

                    System.Xml.XmlNode changefreq3 = XMLDoc.CreateElement("changefreq");
                    changefreq3.AppendChild(XMLDoc.CreateTextNode("Daily"));
                    url.AppendChild(changefreq3);

                    System.Xml.XmlNode lastmod = XMLDoc.CreateElement("lastmod");
                    DateTime dt = SM.AirlineContent[i].InsertedOn;
                    lastmod.AppendChild(XMLDoc.CreateTextNode(dt.ToString("yyyy-MM-dd")));
                    url.AppendChild(lastmod);
                }


                for (int i = 0; i < SM.DealsContent.Count; i++)
                {
                    XMLDoc.AppendChild(DataNode);
                    System.Xml.XmlNode url = XMLDoc.CreateElement("url");
                    XMLDoc.DocumentElement.AppendChild(url);

                    System.Xml.XmlNode loc = XMLDoc.CreateElement("loc");
                    string URL = SM.DealsContent[i].ThemeName.Trim().Replace(" ", "-");
                    loc.AppendChild(XMLDoc.CreateTextNode(GlobalData.URL.ToLower() + "/deals/" + URL));
                    url.AppendChild(loc);

                    System.Xml.XmlNode changefreq3 = XMLDoc.CreateElement("changefreq");
                    changefreq3.AppendChild(XMLDoc.CreateTextNode("Daily"));
                    url.AppendChild(changefreq3);

                    System.Xml.XmlNode lastmod = XMLDoc.CreateElement("lastmod");
                    DateTime dt = SM.DealsContent[i].InsertedOn;
                    lastmod.AppendChild(XMLDoc.CreateTextNode(dt.ToString("yyyy-MM-dd")));
                    url.AppendChild(lastmod);
                }


                string[] StaticUrl = { "/top-international-destinations", "/first-class-flights", "/last-minute-flights", "/about-us", "/terms-condition", "/privacy-policy", "/user-agreement",
                    "/disclaimer","/contact-us","/sitemap","/flights","/airline","/deals","/web-checkin" };
                for (int i = 0; i < StaticUrl.Length; i++)
                {
                    XMLDoc.AppendChild(DataNode);
                    System.Xml.XmlNode url = XMLDoc.CreateElement("url");
                    XMLDoc.DocumentElement.AppendChild(url);

                    System.Xml.XmlNode loc = XMLDoc.CreateElement("loc");
                    string URL = StaticUrl[i];
                    loc.AppendChild(XMLDoc.CreateTextNode(GlobalData.URL.ToLower() + URL));
                    url.AppendChild(loc);


                    System.Xml.XmlNode changefreq1 = XMLDoc.CreateElement("changefreq");
                    changefreq1.AppendChild(XMLDoc.CreateTextNode("Daily"));
                    url.AppendChild(changefreq1);

                    System.Xml.XmlNode lastmod = XMLDoc.CreateElement("lastmod");
                    lastmod.AppendChild(XMLDoc.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd")));
                    url.AppendChild(lastmod);

                    //System.Xml.XmlNode priority = XMLDoc.CreateElement("priority");
                    //priority.AppendChild(XMLDoc.CreateTextNode("1.00"));
                    //url.AppendChild(priority);
                }



                var BasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
                if (!System.IO.Directory.Exists(BasePath))
                {
                    System.IO.Directory.CreateDirectory(BasePath);
                }
                var newfile = string.Format("{0}{1}", "sitemap", ".xml");
                XMLDoc.Save(BasePath + newfile);
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Errorsitemap_" + DateTime.Today.ToString("ddMMyy"), "Error");
            }
        }


        [ActionName("us-top-destination")]
        public ActionResult ustopdestination()
        {
            ViewBag.Message = "us-top-destination";

            return View("US_Top_Destination");
        }
        [ActionName("top-international-destinations")]
        public ActionResult topinternationaldestinations()
        {
            ViewBag.Message = "top-international-destinations";

            return View("Top_International_Destinations");
        }
        [ActionName("first-class-flights")]
        public ActionResult firstclassflights()
        {
            ViewBag.Message = "first-class-flights";

            return View("firstclassflights");
        }
        [ActionName("last-minute-flights")]
        public ActionResult lastminuteflights()
        {
            ViewBag.Message = "last-minute-flights";

            return View("LastMinuteFlights");
        }
        [ActionName("404")]
        public ActionResult error404()
        {
            Response.StatusCode = 404;
            return View("error404");
        }
        [ActionName("500")]
        public ActionResult error500()
        {
            ViewBag.Message = "";
            return View("error500");
        }
        [ActionName("UpdateStaticData")]
        public ActionResult UpdateStaticData()
        {
            Core.FlightUtility.LoadMasterData();
            Bal.MakeFlightItinerary.setMarkupRule();
            ViewBag.Message = "";
            return View("error404");
        }
        //[ActionName("test")]
        //public ActionResult test()
        //{
        //    return View();
        //    //Core.SendEmailRequest sendEmailRequest = new Core.SendEmailRequest() { FromEmail = GlobalData.SendEmail, ToEmail = "kundan@flightsmojo.com", CcEmail = "support@flightsmojo.com", MailSubject = "test by kundan", MailBody = "test by kundan" };
        //    //new Bal.SMTP().SendEMail(sendEmailRequest);
        //    //ViewBag.Message = "pay-now";

        //    //return View("paynow");
        //}

        public void setCookie(string sourceMedia)
        {
            int intSmedia;
            bool bNum = int.TryParse(sourceMedia, out intSmedia);
            sourceMedia = bNum ? intSmedia.ToString() : "1000";
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


        [ActionName("flights")]
        public ActionResult flights()
        {
            return View("flights");
        }


        [ActionName("web-checkin")]
        public ActionResult webcheckin()
        {
            ViewBag.Message = "Your web-checkin page.";

            return View("webcheckin");
        }

        [ActionName("fm")]
        public ActionResult fm(string id)
        {
            ViewBag.Message = "Your web-checkin page.";
            string strId = Decode(id);
            int sid;

            bool success = int.TryParse(strId, out sid);
            if (success)
            {
                var strDtl = new DAL.ShortLink.DalShortLinkOperation().getSearchDetails(sid);
                if (!string.IsNullOrEmpty(strDtl))
                {
                    Core.Flight.FlightSearchRequest fsr = Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Flight.FlightSearchRequest>(StringHelper.DecompressString(strDtl));
                    fsr.userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    fsr.userSessionID = Session.SessionID;
                    fsr.userLogID = fsr.userSearchID = getSearchID();
                    fsr.utm_medium = "retrageting";
                    fsr.utm_campaign = "webengage";
                    Core.Flight.AirContext airContext = new Core.Flight.AirContext(fsr.userIP);

                    airContext.flightSearchRequest = fsr;
                    airContext.IsSearchCompleted = false;
                    airContext.flightRef = new List<string>();

                    FlightOperation.SetAirContext(airContext);
                    return Redirect("/Flight/Result/" + fsr.userSearchID);
                }
            }

            return Redirect("/");
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string Decode(string text)
        {
            text = text.Replace('_', '/').Replace('-', '+');
            switch (text.Length % 4)
            {
                case 2:
                    text += "==";
                    break;
                case 3:
                    text += "=";
                    break;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }
        private string getSearchID()
        {
            return (DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid().ToString("N"));
        }
        #region Ip Tracker
        private IpDetails GetIpDetails(string ip)
        {
            try
            {
                WebClient client = new WebClient();
                var url = "http://api.ipstack.com/" + ip + "?access_key=b9a92c66d32c538f7cd668d9b8986135";
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var kk = client.DownloadString(url);
                Core.IpDetails ipDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<Core.IpDetails>(kk.ToString());
                saveIpDetails(ipDetails);
                return ipDetails;
            }
            catch { return null; }

        }
        private void saveIpDetails(Core.IpDetails ip)
        {
            var save = Task.Run(async () =>
            {
                await saveIP(ip);
            });
        }
        private async System.Threading.Tasks.Task saveIP(Core.IpDetails ipDetails)
        {
            try
            {
                DAL.IP_Details obj = new DAL.IP_Details();

                if (ipDetails != null)
                {
                    obj.saveIpDetails(ipDetails);
                }

            }
            catch { }

        }

        public void setIpTrackerCookie(string sourceMedia)
        {
            HttpCookie IpTrackerSite = new HttpCookie("MojoIpTracker");
            IpTrackerSite["siteID"] = sourceMedia;
            IpTrackerSite.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(IpTrackerSite);
        }
        public string GetIpTrackerCookie()
        {
            HttpCookie IpTrackerSite = Request.Cookies["MojoIpTracker"];
            if (IpTrackerSite != null)
            {
                return IpTrackerSite["siteID"].ToString();
            }
            return "";
        }
        #endregion
    }
}