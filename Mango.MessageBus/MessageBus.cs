using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {

        private string connectionString = "Endpoint=sb://mangoweb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BCk8ly3QlYE+JmRoqPUY5cyDkY6Bfqz/9+ASbOLbZpQ=";

        public async Task PublishMessage(string queue_topic_Name, object message)
        {
            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(queue_topic_Name);

            var jsonMessage = JsonConvert.SerializeObject(message);

            ServiceBusMessage busMessage = new ServiceBusMessage(jsonMessage);

            await sender.SendMessageAsync(busMessage);
            
        }
    }
}
