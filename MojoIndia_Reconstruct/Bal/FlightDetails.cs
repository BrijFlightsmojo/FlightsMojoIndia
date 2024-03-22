using Core;
using Core.Flight;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
//using System.Web;
using System.Net;
//using System.Configuration;


namespace Bal
{
    public class FlightDetails
    {
        public static string API_URL = ConfigurationManager.AppSettings["FlightApiUrl"].ToString();
        public static string authcode = ConfigurationManager.AppSettings["AuthCode"].ToString();
        public static string[] airlineArr = new string[] { "6E", "AI", "G8", "I5", "IX", "QP", "SG", "QQ", "UK" };
        public Stopwatch stopwatch = new Stopwatch();
        public void SearchFlight(ref AirContext airContext, bool isDummyResult)
        {
            #region set flight result and Markup

            #endregion
            try
            {
                if (airContext.flightSearchRequest != null)
                {
                    WebClient client = new WebClient();
                    //var url = API_URL + "Flights/GetFlightResultTest?authcode=" + authcode;
                    var url = API_URL + "Flights/SearchFlight?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(airContext.flightSearchRequest);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    //string kk = "";
                    var kk = client.UploadString(url, serialisedData);
                    // if (isDummyResult)
                    // {
                    //     if (airContext.flightSearchRequest.travelType == TravelType.International)
                    //     {
                    //         //kk = client.UploadString(url, serialisedData);
                    //         string path = System.IO.Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, (airContext.flightSearchRequest.tripType == TripType.OneWay ? "IntResponseOneWay.json" : "IntResponse.json"));
                    //         using (System.IO.StreamReader r = new System.IO.StreamReader(path))
                    //         {
                    //             kk = r.ReadToEnd();
                    //         }
                    //     }
                    //     else
                    //     {
                    //         string path = System.IO.Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, (airContext.flightSearchRequest.tripType == TripType.OneWay ? "DomResponseOneWay.json" : "DomResponse.json"));
                    //         using (System.IO.StreamReader r = new System.IO.StreamReader(path))
                    //         {
                    //             kk = r.ReadToEnd();
                    //         }
                    //     }
                    // }
                    // else
                    // {
                    //     kk = client.UploadString(url, serialisedData);
                    // }
                    //// { "PassengerType":1,"BaseFare":8617.00,"Tax":1738.70,"YQTax":750.00,"AdditionalTxnFeePub":0.0,"AdditionalTxnFeeOfrd":0.0,"PGCharge":0.0,"Markup":0.0,"CommissionEarned":0.0,"TdsOnCommission":0.0,"OtherCharges":971.00}]
                    airContext.flightSearchResponse = JsonConvert.DeserializeObject<FlightSearchResponse>(kk.ToString());
                    airContext.IsSearchCompleted = true;

                    #region setAirline URL

                    foreach (var item in airContext.flightSearchResponse.Results)
                    {
                        foreach (var result in item)
                        {
                            foreach (var fs in result.FlightSegments)
                            {
                                foreach (var seg in fs.Segments)
                                {
                                    if (airlineArr.Contains(seg.Airline))
                                    {
                                        seg.url = "/images/airlinesSvg/" + seg.Airline + ".svg";
                                    }
                                    else
                                    {
                                        seg.url = "/images/flight_small/" + seg.Airline + ".gif";
                                    }
                                }
                            }
                        }
                    }
                    #endregion


                    LogCreater.CreateLogFile(kk, "Log\\Search", airContext.flightSearchRequest.userSearchID + ".txt");
                }
                else
                {
                    airContext.IsSearchCompleted = true;
                    airContext.flightSearchResponse = new FlightSearchResponse();
                    airContext.flightSearchResponse.response.status = TransactionStatus.Error;
                    airContext.flightSearchResponse.response.message = "Request is empty!";
                }
            }
            catch (Exception exrr)
            {
                airContext.IsSearchCompleted = true;
                airContext.flightSearchResponse = new FlightSearchResponse();
                airContext.flightSearchResponse.response = new ResponseStatus();
                airContext.flightSearchResponse.response.status = TransactionStatus.Error;
                airContext.flightSearchResponse.response.message = "Request is empty!";
            }
        }
        public FlightSearchResponse ReSearchFlight(FlightSearchRequest searchRequest)
        {
            try
            {
                if (searchRequest != null)
                {
                    WebClient client = new WebClient();
                    var url = API_URL + "Flights/SearchFlight?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(searchRequest);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    string kk = client.UploadString(url, serialisedData);
                    return JsonConvert.DeserializeObject<FlightSearchResponse>(kk.ToString());
                }
                else
                {
                    FlightSearchResponse flightSearchResponse = new FlightSearchResponse();
                    flightSearchResponse.response.status = TransactionStatus.Error;
                    flightSearchResponse.response.message = "Request is empty!";
                    return flightSearchResponse;
                }
            }
            catch (Exception exrr)
            {
                FlightSearchResponse flightSearchResponse = new FlightSearchResponse();
                flightSearchResponse.response.status = TransactionStatus.Error;
                flightSearchResponse.response.message = "Request is empty!";
                return flightSearchResponse;
            }
        }
        public void priceVerification(ref AirContext airContext, ref StringBuilder sbLogger)
        {
            FlightBookingResponse fbr;
            try
            {
                if (airContext.priceVerificationRequest != null)
                {
                    WebClient client = new WebClient();
                    var url = API_URL + "Flights/FlightVerification?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(airContext.priceVerificationRequest);
                    #region requestLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original Request---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + serialisedData);
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    #region responseLog
                    //sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original response---------------------------------------------");
                    //sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    //sbLogger.Append(Environment.NewLine + kk.ToString());
                    //sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion
                    airContext.priceVerificationResponse = JsonConvert.DeserializeObject<PriceVerificationResponse>(kk.ToString());
                }
                else
                {
                    airContext.priceVerificationResponse = new PriceVerificationResponse();
                    airContext.priceVerificationResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = "Request is empty!" };

                }
            }
            catch (Exception exrr)
            {
                #region Error Log
                //sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
                //sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                //sbLogger.Append(Environment.NewLine + exrr.ToString());
                //sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                #endregion

                airContext.priceVerificationResponse = new PriceVerificationResponse();
                airContext.priceVerificationResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = exrr.ToString() };
            }
        }


        public GfPriceVerifyResponse PriceVerify(GfPriceVerifyRequest fsr)
        {
            try
            {
                if (fsr != null)
                {
                    WebClient client = new WebClient();
                    var url = API_URL + "Flights/GfPriceVrify?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(fsr);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    return JsonConvert.DeserializeObject<GfPriceVerifyResponse>(kk.ToString());
                }
                else
                {
                    return new GfPriceVerifyResponse() { fare = new Fare(), responseStatus = new ResponseStatus { status = TransactionStatus.Error, message = "" } };
                }
            }
            catch (Exception exrr)
            {
                return new GfPriceVerifyResponse() { fare = new Fare(), responseStatus = new ResponseStatus { status = TransactionStatus.Error, message = exrr.ToString() } };
            }
        }
        public void FlightVerification(ref AirContext airContext, ref StringBuilder sbLogger)
        {
            FlightBookingResponse fbr;
            try
            {
                if (airContext.priceVerificationRequest != null)
                {
                    WebClient client = new WebClient();
                    var url = API_URL + "Flights/FlightVerification?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(airContext.priceVerificationRequest);
                    #region requestLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Price Verification Request---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + serialisedData);
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    #region responseLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Price Verification response---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + kk.ToString());
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion
                    airContext.priceVerificationResponse = JsonConvert.DeserializeObject<PriceVerificationResponse>(kk.ToString());
                }
                else
                {
                    airContext.priceVerificationResponse = new PriceVerificationResponse();
                    airContext.priceVerificationResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = "Request is empty!" };

                }
            }
            catch (Exception exrr)
            {
                #region Error Log
                //sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
                //sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                //sbLogger.Append(Environment.NewLine + exrr.ToString());
                //sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                #endregion

                airContext.priceVerificationResponse = new PriceVerificationResponse();
                airContext.priceVerificationResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = exrr.ToString() };
            }
        }

        public void TjPriceVerification(ref AirContext airContext, ref StringBuilder sbLogger)
        {
            FlightBookingResponse fbr;
            try
            {
                if (airContext.priceVerificationRequest != null)
                {
                    WebClient client = new WebClient();
                    // var url = API_URL + "Flights/TjPriceVerification?authcode=" + authcode; FlightVerification
                    var url = API_URL + "Flights/FlightVerification?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(airContext.priceVerificationRequest);
                    #region requestLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Price Verification Request---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + serialisedData);
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    #region responseLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Price Verification response---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + kk.ToString());
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion
                    airContext.priceVerificationResponse = JsonConvert.DeserializeObject<PriceVerificationResponse>(kk.ToString());
                }
                else
                {
                    airContext.priceVerificationResponse = new PriceVerificationResponse();
                    airContext.priceVerificationResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = "Request is empty!" };

                }
            }
            catch (Exception exrr)
            {
                #region Error Log
                //sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
                //sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                //sbLogger.Append(Environment.NewLine + exrr.ToString());
                //sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                #endregion

                airContext.priceVerificationResponse = new PriceVerificationResponse();
                airContext.priceVerificationResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = exrr.ToString() };
            }
        }

        public void Tgy_PriceVerification(ref AirContext airContext, ref StringBuilder sbLogger)
        {
            FlightBookingResponse fbr;
            try
            {
                if (airContext.priceVerificationRequest != null)
                {
                    WebClient client = new WebClient();
                    //var url = API_URL + "Flights/TgyPriceVerification?authcode=" + authcode;FlightVerification
                    var url = API_URL + "Flights/FlightVerification?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(airContext.priceVerificationRequest);
                    #region requestLog
                    sbLogger.Append(Environment.NewLine + "--------------------------------------------- Price Verification Request---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + serialisedData);
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    #region responseLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Price Verification response---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + kk.ToString());
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion
                    airContext.priceVerificationResponse = JsonConvert.DeserializeObject<PriceVerificationResponse>(kk.ToString());
                }
                else
                {
                    airContext.priceVerificationResponse = new PriceVerificationResponse();
                    airContext.priceVerificationResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = "Request is empty!" };

                }
            }
            catch (Exception exrr)
            {
                #region Error Log
                //sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
                //sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                //sbLogger.Append(Environment.NewLine + exrr.ToString());
                //sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                #endregion

                airContext.priceVerificationResponse = new PriceVerificationResponse();
                airContext.priceVerificationResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = exrr.ToString() };
            }
        }



        public void bookFlight(ref AirContext airContext, ref StringBuilder sbLogger)
        {
            FlightBookingResponse fbr;
            try
            {
                if (airContext.flightBookingRequest != null)
                {
                    WebClient client = new WebClient();
                    var url = API_URL + "Flights/BookFlight?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(airContext.flightBookingRequest);
                    #region requestLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original Request---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + serialisedData);
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    #region responseLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original response---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + kk.ToString());
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion
                    airContext.flightBookingResponse = JsonConvert.DeserializeObject<FlightBookingResponse>(kk.ToString());
                }
                else
                {
                    airContext.flightBookingResponse = new FlightBookingResponse();
                    airContext.flightBookingResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = "Request is empty!" };

                }
            }
            catch (Exception exrr)
            {
                #region Error Log
                sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
                sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                sbLogger.Append(Environment.NewLine + exrr.ToString());
                sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                #endregion

                airContext.flightBookingResponse = new FlightBookingResponse();
                airContext.flightBookingResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = exrr.ToString() };
            }
        }

        public void TicketFlight(ref AirContext airContext, ref StringBuilder sbLogger)
        {
            FlightBookingResponse fbr;
            try
            {
                if (airContext.flightBookingRequest != null)
                {
                    WebClient client = new WebClient();
                    var url = API_URL + "Flights/TicketFlight?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(airContext.flightBookingRequest);
                    #region requestLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original Request---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + serialisedData);
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    #region responseLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original response---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + kk.ToString());
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion
                    airContext.flightBookingResponse = JsonConvert.DeserializeObject<FlightBookingResponse>(kk.ToString());
                }
                else
                {
                    airContext.flightBookingResponse = new FlightBookingResponse();
                    airContext.flightBookingResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = "Request is empty!" };

                }
            }
            catch (Exception exrr)
            {
                #region Error Log
                sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
                sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                sbLogger.Append(Environment.NewLine + exrr.ToString());
                sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                #endregion

                airContext.flightBookingResponse = new FlightBookingResponse();
                airContext.flightBookingResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = exrr.ToString() };
            }
        }
        public FlightBookingResponse SaveBookingDetails(FlightBookingRequest flightBookingRequest, ref StringBuilder sbLogger)
        {
            FlightBookingResponse fbr;
            try
            {
                if (flightBookingRequest != null)
                {
                    WebClient client = new WebClient();
                    var url = API_URL + "Flights/SaveBookingDetails?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(flightBookingRequest);
                    #region requestLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original Request---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + serialisedData);
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    #region responseLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original response---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + kk.ToString());
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion
                    fbr = JsonConvert.DeserializeObject<FlightBookingResponse>(kk.ToString());
                }
                else
                {
                    fbr = new FlightBookingResponse();
                    fbr.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = "Request is empty!" };
                }
            }
            catch (Exception exrr)
            {
                #region Error Log
                sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
                sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                sbLogger.Append(Environment.NewLine + exrr.ToString());
                sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                #endregion

                fbr = new FlightBookingResponse();
                fbr.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = exrr.ToString() };
            }
            return fbr;
        }

        public FlightBookingResponse SaveBookingDetails_WithOutPax(FlightBookingRequest flightBookingRequest, ref StringBuilder sbLogger)
        {
            FlightBookingResponse fbr;
            try
            {
                if (flightBookingRequest != null)
                {
                    WebClient client = new WebClient();
                    var url = API_URL + "Flights/SaveBookingDetails_WithOutPax?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(flightBookingRequest);
                    #region requestLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original Request---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + serialisedData);
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    #region responseLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original response---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + kk.ToString());
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion
                    fbr = JsonConvert.DeserializeObject<FlightBookingResponse>(kk.ToString());
                }
                else
                {
                    fbr = new FlightBookingResponse();
                    fbr.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = "Request is empty!" };
                }
            }
            catch (Exception exrr)
            {
                #region Error Log
                sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
                sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                sbLogger.Append(Environment.NewLine + exrr.ToString());
                sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                #endregion

                fbr = new FlightBookingResponse();
                fbr.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = exrr.ToString() };
            }
            return fbr;
        }

        public FlightBookingResponse Update_BookingPaxDetail(FlightBookingRequest flightBookingRequest, ref StringBuilder sbLogger)
        {
            FlightBookingResponse fbr;
            try
            {
                if (flightBookingRequest != null)
                {
                    WebClient client = new WebClient();
                    var url = API_URL + "Flights/Update_BookingPaxDetail?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(flightBookingRequest);
                    #region requestLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original Request---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + serialisedData);
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    #region responseLog
                    //sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original response---------------------------------------------");
                    //sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    //sbLogger.Append(Environment.NewLine + kk.ToString());
                    //sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    bookingLog(ref sbLogger, "SaveBookingWithOutPax Original Request", JsonConvert.SerializeObject(flightBookingRequest));
                    #endregion
                    fbr = JsonConvert.DeserializeObject<FlightBookingResponse>(kk.ToString());
                }
                else
                {
                    fbr = new FlightBookingResponse();
                    fbr.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = "Request is empty!" };
                }
            }
            catch (Exception exrr)
            {
                #region Error Log
                //sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
                //sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                //sbLogger.Append(Environment.NewLine + exrr.ToString());
                //sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                bookingLog(ref sbLogger, "Booking save Error", exrr.ToString());
                LogCreater.CreateLogFile(sbLogger.ToString(), "Log\\error", (flightBookingRequest.bookingID > 0 ? flightBookingRequest.bookingID.ToString() : "error" + DateTime.Today.ToString("ddMMyyy")) + ".txt");
                #endregion

                fbr = new FlightBookingResponse();
                fbr.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = exrr.ToString() };
            }
            return fbr;
        }

        public void bookingLog(ref StringBuilder sbLogger, string requestTitle, string logText)
        {
            sbLogger.Append(Environment.NewLine + "---------------------------------------------" + requestTitle + "" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------------------------------");
            sbLogger.Append(Environment.NewLine + logText);
            sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
        }
        public void tjBookFlight(ref AirContext airContext, ref StringBuilder sbLogger)
        {
            FlightBookingResponse fbr;
            try
            {
                if (airContext.flightBookingRequest != null)
                {
                    WebClient client = new WebClient();
                    var url = API_URL + "Flights/tjBookFlight?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(airContext.flightBookingRequest);
                    #region requestLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original Request---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + serialisedData);
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    #region responseLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original response---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + kk.ToString());
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion
                    airContext.flightBookingResponse = JsonConvert.DeserializeObject<FlightBookingResponse>(kk.ToString());
                }
                else
                {
                    airContext.flightBookingResponse = new FlightBookingResponse();
                    airContext.flightBookingResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = "Request is empty!" };

                }
            }
            catch (Exception exrr)
            {
                #region Error Log
                sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
                sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                sbLogger.Append(Environment.NewLine + exrr.ToString());
                sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                #endregion

                airContext.flightBookingResponse = new FlightBookingResponse();
                airContext.flightBookingResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = exrr.ToString() };
            }
        }

        public void tgyBookFlight(ref AirContext airContext, ref StringBuilder sbLogger)
        {
            FlightBookingResponse fbr;
            try
            {
                if (airContext.flightBookingRequest != null)
                {
                    WebClient client = new WebClient();
                    var url = API_URL + "Flights/tgyBookFlight?authcode=" + authcode;
                    string serialisedData = JsonConvert.SerializeObject(airContext.flightBookingRequest);
                    #region requestLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original Request---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + serialisedData);
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.DateTime dt = DateTime.Now;
                    var kk = client.UploadString(url, serialisedData);
                    #region responseLog
                    sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original response---------------------------------------------");
                    sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                    sbLogger.Append(Environment.NewLine + kk.ToString());
                    sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    #endregion
                    airContext.flightBookingResponse = JsonConvert.DeserializeObject<FlightBookingResponse>(kk.ToString());
                }
                else
                {
                    airContext.flightBookingResponse = new FlightBookingResponse();
                    airContext.flightBookingResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = "Request is empty!" };

                }
            }
            catch (Exception exrr)
            {
                #region Error Log
                sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
                sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
                sbLogger.Append(Environment.NewLine + exrr.ToString());
                sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                #endregion

                airContext.flightBookingResponse = new FlightBookingResponse();
                airContext.flightBookingResponse.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, message = exrr.ToString() };
            }
        }


        //public void SearchFlight_gf(ref FlightSearchRequest objSearchRequest, ref AirContext airContext)
        //{
        //    #region set flight result and Markup
        //    airContext.flightSearchResponse = new FlightSearchResponse();
        //    airContext.flightSearchResponse.airline = new List<Airline>();
        //    //airContext.flightSearchResponse.operatedAirline = new List<Airline>();
        //    airContext.flightSearchResponse.airport = new List<Airport>();         
        //    airContext.flightSearchResponse.adults = objSearchRequest.adults;
        //    airContext.flightSearchResponse.child = objSearchRequest.child;
        //    airContext.flightSearchResponse.infants = objSearchRequest.infants;
        //    //airContext.flightSearchResponse.responsStatus = new ResponseStatus();
        //    #endregion
        //    try
        //    {
        //        if (objSearchRequest != null)
        //        {
        //            WebClient client = new WebClient();
        //            var url = API_URL + "Flights/GetFlightResult_gf?authcode=" + authcode;
        //            string serialisedData = JsonConvert.SerializeObject(objSearchRequest);
        //            client.Headers[HttpRequestHeader.ContentType] = "application/json";
        //            System.DateTime dt = DateTime.Now;
        //            var kk = client.UploadString(url, serialisedData);
        //            airContext.flightSearchResponse = JsonConvert.DeserializeObject<FlightSearchResponse>(kk.ToString());
        //            airContext.IsSearchCompleted = true;

        //        }
        //        else
        //        {
        //            airContext.flightSearchResponse = new FlightSearchResponse();
        //            airContext.flightSearchResponse.response.status = TransactionStatus.Error;
        //            airContext.flightSearchResponse.response.message = "Request is empty!";
        //        }
        //    }
        //    catch (Exception exrr)
        //    {
        //        airContext.flightSearchResponse = new FlightSearchResponse();
        //        airContext.flightSearchResponse.response.status = TransactionStatus.Error;
        //        airContext.flightSearchResponse.response.message = "Request is empty!";
        //    }
        //}

        //public FlightBookingResponse CreateBooking(FlightBookingRequest flightBookingRequest , ref StringBuilder sbLogger)
        //{
        //    FlightBookingResponse fbr;
        //    try
        //    {
        //        if (flightBookingRequest != null)
        //        {
        //            WebClient client = new WebClient();
        //            var url = API_URL + "Flights/CreateBooking?authcode=" + authcode;
        //            string serialisedData = JsonConvert.SerializeObject(flightBookingRequest);
        //            #region requestLog
        //            sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original Request---------------------------------------------");
        //            sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
        //            sbLogger.Append(Environment.NewLine + serialisedData);
        //            sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
        //            #endregion
        //            client.Headers[HttpRequestHeader.ContentType] = "application/json";
        //            System.DateTime dt = DateTime.Now;
        //            var kk = client.UploadString(url, serialisedData);
        //            #region responseLog
        //            sbLogger.Append(Environment.NewLine + "---------------------------------------------Booking Original response---------------------------------------------");
        //            sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
        //            sbLogger.Append(Environment.NewLine + kk.ToString());
        //            sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
        //            #endregion
        //            fbr = JsonConvert.DeserializeObject<FlightBookingResponse>(kk.ToString());
        //        }
        //        else
        //        {
        //            fbr = new FlightBookingResponse();
        //            fbr.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, messege = "Request is empty!" };
        //        }
        //    }
        //    catch (Exception exrr)
        //    {
        //        #region Error Log
        //        sbLogger.Append(Environment.NewLine + "--------------------------------------------- Booking Error---------------------------------------------");
        //        sbLogger.Append(Environment.NewLine + "-----------------------" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------");
        //        sbLogger.Append(Environment.NewLine + exrr.ToString());
        //        sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
        //        #endregion
        //        fbr = new FlightBookingResponse();
        //        fbr.responseStatus = new ResponseStatus() { status = TransactionStatus.Error, messege = exrr.ToString() };
        //    }
        //    return fbr;
        //}
        //public ResponseStatus SendMail(SendEmailRequest sendEmailRequest)
        //{
        //    ResponseStatus responseStatus;
        //    try
        //    {
        //        if (sendEmailRequest != null)
        //        {
        //            sendEmailRequest.MailBody = Bal.StringHelper.CompressString(sendEmailRequest.MailBody);
        //            sendEmailRequest.isBodyCompress = true;
        //            WebClient client = new WebClient();
        //            var url = API_URL + "Email/SendMail?authcode=" + authcode;
        //            string serialisedData = JsonConvert.SerializeObject(sendEmailRequest);
        //            client.Headers[HttpRequestHeader.ContentType] = "application/json";
        //            System.DateTime dt = DateTime.Now;
        //            var kk = client.UploadString(url, serialisedData);
        //            if (kk.ToString().ToLower() == "true")
        //            {
        //                responseStatus = new ResponseStatus();
        //            }
        //            else
        //            {
        //                responseStatus = new ResponseStatus() { status = TransactionStatus.Error, messege = "Mail not sent properly!!" };
        //            }
        //        }
        //        else
        //        {
        //            responseStatus = new ResponseStatus() { status = TransactionStatus.Error, messege = "Request is empty!" };
        //        }
        //    }
        //    catch (Exception exrr)
        //    {
        //        responseStatus = new ResponseStatus() { status = TransactionStatus.Error, messege = exrr.ToString() };
        //    }
        //    return responseStatus;
        //}

        public List<Airport> GetCity(string ID)
        {
            if (!FlightUtility.isMasterDataLoaded)
            {
                FlightUtility.LoadMasterData();
            }

            if (!string.IsNullOrEmpty(ID))
            {
                ID = ID.Replace(" ", "").Replace("  ", "");

                List<Airport> resCityCode = FlightUtility.AirportList.Where(x => (x.cityCode.StartsWith(ID, StringComparison.OrdinalIgnoreCase))).ToList();
                List<Airport> ResCityName = FlightUtility.AirportList.Where(x => (x.cityName.Replace(" ", "").StartsWith(ID, StringComparison.OrdinalIgnoreCase))).ToList();
                List<Airport> ResAirportCode = FlightUtility.AirportList.Where(x => (x.airportCode.StartsWith(ID, StringComparison.OrdinalIgnoreCase))).ToList();
                List<Airport> ResAirportName = FlightUtility.AirportList.Where(x => (x.airportName.Replace(" ", "").StartsWith(ID, StringComparison.OrdinalIgnoreCase))).ToList();

                List<Airport> response = new List<Airport>();

                if (ID.Count() >= 4)
                {
                    List<Airport> firstName = resCityCode.Union(ResAirportCode).Union(ResCityName).Union(ResAirportName).Where(x => (x.countryCode.Equals("IN"))).ToList();
                    List<Airport> secondName = resCityCode.Union(ResAirportCode).Union(ResCityName).Union(ResAirportName).Where(x => (!x.countryCode.Equals("IN"))).ToList();
                    response = firstName.Union(secondName).ToList();
                }
                else
                {
                    List<Airport> firstName = resCityCode.Union(ResAirportCode).Union(ResCityName).Union(ResAirportName).Where(x => (x.countryCode.Equals("IN"))).ToList();
                    List<Airport> secondName = resCityCode.Union(ResAirportCode).Union(ResCityName).Union(ResAirportName).Where(x => (!x.countryCode.Equals("IN"))).ToList();
                    response = firstName.Union(secondName).ToList();

                }
                return response.Take(20).ToList();
                //List<Airport> response2 = response.Where(x => x.airportName.ToLower().IndexOf(" all ") != -1).ToList();
                //if (response2.Count > 0)
                //{
                //    List<Airport> response3 = response.Where(n => n.cityCode.Equals(response2[0].cityCode, StringComparison.OrdinalIgnoreCase)).ToList().Union(response.Where(n => !n.cityCode.Equals(response2[0].cityCode, StringComparison.OrdinalIgnoreCase)).ToList()).ToList();
                //    return response3;
                //}
                //else
                //{
                //    return response;
                //}
            }
            else
            {
                List<Airport> response = new List<Airport>();
                response.Add(FlightUtility.GetAirport("DEL"));
                response.Add(FlightUtility.GetAirport("BOM"));
                response.Add(FlightUtility.GetAirport("PAT"));
                response.Add(FlightUtility.GetAirport("BLR"));
                response.Add(FlightUtility.GetAirport("CCU"));
                response.Add(FlightUtility.GetAirport("GOI"));
                response.Add(FlightUtility.GetAirport("HYD"));
                response.Add(FlightUtility.GetAirport("MAA"));
                response.Add(FlightUtility.GetAirport("PNQ"));
                response.Add(FlightUtility.GetAirport("AMD"));
                response.Add(FlightUtility.GetAirport("DXB"));
                response.Add(FlightUtility.GetAirport("BKK"));
                string output = JsonConvert.SerializeObject(response);
                return response;
            }
        }
        public void testWebHook()
        {

            try
            {

                WebClient client = new WebClient();
                var url = "http://localhost:16898/Flight/webhookCallBack";
                string serialisedData = "{\"amount\":\"14755.00\",\"paymentMode\":\"DC\",\"udf5\":\"\",\"udf3\":\"\",\"split_info\":\"\",\"udf4\":\"\",\"udf1\":\"\",\"udf2\":\"\",\"customerName\":\"ROOPESH\",\"productInfo\":\"  quot paymentParts quot quot name quot   quot FlightsMojo quot quot description quot   quot F\",\"customerPhone\":\"9718560832\",\"additionalCharges\":\"\",\"paymentId\":\"14004365791\",\"customerEmail\":\"gaurchitransh73 @gmail.com\",\"merchantTransactionId\":\"TRN1247\",\"error_Message\":\"No Error\",\"notificationId\":\"12345\",\"bankRefNum\":\"126717039448\",\"hash\":\"72a410ea83e36e9980e42df390f36d3a322f48094896d9b9f3aaf8d2bf85021e50ab1aca077e375446212b90704018377f0927ca909968d87798111feac60293\",\"status\":\"Success\",\"field4\":\"2\"}";


                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                System.DateTime dt = DateTime.Now;
                var kk = client.UploadString(url, serialisedData);


            }
            catch (Exception exrr)
            {

            }
        }
    }
    public class LogWriter
    {
        private string m_exePath = string.Empty;
        public LogWriter(string logMessage, string FileName)
        {
            LogWrite(logMessage, FileName);
        }
        public LogWriter(string logMessage, string FileName, string FolderName)
        {
            LogWrite(logMessage, FileName, FolderName);
        }
        public void LogWrite(string logMessage, string FileName)
        {
            //try
            //{
            using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + FileName + ".txt"))
            {
                Log(logMessage, w);
            }
            //}
            //catch (Exception ex)
            //{
            //}
        }
        public void LogWrite(string logMessage, string FileName, string FolderName)
        {
            try
            {
                using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "\\log\\" + FolderName + "\\" + FileName + ".txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void Log(string logMessage, TextWriter txtWriter)
        {
            //try
            //{
            //txtWriter.Write("\r\nLog Entry : ");
            //txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            //    DateTime.Now.ToLongDateString());
            //txtWriter.WriteLine("  :");
            txtWriter.WriteLine("{0}", logMessage);
            //txtWriter.WriteLine("-------------------------------");
            //}
            //catch (Exception ex)
            //{
            //}
        }
    }



    public class LogCreater
    {
        public static void CreateDirectory(string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
            catch
            {

            }
        }
        public static void CreateLogFile(string logMessage, string PathPrefix, string dirName, string fileName)
        {
            try
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + PathPrefix + "\\" + dirName))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + PathPrefix + "\\" + dirName);
                }

                using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + PathPrefix + "\\" + dirName + "\\" + fileName))
                {
                    w.WriteLine("  :{0}", logMessage);
                }
            }
            catch
            {
            }
        }
        public static void CreateLogFile(string logMessage, string PathPrefix, string fileName)
        {
            try
            {
                using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + PathPrefix + "\\" + fileName))
                {
                    w.WriteLine("  :{0}", logMessage);
                }
            }
            catch
            {
            }
        }
        public static void MoveLogFile(string OldPath, string newPath)
        {
            try
            {
                System.IO.Directory.Move(AppDomain.CurrentDomain.BaseDirectory + OldPath, AppDomain.CurrentDomain.BaseDirectory + newPath);
            }
            catch
            {
            }
        }
    }
}
