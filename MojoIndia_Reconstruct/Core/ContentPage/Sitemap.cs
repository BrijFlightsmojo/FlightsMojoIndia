using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ContentPage
{
    public class Sitemap
    {
        public List<OriginDestinationContent> OnD { get; set; }
        public List<CityContent> cityContent { get; set; }
        public List<AirlineContent> AirlineContent { get; set; }
        public List<DealsContent> DealsContent { get; set; }
    }

    public class SitemapNode
    {
        public SitemapFrequency? Frequency { get; set; }
        public DateTime? LastModified { get; set; }
        public double? Priority { get; set; }
        public string Url { get; set; }
    }

    public enum SitemapFrequency
    {
        Never,
        Yearly,
        Monthly,
        Weekly,
        Daily,
        Hourly,
        Always
    }
}
