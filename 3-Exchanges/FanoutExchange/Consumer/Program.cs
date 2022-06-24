using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
        var queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(queueName,exchangeName, "", null);
        channel.BasicQos(0,1,false);
        
        var consumer = new EventingBasicConsumer(channel);
        channel.BasicConsume(queueName, false,consumer);
        
        //Step Three
        consumer.Received += (object sender, BasicDeliverEventArgs e) =>
        {
            var messages = Encoding.UTF8.GetString(e.Body.ToArray());
            Thread.Sleep(1000);
            Console.WriteLine("Gelen mesaj : "+messages);
            channel.BasicAck(e.DeliveryTag, false);
        };
        
        Console.ReadLine();
    }
}