using Mango.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI.RabbitMQSender
{
    public class RabbitMQCartMessageSender : IRabbitMQCartMessageSender
    {
        public void SendMessage(BaseMessage baseMessage, string queueName)
        {
            throw new NotImplementedException();
        }
    }
}
