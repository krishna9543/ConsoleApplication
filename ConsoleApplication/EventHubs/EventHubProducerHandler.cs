namespace ConsoleApplication.EventHubs
{
    using Azure.Identity;
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Consumer;
    using Azure.Messaging.EventHubs.Producer;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="EventHubProducerHandler" />
    /// </summary>
    internal class EventHubProducerHandler : IEventHubProducerHandler
    {
        /// <summary>
        /// The sendAsync
        /// </summary>
        /// <param name="eventData">The eventData<see cref="EventData"/></param>
        /// <param name="eventhubConnectionString">The eventhubConnectionString<see cref="string"/></param>
        /// <param name="eventhubNamespace">The eventhubNamespace<see cref="string"/></param>
        /// <param name="eventHubName">The eventHubName<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task sendAsync(EventData eventData, string eventhubConnectionString, string eventhubNamespace, string eventHubName)
        {
            EventHubConnectionOptions eventHubConnectionOptions = new()
            {
                TransportType = EventHubsTransportType.AmqpWebSockets
            };
            EventHubProducerClientOptions producerOptions = new EventHubProducerClientOptions();
            producerOptions.RetryOptions = new EventHubsRetryOptions()
            {
                Mode = EventHubsRetryMode.Exponential,
                MaximumRetries = 5,
                Delay = TimeSpan.FromSeconds(100),
                MaximumDelay = TimeSpan.FromSeconds(100)
            };
            producerOptions.ConnectionOptions = new EventHubConnectionOptions()
            {
                TransportType = EventHubsTransportType.AmqpWebSockets
            };

            List<EventData> Data = new List<EventData>();

            Data.Add(eventData);

            if (!string.IsNullOrEmpty(eventhubConnectionString))
            {
                string ConnectionString = string.Concat(eventhubConnectionString + ";EntityPath=" + eventHubName);
                var _newConnection = new EventHubConnection(ConnectionString, eventHubConnectionOptions);
                var _Client = new EventHubProducerClient(_newConnection, producerOptions);
                await _Client.SendAsync(Data);
                Console.WriteLine("Events published using Connection String");
            }
            else
            {
                EventHubProducerClient _Connection = new EventHubProducerClient(eventhubNamespace, eventHubName, new DefaultAzureCredential(), producerOptions);
                await _Connection.SendAsync(Data);
                Console.WriteLine("Events published using Default Azure Credential");
            }
        }

        /// <summary>
        /// The ReceiveEventsAsync
        /// </summary>
        /// <param name="consumerGroup">The consumerGroup<see cref="string"/></param>
        /// <param name="eventhubConnectionString">The eventhubConnectionString<see cref="string"/></param>
        /// <param name="eventhubNamespace">The eventhubNamespace<see cref="string"/></param>
        /// <param name="eventHubName">The eventHubName<see cref="string"/></param>
        /// <param name="storageConnectionString">The storageConnectionString<see cref="string"/></param>
        /// <param name="blobContainerName">The blobContainerName<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task ReceiveEventsAsync(string consumerGroup, string eventhubConnectionString, string eventhubNamespace, string eventHubName, string storageConnectionString, string blobContainerName)
        {
            EventHubConnectionOptions eventHubConnectionOptions = new()
            {
                TransportType = EventHubsTransportType.AmqpWebSockets
            };
            string ConnectionString = string.Concat(eventhubConnectionString + ";EntityPath=" + eventHubName);
            //Connection
            EventHubConnection _newConnection = new EventHubConnection(ConnectionString, eventHubConnectionOptions);

            //clientoptions
            EventHubConsumerClientOptions consumerOptions = new EventHubConsumerClientOptions();
            consumerOptions.ConnectionOptions = new EventHubConnectionOptions()
            {
                TransportType = EventHubsTransportType.AmqpWebSockets
            };
            consumerOptions.RetryOptions = new EventHubsRetryOptions()
            {
                Mode = EventHubsRetryMode.Exponential,
                MaximumRetries = 5,
                Delay = TimeSpan.FromSeconds(100),
                MaximumDelay = TimeSpan.FromSeconds(100)
            };

            if (!string.IsNullOrEmpty(ConnectionString))
            {

                await using (var consumerClient = new EventHubConsumerClient(consumerGroup, _newConnection, consumerOptions))
                {
                    var properties = await consumerClient.GetEventHubPropertiesAsync();
                    Console.WriteLine(@"connection {0}", properties.Name);

                    try
                    {
                        // Read events continuously from all partitions
                        await foreach (PartitionEvent partitionEvent in consumerClient.ReadEventsAsync())
                        {
                            string partitionId = partitionEvent.Partition.PartitionId;
                            string eventBody = partitionEvent.Data.EventBody.ToString();

                            Console.WriteLine($"Partition: {partitionId} | Event: {eventBody}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                }

            }
            else
            {
                EventHubConsumerClient consumerClient = new EventHubConsumerClient(consumerGroup, eventhubNamespace, eventHubName, new DefaultAzureCredential());
                await foreach (var i in consumerClient.ReadEventsAsync())
                {
                    Console.WriteLine(i.Data.EventBody);
                }
            }
        }
    }
}
