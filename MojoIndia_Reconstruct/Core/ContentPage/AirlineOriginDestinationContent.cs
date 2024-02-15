using System;
using System.Collections.Generic;

namespace Core.ContentPage
{
    public class AirlineOriginDestinationContent
	{
		public int ID_AirlineOriginDestinationContent { get; set; }
        public SiteId ID_Site { get; set; }
        public ProductId ID_Product { get; set; }
		public string AirlineName { get; set; }
		public string AirlineCode { get; set; }
		public string OriginName { get; set; }
		public string OriginCode { get; set; }
		public string DestinationName { get; set; }
		public string DestinationCode { get; set; }        
        public int SearchEngineType { get; set; }
        public string SearchEngineFrom { get; set; }
        public string SearchEngineTo { get; set; }
        public string SearchEngineAirline { get; set; }
        public int SearchEngineCabin { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyWord { get; set; }
        public string BannerHeading { get; set; }
        public string DealHeading { get; set; }
        public string AboutODHeading { get; set; }
        public string AboutODDescription { get; set; }
		public string Heading1 { get; set; }
        public string Heading1Description { get; set; }
        public string Heading2 { get; set; }
        public string Heading2Description { get; set; }
		public string DomesticRoute { get; set; }
		public List<DomesticRoute> DomesticRouteList { get; set; }
		public string InternationalRoute { get; set; }
		public List<InternationalRoute> InternationalRouteList { get; set; }
		public string OtherPopularAirline { get; set; }		
		public bool isActivePromotional { get; set; }
        public string PromotionalHeader { get; set; }
        public string PromotionalContent { get; set; }
        public string PromotionalCouponCode { get; set; }
        public double PromotionalInstantOff { get; set; }
        public string PromotionalReview { get; set; }
		public bool isStaticContent { get; set; }
		public bool isActive { get; set; }
        public int InsertedBy { get; set; }
        public string InsertedByName { get; set; }
        public DateTime InsertedOn { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime ModifiedOn { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

		public List<WebsiteFareDeal> websiteFareDeal { get; set; }
	}

	public class DomesticRoute
	{
		public string AirlineCode { get; set; }
		public string OriginName { get; set; }
		public string OriginCode { get; set; }
		public string DestinationName { get; set; }
		public string DestinationCode { get; set; }
	}

	public class InternationalRoute
	{
		public string AirlineCode { get; set; }
		public string OriginName { get; set; }
		public string OriginCode { get; set; }
		public string DestinationName { get; set; }
		public string DestinationCode { get; set; }
	}
}

