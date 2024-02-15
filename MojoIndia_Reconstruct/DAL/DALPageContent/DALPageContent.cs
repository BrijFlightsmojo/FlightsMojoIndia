using Core;
using Core.ContentPage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALPageContent
    {
        public PageContent PageContentForWebsite(SiteId siteID, PageType PageID, bool isActive, string counter)
        {
            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@siteID", SqlDbType.Int);
            param[0].Value = (int)siteID;

            param[1] = new SqlParameter("@PageID", SqlDbType.Int);
            param[1].Value = (int)PageID;

            param[2] = new SqlParameter("@isActive", SqlDbType.Bit);
            param[2].Value = isActive;

            param[3] = new SqlParameter("@Counter", SqlDbType.VarChar, 50);
            param[3].Value = counter;

            using (SqlConnection con = DataConnection.GetConnection())
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "spPageContent_SELECT", param);
                PageContent Pcontent = new PageContent();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                Pcontent.PageTitle = ds.Tables[0].Rows[0]["PageTitle"].ToString();
                Pcontent.MetaDescription = ds.Tables[0].Rows[0]["MetaDescription"].ToString();
                Pcontent.MetaKeyWord = ds.Tables[0].Rows[0]["MetaKeyWord"].ToString();
                Pcontent.BannerHeading = ds.Tables[0].Rows[0]["BannerHeading"].ToString();
                Pcontent.DealHeading = ds.Tables[0].Rows[0]["DealHeading"].ToString();
                Pcontent.AboutHeading = ds.Tables[0].Rows[0]["AboutHeading"].ToString();
                Pcontent.AboutDescription = ds.Tables[0].Rows[0]["AboutDescription"].ToString();
                Pcontent.Heading1 = ds.Tables[0].Rows[0]["Heading1"].ToString();
                Pcontent.Heading2 = ds.Tables[0].Rows[0]["Heading2"].ToString();
                Pcontent.Heading3 = ds.Tables[0].Rows[0]["Heading3"].ToString();
                Pcontent.Heading4 = ds.Tables[0].Rows[0]["Heading4"].ToString();
                Pcontent.Heading5 = ds.Tables[0].Rows[0]["Heading5"].ToString();
                Pcontent.Heading1Description = ds.Tables[0].Rows[0]["Heading1Description"].ToString();
                Pcontent.Heading2Description = ds.Tables[0].Rows[0]["Heading2Description"].ToString();
                Pcontent.Heading3Description = ds.Tables[0].Rows[0]["Heading3Description"].ToString();
                Pcontent.Heading4Description = ds.Tables[0].Rows[0]["Heading4Description"].ToString();
                Pcontent.Heading5Description = ds.Tables[0].Rows[0]["Heading5Description"].ToString();
                Pcontent.FAQQ1 = ds.Tables[0].Rows[0]["FAQQ1"].ToString();
                Pcontent.FAQQ2 = ds.Tables[0].Rows[0]["FAQQ2"].ToString();
                Pcontent.FAQQ3 = ds.Tables[0].Rows[0]["FAQQ3"].ToString();
                Pcontent.FAQQ4 = ds.Tables[0].Rows[0]["FAQQ4"].ToString();
                Pcontent.FAQQ5 = ds.Tables[0].Rows[0]["FAQQ5"].ToString();
                Pcontent.FAQQ6 = ds.Tables[0].Rows[0]["FAQQ6"].ToString();
                Pcontent.FAQQ7 = ds.Tables[0].Rows[0]["FAQQ7"].ToString();
                Pcontent.FAQQ8 = ds.Tables[0].Rows[0]["FAQQ8"].ToString();
                Pcontent.FAQQ9 = ds.Tables[0].Rows[0]["FAQQ9"].ToString();
                Pcontent.FAQQ10 = ds.Tables[0].Rows[0]["FAQQ10"].ToString();
                Pcontent.FAQANS1 = ds.Tables[0].Rows[0]["FAQANS1"].ToString();
                Pcontent.FAQANS2 = ds.Tables[0].Rows[0]["FAQANS2"].ToString();
                Pcontent.FAQANS3 = ds.Tables[0].Rows[0]["FAQANS3"].ToString();
                Pcontent.FAQANS4 = ds.Tables[0].Rows[0]["FAQANS4"].ToString();
                Pcontent.FAQANS5 = ds.Tables[0].Rows[0]["FAQANS5"].ToString();
                Pcontent.FAQANS6 = ds.Tables[0].Rows[0]["FAQANS6"].ToString();
                Pcontent.FAQANS7 = ds.Tables[0].Rows[0]["FAQANS7"].ToString();
                Pcontent.FAQANS8 = ds.Tables[0].Rows[0]["FAQANS8"].ToString();
                Pcontent.FAQANS9 = ds.Tables[0].Rows[0]["FAQANS9"].ToString();
                Pcontent.FAQANS10 = ds.Tables[0].Rows[0]["FAQANS10"].ToString();
                Pcontent.isStaticContent = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["isStaticContent"].ToString()) ? true : Convert.ToBoolean(ds.Tables[0].Rows[0]["isStaticContent"]);
                //content.ResponseStatus = new Core.ResponseStatus();
                //content.ResponseStatus.status = Core.TransactionStatus.Success;
                //content.ResponseStatus.message = "Data pull properly, Please take action!!";

                //if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                //{
                //    content.websiteFareDeal = new List<WebsiteFareDeal>();
                //    foreach (DataRow dr in ds.Tables[1].Rows)
                //    {
                //        WebsiteFareDeal deal = new WebsiteFareDeal()
                //        {
                //            airline = Core.FlightUtility.GetAirline(dr["airline"].ToString()),
                //            origin = FlightUtility.GetAirport(dr["origin"].ToString()),
                //            destination = FlightUtility.GetAirport(dr["destination"].ToString()),
                //            tripType = ((TripType)(string.IsNullOrEmpty(dr["tripType"].ToString()) ? 0 : Convert.ToInt32(dr["tripType"]))).ToString(),
                //            cabinClass = ((CabinType)(string.IsNullOrEmpty(dr["cabinClass"].ToString()) ? 0 : Convert.ToInt32(dr["cabinClass"]))).ToString(),
                //            totalFare = (string.IsNullOrEmpty(dr["totalFare"].ToString()) ? 0 : Convert.ToDecimal(dr["totalFare"])).ToString("f0"),
                //            depDate = (string.IsNullOrEmpty(dr["depDate"].ToString()) ? DateTime.Today : Convert.ToDateTime(dr["depDate"])),
                //            retDate = (string.IsNullOrEmpty(dr["retDate"].ToString()) ? DateTime.Today : Convert.ToDateTime(dr["retDate"]))
                //        };
                //        content.websiteFareDeal.Add(deal);
                //    }
                //}

                //else
                //{
                //    content.ResponseStatus = new Core.ResponseStatus();
                //    content.ResponseStatus.status = Core.TransactionStatus.Error;
                //    content.ResponseStatus.message = "Data not pull properly";
                //}

                return Pcontent;
            }
          
        }
    }
}
