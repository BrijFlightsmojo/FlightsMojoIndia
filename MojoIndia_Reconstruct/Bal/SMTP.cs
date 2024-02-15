using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Bal
{
    public class SMTP
    {
        public bool SendEMail(SendEmailRequest sendEmailRequest)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            StringBuilder sbLogger = new StringBuilder();
            sbLogger.Append(Environment.NewLine + "--------------------------------------------" + Environment.NewLine + "Log Start :" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + Environment.NewLine + "--------------------------------------------" + Environment.NewLine);
            sbLogger.Append(Newtonsoft.Json.JsonConvert.SerializeObject(sendEmailRequest));
            bool response = false;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.office365.com");//("secure.emailsrvr.com");
                mail.From = new MailAddress(sendEmailRequest.FromEmail);
                mail.To.Add(sendEmailRequest.ToEmail);
                if (!string.IsNullOrEmpty(sendEmailRequest.CcEmail))
                    mail.CC.Add(sendEmailRequest.CcEmail.ToLower());
                if (!string.IsNullOrEmpty(sendEmailRequest.BccEmail))
                    mail.Bcc.Add(sendEmailRequest.BccEmail.ToLower());

                mail.Subject = sendEmailRequest.MailSubject;
                mail.IsBodyHtml = true;
                mail.Body = sendEmailRequest.MailBody;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(sendEmailRequest.FromEmail, getPassword(sendEmailRequest.FromEmail));
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                response = true;
                sbLogger.Append(Environment.NewLine + "--------------------------------------------" + Environment.NewLine + "Log Entry :" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + Environment.NewLine + "--------------------------------------------" + Environment.NewLine);
                sbLogger.Append("SendMail:-" + response.ToString());
                sbLogger.Append(Environment.NewLine + "--------------------------------------------" + Environment.NewLine + "Log End :" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + Environment.NewLine + "--------------------------------------------" + Environment.NewLine);
                //new LogWriter(sbLogger.ToString(), "Email-" + DateTime.Today.ToString("ddMMyyyy"));
            }
            catch (Exception ex)
            {
                sbLogger.Append(Environment.NewLine + "--------------------------------------------" + Environment.NewLine + "Log Entry :" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + Environment.NewLine + "--------------------------------------------" + Environment.NewLine);
                sbLogger.Append("SendMail:-" + response.ToString());
                sbLogger.Append(Environment.NewLine + "--------------------------------------------" + Environment.NewLine + "Log Entry :" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + Environment.NewLine + "--------------------------------------------" + Environment.NewLine);
                sbLogger.Append(ex.ToString());
                sbLogger.Append(Environment.NewLine + "--------------------------------------------" + Environment.NewLine + "Log End :" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + Environment.NewLine + "--------------------------------------------" + Environment.NewLine);
                new LogWriter(sbLogger.ToString(), "EmailExeption-" + DateTime.Today.ToString("ddMMyyyy"));
            }
            return response;
        }
        private string getPassword(string FromEmail)
        {
            string Pwd = string.Empty;
            switch (FromEmail.ToLower())
            {
                case "res@flightsmojo.in": Pwd = "Gux72919"; break;

                default: break;
            }
            return Pwd;
        }
    }
}
