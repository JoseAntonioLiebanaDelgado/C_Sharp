


/*Este código implementa un cliente TCP básico que se conecta a un servidor TCP en una dirección IP y puerto específicos
 (en este caso, 127.0.0.1 en el puerto 11000).
 Una vez establecida la conexión, el cliente puede recibir y enviar mensajes al servidor.
 Utiliza hilos (Threads) para manejar simultáneamente la lectura y escritura de mensajes,
 permitiendo así que el cliente pueda enviar mensajes mientras espera recibirlos.
 El programa continúa ejecutándose hasta que el usuario decide cerrarlo manualmente.

 El código se divide en tres partes principales:
 Conexión al servidor: El cliente establece una conexión TCP con el servidor usando la dirección IP y el puerto definidos.
 Comunicación: Una vez conectado, el programa inicia dos hilos: uno para leer los mensajes del servidor y otro para enviar mensajes al servidor.
 Cierre de conexión: La conexión y los recursos asociados se cierran adecuadamente antes de finalizar el programa.*/



// Importa las bibliotecas necesarias para trabajar con redes y codificación de texto
using System.Net;
using System.Net.Sockets;
using System.Text;

// Define el espacio de nombres para la organización del código
namespace Client
{
    // Define la clase Program dentro del espacio de nombres Client
    class Program
    {
        // Variable estática para controlar el estado de ejecución del cliente
        private static bool isRunning = true;

        // Método principal que se ejecuta al iniciar el programa
        static void Main(string[] args)
        {
            // Muestra un mensaje en la consola indicando que el cliente se está iniciando
            Console.WriteLine("Client: Starting client...");

            // Define la dirección IP y puerto del servidor al cual conectarse
            // IPAddress ServerIP; indica que ServerIP es una variable de tipo IPAddress
            IPAddress ServerIP;
            ServerIP = IPAddress.Parse("127.0.0.1"); // Dirección IP del servidor
            int MyPort = 11000; // Puerto para la conexión

            // Crea un nuevo cliente TCP
            // TcpClient Client; indica que Client es una variable de tipo TcpClient
            TcpClient Client = new TcpClient();
            // Conecta el cliente TCP al servidor usando la IP y puerto definidos
            Client.Connect(ServerIP, MyPort);
            // Muestra un mensaje en la consola indicando que la conexión fue exitosa
            Console.WriteLine("Client: Connected to server");

            // Obtiene el flujo de red asociado al cliente TCP,
            // en otras palabras, el canal de comunicación con el servidor
            // Creamos una variable de tipo NetworkStream llamada MyNetworkStream y
            // y le asignamos el valor de Client.GetStream()
            NetworkStream MyNetworkStream = Client.GetStream();

            // Crea e inicia hilos para leer y escribir datos en el flujo de red
            // Creamos dos hilos, uno para leer y otro para escribir
            Thread read = new Thread(Read);
            Thread write = new Thread(Write);
            // Iniciamos los hilos
            read.Start(MyNetworkStream);
            write.Start(MyNetworkStream);
            // Espera a que los hilos de lectura y escritura finalicen
            // join() es un método que espera a que el hilo termine
            read.Join();
            write.Join();
            // Cierra el flujo de red y la conexión TCP
            // Close() es un método que cierra el flujo de red y la conexión TCP
            // El flujo de res se cerrará automáticamente cuando se cierre la conexión TCP
            MyNetworkStream.Close();
            Client.Close();
        }

        // Método para leer datos del servidor
        // Object nsClient indica que nsClient es un objeto de tipo Object
        static void Read(Object nsClient)
        {
            // Convierte el objeto pasado al método en un objeto NetworkStream y
            // lo almacena en la variable NSClient
            NetworkStream NSClient = (NetworkStream)nsClient;
            try
            {
                // Bucle que se ejecuta mientras isRunning sea verdadero
                while (isRunning)
                {
                    // Define un buffer para almacenar los datos recibidos
                    // Un buffer es un área de memoria que se utiliza para almacenar datos temporalmente
                    byte[] buffer = new byte[256];
                    // Lee los datos del flujo de red y almacena el número de bytes leídos
                    int bytesRead = NSClient.Read(buffer, 0, buffer.Length);
                    // Convierte los bytes leídos a una cadena de texto
                    // Los parámetros son el buffer, el índice inicial y el número de bytes leídos
                    // En el buffer esta el array de bytes que se ha leído
                    string input = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    // Muestra el mensaje recibido en la consola
                    // El input es el mensaje que se recibe del servidor
                    Console.WriteLine("Client: Message received: " + input);
                }
            }
            catch (Exception ex)
            {
                // Muestra el error en caso de excepción y detiene el bucle
                Console.WriteLine("Read thread exception: " + ex.Message);
                isRunning = false;
            }
        }

        // Método para enviar datos al servidor
        static void Write(Object nsClient)
        {
            // Convierte el objeto pasado al método en un objeto NetworkStream
            NetworkStream NSClient = (NetworkStream)nsClient;
            try
            {
                // Bucle que se ejecuta mientras isRunning sea verdadero
                while (isRunning)
                {
                    // Lee una línea de texto de la consola y la almacena en la variable message
                    string message = Console.ReadLine();
                    // Comprueba si el mensaje es "exit" para detener el cliente
                    if (message == "exit")
                    {
                        isRunning = false;
                        break;
                    }
                    // Convierte el mensaje a un array de bytes
                    byte[] msg = Encoding.Unicode.GetBytes(message);
                    // Escribe el mensaje en el flujo de red
                    // Los parámetros son el array de bytes, el índice inicial y el número de bytes a escribir
                    NSClient.Write(msg, 0, msg.Length);
                }
            }
            catch (Exception ex)
            {
                // Muestra el error en caso de excepción y detiene el bucle
                Console.WriteLine("Write thread exception: " + ex.Message);
                isRunning = false;
            }
        }
    }
}
