using Core;
using Core.Flight;
//using DAL.SecretDeal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MojoIndia.Controllers
{
    public class serviceController : Controller
    {
        //
        // GET: /service/
        public JsonResult GetCity(string ID)
        {
            //string output = Newtonsoft.Json.JsonConvert.SerializeObject(Core.FlightUtility.AirportList);
            return Json(new Bal.FlightDetails().GetCity(ID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getWebsiteDeal(int dealType, string origin, string destination, string airline, string tripType, string cabinClass)
        {
            DAL.Deal.WebsiteDeal obj = new DAL.Deal.WebsiteDeal();
            tripType = tripType == "" ? "2" : tripType;
            cabinClass = cabinClass == "" ? "1" : cabinClass;
            var kk = obj.GetWebsiteDealOnPage(Convert.ToInt32(dealType), origin, destination, airline, Convert.ToInt16(tripType), Convert.ToInt16(cabinClass));
            return Json(kk, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult getBookingDetails(int reftype, string refNo, string LastName)
        //{
        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult CallBackRequest(string EmailID, string PhoneNo, string Name)
        //{
        //    AirContext airContext = Session["SearchDetails"] as AirContext;
        //    ResponseStatus rs = new ResponseStatus();
        //    DalSecretFareDetails obj = new DalSecretFareDetails();
        //    obj.saveCallBackEnquiry((int)SiteId.FlightsMojo, Name, EmailID, PhoneNo, (int)airContext.flightSearchRequest.tripType,
        //        (airContext.flightSearchRequest.segment[0].originAirport + "-" + airContext.flightSearchRequest.segment[0].destinationAirport),
        //        airContext.flightSearchRequest.segment[0].travelDate,
        //        (airContext.flightSearchRequest.segment.Count > 1 ? airContext.flightSearchRequest.segment[1].travelDate : airContext.flightSearchRequest.segment[0].travelDate),
        //        airContext.flightSearchResponse.flightResult[0].fare.grandTotal, airContext.flightSearchRequest.adults,
        //        airContext.flightSearchRequest.child, airContext.flightSearchRequest.infants);
        //    try
        //    {
        //        SendEmailRequest objSendEmailRequest = new SendEmailRequest();
        //        objSendEmailRequest.FromEmail = GlobalData.SendEmail;
        //        objSendEmailRequest.ToEmail = "care@flightsmojo.com";
        //        objSendEmailRequest.CcEmail = "support@flightsmojo.com";
        //        objSendEmailRequest.BccEmail = "kundan@flightsmojo.com";
        //        objSendEmailRequest.MailSubject = "Call back request with flightsmojo secret deal";
        //        objSendEmailRequest.MailBody = MakeAdminCallBackMailBody(EmailID, PhoneNo, Name, ref airContext);
        //        objSendEmailRequest.BookingID = 0;
        //        objSendEmailRequest.prodID = 0;
        //        objSendEmailRequest.MailType = "CallBack";
        //        new Bal.SMTP().SendEMail(objSendEmailRequest);
        //        //rs.messege = "Our team accept call back request, Our support team call back shortly, Thankyou!";
        //        rs.messege = "We have accepted your call back request. Our support team will call you back shortly. Thank you!";
        //    }
        //    catch (Exception ex)
        //    {
        //        rs.messege = "Due to some technical resion we are not accept your call back request, Please try again After some time, Thankyou!";
        //    }
        //    return Json(rs, JsonRequestBehavior.AllowGet);
        //}
        //private string MakeAdminCallBackMailBody(string EmailID, string PhoneNo, string Name, ref AirContext airContext)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<table border='1' cellpadding='10' cellspacing='10' style='width:700PX;'>");
        //    sb.Append("    <tr><td style='width:30%'>Name</td><td style='width:70%'>" + Name + "</td></tr>");
        //    sb.Append("    <tr><td>EmailID</td><td>" + EmailID + "</td></tr>");
        //    sb.Append("    <tr><td>PhoneNo</td><td>" + PhoneNo + "</td></tr>");
        //    sb.Append("    <tr><td>TripType</td><td>" + airContext.flightSearchRequest.tripType.ToString() + "</td></tr>");
        //    sb.Append("    <tr><td>SearchSector</td><td>" + airContext.flightSearchRequest.segment[0].originAirport + "-" + airContext.flightSearchRequest.segment[0].destinationAirport + "</td></tr>");
        //    sb.Append("    <tr><td>TravelDate</td><td>" + airContext.flightSearchRequest.segment[0].travelDate.ToString("dd MMM yyyy") + (airContext.flightSearchRequest.segment.Count > 1 ? (" - " + airContext.flightSearchRequest.segment[1].travelDate.ToString("dd MMM yyyy")) : "") + "</td></tr>");
        //    sb.Append("    <tr><td>ShowFareToPax</td><td>" + airContext.flightSearchResponse.flightResult[0].fare.grandTotal + "</td></tr>");
        //    sb.Append("    <tr><td>PaxDetails</td><td>Adult:" + airContext.flightSearchRequest.adults + ",Child:" + airContext.flightSearchRequest.child + ",Infant:" + airContext.flightSearchRequest.infants + "</td></tr>");
        //    sb.Append("</table>");
        //    return sb.ToString();
        //}

        public JsonResult SendContactMail(string txtName, string txtEmailID, string txtPhone, string txtYourMsg)
        {
            ResponseStatus rs = new ResponseStatus();
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<table border='1' cellpadding='10' cellspacing='10' style='width:700PX;'>");
                sb.Append("    <tr><td style='width:30%'>Name</td><td style='width:70%'>" + txtName + "</td></tr>");
                sb.Append("    <tr><td>EmailID</td><td>" + txtEmailID + "</td></tr>");
                sb.Append("    <tr><td>PhoneNo</td><td>" + txtPhone + "</td></tr>");
                sb.Append("    <tr><td>Massege</td><td>" + txtYourMsg + "</td></tr>");
                sb.Append("</table>");

                SendEmailRequest objSendEmailRequest = new SendEmailRequest();
                objSendEmailRequest.FromEmail = GlobalData.SendEmail;
                objSendEmailRequest.ToEmail = GlobalData.Email;
                //objSendEmailRequest.CcEmail = "support@flightsmojo.com";
                objSendEmailRequest.BccEmail = "kundan@flightsmojo.com";
                objSendEmailRequest.MailSubject = "Contact us mail for flightsmojo.in";
                objSendEmailRequest.MailBody = sb.ToString();
                objSendEmailRequest.BookingID = 0;
                objSendEmailRequest.prodID = 0;
                objSendEmailRequest.MailType = "ContactUs";

                bool isSend = new Bal.SMTP().SendEMail(objSendEmailRequest);

                rs.message = isSend ? "We have accepted your query. Our support team will contact you shortly. Thank you!" : "Due to some technical resion we are not accept your query, Please try again After some time, Thankyou!";
            }
            catch (Exception ex)
            {
                new LogWriter(ex.ToString(), "Error19_" + DateTime.Today.ToString("ddMMyy"), "Error");
                rs.message = "Due to some technical resion we are not accept your query, Please try again After some time, Thankyou!";
            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }


    }
}