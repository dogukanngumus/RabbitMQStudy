using System.Text;
using RabbitMQ.Client;

namespace Publisher;

class Program
{
    //Topic Exchange Mesajları Kuyruklara Route Key İle Anahtarlayarak Gönderir. Route Keyler Daha Dinamiktir

    static void Main(string[] args)
    {
        string exchangeName = "logs-topic";

        //Step One
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri("amqp://admin:123456@localhost:5672")
        };
        IConnection connection =  connectionFactory.CreateConnection();
        
        //Step Two
        var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);
        
        
        //Step Three
        for (int i = 1; i <= 100; i++)
        {
            Random rnd = new Random();
            LogNames log1 = (LogNames)rnd.Next(1, 5);
            LogNames log2 = (LogNames)rnd.Next(1, 5);
            LogNames log3 = (LogNames)rnd.Next(1, 5);
            string routeKey = $"{log1}-{log2}-{log3}";
            string message = $"log type: {log1}-{log2}-{log3} ";
            var messageBody = Encoding.UTF8.GetBytes(message);
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