// https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-node-get-started-send?tabs=passwordless%2Croles-azure-portal

const { EventHubProducerClient } = require("@azure/event-hubs");
const { DefaultAzureCredential } = require("@azure/identity");
const process = require("process");

// Event hubs 
const eventHubsResourceName = process.env.EVENT_HUBS_RESOURCE_NAME;
const fullyQualifiedNamespace = `${eventHubsResourceName}.servicebus.windows.net`; 
const eventHubName = process.env.EVENT_HUB_NAME;

// Azure Identity - passwordless authentication
const credential = new DefaultAzureCredential();

async function main() {

  // Create a producer client to send messages to the event hub.
  const producer = new EventHubProducerClient(fullyQualifiedNamespace, eventHubName, credential);

  // Prepare a batch of three events.
  const batch = await producer.createBatch();
  batch.tryAdd({ body: "passwordless 4 event" });
  batch.tryAdd({ body: "passwordless 5 event" });
  batch.tryAdd({ body: "passwordless 6 event" });    

  // Send the batch to the event hub.
  await producer.sendBatch(batch);

  // Close the producer client.
  await producer.close();

  console.log("A batch of three events have been sent to the event hub");
}

main().catch((err) => {
  console.log("Error occurred: ", err);
});