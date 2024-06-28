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
        Con.ConnectionString = ConfigurationManager.ConnectionStrings["con"].ToString();
        return Con;
    }
    public static SqlConnection GetConnectionFareCaching()
    {
        SqlConnection Con = new SqlConnection();
        Con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionFareCaching"].ToString();
        return Con;
    }


    public static SqlConnection GetConMetaRank()
    {
        SqlConnection Con = new SqlConnection();
        Con.ConnectionString = ConfigurationManager.ConnectionStrings["ConMetaRank"].ToString();
        return Con;
    }

    public static SqlConnection GetConSearchHistoryAndDeal_RDS()
    {
        SqlConnection Con = new SqlConnection();
        Con.ConnectionString = ConfigurationManager.ConnectionStrings["ConSearchHistoryAndDeal_RDS"].ToString();
        return Con;
    }

    public static SqlConnection GetConnectionUS()
    {
        SqlConnection Con = new SqlConnection();
        Con.ConnectionString = ConfigurationManager.ConnectionStrings["conUs"].ToString();
        return Con;
    }
}
