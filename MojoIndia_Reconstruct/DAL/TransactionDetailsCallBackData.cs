using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TransactionDetailsCallBackData
    {
        public string getTransactonData(long Trns_No)
        {
            string base64String = "";
            using (SqlConnection con = DataConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "usp_TransactionDetailsCallBackDataSelectOnlyData";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Trns_No", Trns_No);
                        con.Open();

                        Byte[] content = cmd.ExecuteScalar() as Byte[];
                        if (content != null)
                            base64String = System.Text.Encoding.UTF8.GetString(content);
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }

                }
            }
            return base64String;
        }
        public string getTransactonData(long Trns_No, long bookingID)
        {
            string base64String = "";
            using (SqlConnection con = DataConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "usp_TransactionDetailsCallBackDataSelectOnlyData";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (Trns_No > 0)
                            cmd.Parameters.AddWithValue("@Trns_No", Trns_No);
                        cmd.Parameters.AddWithValue("@BookingID", bookingID);
                        con.Open();

                        Byte[] content = cmd.ExecuteScalar() as Byte[];
                        if (content != null)
                            base64String = System.Text.Encoding.UTF8.GetString(content);
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }

                }
            }
            return base64String;
        }
        public void SaveTransactonData(long BookingID, long Trns_No, string RequestData)
        {
            using (SqlConnection con = DataConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "usp_TransactionDetailsCallBackDataInsert";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BookingID", BookingID);
                        cmd.Parameters.AddWithValue("@Trns_No", Trns_No);
                        cmd.Parameters.AddWithValue("@RequestData", System.Text.Encoding.UTF8.GetBytes(RequestData));
                        con.Open();

                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
        }

        public DataSet getTransactionDetails(long Trns_No)
        {
            using (SqlConnection con = DataConnection.GetConnection())
            {
                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@Trns_No", SqlDbType.Int);
                param[0].Value = Trns_No;
                try
                {
                    return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Get_TransactionDetails", param);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        //public static string Get(int id, int BookingID, int Trns_No)
        //{
        //    SqlParameter[] param = new SqlParameter[3];
        //    if (id > 0)
        //    {
        //        param[0] = new SqlParameter("@id", SqlDbType.Int);
        //        param[0].Value = id;
        //    }
        //    if (BookingID > 0)
        //    {
        //        param[1] = new SqlParameter("@BookingID", SqlDbType.Int);
        //        param[1].Value = BookingID;
        //    }
        //    if (Trns_No > 0)
        //    {
        //        param[2] = new SqlParameter("@Trns_No", SqlDbType.Int);
        //        param[2].Value = Trns_No;
        //    }
        //    using (SqlConnection con = DataConnection.GetConnection())
        //    {
        //        return SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "usp_TransactionDetailsCallBackDataSelect", param).ToString();
        //    }
        //}
        //public static int SaveTransactionDetailsCallBackData(int BookingID, int Trns_No, string RequestData)
        //{
        //    SqlParameter[] param = new SqlParameter[3];

        //    param[0] = new SqlParameter("@BookingID", SqlDbType.Int);
        //    param[0].Value = BookingID;
        //    param[1] = new SqlParameter("@Trns_No", SqlDbType.Int);
        //    param[1].Value = Trns_No;
        //    param[3] = new SqlParameter("@RequestData", SqlDbType.VarBinary);
        //    param[3].Value = RequestData;

        //    using (SqlConnection con = DataConnection.GetConnection())
        //    {
        //        return SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "usp_TransactionDetailsCallBackDataInsert", param);
        //    }
        //}
    }
}
