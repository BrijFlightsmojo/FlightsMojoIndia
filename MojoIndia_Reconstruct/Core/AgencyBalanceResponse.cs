using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.TVO
{
    [DataContract]
    public class AgencyBalanceResponse
    {
        [DataMember]
        public int AgencyType { get; set; }
        [DataMember]
        public double CashBalance { get; set; }
        [DataMember]
        public string CashBalanceInPrefCurrency { get; set; }
        [DataMember]
        public double CreditBalance { get; set; }
        [DataMember]
        public string CreditBalanceInPrefCurrency { get; set; }
        [DataMember]
        public bool DomHotelConfirmBookingLimitRequired { get; set; }
        [DataMember]
        public int DomHotelHoldBalanceLeft { get; set; }
        [DataMember]
        public string DomHotelHoldBalanceLeftInPrefCurrency { get; set; }
        [DataMember]
        public Error Error { get; set; }
        [DataMember]
        public bool IntlHotelConfirmBookingLimitRequired { get; set; }
        [DataMember]
        public int IntlHotelHoldBalanceLeft { get; set; }
        [DataMember]
        public string IntlHotelHoldBalanceLeftInPrefCurrency { get; set; }
        [DataMember]
        public bool IsNonAirOverrideCreditLimit { get; set; }
        [DataMember]
        public bool IsOverrideCreditLimit { get; set; }
        [DataMember]
        public string PreferredCurrency { get; set; }
        [DataMember]
        public int Status { get; set; }
    }
    [DataContract]
    public class Error
    {
        [DataMember]
        public int ErrorCode { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
