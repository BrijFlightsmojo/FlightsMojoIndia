using Core.Flight;
using System;
using System.Data;
using System.Data.SqlClient;
namespace DAL.Deal
{
    public class UserSearchHistory
    {
        public async System.Threading.Tasks.Task SaveSearchData(VerifyFareDetails vfd)
        {
            SqlParameter[] param = new SqlParameter[10];

            param[0] = new SqlParameter("@userSearchID", SqlDbType.VarChar, 50);
            param[0].Value = vfd.userSearchID;

            param[1] = new SqlParameter("@FirstSearchDate", SqlDbType.DateTime);
            param[1].Value = vfd.FirstSearchDate;

            param[2] = new SqlParameter("@FirstSearchFareID", SqlDbType.VarChar, 200);
            param[2].Value = vfd.FirstSearchFareID;
            param[3] = new SqlParameter("@SeconSearchFareID", SqlDbType.VarChar, 200);
            param[3].Value = vfd.SeconSearchFareID;

            param[4] = new SqlParameter("@SecondSearchDate", SqlDbType.DateTime);
            param[4].Value = vfd.SecondSearchDate;

            param[5] = new SqlParameter("@TripjackBookingID", SqlDbType.VarChar, 50);
            param[5].Value = vfd.TripjackBookingID;

            param[6] = new SqlParameter("@PreviousAmt", SqlDbType.Decimal);
            param[6].Value = vfd.PreviousAmt;
            param[7] = new SqlParameter("@NewAmt", SqlDbType.Decimal);
            param[7].Value = vfd.newAmt;

            param[8] = new SqlParameter("@Origin", SqlDbType.VarChar, 3);
            param[8].Value = vfd.Origin;
            param[9] = new SqlParameter("@Destination", SqlDbType.VarChar, 3);
            param[9].Value = vfd.Destination;
            try
            {
                using (SqlConnection con = DataConnection.GetConnection())
                {
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "VerifyFareDetailsInsert", param);
                }
            }
            catch (Exception ex)
            {

            }
        }
        //public SqlDataReader Get(Int64 id)
        //{
        //    SqlParameter[] param = new SqlParameter[1];
        //    param[0] = new SqlParameter("@id", SqlDbType.BigInt);
        //    param[0].Value = id;

        //    using (SqlConnection con = DataConnection.GetConnection())
        //    {
        //        return SqlHelper.ExecuteReader(con, CommandType.StoredProcedure, "usp_UserSearchHistorySelect", param);
        //    }
        //}
        //public async System.Threading.Tasks.Task SaveUserSearchHistory(FlightSearchRequest fsr, bool isCacheFare, string ip, int totalResult)
        //{
        //    SqlParameter[] param = new SqlParameter[25];

        //    param[1] = new SqlParameter("@siteID", SqlDbType.Int);
        //    param[1].Value = (int)fsr.siteId;
        //    param[2] = new SqlParameter("@sourchMedia", SqlDbType.VarChar, 50);
        //    param[2].Value = fsr.sourceMedia;
        //    param[3] = new SqlParameter("@depOrg", SqlDbType.VarChar, 3);
        //    param[3].Value = fsr.segment[0].originAirport;
        //    param[4] = new SqlParameter("@depDest", SqlDbType.VarChar, 3);
        //    param[4].Value = fsr.segment[0].destinationAirport;
        //    if ((fsr.tripType == Core.TripType.OpenJow || fsr.tripType == Core.TripType.RoundTrip) && fsr.segment.Count > 1)
        //    {
        //        param[5] = new SqlParameter("@retOrg", SqlDbType.VarChar, 3);
        //        param[5].Value = fsr.segment[1].originAirport;
        //        param[6] = new SqlParameter("@retDest", SqlDbType.VarChar, 3);
        //        param[6].Value = fsr.segment[1].destinationAirport;
        //    }
        //    else
        //    {
        //        param[5] = new SqlParameter("@retOrg", SqlDbType.VarChar, 3);
        //        param[5].Value = "";
        //        param[6] = new SqlParameter("@retDest", SqlDbType.VarChar, 3);
        //        param[6].Value = "";
        //    }

        //    param[7] = new SqlParameter("@tripType", SqlDbType.SmallInt);
        //    param[7].Value = (int)fsr.tripType;
        //    param[8] = new SqlParameter("@depDate", SqlDbType.Date);
        //    param[8].Value = fsr.segment[0].travelDate;
        //    if (fsr.segment.Count > 1)
        //    {
        //        param[9] = new SqlParameter("@retDate", SqlDbType.Date);
        //        param[9].Value = fsr.segment[1].travelDate;
        //    }
        //    else
        //    {
        //        param[9] = new SqlParameter("@retDate", SqlDbType.Date);
        //        param[9].Value = fsr.segment[0].travelDate;
        //    }

