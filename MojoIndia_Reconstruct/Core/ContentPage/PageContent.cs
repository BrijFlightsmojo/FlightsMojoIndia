using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ContentPage
{
    public class PageContent
    {
        public int ID { get; set; }
        public PageType PageID { get; set; }
        public SiteId siteID { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyWord { get; set; }
        public string BannerHeading { get; set; }
        public string DealHeading { get; set; }
        public string AboutHeading { get; set; }
        public string AboutDescription { get; set; }
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
        public bool isStaticContent { get; set; }
        public bool isActive { get; set; }
        public int InsertedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
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
    }
}
