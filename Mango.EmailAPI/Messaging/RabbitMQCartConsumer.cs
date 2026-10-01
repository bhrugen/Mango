using Mango.EmailAPI.Models.Dto;
using Mango.EmailAPI.Services.IServices;
using Mango.MessageBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.EmailAPI.Messaging
{
    public class RabbitMQCartConsumer : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQCartConsumer> _logger;
        private readonly IEmailService _emailService;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMQCartConsumer(IConfiguration configuration, ILogger<RabbitMQCartConsumer> logger, IEmailService emailService)
        {
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // 1. Connect to RabbitMQ and open a channel
            _connection = await RabbitMQConnection.CreateFactory().CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            // 2. Make sure the queue exists (declaring is safe to repeat)
            var queueName = _configuration["TopicAndQueueNames:EmailShoppingCartQueue"]!;
            await _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);

            // 3. Start listening; the handler below runs for every message
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var message = JsonConvert.DeserializeObject<CartDto>(json);

                    await _emailService.EmailCartAndLog(message!);

                    // Success: tell RabbitMQ to remove the message from the queue
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from queue {Queue}", queueName);
                    // Failure: reject without requeue so a bad message doesn't loop forever
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };

            // autoAck: false means we acknowledge manually after the work is done
            await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null) await _channel.DisposeAsync();
            if (_connection != null) await _connection.DisposeAsync();
        }
    }
}
