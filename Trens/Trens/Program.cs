using System;
using System.Threading;

namespace Trens
{
    class Program
    {
        static readonly object lockObject = new object();
        static bool isTramoSharedInUse = false;

        static void Main(string[] args)
        {
            Thread TTren1 = new Thread(Tren1);
            Thread TTren2 = new Thread(Tren2);

            TTren1.Start();
            TTren2.Start();

            TTren1.Join();
            TTren2.Join();

            Console.WriteLine("Ambos trenes han finalizado su recorrido.");
        }

        static void Tren1()
        {
            for (int i = 0; i <= 100; i++)
            {
                if (i == 20)
                {
                    RequestAccessToTramoShared("Tren 1");
                }

                if (i >= 20 && i <= 60)
                {
                    Console.WriteLine("\t\tTren 1({0})", i);
                }
                else
                {
                    Console.WriteLine("Tren 1({0})", i);
                }

                if (i == 60)
                {
                    ReleaseAccessToTramoShared("Tren 1");
                }

                Thread.Sleep(250);
            }
        }

        static void Tren2()
        {
            for (int i = 100; i >= 0; i--)
            {
                if (i == 60)
                {
                    RequestAccessToTramoShared("Tren 2");
                }

                if (i <= 60 && i >= 20)
                {
                    Console.WriteLine("\t\t\t\tTren 2({0})", i);
                }
                else
                {
                    Console.WriteLine("\t\t\t\tTren 2({0})", i);
                }

                if (i == 20)
                {
                    ReleaseAccessToTramoShared("Tren 2");
                }

                Thread.Sleep(200);
            }
        }

        static void RequestAccessToTramoShared(string trenName)
        {
            lock (lockObject)
            {
                while (isTramoSharedInUse)
                {
                    Console.WriteLine($"{trenName} esperando para acceder al tramo compartido.");
                    Monitor.Wait(lockObject);
                }
                isTramoSharedInUse = true;
                Console.WriteLine($"{trenName} ha entrado en el tramo compartido.");
            }
        }

        static void ReleaseAccessToTramoShared(string trenName)
        {
            lock (lockObject)
            {
                isTramoSharedInUse = false;
                Monitor.Pulse(lockObject);
                Console.WriteLine($"{trenName} ha salido del tramo compartido.");
            }
        }
    }
}
