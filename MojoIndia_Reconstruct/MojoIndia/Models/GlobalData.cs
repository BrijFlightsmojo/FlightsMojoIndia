using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace MojoIndia
{
    public class GlobalData
    {
        public static string CompanyName { get; set; }
        public static string Address { get; set; }
        public static string City { get; set; }
        public static string State { get; set; }
        public static string Country { get; set; }
        public static string Pincode { get; set; }
        public static string Phone { get; set; }
        public static string PhoneUK { get; set; }
        public static string PhoneCA { get; set; }
        public static string Email { get; set; }
        public static string SendEmail { get; set; }
		public static string URL { get; set; }
        public static bool isBundle { get; set; }
        public static bool isAllFareCallCenter { get; set; }
        public static bool isDummyResult { get; set; }
        public static bool isGoToOnlinePayment { get; set; }
        public static string key { get; set; }
        public static string secret { get; set; }
        static GlobalData()
        {
            CompanyName = ConfigurationManager.AppSettings["CompanyName"];
            Address = ConfigurationManager.AppSettings["Address"];
            City = ConfigurationManager.AppSettings["City"];
            State = ConfigurationManager.AppSettings["State"];
            Country = ConfigurationManager.AppSettings["Country"];
            Pincode = ConfigurationManager.AppSettings["Pincode"];
            Phone = ConfigurationManager.AppSettings["Phone"];
            PhoneUK = ConfigurationManager.AppSettings["PhoneUK"];
            PhoneCA = ConfigurationManager.AppSettings["PhoneCA"];
            Email = ConfigurationManager.AppSettings["Email"];
            SendEmail = ConfigurationManager.AppSettings["SendEmail"];
			URL = ConfigurationManager.AppSettings["URL"];
            isBundle = Convert.ToBoolean(ConfigurationManager.AppSettings["isBundle"]);
            isAllFareCallCenter = Convert.ToBoolean(ConfigurationManager.AppSettings["isAllFareCallCenter"]);
            isDummyResult = Convert.ToBoolean(ConfigurationManager.AppSettings["isDummyResult"]);
            isGoToOnlinePayment = ConfigurationManager.AppSettings["isGoToOnlinePayment"]==null?true: Convert.ToBoolean(ConfigurationManager.AppSettings["isGoToOnlinePayment"]);
            key = ConfigurationManager.AppSettings["key"];
            secret = ConfigurationManager.AppSettings["secret"];
        }
    }
}