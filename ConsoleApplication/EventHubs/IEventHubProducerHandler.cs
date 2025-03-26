using Azure.Messaging.EventHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.EventHubs
{
    internal interface IEventHubProducerHandler
    {
        public Task sendAsync(EventData eventData, string eventhubConnectionString, string eventhubNamespace, string eventHubName);

        public Task ReceiveEventsAsync(string consumerGroup, string eventhubConnectionString, string eventhubNamespace, string eventHubName, string storageConnectionString, string blobContainerName);
    }
}
