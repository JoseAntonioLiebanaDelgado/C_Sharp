using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Imprimir en consola que el servidor está iniciando
            Console.WriteLine("Server: Starting server...");

            // Definir la dirección IP y el puerto para el servidor
            IPAddress myIPAddress = IPAddress.Parse("127.0.0.1");
            int myPort = 11000;

            // Crear un nuevo escuchador TCP (TcpListener) y comenzar a escuchar en la IP y puerto definidos
            TcpListener server = new TcpListener(myIPAddress, myPort);
            server.Start(); // Iniciar el escuchador

            // Imprimir en consola que el servidor está esperando conexiones
            Console.WriteLine("Server: Server Open, Waiting for a connection...");

            try
            {
                // Bucle infinito para aceptar conexiones de clientes de manera continua
                while (true)
                {
                    // Aceptar un cliente que intenta conectarse
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Server: Client connected!");

                    // Por cada cliente conectado, se crea un nuevo hilo (Thread) que manejará la comunicación con ese cliente
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.Start(client); // Iniciar el hilo con el cliente como parámetro
                }
            }
            catch (Exception e)
            {
                // Capturar y manejar cualquier excepción que pueda ocurrir en el servidor
                Console.WriteLine("Server: An exception occurred: " + e.Message);
                server.Stop(); // Detener el servidor en caso de una excepción
            }
        }

        // Método que maneja la comunicación con el cliente
        private static void HandleClient(object obj)
        {
            // Convertir el objeto recibido a TcpClient
            TcpClient client = (TcpClient)obj;
            try
            {
                // Obtener el flujo de red asociado con el cliente para enviar y recibir datos
                NetworkStream stream = client.GetStream();

                // Buffer para almacenar los datos recibidos del cliente
                byte[] buffer = new byte[256];

                // Bucle para leer los datos del cliente continuamente
                while (true)
                {
                    // Leer los datos del flujo de red y almacenar el número de bytes leídos
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Si no se leen bytes, el cliente se ha desconectado

                    // Convertir los bytes recibidos a un string
                    string message = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Server: Message received: " + message);

                    // Invertir el mensaje y convertirlo de nuevo a un array de bytes
                    string reversedMessage = ReverseString(message);
                    byte[] response = Encoding.Unicode.GetBytes(reversedMessage);

                    // Enviar la respuesta al cliente
                    stream.Write(response, 0, response.Length);
                }
            }
            catch (Exception e)
            {
                // Capturar y manejar cualquier excepción específica de este cliente
                Console.WriteLine("Server: An exception occurred with a client: " + e.Message);
            }
            finally
            {
                // Cerrar la conexión con el cliente correctamente
                client.Close();
            }
        }

        // Método para invertir un string
        private static string ReverseString(string s)
        {
            // Convertir el string a un array de caracteres, invertirlo y convertirlo de nuevo a string
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
