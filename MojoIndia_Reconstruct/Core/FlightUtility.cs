using Core.Flight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
//using Core.SecretDeal;

namespace Core
{
    public class FlightUtility
    {
        public static List<Airport> AirportList { get; set; }
        public static List<Airline> AirlineList { get; set; }
        public static List<AircraftDetail> AircraftDetails { get; set; }
        //public static List<AirlineBaggage> airlineBaggage { get; set; }
        //public static List<SecretDealDetails> secretDealDetails { get; set; }
        public static List<AirportWithTimeZone> AirportTimeZoneList { get; set; }
        public static List<Affiliate> AffiliateList { get; set; }
        public static bool isMasterDataLoaded = false;
        public static void LoadMasterData()
        {
            FlightUtility obj = new FlightUtility();
            AirportList = obj.getAllAirport();
            AirlineList = obj.getAllAirline();
            AircraftDetails = obj.getAllAircraftDetail();
            //airlineBaggage = obj.GetAllAirlineBaggage();
            //secretDealDetails = obj.GetAllSecretDealDetails();
            AirportTimeZoneList = obj.getAllAirportWithTimeZone();
            AffiliateList = obj.GetAllAffiliate();
            isMasterDataLoaded = true;
        }
        public static Airport GetAirport(string AirportCode)
        {
            if (!FlightUtility.isMasterDataLoaded)
            {
                FlightUtility.LoadMasterData();
            }

            List<Airport> ResAirportCode = FlightUtility.AirportList.Where(x => (x.airportCode.Equals(AirportCode, StringComparison.OrdinalIgnoreCase))).ToList();
            if (ResAirportCode.Count > 0)
            {
                return ResAirportCode[0];
            }
            else
            {
                return new Airport()
                {
                    airportCode = AirportCode,
                    airportName = AirportCode,
                    cityCode = AirportCode,
                    cityName = AirportCode,
                    countryCode = AirportCode.ToUpper(),
                    countryName = AirportCode
                };
            }
        }
        public static Airline GetAirline(string AirlineCode)
        {
            if (!FlightUtility.isMasterDataLoaded)
            {
                FlightUtility.LoadMasterData();
            }

            List<Airline> ResAirlineCode = FlightUtility.AirlineList.Where(x => (x.code.Equals(AirlineCode, StringComparison.OrdinalIgnoreCase))).ToList();
            if (ResAirlineCode.Count > 0)
            {
                return ResAirlineCode[0];
            }
            else
            {
                return new Airline()
                {
                    code = AirlineCode,
                    name = AirlineCode
                };
            }
        }
        public static AircraftDetail GetAircraftDetail(string AircraftCode)
        {
            if (!FlightUtility.isMasterDataLoaded)
            {
                FlightUtility.LoadMasterData();
            }

            List<AircraftDetail> Aircraft = FlightUtility.AircraftDetails.Where(x => (x.aircraftCode.Equals(AircraftCode, StringComparison.OrdinalIgnoreCase))).ToList();
            if (Aircraft.Count > 0)
            {
                return Aircraft[0];
            }
            else
            {
                return new AircraftDetail()
                {
                    aircraftCode = AircraftCode,
                    aircraftName = "No Information",
                    bodyType = "",
                    formOfPropulsion = "",
                    NoOfSeat = ""
                };
            }
        }
        public static Affiliate GetAffiliate(string SourceMedia)
        {
            if (!FlightUtility.isMasterDataLoaded)
            {
                FlightUtility.LoadMasterData();
            }

            Affiliate aff = FlightUtility.AffiliateList.Where(x => (x.AffiliateId.Equals(SourceMedia, StringComparison.OrdinalIgnoreCase))).ToList().FirstOrDefault();
            if (aff == null)
            {
                aff = new Affiliate()
                {
                    AffiliateName = "",
                    CardConFee = "",
                    AffiliateId = "",
                    EmiConFee = "",
                    NetBankingConFee = "",
                    PayLaterConFee = "",
                    UPIConFee = "",
                    WalletConFee = ""

                };
            }
            return aff;
        }
        public List<Affiliate> GetAllAffiliate()
        {
            List<Affiliate> CF = new List<Affiliate>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM AffiliateDetailes where IsActive=1", con))
                {
                    try
                    {
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Affiliate objCF = new Affiliate();
                                objCF.SiteID = (SiteId)Convert.ToInt32(dr["SiteID"]);
                                objCF.AffiliateId = dr["AffiliateId"].ToString();
                                objCF.AffiliateName = dr["AffiliateName"].ToString();
                                objCF.EmiConFee = dr["EmiConFee"].ToString();
                                objCF.PayLaterConFee = dr["PayLaterConFee"].ToString();
                                objCF.WalletConFee = dr["WalletConFee"].ToString();
                                objCF.NetBankingConFee = dr["NetBankingConFee"].ToString();
                                objCF.CardConFee = dr["CardConFee"].ToString();
                                objCF.UPIConFee = dr["UPIConFee"].ToString();
                                CF.Add(objCF);
                            }
                        }

                        try
                        {
                            dr.Close();
                            con.Close();
                        }
                        catch { }

                    }
                    catch //(Exception ex)
                    {

                    }
                }
            }
            return CF;
        }
        public static AirportWithTimeZone GetAirportTimeZone(string AirportCode)
        {
            if (!FlightUtility.isMasterDataLoaded)
            {
                FlightUtility.LoadMasterData();
            }

            List<AirportWithTimeZone> ResAirportCode = FlightUtility.AirportTimeZoneList.Where(x => (x.airportCode.Equals(AirportCode, StringComparison.OrdinalIgnoreCase))).ToList();
            if (ResAirportCode.Count > 0)
            {
                return ResAirportCode[0];
            }
            else
            {
                return new AirportWithTimeZone()
                {
                    airportCode = AirportCode,
                    timeZone = "",
                    timeZone2 = ""
                };
            }
        }
        //public static AirlineBaggage GetAirlineBaggage(string AirlineCode)
        //{
        //    if (!FlightUtility.isMasterDataLoaded)
        //    {
        //        FlightUtility.LoadMasterData();
        //    }

        //    List<AirlineBaggage> ab = FlightUtility.airlineBaggage.Where(x => (x.AirlineCode.Equals(AirlineCode, StringComparison.OrdinalIgnoreCase))).ToList();
        //    if (ab.Count > 0)
        //    {
        //        return ab[0];
        //    }
        //    else
        //    {
        //        return new AirlineBaggage()
        //        {
        //            AirlineCode = AirlineCode,
        //            AdditionalPolicyLink = "",
        //            AirlineName = "",
        //            CarryOnLink = "",
        //            CheckInLink = "",
        //            FirstBagLink = "",
        //            TripType = "",
        //            SecondBagLink = "",
        //            WebLink = ""
        //        };
        //    }
        //}
        #region GetData From DB
        public List<Airport> getAllAirport()
        {
            List<Airport> arpList = new List<Airport>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("select * from airport_details1  order by showSeq ,AirportCode, CityName", con))
                {
                    try
                    {
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Airport arp = new Airport();
                                arp.airportCode = dr["AirportCode"].ToString().ToUpper();
                                arp.airportName = dr["AirportName"].ToString();
                                arp.cityCode = dr["CityCode"].ToString().ToUpper();
                                arp.cityName = dr["CityName"].ToString();
                                arp.countryCode = dr["Country"].ToString().ToUpper();
                                arp.countryName = dr["CountryName"].ToString();
                                //arp.stateCode = dr["StateCode"].ToString().ToUpper();
                                //arp.stateName = dr["StateName"].ToString();
                                //arp.continent = dr["Continent"].ToString();
                                arp.showSeq = (string.IsNullOrEmpty(dr["showSeq"].ToString()) ? 0 : Convert.ToInt32(dr["showSeq"]));
                                if (arp.countryName.ToUpper() == "USA")
                                {
                                    arp.countryName = arp.countryName.ToUpper();
                                }
                                arpList.Add(arp);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }


            //DataSet ds = SqlHelper.ExecuteDataset(DataConnection.GetConnection(), CommandType.Text, "select * from airport_details  order by CityCode, cityPriority, AirportName ");
            //if (ds != null)
            //{
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        Airport arp = new Airport();
            //        arp.airportCode = dr["AirportCode"].ToString();
            //        arp.airportName = dr["AirportName"].ToString();
            //        arp.cityCode = dr["CityCode"].ToString();
            //        arp.cityName = dr["CityName"].ToString();
            //        arp.countryCode = dr["Country"].ToString();
            //        arp.countryName = dr["CountryName"].ToString();
            //        arp.stateCode = dr["StateCode"].ToString();
            //        arp.stateName = dr["StateName"].ToString();
            //        arp.continent = dr["Continent"].ToString();

            //        if (arp.countryName.ToUpper() == "USA")
            //        {
            //            arp.countryName = arp.countryName.ToUpper();
            //        }
            //        arpList.Add(arp);
            //    }
            //}
            return arpList;
        }

        public List<Airline> getAllAirline()
        {
            List<Airline> arlList = new List<Airline>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Airline_details order by [Name]", con))
                {
                    try
                    {
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Airline arp = new Airline();
                                arp.code = dr["Code"].ToString().ToUpper();
                                // arp.name = dr["Name"].ToString();
                                arp.name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["Name"].ToString().ToLower());
                                arlList.Add(arp);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return arlList;
        }

        public List<AircraftDetail> getAllAircraftDetail()
        {
            List<AircraftDetail> arcftList = new List<AircraftDetail>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM AircraftDetails", con))
                {
                    try
                    {
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                AircraftDetail arp = new AircraftDetail();
                                arp.aircraftCode = dr["AircraftCode"].ToString();
                                arp.bodyType = dr["BodyType"].ToString();
                                arp.aircraftName = dr["AircraftName"].ToString();
                                arp.formOfPropulsion = dr["FormOfPropulsion"].ToString();
                                arp.NoOfSeat = dr["NoOfSeat"].ToString();
                                arcftList.Add(arp);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return arcftList;
        }
        public List<AirportWithTimeZone> getAllAirportWithTimeZone()
        {
            List<AirportWithTimeZone> arpList = new List<AirportWithTimeZone>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("select * from Airport_With_TimeZone", con))
                {
                    try
                    {
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                AirportWithTimeZone arp = new AirportWithTimeZone();
                                arp.airportCode = dr["AirportCode"].ToString().ToUpper();
                                arp.timeZone = dr["TimeZone"].ToString();
                                arp.timeZone2 = dr["TimeZone2"].ToString();
                                arpList.Add(arp);
                            }
                        }
                    }
                    catch //(Exception ex)
                    {

                    }
                }
            }
            return arpList;
        }
        //public List<AirlineBaggage> GetAllAirlineBaggage()
        //{

        //    List<AirlineBaggage> bag = new List<AirlineBaggage>();
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SELECT * FROM AirlineBaggage", con))
        //        {
        //            try
        //            {
        //                con.Open();
        //                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        //                if (dr.HasRows)
        //                {
        //                    while (dr.Read())
        //                    {
        //                        AirlineBaggage obj = new AirlineBaggage();
        //                        obj.ID_AirlineBaggage = Convert.ToInt32(dr["ID_AirlineBaggage"]);
        //                        obj.ID_Site = Convert.ToInt32(dr["ID_Site"]);
        //                        obj.ID_Product = Convert.ToInt32(dr["ID_Product"]);
        //                        obj.TripType = dr["TripType"].ToString();
        //                        obj.AirlineCode = dr["AirlineCode"].ToString();
        //                        obj.AirlineName = dr["AirlineName"].ToString();
        //                        obj.WebLink = dr["WebLink"].ToString();
        //                        obj.FirstBagLink = dr["FirstBagLink"].ToString();
        //                        obj.SecondBagLink = dr["SecondBagLink"].ToString();
        //                        obj.CarryOnLink = dr["CarryOnLink"].ToString();
        //                        obj.AdditionalPolicyLink = dr["AdditionalPolicyLink"].ToString();
        //                        obj.CheckInLink = dr["CheckInLink"].ToString();
        //                        obj.isActive = Convert.ToBoolean(dr["isActive"]);

        //                        bag.Add(obj);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }
        //    }
        //    return bag;
        //}

        //public List<SecretDealDetails> GetAllSecretDealDetails()
        //{
        //    List<SecretDealDetails> sdd = new List<SecretDealDetails>();
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SELECT * FROM WebsiteSecretDealDetails where IsActive='true'", con))
        //        {
        //            try
        //            {
        //                con.Open();
        //                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        //                if (dr.HasRows)
        //                {
        //                    while (dr.Read())
        //                    {
        //                        SecretDealDetails obj = new SecretDealDetails();
        //                        obj.siteID = (SiteId)(string.IsNullOrEmpty(dr["siteID"].ToString()) ? 0 : Convert.ToInt32(dr["siteID"]));
        //                        obj.AffiliateID = (string.IsNullOrEmpty(dr["AffiliateID"].ToString()) ? 0 : Convert.ToInt32(dr["AffiliateID"]));
        //                        obj.SecretFareType = (SearchFareType)(string.IsNullOrEmpty(dr["SecretFareType"].ToString()) ? 0 : Convert.ToInt32(dr["SecretFareType"]));
        //                        obj.StartTime = (string.IsNullOrEmpty(dr["StartTime"].ToString()) ? 0 : Convert.ToInt32(dr["StartTime"]));
        //                        obj.EndTime = (string.IsNullOrEmpty(dr["EndTime"].ToString()) ? 0 : Convert.ToInt32(dr["EndTime"]));
        //                        obj.DiscountAmount = (string.IsNullOrEmpty(dr["DiscountAmount"].ToString()) ? 0 : Convert.ToDecimal(dr["DiscountAmount"]));
        //                        //obj.DiscountType = (AmountType)(string.IsNullOrEmpty(dr["DiscountType"].ToString()) ? 0 : Convert.ToInt32(dr["DiscountType"]));
        //                        obj.ContactNo = dr["ContactNo"].ToString();
        //                        if (string.IsNullOrEmpty(obj.ContactNo))
        //                            obj.ContactNo = ConfigurationManager.AppSettings["Phone"];
        //                        sdd.Add(obj);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }
        //    }
        //    return sdd;
        //}

        #endregion
    }
}
namespace logLibrary
{
    public class LogWriter
    {
        public LogWriter()
        {
        }
        public void SetError(string heading, string massege, string FileName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Environment.NewLine + "--------------------------" + heading + "(" + DateTime.Now.ToString("dd MMM yyyy HH:mm") + ")--------------------------" + Environment.NewLine);
                sb.Append(massege + Environment.NewLine);

                LogWrite(sb.ToString(), FileName);
            }
            catch { }
        }
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
            //try
            //{
            using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + FileName + ".txt"))
            {
                Log(logMessage, w);
            }
            //}
            //catch (Exception ex)
            //{
            //}
        }
        public void LogWrite(string logMessage, string FileName, string FolderName)
        {
            try
            {
                using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "\\log\\" + FolderName + "\\" + FileName + ".txt"))
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
            //try
            //{
            //txtWriter.Write("\r\nLog Entry : ");
            //txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            //    DateTime.Now.ToLongDateString());
            //txtWriter.WriteLine("  :");
            txtWriter.WriteLine("{0}", logMessage);
            //txtWriter.WriteLine("-------------------------------");
            //}
            //catch (Exception ex)
            //{
            //}
        }
    }
}
