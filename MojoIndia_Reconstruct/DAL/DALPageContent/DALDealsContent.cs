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
    public class DALDealsContent
    {
        public DealsContent GetDealsContent(string DealCode, int SiteID, bool isActive, string Counter)
        {
            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@ThemeCode", SqlDbType.VarChar, 50);
            param[0].Value = DealCode;

            param[1] = new SqlParameter("@SiteID", SqlDbType.Int);
            param[1].Value = (int)SiteID;

            param[2] = new SqlParameter("@isActive", SqlDbType.Bit);
            param[2].Value = isActive;

            param[3] = new SqlParameter("@Counter", SqlDbType.VarChar, 50);
            param[3].Value = Counter;

            using (SqlConnection con = DataConnection.GetConnection())
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ThemesContent_SELECT", param);
                DealsContent content = new DealsContent();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    content.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
                    content.SiteID = Convert.ToInt32(ds.Tables[0].Rows[0]["SiteID"]);
                    content.ThemeCode = ds.Tables[0].Rows[0]["ThemeCode"].ToString();
                    content.ThemeName = ds.Tables[0].Rows[0]["ThemeName"].ToString();
                    content.PageTitle = ds.Tables[0].Rows[0]["PageTitle"].ToString();
                    content.MetaDescription = ds.Tables[0].Rows[0]["MetaDescription"].ToString();
                    content.MetaKeyWord = ds.Tables[0].Rows[0]["MetaKeyWord"].ToString();
                    content.BannerHeading = ds.Tables[0].Rows[0]["BannerHeading"].ToString();
                    content.DealHeading = ds.Tables[0].Rows[0]["DealHeading"].ToString();
                    content.AboutThemeHeading = ds.Tables[0].Rows[0]["AboutThemeHeading"].ToString();
                    content.ThemeSubContentHeading1 = ds.Tables[0].Rows[0]["ThemeSubContentHeading1"].ToString();
                    content.ThemeSubContentHeading2 = ds.Tables[0].Rows[0]["ThemeSubContentHeading2"].ToString();
                    content.ThemeSubContentHeading3 = ds.Tables[0].Rows[0]["ThemeSubContentHeading3"].ToString();
                    content.ThemeSubContentHeading4 = ds.Tables[0].Rows[0]["ThemeSubContentHeading4"].ToString();
                    content.ThemeSubContentHeading5 = ds.Tables[0].Rows[0]["ThemeSubContentHeading5"].ToString();
                    content.AboutThemeDescription = ds.Tables[0].Rows[0]["AboutThemeDescription"].ToString();
                    content.ThemeSubDescription1 = ds.Tables[0].Rows[0]["ThemeSubDescription1"].ToString();
                    content.ThemeSubDescription2 = ds.Tables[0].Rows[0]["ThemeSubDescription2"].ToString();
                    content.ThemeSubDescription3 = ds.Tables[0].Rows[0]["ThemeSubDescription3"].ToString();
                    content.ThemeSubDescription4 = ds.Tables[0].Rows[0]["ThemeSubDescription4"].ToString();
                    content.ThemeSubDescription5 = ds.Tables[0].Rows[0]["ThemeSubDescription5"].ToString();

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
                    content.FAQQ11 = ds.Tables[0].Rows[0]["FAQQ11"].ToString();
                    content.FAQQ12 = ds.Tables[0].Rows[0]["FAQQ12"].ToString();
                    content.FAQQ13 = ds.Tables[0].Rows[0]["FAQQ13"].ToString();
                    content.FAQQ14 = ds.Tables[0].Rows[0]["FAQQ14"].ToString();
                    content.FAQQ15 = ds.Tables[0].Rows[0]["FAQQ15"].ToString();
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
                    content.FAQANS11 = ds.Tables[0].Rows[0]["FAQANS11"].ToString();
                    content.FAQANS12 = ds.Tables[0].Rows[0]["FAQANS12"].ToString();
                    content.FAQANS13 = ds.Tables[0].Rows[0]["FAQANS13"].ToString();
                    content.FAQANS14 = ds.Tables[0].Rows[0]["FAQANS14"].ToString();
                    content.FAQANS15 = ds.Tables[0].Rows[0]["FAQANS15"].ToString();
                    content.ResponseStatus = new Core.ResponseStatus();

                }
                else
                {
                    content.ResponseStatus = new Core.ResponseStatus();
                    content.ResponseStatus.status = Core.TransactionStatus.Error;
                }
                return content;
            }
        }
    }
}
