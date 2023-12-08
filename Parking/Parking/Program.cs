using System;
using System.Threading;

namespace Parking
{
    class Program
    {
        static SemaphoreSlim parkingSemaphore = new SemaphoreSlim(5);
        static int TotalCoches = 0;
        static readonly object locker = new object();
        static bool parkingClosed = false;
        
        static void CerrarParking()
        {
            while (Console.ReadKey().KeyChar != 't') { }
            parkingClosed = true;
            Console.WriteLine("Parking cerrado");
        }
        
        static void Main(string[] args)
        {
            Thread Cierre = new Thread(CerrarParking);
            Cierre.Start();
            for (int i = 0; i < 25; i++)
            {
                Thread C = new Thread(Coche);
                C.Start(i);
            }
        }

        static void Coche(object Id)
        {
            Random rnd = new Random();
            int TiempoAcceso;
            int TiempoParking;
            int IdCoche = Convert.ToInt32(Id);

            TiempoAcceso = rnd.Next(1000, 5000);
            TiempoParking = rnd.Next(1000, 5000);

            Thread.Sleep(TiempoAcceso);
            if (parkingClosed)
            {
                Console.WriteLine("Coche {0} pasa de largo", IdCoche);
            }
            else
            {
                Console.WriteLine("Coche {0} quiere entrar al párking", IdCoche);
            
                try
                {
                    parkingSemaphore.Wait();
                    lock (locker)
                    {
                        TotalCoches += 1;
                        Console.WriteLine("             Coche {0} está dentro del párking y hay {1} coches dentro", IdCoche, TotalCoches);
                    }
                    Thread.Sleep(TiempoParking);
                }
                finally
                {
                    lock (locker)
                    {
                        TotalCoches -= 1;
                        Console.WriteLine("                         Coche {0} está fuera del párking y hay {1} coches dentro", IdCoche, TotalCoches);
                    }
                    parkingSemaphore.Release();
                }
            }
        }
    }
}
