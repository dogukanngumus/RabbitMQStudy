using System.Text;
using RabbitMQ.Client;

namespace Publisher;

class Program
{
    static void Main(string[] args)
    {
        string queueName = "hello-world";
      

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

        for (int i = 1; i <= 100; i++)
        {
            string message = $"Hello World {i} !";
            var messageBody = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(string.Empty, queueName, null,messageBody);
            Console.WriteLine($"Mesajınız gönderilmiştir: {message}!");
        }
        Console.ReadLine();
    }
}