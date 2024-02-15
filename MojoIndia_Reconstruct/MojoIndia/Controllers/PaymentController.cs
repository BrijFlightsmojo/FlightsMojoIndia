using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.RP;
using System.Net;
using Razorpay.Api;
using System.Text;
using Core.Flight;
using System.IO;

namespace MojoIndia.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Payment()
        {
            RefundAmt();
            return View();
        }

        [HttpPost]
        public ActionResult Payment(FormCollection FC)
        {
            return View();
        }
        public static string key = "rzp_test_JryOulLTAoMatO";
        public static string secret = "GhSLLbBrwhC4PDY5bPqNBwcL";
        
        public JsonResult Details(string email, string phone, string amount)
        {
            PaymentRP RP = new PaymentRP();
            string orderid = string.Empty;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                String orderId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                RazorpayClient client = new RazorpayClient(key, secret);
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", 500 * 100);
                options.Add("receipt", orderId);
                options.Add("currency", "INR");
                options.Add("payment_capture", "0");
                options.Add("method", "netbanking");
                //options.Add("name", "Test");
                //card,netbanking,wallet,emi,upi,app
                Order order = client.Order.Create(options);
                RP.orderid = order.Attributes["id"];
                //RP.name = "Test";
                RP.email = email;
                RP.phone = phone;
                RP.amount = (Convert.ToInt32(amount) * 100).ToString();
                RP.status = "1";
            }
            catch (Exception ex)
            {
                RP.status = "2";
                RP.name = ex.Message;
            }
            finally
            {

            }
            return Json(RP, JsonRequestBehavior.AllowGet);
            //return Json(true, JsonRequestBehavior.AllowGet);
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

        public JsonResult goToPayment(string orderid, string paymentid, string signature)
        {
            PaymentCaptured pc = new PaymentCaptured();
            string status = "", capture = "", createdat = "", contact = "", email = "", amount = "";
            try
            {
                decimal amtount = 500 * 100;
                string newsignatue = HmacSha256Digest(orderid + "|" + paymentid, secret);
                if (signature == newsignatue)
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
                }
                if (status == "captured")
                {
                    pc.status = "1";
                    pc.orderid = orderid;// Payment Done              
                }
                else if (status == "Authorized")
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
                    if (status == "captured")
                    {
                        pc.status = "1";
                        pc.orderid = orderid;// Payment Done              
                    }
                }
                else
                {
                    pc.status = "0";
                }
            }
            catch (Exception ex)
            {
                pc.status = "2";
                pc.msg = ex.Message;
            }
            return Json(pc, JsonRequestBehavior.AllowGet);
        }



        public string GetMailBody(FlightBookingResponse fsr)
        {
            String result;
            string apiKey = "NDQ3MDY0NmY0NDRiNzAzMTdhNTczNjUyNjQzNzYxNTA=";
            string numbers = fsr.phoneNo; // in a comma seperated list
            string message = "Dear '"+fsr.passengerDetails+"',Thank you for choosing FlightsMojo.in. You flight to Delhi is confirmed and ticketed.Please find the details below:FM Booking ID: '"+fsr.bookingID+ "' Airline PNR: '" + fsr.PNR + "' Travel Date: '"+fsr.bookingID+"' In case of any queries, please reach out to us at 9874563210or email us at care@flightsmojo.in";
            string sender = "FLMOJO";
            String url = "https://api.textlocal.in/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
            //refer to parameters to complete correct url string

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
                return e.Message;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                // Close and clean up the StreamReader
                sr.Close();
            }
            return result;
            //StringBuilder sb = new StringBuilder();
            //return sb.ToString();
        }


        public void RefundAmt()
        {
            string paymentId = "pay_N92sZfWJrlpCOF";
            RazorpayClient client = new RazorpayClient(key, secret);
            Dictionary<string, object> refundRequest = new Dictionary<string, object>();
            refundRequest.Add("amount", 200);
            refundRequest.Add("speed", "normal");
            Dictionary<string, object> notes = new Dictionary<string, object>();
            notes.Add("notes_key_1", "Tea, Earl Grey, Hot");
            notes.Add("notes_key_2", "Tea, Earl Grey… decaf.");
            refundRequest.Add("notes", notes);
            refundRequest.Add("receipt", "Receipt No. #32");
            Refund refund = client.Payment.Fetch(paymentId).Refund(refundRequest);
            string ss = refund.ToString();
        }



    }
}