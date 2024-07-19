using Core.Flight;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bal
{
    public class MakeFlightItinerary
    {
        public static string[] airlineArr = new string[] { "6E", "AI", "G8", "I5", "IX", "QP", "SG", "QQ", "UK" };
        public static bool isSetMarkupRule = false;
        public static Bal.GfMarkup.RuleSet ruleSet = null;
        public Bal.GfMarkup.FeeRule GetFlightResult(AirContext airContext,ref string strAirline)
        {
            string ss = Newtonsoft.Json.JsonConvert.SerializeObject(airContext.flightSearchRequest);
            airContext.flightSearchResponse = new Core.Flight.FlightSearchResponse()
            {
                adults = airContext.flightSearchRequest.adults,
                child = airContext.flightSearchRequest.child,
                infants = airContext.flightSearchRequest.infants,

                aircraftDetail = new List<Core.Flight.AircraftDetail>(),
                airline = new List<Core.Flight.Airline>(),
                airport = new List<Core.Flight.Airport>(),
                Results = new List<List<FlightResult>>(),
                affiliate = Core.FlightUtility.GetAffiliate(airContext.flightSearchRequest.sourceMedia),

                response = new Core.ResponseStatus()
            };
            int gfs = 1;
            Core.Flight.FlightResult result = new Core.Flight.FlightResult()
            {
                ResultCombination = null,
                cabinClass = airContext.flightSearchRequest.cabinType,

                Fare = new Core.Flight.Fare(),
                FareList = new List<Fare>(),
                ffFareType = Core.FareType.PUBLISH,
                FlightSegments = new List<Core.Flight.FlightSegment>(),
                gdsType = Core.GdsType.Amadeus,
                ResultID = "gfs" + (gfs).ToString(),
                ResultIndex = "gfs" + (gfs++).ToString(),
                valCarrier = airContext.googleFlightDeepLink.flightSlice[0].Carrier,
            };
            int ctr = 0;
            foreach (Slice slc in airContext.googleFlightDeepLink.slice)
            {
                FlightSegment fs = new FlightSegment() { Segments = new List<Segment>(), SegName = (slc.id.StartsWith("1") ? "Depart" : "Return"), Duration = 0, stop = slc.sliceId.Count(), LayoverTime = 0 };
                int segCtr = 0;

                foreach (int sID in slc.sliceId)
                {
                    var fSlice = airContext.googleFlightDeepLink.flightSlice.Where(k => k.id == sID).FirstOrDefault();
                    Segment seg = new Segment()
                    {
                        Airline = fSlice.Carrier,
                        OperatingCarrier = fSlice.Carrier,
                        CabinClass = getCabinType(fSlice.Cabin),
                        FlightNumber = fSlice.FlightNumber,

                        equipmentType = "",

                        Origin = fSlice.Origin,
                        Destination = fSlice.Destination,
                        DepTime = DateTime.ParseExact(fSlice.depDate + " " + fSlice.depTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US")),
                        ArrTime = DateTime.ParseExact(fSlice.arrDate + " " + fSlice.arrTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US")),
                        resDesignCode = fSlice.BookingCode,

                        Duration = 0,

                        id = (segCtr).ToString(),
                        FromTerminal = "",
                        ToTerminal = "",

                        layOverTime = 0

                    };
                    if (strAirline.IndexOf(seg.Airline) == -1)
                    {
                        strAirline += seg.Airline;
                    }
                    if (airlineArr.Contains(seg.Airline))
                    {
                        seg.url = "/images/airlinesSvg/" + seg.Airline + ".svg";
                    }
                    else
                    {
                        seg.url = "/images/flight_small/" + seg.Airline + ".gif";
                    }
                    #region calculate eft and layovertime

                    AirportWithTimeZone aprOrg = Core.FlightUtility.GetAirportTimeZone(seg.Origin);
                    AirportWithTimeZone aprDest = Core.FlightUtility.GetAirportTimeZone(seg.Destination);

                    if (aprOrg.timeZone != aprDest.timeZone)
                    {
                        int kk = Convert.ToInt32((seg.ArrTime - seg.DepTime).TotalMinutes);
                        int kk2 = Convert.ToInt32(ToTimeZoneTime(seg.DepTime, aprOrg.timeZone2, aprDest.timeZone2).TotalMinutes);
                        if (kk != 0 && kk2 != 0)
                            seg.Duration = kk - kk2;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(aprOrg.timeZone) && !string.IsNullOrEmpty(aprDest.timeZone))
                        {
                            seg.Duration = Convert.ToInt32((seg.ArrTime - seg.DepTime).TotalMinutes);
                        }
                    }
                    fs.Duration += seg.Duration;

                    if (segCtr > 0)
                    {
                        TimeSpan ts = seg.DepTime - fs.Segments[segCtr - 1].ArrTime;
                        fs.Segments[segCtr - 1].layOverTime = Convert.ToInt32(ts.TotalMinutes);
                        fs.LayoverTime += fs.Segments[segCtr - 1].layOverTime;
                    }
                    segCtr++;
                    #endregion

                    #region set Airport and airling Details
                    if (airContext.flightSearchResponse.airline.Where(o => o.code.Equals(seg.Airline, StringComparison.OrdinalIgnoreCase)).ToList().Count == 0)
                    {
                        airContext.flightSearchResponse.airline.Add(Core.FlightUtility.GetAirline(seg.Airline));
                    }
                    if (airContext.flightSearchResponse.airline.Where(o => o.code.Equals(seg.OperatingCarrier, StringComparison.OrdinalIgnoreCase)).ToList().Count == 0)
                    {
                        airContext.flightSearchResponse.airline.Add(Core.FlightUtility.GetAirline(seg.OperatingCarrier));
                    }
                    if (airContext.flightSearchResponse.airport.Where(o => o.airportCode.Equals(seg.Origin, StringComparison.OrdinalIgnoreCase)).ToList().Count == 0)
                    {
                        airContext.flightSearchResponse.airport.Add(Core.FlightUtility.GetAirport(seg.Origin));
                    }
                    if (airContext.flightSearchResponse.airport.Where(o => o.airportCode.Equals(seg.Destination, StringComparison.OrdinalIgnoreCase)).ToList().Count == 0)
                    {
                        airContext.flightSearchResponse.airport.Add(Core.FlightUtility.GetAirport(seg.Destination));
                    }
                    if (seg.equipmentType != null && airContext.flightSearchResponse.aircraftDetail.Where(o => o.aircraftCode.Equals(seg.equipmentType, StringComparison.OrdinalIgnoreCase)).ToList().Count == 0)
                    {
                        airContext.flightSearchResponse.aircraftDetail.Add(Core.FlightUtility.GetAircraftDetail(seg.equipmentType));
                    }
                    #endregion

                    fs.Segments.Add(seg);
                }
                ctr++;
                result.FlightSegments.Add(fs);

            }
            #region Set fare details

            Core.Flight.Fare fare = new Core.Flight.Fare()
            {
                FB_flight_id = 0,
                FB_static = "",
                BaseFare = airContext.googleFlightDeepLink.DisplayedPrice * 0.75m,
                Tax = airContext.googleFlightDeepLink.DisplayedPrice * 0.25m,
                Currency = "INR",
                Markup = 0,
                PublishedFare = airContext.googleFlightDeepLink.DisplayedPrice,
                NetFare = airContext.googleFlightDeepLink.DisplayedPrice,
                FareType = Core.FareType.PUBLISH,
                cabinType = result.cabinClass,
                gdsType = Core.GdsType.Amadeus,
                SeatAvailable = 9
            };
            fare.mojoFareType = Core.MojoFareType.Publish;
            decimal priceAvg = 0;
            if (airContext.flightSearchRequest.segment[0].orgArp.countryCode.Equals("US", StringComparison.OrdinalIgnoreCase) &&
                airContext.flightSearchRequest.segment[0].destArp.countryCode.Equals("US", StringComparison.OrdinalIgnoreCase))
            {
                priceAvg = airContext.googleFlightDeepLink.DisplayedPrice / (airContext.flightSearchRequest.adults + airContext.flightSearchRequest.child);
            }
            else
            {
                priceAvg = airContext.googleFlightDeepLink.DisplayedPrice / (airContext.flightSearchRequest.adults + airContext.flightSearchRequest.child + airContext.flightSearchRequest.infants);

            }
            fare.fareBreakdown = new List<Core.Flight.FareBreakdown>();
            #region set fare Breakup
            if (airContext.flightSearchRequest.infants > 0)
            {
                Core.Flight.FareBreakdown infFare = new Core.Flight.FareBreakdown();
                infFare.BaseFare = priceAvg * 0.75m;
                infFare.Tax = priceAvg * 0.25m;
                infFare.PassengerType = Core.PassengerType.Infant;
                fare.fareBreakdown.Add(infFare);
            }

            //decimal PaxTotPrice = (Itin.total_payable_price - (1500 * request.infants)) / (request.adults + request.child);
            Core.Flight.FareBreakdown adtFare = new Core.Flight.FareBreakdown();
            adtFare.BaseFare = priceAvg * 0.75m;
            adtFare.Tax = priceAvg * 0.25m;
            adtFare.PassengerType = Core.PassengerType.Adult;
            fare.fareBreakdown.Add(adtFare);
            if (airContext.flightSearchRequest.child > 0)
            {
                Core.Flight.FareBreakdown chdFare = new Core.Flight.FareBreakdown();
                chdFare.BaseFare = priceAvg * 0.75m;
                chdFare.Tax = priceAvg * 0.25m;
                chdFare.PassengerType = Core.PassengerType.Child;
                fare.fareBreakdown.Add(chdFare);
            }

            #endregion
            fare.NetFare = fare.grandTotal = fare.PublishedFare + fare.Markup - fare.CommissionEarned;
            result.Fare = fare;
            result.FareList.Add(fare);

            //decimal priceAvg = 0;
            //if (airContext.flightSearchRequest.segment[0].orgArp.countryCode.Equals("US", StringComparison.OrdinalIgnoreCase) &&
            //    airContext.flightSearchRequest.segment[0].destArp.countryCode.Equals("US", StringComparison.OrdinalIgnoreCase))
            //{
            //    priceAvg = airContext.googleFlightDeepLink.DisplayedPrice / (airContext.flightSearchRequest.adults + airContext.flightSearchRequest.child );
            //}
            //else
            //{
            //    priceAvg = airContext.googleFlightDeepLink.DisplayedPrice / (airContext.flightSearchRequest.adults + airContext.flightSearchRequest.child + airContext.flightSearchRequest.infants );
            //    if (airContext.flightSearchRequest.infants > 0)
            //    {
            //        result.Fare.infantFare = priceAvg * (0.75m);
            //        result.Fare.infantTax = priceAvg * (0.25m);
            //    }
            //}
            //result.Fare.adultFare = priceAvg * (0.75m);
            //result.Fare.adultTax = priceAvg * (0.25m);
            //if (airContext.flightSearchRequest.child > 0)
            //{
            //    result.fare.childFare = priceAvg * (0.75m);
            //    result.fare.childTax = priceAvg * (0.25m);
            //}        
            //result.Fare.Currency = currency;
            //result.Fare.grandTotal = ((result.Fare.adultFare + result.Fare.adultTax + result.Fare.adultMarkup) * airContext.flightSearchRequest.adults) +
            //    ((result.fare.childFare + result.fare.childTax + result.fare.childMarkup) * airContext.flightSearchRequest.child) +
            //    ((result.fare.infantFare + result.fare.infantTax + result.fare.infantMarkup) * airContext.flightSearchRequest.infants);
            //result.FareList.Add( result.Fare);
            #endregion
            List<FlightResult> lstResult = new List<FlightResult>();
            lstResult.Add(result);
            airContext.flightSearchResponse.Results.Add(lstResult);

            List<Core.CabinType> cabin = new List<Core.CabinType>();
            //List<string> Airline = new List<string>();
            string sigAirline = string.Empty;
            int i = 0;
            foreach (var fs in airContext.flightSearchResponse.Results[0][0].FlightSegments)
            {
                foreach (var seg in fs.Segments)
                {
                    if (!cabin.Exists(o => o == seg.CabinClass))
                    {
                        cabin.Add(seg.CabinClass);
                    }
                    //if (!Airline.Exists(o => o.Equals(seg.airline, StringComparison.OrdinalIgnoreCase)))
                    //{
                    //    Airline.Add(seg.airline);
                    //}
                    if (i == 0 && string.IsNullOrEmpty(sigAirline))
                    {
                        if (Core.FlightUtility.GetAirport(seg.Origin).countryCode != Core.FlightUtility.GetAirport(seg.Destination).countryCode)
                        {
                            sigAirline = seg.Airline;
                        }
                    }
                }
                i++;
            }
            if (string.IsNullOrEmpty(sigAirline))
            {
                sigAirline = airContext.flightSearchResponse.Results[0][0].FlightSegments[0].Segments[0].Airline;
            }
            if (isSetMarkupRule == false)
            {
                setMarkupRule();
            }
            var fareRule = ruleSet.feeRule.Where(k => k.isKeepMarkup
                && (k.FromCountry.Count == 0 || (k.FromCountry.Contains(airContext.flightSearchRequest.segment.FirstOrDefault().orgArp.countryCode)))
                && (k.ToCountry.Count == 0 || (k.ToCountry.Contains(airContext.flightSearchRequest.segment.FirstOrDefault().destArp.countryCode)))
                && (k.Airline.Count == 0 || (k.Airline.Contains(sigAirline)))
                //&& CheckAirline(k.Airline, Airline)
                && (k.MinPrice == 0 || k.MinPrice <= (priceAvg))
                && CheckCabin(k.CabinClass.FirstOrDefault(), cabin, k.CabinClassMatchMode)
            ).OrderBy(k => k.SequenceNum).ToList();

            return fareRule.FirstOrDefault();
        }
        public Bal.GfMarkup.FeeRule getFareRule(AirContext airContext, decimal totFare)
        {
            List<Core.CabinType> cabin = new List<Core.CabinType>();
            string sigAirline = string.Empty;
            int i = 0;
            foreach (var fs in airContext.flightSearchResponse.Results[0][0].FlightSegments)
            {
                foreach (var seg in fs.Segments)
                {
                    if (!cabin.Exists(o => o == seg.CabinClass))
                    {
                        cabin.Add(seg.CabinClass);
                    }
                    //if (!Airline.Exists(o => o.Equals(seg.airline, StringComparison.OrdinalIgnoreCase)))
                    //{
                    //    Airline.Add(seg.airline);
                    //}
                    if (i == 0 && string.IsNullOrEmpty(sigAirline))
                    {
                        if (Core.FlightUtility.GetAirport(seg.Origin).countryCode != Core.FlightUtility.GetAirport(seg.Destination).countryCode)
                        {
                            sigAirline = seg.Airline;
                        }
                    }
                }
                i++;
            }
            if (string.IsNullOrEmpty(sigAirline))
            {
                sigAirline = airContext.flightSearchResponse.Results[0][0].FlightSegments[0].Segments[0].Airline;
            }
            if (isSetMarkupRule == false)
            {
                setMarkupRule();
            }
            var fareRule = ruleSet.feeRule.Where(k => k.isKeepMarkup
               && (k.FromCountry.Count == 0 || (k.FromCountry.Contains(airContext.flightSearchRequest.segment.FirstOrDefault().orgArp.countryCode)))
               && (k.ToCountry.Count == 0 || (k.ToCountry.Contains(airContext.flightSearchRequest.segment.FirstOrDefault().destArp.countryCode)))
               && (k.Airline.Count == 0 || (k.Airline.Contains(sigAirline)))
               //&& CheckAirline(k.Airline, Airline)
               && (k.MinPrice == 0 || k.MinPrice <= (totFare))
               && CheckCabin(k.CabinClass.FirstOrDefault(), cabin, k.CabinClassMatchMode)
           ).OrderBy(k => k.SequenceNum).ToList();

            return fareRule.FirstOrDefault();

        }
        public static Core.CabinType getCabinType(string cabin)
        {
            if (cabin.Equals("PremiumEconomy", StringComparison.OrdinalIgnoreCase) || cabin.Equals("PREMIUM", StringComparison.OrdinalIgnoreCase))
            {
                return Core.CabinType.PremiumEconomy;
            }
            else if (cabin.Equals("Business", StringComparison.OrdinalIgnoreCase))
            {
                return Core.CabinType.Business;
            }
            else if (cabin.Equals("First", StringComparison.OrdinalIgnoreCase))
            {
                return Core.CabinType.First;
            }
            else
            {
                return Core.CabinType.Economy;
            }
        }
        public TimeSpan ToTimeZoneTime(DateTime time, string timeZoneId, string timeZoneId2)
        {
            TimeZoneInfo tzi = getTimeZone(timeZoneId);
            TimeZoneInfo tzi2 = getTimeZone(timeZoneId2);
            if (tzi != null && tzi2 != null)
            {
                TimeSpan ts = tzi.GetUtcOffset(time);
                TimeSpan ts2 = tzi2.GetUtcOffset(time);
                return ts2 - ts;
            }
            else
            {
                return new TimeSpan();
            }
        }
        public static TimeZoneInfo getTimeZone(string windowsTimeZoneId)
        {
            var windowsTimeZone = default(TimeZoneInfo);

            try { windowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId); }
            catch (TimeZoneNotFoundException ex) { }
            catch (InvalidTimeZoneException) { }

            return windowsTimeZone;
        }

        public bool CheckAirline(List<string> markupAirline, List<string> segAirline)
        {
            if (markupAirline.Count == 0 || (markupAirline.Where(i => segAirline.Contains(i)).ToList().Count == segAirline.Count))
            {
                return true;
            }
            else
                return false;
        }
        public bool CheckCabin(Core.CabinType markupCabin, List<Core.CabinType> segCabin, bool CabinMatchType)
        {
            bool retStatus = false;
            if (markupCabin == Core.CabinType.None)
            {
                retStatus = true;
            }
            else
            {
                if (CabinMatchType)
                {
                    retStatus = (segCabin.Count == 1 && segCabin[0] == markupCabin);
                }
                else
                {
                    retStatus = segCabin.Exists(k => k == markupCabin);
                }

            }
            return retStatus;
        }


        public static void setMarkupRule()
        {
            string path = System.IO.Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "Rules.xml");
            string data = "";
            using (System.IO.StreamReader r = new System.IO.StreamReader(path))
            {
                data = r.ReadToEnd();
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);

            Bal.GfMarkup.RuleSet _ruleSet = new GfMarkup.RuleSet()
            {
                carrierGroup = new List<GfMarkup.CarrierGroup>(),
                feeRule = new List<GfMarkup.FeeRule>(),
                locationGroup = new List<GfMarkup.LocationGroup>(),

            };
            if (xmlDoc.SelectSingleNode("RuleSet/LocationGroup") != null)
            {
                foreach (XmlNode xnd in xmlDoc.SelectNodes("RuleSet/LocationGroup"))
                {
                    GfMarkup.LocationGroup locGrp = new GfMarkup.LocationGroup() { TableNum = Convert.ToInt32(xnd.SelectSingleNode("TableNum").InnerText), location = new List<string>() };
                    foreach (XmlNode xndChd in xnd.SelectNodes("Location"))
                    {
                        locGrp.location.Add(xndChd.Attributes["id"].Value.Replace("N:", ""));
                    }
                    _ruleSet.locationGroup.Add(locGrp);
                }
            }
            if (xmlDoc.SelectSingleNode("RuleSet/CarrierGroup") != null)
            {
                foreach (XmlNode xnd in xmlDoc.SelectNodes("RuleSet/CarrierGroup"))
                {
                    GfMarkup.CarrierGroup carrGrp = new GfMarkup.CarrierGroup() { TableNum = Convert.ToInt32(xnd.SelectSingleNode("TableNum").InnerText), carrier = new List<string>() };
                    foreach (XmlNode xndChd in xnd.SelectNodes("Carrier"))
                    {
                        carrGrp.carrier.Add(xndChd.Attributes["id"].Value.Replace("N:", ""));
                    }
                    _ruleSet.carrierGroup.Add(carrGrp);
                }
            }
            if (xmlDoc.SelectSingleNode("RuleSet/FeeRule") != null)
            {
                foreach (XmlNode xnd in xmlDoc.SelectNodes("RuleSet/FeeRule"))
                {
                    GfMarkup.FeeRule fee = new GfMarkup.FeeRule() { Airline = new List<string>(), FromCountry = new List<string>(), ToCountry = new List<string>(), CabinClass = new List<Core.CabinType>() };
                    fee.carrierAtr = xnd.Attributes["carrier"].Value;
                    if (!fee.carrierAtr.Equals("**", StringComparison.OrdinalIgnoreCase))
                    {
                        if (fee.carrierAtr.IndexOf("U:", StringComparison.OrdinalIgnoreCase) == -1)
                        {
                            fee.Airline.Add(fee.carrierAtr);
                        }
                        else
                        {
                            foreach (var item in _ruleSet.carrierGroup.Where(k => k.TableNum == Convert.ToInt32(fee.carrierAtr.Replace("U:", ""))).FirstOrDefault().carrier)
                            {
                                fee.Airline.Add(item);
                            }
                        }
                    }

                    if (xnd.SelectSingleNode("Origin") != null)
                    {
                        fee.Origin = xnd.SelectSingleNode("Origin").InnerText;
                        if (fee.Origin.IndexOf("N:", StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            fee.FromCountry.Add(fee.Origin.Replace("N:", ""));
                        }
                        else if (fee.Origin.IndexOf("U:", StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            foreach (var item in _ruleSet.locationGroup.Where(k => k.TableNum == Convert.ToInt32(fee.Origin.Replace("U:", ""))).FirstOrDefault().location)
                            {
                                fee.FromCountry.Add(item.Replace("N:", ""));
                            }
                        }
                    }

                    if (xnd.SelectSingleNode("Destination") != null)
                    {
                        fee.Destination = xnd.SelectSingleNode("Destination").InnerText;
                        if (fee.Destination.IndexOf("N:", StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            fee.ToCountry.Add(fee.Origin.Replace("N:", ""));
                        }
                        else if (fee.Destination.IndexOf("U:", StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            foreach (var item in _ruleSet.locationGroup.Where(k => k.TableNum == Convert.ToInt32(fee.Destination.Replace("U:", ""))).FirstOrDefault().location)
                            {
                                fee.ToCountry.Add(item.Replace("N:", ""));
                            }
                        }
                    }

                    fee.SequenceNum = Convert.ToInt32(xnd.SelectSingleNode("SequenceNum").InnerText);
                    if (xnd.SelectSingleNode("CabinClass") != null)
                    {
                        fee.CabinClass.Add(getCabinType(xnd.SelectSingleNode("CabinClass").InnerText));
                    }
                    if (xnd.SelectSingleNode("CabinClassMatchMode") != null)
                    {
                        if (xnd.SelectSingleNode("CabinClassMatchMode").InnerText.Equals("ALL_SEGMENTS", StringComparison.OrdinalIgnoreCase))
                        {
                            fee.CabinClassMatchMode = true;
                        }
                    }
                    if (xnd.SelectSingleNode("Suppress") != null)
                    {
                        fee.Suppress = Convert.ToBoolean(xnd.SelectSingleNode("Suppress").InnerText);
                    }
                    if (xnd.SelectSingleNode("FeePercent") != null)
                    {
                        fee.FeePercent = Convert.ToDecimal(xnd.SelectSingleNode("FeePercent").InnerText.Replace("USD", "").Replace("CAD", "").Replace("INR", ""));
                        fee.isKeepMarkup = true;
                    }
                    if (xnd.SelectSingleNode("FeeAmount") != null)
                    {
                        fee.FeeAmount = Convert.ToDecimal(xnd.SelectSingleNode("FeeAmount").InnerText.Replace("USD", "").Replace("CAD", "").Replace("INR", ""));
                        fee.isKeepMarkup = true;
                    }
                    if (xnd.SelectSingleNode("MinPrice") != null)
                    {
                        fee.MinPrice = Convert.ToDecimal(xnd.SelectSingleNode("MinPrice").InnerText.Replace("USD", "").Replace("CAD", "").Replace("INR", ""));
                    }
                    _ruleSet.feeRule.Add(fee);
                }
            }
            ruleSet = _ruleSet;
            isSetMarkupRule = true;
        }
    }


}
namespace Bal.GfMarkup
{
    public class RuleSet
    {
        public List<LocationGroup> locationGroup { get; set; }
        public List<CarrierGroup> carrierGroup { get; set; }
        public List<FeeRule> feeRule { get; set; }
    }
    public class LocationGroup
    {
        public int TableNum { get; set; }
        public List<string> location { get; set; }
    }
    public class CarrierGroup
    {
        public int TableNum { get; set; }
        public List<string> carrier { get; set; }
    }
    public class FeeRule
    {
        public int SequenceNum { get; set; }
        public string carrierAtr { get; set; }
        //public CarrierGroup CarrierList { get; set; }
        public bool Suppress { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        //public string OriginCountry { get; set; }

        public List<string> Airline { get; set; }
        public List<string> FromCountry { get; set; }
        public List<string> ToCountry { get; set; }
        public List<Core.CabinType> CabinClass { get; set; }
        public bool CabinClassMatchMode { get; set; }
        public decimal FeePercent { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal MinPrice { get; set; }
        public bool isKeepMarkup { get; set; }
    }
}
