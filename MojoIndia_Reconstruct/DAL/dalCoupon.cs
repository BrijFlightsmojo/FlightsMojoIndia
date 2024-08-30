using Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalCoupon
    {
        public CouponData GetCouponDetail(CouponStatusRequest couponStatusRequest)
        {
            CouponData cd = null;
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@CouponCode", SqlDbType.VarChar, 50);
            param[0].Value = couponStatusRequest.CouponCode;

            using (SqlConnection con = DataConnection.GetConnection())
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "uspGetCouponDetails", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cd = new CouponData();
                    //cd.stieID = (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["SiteId"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["SiteId"]));

                    cd.NoOfUsedCoupon = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["NoOfUsedCoupon"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["NoOfUsedCoupon"]);
                    cd.NumberOfCoupon = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["NumberOfCoupon"].ToString()) ? 100000 : Convert.ToInt32(ds.Tables[0].Rows[0]["NumberOfCoupon"]);
                    cd.NumberOfCoupon = cd.NumberOfCoupon == 0 ? 100000 : cd.NumberOfCoupon;


                    cd.ClientType = (ClientType)(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ClientType"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["ClientType"]));
                    cd.SourceMedia = ds.Tables[0].Rows[0]["SourceMedia"].ToString();
                    cd.CouponType = (CouponType)(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CouponType"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["CouponType"]));
                    cd.CabinClass = (CabinType)(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CabinClass"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["CabinClass"]));

                    cd.MinimumAmount = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["MinimumAmount"].ToString()) ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["MinimumAmount"]);
                    cd.MaximumAmount = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["MaximumAmount"].ToString()) ? 100000 : Convert.ToDecimal(ds.Tables[0].Rows[0]["MaximumAmount"]);
                    cd.MaximumAmount = cd.MaximumAmount == 0 ? 100000 : cd.MaximumAmount;

                    cd.AmountPerCoupon = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["AmountPerCoupon"].ToString()) ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["AmountPerCoupon"]);
                    cd.CouponAmountPerUnit = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CouponAmountPerUnit"].ToString()) ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["CouponAmountPerUnit"]);

                    cd.TravelFromDate = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TravelFromDate"].ToString()) ? DateTime.Today : Convert.ToDateTime(ds.Tables[0].Rows[0]["TravelFromDate"]);
                    cd.TravelFromDate = cd.TravelFromDate.ToString("yyyy-MM-dd") == "0001-01-01" ? DateTime.Today : cd.TravelFromDate;
                    cd.TravelToDate = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TravelToDate"].ToString()) ? DateTime.Today.AddYears(1) : Convert.ToDateTime(ds.Tables[0].Rows[0]["TravelToDate"]);
                    cd.TravelToDate = cd.TravelToDate.ToString("yyyy-MM-dd") == "0001-01-01" ? DateTime.Today.AddYears(1) : cd.TravelToDate;

                    cd.ApplicableFromDate = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplicableFromDate"].ToString()) ? DateTime.Today : Convert.ToDateTime(ds.Tables[0].Rows[0]["ApplicableFromDate"]);
                    cd.ApplicableFromDate = cd.ApplicableFromDate.ToString("yyyy-MM-dd") == "0001-01-01" ? DateTime.Today : cd.ApplicableFromDate;
                    cd.ApplicableToDate = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplicableToDate"].ToString()) ? DateTime.Today.AddYears(1) : Convert.ToDateTime(ds.Tables[0].Rows[0]["ApplicableToDate"]);
                    cd.ApplicableToDate = cd.ApplicableToDate.ToString("yyyy-MM-dd") == "0001-01-01" ? DateTime.Today.AddYears(1) : cd.ApplicableToDate;

                    cd.MinimumPax = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["MinimumPax"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["MinimumPax"]);
                    cd.MaximumPax = (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["MaximumPax"].ToString()) || ds.Tables[0].Rows[0]["MaximumPax"].ToString() == "0") ? 9 : Convert.ToInt32(ds.Tables[0].Rows[0]["MaximumPax"]);
                }
            }
            return cd;
        }

        public CouponDetail GetCouponData(string CouponCode)
        {
            CouponDetail data = null;
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@CouponCode", SqlDbType.VarChar,50);
            param[0].Value = CouponCode;

            param[1] = new SqlParameter("@Counter", SqlDbType.VarChar, 500);
            param[1].Value = "Select";
            try
            {
                using (SqlConnection con = DataConnection.GetConnection())
                {
                    SqlDataReader reader = SqlHelper.ExecuteReader(con, CommandType.StoredProcedure, "Get_Set_CouponManagement", param);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            data = new CouponDetail();
                            data.id = Convert.ToInt32(reader["id"]);
                            data.CouponCode = Convert.ToString(reader["CouponCode"]);
                            data.couponAmount = string.IsNullOrEmpty(reader["Couponamount"].ToString()) ? 0 : Convert.ToInt32(reader["Couponamount"]);
                            data.amountType = (CouponAmountType)(string.IsNullOrEmpty(reader["amountType"].ToString()) ? 0 : Convert.ToInt32(reader["amountType"]));
                            data.noOfPax = string.IsNullOrEmpty(reader["noOfPax"].ToString()) ? 0 : Convert.ToInt32(reader["noOfPax"]);
                            data.maxAmount = string.IsNullOrEmpty(reader["maxAmount"].ToString()) ? 0 : Convert.ToInt32(reader["maxAmount"]);
                            data.isValidateSourceMedia = string.IsNullOrEmpty(reader["isValidateSourceMedia"].ToString()) ? false : Convert.ToBoolean(reader["isValidateSourceMedia"]);
                            data.isValidateByEmail = string.IsNullOrEmpty(reader["isValidateByEmail"].ToString()) ? false : Convert.ToBoolean(reader["isValidateByEmail"]);
                            data.isValidateByCount = string.IsNullOrEmpty(reader["isValidateByCount"].ToString()) ? false : Convert.ToBoolean(reader["isValidateByCount"]);
                            data.isValidateByNoOfPax = string.IsNullOrEmpty(reader["isValidateByNoOfPax"].ToString()) ? false : Convert.ToBoolean(reader["isValidateByNoOfPax"]);
                            data.isValidateByTotalAmt = string.IsNullOrEmpty(reader["isValidateByTotalAmt"].ToString()) ? false : Convert.ToBoolean(reader["isValidateByTotalAmt"]);
                            data.isValidateByCabinClass = string.IsNullOrEmpty(reader["isValidateByCabinClass"].ToString()) ? false : Convert.ToBoolean(reader["isValidateByCabinClass"]);
                            data.isValidateByAirline = string.IsNullOrEmpty(reader["isValidateByAirline"].ToString()) ? false : Convert.ToBoolean(reader["isValidateByAirline"]);
                            data.isValidateByBookingDate = string.IsNullOrEmpty(reader["isValidateByBookingDate"].ToString()) ? false : Convert.ToBoolean(reader["isValidateByBookingDate"]);
                            data.isValidateByTravelDate = string.IsNullOrEmpty(reader["isValidateByTravelDate"].ToString()) ? false : Convert.ToBoolean(reader["isValidateByTravelDate"]);
                            data.isLessConvenceFee = string.IsNullOrEmpty(reader["isLessConvenceFee"].ToString()) ? false : Convert.ToBoolean(reader["isLessConvenceFee"]);
                            data.isValidationByUTFCampaign = string.IsNullOrEmpty(reader["isValidationByUTFCampaign"].ToString()) ? false : Convert.ToBoolean(reader["isValidationByUTFCampaign"]);
                            data.sourceMedia = reader["sourceMedia"].ToString();//(SourceMedia)(string.IsNullOrEmpty(Convert.ToInt32(reader["sourceMedia"]).ToString()) ? 0 : ((SourceMedia)Convert.ToInt32(reader["sourceMedia"])).ToString();
                            data.emailID = Convert.ToString(reader["emailID"]);
                            data.noOfCoupon = Convert.ToInt32(reader["noOfCoupon"]);
                            data.totalConsume = string.IsNullOrEmpty(reader["totalConsume"].ToString()) ? 0 : Convert.ToInt32(reader["totalConsume"]);//Convert.ToInt32(reader["totalConsume"]);
                            data.minAmount = string.IsNullOrEmpty(reader["minAmount"].ToString()) ? 0 : Convert.ToInt32(reader["minAmount"]);//Convert.ToInt32(reader["minAmount"]);
                            data.cabinClass = (CabinType)(string.IsNullOrEmpty(reader["cabinClass"].ToString()) ? 0 : Convert.ToInt32(reader["cabinClass"]));//;
                            data.airline = Convert.ToString(reader["airline"]);
                            data.UTFCampaign = Convert.ToString(reader["UTFCampaign"]);
                            if (data.isValidateByBookingDate)
                            {
                                data.bookingDateFrom = (string.IsNullOrEmpty(reader["bookingDateFrom"].ToString())) ? DateTime.Today.AddDays(-1) : Convert.ToDateTime(reader["bookingDateFrom"]);
                            }
                            else
                            {
                                data.bookingDateFrom = DateTime.Today.AddDays(-1);
                            }
                            if (data.isValidateByBookingDate)
                            {
                                data.bookingDateTo = (string.IsNullOrEmpty(reader["bookingDateTo"].ToString())) ? DateTime.Today.AddDays(365) : Convert.ToDateTime(reader["bookingDateTo"]);
                            }
                            else
                            {
                                data.bookingDateTo = DateTime.Today.AddDays(365);
                            }


                            if (data.isValidateByTravelDate)
                            {
                                data.travelDateFrom = (string.IsNullOrEmpty(reader["travelDateFrom"].ToString())) ? DateTime.Today.AddDays(-1) : Convert.ToDateTime(reader["travelDateFrom"]);
                            }
                            else
                            {
                                data.travelDateFrom = DateTime.Today.AddDays(-1);
                            }
                            if (data.isValidateByTravelDate)
                            {
                                data.travelDateTo = (string.IsNullOrEmpty(reader["travelDateTo"].ToString())) ? DateTime.Today.AddDays(365) : Convert.ToDateTime(reader["travelDateTo"]);
                            }
                            else
                            {
                                data.travelDateTo = DateTime.Today.AddDays(365);
                            }

                        
                          
                            data.isActive = string.IsNullOrEmpty(reader["isActive"].ToString()) ? false : Convert.ToBoolean(reader["isActive"]);
                                                      
                        }
                    }
                    else
                    {
                       
                    }
                }
            }
            catch (Exception ex)
            {
                //ex.Message();
            }
            return data;
        }
    }
}
