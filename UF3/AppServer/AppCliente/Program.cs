using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Inicializando cliente...");

            // Deinimos el IP y el puerto del servidor
            IPAddress ServerIP;
            ServerIP = IPAddress.Parse("127.0.0.1");
            int MyPort = 11000;

            // Creamos un nuevo TCP client y nos conectamos al servidor
            TcpClient Client = new TcpClient();

            // Hacemos la conexión al servidor
            Client.Connect(ServerIP, MyPort);

            Console.WriteLine("Connected to server");

            //Obtenemos el Stream de intercambio de datos
            NetworkStream MyNetworkStream = Client.GetStream();

            // Pasamos el mensaje a bytes
            byte[] msg = Encoding.Unicode.GetBytes("Texto B para enviar al servidor");

            // Enviamos el mensaje
            MyNetworkStream.Write(msg, 0, msg.Length);

            byte[] BufferLocal = new Byte[256];
            int BytesRebuts = MyNetworkStream.Read(BufferLocal, 0, BufferLocal.Length);
            String msg2 = Encoding.Unicode.GetString(BufferLocal, 0, BytesRebuts);
            Console.WriteLine("Mensaje recibido: " + msg2);
            
            Console.ReadLine();

            // Cerramos la conexión
            MyNetworkStream.Close();
            Client.Close();
        }
    }
}