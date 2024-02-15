using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RP.Webhook
{
    public class AcquirerData
    {
        public string bank_transaction_id { get; set; }
    }

    public class Entity
    {
        public string id { get; set; }
        public string entity { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public string order_id { get; set; }
        public object invoice_id { get; set; }
        public bool international { get; set; }
        public string method { get; set; }
        public int amount_refunded { get; set; }
        public object refund_status { get; set; }
        public bool captured { get; set; }
        public string description { get; set; }
        public object card_id { get; set; }
        public string bank { get; set; }
        public object wallet { get; set; }
        public object vpa { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public List<object> notes { get; set; }
        public object fee { get; set; }
        public object tax { get; set; }
        public object error_code { get; set; }
        public object error_description { get; set; }
        public object error_source { get; set; }
        public object error_step { get; set; }
        public object error_reason { get; set; }
        public AcquirerData acquirer_data { get; set; }
        public int created_at { get; set; }
    }

    public class Payload
    {
        public Payment payment { get; set; }
    }

    public class Payment
    {
        public Entity entity { get; set; }
    }

    public class WebhookSuccess
    {
        public string entity { get; set; }
        public string account_id { get; set; }
        public string @event { get; set; }
        public List<string> contains { get; set; }
        public Payload payload { get; set; }
        public int created_at { get; set; }
    }
    public class RazorPay_WebhooksDetails
    {
     
        public int GatewayID { get; set; }
        public string WebhookPageType { get; set; }
        public string order_id { get; set; }
        public string account_id { get; set; }
        public string entity_id { get; set; }
        public string entity { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public string invoice_id { get; set; }
        public string international { get; set; }
        public string method { get; set; }
        public decimal amount_refunded { get; set; }
        public string refund_status { get; set; }
        public string captured { get; set; }
        public string description { get; set; }
        public string card_id { get; set; }
        public string bank { get; set; }
        public string wallet { get; set; }
        public string vpa { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public decimal fee { get; set; }
        public decimal tax { get; set; }
        public string error_code { get; set; }
        public string error_description { get; set; }
        public string error_source { get; set; }
        public string error_step { get; set; }
        public string error_reason { get; set; }
        public string bank_transaction_id { get; set; }
      
       
        }
    }
