using System;
using System.Collections.Generic;
using Core;

using Core.Flight;

namespace Core.ContentPage
{
    public class OriginDestinationContent
    {
        public string OriginCode { get; set; }
        public string OriginName { get; set; }
        public string DestinationCode { get; set; }
        public string DestinationName { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyWord { get; set; }
        public string BannerHeading { get; set; }
        public string DealHeading { get; set; }
        public string AboutODHeading { get; set; }
        public string AboutODDescription { get; set; }
        public bool isStaticContent { get; set; }
        public string Heading1 { get; set; }
        public string Heading2 { get; set; }
        public string Heading3 { get; set; }
        public string Heading4 { get; set; }
        public string Heading5 { get; set; }
        public string Heading1Description { get; set; }
        public string Heading2Description { get; set; }
        public string Heading3Description { get; set; }
        public string Heading4Description { get; set; }
        public string Heading5Description { get; set; }

        public string OriginAirportAddress { get; set; }
        public string DestinationAirportAddress { get; set; }
        public string FAQQ1 { get; set; }
        public string FAQQ2 { get; set; }
        public string FAQQ3 { get; set; }
        public string FAQQ4 { get; set; }
        public string FAQQ5 { get; set; }
        public string FAQQ6 { get; set; }
        public string FAQQ7 { get; set; }
        public string FAQQ8 { get; set; }
        public string FAQQ9 { get; set; }
        public string FAQQ10 { get; set; }
        public string FAQQ11 { get; set; }
        public string FAQQ12 { get; set; }
        public string FAQQ13 { get; set; }
        public string FAQQ14 { get; set; }
        public string FAQQ15 { get; set; }
        public string FAQANS1 { get; set; }
        public string FAQANS2 { get; set; }
        public string FAQANS3 { get; set; }
        public string FAQANS4 { get; set; }
        public string FAQANS5 { get; set; }
        public string FAQANS6 { get; set; }
        public string FAQANS7 { get; set; }
        public string FAQANS8 { get; set; }
        public string FAQANS9 { get; set; }
        public string FAQANS10 { get; set; }
        public string FAQANS11 { get; set; }
        public string FAQANS12 { get; set; }
        public string FAQANS13 { get; set; }
        public string FAQANS14 { get; set; }
        public string FAQANS15 { get; set; }

        public List<OriginDestinationContent> OnD { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<WebsiteFareDeal> websiteFareDeal { get; set; }
        public SearchEnginDetails searchEnginDetails { get; set; }


        public List<WebsiteFareDealInt> websiteFareDealInt { get; set; }


        public List<Airline> airline { get; set; }     
        public List<Airport> airport { get; set; }      
        public List<AircraftDetail> aircraftDetail { get; set; }
        public List<FlightResult> flightResult { get; set; }

        public DateTime Created { get; set; }
    }
}

