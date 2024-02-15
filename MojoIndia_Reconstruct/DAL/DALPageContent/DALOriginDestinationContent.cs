using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Core.ContentPage;
using Core;

namespace Dal
{
    public class DALOriginDestinationContent
    {
        public OriginDestinationContent OriginDestinationWithDeal(SiteId siteID, string OriginCode, string DestinationCode, bool isActive,string counter)
        {
            SqlParameter[] param = new SqlParameter[5];

            param[0] = new SqlParameter("@siteID", SqlDbType.Int);
            param[0].Value = (int)siteID;

            param[1] = new SqlParameter("@OriginCode", SqlDbType.VarChar, 3);
            param[1].Value = OriginCode;

            param[2] = new SqlParameter("@DestinationCode", SqlDbType.VarChar, 3);
            param[2].Value = DestinationCode;

            param[3] = new SqlParameter("@isActive", SqlDbType.Bit);
            param[3].Value = isActive;

            param[4] = new SqlParameter("@Counter", SqlDbType.VarChar,50);
            param[4].Value = counter;

            using (SqlConnection con = DataConnection.GetConnection())
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "spOriginDestinationContent_SELECT", param);
                OriginDestinationContent content = new OriginDestinationContent();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    content.OriginCode = ds.Tables[0].Rows[0]["OriginCode"].ToString();
                    content.OriginName = ds.Tables[0].Rows[0]["OriginName"].ToString();
                    content.DestinationCode = ds.Tables[0].Rows[0]["DestinationCode"].ToString();
                    content.DestinationName = ds.Tables[0].Rows[0]["DestinationName"].ToString();
                    content.PageTitle = ds.Tables[0].Rows[0]["PageTitle"].ToString();
                    content.MetaDescription = ds.Tables[0].Rows[0]["MetaDescription"].ToString();
                    content.MetaKeyWord = ds.Tables[0].Rows[0]["MetaKeyWord"].ToString();
                    content.BannerHeading = ds.Tables[0].Rows[0]["BannerHeading"].ToString();
                    content.DealHeading = ds.Tables[0].Rows[0]["DealHeading"].ToString();
                    content.AboutODHeading = ds.Tables[0].Rows[0]["AboutODHeading"].ToString();
                    content.AboutODDescription = ds.Tables[0].Rows[0]["AboutODDescription"].ToString();
                    content.Heading1 = ds.Tables[0].Rows[0]["Heading1"].ToString();
                    content.Heading2 = ds.Tables[0].Rows[0]["Heading2"].ToString();
                    content.Heading3 = ds.Tables[0].Rows[0]["Heading3"].ToString();
                    content.Heading4 = ds.Tables[0].Rows[0]["Heading4"].ToString();
                    content.Heading5 = ds.Tables[0].Rows[0]["Heading5"].ToString();
                    content.Heading1Description = ds.Tables[0].Rows[0]["Heading1Description"].ToString();
                    content.Heading2Description = ds.Tables[0].Rows[0]["Heading2Description"].ToString();
                    content.Heading3Description = ds.Tables[0].Rows[0]["Heading3Description"].ToString();
                    content.Heading4Description = ds.Tables[0].Rows[0]["Heading4Description"].ToString();
                    content.Heading5Description = ds.Tables[0].Rows[0]["Heading5Description"].ToString();
                    content.OriginAirportAddress = ds.Tables[0].Rows[0]["OriginAirportAddress"].ToString();
                    content.DestinationAirportAddress = ds.Tables[0].Rows[0]["DestinationAirportAddress"].ToString();
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
                    content.isStaticContent = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["isStaticContent"].ToString()) ? true : Convert.ToBoolean(ds.Tables[0].Rows[0]["isStaticContent"]);
                    content.ResponseStatus = new Core.ResponseStatus();
                    content.ResponseStatus.status = Core.TransactionStatus.Success;
                    content.ResponseStatus.message = "Data pull properly, Please take action!!";

