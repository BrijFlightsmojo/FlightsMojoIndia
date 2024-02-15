using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Core.ContentPage;
using Core;

/// <summary>
/// Summary description for WebsiteContentData
/// </summary>
namespace Dal
{
    public class DALGenericContent
    {
        public GenericContent GetGenericContent(string GenericName)
        {
			SqlParameter[] param = new SqlParameter[3];

			param[0] = new SqlParameter("@GenericName", SqlDbType.VarChar, 50);
			param[0].Value = GenericName;

			param[1] = new SqlParameter("@SiteID", SqlDbType.Int);
			param[1].Value = SiteId.FlightsMojo;

			param[2] = new SqlParameter("@ProductID", SqlDbType.Int);
			param[2].Value = ProductId.Flight;

			using (SqlConnection con = DataConnection.GetConnection())
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "spGenericContent_SELECT", param);
				GenericContent content = new GenericContent();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

					content.ID_GenericContent = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_GenericContent"]);
					content.ID_Site = (SiteId)(ds.Tables[0].Rows[0]["ID_Site"]);
					content.ID_Product = (ProductId)(ds.Tables[0].Rows[0]["ID_Product"]);


					content.GenericCode = ds.Tables[0].Rows[0]["GenericCode"].ToString();
					content.GenericName = ds.Tables[0].Rows[0]["GenericName"].ToString();

					content.SearchEngineType = Convert.ToInt32(ds.Tables[0].Rows[0]["SearchEngineType"]);

					content.SearchEngineFrom = ds.Tables[0].Rows[0]["SearchEngineFrom"].ToString();
					content.SearchEngineTo = ds.Tables[0].Rows[0]["SearchEngineTo"].ToString();
					content.SearchEngineAirline = ds.Tables[0].Rows[0]["SearchEngineAirline"].ToString();
					content.SearchEngineCabin = Convert.ToInt32(ds.Tables[0].Rows[0]["SearchEngineCabin"]);

					content.PageTitle = ds.Tables[0].Rows[0]["PageTitle"].ToString();
					content.MetaDescription = ds.Tables[0].Rows[0]["MetaDescription"].ToString();
					content.MetaKeyWord = ds.Tables[0].Rows[0]["MetaKeyWord"].ToString();

					content.BannerHeading = ds.Tables[0].Rows[0]["BannerHeading"].ToString();
					content.DealHeading = ds.Tables[0].Rows[0]["DealHeading"].ToString();
					content.AboutHeading = ds.Tables[0].Rows[0]["AboutHeading"].ToString();
					content.AboutDescription = ds.Tables[0].Rows[0]["AboutDescription"].ToString();
					content.Heading1 = ds.Tables[0].Rows[0]["Heading1"].ToString();
					content.Heading1Description = ds.Tables[0].Rows[0]["Heading1Description"].ToString();
					content.Heading2 = ds.Tables[0].Rows[0]["Heading2"].ToString();
					content.Heading2Description = ds.Tables[0].Rows[0]["Heading2Description"].ToString();

					content.isActivePromotional = Convert.ToBoolean(ds.Tables[0].Rows[0]["isActivePromotional"]);
					content.PromotionalHeader = ds.Tables[0].Rows[0]["PromotionalHeader"].ToString();
					content.PromotionalContent = ds.Tables[0].Rows[0]["PromotionalContent"].ToString();
					content.PromotionalCouponCode = ds.Tables[0].Rows[0]["PromotionalCouponCode"].ToString();
					content.PromotionalInstantOff = Convert.ToDouble(ds.Tables[0].Rows[0]["PromotionalInstantOff"]);
					content.PromotionalReview = ds.Tables[0].Rows[0]["PromotionalReview"].ToString();

					content.PopularDealID = ds.Tables[0].Rows[0]["PopularDealID"].ToString();

					content.isStaticContent = Convert.ToBoolean(ds.Tables[0].Rows[0]["isStaticContent"]);
					content.isActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["isActive"]);
					content.InsertedBy = Convert.ToInt32(ds.Tables[0].Rows[0]["InsertedBy"]);
					content.InsertedByName = ds.Tables[0].Rows[0]["InsertedBy"].ToString();
					content.InsertedOn = Convert.ToDateTime(ds.Tables[0].Rows[0]["InsertedOn"]);
					content.ModifiedBy = Convert.ToInt32(ds.Tables[0].Rows[0]["ModifiedBy"]);
					content.ModifiedByName = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
					content.ModifiedOn = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedOn"]);


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