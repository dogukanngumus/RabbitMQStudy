using System.Text;
using RabbitMQ.Client;

namespace Publisher;

class Program
{
    //Fanout Exchange Mesajları Kuyruklara Broadcast Olarak Dağıtır.
    //Kuyruğu consumerların oluşturması daha mantıklıdır.
    
    static void Main(string[] args)
    {
        string exchangeName = "hello-world";
      

        //Step One
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri("amqp://admin:123456@localhost:5672")
        };
        IConnection connection =  connectionFactory.CreateConnection();
        
        //Step Two
        var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);
       
        //Step Three
        for (int i = 1; i <= 100; i++)
        {
            string message = $"Hello World {i} !";
            var messageBody = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchangeName, "", null,messageBody);
            Console.WriteLine($"Mesajınız gönderilmiştir: {message}!");
        }
        Console.ReadLine();
    }
}