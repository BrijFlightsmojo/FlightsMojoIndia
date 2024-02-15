using Core;
using Core.Flight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bal
{
    public class MarkupCalculator
    {
        public static bool isDomestic(string FromAirportCode, string ToAirportCode)
        {
            List<Airport> FromArp = FlightUtility.AirportList.Where(x => (x.airportCode.Equals(FromAirportCode, StringComparison.OrdinalIgnoreCase))).ToList();
            List<Airport> ToArp = FlightUtility.AirportList.Where(x => (x.airportCode.Equals(ToAirportCode, StringComparison.OrdinalIgnoreCase))).ToList();
            if (FromArp.Count > 0 && FromArp[0].countryCode.Equals("US", StringComparison.OrdinalIgnoreCase) && ToArp.Count > 0 && ToArp[0].countryCode.Equals("US", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (FromArp.Count > 0 && FromArp[0].countryCode.Equals("CA", StringComparison.OrdinalIgnoreCase) && ToArp.Count > 0 && ToArp[0].countryCode.Equals("CA", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
                return false;
        }

        public static void LoadStatiData()
        {
            if (!FlightUtility.isMasterDataLoaded)
            {
                FlightUtility.LoadMasterData();
            }
        }
    }
}
