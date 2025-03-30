using Microsoft.Azure.Cosmos;
using System;


var connectionString = environment.GetEnvironmentVariable("AZURE_COSMOS_DB_CONNECTION_STRING");
var dbName = "cosmicworks";
var containerName = "products";
var partitionKey = "/category";
var throughputValue = 400;
var category = "Pitsat";


var cosmosClient = new CosmosClient(connectionString);
Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(dbName);
Container container = await database.CreateContainerIfNotExistsAsync(id: $"{containerName}", partitionKeyPath: $"{partitionKey}", throughput: throughputValue);


// QueryDefinition query = new QueryDefinition(
//     query: "SELECT * FROM products p WHERE p.category = @category"
//     )
//     .WithParameter("@category", category);

QueryDefinition query = new QueryDefinition(
    query: "SELECT * FROM products"
    );

using FeedIterator<Product> feed = container.GetItemQueryIterator<Product>(
    queryDefinition: query
);

Console.WriteLine($"Ran query:\t{query.QueryText}");

List<Product> items = new();
double requestCharge = 0d;
while (feed.HasMoreResults)
{
    FeedResponse<Product> response = await feed.ReadNextAsync();
    foreach (Product item in response)
    {
        items.Add(item);
    }
    requestCharge += response.RequestCharge;
}

foreach (var item in items)
{
    Console.WriteLine($"Found item:\t{item.name}\t[{item.id}]");
}
Console.WriteLine($"Request charge:\t{requestCharge:0.00}");

Console.WriteLine($"Running query with partition key value in requestOptions: {category}");
using FeedIterator<Product> resultSet = container.GetItemQueryIterator<Product>(
    query,
    requestOptions: new QueryRequestOptions()
    {
        PartitionKey = new PartitionKey(category),
        MaxItemCount = 1
    });

while (resultSet.HasMoreResults)
{
    FeedResponse<Product> response = await resultSet.ReadNextAsync();
    foreach (Product item in response)
    {
        Console.WriteLine($"Item: {item.id} {item.category}");
    }
}