using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Core.ContentPage;
using Core;

/// <summary>
/// Summary description for WebsiteContentData
/// </summary>
namespace Dal
{
	public class DALAirportContent
    {
        public static AirportContentSummeryDetails Get(int ID, string AirportCode, int WebsiteID, int PageType, string counter)
        {
            SqlParameter[] param = new SqlParameter[5];
            if (ID > 0)
            {
                param[0] = new SqlParameter("@ID", SqlDbType.Int);
                param[0].Value = ID;
            }
            if (!string.IsNullOrEmpty(AirportCode))
            {
                param[1] = new SqlParameter("@AirportCode", SqlDbType.VarChar, 50);
                param[1].Value = AirportCode;
            }
            if (WebsiteID > 0)
            {
                param[2] = new SqlParameter("@WebsiteID", SqlDbType.Int);
                param[2].Value = WebsiteID;
            }
            if (PageType > 0)
            {
                param[3] = new SqlParameter("@PageType", SqlDbType.Int);
                param[3].Value = PageType;
            }
            if (!string.IsNullOrEmpty(counter))
            {
                param[4] = new SqlParameter("@counter", SqlDbType.VarChar, 50);
                param[4].Value = counter;
            }
            using (SqlConnection con = DataConnection.GetConnection())
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_AirportContentDataSelect", param);
				AirportContentSummeryDetails obj = new AirportContentSummeryDetails();
                obj.data = new List<AirportContentSummery>();
                obj.ResponseStatus = new Core.ResponseStatus();

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
						AirportContentSummery content = new AirportContentSummery();

                        content.ID = dr["ID"].ToString();
                        content.AirportCode = dr["AirportCode"].ToString();
                        content.AirportName = dr["AirportName"].ToString();
                        content.WebsiteID = dr["WebsiteID"].ToString();
                        content.PageType = dr["PageType"].ToString();
                        content.ModifyBy = dr["ModifyBy"].ToString();
                        content.ModifyOn = Convert.ToDateTime(dr["ModifyOn"]).ToString("dd MMM yy HH:mm");
                        content.isActive = dr["isActive"].ToString();

                        obj.data.Add(content);
                    }
                    if (obj.data.Count == 0)
                    {
                        obj.ResponseStatus.status = Core.TransactionStatus.Error;
                        obj.ResponseStatus.messege = "No Data!!";
                    }
                }
                else
                {
                    obj.ResponseStatus.status = Core.TransactionStatus.Error;
                    obj.ResponseStatus.messege = "Data not pull properly";
                }
                return obj;
            }
        }
        public static AirportContent Get(string airportCode)
        {
            SqlParameter[] param = new SqlParameter[3];
			
			if (!string.IsNullOrEmpty(airportCode))
			{
				param[0] = new SqlParameter("@AirportCode", SqlDbType.VarChar, 50);
				param[0].Value = airportCode;
			}

			param[1] = new SqlParameter("@WebsiteID", SqlDbType.Int);
			param[1].Value = SiteId.FlightsMojo;

			param[2] = new SqlParameter("@counter", SqlDbType.VarChar, 50);
			param[2].Value = "SelectAll";

			using (SqlConnection con = DataConnection.GetConnection())
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_AirportContentDataSelect", param);
				AirportContent content = new AirportContent();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    content.ID = ds.Tables[0].Rows[0]["ID"].ToString();
                    content.AirportCode = ds.Tables[0].Rows[0]["AirportCode"].ToString();
                    content.AirportName = ds.Tables[0].Rows[0]["AirportName"].ToString();
                    content.WebsiteID = ds.Tables[0].Rows[0]["WebsiteID"].ToString();
                    content.PageType = ds.Tables[0].Rows[0]["PageType"].ToString();
                    content.SearchEngineType = ds.Tables[0].Rows[0]["SearchEngineType"].ToString();
                    content.SearchEngineFrom = ds.Tables[0].Rows[0]["SearchEngineFrom"].ToString();
                    content.SearchEngineTo = ds.Tables[0].Rows[0]["SearchEngineTo"].ToString();
                    content.SearchEngineAirline = ds.Tables[0].Rows[0]["SearchEngineAirline"].ToString();
                    content.SearchEngineCabin = ds.Tables[0].Rows[0]["SearchEngineCabin"].ToString();
                    content.PageTitle = ds.Tables[0].Rows[0]["PageTitle"].ToString();
                    content.MetaDescription = ds.Tables[0].Rows[0]["MetaDescription"].ToString();
                    content.MetaKeyWord = ds.Tables[0].Rows[0]["MetaKeyWord"].ToString();
                    content.Header1 = ds.Tables[0].Rows[0]["Header1"].ToString();
                    content.Header2 = ds.Tables[0].Rows[0]["Header2"].ToString();
                    content.Header3 = ds.Tables[0].Rows[0]["Header3"].ToString();
                    content.Header4 = ds.Tables[0].Rows[0]["Header4"].ToString();
                    content.Header5 = ds.Tables[0].Rows[0]["Header5"].ToString();
                    content.Header6 = ds.Tables[0].Rows[0]["Header6"].ToString();
                    content.PopularAirlineFrom = ds.Tables[0].Rows[0]["PopularAirlineFrom"].ToString();
                    content.PopularAirlineTo = ds.Tables[0].Rows[0]["PopularAirlineTo"].ToString();
                    content.PopularDestination = ds.Tables[0].Rows[0]["PopularDestination"].ToString();
                    content.NearByAirportFrom = ds.Tables[0].Rows[0]["NearByAirportFrom"].ToString();
                    content.NearByAirportTo = ds.Tables[0].Rows[0]["NearByAirportTo"].ToString();
                    content.PageContent = ds.Tables[0].Rows[0]["PageContent"].ToString();
                    content.SubContent1 = ds.Tables[0].Rows[0]["SubContent1"].ToString();
                    content.SubContent2 = ds.Tables[0].Rows[0]["SubContent2"].ToString();
                    content.SubContent3 = ds.Tables[0].Rows[0]["SubContent3"].ToString();
                    content.SubContent4 = ds.Tables[0].Rows[0]["SubContent4"].ToString();
                    content.SubContent5 = ds.Tables[0].Rows[0]["SubContent5"].ToString();
                    content.SubContent6 = ds.Tables[0].Rows[0]["SubContent6"].ToString();
                    content.SubContent7 = ds.Tables[0].Rows[0]["SubContent7"].ToString();
                    content.isActivePromotional = ds.Tables[0].Rows[0]["isActivePromotional"].ToString();
                    content.PromotionalHeader = ds.Tables[0].Rows[0]["PromotionalHeader"].ToString();
                    content.PromotionalContent = ds.Tables[0].Rows[0]["PromotionalContent"].ToString();
                    content.PromotionalCouponCode = ds.Tables[0].Rows[0]["PromotionalCouponCode"].ToString();
                    content.PromotionalInstantOff = ds.Tables[0].Rows[0]["PromotionalInstantOff"].ToString();
                    content.PromotionalReview = ds.Tables[0].Rows[0]["PromotionalReview"].ToString();
                    content.AirlineServingByAirport = ds.Tables[0].Rows[0]["AirlineServingByAirport"].ToString();
                    content.PopularDealID = ds.Tables[0].Rows[0]["PopularDealID"].ToString();
                    content.isStaticContent = ds.Tables[0].Rows[0]["isStaticContent"].ToString();
                    content.isActive = ds.Tables[0].Rows[0]["isActive"].ToString();
                    content.ModifyBy = ds.Tables[0].Rows[0]["ModifyBy"].ToString();
                    content.ModifyOn = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifyOn"]).ToString("dd MMM yy HH:mm");
                    content.ResponseStatus = new Core.ResponseStatus();
                    content.ResponseStatus.messege = "Data pull properly, Please take action!!";
                }
                else
                {
                    content.ResponseStatus = new Core.ResponseStatus();
                    content.ResponseStatus.status = Core.TransactionStatus.Error;
                    content.ResponseStatus.messege = "Data not pull properly";
                }
                return content;
            }
        }
    }
}