using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Core.ContentPage;
using Core;

/// <summary>
/// Summary description for WebsiteContentData
/// </summary>
namespace Dal
{
    public class DALCityContent
    {
        public CityContent GetCityContent(string CityCode,int SiteID,bool isActive,string Counter)
        {
            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@CityCode", SqlDbType.VarChar, 50);
            param[0].Value = CityCode;

            param[1] = new SqlParameter("@SiteID", SqlDbType.Int);
            param[1].Value = (int)SiteID;

            param[2] = new SqlParameter("@isActive", SqlDbType.Bit);
            param[2].Value = isActive;

            param[3] = new SqlParameter("@Counter", SqlDbType.VarChar, 50);
            param[3].Value = Counter;
            using (SqlConnection con = DataConnection.GetConnection())
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_CityContent_SELECT", param);
                CityContent content = new CityContent();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    content.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
                    content.SiteID = (SiteId)(ds.Tables[0].Rows[0]["SiteID"]);
                    content.CityCode = ds.Tables[0].Rows[0]["CityCode"].ToString();
                    content.CityName = ds.Tables[0].Rows[0]["CityName"].ToString();
                    content.PageTitle = ds.Tables[0].Rows[0]["PageTitle"].ToString();
                    content.MetaDescription = ds.Tables[0].Rows[0]["MetaDescription"].ToString();
                    content.MetaKeyWord = ds.Tables[0].Rows[0]["MetaKeyWord"].ToString();
                    content.BannerHeading = ds.Tables[0].Rows[0]["BannerHeading"].ToString();
                    content.DealHeading = ds.Tables[0].Rows[0]["DealHeading"].ToString();
                    content.AboutCityHeading = ds.Tables[0].Rows[0]["AboutCityHeading"].ToString();
                    content.CitySubContentHeading1 = ds.Tables[0].Rows[0]["CitySubContentHeading1"].ToString();
                    content.CitySubContentHeading2 = ds.Tables[0].Rows[0]["CitySubContentHeading2"].ToString();
                    content.CitySubContentHeading3 = ds.Tables[0].Rows[0]["CitySubContentHeading3"].ToString();
                    content.CitySubContentHeading4 = ds.Tables[0].Rows[0]["CitySubContentHeading4"].ToString();
                    content.CitySubContentHeading5 = ds.Tables[0].Rows[0]["CitySubContentHeading5"].ToString();
                    content.AboutCityDescription = ds.Tables[0].Rows[0]["AboutCityDescription"].ToString();
                    content.CitySubDescription1 = ds.Tables[0].Rows[0]["CitySubDescription1"].ToString();
                    content.CitySubDescription2 = ds.Tables[0].Rows[0]["CitySubDescription2"].ToString();
                    content.CitySubDescription3 = ds.Tables[0].Rows[0]["CitySubDescription3"].ToString();
                    content.CitySubDescription4 = ds.Tables[0].Rows[0]["CitySubDescription4"].ToString();
                    content.CitySubDescription5 = ds.Tables[0].Rows[0]["CitySubDescription5"].ToString();

                    content.OriginAirportAddress = ds.Tables[0].Rows[0]["AirportAddress"].ToString();
                    content.FAQQ1 = ds.Tables[0].Rows[0]["FAQQ1"].ToString();
                    content.FAQQ2 = ds.Tables[0].Rows[0]["FAQQ2"].ToString();
                    content.FAQQ3 = ds.Tables[0].Rows[0]["FAQQ3"].ToString();
                    content.FAQQ4 = ds.Tables[0].Rows[0]["FAQQ4"].ToString();
                    content.FAQQ5 = ds.Tables[0].Rows[0]["FAQQ5"].ToString();
                    content.FAQQ6 = ds.Tables[0].Rows[0]["FAQQ6"].ToString();
                    content.FAQQ7 = ds.Tables[0].Rows[0]["FAQQ7"].ToString();
                    content.FAQQ8 = ds.Tables[0].Rows[0]["FAQQ8"].ToString();
                    content.FAQQ9 = ds.Tables[0].Rows[0]["FAQQ9"].ToString();
                    content.FAQQ10 = ds.Tables[0].Rows[0]["FAQQ10"].ToString();

                    content.FAQQ11 = ds.Tables[0].Rows[0]["FAQQ11"].ToString();
                    content.FAQQ12 = ds.Tables[0].Rows[0]["FAQQ12"].ToString();
                    content.FAQQ13 = ds.Tables[0].Rows[0]["FAQQ13"].ToString();
                    content.FAQQ14 = ds.Tables[0].Rows[0]["FAQQ14"].ToString();
                    content.FAQQ15 = ds.Tables[0].Rows[0]["FAQQ15"].ToString();


                    content.FAQANS1 = ds.Tables[0].Rows[0]["FAQANS1"].ToString();
                    content.FAQANS2 = ds.Tables[0].Rows[0]["FAQANS2"].ToString();
                    content.FAQANS3 = ds.Tables[0].Rows[0]["FAQANS3"].ToString();
                    content.FAQANS4 = ds.Tables[0].Rows[0]["FAQANS4"].ToString();
                    content.FAQANS5 = ds.Tables[0].Rows[0]["FAQANS5"].ToString();
                    content.FAQANS6 = ds.Tables[0].Rows[0]["FAQANS6"].ToString();
                    content.FAQANS7 = ds.Tables[0].Rows[0]["FAQANS7"].ToString();
                    content.FAQANS8 = ds.Tables[0].Rows[0]["FAQANS8"].ToString();
                    content.FAQANS9 = ds.Tables[0].Rows[0]["FAQANS9"].ToString();
                    content.FAQANS10 = ds.Tables[0].Rows[0]["FAQANS10"].ToString();

                    content.FAQANS11 = ds.Tables[0].Rows[0]["FAQANS11"].ToString();
                    content.FAQANS12 = ds.Tables[0].Rows[0]["FAQANS12"].ToString();
                    content.FAQANS13 = ds.Tables[0].Rows[0]["FAQANS13"].ToString();
                    content.FAQANS14 = ds.Tables[0].Rows[0]["FAQANS14"].ToString();
                    content.FAQANS15 = ds.Tables[0].Rows[0]["FAQANS15"].ToString();

                    content.isStaticContent = Convert.ToBoolean(ds.Tables[0].Rows[0]["isStaticContent"]);
                    content.isActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["isActive"]);
                    content.InsertedBy = Convert.ToInt32(ds.Tables[0].Rows[0]["InsertedBy"]);
                    content.InsertedOn = Convert.ToDateTime(ds.Tables[0].Rows[0]["InsertedOn"]);
                    content.ModifiedBy = Convert.ToInt32(ds.Tables[0].Rows[0]["ModifiedBy"]);
                    content.ModifiedOn = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedOn"]);
                    content.ResponseStatus = new Core.ResponseStatus();
                    content.ResponseStatus.message = "Data pull properly, Please take action!!";

