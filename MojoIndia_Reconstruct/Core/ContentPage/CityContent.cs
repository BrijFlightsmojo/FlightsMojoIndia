using System;
using System.Collections.Generic;

namespace Core.ContentPage
{
    public class CityContent
    {
        public int ID { get; set; }
        public SiteId SiteID { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyWord { get; set; }
        public string BannerHeading { get; set; }
        public string DealHeading { get; set; }
        public string AboutCityHeading { get; set; }
        public string AboutCityDescription { get; set; }
        public string CitySubContentHeading1 { get; set; }
        public string CitySubContentHeading2 { get; set; }
        public string CitySubContentHeading3 { get; set; }
        public string CitySubContentHeading4 { get; set; }
        public string CitySubContentHeading5 { get; set; }
        public string CitySubDescription1 { get; set; }
        public string CitySubDescription2 { get; set; }
        public string CitySubDescription3 { get; set; }
        public string CitySubDescription4 { get; set; }
        public string CitySubDescription5 { get; set; }

        public string OriginAirportAddress { get; set; }
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


        public bool isStaticContent { get; set; }
        public bool isActive { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public SearchEnginDetails searchEnginDetails { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public List<WebsiteFareDeal> websiteFareDeal { get; set; }
    }
    public class SearchEnginDetails
    {
        public Core.Flight.Airport origin { get; set; }
        public Core.Flight.Airport destination { get; set; }
      
    }
}

