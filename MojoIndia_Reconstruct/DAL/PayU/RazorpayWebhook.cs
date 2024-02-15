using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.PayU
{
    public class RazorpayWebhook
    {
        public void SaveRazorPay_WebhooksDetails(string WebhookPageType, Core.RP.Webhook.WebhookSuccess objWHD)
        {
            SqlParameter[] param = new SqlParameter[33];

            param[1] = new SqlParameter("@GatewayID", SqlDbType.Int);
            param[1].Value = 2;
            param[2] = new SqlParameter("@WebhookPageType", SqlDbType.VarChar, 50);
            param[2].Value = WebhookPageType;
            param[3] = new SqlParameter("@order_id", SqlDbType.VarChar, 50);
            param[3].Value = objWHD.payload.payment.entity.order_id;
            param[4] = new SqlParameter("@account_id", SqlDbType.VarChar, 50);
            param[4].Value = objWHD.account_id;
            param[5] = new SqlParameter("@entity_id", SqlDbType.VarChar, 50);
            param[5].Value = objWHD.payload.payment.entity.id;
            param[6] = new SqlParameter("@entity", SqlDbType.VarChar, 50);
            param[6].Value = objWHD.payload.payment.entity.entity;
            param[7] = new SqlParameter("@amount", SqlDbType.Decimal);
            param[7].Value = objWHD.payload.payment.entity.amount;
            param[8] = new SqlParameter("@currency", SqlDbType.VarChar, 50);
            param[8].Value = objWHD.payload.payment.entity.currency;
            param[9] = new SqlParameter("@status", SqlDbType.VarChar, 50);
            param[9].Value = objWHD.payload.payment.entity.status;
            param[10] = new SqlParameter("@invoice_id", SqlDbType.VarChar, 50);
            param[10].Value = objWHD.payload.payment.entity.invoice_id;
            param[11] = new SqlParameter("@international", SqlDbType.VarChar, 50);
            param[11].Value = objWHD.payload.payment.entity.international;
            param[12] = new SqlParameter("@method", SqlDbType.VarChar, 50);
            param[12].Value = objWHD.payload.payment.entity.method;
            param[13] = new SqlParameter("@amount_refunded", SqlDbType.Decimal);
            param[13].Value = objWHD.payload.payment.entity.amount_refunded;
            param[14] = new SqlParameter("@refund_status", SqlDbType.VarChar, 50);
            param[14].Value = objWHD.payload.payment.entity.refund_status;
            param[15] = new SqlParameter("@captured", SqlDbType.VarChar, 50);
            param[15].Value = objWHD.payload.payment.entity.captured;
            param[16] = new SqlParameter("@description", SqlDbType.VarChar, 50);
            param[16].Value = objWHD.payload.payment.entity.description;
            param[17] = new SqlParameter("@card_id", SqlDbType.VarChar, 50);
            param[17].Value = objWHD.payload.payment.entity.card_id;
            param[18] = new SqlParameter("@bank", SqlDbType.VarChar, 50);
            param[18].Value = objWHD.payload.payment.entity.bank;
            param[19] = new SqlParameter("@wallet", SqlDbType.VarChar, 50);
            param[19].Value = objWHD.payload.payment.entity.wallet;
            param[20] = new SqlParameter("@vpa", SqlDbType.VarChar, 50);
            param[20].Value = objWHD.payload.payment.entity.vpa;
            param[21] = new SqlParameter("@email", SqlDbType.VarChar, 50);
            param[21].Value = objWHD.payload.payment.entity.email;
            param[22] = new SqlParameter("@contact", SqlDbType.VarChar, 50);
            param[22].Value = objWHD.payload.payment.entity.contact;
            param[23] = new SqlParameter("@fee", SqlDbType.Decimal);
            param[23].Value = objWHD.payload.payment.entity.fee;
            param[24] = new SqlParameter("@tax", SqlDbType.Decimal);
            param[24].Value = objWHD.payload.payment.entity.tax;
            param[25] = new SqlParameter("@error_code", SqlDbType.VarChar, 50);
            param[25].Value = objWHD.payload.payment.entity.error_code;
            param[26] = new SqlParameter("@error_description", SqlDbType.VarChar, 50);
            param[26].Value = objWHD.payload.payment.entity.error_description;
            param[27] = new SqlParameter("@error_source", SqlDbType.VarChar, 50);
            param[27].Value = objWHD.payload.payment.entity.error_source;
            param[28] = new SqlParameter("@error_step", SqlDbType.VarChar, 50);
            param[28].Value = objWHD.payload.payment.entity.error_step;
            param[29] = new SqlParameter("@error_reason", SqlDbType.VarChar, 50);
            param[29].Value = objWHD.payload.payment.entity.error_reason;
            param[30] = new SqlParameter("@bank_transaction_id", SqlDbType.VarChar, 50);
            param[30].Value = objWHD.payload.payment.entity.acquirer_data.bank_transaction_id;
            param[31] = new SqlParameter("@Counter", SqlDbType.VarChar, 500);
            param[31].Value = "INSERT";
            param[32] = new SqlParameter("@OutputStatus", SqlDbType.VarChar, 500);
            param[32].Direction = ParameterDirection.Output;
            try
            {
                using (SqlConnection con = DataConnection.GetConnection())
                {
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "GET_SET_RazorPay_WebhooksDetails", param);
                    UpdateTransation_WebhooksDetails(objWHD.payload.payment.entity.description);
                    UpdateRazorPay_WebhooksDetails(objWHD.payload.payment.entity.id);
                }
            }
            catch (Exception ex)
            {
                 new LogWriter(ex.ToString(), "WebHookException_" + objWHD.payload.payment.entity.description, "WebHookException");
            }
        }

        private void UpdateTransation_WebhooksDetails(string description)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@BookingID", SqlDbType.VarChar, 50);
            param[0].Value = description;

            try
            {
                using (SqlConnection con = DataConnection.GetConnection())
                {
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "GET_SET_RazorPay_WebhooksDetails_V2", param);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public Core.RP.Webhook.RazorPay_WebhooksDetails GetRazorPay_WebhooksDetails(string PaymentID)
        {
            Core.RP.Webhook.RazorPay_WebhooksDetails objWebHook = null;
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@order_id", SqlDbType.VarChar, 50);
            param[0].Value = PaymentID;

            param[1] = new SqlParameter("@Counter", SqlDbType.VarChar, 500);
            param[1].Value = "SELECT";

            try
            {
                using (SqlConnection con = DataConnection.GetConnection())
                {
                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GET_SET_RazorPay_WebhooksDetails", param);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        objWebHook = new Core.RP.Webhook.RazorPay_WebhooksDetails()
                        {
                            order_id = ds.Tables[0].Rows[0]["order_id"].ToString(),
                            account_id = ds.Tables[0].Rows[0]["account_id"].ToString(),
                            entity = ds.Tables[0].Rows[0]["entity"].ToString(),
                            status = ds.Tables[0].Rows[0]["status"].ToString(),
                            amount = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["amount"].ToString()) ? 0m : Convert.ToDecimal(ds.Tables[0].Rows[0]["amount"].ToString()),
                            amount_refunded = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["amount_refunded"].ToString()) ? 0m : Convert.ToDecimal(ds.Tables[0].Rows[0]["amount_refunded"].ToString()),
                            tax = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["tax"].ToString()) ? 0m : Convert.ToDecimal(ds.Tables[0].Rows[0]["tax"].ToString()),
                            fee = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["fee"].ToString()) ? 0m : Convert.ToDecimal(ds.Tables[0].Rows[0]["fee"].ToString()),
                            bank = ds.Tables[0].Rows[0]["bank"].ToString(),
                            bank_transaction_id = ds.Tables[0].Rows[0]["bank_transaction_id"].ToString(),
                            captured = ds.Tables[0].Rows[0]["captured"].ToString(),
                            card_id = ds.Tables[0].Rows[0]["card_id"].ToString(),
                            contact = ds.Tables[0].Rows[0]["contact"].ToString(),
                            currency = ds.Tables[0].Rows[0]["currency"].ToString(),
                            description = ds.Tables[0].Rows[0]["description"].ToString(),
                            email = ds.Tables[0].Rows[0]["email"].ToString(),
                            entity_id = ds.Tables[0].Rows[0]["entity_id"].ToString(),
                            error_code = ds.Tables[0].Rows[0]["error_code"].ToString(),
                            error_description = ds.Tables[0].Rows[0]["error_description"].ToString(),
                            error_reason = ds.Tables[0].Rows[0]["error_reason"].ToString(),
                            error_source = ds.Tables[0].Rows[0]["error_source"].ToString(),
                            error_step = ds.Tables[0].Rows[0]["error_step"].ToString(),
                            GatewayID = 2,
                            international = ds.Tables[0].Rows[0]["international"].ToString(),
                            invoice_id = ds.Tables[0].Rows[0]["invoice_id"].ToString(),
                            method = ds.Tables[0].Rows[0]["method"].ToString(),
                            refund_status = ds.Tables[0].Rows[0]["refund_status"].ToString(),
                            vpa = ds.Tables[0].Rows[0]["vpa"].ToString(),
                            wallet = ds.Tables[0].Rows[0]["wallet"].ToString(),
                            WebhookPageType = ds.Tables[0].Rows[0]["WebhookPageType"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objWebHook;
        }

        public void UpdateRazorPay_WebhooksDetails(string order_id)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@order_id", SqlDbType.VarChar, 50);
            param[0].Value = order_id;

            try
            {
                using (SqlConnection con = DataConnection.GetConnection())
                {
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "GET_SET_RazorPay_WebhooksDetails_V1", param);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public class LogWriter
        {
            private string m_exePath = string.Empty;
            public LogWriter(string logMessage, string FileName)
            {
                LogWrite(logMessage, FileName);
            }
            public LogWriter(string logMessage, string FileName, string FolderName)
            {
                LogWrite(logMessage, FileName, FolderName);
            }
            public void LogWrite(string logMessage, string FileName)
            {
                using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + FileName + ".txt"))
                {
                    Log(logMessage, w);
                }
            }
            public void LogWrite(string logMessage, string FileName, string FolderName)
            {
                try
                {
                    using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + FolderName + "\\" + FileName + ".txt"))
                    {
                        Log(logMessage, w);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            public void Log(string logMessage, TextWriter txtWriter)
            {
                txtWriter.WriteLine("{0}", logMessage);
            }
        }
    }
}
