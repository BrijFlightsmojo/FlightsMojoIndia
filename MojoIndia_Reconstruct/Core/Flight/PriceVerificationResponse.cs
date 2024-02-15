using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class PriceVerificationResponse
    {
        [DataMember]
        public FareQuoteResponse fareQuoteResponse { get; set; }
        [DataMember]
        public List<FareRuleResponses> fareRuleResponse { get; set; }
        [DataMember]
        public ResponseStatus responseStatus { get; set; }
    }
    [DataContract]
    public class FareQuoteResponse
    {
        [DataMember]
        public List<FlightResult> flightResult { get; set; }
        [DataMember]
        public bool isFareChange { get; set; }
        [DataMember]
        public decimal fareIncreaseAmount { get; set; }
        //[DataMember]
        //public List<Fare> Newfare { get; set; }
        [DataMember]
        public ResponseStatus responseStatus { get; set; }
        [DataMember]
        public int ErrorCode { get; set; }
        [DataMember]
        public bool IsGSTMandatory { get; set; }
        [DataMember]
        public string TjBookingID { get; set; }
        [DataMember]
        public string TjReturnBookingID { get; set; }
        [DataMember]
        public decimal VerifiedTotalPrice { get; set; }
        [DataMember]
        public List<string> tgy_Flight_Key { get; set; }
        [DataMember]
        public List<bool> tgy_Block_Ticket_Allowed { get; set; }

        [DataMember]
        public bool isRunFareQuoteFalse { get; set; }


        [DataMember]
        public string STSessionID { get; set; }
    }
    [DataContract]
    public class FareRuleResponses
    {
        [DataMember]
        public List<FareRule> FareRules { get; set; }

    }
    [DataContract]
    public class FareRule
    {
        [DataMember]
        public string Airline { get; set; }
        //[DataMember]
        //public DateTime DepartureTime { get; set; }
        [DataMember]
        public string Destination { get; set; }
        [DataMember]
        public string FareBasisCode { get; set; }
        [DataMember]
        public object FareRestriction { get; set; }
        [DataMember]
        public string FareRuleDetail { get; set; }
        //[DataMember]
        //public int FlightId { get; set; }
        [DataMember]
        public string Origin { get; set; }
        //[DataMember]
        //public DateTime ReturnDate { get; set; }
    }

}
