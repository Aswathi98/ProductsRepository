using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using DeliVeggieApplication.Interfaces;
using DeliVeggieApplication.Models;
using DeliVeggieApplication.Producer;

public class RabbitMqConsumer : BackgroundService
{
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly IProductService _productService;
    private readonly ILogger<RabbitMqConsumer> _logger;

    public RabbitMqConsumer(IConnection connection, IProductService productService, ILogger<RabbitMqConsumer> logger)
    {
        _channel = connection.CreateModel();
        _queueName = "product_queue";
        _productService = productService;
        _logger = logger;

        _channel.QueueDeclare(queue: _queueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var request = JsonSerializer.Deserialize<RequestModel>(message);

            if (request != null)
            {
                try
                {
                    // Fetch all products
                    var products = await _productService.GetAllProducts();

                    // Handle the response here, such as logging or sending a message to another queue
                    _logger.LogInformation($"Received request: {request.ProductId}, processed {products.Count} products.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing the RabbitMQ message.");
                }
            }
        };

        _channel.BasicConsume(queue: _queueName,
                              autoAck: true,
                              consumer: consumer);

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _channel.Close();
        _channel.Dispose();
        return base.StopAsync(cancellationToken);
    }
}
