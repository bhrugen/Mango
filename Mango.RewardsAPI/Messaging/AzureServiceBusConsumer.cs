using Azure.Messaging.ServiceBus;
using Mango.RewardsAPI.Services.IServices;
using Mango.Web.Models;
using Newtonsoft.Json;
using System.Text;

namespace Mango.RewardsAPI.Messaging
{
    public class AzureServiceBusConsumer : IHostedService
    {
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor _rewardsProcessor;
        private readonly IRewardService _rewardService;   
        private readonly string serviceBusConnectionString;
        private readonly string orderCreatedTopic;
        private readonly string orderCreatedRewardSubscription;

        private readonly ILogger<AzureServiceBusConsumer> _logger;


        public AzureServiceBusConsumer(IConfiguration configuration, ILogger<AzureServiceBusConsumer> logger, IRewardService rewardService)
        {
            _configuration = configuration;
            _logger = logger;
            _rewardService = rewardService;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString")
                ?? throw new InvalidOperationException("Missing configuration value: ServiceBusConnectionString");
            orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic")
               ?? throw new InvalidOperationException("Missing configuration value: OrderCreatedTopic");
            orderCreatedRewardSubscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedRewardsSubscription")
                ?? throw new InvalidOperationException("Missing configuration value: OrderCreatedRewardsSubscription");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _rewardsProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedRewardSubscription, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false
            });
           
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _rewardsProcessor.ProcessMessageAsync += OnNewOrderRewardsRequestReceived;
            _rewardsProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardsProcessor.StartProcessingAsync(cancellationToken);
        }

        private async Task OnNewOrderRewardsRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            try
            {
                OrderHeaderDto? orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(body);
                if (orderHeader == null)
                {
                    _logger.LogError("Failed to deserialize message body to OrderHeaderDto. Message body: {MessageBody}", body);
                    return;
                }

               
                await _rewardService.UpdateRewards(orderHeader);
                await args.CompleteMessageAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing message from Service Bus.");
                await args.DeadLetterMessageAsync(message, "DeserializationError", ex.Message);
            }
        }

        private  Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Error occurred while processing message from Service Bus.");
            return Task.CompletedTask;
        }

      

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _rewardsProcessor.StopProcessingAsync(cancellationToken);
            await _rewardsProcessor.DisposeAsync();
        }
    }
}
