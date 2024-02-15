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
       
    }
}