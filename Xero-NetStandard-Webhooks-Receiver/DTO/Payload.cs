using System.Collections.Generic;

namespace XeroWebhooks.DTO{
    public class Payload
    {
        public List<PayloadEvent> Events {get; set;}
        public int LastEventSequence {get; set;}
        public int FirstEventSequence {get; set;}
        public string Entropy {get; set;}
    }    
}