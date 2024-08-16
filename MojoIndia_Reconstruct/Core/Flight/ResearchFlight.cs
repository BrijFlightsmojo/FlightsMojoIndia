using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class ResearchFlight
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
        [DataMember]
        public SiteId siteID { get; set; }
        [DataMember]
        public string sourceMedia { get; set; }
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
        public decimal CouponIncreaseAmount { get; set; }

        [DataMember]
        public Fare sumFare { get; set; }

        [DataMember]
        public List<bool> isTickted { get; set; }
        [DataMember]
        public Core.TravelType travelType { get; set; }
        [DataMember]
        public BookingStatus bookingStatus { get; set; }

        [DataMember]
        public string TjBookingID { get; set; }
        [DataMember]
        public string TjReturnBookingID { get; set; }
        [DataMember]
        public decimal VerifiedTotalPrice { get; set; }

        [DataMember]
        public ResponseStatus responseStatus { get; set; }
        [DataMember]
        public Affiliate affiliate { get; set; }

        [DataMember]
        public Device device { get; set; }
        [DataMember]
        public string utm_campaign { get; set; }
        [DataMember]
        public string utm_medium { get; set; }

        [DataMember]
        public DateTime TravelDate { get; set; }
        [DataMember]
        public DateTime? ReturnDate { get; set; }

        [DataMember]
        public CabinType CabinType { get; set; }
        [DataMember]
        public TripType TripType { get; set; }
        [DataMember]
        public string Origin { get; set; }
        [DataMember]
        public string Destination { get; set; }


    //        public string Provider { get; set; }
    //        public string Booking_Date_Time { get; set; }
    //        public string Total_Amount { get; set; }
    //        public string SiteID { get; set; }
    //        public string Source_Media { get; set; }
    //        public string Product_Type { get; set; }
    //        public string MobileNo { get; set; }
    //        public string PhoneNo
    //{ get; set; }
    //        public string EmailID { get; set; }
    //        public string TripType { get; set; }
    //        public string CabinClass { get; set; }
    //        
    //
    //ValCarrier
    //TravelDate
    //ReturnDate
    //PaxFirstName
    //PaxMiddleName
    //PaxLastName
    //adult
    //child
    //infant
    //infantWs
    //AirlineLocator
    //TicketionPCC
    //SubStatus
    //outEft
    //InEft
    //MarkupID
    //FareType
    //FareTypeReturn
    //TravelType

    //ModifyBy
    //ModifyOn
    //userSessionID
    //searchID
    //EFT
    //isNearByResult
    //isFlexiResult
    //AssignDate
    //CancelDate
    //InsuranceID
    //TravelAssistance
    //CancellaionPolicy
    //FlexibleTicket
    //CouponCode
    //UserIP
    //BrowserDetails
    //OBSupplier
    //IBSupplier
    //SupplierMco
    //IsWhatsapp
    //MojofareType
    //MojofareTypeReturn
    //MarkupRule
    //device
    //utm_campaign
    //utm_medium
}
}
