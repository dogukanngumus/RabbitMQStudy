using System.Text;
using RabbitMQ.Client;

namespace Publisher;

class Program
{
    //Direct Exchange Mesajları Kuyruklara Route Key İle Anahtarlayarak Gönderir.

    static void Main(string[] args)
    {
        string exchangeName = "logs-direct";

        //Step One
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri("amqp://admin:123456@localhost:5672")
        };
        IConnection connection =  connectionFactory.CreateConnection();
        
        //Step Two
        var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);
        
        Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
        {
            string routeKey = $"route-{x}";
            string queueName = $"direct-queue-{x}";
            channel.QueueDeclare(queueName, true, false, false);
            channel.QueueBind(queueName, exchangeName, routeKey,null);
        });
        
        //Step Three
        for (int i = 1; i <= 100; i++)
        {
            LogNames log = (LogNames)new Random().Next(1, 4);
            string message = $"log type: {log} ";
            var messageBody = Encoding.UTF8.GetBytes(message);
            string routeKey = $"route-{log}";
            channel.BasicPublish(exchangeName, routeKey, null,messageBody);
            Console.WriteLine($"Log gönderilmiştir: {message}!");
        }
        Console.ReadLine();
    }
}

enum LogNames
{
    Critical = 1,
    Error = 2,
    Warning = 3,
    Info = 4
}