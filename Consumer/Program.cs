using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            string message = string.Empty;

            do
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "dev-queue", autoDelete: false, durable: false, exclusive: false);
                    var consumer = new EventingBasicConsumer(channel);

                    Thread.Sleep(1000);

                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body;
                        message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine(message);
                    };



                    channel.BasicConsume("dev-queue", true, consumer);
                }
            } while (true);

        }
    }
}
