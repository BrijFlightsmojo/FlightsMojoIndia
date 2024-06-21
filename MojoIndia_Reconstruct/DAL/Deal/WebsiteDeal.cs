using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Deal
{
    public class WebsiteDeal
    {
        public List<Core.ContentPage.WebsiteCustomDeal> GetWebsiteDealOnPage(int dealType, string origin, string destination, string airline, Int16 tripType, Int16 cabinClass)
        {
            SqlParameter[] param = new SqlParameter[6];

            param[0] = new SqlParameter("@dealType", SqlDbType.Int);
            param[0].Value = dealType;

            if (!string.IsNullOrEmpty(origin))
            {
                param[1] = new SqlParameter("@origin", SqlDbType.VarChar, 3);
                param[1].Value = origin;
            }
            if (!string.IsNullOrEmpty(destination))
            {
                param[2] = new SqlParameter("@destination", SqlDbType.VarChar, 3);
                param[2].Value = destination;
            }
            if (!string.IsNullOrEmpty(airline))
            {
                param[3] = new SqlParameter("@airline", SqlDbType.VarChar, 3);
                param[3].Value = airline;
            }
            if (tripType > 0)
            {
                param[4] = new SqlParameter("@tripType", SqlDbType.SmallInt);
                param[4].Value = tripType;
            }
            if (cabinClass > 0)
            {
                param[5] = new SqlParameter("@cabinClass", SqlDbType.SmallInt);
                param[5].Value = cabinClass;
            }
            using (SqlConnection con = DataConnection.GetConSearchHistoryAndDeal_RDS())
            {
                SqlDataReader dr = SqlHelper.ExecuteReader(con, CommandType.StoredProcedure, "usp_WebsiteDealSelectForWebsite", param);
                List<Core.ContentPage.WebsiteCustomDeal> objList = new List<Core.ContentPage.WebsiteCustomDeal>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Core.ContentPage.WebsiteCustomDeal obj = new Core.ContentPage.WebsiteCustomDeal();
                        obj.origin = Core.FlightUtility.GetAirport(dr["origin"].ToString());
                        obj.destination = Core.FlightUtility.GetAirport(dr["destination"].ToString());
                        obj.depDate = Convert.ToDateTime(dr["depDate"]).ToString("yyyy-MM-dd");
                        string retDate = dr["retDate"].ToString();
                        obj.retDate = string.IsNullOrEmpty(retDate) ? "" : Convert.ToDateTime(retDate).ToString("yyyy-MM-dd");
                        obj.airline = Core.FlightUtility.GetAirline(dr["airline"].ToString());
                        obj.tripType = ((Core.TripType)Convert.ToInt32(dr["tripType"])).ToString();
                        obj.cabinClass = ((Core.CabinType)Convert.ToInt32(dr["cabinClass"])).ToString();
                        //obj.totalFare = Convert.ToDecimal(dr["TotalFare"]).ToString("C", new System.Globalization.CultureInfo("EN-in"));
                        obj.totalFare = Convert.ToDecimal(dr["TotalFare"]).ToString("0,0", new System.Globalization.CultureInfo("HI-in"));
                        obj.deepLink = "/flight/searchFlightResult?org=" + obj.origin.airportCode + "&dest=" + obj.destination.airportCode + "&depdate=" + Convert.ToDateTime(dr["depDate"]).ToString("dd-MM-yyyy") + "&retdate=" + (obj.tripType.Equals("OneWay") ? "" : Convert.ToDateTime(retDate).ToString("dd-MM-yyyy")) + "&tripType=" + (obj.tripType.Equals("OneWay") ? "O" : "R") + "&adults=1&child=0&infants=0&cabin=" + ((int)((Core.CabinType)Convert.ToInt32(dr["cabinClass"]))) + "&utm_source=1000&currency=inr";
                        objList.Add(obj);
                    }
                }
                return objList;
            }
        }

        //public DataSet GetWebsiteDeal(int dealType, string origin, string destination, string airline, Int16 tripType, Int16 cabinClass)
        //{
        //    SqlParameter[] param = new SqlParameter[6];

        //    param[0] = new SqlParameter("@dealType", SqlDbType.Int);
        //    param[0].Value = dealType;

        //    if (!string.IsNullOrEmpty(origin))
        //    {
        //        param[1] = new SqlParameter("@origin", SqlDbType.VarChar, 3);
        //        param[1].Value = origin;
        //    }
        //    if (!string.IsNullOrEmpty(destination))
        //    {
        //        param[2] = new SqlParameter("@destination", SqlDbType.VarChar, 3);
        //        param[2].Value = destination;
        //    }
        //    if (!string.IsNullOrEmpty(airline))
        //    {
        //        param[3] = new SqlParameter("@airline", SqlDbType.VarChar, 3);
        //        param[3].Value = airline;
        //    }
        //    if (tripType > 0)
        //    {
        //        param[4] = new SqlParameter("@tripType", SqlDbType.SmallInt);
        //        param[4].Value = tripType;
        //    }
        //    if (cabinClass > 0)
        //    {
        //        param[5] = new SqlParameter("@cabinClass", SqlDbType.SmallInt);
        //        param[5].Value = cabinClass;
        //    }
        //    using (SqlConnection con = DataConnection.GetConSearchHistoryAndDeal_RDS())
        //    {
        //        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_WebsiteDealSelectForWebsite", param);
        //    }
        //}

        public List<Core.Flight.fareMatrix> GetFareMatrix(string org, string dest)
        {
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@org", SqlDbType.VarChar, 3);
            param[0].Value = org;

            param[1] = new SqlParameter("@dest", SqlDbType.VarChar, 3);
            param[1].Value = dest;


            using (SqlConnection con = DataConnection.GetConMetaRank())
            {
                SqlDataReader dr = SqlHelper.ExecuteReader(con, CommandType.StoredProcedure, "get_MetaRankO&DFare", param);
                List<Core.Flight.fareMatrix> objList = new List<Core.Flight.fareMatrix>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Core.Flight.fareMatrix obj = new Core.Flight.fareMatrix();
                        obj.sqNo = objList.Count;
                        obj.fare =Convert.ToDecimal( dr["fare"]);
                        obj.tDate = Convert.ToDateTime(dr["depDate"]).ToString("MMM-dd");// dr["depDate"].ToString();

                        objList.Add(obj);
                    }
                }
                return objList;
            }
        }
    }

}

