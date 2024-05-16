using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

class Receptor
{
    static void Main(string[] args)
    {
        try
        {
            // Iniciar el listener en el puerto 8888
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
            listener.Start();
            Console.WriteLine("Esperando conexión del emisor...");

            // Aceptar la conexión del emisor
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Conectado al emisor.");

            NetworkStream stream = client.GetStream();

            // Generar un par de claves RSA
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // Obtener la clave pública
                string publicKey = rsa.ToXmlString(false);
                byte[] publicKeyBytes = Encoding.ASCII.GetBytes(publicKey);

                // Envío de clave pública al emisor
                stream.Write(publicKeyBytes, 0, publicKeyBytes.Length);
                Console.WriteLine("Clave pública enviada al emisor.");

                // Recepción del mensaje cifrado
                byte[] mensajeCifradoBytes = new byte[256]; // Tamaño ajustado para RSA
                int bytesRead = stream.Read(mensajeCifradoBytes, 0, mensajeCifradoBytes.Length);
                Array.Resize(ref mensajeCifradoBytes, bytesRead); // Ajustar tamaño del array

                // Descifrar el mensaje cifrado
                byte[] mensajeBytes = rsa.Decrypt(mensajeCifradoBytes, false);
                string mensajeDesencriptado = Encoding.UTF8.GetString(mensajeBytes);
                Console.WriteLine("Mensaje desencriptado: " + mensajeDesencriptado);
            }

            stream.Close();
            client.Close();
            listener.Stop();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }
}
