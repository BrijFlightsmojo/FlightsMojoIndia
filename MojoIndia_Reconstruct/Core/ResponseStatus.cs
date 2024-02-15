using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [DataContract]
    public class ResponseStatus
    {
        [DataMember]
        public TransactionStatus status { get; set; }
        [DataMember]
        public string message { get; set; }
        public ResponseStatus()
        {
            status = TransactionStatus.Success;
            message = "Success";
        }

        [DataMember]
        public string Error_Code { get; set; }
        [DataMember]
        public string Error_Desc { get; set; }
        [DataMember]
        public string Error_InnerException { get; set; }
    }
}
