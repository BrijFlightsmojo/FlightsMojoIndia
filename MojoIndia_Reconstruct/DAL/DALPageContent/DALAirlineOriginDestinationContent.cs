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
    public class DALAirlineOriginDestinationContent
	{
		public AirlineOriginDestinationContent GetAirlineOriginDestinationContent(string airlineCode, string originCode, string destinationCode)
		{
			SqlParameter[] param = new SqlParameter[5];			

			param[0] = new SqlParameter("@SiteID", SqlDbType.Int);
			param[0].Value = SiteId.FlightsMojo;

			param[1] = new SqlParameter("@ProductID", SqlDbType.Int);
			param[1].Value = ProductId.Flight;

			param[2] = new SqlParameter("@AirlineCode", SqlDbType.VarChar, 2);
			param[2].Value = airlineCode;

			param[3] = new SqlParameter("@OriginCode", SqlDbType.VarChar, 3);
			param[3].Value = originCode;

			param[4] = new SqlParameter("@DestinationCode", SqlDbType.VarChar, 3);
			param[4].Value = destinationCode;

			using (SqlConnection con = DataConnection.GetConnection())
			{
				DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "spAirlineOriginDestinationContent_SELECT", param);
				AirlineOriginDestinationContent content = new AirlineOriginDestinationContent();
				List<DomesticRoute> domesticRouteList = new List<DomesticRoute>();
				List<InternationalRoute> internationalRouteList = new List<InternationalRoute>();

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					#region All ailine origin & Destination content
					content.ID_AirlineOriginDestinationContent = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_AirlineOriginDestinationContent"]);
					content.ID_Site = (SiteId)(ds.Tables[0].Rows[0]["ID_Site"]);
					content.ID_Product = (ProductId)(ds.Tables[0].Rows[0]["ID_Product"]);

					content.AirlineCode = ds.Tables[0].Rows[0]["AirlineCode"].ToString();
					content.AirlineName = ds.Tables[0].Rows[0]["AirlineName"].ToString();

					content.OriginCode = ds.Tables[0].Rows[0]["OriginCode"].ToString();
					content.OriginName = ds.Tables[0].Rows[0]["OriginName"].ToString();
					content.DestinationCode = ds.Tables[0].Rows[0]["DestinationCode"].ToString();
					content.DestinationName = ds.Tables[0].Rows[0]["DestinationName"].ToString();

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
					content.AboutODHeading = ds.Tables[0].Rows[0]["AboutODHeading"].ToString();
					content.AboutODDescription = ds.Tables[0].Rows[0]["AboutODDescription"].ToString();
					content.Heading1 = ds.Tables[0].Rows[0]["Heading1"].ToString();
					content.Heading1Description = ds.Tables[0].Rows[0]["Heading1Description"].ToString();
					content.Heading2 = ds.Tables[0].Rows[0]["Heading2"].ToString();
					content.Heading2Description = ds.Tables[0].Rows[0]["Heading2Description"].ToString();

					content.DomesticRoute = ds.Tables[0].Rows[0]["DomesticRoute"].ToString();
					content.InternationalRoute = ds.Tables[0].Rows[0]["InternationalRoute"].ToString();
					content.OtherPopularAirline = ds.Tables[0].Rows[0]["OtherPopularAirline"].ToString();

					content.isActivePromotional = Convert.ToBoolean(ds.Tables[0].Rows[0]["isActivePromotional"]);
					content.PromotionalHeader = ds.Tables[0].Rows[0]["PromotionalHeader"].ToString();
					content.PromotionalContent = ds.Tables[0].Rows[0]["PromotionalContent"].ToString();
					content.PromotionalCouponCode = ds.Tables[0].Rows[0]["PromotionalCouponCode"].ToString();
					content.PromotionalInstantOff = Convert.ToDouble(ds.Tables[0].Rows[0]["PromotionalInstantOff"]);
					content.PromotionalReview = ds.Tables[0].Rows[0]["PromotionalReview"].ToString();

					content.isStaticContent = Convert.ToBoolean(ds.Tables[0].Rows[0]["isStaticContent"]);
					content.isActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["isActive"]);
					content.InsertedBy = Convert.ToInt32(ds.Tables[0].Rows[0]["InsertedBy"]);
					content.InsertedOn = Convert.ToDateTime(ds.Tables[0].Rows[0]["InsertedOn"]);
					content.ModifiedBy = Convert.ToInt32(ds.Tables[0].Rows[0]["ModifiedBy"]);
					content.ModifiedOn = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedOn"]);

					content.ResponseStatus = new Core.ResponseStatus();
					content.ResponseStatus.messege = "Data pull properly, Please take action!!";
					#endregion All ailine origin & Destination content

					if(ds.Tables[1].Rows.Count > 0)
					{
						#region Domestic Rout detail               
						foreach (DataRow dr in ds.Tables[1].Rows)
						{
							DomesticRoute domesticRoute = new DomesticRoute();
							domesticRoute.AirlineCode = dr["AirlineCode"].ToString();
							domesticRoute.OriginName = dr["OriginName"].ToString();
							domesticRoute.OriginCode = dr["OriginCode"].ToString();
							domesticRoute.DestinationName = dr["DestinationName"].ToString();
							domesticRoute.DestinationCode = dr["DestinationCode"].ToString();

							domesticRouteList.Add(domesticRoute);
						}
						#endregion End Domestic Rout detail   
					}
					if (ds.Tables[2].Rows.Count > 0)
					{
						#region International Rout detail   
						foreach (DataRow dr in ds.Tables[2].Rows)
						{
							InternationalRoute internationalRoute = new InternationalRoute();
							internationalRoute.AirlineCode = dr["AirlineCode"].ToString();
							internationalRoute.OriginName = dr["OriginName"].ToString();
							internationalRoute.OriginCode = dr["OriginCode"].ToString();
							internationalRoute.DestinationName = dr["DestinationName"].ToString();
							internationalRoute.DestinationCode = dr["DestinationCode"].ToString();

							internationalRouteList.Add(internationalRoute);
						}
						#endregion International Rout detail   
					}

					content.DomesticRouteList = domesticRouteList;
					content.InternationalRouteList = internationalRouteList;
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