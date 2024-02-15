using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [DataContract]
    public class SendEmailRequest
    {
        [DataMember]
        public string FromEmail { get; set; }
        [DataMember]
        public string ToEmail { get; set; }
        [DataMember]
        public string CcEmail { get; set; }
        [DataMember]
        public string BccEmail { get; set; }
        [DataMember]
        public string MailSubject { get; set; }
        [DataMember]
        public string MailBody { get; set; }
        [DataMember]
        public long BookingID { get; set; }
        [DataMember]
        public int prodID { get; set; }
        [DataMember]
        public string MailType { get; set; }
        [DataMember]
        public bool isBodyCompress { set; get; }
    }
}
