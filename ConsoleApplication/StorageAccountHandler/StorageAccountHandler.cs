using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Storage;
using Azure.Identity;
using Azure.Storage.Blobs.Models;

namespace ConsoleApplication.StorageAccountHandler
{
    class StorageAccountHandler: IStorageAccountHandler
    {
        BlobServiceClient _blobServiceClient;

        public StorageAccountHandler(string StorageAccountName,  string Accesskey, int maxdelayMS, int Maxretry)
        {
            BlobClientOptions BlobClientOptions = new BlobClientOptions();
            BlobClientOptions.Retry.Mode = RetryMode.Exponential;
            BlobClientOptions.Retry.MaxRetries = Maxretry;
            BlobClientOptions.Retry.MaxDelay = TimeSpan.FromSeconds(maxdelayMS);
            string uriString = string.Concat("Https://" + StorageAccountName + ".blob.core.windows.net");
            _blobServiceClient = new BlobServiceClient(new Uri(uriString), new StorageSharedKeyCredential(StorageAccountName, Accesskey), BlobClientOptions);
        }
        
        public StorageAccountHandler(string StorageAccountName, int maxdelayMS, int Maxretry)
        {
            BlobClientOptions BlobClientOptions = new BlobClientOptions();
            BlobClientOptions.Retry.Mode = RetryMode.Exponential;
            BlobClientOptions.Retry.MaxRetries = Maxretry;
            BlobClientOptions.Retry.MaxDelay = TimeSpan.FromSeconds(maxdelayMS);
            string uriString = string.Concat("Https://" + StorageAccountName + ".blob.core.windows.net");
            _blobServiceClient = new BlobServiceClient(new Uri(uriString), new DefaultAzureCredential(), BlobClientOptions);
        }

        public StorageAccountHandler(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }


        public async Task DeleteBlobAsync(string blobName, string ContainerName)
        {
            BlobContainerClient _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            var response = await _blobContainerClient.DeleteBlobAsync(blobName);
            Console.WriteLine("Deleted.......{0}",response);
        }
        public async Task ReadBlobAsync(string blobName, string ContainerName)
        {
            BlobContainerClient _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadAsync();
            using (StreamReader reader = new StreamReader(response.Value.Content, Encoding.UTF8))
            {
                string content = await reader.ReadToEndAsync();
                Console.WriteLine("Blob Content: ");
                Console.WriteLine(content);
            }
        }
    }
}



