using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RP
{
   public class PaymentCaptured
    {
        public string orderid { get; set; }
        public string Paymentid { get; set; }
        public string amount { get; set; }
        public string status { get; set; }
        public string msg { get; set; }
        public string RedirectUrl { get; set; }
    }
}
