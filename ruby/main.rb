require 'azure/storage/blob'

account_name = ENV['AZURE_SA_NAME']
account_key = ENV['AZURE_SA_KEY']

# puts "Account Name: #{account_name}"
# puts "Account Key: #{account_key}"

blob_service = Azure::Storage::Blob::BlobService.create(
    storage_account_name: account_name,
    storage_access_key: account_key
)

puts "Listing blobs..."
blob_service.list_containers.each do |container|
    puts "Container: #{container.name}"
    blob_service.list_blobs(container.name).each do |blob|
        puts "  Blob: #{blob.name}"
    end
end

puts "Creating a container..."
container_name = 'container-from-ruby'
# blob_service.create_container(container_name) 

puts "Uploading a blob..."
blob_name = 'myblob'
blob_service.create_block_blob(container_name, blob_name, 'Hello, World!')

puts "Downloading a blob..."
blob, content = blob_service.get_blob(container_name, blob_name)
puts "  Content: #{content}"



