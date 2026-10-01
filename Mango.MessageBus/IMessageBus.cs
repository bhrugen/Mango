namespace Mango.MessageBus
{
    public interface IMessageBus
    {
        /// <summary>Publishes to a durable queue (point-to-point).</summary>
        Task PublishMessage(string queueName, object message);

        /// <summary>Publishes to a fanout exchange; every queue bound to it receives a copy (pub/sub).</summary>
        Task PublishToExchange(string exchangeName, object message);
    }
}
