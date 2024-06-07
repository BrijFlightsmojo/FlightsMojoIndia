using Core.Flight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace MojoIndia.Controllers
{
    public class FlightOperation
    {

        public static AirContext GetAirContext(string searchId)
        {
            AirContext context = null;
            try
            {
                if (HttpRuntime.Cache.Get(searchId.ToLower()) != null)
                {
                    context = HttpRuntime.Cache.Get(searchId.ToLower()) as AirContext;
                    //if (DateTime.Now.Subtract(context.CreationTime).Minutes > 10)
                    //{
                    context.CreationTime = DateTime.Now;
                    SetAirContext(context);
                    //}
                }
            }
            catch (Exception ex)
            {
                //Utility.Logger.Error("CheapFlightFares.Business.Flight.FlightOperation.GetAirContext():" + ex.ToString());
            }
            return context;
        }
        public static void SetAirContext(AirContext context)
        {
            try
            {
                HttpRuntime.Cache.Insert(
                     context.flightSearchRequest.userSearchID.ToLower()
                     , context
                     , null
                     , DateTime.Now.AddMinutes(30)
                     , Cache.NoSlidingExpiration
                     );
            }
            catch (Exception ex)
            {
                //Utility.Logger.Error("CheapFlightFares.Business.Flight.FlightOperation.SetAirContext():" + ex.ToString());
            }
        }


        public static List<fareMatrix> GetMatrixFare(string org, string dest, DateTime travelDate)
        {
            List<fareMatrix> fareList = new List<fareMatrix>();
            List<fareMatrix> fareListTemp = null;
            try
            {
                if (HttpRuntime.Cache.Get(org + dest + "_MatrixFare") == null)
                {

                    fareListTemp = new DAL.Deal.WebsiteDeal().GetFareMatrix(org, dest);
                    if (fareListTemp != null && fareListTemp.Count > 0)
                        SetMatrixFare(org, dest, fareListTemp);
                }
                else
                {
                    fareListTemp = HttpRuntime.Cache.Get(org + dest + "_MatrixFare") as List<fareMatrix>;
                }
                int i = 0;
                if (fareListTemp.Count > 0)
                {
                    DateTime startdate = travelDate.AddDays(-15);
                    if (startdate < DateTime.Today)
                        startdate = DateTime.Today;

                    for (DateTime dd = startdate; dd < startdate.AddDays(30);dd= dd.AddDays(1))
                    {
                        fareMatrix fm = new fareMatrix();
                        fm.sqNo = i++;
                        fm.tDate = dd.ToString("dd-MMM");
                        fareMatrix fare = fareListTemp.Where(k => k.tDate.Equals(fm.tDate, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (fare == null)
                        {
                            fare = fareListTemp.Where(k => k.tDate.Contains(dd.ToString("MMM"))).OrderBy(k=>k.fare).FirstOrDefault();
                        }
                        if (fare == null)
                        {
                            fare = fareListTemp.OrderBy(k => k.fare).FirstOrDefault();
                        }
                        fm.fare = fare.fare;
                        fareList.Add(fm);
                    }
                }
            }
            catch (Exception ex)
            {
                //Utility.Logger.Error("CheapFlightFares.Business.Flight.FlightOperation.GetAirContext():" + ex.ToString());
            }
            return fareList;
        }
        public static void SetMatrixFare(string org, string dest, List<fareMatrix> fareList)
        {
            try
            {
                HttpRuntime.Cache.Insert(
                  (org + dest + "_MatrixFare")
                     , fareList
                     , null
                     , DateTime.Now.AddHours(4)
                     , Cache.NoSlidingExpiration
                     );
            }
            catch (Exception ex)
            {
                //Utility.Logger.Error("CheapFlightFares.Business.Flight.FlightOperation.SetAirContext():" + ex.ToString());
            }
        }

    }
}