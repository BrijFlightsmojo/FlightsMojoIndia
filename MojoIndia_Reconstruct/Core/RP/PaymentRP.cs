using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RP
{
   public class PaymentRP
    {
        public string orderid { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string amount { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string status { get; set; }

        public string RedirectUrl { get; set; }
    }
}
