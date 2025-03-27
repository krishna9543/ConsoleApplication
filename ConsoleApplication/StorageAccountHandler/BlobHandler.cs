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
using System.ComponentModel.DataAnnotations;

namespace ConsoleApplication.StorageAccountHandler
{
    class BlobHandler : IBlobHandler
    {
        BlobServiceClient _blobServiceClient;

        public BlobHandler(string StorageAccountName, string Accesskey, int maxdelayMS, int Maxretry)
        {
            BlobClientOptions BlobClientOptions = new BlobClientOptions();
            BlobClientOptions.Retry.Mode = RetryMode.Exponential;
            BlobClientOptions.Retry.MaxRetries = Maxretry;
            BlobClientOptions.Retry.MaxDelay = TimeSpan.FromSeconds(maxdelayMS);
            string uriString = string.Concat("Https://" + StorageAccountName + ".blob.core.windows.net");
            _blobServiceClient = new BlobServiceClient(new Uri(uriString), new StorageSharedKeyCredential(StorageAccountName, Accesskey), BlobClientOptions);
        }

        public BlobHandler(string StorageAccountName, int maxdelayMS, int Maxretry)
        {
            BlobClientOptions BlobClientOptions = new BlobClientOptions();
            BlobClientOptions.Retry.Mode = RetryMode.Exponential;
            BlobClientOptions.Retry.MaxRetries = Maxretry;
            BlobClientOptions.Retry.MaxDelay = TimeSpan.FromSeconds(maxdelayMS);
            string uriString = string.Concat("Https://" + StorageAccountName + ".blob.core.windows.net");
            _blobServiceClient = new BlobServiceClient(new Uri(uriString), new DefaultAzureCredential(), BlobClientOptions);
        }

        public BlobHandler(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }


        public async Task DeleteBlobAsync(string blobName, string ContainerName)
        {
            BlobContainerClient _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            var response = await _blobContainerClient.DeleteBlobAsync(blobName);
            Console.WriteLine("Deleted.......{0}", response);
        }
        public async Task<byte[]> ReadBlobAsync(string blobName, string ContainerName)
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

            var response1 = await _blobServiceClient.GetBlobContainerClient(ContainerName).GetBlobClient(blobName).DownloadStreamingAsync();
            return readFilly((Stream)response1.Value.Content);

        }

        private static byte[] readFilly(Stream stream)  // need learn
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            byte[] array = new byte[4042];
            using MemoryStream memoryStream = new MemoryStream();
            int count;
            while((count = binaryReader.Read(array, 0, array.Length)) > 0)
            {
                memoryStream.Write(array, 0, count);
            }
            memoryStream.FlushAsync();
            return array;
        }

        public async Task GetAllContainerNames()
        {
            await foreach (var container in _blobServiceClient.GetBlobContainersAsync())
            {
                Console.WriteLine(container.Name);
            }
        }
        
        public async Task GetAllBlobAsync(string ContainerName)
        {
            var _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            await foreach (var names in _blobContainerClient.GetBlobsAsync())
            {
                Console.WriteLine(names.Name);
            }
        }

        public async void samplemethod()
        {
            BlobContainerClient blobContainerClient  = _blobServiceClient.CreateBlobContainer("samplecontainer");

            BlobClient blobClient = blobContainerClient.GetBlobClient("samplecontainer");

        }

    }
}



