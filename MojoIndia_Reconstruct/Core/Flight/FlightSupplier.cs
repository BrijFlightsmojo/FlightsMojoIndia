using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    public class Affiliate
    {
        public string AffiliateId { get; set; }
        public string AffiliateName { get; set; }
        public string EmiConFee { get; set; }
        public string PayLaterConFee { get; set; }
        public string WalletConFee { get; set; }
        public string NetBankingConFee { get; set; }
        public string CardConFee { get; set; }
        public string UPIConFee { get; set; }
        public SiteId SiteID { get; set; }

    }
    //public class FlightSupplier
    //{
    //    public int Id { get; set; }
    //    public SiteId siteId { get; set; }
    //    public GdsType Provider { get; set; }
    //    public List<string> SourceMedia { get; set; }
    //    public List<string> SourceMedia_Not { get; set; }
    //    public List<string> FromCountry { get; set; }
    //    public List<string> FromCountry_Not { get; set; }
    //    public List<string> ToCountry { get; set; }
    //    public List<string> ToCountry_Not { get; set; }
    //    public FareType FareType { get; set; }
    //    //public string Currency { get; set; }
    //    public int FarePrioritySequence { get; set; }
    //    public int CustomerType { get; set; }
    //    public bool isMeta { get; set; }
    //    public FlightSupplier()
    //    {
           
    //    }
    //}
}
