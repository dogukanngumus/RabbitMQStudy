using System.Text;
using RabbitMQ.Client;

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
        channel.ExchangeDeclare(exchangeName, ExchangeType.Headers, true);
        Dictionary<string, object> headers = new Dictionary<string, object>();
        headers.Add("format","pdf");
        headers.Add("shape","a4");

        var properties = channel.CreateBasicProperties();
        properties.Headers = headers;
        
        channel.BasicPublish(exchangeName, String.Empty, properties,Encoding.UTF8.GetBytes("test mesaj"));
        Console.WriteLine("Mesaj gönderilmiştir.");
        Console.ReadLine();
    }
}