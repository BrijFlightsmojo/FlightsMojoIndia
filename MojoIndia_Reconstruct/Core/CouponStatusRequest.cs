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
        public CouponType CouponType { get; set; }
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
        public bool isLessConvenceFee { get; set; }
        public ResponseStatus responseStatus { get; set; }
        public CouponStatusResponse()
        {
            responseStatus = new ResponseStatus();
        }
        public string RedirectUrl { get; set; }
    }

    public class CouponDetails
    {
        public ResponseStatus responseStatus { get; set; }

        public List<CouponDetail> couponDetailList { get; set; }
    }
    public class CouponDetail
    {
        public int id { get; set; }
        public string CouponCode { get; set; }
        public int couponAmount { get; set; }
        public CouponAmountType amountType { get; set; }
        public int maxAmount { get; set; }
        public bool isValidateSourceMedia { get; set; }
        public bool isValidateByEmail { get; set; }
        public bool isValidateByCount { get; set; }
        public bool isValidateByNoOfPax { get; set; }
        public bool isValidateByTotalAmt { get; set; }
        public bool isValidateByCabinClass { get; set; }
        public bool isValidateByAirline { get; set; }
        public bool isValidateByBookingDate { get; set; }
        public bool isValidateByTravelDate { get; set; }
        public bool isValidationByUTFCampaign { get; set; }
        public bool isLessConvenceFee { get; set; }
        public bool isValidNoOfCoupon { get; set; }
        public string sourceMedia { get; set; }
        public string emailID { get; set; }
        public int noOfCoupon { get; set; }
        public int noOfPax { get; set; }
        public int totalConsume { get; set; }
        public int minAmount { get; set; }
        public CabinType cabinClass { get; set; }
        public string airline { get; set; }
        public DateTime bookingDateFrom { get; set; }
        public DateTime bookingDateTo { get; set; }
        public DateTime travelDateFrom { get; set; }
        public DateTime travelDateTo { get; set; }
        public string UTFCampaign { get; set; }
        public bool isActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int updatedBy { get; set; }
        public DateTime updatedOn { get; set; }
        public string Counter { get; set; }
        public bool isValidTripType { get; set; }
        public bool isValidTravelType { get; set; }
        public TripType TripType { get; set; }
        public TravelType TravelType { get; set; }


    }
}
