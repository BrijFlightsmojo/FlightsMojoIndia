using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Whatsapp
{
    public class AirtelWhatsapp
    {
        public string templateId { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        public Message message { get; set; }
        public MediaAttachment mediaAttachment { get; set; }
    }

    public class LoginDetails
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class Message
    {
        public List<string> variables { get; set; }
    }
    public class MediaAttachment
    {
        public string type { get; set; }
        public string url { get; set; }
        public string fileName { get; set; }
    }
}
