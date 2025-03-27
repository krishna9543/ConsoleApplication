namespace ConsoleApplication.StorageAccountHandler
{
    using Azure.Core;
    using Azure.Identity;
    using Azure.Storage;
    using Azure.Storage.Blobs;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="BlobHandler" />
    /// </summary>
    internal class BlobHandler : IBlobHandler
    {
        /// <summary>
        /// Defines the _blobServiceClient
        /// </summary>
        internal BlobServiceClient _blobServiceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobHandler"/> class.
        /// </summary>
        /// <param name="StorageAccountName">The StorageAccountName<see cref="string"/></param>
        /// <param name="Accesskey">The Accesskey<see cref="string"/></param>
        /// <param name="maxdelayMS">The maxdelayMS<see cref="int"/></param>
        /// <param name="Maxretry">The Maxretry<see cref="int"/></param>
        public BlobHandler(string StorageAccountName, string Accesskey, int maxdelayMS, int Maxretry)
        {
            BlobClientOptions BlobClientOptions = new BlobClientOptions();
            BlobClientOptions.Retry.Mode = RetryMode.Exponential;
            BlobClientOptions.Retry.MaxRetries = Maxretry;
            BlobClientOptions.Retry.MaxDelay = TimeSpan.FromSeconds(maxdelayMS);
            string uriString = string.Concat("Https://" + StorageAccountName + ".blob.core.windows.net");
            _blobServiceClient = new BlobServiceClient(new Uri(uriString), new StorageSharedKeyCredential(StorageAccountName, Accesskey), BlobClientOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobHandler"/> class.
        /// </summary>
        /// <param name="StorageAccountName">The StorageAccountName<see cref="string"/></param>
        /// <param name="maxdelayMS">The maxdelayMS<see cref="int"/></param>
        /// <param name="Maxretry">The Maxretry<see cref="int"/></param>
        public BlobHandler(string StorageAccountName, int maxdelayMS, int Maxretry)
        {
            BlobClientOptions BlobClientOptions = new BlobClientOptions();
            BlobClientOptions.Retry.Mode = RetryMode.Exponential;
            BlobClientOptions.Retry.MaxRetries = Maxretry;
            BlobClientOptions.Retry.MaxDelay = TimeSpan.FromSeconds(maxdelayMS);
            string uriString = string.Concat("Https://" + StorageAccountName + ".blob.core.windows.net");
            _blobServiceClient = new BlobServiceClient(new Uri(uriString), new DefaultAzureCredential(), BlobClientOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobHandler"/> class.
        /// </summary>
        /// <param name="blobServiceClient">The blobServiceClient<see cref="BlobServiceClient"/></param>
        public BlobHandler(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        /// <summary>
        /// The DeleteBlobAsync
        /// </summary>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <param name="ContainerName">The ContainerName<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task DeleteBlobAsync(string blobName, string ContainerName)
        {
            BlobContainerClient _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            var response = await _blobContainerClient.DeleteBlobAsync(blobName);
            Console.WriteLine("Deleted.......{0}", response);
        }

        /// <summary>
        /// The ReadBlobAsync
        /// </summary>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <param name="ContainerName">The ContainerName<see cref="string"/></param>
        /// <returns>The <see cref="Task{byte[]}"/></returns>
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

        /// <summary>
        /// The readFilly
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/></param>
        /// <returns>The <see cref="byte[]"/></returns>
        private static byte[] readFilly(Stream stream)  // need learn
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            byte[] array = new byte[4042];
            using MemoryStream memoryStream = new MemoryStream();
            int count;
            while ((count = binaryReader.Read(array, 0, array.Length)) > 0)
            {
                memoryStream.Write(array, 0, count);
            }
            memoryStream.FlushAsync();
            return array;
        }

        /// <summary>
        /// The GetAllContainerNames
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task GetAllContainerNames()
        {
            await foreach (var container in _blobServiceClient.GetBlobContainersAsync())
            {
                Console.WriteLine(container.Name);
            }
        }

        /// <summary>
        /// The GetAllBlobAsync
        /// </summary>
        /// <param name="ContainerName">The ContainerName<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task GetAllBlobAsync(string ContainerName)
        {
            var _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            await foreach (var names in _blobContainerClient.GetBlobsAsync())
            {
                Console.WriteLine(names.Name);
            }
        }

        /// <summary>
        /// The samplemethod
        /// </summary>
        public async void samplemethod()
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.CreateBlobContainer("samplecontainer");

            BlobClient blobClient = blobContainerClient.GetBlobClient("samplecontainer");
        }
    }
}
