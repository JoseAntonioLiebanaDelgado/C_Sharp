using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverIP = "127.0.0.1";
            int serverPort = 50000;

            using (TcpClient client = new TcpClient())
            {
                client.Connect(serverIP, serverPort);
                Console.WriteLine("Conectado al servidor.");

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Datos recibidos: {message}");

                try
                {
                    ClasseJugador player = JsonSerializer.Deserialize<ClasseJugador>(message);
                    Console.WriteLine($"ID: {player.id_jugador}, Color: {player.color}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al deserializar: {ex.Message}");
                }
            }
        }
    }
}