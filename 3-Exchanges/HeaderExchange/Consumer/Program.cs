using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Publisher;

class Program
{
    //Header Exchange Key Value Olarak Çalışır.

    static void Main(string[] args)
    {
        string exchangeName = "header-exchange";
      

        //Step One
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri("amqp://admin:123456@localhost:5672")
        };
        IConnection connection =  connectionFactory.CreateConnection();
        
        //Step Two
        var channel = connection.CreateModel();
        channel.BasicQos(0,1,false);
        var consumer = new EventingBasicConsumer(channel);
        var queueName = channel.QueueDeclare().QueueName;
        Dictionary<string, object> headers = new Dictionary<string, object>();
        headers.Add("format","pdf");
        headers.Add("shape","a4");
        headers.Add("x-match","all");
        channel.QueueBind(queueName, exchangeName,string.Empty,headers);
        channel.BasicConsume(queueName,false,consumer);
        consumer.Received += (sender, eventArgs) =>
        {
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            Thread.Sleep(1000);
            Console.WriteLine("Gelen mesaj:"+message);
            channel.BasicAck(eventArgs.DeliveryTag,false);
        };
        Console.ReadLine();
        
    }
}