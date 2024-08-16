using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.RefineResult
{

    public class ResultResponse
    {
        public List<AirlineList> airlineList { get; set; }
        public List<Stop> stop { get; set; }
        public List<Result> result { get; set; }
        public List<Result> resultReturn { get; set; }
        public string depCity { get; set; }
        public string retCity { get; set; }
        public string depDate { get; set; }
        public string retDate { get; set; }
        public List<int> gdsList { get; set; }
        public bool isReturn { get; set; }
        public int minPrice { get; set; }
        public int maxPrice { get; set; }
        public List<int> depTimeDur { get; set; }
        public List<int> arrTimeDur { get; set; }
        public Flight.FlightSearchRequest request { get; set; }
        public string flexiFare { get; set; }
        public string nearByFare { get; set; }
        public string nonStopFare { get; set; }
        public bool isShowDetails { get; set; }
        public string Currency { get; set; }
        public string redirectURl { get; set; }
        public int totpax { get; set; }
        public ResultResponse()
        {
            airlineList = new List<AirlineList>();
            stop = new List<Stop>();
            result = new List<Result>();
            gdsList = new List<int>();
            depTimeDur = new List<int>();
            depTimeDur.Add(1);
            depTimeDur.Add(2);
            depTimeDur.Add(3);
            depTimeDur.Add(4);

            arrTimeDur = new List<int>();
            arrTimeDur.Add(1);
            arrTimeDur.Add(2);
            arrTimeDur.Add(3);
            arrTimeDur.Add(4);
            flexiFare = "0";
            nearByFare = "0";
            nonStopFare = "0";
        }
    }
    public class AirlineList
    {
        public string code { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public decimal fare { get; set; }
    }
    public class TimeZone
    {
        public string name { get; set; }
    }
    public class Stop
    {
        public decimal price { get; set; }
        public int stop { get; set; }
    }
    public class Result
    {
        public string GDS { get; set; }
        public string ValCar { get; set; }
        public string resultID { get; set; }
        public decimal price { get; set; }
        //public string dPrice { get; set; }
        public string CutPrice { get; set; }
        public string CPrice { get; set; }
        public string sPrice { get; set; }
        public decimal dPrice { get; set; }
        public string bFare { get; set; }
        public string tax { get; set; }
        public decimal maxSeat { get; set; }
        public string airlineCode { get; set; }
        public int stop { get; set; }     
        public List<FlightSegment> flightSegments { get; set; }
        public int depTimeDur { get; set; }
        public int arrTimeDur { get; set; }
        public string fareType { get; set; }
        public string airlineClass { get; set; }
        public List<string> markupID { get; set; }
        public string mojofare { get; set; }
        public int TotPax { get; set; }
        public string dealType { get; set; }
        public int SeatAvailable { get; set; }
        public int subProvider { get; set; }
    }
  
    public class FlightSegment
    {
        public string segName { get; set; }
        public int Eft { get; set; }
        public string LayoverTime { get; set; }
        public string TotalTime { get; set; }
        public List<Segment> segments { get; set; }
    }
    public class Segment
    {
        public Airline airline { get; set; }
        public string url { get; set; }
        public string flightNo { get; set; }
        public string equipType { get; set; }
        public string equipDesc { get; set; }
        public Airport org { get; set; }
        public string fTerminal { get; set; }
        public string fFullAprName { get; set; }
        public string depDate { get; set; }
        public string depTime { get; set; }
        public Airport dest { get; set; }
        public string tTerminal { get; set; }
        public string tFullAprName { get; set; }
        public string arrDate { get; set; }
        public string arrTime { get; set; }
        public Airline operatedBy { get; set; }
        public int eft { get; set; }
        public int stop { get; set; }
        public string cabinType { get; set; }
        public string baggage { get; set; }
        public string layOverTime { get; set; }
        public bool isNearByOrg { get; set; }
        public bool isNearByDest { get; set; }
        public bool isFlexiFare { get; set; }
		public string fTerShort { get; set; }
		 public string tTerShort { get; set; }
    }
    public class Airline
    {
        public string code { get; set; }
        public string name { get; set; }
    }
    public class Airport
    {
        public string airportName { get; set; }
        public string airportCode { get; set; }
        public string cityName { get; set; }
    }
}
