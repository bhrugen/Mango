using Azure.Messaging.ServiceBus;
using Mango.EmailAPI.Models.Dto;
using Newtonsoft.Json;
using System.Text;

namespace Mango.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IHostedService
    {
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor _emailCartProcessor;
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;

       

        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString")
                ?? throw new InvalidOperationException("Missing configuration value: ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue")
                ?? throw new InvalidOperationException("Missing configuration value: EmailShoppingCartQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync(cancellationToken);
        }

        private  Task ErrorHandler(ProcessErrorEventArgs args)
        {
            //TO DO: Log the error
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
                    // Handle null case
                    return;
                }
                
                //Send Email 
            }
            catch(Exception ex)
            {
                // Handle exception
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _emailCartProcessor.StopProcessingAsync(cancellationToken);
            await _emailCartProcessor.DisposeAsync();
        }
    }
}
