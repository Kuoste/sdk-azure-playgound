
dotnet new console -n redis-cache
dotnet add package StackExchange.Redis

az group create --name "rg-redis" --location "northeurope"
az provider register --namespace Microsoft.Cache

az redis create --location northeurope -g rg-redis -n az204redis250403 --sku basic --vm-size c0

Creating takes ten minutes. See the pricing while waiting https://azure.microsoft.com/en-us/pricing/details/cache/#pricing


