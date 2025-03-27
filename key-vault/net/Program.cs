using System;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure.Core;
using System.Threading;
using System.Threading.Tasks;
// dotnet add package Azure.Identity
// dotnet add package Azure.Security.KeyVault.Secrets

/// Use the Azure.Identity library to authenticate the application with Azure Key Vault.
/// Install Azure CLI and login with az login --use-device-code
/// Set the environment variables AZURE_KEYVAULT_NAME and AZURE_KEYVAULT_SECRET_NAME
/// Azure CLI install e.g. curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
class Program
{
    static async Task Main(string[] args)
    {
        // Retrieve a secret from the Key Vault
        string keyVaultName = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_NAME");
        string secretName = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_SECRET_NAME");

        if (string.IsNullOrEmpty(keyVaultName) || string.IsNullOrEmpty(secretName))
        {
            Console.WriteLine("Please set the AZURE_KEYVAULT_NAME and AZURE_KEYVAULT_SECRET_NAME environment variables.");
            return;
        }

        // Construct the Key Vault URI
        string keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";

        SecretClientOptions options = new SecretClientOptions()
        {
            Retry = {
                Delay= TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
        };

        // Create a SecretClient using DefaultAzureCredential
        var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential(), options);

        Console.Write("Input the value of your secret > ");
        var secretValue = Console.ReadLine();

        Console.Write($"Creating a secret in {keyVaultName} called '{secretName}' with the value '{secretValue}' ...");
        await client.SetSecretAsync(secretName, secretValue);
        Console.WriteLine(" done.");

        Console.WriteLine($"Retrieving your secret from {keyVaultName}.");
        var secret = await client.GetSecretAsync(secretName);
        Console.WriteLine($"Your secret is '{secret.Value.Value}'.");

        Console.Write($"Deleting your secret from {keyVaultName} ...");
        DeleteSecretOperation operation = await client.StartDeleteSecretAsync(secretName);
        // You only need to wait for completion if you want to purge or recover the secret.
        await operation.WaitForCompletionAsync();
        Console.WriteLine(" done.");

        Console.Write($"Purging your secret from {keyVaultName} ...");
        await client.PurgeDeletedSecretAsync(secretName);
        Console.WriteLine(" done.");

    }
}