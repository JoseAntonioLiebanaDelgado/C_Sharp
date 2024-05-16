using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

class Emisor
{
    static void Main(string[] args)
    {
        try
        {
            // Conexión al receptor
            TcpClient client = new TcpClient("127.0.0.1", 8888);
            Console.WriteLine("Conectado al receptor.");

            NetworkStream stream = client.GetStream();

            // Solicitar mensaje al usuario
            Console.Write("Ingrese el mensaje a enviar: ");
            string mensaje = Console.ReadLine();

            // Recibir la clave pública del receptor
            byte[] publicKeyBytes = new byte[2048];
            int bytesRead = stream.Read(publicKeyBytes, 0, publicKeyBytes.Length);
            string publicKey = Encoding.ASCII.GetString(publicKeyBytes, 0, bytesRead);
            Console.WriteLine("Clave pública del receptor recibida: " + publicKey);

            // Crear un objeto RSA y cargar la clave pública del receptor
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);

                // Cifrar el mensaje con la clave pública del receptor
                byte[] mensajeCifradoBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(mensaje), false);

                // Envío del mensaje cifrado
                stream.Write(mensajeCifradoBytes, 0, mensajeCifradoBytes.Length);
                Console.WriteLine("Mensaje cifrado enviado al receptor.");
            }

            stream.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }
}