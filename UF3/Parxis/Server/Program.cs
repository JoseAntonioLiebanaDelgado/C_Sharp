using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Server
{
    class Program
    {
        private static readonly IPAddress serverIP = IPAddress.Parse("127.0.0.1");
        private static readonly int serverPort = 50000;
        private static int idPlayer = 0;
        private static int connectedClients = 0;
        private static readonly int maxClients = 4;
        private static readonly List<string> colors = new List<string> { "Rojo", "Amarillo", "Verde", "Azul" };
        private static readonly object listLock = new object();

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(serverIP, serverPort);
            server.Start();
            Console.WriteLine("Servidor iniciado...");

            while (true)
            {
                if (connectedClients < maxClients)
                {
                    TcpClient client = server.AcceptTcpClient();
                    connectedClients++;
                    Console.WriteLine($"Cliente conectado. Total de clientes: {connectedClients}");
                    Thread clientThread = new Thread(ClientHandler);
                    clientThread.Start(client);
                }
                else
                {
                    Console.WriteLine("Máximo número de clientes alcanzado. Esperando que un cliente se desconecte...");
                    Thread.Sleep(1000);
                }
            }
        }

        static void ClientHandler(object clientObject)
        {
            TcpClient client = (TcpClient)clientObject;
            NetworkStream stream = client.GetStream();

            ClasseJugador player;
            lock (listLock)
            {
                idPlayer = (idPlayer % maxClients) + 1; 
                player = new ClasseJugador { id_jugador = idPlayer, color = colors[idPlayer - 1] };
            }

            string jsonString = JsonSerializer.Serialize(player);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            stream.Write(jsonBytes, 0, jsonBytes.Length);
            Console.WriteLine($"Enviando datos a cliente: {jsonString}");

            stream.Close();
            client.Close();

            lock (listLock)
            {
                connectedClients--;
                Console.WriteLine($"Cliente desconectado. Total de clientes: {connectedClients}");
            }
        }
    }
}
