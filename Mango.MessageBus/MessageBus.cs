using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        public async Task PublishMessage(string queueName, object message)
        {
            await using var connection = await RabbitMQConnection.CreateFactory().CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);
            await PublishAsync(channel, exchange: "", routingKey: queueName, message);
        }

        public async Task PublishToExchange(string exchangeName, object message)
        {
            await using var connection = await RabbitMQConnection.CreateFactory().CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout, durable: true);
            await PublishAsync(channel, exchange: exchangeName, routingKey: "", message);
        }

        private static async Task PublishAsync(IChannel channel, string exchange, string routingKey, object message)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var props = new BasicProperties { Persistent = true, ContentType = "application/json" };
            await channel.BasicPublishAsync(exchange, routingKey, mandatory: false, props, body);
        }
    }
}
