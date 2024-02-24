using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;

namespace Simulador
{
    class Program
    {
        static private Process P;
        static private Queue<Thread> pila = new Queue<Thread>();
        static private Queue<Process> process = new Queue<Process>();
        static private SemaphoreSlim semaphore = new SemaphoreSlim(10);
        static private String ruta = @"..\..\..\..\Client\bin\Debug\net8.0\Client.exe";
        static void Main(string[] args)
        {
            Thread lectura = new Thread(Planificador);  //|
            lectura.Start();                            //|-> Thread que s'encarrega de planificar els processos

            Console.WriteLine("SIMULADOR INICIAT!\n");

            //Bucle que s'encarrega de llegir les dades que introdueix el client constantment
            while (true)
            {
                P = new Process();
                P.StartInfo.FileName = ruta;
                P.StartInfo.UseShellExecute = true;                 //|
                P.StartInfo.WindowStyle = ProcessWindowStyle.Normal;//|-> Para que se abra la consola del cliente

                Console.Write("\nQuieres ejecutar otro cliente?(s/n): ");
                String option = Console.ReadLine(); //Para que el usuario introduzca su respuesta
                Console.WriteLine("\n\n");
                Thread thread = new Thread(ClientCall); //Thread amb el process del client que s'afegeix a la pila

                //Per sortir del servidor ha d'introduïr exit
                if (option == "n" || option == "N")
                {
                    Console.WriteLine("Saliendo del simulador...");
                    break;
                }

                pila.Enqueue(thread);// Afegeix el thread a la pila.
                process.Enqueue(P);  // Afegeix el process a la pila.
            }
        }

        //Funció que s'encarrega de cridar els clients
        static void ClientCall()
        {
            Process Call = process.Dequeue(); // Agafa el primer element de la pila
            Call.Start();          // Inicia el client.
            Call.WaitForExit();    // Espera a que el client acabi.
            semaphore.Release();   // Allibera un espai del semàfor.
            Console.WriteLine("\nS'ha alliberat un espai, en queden: " + (semaphore.CurrentCount) + "\n");
        }

        //Funció que s'encarrega de planificar els processos dels clients y gestionar la pila
        static void Planificador()
        {
            Thread input;// Thread que s'encarrega de cridar els clients.

            // Bucle que s'encarrega de cridar els clients i treure'ls de la pila
            while (true)
            {
                //Si la pila no està buida, crida un client en quan s'allibera un espai del semàfor
                if (pila.Count != 0)
                {
                    semaphore.Wait();       // Espera a que hi hagi un espai al semàfor.
                    input = pila.Dequeue(); // Agafa el primer element de la pila
                    input.Start();          // Inicia el client.
                    Console.WriteLine("\nHa ocupat un espai, ens queden: " + (semaphore.CurrentCount) + "\n");
                }
            }
        }
    }
}