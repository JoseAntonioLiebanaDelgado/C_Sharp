namespace escribir_letra
{
    class Program
    {
        static void Main(string[] args)
        {
            string lletra = args[0];
            int num = Convert.ToInt32(args[1]);
            escriure_lletra(lletra, num);
        }
        static void escriure_lletra(string lletra, int num)
        {
            for (int i = 0; i < num; i++)
            {
                Console.Write(lletra);
                Thread.Sleep(10);
            }
        }
    }
}