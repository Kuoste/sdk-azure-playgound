from azure.storage.blob import BlobServiceClient
import os

# Create a BlobServiceClient
account_name = os.getenv('AZURE_SA_NAME')
account_key = os.getenv('AZURE_SA_KEY')
service = BlobServiceClient(account_url=f"https://{account_name}.blob.core.windows.net", credential=account_key)

continers = service.list_containers()
for container in continers:
    print(container.name)
    blobs = service.get_container_client(container.name).list_blobs()
    for blob in blobs:
        print(blob.name)