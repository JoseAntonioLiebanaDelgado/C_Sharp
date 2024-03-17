// Importamos las librerías necesarias para trabajar con conexiones de red, codificación de texto y serialización JSON.
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

// Declaramos el espacio de nombres 'Client' para nuestro programa.
namespace Client
{
    // Definimos una clase 'Program' que contendrá nuestro método 'Main'.
    class Program
    {
        // El método 'Main' es el punto de entrada del programa.
        static void Main(string[] args)
        {
            // Definimos la dirección IP del servidor al cual nos queremos conectar.
            string serverIP = "127.0.0.1";
            // Definimos el puerto del servidor al cual nos queremos conectar.
            int serverPort = 50000;

            // Utilizamos la declaración 'using' para asegurar que 'TcpClient' se cierre automáticamente.
            using (TcpClient client = new TcpClient())
            {
                // Establecemos la conexión con el servidor.
                client.Connect(serverIP, serverPort);
                // Indicamos en la consola que se ha realizado la conexión.
                Console.WriteLine("Conectado al servidor.");

                // Obtenemos el flujo de red para enviar y recibir datos.
                NetworkStream stream = client.GetStream();
                // Creamos un buffer para almacenar los datos que recibimos.
                byte[] buffer = new byte[1024];
                // Leemos los datos del flujo de red y guardamos el número de bytes leídos.
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                // Convertimos los bytes recibidos a una cadena de texto utilizando UTF-8.
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                // Mostramos el mensaje recibido en la consola.
                Console.WriteLine($"Datos recibidos: {message}");

                // Utilizamos un bloque 'try-catch' para manejar posibles excepciones durante la deserialización.
                try
                {
                    // Deserializamos el mensaje JSON recibido a un objeto 'ClasseJugador'.
                    ClasseJugador player = JsonSerializer.Deserialize<ClasseJugador>(message);
                    // Mostramos en la consola la información del jugador deserializada.
                    Console.WriteLine($"ID: {player.id_jugador}, Color: {player.color}");
                }
                catch (Exception ex)
                {
                    // En caso de error durante la deserialización, mostramos el mensaje de error en la consola.
                    Console.WriteLine($"Error al deserializar: {ex.Message}");
                }
                // El 'TcpClient' se cierra automáticamente al final del bloque 'using'.
            }
        }
    }
}

// Suponemos que existe una definición de la clase 'ClasseJugador' en otro lugar del código,
// que contiene al menos dos propiedades: 'id_jugador' y 'color'.
