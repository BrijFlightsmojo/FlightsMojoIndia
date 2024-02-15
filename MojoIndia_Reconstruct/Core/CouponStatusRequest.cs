using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class CouponData
    {
        public SiteId stieID { get; set; }
        public int NumberOfCoupon { get; set; }
        public int NoOfUsedCoupon { get; set; }
        public ClientType ClientType { get; set; }
        public string SourceMedia { get; set; }
        //public CouponType CouponType { get; set; }
        public CabinType CabinClass { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
        public decimal AmountPerCoupon { get; set; }
        public decimal CouponAmountPerUnit { get; set; }
        public DateTime TravelFromDate { get; set; }
        public DateTime TravelToDate { get; set; }
        public DateTime ApplicableFromDate { get; set; }
        public DateTime ApplicableToDate { get; set; }
        public int MinimumPax { get; set; }
        public int MaximumPax { get; set; }
    }
    public class CouponStatusRequest
    {
        public SiteId SiteID { get; set; }
        public ClientType clientType { get; set; }
        public string CouponCode { get; set; }
        public CabinType CabinClass { get; set; }
        public string SourceMedia { get; set; }  
        public decimal MCOAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalUnit { get; set; }
        public DateTime TravelDate { get; set; }
      
    }
    public class CouponStatusResponse
    {

        public long BookingID { get; set; }
        public decimal CouponAmount { get; set; }
        public ResponseStatus responseStatus { get; set; }
        public CouponStatusResponse()
        {
            responseStatus = new ResponseStatus();
        }
        public string RedirectUrl { get; set; }
    }
}
