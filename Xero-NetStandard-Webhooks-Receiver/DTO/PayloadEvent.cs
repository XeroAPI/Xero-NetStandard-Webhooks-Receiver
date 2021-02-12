using System;

namespace XeroWebhooks.DTO{
    public class PayloadEvent
    {
        public string ResourceUrl {get; set;}
        public Guid ResourceId {get; set;}
        public DateTime EventDateUtc {get; set;}
        public EventType EventType {get; set;}
        public EventCategory EventCategory {get; set;}
        public Guid TenantId {get; set;}
        public TenantType TenantType {get; set;}    
   
    }
}