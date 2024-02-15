using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class Airline
    {
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string name { get; set; }
    }
    [DataContract]
    public class AirlineWitPrice
    {
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public decimal price { get; set; }
        public AirlineWitPrice() 
        {
        }
        public AirlineWitPrice(string _code,string _name,decimal _price)
        {
            code = _code;
            name = _name;
            price = _price;
        }
    }
    [DataContract]
    public class StopWithPrice
    {
        [DataMember]
        public int noOfStop { get; set; }
        [DataMember]
        public string stopName { get; set; }
        [DataMember]
        public decimal price { get; set; }
        public StopWithPrice()
        {
        }
        public StopWithPrice(int _noOfStop, string _stopName, decimal _price)
        {
            noOfStop = _noOfStop;
            stopName = _stopName;
            price = _price;
        }
    }
    [DataContract]
    public class AircraftDetail
    {
        [DataMember]
        public string aircraftCode { get; set; }
        [DataMember]
        public string bodyType { get; set; }
        [DataMember]
        public string aircraftName { get; set; }
        [DataMember]
        public string formOfPropulsion { get; set; }
        [DataMember]
        public string NoOfSeat { get; set; }
        //public AircraftDetail(string _AircraftCode, string _BodyType, string _AircraftName, string _FormOfPropulsion, string _NoOfSeat)
        //{
        //    AircraftCode = _AircraftCode;
        //    BodyType = _BodyType;
        //    AircraftName = _AircraftName;
        //    FormOfPropulsion = _FormOfPropulsion;
        //    NoOfSeat = _NoOfSeat;
        //}
        public AircraftDetail() { }
    }
}
