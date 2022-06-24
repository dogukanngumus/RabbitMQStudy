using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer;

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
        channel.BasicQos(0,1, false);
        var consumer = new EventingBasicConsumer(channel);
        channel.BasicConsume(queueName,false,consumer);
        
        consumer.Received += (object sender, BasicDeliverEventArgs e) =>
        {
            var messages = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine("Gelen mesaj : "+messages);
            channel.BasicAck(e.DeliveryTag, false);
        };

        Console.ReadLine();
    }
}