using System;
using System.Diagnostics;
using System.Threading;

namespace Atletisme
{
    class Program
    {
        static Random rnd1 = new Random();

        static void Main(string[] args)
        {
            // Sin relevos
            Thread firstRunner = new Thread(() => corredor(1, "Nico"));
            Thread secondRunner = new Thread(() => corredor(2, "Zarcero"));
            Thread thirdRunner = new Thread(() => corredor(3, "Garcia"));
            Thread fourthRunner = new Thread(() => corredor(4, "NZG"));
            
            firstRunner.Start();
            secondRunner.Start();
            thirdRunner.Start();
            fourthRunner.Start();
            
            firstRunner.Join();
            secondRunner.Join();
            thirdRunner.Join();
            fourthRunner.Join();
            
            // Con relevos
            firstRunner = new Thread(() => corredor(1, "Nico"));
            secondRunner = new Thread(() => corredor(2, "Zarcero"));
            thirdRunner = new Thread(() => corredor(3, "Garcia"));
            fourthRunner = new Thread(() => corredor(4, "NZG"));
            
            firstRunner.Start();
            firstRunner.Join();
            
            secondRunner.Start();
            secondRunner.Join();
            
            thirdRunner.Start();
            thirdRunner.Join();
            
            fourthRunner.Start();
            fourthRunner.Join();
        }

        static void corredor(int dorsal, String name)
        {
            Console.WriteLine("Running: " + name + " (" + dorsal + ")");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //La variable temps representa el quanta estona estarpa corrent el corredor,
            //que serà un valor entre 10 i 15 segons.
            int temps;
            temps = rnd1.Next(1000, 1500);
            Thread.Sleep(temps);
            stopwatch.Stop();
            
            Console.WriteLine("Finished " + name + " (" + dorsal + "). Time: " + stopwatch.ElapsedMilliseconds / 100.0 + " seconds");
        }
    }
}
