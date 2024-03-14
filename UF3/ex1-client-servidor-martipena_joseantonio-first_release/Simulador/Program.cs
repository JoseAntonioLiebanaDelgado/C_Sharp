


/*Este código simula un entorno donde múltiples instancias de un programa cliente (representadas por el proceso Client.exe)
 pueden ser ejecutadas concurrentemente. Utiliza un semáforo para limitar el número de instancias que pueden ejecutarse simultáneamente 
 a un máximo definido (en este caso, 10). Los usuarios pueden iniciar nuevas instancias del cliente a través de la consola, 
 y el programa planifica la ejecución de estas instancias según la disponibilidad permitida por el semáforo.
   
 Simulación de ejecución: El programa simula la ejecución de múltiples instancias de un cliente, controlando la concurrencia con un semáforo.
 Interacción del usuario: A través de la consola, los usuarios pueden decidir lanzar nuevas instancias del cliente.
 Planificación: Un hilo planificador gestiona la cola de ejecución de las instancias del cliente, iniciándolas según la disponibilidad 
 de recursos definida por el semáforo.
 Control de concurrencia: El semáforo limita el número de instancias del cliente que pueden ejecutarse al mismo tiempo, 
 asegurando que no se exceda el límite de recursos.*/



// Importa las librerías necesarias para el funcionamiento del programa
using System;
using System.Diagnostics; // Para trabajar con procesos
using System.Collections.Generic; // Para usar estructuras de datos como Queue
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; // Para trabajar con hilos (Threads) y semáforos

namespace Simulador
{
    class Program
    {
        // Declara variables estáticas para su uso en la clase
        static private Process P; // Para instanciar procesos
        static private Queue<Thread> pila = new Queue<Thread>(); // Cola para almacenar hilos que representan clientes
        static private Queue<Process> process = new Queue<Process>(); // Cola para almacenar procesos de clientes
        static private SemaphoreSlim semaphore = new SemaphoreSlim(10); // Semáforo para controlar la concurrencia, permite hasta 10 recursos concurrentes
        static private String ruta = @"..\..\..\..\Client\bin\Debug\net8.0\Client.exe"; // Ruta del ejecutable del cliente

        // Método principal que se ejecuta al iniciar el programa
        static void Main(string[] args)
        {
            Thread lectura = new Thread(Planificador); // Crea un hilo para el planificador de procesos
            lectura.Start(); // Inicia el hilo del planificador

            Console.WriteLine("SIMULADOR INICIAT!\n");

            // Bucle infinito para leer continuamente las entradas del usuario
            while (true)
            {
                P = new Process();
                P.StartInfo.FileName = ruta; // Establece la ruta del ejecutable del cliente
                P.StartInfo.UseShellExecute = true; // Permite la ejecución del proceso
                P.StartInfo.WindowStyle = ProcessWindowStyle.Normal; // Establece el estilo de ventana del proceso

                Console.Write("\nQuieres ejecutar otro cliente?(s/n): ");
                String option = Console.ReadLine(); // Lee la opción del usuario
                Console.WriteLine("\n\n");
                Thread thread = new Thread(ClientCall); // Crea un hilo para la llamada al cliente

                // Si el usuario elige no continuar, se rompe el bucle
                if (option == "n" || option == "N")
                {
                    Console.WriteLine("Saliendo del simulador...");
                    break;
                }

                pila.Enqueue(thread); // Encola el hilo del cliente
                process.Enqueue(P); // Encola el proceso del cliente
            }
        }

        // Método para llamar a los clientes
        static void ClientCall()
        {
            Process Call = process.Dequeue(); // Desencola un proceso de cliente
            Call.Start(); // Inicia el proceso del cliente
            Call.WaitForExit(); // Espera a que el proceso del cliente termine
            semaphore.Release(); // Libera un recurso del semáforo
            Console.WriteLine("\nS'ha alliberat un espai, en queden: " + (semaphore.CurrentCount) + "\n");
        }

        // Método planificador que gestiona la ejecución de los procesos de los clientes
        static void Planificador()
        {
            Thread input; // Variable para almacenar el hilo que se va a ejecutar

            // Bucle infinito para gestionar la ejecución de los clientes
            while (true)
            {
                if (pila.Count != 0) // Si hay hilos en la cola
                {
                    semaphore.Wait(); // Espera a obtener un recurso del semáforo
                    input = pila.Dequeue(); // Desencola un hilo
                    input.Start(); // Inicia el hilo del cliente
                    Console.WriteLine("\nHa ocupat un espai, ens queden: " + (semaphore.CurrentCount) + "\n");
                }
            }
        }
    }
}
