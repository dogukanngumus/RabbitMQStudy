using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
        var queueName = "direct-queue-Warning";
        var consumer = new EventingBasicConsumer(channel);
        channel.BasicQos(0,1,false);
        channel.BasicConsume(queueName,false,consumer);
        consumer.Received += (sender, eventArgs) =>
        {
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            Thread.Sleep(1000);
            Console.WriteLine("Gelen mesaj:"+ message);
            channel.BasicAck(eventArgs.DeliveryTag, false);
        };
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