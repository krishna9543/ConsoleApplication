using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.StorageAccountHandler
{
    interface IStorageAccountHandler
    {
        public Task DeleteBlobAsync(string blobName, string ContainerName);
    }
}
