namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 3)
            {
                string nombre = args[0];
                string apellido = args[1];
                int numCurso;

                if (int.TryParse(args[2], out numCurso))
                {
                    GenerarBoletin(nombre, apellido, numCurso);
                }
                else
                {
                    Console.WriteLine("El número de curso no es válido.");
                }
            }
            else
            {
                Console.WriteLine("Por favor, proporciona nombre, apellido y número de curso.");
            }
        }

        static void GenerarBoletin(string nombre, string apellido, int numCurso)
        {
            Console.WriteLine("Alumno: " + nombre + " " + apellido);
            Console.WriteLine("Curso: DAM" + numCurso);

            if (numCurso == 1 || numCurso == 2)
            {
                Random randomGrade = new Random();

                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("M0" + (i + 1) + ": " + randomGrade.Next(0, 11));
                    Thread.Sleep(20);
                }
            }
            else
            {
                Console.WriteLine("El curso DAM" + numCurso + " no existe!");
            }
        }
    }
}