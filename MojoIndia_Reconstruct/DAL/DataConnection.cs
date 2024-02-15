using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DataConnection
/// </summary>
public class DataConnection
{
    private DataConnection()
    {

    }
    public static SqlConnection GetConnection()
    {
        SqlConnection Con = new SqlConnection();
        Con.ConnectionString = ConfigurationManager.ConnectionStrings["con"].ToString();//ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        return Con;
    }
    public static SqlConnection GetConnectionFareCaching()
    {
        SqlConnection Con = new SqlConnection();
        Con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionFareCaching"].ToString();//ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        return Con;
    }

}
