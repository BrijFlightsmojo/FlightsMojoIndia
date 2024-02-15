using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Whatsapp
{
    public class Destination
    {
        public List<string> waid { get; set; }
    }
    
    public class WA
    {
        public string appid { get; set; }
        public string deliverychannel { get; set; }
        public Message message { get; set; }
        public List<Destination> destination { get; set; }
    }

    public class Message
    {
        public string template { get; set; }
        public Parameters parameters { get; set; }
    }

    public class Parameters
    {
        public string variable1 { get; set; }
        public string variable2 { get; set; }
        public string variable3 { get; set; }
        public string variable4 { get; set; }
        public string variable5 { get; set; }
        public string variable6 { get; set; }
        public string variable7 { get; set; }
        public string variable8 { get; set; }
        public string variable9 { get; set; }
        public string variable10 { get; set; }
        public string variable11 { get; set; }
        public string variable12 { get; set; }
        public string variable13 { get; set; }
        public string variable14 { get; set; }
        public document document { get; set; }
    }

    public class document
    {
        public string link { get; set; }
        public string filename { get; set; }
    }
}
