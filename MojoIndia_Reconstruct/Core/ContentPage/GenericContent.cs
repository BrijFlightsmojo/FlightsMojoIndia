using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Core.ContentPage
{
	public class GenericContent
	{
		public int ID_GenericContent { get; set; }
		public SiteId ID_Site { get; set; }
		public ProductId ID_Product { get; set; }
		public string GenericCode { get; set; }
		public string GenericName { get; set; }
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
		public string AboutHeading { get; set; }
		public string AboutDescription { get; set; }
		public string Heading1 { get; set; }
		public string Heading1Description { get; set; }
		public string Heading2 { get; set; }
		public string Heading2Description { get; set; }
		public bool isActivePromotional { get; set; }
		public string PromotionalHeader { get; set; }
		public string PromotionalContent { get; set; }
		public string PromotionalCouponCode { get; set; }
		public double PromotionalInstantOff { get; set; }
		public string PromotionalReview { get; set; }
		public string PopularDealID { get; set; }
		public bool isStaticContent { get; set; }
		public bool isActive { get; set; }
		public int InsertedBy { get; set; }
		public string InsertedByName { get; set; }
		public DateTime InsertedOn { get; set; }
		public int ModifiedBy { get; set; }
		public string ModifiedByName { get; set; }
		public DateTime ModifiedOn { get; set; }

		public ResponseStatus ResponseStatus { get; set; }
		public string ActionType { get; set; }

		public List<WebsiteFareDeal> websiteFareDeal { get; set; }
	}
}

