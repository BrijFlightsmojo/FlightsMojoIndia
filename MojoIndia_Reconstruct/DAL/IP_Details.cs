using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class IP_Details
    {
        public bool isExistIP(string IP)
        {
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@ip", SqlDbType.VarChar, 50);
            param[0].Value = IP;

            param[1] = new SqlParameter("@Counter", SqlDbType.VarChar, 500);
            param[1].Value = "isExist";
            using (SqlConnection con = DataConnection.GetConnectionUS())
            {
                var kk = SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "GET_SET_ip_Details", param);
                if (Convert.ToInt32(kk) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Core.IpDetails GetIpDetails(string IP)
        {
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@ip", SqlDbType.VarChar, 50);
            param[0].Value = IP;

            param[1] = new SqlParameter("@Counter", SqlDbType.VarChar, 500);
            param[1].Value = "SELECT";
            using (SqlConnection con = DataConnection.GetConnectionUS())
            {
                SqlDataReader dr = SqlHelper.ExecuteReader(con, CommandType.StoredProcedure, "GET_SET_ip_Details", param);
                if (dr.HasRows)
                {
                    Core.IpDetails obj = new Core.IpDetails();
                    while (dr.Read())
                    {
                        obj.ip = dr["ip"].ToString();
                        obj.type = dr["type"].ToString();
                        obj.continent_code = dr["continent_code"].ToString();
                        obj.continent_name = dr["continent_name"].ToString();
                        obj.country_code = dr["country_code"].ToString();
                        obj.country_name = dr["country_name"].ToString();
                        obj.region_code = dr["region_code"].ToString();
                        obj.region_name = dr["region_name"].ToString();
                        obj.city = dr["city"].ToString();
                        obj.zip = dr["zip"].ToString();
                        string ss = dr["latitude"].ToString();
                        obj.latitude = string.IsNullOrEmpty(ss) ? 0 : Convert.ToDouble(ss);
                        string ss1 = dr["longitude"].ToString();
                        obj.longitude = string.IsNullOrEmpty(ss1) ? 0 : Convert.ToDouble(ss1);


                    }
                    return obj;
                }
                else { return null; }
            }
            return null;
        }
        public void saveIpDetails(Core.IpDetails ipDetails)
        {
            SqlParameter[] param = new SqlParameter[13];
            param[0] = new SqlParameter("@ip", SqlDbType.VarChar, 50);
            param[0].Value = ipDetails.ip;
            param[1] = new SqlParameter("@type", SqlDbType.VarChar, 50);
            param[1].Value = ipDetails.type;
            param[2] = new SqlParameter("@continent_code", SqlDbType.VarChar, 3);
            param[2].Value = ipDetails.continent_code;
            param[3] = new SqlParameter("@continent_name", SqlDbType.VarChar, 50);
            param[3].Value = ipDetails.continent_name;
            param[4] = new SqlParameter("@country_code", SqlDbType.VarChar, 3);
            param[4].Value = ipDetails.country_code;
            param[5] = new SqlParameter("@country_name", SqlDbType.VarChar, 50);
            param[5].Value = ipDetails.country_name;
            param[6] = new SqlParameter("@region_code", SqlDbType.VarChar, 3);
            param[6].Value = ipDetails.region_code;
            param[7] = new SqlParameter("@region_name", SqlDbType.VarChar, 50);
            param[7].Value = ipDetails.region_name;
            param[8] = new SqlParameter("@city", SqlDbType.VarChar, 50);
            param[8].Value = ipDetails.city;
            param[9] = new SqlParameter("@zip", SqlDbType.VarChar, 10);
            param[9].Value = ipDetails.zip;
            param[10] = new SqlParameter("@latitude", SqlDbType.VarChar, 50);
            param[10].Value = ipDetails.latitude;
            param[11] = new SqlParameter("@longitude", SqlDbType.VarChar, 50);
            param[11].Value = ipDetails.longitude;
            param[12] = new SqlParameter("@Counter", SqlDbType.VarChar, 500);
            param[12].Value = "INSERT";

            using (SqlConnection con = DataConnection.GetConnectionUS())
            {
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "GET_SET_ip_Details", param);
            }
        }
    }
}
