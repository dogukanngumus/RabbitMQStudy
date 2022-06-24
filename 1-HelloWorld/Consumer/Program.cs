using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer;

class Program
{
    static void Main(string[] args)
    {
        string queueName = "hello-world";
        string message = "Hello World !";

        //Step One
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri("amqp://admin:123456@localhost:5672")
        };
        IConnection connection =  connectionFactory.CreateConnection();
        
        //Step Two
        var channel = connection.CreateModel();
        channel.QueueDeclare(queueName, true, false, false);
        
        //Step Three
        var consumer = new EventingBasicConsumer(channel);
        channel.BasicConsume(queueName, true, consumer);

        consumer.Received += (sender, eventArgs) =>
        {
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            Console.WriteLine("Gelen Mesaj :" + message);
        };
        Console.ReadLine();
    }
    
    
}