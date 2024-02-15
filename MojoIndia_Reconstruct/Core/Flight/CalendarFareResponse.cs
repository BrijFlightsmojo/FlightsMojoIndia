using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class CalendarFareResponse
    {
        [DataMember]
        public ResponseStatus response { get; set; }
        [DataMember]
        public string TraceId { get; set; }
        [DataMember]
        public List<SearchResult> SearchResults { get; set; }
    }

    public class SearchResult
    {
        [DataMember]
        public string AirlineCode { get; set; }
        [DataMember]
        public string AirlineName { get; set; }
        [DataMember]
        public DateTime DepartureDate { get; set; }
        [DataMember]
        public bool IsLowestFareOfMonth { get; set; }
        [DataMember]
        public decimal Fare { get; set; }
        [DataMember]
        public decimal BaseFare { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public decimal OtherCharges { get; set; }
        [DataMember]
        public decimal FuelSurcharge { get; set; }
    }


}
