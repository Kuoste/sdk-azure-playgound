using System;
using Azure.Storage.Blobs;

class Program
{
    static void Main(string[] args)
    {
        // Retrieve Azure Storage account details from environment variables
        string accountName = Environment.GetEnvironmentVariable("AZURE_SA_NAME");
        string accountKey = Environment.GetEnvironmentVariable("AZURE_SA_KEY");

        if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(accountKey))
        {
            Console.WriteLine("Please set the AZURE_SA_NAME and AZURE_SA_KEY environment variables.");
            return;
        }

        string connectionString = $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={accountKey};EndpointSuffix=core.windows.net";

        // Create a BlobServiceClient
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

        // List containers
        Console.WriteLine("Listing containers...");
        foreach (var container in blobServiceClient.GetBlobContainers())
        {
            Console.WriteLine($"Container: {container.Name}");
        }

        // Create a new container
        string containerName = "container-from-csharp";
        Console.WriteLine($"Creating container: {containerName}");
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        containerClient.CreateIfNotExists();

        // Upload a blob
        string blobName = "myblob";
        string blobContent = "Hello, World!";
        Console.WriteLine($"Uploading blob: {blobName}");
        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        blobClient.Upload(BinaryData.FromString(blobContent), overwrite: true);

        // Download the blob
        Console.WriteLine($"Downloading blob: {blobName}");
        var downloadedBlob = blobClient.DownloadContent();
        Console.WriteLine($"Blob content: {downloadedBlob.Value.Content.ToString()}");
    }
}