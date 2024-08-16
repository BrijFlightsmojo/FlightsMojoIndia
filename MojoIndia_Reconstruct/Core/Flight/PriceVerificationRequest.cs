using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class PriceVerificationRequest
    {
        [DataMember]
        public string userSearchID { get; set; }
        [DataMember]
        public string userLogID { get; set; }
        [DataMember]
        public string userSessionID { get; set; }
        [DataMember]
        public string userIP { get; set; }
        [DataMember]
        public List<FlightResult> flightResult { get; set; }
        [DataMember]
        public List<string> PriceID { get; set; }
        [DataMember]
        public int adults { get; set; }
        [DataMember]
        public int child { get; set; }
        [DataMember]
        public int infants { get; set; }
        [DataMember]
        public int infantsWs { get; set; }
        [DataMember]
        public SiteId siteID { get; set; }
        [DataMember]
        public string sourceMedia { get; set; }
        [DataMember]
        public string TvoTraceId { get; set; }
        [DataMember]
        public bool isFareQuote { get; set; }
        [DataMember]
        public bool isSSR { get; set; }
        [DataMember]
        public bool isFareRule { get; set; }
        [DataMember]
        public string tgy_Search_Key { get; set; }
        [DataMember]
        public string PhoneNo { get; set; }
        [DataMember]
        public string tgy_Request_id { get; set; }
        [DataMember]
        public string TvoResultIndex { get; set; }
        [DataMember]
        public string ST_ResultSessionID { get; set; }

        [DataMember]
        public string GFS_FlightKey { get; set; }

        [DataMember]
        public string origin { get; set; }
        [DataMember]
        public string destination { get; set; }

        [DataMember]
        public string depDate { get; set; }


        [DataMember]
        public string nextraflightkey { get; set; }
        [DataMember]
        public string flightdeeplinkurl { get; set; }
        [DataMember]
        public string nextracustomstr { get; set; }

        [DataMember]
        public string TravelType { get; set; }
        [DataMember]
        public string TripType { get; set; }
        [DataMember]
        public string selectedflighttw { get; set; }
}
}
