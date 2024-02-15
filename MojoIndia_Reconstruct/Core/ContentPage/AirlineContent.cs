using System;
using System.Collections.Generic;

namespace Core.ContentPage
{
    public class AirlineContent
    {
        public int ID { get; set; }
        public SiteId SiteID { get; set; }
        public string AirlineCode { get; set; }
        public string AirlineName { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyWord { get; set; }
        public string BannerHeading { get; set; }
        public string DealHeading { get; set; }
        public string AboutAirlineHeading { get; set; }
        public string AboutAirlineDescription { get; set; }
        public string AirlineSubContentHeading1 { get; set; }
        public string AirlineSubContentHeading2 { get; set; }
        public string AirlineSubContentHeading3 { get; set; }
        public string AirlineSubContentHeading4 { get; set; }
        public string AirlineSubContentHeading5 { get; set; }
        public string AirlineSubDescription1 { get; set; }
        public string AirlineSubDescription2 { get; set; }
        public string AirlineSubDescription3 { get; set; }
        public string AirlineSubDescription4 { get; set; }
        public string AirlineSubDescription5 { get; set; }
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
        public bool isStaticContent { get; set; }
        public bool isActive { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public SearchEnginDetails searchEnginDetails { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}

