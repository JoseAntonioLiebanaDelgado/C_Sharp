// Importaciones necesarias para el funcionamiento del servidor TCP.
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

// Espacio de nombres Server para organizar las clases relacionadas con el servidor.
namespace Server
{
    // Clase principal del servidor.
    class Program
    {
        // Configuraciones del servidor como IP, puerto y máximos clientes conectados.
        private static readonly IPAddress serverIP = IPAddress.Parse("127.0.0.1");
        private static readonly int serverPort = 50000;
        private static int idPlayer = 0; // ID para asignar a cada jugador.
        private static int connectedClients = 0; // Contador de clientes conectados.
        private static readonly int maxClients = 4; // Máximo número de clientes simultáneos.
        // Lista predefinida de colores para asignar a los jugadores.
        private static readonly List<string> colors = new List<string> { "Rojo", "Amarillo", "Verde", "Azul" };
        // Objeto para bloquear secciones críticas y garantizar la sincronización entre hilos.
        private static readonly object listLock = new object();

        // Punto de entrada del programa.
        static void Main(string[] args)
        {
            // Inicia el servidor TCP.
            TcpListener server = new TcpListener(serverIP, serverPort);
            server.Start();
            Console.WriteLine("Servidor iniciado...");

            // Bucle infinito para aceptar clientes.
            while (true)
            {
                // Verifica si el número de clientes conectados es menor que el máximo permitido.
                if (connectedClients < maxClients)
                {
                    // Acepta la conexión de un cliente.
                    TcpClient client = server.AcceptTcpClient();
                    connectedClients++; // Incrementa el contador de clientes conectados.
                    Console.WriteLine($"Cliente conectado. Total de clientes: {connectedClients}");
                    // Inicia un nuevo hilo para manejar al cliente.
                    Thread clientThread = new Thread(ClientHandler);
                    clientThread.Start(client);
                }
                else
                {
                    // Si se alcanza el máximo de clientes, espera.
                    Console.WriteLine("Máximo número de clientes alcanzado. Esperando que un cliente se desconecte...");
                    Thread.Sleep(1000); // Pausa el bucle principal por un segundo.
                }
            }
        }

        // Método para manejar la conexión con un cliente.
        static void ClientHandler(object clientObject)
        {
            // Casting del objeto a TcpClient y obtención del flujo de red.
            TcpClient client = (TcpClient)clientObject;
            NetworkStream stream = client.GetStream();

            ClasseJugador player;
            // Sección crítica para asignar ID y color al jugador de forma segura.
            lock (listLock)
            {
                idPlayer = (idPlayer % maxClients) + 1;
                player = new ClasseJugador { id_jugador = idPlayer, color = colors[idPlayer - 1] };
            }

            // Serializa la información del jugador a JSON y la envía al cliente.
            string jsonString = JsonSerializer.Serialize(player);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            stream.Write(jsonBytes, 0, jsonBytes.Length);
            Console.WriteLine($"Enviando datos a cliente: {jsonString}");

            // Cierra la conexión con el cliente.
            stream.Close();
            client.Close();

            // Actualiza el contador de clientes conectados de forma segura.
            lock (listLock)
            {
                connectedClients--;
                Console.WriteLine($"Cliente desconectado. Total de clientes: {connectedClients}");
            }
        }
    }
}
