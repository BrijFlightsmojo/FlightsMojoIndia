using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Core;
using Core.ContentPage;

/// <summary>
/// Summary description for WebsiteContentData
/// </summary>
namespace Dal
{
    public class DALAirlineContent
    {
        public AirlineContent GetAirlineContent(string AirlineCode, int SiteID, bool isActive, string Counter)
        {
            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@AirlineCode", SqlDbType.VarChar, 50);
            param[0].Value = AirlineCode;

            param[1] = new SqlParameter("@SiteID", SqlDbType.Int);
            param[1].Value = (int)SiteID;

            param[2] = new SqlParameter("@isActive", SqlDbType.Bit);
            param[2].Value = isActive;

            param[3] = new SqlParameter("@Counter", SqlDbType.VarChar, 50);
            param[3].Value = Counter;

            using (SqlConnection con = DataConnection.GetConnection())
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_AirlineContent_SELECT", param);
                AirlineContent content = new AirlineContent();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    content.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
                    content.SiteID = (SiteId)(ds.Tables[0].Rows[0]["SiteID"]);
                    content.AirlineCode = ds.Tables[0].Rows[0]["AirlineCode"].ToString();
                    content.AirlineName = ds.Tables[0].Rows[0]["AirlineName"].ToString();
                    content.PageTitle = ds.Tables[0].Rows[0]["PageTitle"].ToString();
                    content.MetaDescription = ds.Tables[0].Rows[0]["MetaDescription"].ToString();
                    content.MetaKeyWord = ds.Tables[0].Rows[0]["MetaKeyWord"].ToString();
                    content.BannerHeading = ds.Tables[0].Rows[0]["BannerHeading"].ToString();
                    content.DealHeading = ds.Tables[0].Rows[0]["DealHeading"].ToString();
                    content.AboutAirlineHeading = ds.Tables[0].Rows[0]["AboutAirlineHeading"].ToString();
                    content.AirlineSubContentHeading1 = ds.Tables[0].Rows[0]["AirlineSubContentHeading1"].ToString();
                    content.AirlineSubContentHeading2 = ds.Tables[0].Rows[0]["AirlineSubContentHeading2"].ToString();
                    content.AirlineSubContentHeading3 = ds.Tables[0].Rows[0]["AirlineSubContentHeading3"].ToString();
                    content.AirlineSubContentHeading4 = ds.Tables[0].Rows[0]["AirlineSubContentHeading4"].ToString();
                    content.AirlineSubContentHeading5 = ds.Tables[0].Rows[0]["AirlineSubContentHeading5"].ToString();
                    content.AboutAirlineDescription = ds.Tables[0].Rows[0]["AboutAirlineDescription"].ToString();
                    content.AirlineSubDescription1 = ds.Tables[0].Rows[0]["AirlineSubDescription1"].ToString();
                    content.AirlineSubDescription2 = ds.Tables[0].Rows[0]["AirlineSubDescription2"].ToString();
                    content.AirlineSubDescription3 = ds.Tables[0].Rows[0]["AirlineSubDescription3"].ToString();
                    content.AirlineSubDescription4 = ds.Tables[0].Rows[0]["AirlineSubDescription4"].ToString();
                    content.AirlineSubDescription5 = ds.Tables[0].Rows[0]["AirlineSubDescription5"].ToString();
                    
                    content.FAQQ1 = ds.Tables[0].Rows[0]["FAQQ1"].ToString();
                    content.FAQQ2 = ds.Tables[0].Rows[0]["FAQQ2"].ToString();
                    content.FAQQ3 = ds.Tables[0].Rows[0]["FAQQ3"].ToString();
                    content.FAQQ4 = ds.Tables[0].Rows[0]["FAQQ4"].ToString();
                    content.FAQQ5 = ds.Tables[0].Rows[0]["FAQQ5"].ToString();
                    content.FAQQ6 = ds.Tables[0].Rows[0]["FAQQ6"].ToString();
                    content.FAQQ7 = ds.Tables[0].Rows[0]["FAQQ7"].ToString();
                    content.FAQQ8 = ds.Tables[0].Rows[0]["FAQQ8"].ToString();
                    content.FAQQ9 = ds.Tables[0].Rows[0]["FAQQ9"].ToString();
                    content.FAQQ10 = ds.Tables[0].Rows[0]["FAQQ10"].ToString();
                    content.FAQANS1 = ds.Tables[0].Rows[0]["FAQANS1"].ToString();
                    content.FAQANS2 = ds.Tables[0].Rows[0]["FAQANS2"].ToString();
                    content.FAQANS3 = ds.Tables[0].Rows[0]["FAQANS3"].ToString();
                    content.FAQANS4 = ds.Tables[0].Rows[0]["FAQANS4"].ToString();
                    content.FAQANS5 = ds.Tables[0].Rows[0]["FAQANS5"].ToString();
                    content.FAQANS6 = ds.Tables[0].Rows[0]["FAQANS6"].ToString();
                    content.FAQANS7 = ds.Tables[0].Rows[0]["FAQANS7"].ToString();
                    content.FAQANS8 = ds.Tables[0].Rows[0]["FAQANS8"].ToString();
                    content.FAQANS9 = ds.Tables[0].Rows[0]["FAQANS9"].ToString();
                    content.FAQANS10 = ds.Tables[0].Rows[0]["FAQANS10"].ToString();


                    content.isStaticContent = Convert.ToBoolean(ds.Tables[0].Rows[0]["isStaticContent"]);
                    content.isActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["isActive"]);
                    content.InsertedBy = Convert.ToInt32(ds.Tables[0].Rows[0]["InsertedBy"]);
                    content.InsertedOn = Convert.ToDateTime(ds.Tables[0].Rows[0]["InsertedOn"]);
                    content.ModifiedBy = Convert.ToInt32(ds.Tables[0].Rows[0]["ModifiedBy"]);
                    content.ModifiedOn = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedOn"]);
                    content.ResponseStatus = new Core.ResponseStatus();
                    content.ResponseStatus.message = "Data pull properly, Please take action!!";
                }
                else
                {
                    content.ResponseStatus = new Core.ResponseStatus();
                    content.ResponseStatus.status = Core.TransactionStatus.Error;
                    content.ResponseStatus.message = "Data not pull properly";
                }
                return content;
            }
        }
    }
}