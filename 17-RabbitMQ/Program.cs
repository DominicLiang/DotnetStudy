using RabbitMQ.Client;
using System.Text;

var connFactory = new ConnectionFactory();
connFactory.HostName = "localhost";
connFactory.DispatchConsumersAsync = true;
var conn = connFactory.CreateConnection();
string exchangeName = "exchange1";
while (true)
{
    using var channel = conn.CreateModel();
    var prop = channel.CreateBasicProperties();
    prop.DeliveryMode = 2;
    channel.ExchangeDeclare(exchangeName, "direct");
    byte[] bytes = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
    channel.BasicPublish(exchangeName, routingKey: "Key1", mandatory: true, basicProperties: prop, body: bytes);
    Console.WriteLine("ok " + DateTime.Now);
    Thread.Sleep(1000);
}
