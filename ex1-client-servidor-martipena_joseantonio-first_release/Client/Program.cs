using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        private static bool isRunning = true;
        static void Main(string[] args)
        {
            Console.WriteLine("Client: Starting client...");

            // Define the server IP address and port for server connection
            IPAddress ServerIP;
            ServerIP = IPAddress.Parse("127.0.0.1");
            int MyPort = 11000;

            // Create a new TCP client and connect to server
            TcpClient Client = new TcpClient();
            // Connect to server
            Client.Connect(ServerIP, MyPort);
            Console.WriteLine("Client: Connected to server");

            NetworkStream MyNetworkStream = Client.GetStream();
            Thread read = new Thread(Read);
            Thread write = new Thread(Write);
            read.Start(MyNetworkStream);
            write.Start(MyNetworkStream);
            read.Join();
            write.Join();
            MyNetworkStream.Close();
            Client.Close();
        }

        static void Read(Object nsClient)
        {
            NetworkStream NSClient = (NetworkStream)nsClient;
            try
            {
                while (isRunning)
                {
                    byte[] buffer = new byte[256];
                    int bytesRead = NSClient.Read(buffer, 0, buffer.Length);
                    string input = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Client: Message received: " + input);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Read thread exception: " + ex.Message);
                // Handle exception (e.g., log or clean up resources)
                isRunning = false;
            }
        }

        static void Write(Object nsClient)
        {
            NetworkStream NSClient = (NetworkStream)nsClient;
            try
            {
                while (isRunning)
                {
                    string message = Console.ReadLine();
                    if (message == "exit") // Example command to stop the client
                    {
                        isRunning = false;
                        break;
                    }
                    byte[] msg = Encoding.Unicode.GetBytes(message);
                    NSClient.Write(msg, 0, msg.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Write thread exception: " + ex.Message);
                // Handle exception (e.g., log or clean up resources)
                isRunning = false;
            }
        }
    }
}