        //    param[10] = new SqlParameter("@cabinClass", SqlDbType.SmallInt);
        //    param[10].Value = (int)fsr.cabinType;
        //    param[11] = new SqlParameter("@adult", SqlDbType.SmallInt);
        //    param[11].Value = fsr.adults;
        //    param[12] = new SqlParameter("@child", SqlDbType.SmallInt);
        //    param[12].Value = fsr.child;
        //    param[13] = new SqlParameter("@infatnt", SqlDbType.SmallInt);
        //    param[13].Value = fsr.infants;
        //    param[14] = new SqlParameter("@infantWs", SqlDbType.SmallInt);
        //    param[14].Value = fsr.infantsWs;
        //    param[15] = new SqlParameter("@airline", SqlDbType.VarChar, 3);
        //    param[15].Value = fsr.airline;
        //    param[16] = new SqlParameter("@isDirectAirline", SqlDbType.Bit);
        //    param[16].Value = fsr.searchDirectFlight;
        //    param[17] = new SqlParameter("@isFlexiableDate", SqlDbType.Bit);
        //    param[17].Value = fsr.flexibleSearch;
        //    param[18] = new SqlParameter("@isNearBy", SqlDbType.Bit);
        //    param[18].Value = fsr.isNearBy;
        //    param[19] = new SqlParameter("@searchID", SqlDbType.VarChar, 50);
        //    param[19].Value = fsr.userSearchID;
        //    param[20] = new SqlParameter("@userSessionID", SqlDbType.VarChar, 50);
        //    param[20].Value = fsr.userSessionID;
        //    param[21] = new SqlParameter("@userIP", SqlDbType.VarChar, 20);
        //    param[21].Value = fsr.userIP;
        //    param[22] = new SqlParameter("@serverIP", SqlDbType.VarChar, 20);
        //    param[22].Value = ip;
        //    param[23] = new SqlParameter("@bookingID", SqlDbType.Int);
        //    param[23].Value = 0;
        //    param[24] = new SqlParameter("@totalResult", SqlDbType.Int);
        //    param[24].Value = totalResult;

        //    using (SqlConnection con = DataConnection.GetConnection())
        //    {
        //        SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "usp_UserSearchHistoryInsert", param);
        //    }
        //}
        //public int UpdateUserSearchHistory(Int64 id, int siteID, string sourchMedia, string depOrg, string depDest, string retOrg, string retDest, string tripType, DateTime depDate, DateTime retDate, string cabinClass, string adult, string child, string infatnt, string infantWs, string airline, bool isDirectAirline, bool isFlexiableDate, bool isNearBy, string searchID, string userSessionID, string userIP, string serverIP, int bookingID, int totalResult, DateTime searchDateTime)
        //{
        //    SqlParameter[] param = new SqlParameter[26];
        //    param[0] = new SqlParameter("@id", SqlDbType.BigInt);
        //    param[0].Value = id;
        //    param[1] = new SqlParameter("@siteID", SqlDbType.Int);
        //    param[1].Value = siteID;
        //    param[2] = new SqlParameter("@sourchMedia", SqlDbType.VarChar, 50);
        //    param[2].Value = sourchMedia;
        //    param[3] = new SqlParameter("@depOrg", SqlDbType.VarChar, 3);
        //    param[3].Value = depOrg;
        //    param[4] = new SqlParameter("@depDest", SqlDbType.VarChar, 3);
        //    param[4].Value = depDest;
        //    param[5] = new SqlParameter("@retOrg", SqlDbType.VarChar, 3);
        //    param[5].Value = retOrg;
        //    param[6] = new SqlParameter("@retDest", SqlDbType.VarChar, 3);
        //    param[6].Value = retDest;
        //    param[7] = new SqlParameter("@tripType", SqlDbType.SmallInt);
        //    param[7].Value = tripType;
        //    param[8] = new SqlParameter("@depDate", SqlDbType.Date);
        //    param[8].Value = depDate;
        //    param[9] = new SqlParameter("@retDate", SqlDbType.Date);
        //    param[9].Value = retDate;
        //    param[10] = new SqlParameter("@cabinClass", SqlDbType.SmallInt);
        //    param[10].Value = cabinClass;
        //    param[11] = new SqlParameter("@adult", SqlDbType.SmallInt);
        //    param[11].Value = adult;
        //    param[12] = new SqlParameter("@child", SqlDbType.SmallInt);
        //    param[12].Value = child;
        //    param[13] = new SqlParameter("@infatnt", SqlDbType.SmallInt);
        //    param[13].Value = infatnt;
        //    param[14] = new SqlParameter("@infantWs", SqlDbType.SmallInt);
        //    param[14].Value = infantWs;
        //    param[15] = new SqlParameter("@airline", SqlDbType.VarChar, 3);
        //    param[15].Value = airline;
        //    param[16] = new SqlParameter("@isDirectAirline", SqlDbType.Bit);
        //    param[16].Value = isDirectAirline;
        //    param[17] = new SqlParameter("@isFlexiableDate", SqlDbType.Bit);
        //    param[17].Value = isFlexiableDate;
        //    param[18] = new SqlParameter("@isNearBy", SqlDbType.Bit);
        //    param[18].Value = isNearBy;
        //    param[19] = new SqlParameter("@searchID", SqlDbType.VarChar, 50);
        //    param[19].Value = searchID;
        //    param[20] = new SqlParameter("@userSessionID", SqlDbType.VarChar, 50);
        //    param[20].Value = userSessionID;
        //    param[21] = new SqlParameter("@userIP", SqlDbType.VarChar, 20);
        //    param[21].Value = userIP;
        //    param[22] = new SqlParameter("@serverIP", SqlDbType.VarChar, 20);
        //    param[22].Value = serverIP;
        //    param[23] = new SqlParameter("@bookingID", SqlDbType.Int);
        //    param[23].Value = bookingID;
        //    param[24] = new SqlParameter("@totalResult", SqlDbType.Int);
        //    param[24].Value = totalResult;
        //    param[25] = new SqlParameter("@searchDateTime", SqlDbType.DateTime);
        //    param[25].Value = searchDateTime;
        //    using (SqlConnection con = DataConnection.GetConnection())
        //    {
        //        return SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "usp_UserSearchHistoryUpdate", param);
        //    }
        //}
        //public int DeleteUserSearchHistory(Int64 id)
        //{
        //    SqlParameter[] param = new SqlParameter[1];
        //    param[0] = new SqlParameter("@id", SqlDbType.BigInt);
        //    param[0].Value = id;
        //    using (SqlConnection con = DataConnection.GetConnection())
        //    {
        //        return SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "usp_UserSearchHistoryDelete", param);
        //    }
        //}
    }

}

