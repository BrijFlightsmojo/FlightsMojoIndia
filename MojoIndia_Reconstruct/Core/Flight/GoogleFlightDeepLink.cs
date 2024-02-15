using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class GoogleFlightDeepLink
    {
        [DataMember]
        public int Adult { get; set; }
        [DataMember]
        public int child { get; set; }
        [DataMember]
        public int infant { get; set; }
        [DataMember]
        public string PointOfSaleCountry { get; set; }
        [DataMember]
        public string UserLanguage { get; set; }
        [DataMember]
        public decimal DisplayedPrice { get; set; }
        [DataMember]
        public string DisplayedPriceCurrency { get; set; }
        [DataMember]
        public string UserCurrency { get; set; }
        [DataMember]
        public string TripType { get; set; }
        //[DataMember]
        //public string Slice1 { get; set; }
        //[DataMember]
        //public string Slice2 { get; set; }
        [DataMember]
        public List<Slice> slice { get; set; }
        [DataMember]
        public List<FlightSlice> flightSlice { get; set; }
        [DataMember]
        public string ReferralId { get; set; }
    }
    [DataContract]
    public class Slice
    {       
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public List<int> sliceId { get; set; }
       
    }
    [DataContract]
    public class FlightSlice
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string Cabin { get; set; }
        [DataMember]
        public string Carrier { get; set; }
        [DataMember]
        public string depDate { get; set; }
        [DataMember]
        public string depTime { get; set; }
        [DataMember]
        public string arrDate { get; set; }
        [DataMember]
        public string arrTime { get; set; }
        [DataMember]
        public string Origin { get; set; }
        [DataMember]
        public string Destination { get; set; }
        [DataMember]
        public string FlightNumber { get; set; }
        [DataMember]
        public string BookingCode { get; set; }
    }
}

