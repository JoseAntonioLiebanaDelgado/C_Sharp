using System.Diagnostics;

namespace CreadorProceso
{
    class Program
    {
        static void Main(string[] args)
        {
            Process P = new Process();
            P.StartInfo.FileName = @"/Users/nicolaszarcerogarcia/RiderProjects/escribir_letra/EscribirLetra/bin/Debug/net7.0/EscribirLetra";
            P.StartInfo.Arguments = "a 500";
            Console.WriteLine("Process is about to start");
            P.Start();
            Console.WriteLine("Process was started");
            P.WaitForExit();
            Console.WriteLine("\nProcess ended");
        }
    }
}