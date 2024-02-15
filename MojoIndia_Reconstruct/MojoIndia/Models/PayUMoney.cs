using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MojoIndia.Models
{
    public class PayUMoney
    {
        public string GetJson( decimal Amount, string MerchentID,string Commission)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;

            DataTable dtEmployee = new DataTable();
            dtEmployee.Columns.Add("name", typeof(string));
            dtEmployee.Columns.Add("description", typeof(string));
            dtEmployee.Columns.Add("value", typeof(string));
            dtEmployee.Columns.Add("merchantId", typeof(string));
            dtEmployee.Columns.Add("commission", typeof(string));
            dtEmployee.Rows.Add("FlightsMojo", "FlightsMojo", Amount.ToString("f2"), MerchentID, Commission);
            //dtEmployee.Rows.Add("EMAAR20147f606866-5754-4a80-afa5-d44c0c9c3abe",
            //    "EMAAR Test DescriptionInstallment*1Installmen", Amount.ToString("g29"), "396446", Commission);
            foreach (DataRow dr in dtEmployee.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dtEmployee.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        public string Generatehash512(string text)
        {
            byte[] message = Encoding.UTF8.GetBytes(text);
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
        public string PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");
            foreach (System.Collections.DictionaryEntry key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                               "\" value=\"" + key.Value + "\">");
            }
            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }
        public class SplitAggregator
        {
            public string name;
            public string description;
            public string value;
            public string merchantId;
        }
    }
}