using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [DataContract]
    public class PaymentDetails
    {
        public PaymentDetails(bool withValue)
        {
            if (withValue)
            {
                cardCode = "VI";
                postalCode = "120363";
                state = "Haryana";
                city = "Gurgoan";
                country = "In";
                countryName = "India";
                address1 = "Plot No 83 Gurgoan haryana 120363";
                address2 = "";
                expiryMonth = "10";
                expiryYear = "2020";
                cvvNo = "123";
                cardHolderName = "ajay singh";
                cardNumber = "4111111111111111";
            }
        }
        public PaymentDetails()
        {
            
        }
        [DataMember]
        public string cardType { get; set; }
        [DataMember]
        public string cardCode { get; set; }     
        [DataMember]
        public string cardNumber { get; set; }
        [DataMember]
        public string cardHolderName { get; set; }
        [DataMember]
        public string expiryMonth { get; set; }
        [DataMember]
        public string expiryYear { get; set; }
        [DataMember]
        public string cvvNo { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string countryName { get; set; }
        [DataMember]
        public string address1 { get; set; }
        [DataMember]
        public string address2 { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string stateOther { get; set; }
        [DataMember]
        public string postalCode { get; set; }
        [DataMember]
        public string billingPhoneNo { get; set; }
        [DataMember]
        public string OnlinePaymentStauts { get; set; }
        [DataMember]
        public string Hash { get; set; }
        [DataMember]
        public bool IsReturnHashMatched { get; set; }
        [DataMember]
        public bool IsAmountMatch { get; set; }
        [DataMember]
        public string Rz_Amt { get; set; }
    }
}
