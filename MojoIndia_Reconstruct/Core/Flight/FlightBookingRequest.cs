using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class FlightBookingRequest
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
        //[DataMember]
        //public string emailID2 { get; set; }
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

        [DataMember]
        public decimal CouponIncreaseAmount { get; set; }

        [DataMember]
        public List<FareRuleResponses> fareRuleResponse { get; set; }
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
        public List<bool> isTickted { get; set; }
        [DataMember]
        public Core.TravelType travelType { get; set; }
        [DataMember]
        public BookingStatus bookingStatus { get; set; }
        [DataMember]
        public PaymentStatus paymentStatus { get; set; }
        [DataMember]
        public string TjBookingID { get; set; }
        [DataMember]
        public string TjReturnBookingID { get; set; }
        [DataMember]
        public decimal VerifiedTotalPrice { get; set; }
        [DataMember]
        public Core.GetWayType gatewayType { get; set; }
        [DataMember]
        public Core.PaymentMode paymentMode { get; set; }
        [DataMember]
        public string GSTNo { get; set; }
        [DataMember]
        public string GSTCompany { get; set; }
        [DataMember]
        public string GSTAddress { get; set; }
        [DataMember]
        public string razorpayOrderID { get; set; }
        [DataMember]
        public string razorpayTransectionID { get; set; }

        [DataMember]
        public decimal convenienceFee { get; set; }

        [DataMember]
        public bool isWhatsapp { get; set; }
        [DataMember]
        public bool isGST { get; set; }

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
        public ResponseStatus responseStatus { get; set; }
        [DataMember]
        public bool isMakeBookingInprogress { get; set; }

        [DataMember]
        public bool isShowFareIncrease { get; set; }
        [DataMember]
        public string FB_booking_token_id { get; set; }
        [DataMember]
        public Affiliate affiliate { get; set; }

        [DataMember]
        public string ResultIndex { get; set; }
        [DataMember]
        public string ST_ResultSessionID { get; set; }

        [DataMember]
        public string STSessionID { get; set; }
        [DataMember]
        public bool isBuyCancellaionPolicy { get; set; }
        [DataMember]
        public bool isBuyRefundPolicy { get; set; }
        [DataMember]
        public string AQ_ticket_id { get; set; }
        [DataMember]
        public decimal RefundPolicyAmt { get; set; }
        [DataMember]
        public decimal CancellaionPolicyAmt { get; set; }
        [DataMember]
        public Device device { get; set; }
        [DataMember]
        public string utm_campaign { get; set; }
        [DataMember]
        public string utm_medium { get; set; }

        [DataMember]
        public string BookingKey { get; set; }
		
		 [DataMember]
        public string tinyUrlID { get; set; }
    }

}
