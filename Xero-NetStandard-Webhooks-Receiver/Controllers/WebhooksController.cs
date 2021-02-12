using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using XeroWebhooks.DTO;
using System.Collections.Concurrent;
using System.ComponentModel;
using Newtonsoft.Json;

namespace XeroNetStandardWebhooks.Controllers
{
    
    [Route("/webhooks")]
    public class WebhooksController : Controller
    {
        private readonly IOptions<WebhookSettings> webhookSettings;
        private readonly ConcurrentQueue<Payload> payloadQueue = new ConcurrentQueue<Payload>();

        private BackgroundWorker _ProcessPayloadWorker = new BackgroundWorker();

        public WebhooksController(IOptions<WebhookSettings> webhookSettings)
        {
            this.webhookSettings = webhookSettings;

            // Configure background worker 
            _ProcessPayloadWorker.WorkerSupportsCancellation = true;
            _ProcessPayloadWorker.DoWork += ProcessPayloadWorker_DoWork;
        }

        // Method called when webhook notification sent
        [HttpPost]
        public IActionResult Index()
        {
            var payloadString = GetRequestBody().Result.ToString();
            var signature = Request.Headers[webhookSettings.Value.XeroSignature].FirstOrDefault();

            if (!VerifySignature(payloadString, signature)){
                // Webhook signature invalid, reject payload
                return Unauthorized();
            }

            // Valid signature, enqueue payload to queue and start asynchronous processing of payload
            payloadQueue.Enqueue(JsonConvert.DeserializeObject<Payload>(payloadString));
            _ProcessPayloadWorker.RunWorkerAsync();
            return Ok();
        }
    
        // Gets request body from HTML POST
        private async Task<string> GetRequestBody(){
            using (var reader = new StreamReader(Request.Body)){
                return await reader.ReadToEndAsync();
            }
        }
        
        // Validate webhook signature, signature must match hash of json payload using webhook key as the hash key
        private bool VerifySignature(string payload, string signature){
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(webhookSettings.Value.WebhookKey);
            byte[] payloadByte = encoding.GetBytes(payload);

            using (var hmac = new HMACSHA256(keyByte)){
                byte[] hashMessage = hmac.ComputeHash(payloadByte);
                var hashMsg = Convert.ToBase64String(hashMessage);
                return Convert.ToBase64String(hashMessage) == signature ? true : false;
            }
        }

        // Invokes background worker to process payload
        private void ProcessPayloadWorker_DoWork(object sender, DoWorkEventArgs e){
            ProcessPayloadQueue();
        }

        // Use method to process payloads
        private void ProcessPayloadQueue(){
            while(payloadQueue.Count > 0){
                payloadQueue.TryDequeue(out Payload payload);
                foreach(PayloadEvent payloadEvent in payload.Events){
                    // Process payloads here
                    Debug.WriteLine("\nEvent Type: " + payloadEvent.EventType.ToString());
                    Debug.WriteLine("Event Category: " + payloadEvent.EventCategory.ToString() + "\n");
                }
            }
        }

    }
}
