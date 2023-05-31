using RabbitMQ.Client;
using System.Text;

string exchangeName = "exchange1";
string eventName = "myEvent";
var connFactory = new ConnectionFactory();
connFactory.HostName = "localhost";
connFactory.DispatchConsumersAsync = true;
var conn = connFactory.CreateConnection();
while (true)
{
    string msg = DateTime.Now.TimeOfDay.ToString();
    using var channel = conn.CreateModel();
    var prop = channel.CreateBasicProperties();
    prop.DeliveryMode = 2;
    channel.ExchangeDeclare(exchange: exchangeName, type: "direct");
    byte[] bytes = Encoding.UTF8.GetBytes(msg);
    channel.BasicPublish(exchange:exchangeName, routingKey: eventName, mandatory: true, basicProperties: prop, body: bytes);
    Console.WriteLine("ok " + DateTime.Now);
    Thread.Sleep(1000);
}
