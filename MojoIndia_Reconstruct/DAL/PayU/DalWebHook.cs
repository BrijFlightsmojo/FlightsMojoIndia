using Core.PayU;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.PayU
{
    public class DalWebHook
    {
        public void SavePayU_WebhooksDetails(string WebhookPageType, Core.PayU.WebhookSuccessDetails objWHD)
        {
            SqlParameter[] param = new SqlParameter[25];

            param[0] = new SqlParameter("@WebhookPageType", SqlDbType.VarChar, 50);
            param[0].Value = WebhookPageType;
            param[1] = new SqlParameter("@paymentId", SqlDbType.VarChar, 50);
            param[1].Value = objWHD.paymentId;
            param[2] = new SqlParameter("@PayStatus", SqlDbType.VarChar, 100);
            param[2].Value = objWHD.status;
            param[3] = new SqlParameter("@amount", SqlDbType.Decimal);
            param[3].Value = objWHD.amount;
            param[4] = new SqlParameter("@paymentMode", SqlDbType.VarChar, 50);
            param[4].Value = objWHD.paymentMode;
            param[5] = new SqlParameter("@udf5", SqlDbType.VarChar, 100);
            param[5].Value = objWHD.udf5;
            param[6] = new SqlParameter("@udf3", SqlDbType.VarChar, 100);
            param[6].Value = objWHD.udf3;
            param[7] = new SqlParameter("@split_info", SqlDbType.VarChar, 100);
            param[7].Value = objWHD.split_info;
            param[8] = new SqlParameter("@udf4", SqlDbType.VarChar, 100);
            param[8].Value = objWHD.udf4;
            param[9] = new SqlParameter("@udf1", SqlDbType.VarChar, 100);
            param[9].Value = objWHD.udf1;
            param[10] = new SqlParameter("@udf2", SqlDbType.VarChar, 100);
            param[10].Value = objWHD.udf2;
            param[11] = new SqlParameter("@customerName", SqlDbType.VarChar, 100);
            param[11].Value = objWHD.customerName;
            param[12] = new SqlParameter("@productInfo", SqlDbType.VarChar, 1000);
            param[12].Value = objWHD.productInfo;
            param[13] = new SqlParameter("@customerPhone", SqlDbType.VarChar, 100);
            param[13].Value = objWHD.customerPhone;
            param[14] = new SqlParameter("@additionalCharges", SqlDbType.VarChar, 100);
            param[14].Value = objWHD.additionalCharges;
            param[15] = new SqlParameter("@customerEmail", SqlDbType.VarChar, 100);
            param[15].Value = objWHD.customerEmail;
            param[16] = new SqlParameter("@merchantTransactionId", SqlDbType.VarChar, 100);
            param[16].Value = objWHD.merchantTransactionId.Replace("TRN","");
            param[17] = new SqlParameter("@error_Message", SqlDbType.VarChar, 100);
            param[17].Value = objWHD.error_Message;
            param[18] = new SqlParameter("@notificationId", SqlDbType.VarChar, 100);
            param[18].Value = objWHD.notificationId;
            param[19] = new SqlParameter("@bankRefNum", SqlDbType.VarChar, 100);
            param[19].Value = objWHD.bankRefNum;
            param[20] = new SqlParameter("@hash", SqlDbType.VarChar, 500);
            param[20].Value = objWHD.hash;
            param[21] = new SqlParameter("@field4", SqlDbType.VarChar, 100);
            param[21].Value = objWHD.field4;
            param[22] = new SqlParameter("@Counter", SqlDbType.VarChar, 500);
            param[22].Value = "INSERT";
            param[23] = new SqlParameter("@Status", SqlDbType.VarChar, 500);
            param[23].Direction = ParameterDirection.Output;
            param[24] = new SqlParameter("@GatewayID", SqlDbType.Int);
            param[24].Value = 1;
            try
            {
                using (SqlConnection con = DataConnection.GetConnection())
                {
                     SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "GET_SET_PayU_WebhooksDetails", param);                  
                }
            }
            catch (Exception ex)
            {
               
            }
        }


        public Core.ResponseStatus UpdatePayU_WebhooksDetails(int id, string WebhookPageType, string paymentId, string status, string amount, string paymentMode, string udf5, string udf3, string split_info, string udf4, string udf1, string udf2, string customerName, string productInfo, string customerPhone, string additionalCharges, string customerEmail, string merchantTransactionId, string error_Message, string notificationId, string bankRefNum, string hash, string field4, DateTime Created)
        {
            SqlParameter[] param = new SqlParameter[242];

            param[0] = new SqlParameter("@id", SqlDbType.Int);
            param[0].Value = id;
            param[1] = new SqlParameter("@WebhookPageType", SqlDbType.VarChar, 50);
            param[1].Value = WebhookPageType;
            param[2] = new SqlParameter("@paymentId", SqlDbType.VarChar, 50);
            param[2].Value = paymentId;
            param[3] = new SqlParameter("@status", SqlDbType.VarChar, 100);
            param[3].Value = status;
            param[4] = new SqlParameter("@amount", SqlDbType.Decimal);
            param[4].Value = amount;
            param[5] = new SqlParameter("@paymentMode", SqlDbType.VarChar, 50);
            param[5].Value = paymentMode;
            param[6] = new SqlParameter("@udf5", SqlDbType.VarChar, 100);
            param[6].Value = udf5;
            param[7] = new SqlParameter("@udf3", SqlDbType.VarChar, 100);
            param[7].Value = udf3;
            param[8] = new SqlParameter("@split_info", SqlDbType.VarChar, 100);
            param[8].Value = split_info;
            param[9] = new SqlParameter("@udf4", SqlDbType.VarChar, 100);
            param[9].Value = udf4;
            param[10] = new SqlParameter("@udf1", SqlDbType.VarChar, 100);
            param[10].Value = udf1;
            param[11] = new SqlParameter("@udf2", SqlDbType.VarChar, 100);
            param[11].Value = udf2;
            param[12] = new SqlParameter("@customerName", SqlDbType.VarChar, 100);
            param[12].Value = customerName;
            param[13] = new SqlParameter("@productInfo", SqlDbType.VarChar, 1000);
            param[13].Value = productInfo;
            param[14] = new SqlParameter("@customerPhone", SqlDbType.VarChar, 100);
            param[14].Value = customerPhone;
            param[15] = new SqlParameter("@additionalCharges", SqlDbType.VarChar, 100);
            param[15].Value = additionalCharges;
            param[16] = new SqlParameter("@customerEmail", SqlDbType.VarChar, 100);
            param[16].Value = customerEmail;
            param[17] = new SqlParameter("@merchantTransactionId", SqlDbType.VarChar, 100);
            param[17].Value = merchantTransactionId;
            param[18] = new SqlParameter("@error_Message", SqlDbType.VarChar, 100);
            param[18].Value = error_Message;
            param[19] = new SqlParameter("@notificationId", SqlDbType.VarChar, 100);
            param[19].Value = notificationId;
            param[20] = new SqlParameter("@bankRefNum", SqlDbType.VarChar, 100);
            param[20].Value = bankRefNum;
            param[21] = new SqlParameter("@hash", SqlDbType.VarChar, 500);
            param[21].Value = hash;
            param[22] = new SqlParameter("@field4", SqlDbType.VarChar, 100);
            param[22].Value = field4;
            param[23] = new SqlParameter("@Created", SqlDbType.DateTime);
            param[23].Value = Created;
            param[24] = new SqlParameter("@Counter", SqlDbType.VarChar, 500);
            param[24].Value = "INSERT";
            param[25] = new SqlParameter("@Status", SqlDbType.VarChar, 500);
            param[25].Direction = ParameterDirection.Output;
            try
            {
                using (SqlConnection con = DataConnection.GetConnection())
                {
                    int i = SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "GET_SET_PayU_WebhooksDetails", param);
                    if (i > 0 && param[25].Value.ToString() == "true")
                    {
                        return new Core.ResponseStatus() { message = "Data is update properly!" };
                    }
                    else
                    {
                        return new Core.ResponseStatus() { status = Core.TransactionStatus.Error, message = "Data is not update properly!" };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Core.ResponseStatus() { status = Core.TransactionStatus.Error, message = ex.ToString() };
            }
        }

        public WebhookSuccessDetails GetPayU_WebhooksDetails(string WebhookPageType, string merchantTransactionId)
        {
            WebhookSuccessDetails objWebHook = null;
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@WebhookPageType", SqlDbType.VarChar, 50);
            param[0].Value = WebhookPageType;

            param[1] = new SqlParameter("@merchantTransactionId", SqlDbType.VarChar, 100);
            param[1].Value = merchantTransactionId;

            param[2] = new SqlParameter("@Counter", SqlDbType.VarChar, 500);
            param[2].Value = "SELECT";

            try
            {
                using (SqlConnection con = DataConnection.GetConnection())
                {
                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GET_SET_PayU_WebhooksDetails", param);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        objWebHook = new WebhookSuccessDetails();
                        objWebHook.WebhookPageType = ds.Tables[0].Rows[0]["WebhookPageType"].ToString();
                        objWebHook.paymentId = ds.Tables[0].Rows[0]["paymentId"].ToString();
                        objWebHook.status = ds.Tables[0].Rows[0]["status"].ToString();
                        objWebHook.amount = ds.Tables[0].Rows[0]["amount"].ToString();
                        objWebHook.paymentMode = ds.Tables[0].Rows[0]["paymentMode"].ToString();
                        objWebHook.udf5 = ds.Tables[0].Rows[0]["udf5"].ToString();
                        objWebHook.udf3 = ds.Tables[0].Rows[0]["udf3"].ToString();
                        objWebHook.split_info = ds.Tables[0].Rows[0]["split_info"].ToString();
                        objWebHook.udf4 = ds.Tables[0].Rows[0]["udf4"].ToString();
                        objWebHook.udf1 = ds.Tables[0].Rows[0]["udf1"].ToString();
                        objWebHook.udf2 = ds.Tables[0].Rows[0]["udf2"].ToString();
                        objWebHook.customerName = ds.Tables[0].Rows[0]["customerName"].ToString();
                        objWebHook.productInfo = ds.Tables[0].Rows[0]["productInfo"].ToString();
                        objWebHook.customerPhone = ds.Tables[0].Rows[0]["customerPhone"].ToString();
                        objWebHook.additionalCharges = ds.Tables[0].Rows[0]["additionalCharges"].ToString();
                        objWebHook.customerEmail = ds.Tables[0].Rows[0]["customerEmail"].ToString();
                        objWebHook.merchantTransactionId = ds.Tables[0].Rows[0]["merchantTransactionId"].ToString();
                        objWebHook.error_Message = ds.Tables[0].Rows[0]["error_Message"].ToString();
                        objWebHook.notificationId = ds.Tables[0].Rows[0]["notificationId"].ToString();
                        objWebHook.bankRefNum = ds.Tables[0].Rows[0]["bankRefNum"].ToString();
                        objWebHook.hash = ds.Tables[0].Rows[0]["hash"].ToString();
                        objWebHook.field4 = ds.Tables[0].Rows[0]["field4"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objWebHook;
        }
    }
}
