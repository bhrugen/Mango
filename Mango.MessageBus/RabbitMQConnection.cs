using RabbitMQ.Client;

namespace Mango.MessageBus
{
    public static class RabbitMQConnection
    {
        public static ConnectionFactory CreateFactory() => new()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            AutomaticRecoveryEnabled = true
        };
    }
}