                    if ( ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        content.websiteFareDeal = new List<WebsiteFareDeal>();
                        foreach (DataRow dr in ds.Tables[1].Rows)
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

        public OriginDestinationContent OriginDestinationWithDealHomePage(int dealtype, string OriginCode, string DestinationCode, string airline, int triptype, int cabinClass)
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@dealType", SqlDbType.Int);
            param[0].Value = (int)dealtype;

            param[1] = new SqlParameter("@origin", SqlDbType.VarChar, 3);
            param[1].Value = OriginCode;

            param[2] = new SqlParameter("@destination", SqlDbType.VarChar, 3);
            param[2].Value = DestinationCode;

            param[3] = new SqlParameter("@airline", SqlDbType.VarChar, 3);
            param[3].Value = airline;

            param[4] = new SqlParameter("@tripType", SqlDbType.Int);
            param[4].Value = triptype;

            param[5] = new SqlParameter("@cabinClass", SqlDbType.Int);
            param[5].Value = cabinClass;

            using (SqlConnection con = DataConnection.GetConnection())
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_WebsiteDealSelectForWebsite", param);
                OriginDestinationContent content = new OriginDestinationContent();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                        content.websiteFareDeal = new List<WebsiteFareDeal>();
                        foreach (DataRow dr in ds.Tables[0].Rows)
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
                else
                {
                    content.ResponseStatus = new Core.ResponseStatus();
                    content.ResponseStatus.status = Core.TransactionStatus.Error;
                    content.ResponseStatus.message = "Data not pull properly";
                }
                return content;
            }
        }



        public Sitemap GetSitemap()
        {
            Sitemap sitemap = new Sitemap();
            try
            {
                using (SqlConnection con = DataConnection.GetConnection())
                {
                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "spOriginDestinationContent_List_V1", null);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0] != null)
                        {
                            sitemap.OnD = new List<OriginDestinationContent>();

                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                OriginDestinationContent ODC = new OriginDestinationContent();
                                ODC.OriginCode= row["OriginCode"].ToString();
                                ODC.DestinationCode = row["DestinationCode"].ToString();
                                ODC.OriginName = row["OriginName"].ToString();
                                ODC.DestinationName = row["DestinationName"].ToString();
                                ODC.Created = (string.IsNullOrEmpty(row["InsertedOn"].ToString()) ? DateTime.Today : Convert.ToDateTime(row["InsertedOn"]));
                                sitemap.OnD.Add(ODC);
                            }
                        }


                        if (ds.Tables.Count > 1 && ds.Tables[1] != null)
                        {
                            sitemap.cityContent = new List<CityContent>();

                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                CityContent CC = new CityContent();
                                CC.CityCode = dr["CityCode"].ToString();
                                CC.CityName = dr["CityName"].ToString();
                                CC.InsertedOn = (string.IsNullOrEmpty(dr["InsertedOn"].ToString()) ? DateTime.Today : Convert.ToDateTime(dr["InsertedOn"]));
                                sitemap.cityContent.Add(CC);
                            }
                        }


                        if (ds.Tables.Count > 1 && ds.Tables[2] != null)
                        {
                            sitemap.AirlineContent = new List<AirlineContent>();

                            foreach (DataRow dr in ds.Tables[2].Rows)
                            {
                                AirlineContent AC = new AirlineContent();
                                AC.AirlineCode = dr["AirlineCode"].ToString();
                                AC.AirlineName = dr["AirlineName"].ToString();
                                AC.InsertedOn = (string.IsNullOrEmpty(dr["InsertedOn"].ToString()) ? DateTime.Today : Convert.ToDateTime(dr["InsertedOn"]));
                                sitemap.AirlineContent.Add(AC);
                            }
                        }


                        if (ds.Tables.Count > 1 && ds.Tables[3] != null)
                        {
                            sitemap.DealsContent = new List<DealsContent>();

                            foreach (DataRow dr in ds.Tables[3].Rows)
                            {
                                DealsContent DC = new DealsContent();
                                DC.ThemeName = dr["ThemeName"].ToString();
                                DC.InsertedOn = (string.IsNullOrEmpty(dr["InsertedOn"].ToString()) ? DateTime.Today : Convert.ToDateTime(dr["InsertedOn"]));
                                sitemap.DealsContent.Add(DC);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sitemap;
        }
    }
}