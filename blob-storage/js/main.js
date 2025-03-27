const { BlobServiceClient } = require("@azure/storage-blob");

async function main() {
    // Retrieve Azure Storage account details from environment variables
    const accountName = process.env.AZURE_SA_NAME;
    const accountKey = process.env.AZURE_SA_KEY;

    if (!accountName || !accountKey) {
        console.error("Please set the AZURE_SA_NAME and AZURE_SA_KEY environment variables.");
        return;
    }

    // Create a connection string
    const connectionString = `DefaultEndpointsProtocol=https;AccountName=${accountName};AccountKey=${accountKey};EndpointSuffix=core.windows.net`;

    // Create a BlobServiceClient
    const blobServiceClient = BlobServiceClient.fromConnectionString(connectionString);

    // List containers
    console.log("Listing containers...");
    for await (const container of blobServiceClient.listContainers()) {
        console.log(`Container: ${container.name}`);
    }

    // Create a new container
    const containerName = "container-from-js";
    console.log(`Creating container: ${containerName}`);
    const containerClient = blobServiceClient.getContainerClient(containerName);
    await containerClient.createIfNotExists();

    // Upload a blob
    const blobName = "myblob";
    const blobContent = "Hello, World!";
    console.log(`Uploading blob: ${blobName}`);
    const blockBlobClient = containerClient.getBlockBlobClient(blobName);
    await blockBlobClient.upload(blobContent, blobContent.length);

    // Download the blob
    console.log(`Downloading blob: ${blobName}`);
    const downloadBlockBlobResponse = await blockBlobClient.download(0);
    const downloadedContent = await streamToString(downloadBlockBlobResponse.readableStreamBody);
    console.log(`Blob content: ${downloadedContent}`);
}

// Helper function to convert a readable stream to a string
async function streamToString(readableStream) {
    return new Promise((resolve, reject) => {
        const chunks = [];
        readableStream.on("data", (data) => {
            chunks.push(data.toString());
        });
        readableStream.on("end", () => {
            resolve(chunks.join(""));
        });
        readableStream.on("error", reject);
    });
}

main().catch((err) => {
    console.error("Error running sample:", err.message);
});