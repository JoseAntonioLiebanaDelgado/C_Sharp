using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Scheduler
{
    class Program
    {
        // Semaphore to limit the number of processes in progress
        static SemaphoreSlim semaphore = new SemaphoreSlim(3, 3);
        // Queue to store the processes that are waiting to be executed
        static Queue<Tuple<string, string, string>> processQueue = new Queue<Tuple<string, string, string>>();
        // Number of processes in progress
        static int processesInProgress = 0;
        // Flag to exit the program
        static bool exit;

        /**
         * Main method
         * This method starts the user interface and the process queue
         * @param args Arguments
         * @return void
         */
        static void Main(string[] args)
        {
            Thread tUI = new Thread(UserInterface);
            tUI.Start();

            Thread tProcessQueue = new Thread(ProcessQueue);
            tProcessQueue.Start();

            tUI.Join();
            tProcessQueue.Join();
        }

        /**
         * UserInterface method
         * This method shows the user interface and calls the StartProcess method to start a new process
         * with the given parameters if the semaphore is available, otherwise it adds the process to the queue
         * @return void
         */
        static void UserInterface()
        {
            while (!exit)
            {
                int intOption;

                Console.WriteLine($"Clients in process: {processesInProgress}");
                Console.Write("What do you want to do?\n[1] Generate report\n[2] Exit\n> ");
                string option = Console.ReadLine();

                try
                {
                    intOption = Convert.ToInt32(option);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid option!");
                    continue;
                }

                switch (intOption)
                {
                    case 1:
                        Console.WriteLine("Enter name:");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter surname:");
                        string surname = Console.ReadLine();
                        Console.WriteLine("Enter course:");
                        string course = Console.ReadLine();

                        if (semaphore.Wait(0))
                        {
                            Interlocked.Increment(ref processesInProgress);
                            StartProcess(name, surname, course);
                        }
                        else
                        {
                            Console.WriteLine("Process queued.");
                            lock (processQueue)
                            {
                                processQueue.Enqueue(Tuple.Create(name, surname, course));
                            }
                        }
                        break;
                    case 2:
                        Console.WriteLine("Bye!");
                        exit = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        /**
         * StartProcess method
         * This method starts a new process with the given parameters
         * @param name Name
         * @param surname Surname
         * @param course Course
         * @return void
         */
        static void StartProcess(string name, string surname, string course)
        {
            Process P = new Process();
            // Change this path to the client.exe (on windows) or client (on mac/linux) path
            P.StartInfo.FileName = @"/Users/nicolaszarcerogarcia/RiderProjects/Client/Client/bin/Debug/net7.0/Client";
            P.StartInfo.Arguments = name + " " + surname + " " + course;
            P.EnableRaisingEvents = true;
            P.Exited += (sender, e) =>
            {
                semaphore.Release();
                Interlocked.Decrement(ref processesInProgress);
            };

            P.Start();
        }

        /**
         * ProcessQueue method
         * This method processes the queue and starts the processes
         * @return void
         */
        static void ProcessQueue()
        {
            while (true)
            {
                lock (processQueue)
                {
                    if (processQueue.Count > 0 && semaphore.Wait(0))
                    {
                        var processInfo = processQueue.Dequeue();
                        Interlocked.Increment(ref processesInProgress);
                        StartProcess(processInfo.Item1, processInfo.Item2, processInfo.Item3);
                    }
                }

                Thread.Sleep(100);
            }
        }
    }
}