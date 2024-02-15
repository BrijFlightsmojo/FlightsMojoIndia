using Core.Flight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DAL.Deal
{
    public class UserSearchFareDeal
    {
        //public static SqlDataReader Get(Int64 id)
        //{
        //    SqlParameter[] param = new SqlParameter[1];
        //    param[0] = new SqlParameter("@id", SqlDbType.BigInt);
        //    param[0].Value = id;

        //    using (SqlConnection con = DataConnection.GetConnection())
        //    {
        //        return SqlHelper.ExecuteReader(con, CommandType.StoredProcedure, "usp_UserSearchFareDealSelect", param);
        //    }
        //}
        //public async System.Threading.Tasks.Task SaveUserSearchFareDeal(FlightSearchRequest fsr, List<FlightResult> result)
        //{
        //    StringBuilder strCarrier = new StringBuilder();
        //    using (SqlConnection conn = DataConnection.GetConnection())
        //    {
        //        try
        //        {
        //            conn.Open();
        //            foreach (var item in result)
        //            {
        //                if (strCarrier.ToString().IndexOf(item.outBound[0].airline, StringComparison.OrdinalIgnoreCase) == -1)
        //                {
        //                    strCarrier.Append(item.outBound[0].airline + ",");
        //                    using (SqlCommand cmd = new SqlCommand("usp_UserSearchFareDealInsert", conn))
        //                    {
        //                        cmd.CommandType = CommandType.StoredProcedure;
        //                        cmd.Parameters.AddWithValue("@siteID", (int)fsr.siteId);
        //                        cmd.Parameters.AddWithValue("@sourchMedia", fsr.sourceMedia);
        //                        cmd.Parameters.AddWithValue("@origin", item.outBound[0].fromAirport);
        //                        cmd.Parameters.AddWithValue("@destination", item.outBound[item.outBound.Count - 1].toAirport);
        //                        cmd.Parameters.AddWithValue("@depDate", item.outBound[0].depDate);
        //                        cmd.Parameters.AddWithValue("@retDate", (item.inBound!=null&& item.inBound.Count>0? item.inBound[0].depDate : item.outBound[0].depDate));
        //                        cmd.Parameters.AddWithValue("@airline", item.outBound[0].airline);
        //                        cmd.Parameters.AddWithValue("@tripType", (Int16)fsr.tripType);
        //                        cmd.Parameters.AddWithValue("@cabinClass", (Int16)fsr.cabinType);
        //                        cmd.Parameters.AddWithValue("@baseFare", item.fare.adultFare);
        //                        cmd.Parameters.AddWithValue("@tax", item.fare.adultTax);
        //                        cmd.Parameters.AddWithValue("@markup", item.fare.adultMarkup);

        //                        cmd.ExecuteNonQuery();
        //                    }
        //                }
        //            }
        //        }
        //        catch { }
        //        finally
        //        {
        //            conn.Close();
        //        }
        //    }
        //}
        //public static int UpdateUserSearchFareDeal(Int64 id, int siteID, string sourchMedia, string origin, string destination, string airline, string tripType, string cabinClass, decimal baseFare, decimal tax, decimal markup, DateTime searchDateTime)
        //{
        //    SqlParameter[] param = new SqlParameter[12];
        //    param[0] = new SqlParameter("@id", SqlDbType.BigInt);
        //    param[0].Value = id;
        //    param[1] = new SqlParameter("@siteID", SqlDbType.Int);
        //    param[1].Value = siteID;
        //    param[2] = new SqlParameter("@sourchMedia", SqlDbType.VarChar, 50);
        //    param[2].Value = sourchMedia;
        //    param[3] = new SqlParameter("@origin", SqlDbType.VarChar, 3);
        //    param[3].Value = origin;
        //    param[4] = new SqlParameter("@destination", SqlDbType.VarChar, 3);
        //    param[4].Value = destination;
        //    param[5] = new SqlParameter("@airline", SqlDbType.VarChar, 2);
        //    param[5].Value = airline;
        //    param[6] = new SqlParameter("@tripType", SqlDbType.SmallInt);
        //    param[6].Value = tripType;
        //    param[7] = new SqlParameter("@cabinClass", SqlDbType.SmallInt);
        //    param[7].Value = cabinClass;
        //    param[8] = new SqlParameter("@baseFare", SqlDbType.Decimal);
        //    param[8].Value = baseFare;
        //    param[9] = new SqlParameter("@tax", SqlDbType.Decimal);
        //    param[9].Value = tax;
        //    param[10] = new SqlParameter("@markup", SqlDbType.Decimal);
        //    param[10].Value = markup;
        //    param[11] = new SqlParameter("@searchDateTime", SqlDbType.DateTime);
        //    param[11].Value = searchDateTime;
        //    using (SqlConnection con = DataConnection.GetConnection())
        //    {
        //        return SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "usp_UserSearchFareDealUpdate", param);
        //    }
        //}
        //public static int DeleteUserSearchFareDeal(Int64 id)
        //{
        //    SqlParameter[] param = new SqlParameter[1];
        //    param[0] = new SqlParameter("@id", SqlDbType.BigInt);
        //    param[0].Value = id;
        //    using (SqlConnection con = DataConnection.GetConnection())
        //    {
        //        return SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "usp_UserSearchFareDealDelete", param);
        //    }
        //}
    }

}

