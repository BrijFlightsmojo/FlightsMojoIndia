using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public enum ProductType : byte
    {
        None = 0,
        Flight = 1,
        Hotel = 2
    }
    public enum TripType : byte
    {
        NONE = 0,
        [Description("One way")]
        OneWay = 1,
        [Description("Round trip")]
        RoundTrip = 2,
        [Description("Multi job")]
        MultiCity = 3,
        [Description("All")]
        ALL = 4,
        OpenJow = 5
    }
    public enum ClientType : int
    {
        None = 0,
        Web = 1,
        Mobile = 2,
        CRM = 3,
        Meta = 4
    }
    public enum Device : int
    {
        None = 0,
        Desktop = 1,
        Mobile = 2
    }
    public enum CabinType : int
    {
        None = 0,
        Economy = 1,
        PremiumEconomy = 2,
        Business = 3,
        First = 4,
        ALL = 5
    }
    public enum GdsType : int
    {
        None = 0,
        Tbo = 1,
        TripJack = 2,
        Travelogy = 3,
        FareBoutique = 4,
        AirIQ = 5,
        GFS = 6,
        OneDFare = 8,
        SatkarTravel = 9,
        Ease2Fly = 10,
        Amadeus=11,
        Travelopedia = 13
    }
    public enum TransactionStatus : int
    {
        Error = 0,
        Success = 1,
    }
    public enum FareType : int
    {
        NONE = 0,
        ALL = 1,
        PUBLISH = 2,//TRIPJACK PUBLISHEDFARE
        COUPON = 3,//TRIPJACK COUPON
        CORPORATE = 4,//TRIPJACK COPORATE
        INSTANTPUR = 5,// INSTANTOFFERFARE
        SMECRPCON = 6,//TRIPJACK SME
        INST_SERIESPUR = 7,  // Offer Fare
        SAVER = 8,
        FLEXI = 9,//TRIPJACK FLEXI
        FLEXIFARE = 10,
        FAMILYFARE = 11,
        TACTICAL = 12,
        PREMIUM = 13,
        GOMORE = 14,
        CORPORATEFLEX = 15,
        FLEXIPLUS = 16,
        SPECIALRETURN = 17, 
        PREMIUMFLEX = 18,
        GOMOREFARE = 19,
        OFFERFARE = 20, // Offer Fare
        STANDARD = 21,
        CORP_CONNECT = 22,
        OFFER_FARE_WITH_PNR = 23, // Offer Fare
        OFFER_FARE_WITHOUT_PNR = 24, // Offer Fare
        PROMO = 25,
        SALE = 26,
        EXTRA = 27,
        LITE = 28,
        VALUE = 29,
        BUSINESS = 30,
        EXPRESS_VALUE = 31,
        VAL = 32,
        FLX = 33,
        SUPSAV = 34,
        FLEX = 35,
        BIGLITE = 36,
        BIGEASY = 37,
        FLY = 38,
        SCOOTPLUS = 39,
        EXPRESS_FLEXI = 40,
        COMFORT = 41,
        OFFER_RETURN_FARE_WITH_PNR = 42,// Offer Fare
        EXPRESS_MIXED = 43,
        OFFER_RETURN_FARE_WITHOUT_PNR = 44,// Offer Fare
        LIGHT = 45,
        CORPORATE_GOMORE = 46,
        RTSPLFARE = 47,
        DEALFARE = 48,
        RETAILFARE = 49,
        REGULAR = 50,
        GOFLEXI = 51,
        COUPON_FARE = 52,
        PRIVATE = 53,
        SPICEMAX = 54,
        SOTO = 55,
        VISTA_FLEX = 56,
        FLEXIBLE = 57,
        LATITUDE = 58,
        SPECIAL = 59,
        SME = 60,
        RESTRICTED = 61, // Offer Fare
        SUPER6E = 62,
        CLUSTER = 63,
        INST_SERIESPURPF2 = 64,
        ECONOMY = 65,
        Business_Standard = 66,
        Web_Special_Business = 67,
        Super_Value_Business = 68,
        FareFlexi_Saver_Economy = 69,
        Eco_Lite = 70,
        Flexi_Saver_Economy = 71,
        Flexible_Economy = 72,
        Business_Comfort = 73,
        Super_Flexible_Business = 74,
        NDC=75
        //[Description("Economy Lite")]
        //EconomyLite=65
        //Super6E = 65
    }
    public enum MojoFareType : int
    {
        None = 0,
        Publish = 1,
        Family = 2,
        Corporate = 3,
        Unknown = 4,
        Flexi = 5,
        SeriesFareWithPNR = 6,
        SeriesFareWithoutPNR = 7,
        SpecialReturn = 8,
        Promo = 9,
        Sale = 10,
        SME = 11,
        Business = 12,
        NDC=13
    }
    public enum AmountType : byte
    {
        None = 0,
        Amount = 1,
        Percentage = 2
    }
    public enum MarkupCalculationBasedOn : int
    {
        TotalBaseFare = 0,
        TotalTax = 1,
        TotalAmount = 2
    }
    public enum RuleType : int
    {
        None = 0,
        Discount = 1,
        Markup = 2
    }
    public enum PassengerType : int
    {
        None = 0,
        Adult = 1,
        Child = 2,
        Infant = 3,
        InfantWs = 4
    }
    public enum Gender : int
    {
        None = 0,
        Male = 1,
        Female = 2,
    }
    public enum TravelType : int
    {
        None = 0,
        Domestic = 1,
        International = 2
    }
    public enum SiteId : int
    {
        NONE = 0,
        ALL = 1,
        FlightsMojoIN = 2
    }
    public enum Stops : int
    {
        None = 0,
        NonStop = 1,
        OneStop = 2,
        TwoStop = 3,
        Morethan2 = 4,
    }
    public enum ProductId : int
    {
        NONE = 0,
        ALL = 1,
        Flight = 2,
        Hotel = 3,
        Car = 4,
    }
    public enum BookingStatus : byte
    {
        NONE = 0,
        Incomplete = 1,
        Confirmed = 2,
        Ticketed = 3,
        Failed = 4,
        Cancelled = 5,
        Pending = 6,
        InProgress = 7
    }
    public enum PaymentStatus : byte
    {
        NONE = 0,
        PaymentPending = 1,
        Completed = 2,
        CardDecline = 3,
        Refund = 4,
    }

    public enum WebsiteDeal : int
    {
        NONE = 0,
        DestinationDeal = 1,
        AirlineDeal = 2,
        HomePageDeal = 3,
    }
    public enum BookingType : byte
    {
        NONE = 0,
        WebBooking = 1,
        PnrImport = 2
    }
    public enum CardType
    {
        None = 0,
        Visa = 1,
        MasterCard = 2,
        AmericanExpress = 3,
        DinersClub = 4,
        Discover = 5,
        CarteBlanche = 6,
        Maestro = 7,
        BCCard = 8,
        JapanCreditBureau = 9,
        CartaSi = 10,
        CarteBleue = 11,
        VisaElectron = 12
    }
    public enum ChargeID
    {
        None = 0,
        BaseFare = 1,
        Tax = 2,
        Markup = 3,
        Insurance = 4,
        ConvenienceFee = 5,
        CancellaionPolicy = 6,
        FlexibleTicket = 7,
        Coupon = 8,
        AgentMarkup = 9,
        SeatsAssignCharges = 10,
        OtherCharges = 11,
        CommissionEarned = 12,
        FareIncreaseAmount = 13,
        ServiceFee = 14,
        CouponIncreaseAmount = 15

    }
    public enum ChargeFor
    {
        None = 0,
        Adult = 1,
        Child = 2,
        Infant = 3,
        InfantWS = 4,
        AllPax = 5,
        NA = 6,
    }
    public enum airlineBlockActionType : int
    {
        None = 0,
        CallCentreFare = 1,
        Masking = 2,
        Block = 3
    }
    public enum AirlineMatchType : int
    {
        None = 0,
        ConatinsAny = 1,
        ExactMatch = 2,
        DoesNotContain = 3
    }
    public enum AirlineClassMatchType : int
    {
        None = 0,
        ContainsAny = 1,
        ExactMatch = 2
    }
    public enum CustomerType : int
    {
        None = 0,
        B2B = 1,
        B2C = 2
    }
    public enum ServiceUseType : int
    {
        None = 0,
        Yes = 1,
        No = 2
    }
    public enum WeekDays : int
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }
    public enum CurrencyType : int
    {
        None = 0,
        INR = 1
    }
    public enum BookingAction : int
    {
        None = 0,
        MakePNR = 1,
        WithoutPNRConfirm = 2,
        Failed = 4,
        InProgress = 5
    }
    public enum CurrentBookingStatus : int
    {
        None = 0,
        Success = 1,
        Fail = 2,
        InProgress = 3
    }
    public enum CheckOperatedBy : int
    {
        None = 0,
        Match = 1
    }
    public enum RefundType
    {
        NonRefundable = 0,
        Refundable = 1,
        PartialRefundable = 2
    }
    public enum GetWayType : int
    {
        None = 0,
        PayU = 1,
        Razorpay = 2,
    }
    public enum PaymentMode : int
    {
        NONE = 0,
        upi = 1,
        card = 2,
        netbanking = 3,
        wallet = 4,
        emi = 5,
        paylater = 6,
        Paytm = 7,
        GooglePay = 8
    }
    public enum AirlineBlockAction : int
    {
        NONE = 0,
        Block = 1,
      
    }
      public enum PageType : int
    {
        None = 0,
        Flights = 1,
        City = 2,
        Airline = 3,
        Deals=4
    }

    public enum SubProvider : int
    {
        None = 0,
        AirIQ = 1,
        YatraTravelsEServices = 2,
        EconomicTravels = 3,
        IndianGamers = 4,
        KAVERITRAVELS = 5,
        CheapFixDeparture = 6,
        TransglobalHolidays = 7,
        FLYNEXT = 8,
        JUSTMYTRIP_IN = 9,
        SATKARTRAVELS = 10,
        AVMHOLIDAYS = 11,
        METROPOLITANTRAVELS = 12,
        OMTOURSANDTRAVELS = 13,
        DIAMONDAIRSERVICESPVT_LTD = 14,
        SSTRAVELHOUSE = 15,
        QIBLATAINTRAVELSPVTLTD = 16,
        AIRTICKETSERVICEINDIAPVTLTD = 17,
        DESTINYTOURSANDTRAVELS = 18,
        ABHISHEKAIRTICKET = 19,
        BESTFARES = 20,
        FDWALA = 21,
        METROTRAVELS = 22,
        TRIPCIRCUITHOLIDAYSPVTLTD = 23,
        TRIPMAKETRAVELPVT_LTD = 24,
        YATRATRAVELSANDESERVICES = 25,
        ONLINESERVICES = 26,
        KALAWATITOURANDTRAVELS = 27,
        MAHALAXMITOURSANDTRAVELS = 28,
        JOURNEYWITHUS = 29,
        BALAJITOURANDTRAVELS = 30,
        MALITRAVELS = 31,
        SETURTRIP = 32,
        CLICKMYLINKSHOLIDAYS = 33,
        HRTRAVELS = 34,
        FLYBIHAR = 35,
        MAHAVEERTHETRAVELSHOPPVTLTD = 36,
        GOFARE = 37,
        STARTRAVELSANDHOLIDAY = 38,
        OMSAITOURSANDTRAVELS = 39,
        AIRTB = 40
    }

}
