using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class CalendarFareUpdateRequest
    {
        [DataMember]
        public SearchResult searchResults { get; set; }
        [DataMember]
        public List<SearchSegment> segment { get; set; }
        [DataMember]
        public string userSessionID { get; set; }
        [DataMember]
        public string userSearchID { get; set; }
        [DataMember]
        public string TraceId { get; set; }
        [DataMember]
        public string userIP { get; set; }
        [DataMember]
        public TripType tripType { get; set; }
        [DataMember]
        public int adults { get; set; }
        [DataMember]
        public int child { get; set; }
        [DataMember]
        public int infants { get; set; }
        [DataMember]
        public CabinType cabinType { get; set; }
        [DataMember]
        public string airline { get; set; }
        [DataMember]
        public Core.TravelType travelType { get; set; }
    }
}
