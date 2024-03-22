using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class Airport
    {
        [DataMember]
        public string airportCode { get; set; }
        [DataMember]
        public string airportName { get; set; }
        [DataMember]
        public string cityCode { get; set; }
        [DataMember]
        public string cityName { get; set; }
        [DataMember]
        public string countryCode { get; set; }
        [DataMember]
        public string countryName { get; set; }
        [DataMember]
        public int showSeq { get; set; }
        //[DataMember]
        //public string stateCode { get; set; }
        //[DataMember]
        //public string stateName { get; set; }
        //[DataMember]
        //public string continent { get; set; }
        //[IgnoreDataMember]
        //public string timeZone { get; set; }
        //[IgnoreDataMember]
        //public string timeZone2 { get; set; }
    }
    [DataContract]
    public class AirportWithTimeZone
    {
        [DataMember]
        public string airportCode { get; set; }
        [DataMember]
        public string timeZone { get; set; }
        [DataMember]
        public string timeZone2 { get; set; }
    }
}
