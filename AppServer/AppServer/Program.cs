using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Inicializando servidor...");

            // Defininimos la IP y el puerto del servidor
            IPAddress MyIPAddress;
            MyIPAddress = IPAddress.Parse("127.0.0.1");
            int MyPort = 11000;

            // Creamos un nuevo TCP listener y empezamos a escuchar
            TcpListener Server = new TcpListener(MyIPAddress, MyPort);
            // Comenzamos a escuchar peticiones
            Server.Start();

            Console.WriteLine("Servidor abierto, esperando conexión...");

            TcpClient Client = Server.AcceptTcpClient();
            Console.WriteLine("Cliente conectado!");

            //Obtenemos el Stream de intercambio de datos
            NetworkStream MyNetworkStream = Client.GetStream();

            byte[] BufferLocal = new Byte[256];
            int BytesRebuts = MyNetworkStream.Read(BufferLocal, 0, BufferLocal.Length);
            String msg = Encoding.Unicode.GetString(BufferLocal, 0, BytesRebuts);
            Console.WriteLine("Mensaje recibido: " + msg);
            
            // Pasamos el mensaje a bytes
            String input = Console.ReadLine();
            byte[] msg2 = Encoding.Unicode.GetBytes(input);

            // Enviamos el mensaje
            MyNetworkStream.Write(msg2, 0, msg2.Length);

            Console.ReadLine();

            //Cerramos la conexión
            MyNetworkStream.Close();
            Client.Close();
        }
    }
}