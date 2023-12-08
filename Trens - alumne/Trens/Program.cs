using System;
using System.Threading;

namespace Trens
{
    class Program
    {
        static SemaphoreSlim semaforo = new SemaphoreSlim(1);
        static int tren1Position = 0;
        static int tren2Position = 100;
        static bool stopTrains = false;

        static void Main(string[] args)
        {
            Thread TTren1 = new Thread(Tren1);
            Thread TTren2 = new Thread(Tren2);
            Thread TVigilante = new Thread(Vigilante);

            TTren1.Start();
            TTren2.Start();
            TVigilante.Start();

            Console.ReadLine();
            
            stopTrains = true;
            
            TTren1.Join();
            TTren2.Join();
            TVigilante.Join();
        }

        static void Tren1()
        {
            for (int i = 0; i <= 100; i++)
            {
                if (stopTrains)
                    break;

                if (i == 20)
                {
                    semaforo.Wait();
                }
                else if (i == 60)
                {
                    semaforo.Release();
                }

                tren1Position = i;

                if ((i >= 20) && (i <= 60))
                {
                    Console.WriteLine("\tTren 1({0})", i);
                }
                else
                {
                    Console.WriteLine("Tren 1({0})", i);
                }

                Thread.Sleep(100);
            }
        }

        static void Tren2()
        {
            for (int i = 100; i >= 0; i--)
            {
                if (stopTrains)
                    break;

                if (i == 60)
                {
                    semaforo.Wait();
                }
                else if (i == 20)
                {
                    semaforo.Release();
                }

                tren2Position = i;

                if ((i >= 20) && (i <= 60))
                {
                    Console.WriteLine("\tTren 2({0})", i);
                }
                else
                {
                    Console.WriteLine("\t\tTren 2({0})", i);
                }

                Thread.Sleep(110);
            }
        }

        static void Vigilante()
        {
            while (true)
            {
                int tren1Pos = GetCurrentPosition(tren1Position);
                int tren2Pos = GetCurrentPosition(tren2Position);

                if ((tren1Pos >= 20 && tren1Pos <= 60) && (tren2Pos >= 20 && tren2Pos <= 60))
                {
                    Console.WriteLine("ALERT: Collision imminent! Stopping trains...");
                    stopTrains = true;
                    break;
                }

                Thread.Sleep(50); // Adjust the sleep time based on your needs
            }
        }

        static int GetCurrentPosition(int position)
        {
            return position;
        }
    }
}
