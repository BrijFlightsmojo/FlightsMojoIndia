using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class FlightSearchResponse
    {
        [DataMember]
        public ResponseStatus response { get; set; }
        [DataMember]
        public string TraceId { get; set; }
        [DataMember]
        public string tgy_Search_Key { get; set; }
        [DataMember]
        public List<List<FlightResult>> Results { get; set; }
        [DataMember]
        public List<Airline> airline { get; set; }
        [DataMember]
        public List<Airport> airport { get; set; }
        [DataMember]
        public List<AircraftDetail> aircraftDetail { get; set; }
        [DataMember]
        public int adults { get; set; }
        [DataMember]
        public int child { get; set; }
        [DataMember]
        public int infants { get; set; }
        [DataMember]
        public string result1Index { get; set; }
        [DataMember]
        public string result2Index { get; set; }
        [DataMember]
        public List<string> listGroupID { get; set; }
        [DataMember]
        public DateTime searchDate { get; set; }
        [DataMember]
        public bool isCacheFare { get; set; }
        [DataMember]
        public FlightSearchRequest SearchRequest { get; set; }
        [DataMember]
        public string FB_booking_token_id { get; set; }
        [DataMember]
        public Affiliate affiliate { get; set; }
        [DataMember]
        public string userSearchID { get; set; }
        [DataMember]
        public string userLogID { get; set; }
        [DataMember]
        public string ST_ResultSessionID { get; set; }

        [DataMember]
        public int E2F_id { get; set; }

        [DataMember]
        public string d_owner { get; set; }
        [DataMember]
        public string FlightKey { get; set; }

        [DataMember]
        public string GFS_FlightKey { get; set; }
        public FlightSearchResponse()
        {
        }
        public FlightSearchResponse(FlightSearchRequest request)
        {
            adults = request.adults;
            child = request.child;
            infants = request.infants;
            aircraftDetail = new List<Core.Flight.AircraftDetail>();
            airline = new List<Core.Flight.Airline>();
            airport = new List<Core.Flight.Airport>();
            response = new Core.ResponseStatus();
            Results = new List<List<Core.Flight.FlightResult>>();
            listGroupID = new List<string>();
            searchDate = DateTime.Now;
            SearchRequest = request;
            userSearchID = request.userSearchID;
            userLogID = request.userLogID;
        }
    }
    [DataContract]
    public class FlightSearchResponseShort
    {
        [DataMember]
        public ResponseStatus response { get; set; }
        [DataMember]
        public string TraceId { get; set; }
        [DataMember]
        public string tgy_Search_Key { get; set; }
        [DataMember]
        public List<List<FlightResult>> Results { get; set; }
        [DataMember]
        public string result1Index { get; set; }
        [DataMember]
        public string result2Index { get; set; }
        [DataMember]
        public List<string> listGroupID { get; set; }
        [DataMember]
        public string FB_booking_token_id { get; set; }
        [DataMember]
        public FlightSearchRequest SearchRequest { get; set; }
        [DataMember]
        public string ST_ResultSessionID { get; set; }
        public FlightSearchResponseShort()
        {
        }
        public FlightSearchResponseShort(FlightSearchRequest request)
        {
            response = new Core.ResponseStatus();
            Results = new List<List<Core.Flight.FlightResult>>();
            listGroupID = new List<string>();
            SearchRequest = request;
        }
    }


    [DataContract]
    public class FlightResult
    {
        [DataMember]
        public string ResultID { get; set; }
        [DataMember]
        public string ResultIndex { get; set; }
        [DataMember]
        public int Source { get; set; }
        [DataMember]
        public string valCarrier { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public GdsType gdsType { get; set; }
        [DataMember]
        public bool IsLCC { get; set; }
        [DataMember]
        public bool IsRefundable { get; set; }
        [DataMember]
        public string AirlineRemark { get; set; }
        [DataMember]
        public Fare Fare { get; set; }
        [DataMember]
        public List<Fare> FareList { get; set; }
        [DataMember]
        public List<FlightSegment> FlightSegments { get; set; }
        [DataMember]
        public DateTime? LastTicketDate { get; set; }
        [DataMember]
        public string TicketAdvisory { get; set; }
        [DataMember]
        public CabinType cabinClass { get; set; }
        [DataMember]
        public string groupID { get; set; }
        [DataMember]
        public bool isPreCuponAvailable { get; set; }
        [DataMember]
        public string ffResultIndex { get; set; }
        [DataMember]
        public FareType ffFareType { get; set; }
        [DataMember]
        public string ResultCombination { get; set; }
        [DataMember]
        public string Tgy_Flight_Id { get; set; }
        [DataMember]
        public string tgy_Flight_Key { get; set; }
        [DataMember]
        public string Tgy_Flight_No { get; set; }
        [DataMember]
        public InventoryFrom FB_InventoryFrom { get; set; }

        [DataMember]
        public string ST_ResultSessionID { get; set; }


    }
    [DataContract]
    public class BaggageInfo
    {
        public string checkInBaggage { get; set; }
        public string cabinBaggage { get; set; }
    }
    [DataContract]
    public class Fare
    {
        [DataMember]
        public int FB_flight_id { get; set; }
        [DataMember]
        public string FB_static { get; set; }
        [DataMember]
        public string tboResultIndex { get; set; }
        [DataMember]
        public string Tgy_FareID { get; set; }
        [DataMember]
        public string tjID { get; set; }
        [DataMember]
        public RefundType refundType { get; set; }
        [DataMember]
        public FareType FareType { get; set; }
        [DataMember]
        public MojoFareType mojoFareType { get; set; }
        [DataMember]
        public CabinType cabinType { get; set; }
        [DataMember]
        public string bookingClass { get; set; }
        [DataMember]
        public BaggageInfo baggageInfo { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public decimal NetFare { get; set; }
        [DataMember]
        public decimal PublishedFare { get; set; }
        [DataMember]
        public decimal grandTotal { get; set; }

        [DataMember]
        public decimal ShowFare { get; set; }

        //[DataMember]
        //public decimal showGrandTotal { get; set; }
        [DataMember]
        public decimal BaseFare { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public decimal YQTax { get; set; }
        [DataMember]
        public decimal AdditionalTxnFeePub { get; set; }
        [DataMember]
        public decimal AdditionalTxnFeeOfrd { get; set; }
        [DataMember]
        public decimal OtherCharges { get; set; }
        [DataMember]
        public decimal CommissionEarned { get; set; }
        [DataMember]
        public decimal PGCharge { get; set; }
        [DataMember]
        public decimal Discount { get; set; }
        [DataMember]
        public decimal OfferedFare { get; set; }
        [DataMember]
        public decimal TdsOnCommission { get; set; }
        [DataMember]
        public decimal TdsOnPLB { get; set; }
        [DataMember]
        public decimal TdsOnIncentive { get; set; }
        [DataMember]
        public decimal ServiceFee { get; set; }
        [DataMember]
        public decimal GST { get; set; }

        [DataMember]
        public decimal Markup { get; set; }
        [DataMember]
        public List<FareBreakdown> fareBreakdown { get; set; }
        [DataMember]
        public string markupID { get; set; }
        [DataMember]
        public decimal ffDiscount { get; set; }
        [DataMember]
        public decimal ConvenienceFee { get; set; }
        public List<string> msri { get; set; }
        public string sri { get; set; }
        [DataMember]
        public int SeatAvailable { get; set; }
        [DataMember]
        public bool isBlock { get; set; }
        [DataMember]
        public GdsType gdsType { get; set; }
        [DataMember]
        public int subProvider { get; set; }
        #region Satkar
        [DataMember]
        public decimal cgstax { get; set; }
        [DataMember]
        public decimal igstax { get; set; }
        [DataMember]
        public decimal sgstax { get; set; }
        [DataMember]
        public decimal flat { get; set; }
        [DataMember]
        public decimal yQTax { get; set; }
        [DataMember]
        public decimal additionalTxnFeeOfrd { get; set; }
        [DataMember]
        public decimal additionalTxnFeePub { get; set; }
        [DataMember]
        public decimal pGCharge { get; set; }
        [DataMember]
        public decimal artGST { get; set; }
        [DataMember]
        public decimal artGSTOnMFee { get; set; }
        [DataMember]
        public decimal artTDS { get; set; }
        [DataMember]
        public decimal otherCharges { get; set; }
        [DataMember]
        public decimal discount { get; set; }
        [DataMember]
        public decimal commissionEarned { get; set; }
        [DataMember]
        public decimal pLBEarned { get; set; }
        [DataMember]
        public decimal incentiveEarned { get; set; }
        [DataMember]
        public decimal artIncentive { get; set; }
        [DataMember]
        public decimal offeredFare { get; set; }
        [DataMember]
        public decimal tdsOnCommission { get; set; }
        [DataMember]
        public decimal tdsOnPLB { get; set; }
        [DataMember]
        public decimal tdsOnIncentive { get; set; }
        [DataMember]
        public decimal serviceFee { get; set; }
        [DataMember]
        public decimal totalBaggageCharges { get; set; }
        [DataMember]
        public decimal totalMealCharges { get; set; }
        [DataMember]
        public decimal totalSeatCharges { get; set; }
        [DataMember]
        public decimal totalSpecialServiceCharges { get; set; }
        [DataMember]
        public decimal transactionFee { get; set; }
        [DataMember]
        public decimal managementFee { get; set; }
        [DataMember]
        public decimal cGSTax { get; set; }
        [DataMember]
        public decimal sGSTax { get; set; }
        [DataMember]
        public decimal iGSTax { get; set; }
        [DataMember]
        public decimal feeSurcharges { get; set; }
        [DataMember]
        public decimal instantDiscontOnFare { get; set; }
        [DataMember]
        public decimal publishedFareSoldInSwapping { get; set; }
        [DataMember]
        public decimal taxSoldInSwapping { get; set; }
        [DataMember]
        public decimal baseFareSoldInSwapping { get; set; }
        [DataMember]
        public decimal instantDiscountSoldInSwapping { get; set; }
        [DataMember]
        public decimal yqSoldInSwapping { get; set; }
        [DataMember]
        public bool isFareSwapped { get; set; }

        [DataMember]
        public string ST_ResultSessionID { get; set; }

        #endregion

        [DataMember]
        public string  AQ_ticket_id { get; set; }
        [DataMember]
        public int E2F_id { get; set; }

        [DataMember]
        public string d_owner { get; set; }
        [DataMember]
        public string FlightKey { get; set; }

        [DataMember]
        public string GFS_FlightKey { get; set; }

    }

    [DataContract]
    public class FareBreakdown
    {
        [DataMember]
        public PassengerType PassengerType { get; set; }
        [DataMember]
        public decimal BaseFare { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public decimal YQTax { get; set; }
        [DataMember]
        public decimal AdditionalTxnFeePub { get; set; }
        [DataMember]
        public decimal AdditionalTxnFeeOfrd { get; set; }
        [DataMember]
        public decimal PGCharge { get; set; }
        [DataMember]
        public decimal Markup { get; set; }
        [DataMember]
        public decimal CommissionEarned { get; set; }
        [DataMember]
        public decimal TdsOnCommission { get; set; }
        [DataMember]
        public decimal GST { get; set; }
        [DataMember]
        public decimal ServiceFee { get; set; }
        [DataMember]
        public decimal OtherCharges { get; set; }
        [DataMember]
        public decimal Discount { get; set; }
    }

    [DataContract]
    public class FlightSegment
    {
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public string SegName { get; set; }
        [DataMember]
        public int stop { get; set; }
        [DataMember]
        public int LayoverTime { get; set; }
        [DataMember]
        public List<Segment> Segments { get; set; }
    }

    [DataContract]
    public class Segment
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public int SegmentIndicator { get; set; }
        [DataMember]
        public string Airline { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string FlightNumber { get; set; }
        [DataMember]
        public string equipmentType { get; set; }
        [DataMember]
        public string FareClass { get; set; }
        [DataMember]
        public string OperatingCarrier { get; set; }
        [DataMember]
        public string Origin { get; set; }
        [DataMember]
        public DateTime DepTime { get; set; }
        [DataMember]
        public string FromTerminal { get; set; }
        [DataMember]
        public string Destination { get; set; }
        [DataMember]
        public DateTime ArrTime { get; set; }
        [DataMember]
        public string ToTerminal { get; set; }
        [DataMember]
        public CabinType CabinClass { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public int layOverTime { get; set; }

        [DataMember]
        public bool IsETicketEligible { get; set; }
        [DataMember]
        public string resDesignCode { get; set; }
    }

    [DataContract]
    public class InventoryFrom
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string company_name { get; set; }
        [DataMember]
        public string website { get; set; }
        [DataMember]
        public string email_id { get; set; }
        [DataMember]
        public string mobile { get; set; }
        [DataMember]
        public string share_id { get; set; }
        [DataMember]
        public int balance { get; set; }
        [DataMember]
        public string logo { get; set; }
    }
}
