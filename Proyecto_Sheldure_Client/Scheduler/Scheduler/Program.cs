using System.Diagnostics;

namespace Scheduler
{
    class Program
    {
        static SemaphoreSlim semaphore = new SemaphoreSlim(3, 3);
        static Queue<Tuple<string, string, string>> processQueue = new Queue<Tuple<string, string, string>>();
        static bool exit;

        static void Main(string[] args)
        {
            Thread tUI = new Thread(UserInterface);
            tUI.Start();

            Thread tProcessQueue = new Thread(ProcessQueue);
            tProcessQueue.Start();
            
            tUI.Join();
            tProcessQueue.Join();
        }

        static void UserInterface()
        {
            while (!exit)
            {
                int intOption;

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

        static void StartProcess(string name, string surname, string course)
        {
            Process P = new Process();
            P.StartInfo.FileName = @"/Users/nicolaszarcerogarcia/RiderProjects/Client/Client/bin/Debug/net7.0/Client";
            P.StartInfo.Arguments = name + " " + surname + " " + course;
            P.EnableRaisingEvents = true;
            P.Exited += (sender, e) =>
            {
                semaphore.Release();
            };

            P.Start();
            P.WaitForExit();
        }

        static void ProcessQueue()
        {
            while (true)
            {
                lock (processQueue)
                {
                    if (processQueue.Count > 0 && semaphore.Wait(0))
                    {
                        var processInfo = processQueue.Dequeue();
                        StartProcess(processInfo.Item1, processInfo.Item2, processInfo.Item3);
                    }
                }
                
                Thread.Sleep(100);
            }
        }
    }
}
