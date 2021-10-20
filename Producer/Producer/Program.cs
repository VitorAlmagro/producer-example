using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            int messageCount = 1000000;
            string connectionString = "tcp://*:12345";

            int sendedMessages = 0;

            Random rand = new Random(50);
            using (var pubSocket = new PublisherSocket())
            {
                Console.WriteLine("Publisher socket binding...");
                pubSocket.Options.SendHighWatermark = 1000;
                pubSocket.Bind(connectionString);

                Console.WriteLine($"Sending {messageCount} messages");

                for (var i = 0; i < messageCount; i++)
                {
                    var randomizedTopic = rand.NextDouble();
                    if (randomizedTopic > 0.5)
                    {
                        var msg = "TopicA msg-" + i;
                        Console.WriteLine("Sending message : {0}", msg);
                        pubSocket.SendMoreFrame("TopicA").SendFrame(msg);
                    }
                    else
                    {
                        var msg = "TopicB msg-" + i;
                        Console.WriteLine("Sending message : {0}", msg);
                        pubSocket.SendMoreFrame("TopicB").SendFrame(msg);
                    }

                    sendedMessages++;
                    //Thread.Sleep(500);
                }

                Console.WriteLine($"Expected messages: {messageCount} | Sended Messages {sendedMessages}");

                Console.WriteLine("Press any key to close the application");
                Console.ReadKey();
            }
        }
    }
}
