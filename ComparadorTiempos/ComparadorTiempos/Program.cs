using System;
using System.Diagnostics;
using System.Threading;

namespace ComparadorTiempos
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            EscribirA();
            EscribirB();
            EscribirC();
            stopWatch.Stop();

            TimeSpan sequentialTS = stopWatch.Elapsed;
            string elapsedTimeSequential = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                                sequentialTS.Hours,
                                                sequentialTS.Minutes,
                                                sequentialTS.Seconds,
                                                sequentialTS.Milliseconds / 10);
            Console.WriteLine("Sequential TimeStamp: " + elapsedTimeSequential);

            Thread t1 = new Thread(EscribirA);
            Thread t2 = new Thread(EscribirB);
            Thread t3 = new Thread(EscribirC);

            Stopwatch stopWatch2 = new Stopwatch();
            stopWatch2.Start();
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            stopWatch2.Stop();

            TimeSpan concurrentTS = stopWatch2.Elapsed;
            string elapsedTimeConcurrent = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                                concurrentTS.Hours,
                                                concurrentTS.Minutes,
                                                concurrentTS.Seconds,
                                                concurrentTS.Milliseconds / 10);
            Console.WriteLine("Concurrent TimeStamp: " + elapsedTimeConcurrent);

            Console.ReadLine();
        }

        private static void EscribirA()
        {
            for (int i = 0; i < 500; i++)
            {
                Console.Write("a");
                Thread.Sleep(10);
            }
        }

        private static void EscribirB()
        {
            for (int i = 0; i < 500; i++)
            {
                Console.Write("b");
                Thread.Sleep(10);
            }
        }

        private static void EscribirC()
        {
            for (int i = 0; i < 500; i++)
            {
                Console.Write("c");
                Thread.Sleep(10);
            }
        }
    }
}
