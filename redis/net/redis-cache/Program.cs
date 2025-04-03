
using StackExchange.Redis;

string? connectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Please set the environment variable REDIS_CONNECTION_STRING.");
    return;
}

using (var cache = ConnectionMultiplexer.Connect(connectionString))
{
    IDatabase db = cache.GetDatabase();

    // Snippet below executes a PING to test the server connection
    var result = await db.ExecuteAsync("ping");
    Console.WriteLine($"PING = {result.Resp2Type} : {result}");

    // Key is found if less than 5 seconds from the last execution
    string getValue = await db.StringGetAsync("test:key");
    Console.WriteLine($"GET: {getValue}");

    // Call StringSetAsync on the IDatabase object to set the key "test:key" to the value "100"
    bool setValue = await db.StringSetAsync("test:key", "100");
    Console.WriteLine($"SET: {setValue}");

    // StringGetAsync retrieves the value for the "test" key
    getValue = await db.StringGetAsync("test:key");
    Console.WriteLine($"GET: {getValue}");

    // set ttl
    bool setExpiry = await db.KeyExpireAsync("test:key", TimeSpan.FromSeconds(5));
    Console.WriteLine($"SET TTL: {setExpiry}");
}