                    //if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    //{
                    //    content.websiteFareDeal = new List<WebsiteFareDeal>();
                    //    foreach (DataRow dr in ds.Tables[1].Rows)
                    //    {
                    //        WebsiteFareDeal deal = new WebsiteFareDeal()
                    //        {
                    //            airline = Core.FlightUtility.GetAirline(dr["airline"].ToString()),
                    //            origin = FlightUtility.GetAirport(dr["origin"].ToString()),
                    //            destination = FlightUtility.GetAirport(dr["destination"].ToString()),
                    //            tripType = ((TripType)(string.IsNullOrEmpty(dr["tripType"].ToString()) ? 0 : Convert.ToInt32(dr["tripType"]))).ToString(),
                    //            cabinClass = ((CabinType)(string.IsNullOrEmpty(dr["cabinClass"].ToString()) ? 0 : Convert.ToInt32(dr["cabinClass"]))).ToString(),
                    //            totalFare = (string.IsNullOrEmpty(dr["totalFare"].ToString()) ? 0 : Convert.ToDecimal(dr["totalFare"])).ToString("f0"),
                    //            depDate = (string.IsNullOrEmpty(dr["depDate"].ToString()) ? DateTime.Today : Convert.ToDateTime(dr["depDate"])),
                    //            retDate = (string.IsNullOrEmpty(dr["retDate"].ToString()) ? DateTime.Today : Convert.ToDateTime(dr["retDate"]))
                    //        };
                    //        content.websiteFareDeal.Add(deal);
                    //    }
                    //}

                    using (SqlConnection conn = DataConnection.GetConSearchHistoryAndDeal_RDS())
                    {
                        DataSet dset = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "[usp_CityContent_SELECT]", param);

                        content.websiteFareDeal = new List<WebsiteFareDeal>();
                        foreach (DataRow dr in dset.Tables[0].Rows)
                        {
                            WebsiteFareDeal deal = new WebsiteFareDeal()
                            {
                                airline = Core.FlightUtility.GetAirline(dr["airline"].ToString()),
                                origin = FlightUtility.GetAirport(dr["origin"].ToString()),
                                destination = FlightUtility.GetAirport(dr["destination"].ToString()),
                                tripType = ((TripType)(string.IsNullOrEmpty(dr["tripType"].ToString()) ? 0 : Convert.ToInt32(dr["tripType"]))).ToString(),
                                cabinClass = ((CabinType)(string.IsNullOrEmpty(dr["cabinClass"].ToString()) ? 0 : Convert.ToInt32(dr["cabinClass"]))).ToString(),
                                totalFare = (string.IsNullOrEmpty(dr["totalFare"].ToString()) ? 0 : Convert.ToDecimal(dr["totalFare"])).ToString("f0"),
                                depDate = (string.IsNullOrEmpty(dr["depDate"].ToString()) ? DateTime.Today : Convert.ToDateTime(dr["depDate"])),
                                retDate = (string.IsNullOrEmpty(dr["retDate"].ToString()) ? DateTime.Today : Convert.ToDateTime(dr["retDate"]))
                            };
                            content.websiteFareDeal.Add(deal);
                        }
                    }
                }
                else
                {
                    content.ResponseStatus = new Core.ResponseStatus();
                    content.ResponseStatus.status = Core.TransactionStatus.Error;
                    content.ResponseStatus.message = "Data not pull properly";
                }
                return content;
            }
        }
    }
}