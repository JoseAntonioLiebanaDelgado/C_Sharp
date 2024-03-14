


/*Este código configura un servidor TCP que escucha las conexiones entrantes en una dirección IP y puerto específicos (127.0.0.1 en el puerto 11000).
 El servidor puede manejar múltiples clientes simultáneamente gracias al uso de hilos. Para cada cliente que se conecta, 
 el servidor inicia un nuevo hilo que maneja la comunicación con ese cliente. El servidor lee los mensajes enviados por el cliente, los procesa 
 (en este caso, invirtiendo el contenido del mensaje), y luego envía una respuesta al cliente.

 El servidor se ejecuta en un bucle infinito, aceptando conexiones de clientes continuamente.
 Escucha de conexiones: El servidor comienza a escuchar conexiones entrantes en la dirección IP y puerto especificados.
 Manejo de clientes: Por cada cliente que se conecta, se crea un nuevo hilo para manejar la comunicación de manera independiente.
 Procesamiento de mensajes: El servidor recibe mensajes de los clientes, los procesa (invierte el mensaje) y envía una respuesta.
 Concurrencia: El uso de hilos permite al servidor manejar múltiples conexiones de clientes al mismo tiempo.*/



// Importa las librerías necesarias para trabajar con redes, codificación de texto y hilos
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

// Define el espacio de nombres para la organización del código
namespace Server
{
    // Define la clase Program dentro del espacio de nombres Server
    class Program
    {
        // Método principal que se ejecuta al iniciar el programa
        static void Main(string[] args)
        {
            // Imprime un mensaje en la consola indicando que el servidor se está iniciando
            Console.WriteLine("Server: Starting server...");

            // Define la dirección IP y el puerto en el que el servidor escuchará
            IPAddress myIPAddress = IPAddress.Parse("127.0.0.1");
            int myPort = 11000;

            // Crea un nuevo objeto TcpListener para escuchar conexiones en la IP y puerto definidos
            TcpListener server = new TcpListener(myIPAddress, myPort);
            server.Start(); // Inicia el escuchador

            // Muestra un mensaje indicando que el servidor está listo y esperando conexiones
            Console.WriteLine("Server: Server Open, Waiting for a connection...");

            try
            {
                // Bucle infinito para aceptar conexiones de clientes continuamente
                while (true)
                {
                    // Acepta una conexión de cliente entrante
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Server: Client connected!");

                    //Creamos un hilo para manejar la conexion con el cliente conectado
                    //Esta linea lo que hace es crear un nuevo hilo que ejecutará el método HandleClient
                    //HandleClient es el método que se encargará de manejar la conexión con el cliente
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.Start(client); // Inicia el hilo pasando el objeto TcpClient como argumento
                }
            }
            catch (Exception e)
            {
                // Captura y maneja cualquier excepción que ocurra
                Console.WriteLine("Server: An exception occurred: " + e.Message);
                server.Stop(); // Detiene el escuchador en caso de una excepción
            }
        }

        // Método para manejar la comunicación con un cliente
        private static void HandleClient(object obj)
        {
            // Convierte el argumento a un objeto TcpClient
            TcpClient client = (TcpClient)obj;
            try
            {
                // Obtiene el flujo de red asociado al cliente para enviar y recibir datos
                NetworkStream stream = client.GetStream();

                // Define un buffer para almacenar los datos recibidos del cliente
                byte[] buffer = new byte[256];

                // Bucle para leer datos del cliente continuamente
                while (true)
                {
                    // Lee los datos enviados por el cliente
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Si no se leen datos, significa que el cliente se ha desconectado

                    // Convierte los bytes leídos a una cadena de texto
                    string message = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Server: Message received: " + message);

                    // Invierte el mensaje recibido y lo codifica en bytes
                    string reversedMessage = ReverseString(message);
                    byte[] response = Encoding.Unicode.GetBytes(reversedMessage);

                    // Envía la respuesta al cliente
                    stream.Write(response, 0, response.Length);
                }
            }
            catch (Exception e)
            {
                // Captura y maneja cualquier excepción que ocurra con este cliente en particular
                Console.WriteLine("Server: An exception occurred with a client: " + e.Message);
            }
            finally
            {
                // Cierra la conexión con el cliente de forma limpia
                client.Close();
            }
        }

        // Método para invertir un string
        private static string ReverseString(string s)
        {
            // Convierte el string a un array de caracteres, lo invierte y lo convierte de nuevo a string
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
