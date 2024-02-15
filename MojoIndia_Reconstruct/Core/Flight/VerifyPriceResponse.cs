using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Flight
{
    [DataContract]
    public class VerifyPriceResponse
    {
        [DataMember]
        public bool isFareChange { get; set; }
        [DataMember]
        public decimal adultFare { get; set; }
        [DataMember]
        public decimal childFare { get; set; }
        [DataMember]
        public decimal infantFare { get; set; }
        [DataMember]
        public decimal TaxWithMakrup { get; set; }
        [DataMember]
        public decimal grandTotal { get; set; }
        [DataMember]
        public decimal fareIncreaseAmount { get; set; }
        [DataMember]
        public decimal CouponIncreaseAmount { get; set; }
        [DataMember]
        public Fare sumFare { get; set; }
        [DataMember]
        public string RedirectUrl { get; set; }
       
    }
}
