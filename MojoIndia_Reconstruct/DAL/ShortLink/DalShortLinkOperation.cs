using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ShortLink
{
    public class DalShortLinkOperation
    {
        public int SaveSearchDetails( string resultData)
        {
            int ret = 0;
            using (SqlConnection con = DataConnection.GetConFlightsmojoindia_RDS())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "Get_Set_Webengage_ShortLink";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Details", resultData);

                        SqlParameter param = new SqlParameter("@rid", SqlDbType.Int);                     
                        param.Direction = ParameterDirection.InputOutput;
                        cmd.Parameters.Add(param);
                        cmd.Parameters.AddWithValue("@Counter", "insert");
                        con.Open();

                    int kk=    cmd.ExecuteNonQuery();
                        if(kk>0)
                        ret = Convert.ToInt32(param.Value);

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
            return ret;
        }
        public string getSearchDetails(int SearchID)
        {
            string data = "";
            using (SqlConnection con = DataConnection.GetConFlightsmojoindia_RDS())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "Get_Set_Webengage_ShortLink";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", SearchID);
                        cmd.Parameters.AddWithValue("@Counter", "select");
                        con.Open();

                       var kk = cmd.ExecuteScalar() ;
                        data = Convert.ToString(kk);
                    }
                    catch
                    {

                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }

                }
            }
            return data;         
        }
    }
}
