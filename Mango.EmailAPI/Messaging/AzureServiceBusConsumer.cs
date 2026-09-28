using Azure.Messaging.ServiceBus;
using Mango.EmailAPI.Models.Dto;
using Mango.EmailAPI.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace Mango.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IHostedService
    {
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor _emailCartProcessor;
        private readonly IEmailService _emailService;   
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;

        private readonly ILogger<AzureServiceBusConsumer> _logger;


        public AzureServiceBusConsumer(IConfiguration configuration, ILogger<AzureServiceBusConsumer> logger, IEmailService emailService)
        {
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString")
                ?? throw new InvalidOperationException("Missing configuration value: ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue")
                ?? throw new InvalidOperationException("Missing configuration value: EmailShoppingCartQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false
            });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync(cancellationToken);
        }

        private  Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Error occurred while processing message from Service Bus.");
            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body =Encoding.UTF8.GetString(message.Body);

            try
            {
                CartDto? objMessage = JsonConvert.DeserializeObject<CartDto>(body);
                if (objMessage == null)
                {
                    _logger.LogError("Failed to deserialize message body to CartDto. Message body: {MessageBody}", body);
                    return;
                }
                
                //Send Email 
                _emailService.EmailCartAndLog(objMessage).GetAwaiter().GetResult();
                await args.CompleteMessageAsync(message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing message from Service Bus.");
                await args.DeadLetterMessageAsync(message, "DeserializationError", ex.Message);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _emailCartProcessor.StopProcessingAsync(cancellationToken);
            await _emailCartProcessor.DisposeAsync();
        }
    }
}
