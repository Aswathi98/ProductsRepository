using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using DeliVeggieApplication.Producer;

public class RabbitMqProducer 
{
    private readonly IModel _channel;
    private readonly string _exchangeName;

    public RabbitMqProducer(IConnection connection)
    {
        _channel = connection.CreateModel();
        _exchangeName = "product_exchange";

        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);
    }

    public void PublishRequest(string routingKey, object message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _channel.BasicPublish(exchange: _exchangeName,
                              routingKey: routingKey,
                              basicProperties: null,
                              body: body);
    }
}

