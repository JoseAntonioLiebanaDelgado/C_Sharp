using System;

namespace Test // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static bool done = false;
        static readonly object locker = new object();
        static int contador = 0;
        
        static void Main(string[] args)
        {
            // Thread t1 = new Thread(Go);
            // Thread t2 = new Thread(Go);
            // t1.Start();
            // t2.Start();
            // Console.ReadLine();
            
            Thread thread1 = new Thread(Run);
            Thread thread2 = new Thread(Run);

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            Console.WriteLine(contador);
            Console.ReadLine();
        }

        // static void Go()
        // {
        //     lock (locker)
        //     {
        //         if (!done)
        //         {
        //             Thread.Sleep(10);
        //             done = true;
        //             Console.WriteLine("Done");
        //         }
        //     }
        // }

        static void Run()
        {
            for (int i = 0; i < 1000000; i++)
            {
                lock (locker) {
                    contador++;
                }
            }
        }
    }
}