using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            int i = 0;
            var random = new Random();

            do
            {
                var timeout = random.Next(1000, 3000);
                Thread.Sleep(timeout);

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "dev-queue", autoDelete: false, durable: false, exclusive: false);
                        string message = $"This is new message from publisher with duration {timeout}";

                        var messageBody = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish("", "dev-queue", null, messageBody);

                        Console.WriteLine("Message is sent " + timeout);
                    }

                }

                i++;
            }

            while (i < 50);

        }
    }
}
