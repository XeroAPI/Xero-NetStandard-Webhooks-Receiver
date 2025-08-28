📍 __Notice__ This application is now in Read Only status as it is now deprecated

# Xero Webhook Receiver - NetStandard
This application demonstrates how to receive webhooks from Xero.

<b> Application Features </b>

1. Accept a POST request from Xero
1. Verify payload signature 
1. Store payload to a queue 
1. Return correct HTTP status code
1. Background worker for asynchronous processing of payload queue

## Running Application
### Prerequisites
- A private or partner app connected to at least one Xero Organisation, to generate webhook events, if you don't have a xero app you can create one for free at the [developer portal](https://developer.xero.com/documentation/getting-started/getting-started-guide)
- [Ngrok](https://ngrok.com/), to tunnel network traffic to localhost

### From Source
1. Clone this repository
1. Open solution in a compatible IDE (e.g. Visual Studio Code)
1. Update `appsettings.json` with your app webhook key
1. Build and run solution, note, no front-end changes have been made, so, site will be default MVC home view 
1. Make server accessible to Xero using ngrok

### Making your server accessible from Xero (Ngrok)
1. Download ngrok using this [guide](https://www.twilio.com/docs/usage/tutorials/how-use-ngrok-windows-and-visual-studio-test-webhooks#:~:text=Chose%20%22Start%20ngrok%20Tunnel%22%20from,that%20URL%20in%20your%20browser)
1. Set you app's webhook delivery URL to {negrok_address}**/webhooks** (e.g. https://daaf38b6.ngrok.io/webhooks) in the developer portal 
1. Create or modify any contact or invoice in a Xero Organisation connected to your app and wait for the event to arrive, solution will display info in console

## Solution Structure
Directory | Description
--- | ---
`DTO` | 'Data-Transfer-Object', holds classes that model a Xero webhook payload
`Config` | Holds a class that the models webhook settings from `appsettings.json`
