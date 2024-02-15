using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [DataContract]
    public class PassengerDetails
    {
        [DataMember]
        public int travelerNo { get; set; }
        [DataMember]
        public PassengerType passengerType { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string middleName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]       
        public string day { get; set; }
        [DataMember]
        public string month { get; set; }
        [DataMember]
        public string year { get; set; }
        [DataMember]
        public DateTime dateOfBirth { get; set; }
        [DataMember]
        public Gender gender { get; set; }
        [DataMember]
        public string passportNumber { get; set; }
        [DataMember]
        public string nationality { get; set; }
        [DataMember]
        public string issueCountry { get; set; }
        [DataMember]
        public string exDay { get; set; }
        [DataMember]
        public string exMonth { get; set; }
        [DataMember]
        public string exYear { get; set; }
        [DataMember]
        public DateTime? expiryDate { get; set; }
        [DataMember]
        public string Meal { get; set; }
        [DataMember]
        public string Seat { get; set; }
        [DataMember]        
        public string SpecialAssistance { get; set; }
        [DataMember]        
        public string FFMiles { get; set; }
        [DataMember]
        public string TSA_Precheck { get; set; }
        [DataMember]
        public string RedressNumber { get; set; }
        [DataMember]
        public string ticketNo { get; set; }
        public PassengerDetails()
        {
            firstName = "";
            lastName = "";
            nationality = "";
            issueCountry = "";
            expiryDate = DateTime.Now.AddMonths(12);
        }
    }
}
