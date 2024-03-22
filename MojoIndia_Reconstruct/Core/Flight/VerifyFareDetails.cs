using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    public class VerifyFareDetails
    {
        public string userSearchID { get; set; }
        public DateTime FirstSearchDate { get; set; }
        public DateTime SecondSearchDate { get; set; }
        public string FirstSearchFareID { get; set; }
        public string SeconSearchFareID { get; set; }
        public string TripjackBookingID { get; set; }
        public decimal PreviousAmt { get; set; }
        public decimal newAmt { get; set; }

        public string Origin { get; set; }
        public string Destination { get; set; }
    }
   
    public class GfPriceVerifyResponse
    {        
        public ResponseStatus responseStatus { get; set; }        
        public Fare fare { get; set; }
    }
   
    public class GfPriceVerifyRequest
    {        
        public int adults { get; set; }        
        public int child { get; set; }        
        public int infants { get; set; }        
        public int infantsWs { get; set; }        
        public List<FlightResult> flightResult { get; set; }
    }
}
