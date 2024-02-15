using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ContentPage
{
    public class DealsContent
    {
        public int ID { get; set; }
        public int SiteID { get; set; }
        public string ThemeCode { get; set; }
        public string ThemeName { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyWord { get; set; }
        public string BannerHeading { get; set; }
        public string DealHeading { get; set; }
        public string AboutThemeHeading { get; set; }
        public string AboutThemeDescription { get; set; }
        public string ThemeSubContentHeading1 { get; set; }
        public string ThemeSubContentHeading2 { get; set; }
        public string ThemeSubContentHeading3 { get; set; }
        public string ThemeSubContentHeading4 { get; set; }
        public string ThemeSubContentHeading5 { get; set; }
        public string ThemeSubDescription1 { get; set; }
        public string ThemeSubDescription2 { get; set; }
        public string ThemeSubDescription3 { get; set; }
        public string ThemeSubDescription4 { get; set; }
        public string ThemeSubDescription5 { get; set; }
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
        public ResponseStatus ResponseStatus { get; set; }
        public List<WebsiteFareDeal> websiteFareDeal { get; set; }
    }
}
