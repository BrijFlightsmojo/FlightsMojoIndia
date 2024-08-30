using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bal
{
    public class BalCoupon
    {
        public void ValidateCoupon(Core.CouponStatusRequest Request, ref CouponStatusResponse Response)
        {
            try
            {
                CouponData cd = new DAL.dalCoupon().GetCouponDetail(Request);
                if (cd != null)
                {
                    if (!IsBetween((cd.NoOfUsedCoupon + 1), 0, cd.NumberOfCoupon))
                    {
                        Response.responseStatus.message = "Invalide coupon, Please try another coupon.";
                        Response.responseStatus.status = TransactionStatus.Error;
                    }
                    else if (cd.stieID != 0 && Request.SiteID != cd.stieID)
                    {
                        Response.responseStatus.message = "Invalide coupon, Please try another coupon.";
                        Response.responseStatus.status = TransactionStatus.Error;
                    }
                    else if (cd.ClientType != ClientType.None && Request.clientType != cd.ClientType)
                    {
                        Response.responseStatus.message = "Invalide coupon, Please try another coupon.";
                        Response.responseStatus.status = TransactionStatus.Error;
                    }
                    else if (!IsBetween(Request.TravelDate, cd.TravelFromDate, cd.TravelToDate))
                    {
                        Response.responseStatus.message = "Invalide coupon, Please try another coupon.";
                        Response.responseStatus.status = TransactionStatus.Error;
                    }
                    else if (!IsBetween(DateTime.Today, cd.ApplicableFromDate, cd.ApplicableToDate))
                    {
                        Response.responseStatus.message = "Invalide coupon, Please try another coupon.";
                        Response.responseStatus.status = TransactionStatus.Error;
                    }
                    else if (!string.IsNullOrEmpty(cd.SourceMedia) && cd.SourceMedia.IndexOf(Request.SourceMedia) == -1)
                    {
                        Response.responseStatus.message = "Invalide coupon, Please try another coupon.";
                        Response.responseStatus.status = TransactionStatus.Error;
                    }
                    else if (!IsBetween(Request.TotalUnit, cd.MinimumPax, cd.MaximumPax))
                    {
                        Response.responseStatus.message = "Invalide coupon, Please try another coupon.";
                        Response.responseStatus.status = TransactionStatus.Error;
                    }
                    else if (!IsBetween(Request.TotalAmount, cd.MinimumAmount, cd.MaximumAmount))
                    {
                        Response.responseStatus.message = "Invalide coupon, Please try another coupon.";
                        Response.responseStatus.status = TransactionStatus.Error;
                    }
                    if (Response.responseStatus.status == TransactionStatus.Success)
                    {
                        if (cd.CouponType == CouponType.PerBooking)
                        {
                            Response.CouponAmount = cd.AmountPerCoupon;
                        }
                        else if (cd.CouponType == CouponType.PerUnit)
                        {
                            Response.CouponAmount = cd.CouponAmountPerUnit * Request.TotalUnit;
                        }
                        if (Response.CouponAmount <= 0)
                        {
                            Response.responseStatus.status = TransactionStatus.Error;
                            Response.responseStatus.message = "Invalide coupon, Please try another coupon.";
                        }
                        else
                        {
                            Response.responseStatus.message = "Coupon is successfully applied";
                        }
                    }
                }
                else
                {
                    Response.responseStatus.status = TransactionStatus.Error;
                    Response.responseStatus.message = "Invalide coupon, Please try another coupon.";
                }
            }
            catch (Exception ex)
            {
                Response.responseStatus.status = TransactionStatus.Error;
                Response.responseStatus.message = "Invalide coupon, Please try another coupon.";
            }
        }
        public bool IsBetween<T>(T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }
    }
}
