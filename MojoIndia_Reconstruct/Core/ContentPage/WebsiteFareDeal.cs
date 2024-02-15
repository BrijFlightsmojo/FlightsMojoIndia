using Core.Flight;
using System;

namespace Core.ContentPage
{
	public class WebsiteFareDeal
    {
        public Airport origin { get; set; }
        public Airport destination { get; set; }
        public DateTime depDate { get; set; }
        public DateTime retDate { get; set; }
        public Airline airline { get; set; }
        public string tripType { get; set; }
        public string cabinClass { get; set; }
        public string totalFare { get; set; }
    }
    public class WebsiteCustomDeal
    {
        public Airport origin { get; set; }
        public Airport destination { get; set; }
        public string depDate { get; set; }
        //public string depDateCal { get; set; }
        public string retDate { get; set; }
        //public string retDateCal { get; set; }
        public Airline airline { get; set; }
        public string tripType { get; set; }
        public string cabinClass { get; set; }
        public string totalFare { get; set; }
        public string deepLink { get; set; }
    }
}
