using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    public class AirContext
    {
        public FlightSearchRequest flightSearchRequest { get; set; }
        public FlightSearchResponse flightSearchResponse { get; set; }
        //public Core.NewResult.NewFlightResponse NewFlightResponse { get; set; }
        public bool IsSearchCompleted { get; set; }
        public bool IsGoToPaymentPage { get; set; }       
        public List<FilterData> filterData { get; set; }
        public List<string> flightRef { get; set; }
        public PriceVerificationRequest priceVerificationRequest { get; set; }
        public PriceVerificationResponse priceVerificationResponse { get; set; }
        public FlightBookingRequest flightBookingRequest { get; set; }
        public FlightBookingResponse flightBookingResponse { get; set; }
        public GoogleFlightDeepLink googleFlightDeepLink { get; set; }       
        public long bookingID { get; set; }
        public bool isDomestic { get; set; }
        public bool IsBookingCompleted { get; set; }
        public int NoOfPage = 10;
        public bool isShowDetails { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsGFMatch { get; set; }

        public bool IsBookingRequestSend { get; set; }
       

        public AirContext()
        {
            CreationTime = DateTime.Now;
        }
        public AirContext(string ip)
        {
            CreationTime = DateTime.Now;
            if (ip == "::1" || ip == "127.0.0.1" || ip == "182.72.103.98"|| ip == "103.160.243.202"|| ip == "150.129.248.16" || ip == "49.249.114.250" || ip == "152.52.22.202")
                isShowDetails = true;
        }
    }
    public class FilterData
    {
        public string resultID { get; set; }
        public int stop { get; set; }
        public decimal price { get; set; }
        public string airline { get; set; }
        public int depTime { get; set; }
        public int retTime { get; set; }
    }
}
