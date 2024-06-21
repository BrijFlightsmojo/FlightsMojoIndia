using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class FlightBookingResponse
    {
        [DataMember]
        public string userSearchID { get; set; }
        [DataMember]
        public string userLogID { get; set; }
        [DataMember]
        public string userSessionID { get; set; }
        [DataMember]
        public string userIP { get; set; }
        [DataMember]
        public string BrowserDetails { get; set; }
        [DataMember]
        public long bookingID { get; set; }
        [DataMember]
        public int prodID { get; set; }
        [DataMember]
        public long transactionID { get; set; }
        [DataMember]
        public string redirectID { get; set; }
        [DataMember]
        public List<FlightResult> flightResult { get; set; }
        //[DataMember]
        //public List<FlightResult> bookResult { get; set; }
        [DataMember]
        public List<string> PriceID { get; set; }
        [DataMember]
        public List<FareType> FareTypeList { get; set; }
        [DataMember]
        public int adults { get; set; }
        [DataMember]
        public int child { get; set; }
        [DataMember]
        public int infants { get; set; }
        [DataMember]
        public int infantsWs { get; set; }
        [DataMember]
        public PaymentDetails paymentDetails { get; set; }
        [DataMember]
        public List<PassengerDetails> passengerDetails { get; set; }
        [DataMember]
        public decimal updatedBookingAmount { get; set; }
        [DataMember]
        public string phoneNo { get; set; }
        [DataMember]
        public string mobileNo { get; set; }
        [DataMember]
        public string emailID { get; set; }
        [DataMember]
        public SiteId siteID { get; set; }
        [DataMember]
        public string sourceMedia { get; set; }
        [DataMember]
        public string currencyCode { get; set; }
        [DataMember]
        public List<Airline> airline { get; set; }
        [DataMember]
        public List<Airport> airport { get; set; }
        [DataMember]
        public List<AircraftDetail> aircraftDetail { get; set; }
        [DataMember]
        public string deepLink { get; set; }
        [DataMember]
        public DateTime LastCheckInDate { get; set; }
        [DataMember]
        public string CouponCode { get; set; }
        [DataMember]
        public decimal CouponAmount { get; set; }
        [DataMember]
        public int AdminID { get; set; }
        [DataMember]
        public string TvoTraceId { get; set; }
        [DataMember]
        public bool isFareChange { get; set; }
        [DataMember]
        public decimal fareIncreaseAmount { get; set; }
        //[DataMember]
        //public List<Fare> Newfare { get; set; }
        [DataMember]
        public Fare sumFare { get; set; }


        [DataMember]
        public long TvoBookingID { get; set; }
        [DataMember]
        public long TvoReturnBookingID { get; set; }
        [DataMember]
        public string PNR { get; set; }
        [DataMember]
        public string ReturnPNR { get; set; }
        [DataMember]
        public BookingStatus bookingStatus { get; set; }
        [DataMember]
        public PaymentStatus paymentStatus { get; set; }
        [DataMember]
        public List<bool> isTickted { get; set; }
        [DataMember]
        public ResponseStatus responseStatus { get; set; }

        //[DataMember]//Use for kayak S2S Pixiel
        //public string kayakClickId { get; set; }
        //[DataMember]//Use for wego S2S Pixiel
        //public string wegoClickId { get; set; }
        [DataMember]
        public List<Invoice> invoice { get; set; }
        [DataMember]
        public string TjBookingID { get; set; }
        [DataMember]
        public string TjReturnBookingID { get; set; }
        [DataMember]
        public decimal VerifiedTotalPrice { get; set; }
        [DataMember]
        public string GSTNo { get; set; }
        [DataMember]
        public string GSTCompany { get; set; }
        [DataMember]
        public Core.GetWayType gatewayType { get; set; }
        [DataMember]
        public Core.PaymentMode paymentMode { get; set; }
        [DataMember]
        public string razorpayOrderID { get; set; }
        [DataMember]
        public string razorpayTransectionID { get; set; }
        [DataMember]
        public decimal conveniencefee { get; set; }
        [DataMember]
        public bool isWhatsapp { get; set; }
        [DataMember]
        public string tgy_Search_Key { get; set; }

        [DataMember]
        public string tgy_Request_id { get; set; }
        [DataMember]
        public List<string> tgy_Flight_Key { get; set; }
        [DataMember]
        public List<bool> tgy_Block_Ticket_Allowed { get; set; }
        [DataMember]
        public string tgy_Booking_RefNo { get; set; }

        [DataMember]
        public decimal CouponIncreaseAmount { get; set; }
        [DataMember]
        public string Fb_Reference_id { get; set; }
        [DataMember]
        public Affiliate affiliate { get; set; }

        [DataMember]
        public bool isBuyCancellaionPolicy { get; set; }
        [DataMember]
        public string isBuyRefundPolicy { get; set; }

        [DataMember]
        public decimal RefundPolicyAmt { get; set; }
        [DataMember]
        public decimal CancellaionPolicyAmt { get; set; }
        [DataMember]
        public Device device { get; set; }
        public FlightBookingResponse()
        {

        }

        public FlightBookingResponse(FlightBookingRequest fbr)
        {
            userSearchID = fbr.userSearchID;
            userSessionID = fbr.userSessionID;
            userIP = fbr.userIP;
            BrowserDetails = fbr.BrowserDetails;
            bookingID = fbr.bookingID;
            prodID = fbr.prodID;
            transactionID = fbr.transactionID;
            flightResult = fbr.flightResult;
            //bookResult = fbr.bookResult;
            PriceID = fbr.PriceID;
            adults = fbr.adults;
            child = fbr.child;
            infants = fbr.infants;
            infantsWs = fbr.infantsWs;
            paymentDetails = fbr.paymentDetails;
            passengerDetails = fbr.passengerDetails;
            updatedBookingAmount = fbr.updatedBookingAmount;
            phoneNo = fbr.phoneNo;
            mobileNo = fbr.mobileNo;
            emailID = fbr.emailID;
            siteID = fbr.siteID;
            sourceMedia = fbr.sourceMedia;
            currencyCode = fbr.currencyCode;
            deepLink = fbr.deepLink;
            LastCheckInDate = fbr.LastCheckInDate;
            CouponCode = fbr.CouponCode;
            CouponAmount = fbr.CouponAmount;
            AdminID = fbr.AdminID;
            TvoTraceId = fbr.TvoTraceId;
            isFareChange = fbr.isFareChange;
            fareIncreaseAmount = fbr.fareIncreaseAmount;
            sumFare = fbr.sumFare;
            TvoBookingID = fbr.TvoBookingID;
            TvoReturnBookingID = fbr.TvoReturnBookingID;
            PNR = fbr.PNR;
            ReturnPNR = fbr.ReturnPNR;
            bookingStatus = fbr.bookingStatus;
            paymentStatus = fbr.paymentStatus;
            isTickted = new List<bool>();
            responseStatus = new ResponseStatus();
            invoice = new List<Invoice>();
            TjBookingID = fbr.TjBookingID;
            TjReturnBookingID = fbr.TjReturnBookingID;
            razorpayOrderID = fbr.razorpayOrderID;
            conveniencefee = fbr.convenienceFee;
            razorpayTransectionID = fbr.razorpayTransectionID;
            paymentMode = fbr.paymentMode;
            isWhatsapp = fbr.isWhatsapp;
            tgy_Block_Ticket_Allowed = fbr.tgy_Block_Ticket_Allowed;
            tgy_Booking_RefNo = fbr.tgy_Booking_RefNo;
            tgy_Flight_Key = fbr.tgy_Flight_Key;
            tgy_Request_id = fbr.tgy_Request_id;
            tgy_Search_Key = fbr.tgy_Search_Key;
            RefundPolicyAmt = fbr.RefundPolicyAmt;
            CancellaionPolicyAmt = fbr.CancellaionPolicyAmt;
            device = fbr.device;
        }
    }
    [DataContract]
    public class Invoice
    {
        [DataMember]
        public decimal InvoiceAmount { get; set; }
        [DataMember]
        public string InvoiceNo { get; set; }
    }
}
