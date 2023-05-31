using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var connFactory = new ConnectionFactory();
connFactory.HostName = "localhost";
connFactory.DispatchConsumersAsync = true;
var conn = connFactory.CreateConnection();
string exchangeName = "exchange1";
string eventName = "myEvent";
using var channel = conn.CreateModel();
string queueName = "queue1";
channel.ExchangeDeclare(exchange: exchangeName, type: "direct");
channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: eventName);

AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
consumer.Received += Consumer_Received;
channel.BasicConsume(queueName, autoAck: false, consumer: consumer);
Console.ReadKey();
async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
{
    try
    {
        var bytes = @event.Body.ToArray();
        string msg = Encoding.UTF8.GetString(bytes);
        Console.WriteLine(DateTime.Now + " 收到消息 " + msg);
        channel.BasicAck(@event.DeliveryTag, multiple: false);
        await Task.Delay(1000);
    }
    catch (Exception ex)
    {
        channel.BasicReject(@event.DeliveryTag, true);
        Console.WriteLine(ex.ToString());
    }
}