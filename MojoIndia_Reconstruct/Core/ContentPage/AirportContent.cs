using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Core.ContentPage
{
	public class AirportContentSummeryDetails
	{
		public List<AirportContentSummery> data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }

	}
	public class AirportContentSummery
	{
		public string ID { get; set; }
		public string AirportCode { get; set; }
		public string AirportName { get; set; }
		public string WebsiteID { get; set; }
		public string PageType { get; set; }
		public string ModifyBy { get; set; }
		public string ModifyOn { get; set; }
		public string isActive { get; set; }
	}
	public class AirportContent
    {
        public string ID { get; set; }
        public string AirportCode { get; set; }
        public string AirportName { get; set; }
        public string WebsiteID { get; set; }
        public string PageType { get; set; }
        public string SearchEngineType { get; set; }
        public string SearchEngineFrom { get; set; }
        public string SearchEngineTo { get; set; }
        public string SearchEngineAirline { get; set; }
        public string SearchEngineCabin { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyWord { get; set; }
        public string Header1 { get; set; }
        public string Header2 { get; set; }
        public string Header3 { get; set; }
        public string Header4 { get; set; }
        public string Header5 { get; set; }
        public string Header6 { get; set; }
        public string PopularAirlineFrom { get; set; }
        public string PopularAirlineTo { get; set; }
        public string PopularDestination { get; set; }
        public string NearByAirportFrom { get; set; }
        public string NearByAirportTo { get; set; }
        public string PageContent { get; set; }
        public string SubContent1 { get; set; }
        public string SubContent2 { get; set; }
        public string SubContent3 { get; set; }
        public string SubContent4 { get; set; }
        public string SubContent5 { get; set; }
        public string SubContent6 { get; set; }
        public string SubContent7 { get; set; }
        public string isActivePromotional { get; set; }
        public string PromotionalHeader { get; set; }
        public string PromotionalContent { get; set; }
        public string PromotionalCouponCode { get; set; }
        public string PromotionalInstantOff { get; set; }
        public string PromotionalReview { get; set; }
        public string AirlineServingByAirport { get; set; }
        public string PopularDealID { get; set; }
        public string isStaticContent { get; set; }
        public string isActive { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyOn { get; set; }

        public  ResponseStatus ResponseStatus { get; set; }
        public string ActionType { get; set; }
    }
}

