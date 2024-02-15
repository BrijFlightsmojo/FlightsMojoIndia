using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class IdGenrator
    {
        public static long Get(string STRPREFIX)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@STRBOOKREFNO", SqlDbType.VarChar, 25);
            param[0].Direction = ParameterDirection.Output;
            param[1] = new SqlParameter("@INTBOOKREFSuffix", SqlDbType.Float);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@STRPREFIX", SqlDbType.VarChar, 25);
            param[2].Value = STRPREFIX;

            using (SqlConnection con = DataConnection.GetConnection())
            {
                SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Genrate_IDs", param);
                return System.Convert.ToInt64(param[1].Value);
            }
        }
    }
}
