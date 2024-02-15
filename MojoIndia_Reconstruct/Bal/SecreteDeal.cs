using Core;
using Core.Flight;
//using Core.SecretDeal;
//using DAL.SecretDeal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bal
{
    public class SecreteDeal
    {
        //public FlightSearchResponse MakeResult( FlightSearchRequest fsr)
        //{
        //    FlightSearchResponse response = new FlightSearchResponse() {
        //        adults = fsr.adults,
        //        child = fsr.child,
        //        infants = fsr.infants,
               
        //        airline = new List<Airline>(),
        //        aircraftDetail=new List<AircraftDetail>(),
        //        airport=new List<Airport>(),
        //        flightResult=new List<FlightResult>(),
        //        operatedAirline=new List<Airline>(),
        //        responsStatus=new ResponseStatus()
                
        //    };
          
        //    List<string> allAirline = new List<string>();
        //    string orgCountry = FlightUtility.GetAirport(fsr.segment[0].originAirport).countryCode;
        //    string destCountry = FlightUtility.GetAirport(fsr.segment[0].destinationAirport).countryCode;

        //    List<SecretFareDetails> Fresult = new DalSecretFareDetails().Get_FareDetails(0, orgCountry, destCountry, fsr.segment[0].originAirport, fsr.segment[0].originAirport,(int)fsr.cabinType);
        //    DateTime travelDate = fsr.segment[0].travelDate;
        //    if (Fresult.Count > 0)
        //    {
        //        Random random = new Random();
        //        foreach (SecretFareDetails fd in Fresult)
        //        {
        //            decimal fare = (random.Next(Convert.ToInt32(fd.MinFare), Convert.ToInt32(fd.MaxFare)));
        //            decimal incrimentVal = fd.Markup > 0 ? fd.Markup : ((fd.MinFare * 10) / 100);
        //            int ctr = 0;
        //            List<string> lstStop = fd.stop.Split(',').ToList();
        //            foreach (string strAirline in fd.Airline)
        //            {
        //                FlightResult result = new FlightResult();
        //                result.isCallCenterFare = true;
        //                result.cabinClass = fsr.cabinType;
        //                result.fareType = FareType.Private;
        //                result.AllSegmentKey = fare.ToString();
        //                result.gdsType = GdsType.InHouse;
        //                result.fare = new Fare();
        //                decimal rfare = fare + (incrimentVal * ctr) + Convert.ToDecimal(random.Next(10, 99) * .01);
        //                result.fare.adultFare = rfare * .50M;
        //                result.fare.adultTax = rfare * .50M;
        //                if (fsr.child > 0)
        //                {
        //                    result.fare.childFare = rfare * .50M;
        //                    result.fare.childTax = rfare * .50M;
        //                }
        //                if (fsr.infants > 0)
        //                {
        //                    result.fare.infantFare = rfare * .50M;
        //                    result.fare.infantTax = rfare * .50M;
        //                }
        //                result.fare.grandTotal = rfare * (fsr.adults + fsr.child + fsr.infants);
        //                result.flightSegments = new List<FlightSegment>();
        //                int stop = lstStop.Count - 1 >= ctr ? Convert.ToInt32(string.IsNullOrEmpty(lstStop[ctr]) ? "0" : lstStop[ctr]) : 0;
        //                int segCtr = 0;
        //                foreach (SearchSegment ss in fsr.segment)
        //                {
        //                    FlightSegment fs = new FlightSegment();
        //                    fs.eft = 0;
        //                    fs.LayoverTime = 0;
        //                    fs.segName = segCtr == 0 ? "Depart" : "Return";
        //                    fs.stop = stop+1;
        //                    fs.segments = new List<Segment>();
        //                    for (int i = 0; i <= stop; i++)
        //                    {
        //                        Segment seg = new Segment() {
        //                            airline = strAirline,
        //                            baggage = "",
        //                            cabinClass = fsr.cabinType,
        //                            cabinType = fsr.cabinType.ToString(),
        //                            depDate = ss.travelDate,
        //                            eft=0,
        //                            equipmentType="",
        //                            estimateTime=0,
        //                            fareBaseCode="",
        //                            fareType="",
        //                            flightDuration=0,
        //                            flightID=segCtr.ToString(),
        //                            flightNo="",
        //                            fromAirport=i==0?ss.originAirport:"000",
        //                            fromTerminal="",
        //                            isFlexiFare=false,
        //                            isHideAirline=false,
        //                            isNearByDest=false,
        //                            isNearByOrg=false,
        //                            layOverTime=0,
        //                            opratingAirline=strAirline,
        //                            orderNo=segCtr,
        //                            reachDate=ss.travelDate,
        //                            resDesignCode="",
        //                            seats=9,
        //                            SequenceNumber=segCtr,
        //                            sliceAndDiceCode="",
        //                            techStopDetails=new List<TechnicalStopDetails>(),
        //                            techStops="",
        //                            toAirport="000",
        //                            toTerminal=""
        //                        };                               
        //                        fs.segments.Add(seg);
        //                    }
        //                    fs.segments.LastOrDefault().toAirport = ss.destinationAirport;
        //                    result.flightSegments.Add(fs);
        //                    segCtr++;
        //                }
        //                response.flightResult.Add(result);
        //                ctr++;
        //            }
        //        }
        //        response.minPrice = 0;
        //        response.maxPrice = 0;
        //        response.noOfResult = response.flightResult.Count;
                
        //        foreach (var item in response.flightResult)
        //        {
        //            foreach (var flightSegment in item.flightSegments)
        //            {
        //                foreach (var seg in flightSegment.segments)
        //                {
        //                    if (response.airline.Where(o => o.code.Equals(seg.airline, StringComparison.OrdinalIgnoreCase)).ToList().Count == 0)
        //                    {
        //                        response.airline.Add(FlightUtility.GetAirline(seg.airline));
        //                    }
        //                    if (response.operatedAirline.Where(o => o.code.Equals(seg.opratingAirline, StringComparison.OrdinalIgnoreCase)).ToList().Count == 0)
        //                    {
        //                        response.operatedAirline.Add(FlightUtility.GetAirline(seg.opratingAirline));
        //                    }
        //                    if (response.airport.Where(o => o.airportCode.Equals(seg.fromAirport, StringComparison.OrdinalIgnoreCase)).ToList().Count == 0)
        //                    {
        //                        response.airport.Add(FlightUtility.GetAirport(seg.fromAirport));
        //                    }
        //                    if (response.airport.Where(o => o.airportCode.Equals(seg.toAirport, StringComparison.OrdinalIgnoreCase)).ToList().Count == 0)
        //                    {
        //                        response.airport.Add(FlightUtility.GetAirport(seg.toAirport));
        //                    }
        //                    if (response.aircraftDetail.Where(o => o.aircraftCode.Equals(seg.equipmentType, StringComparison.OrdinalIgnoreCase)).ToList().Count == 0)
        //                    {
        //                        response.aircraftDetail.Add(FlightUtility.GetAircraftDetail(seg.equipmentType));
        //                    }                            
        //                }                    
        //            }
        //        }
        //    }
        //    return response;
        //}
    }
}
