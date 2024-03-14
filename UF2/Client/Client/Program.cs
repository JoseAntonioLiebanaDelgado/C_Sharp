namespace Client
{
    class Program
    {
        /**
         * This program generates a report for a student taking the parameters from the command line.
         * It will be used by the scheduler program to test it.
         * @param args The student's name, surname and course number.
         * @param extra params will be ignored.
         * @return void. The report is printed to the console.
         */
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

        /**
         * Generates a report for a student.
         * @param nombre The student's name.
         * @param apellido The student's surname.
         * @param numCurso The student's course number.
         * @return void. The report is printed to the console.
         */
        static void GenerarBoletin(string nombre, string apellido, int numCurso)
        {
            Thread.Sleep(20000);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Alumno: " + nombre + " " + apellido);
            Console.WriteLine("Curso: DAM" + numCurso);

            if (numCurso == 1 || numCurso == 2)
            {
                Random randomGrade = new Random();

                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("M0" + (i + 1) + ": " + randomGrade.Next(0, 11));
                }
            }
            else
            {
                Console.WriteLine("El curso DAM" + numCurso + " no existe!");
            }
            Console.WriteLine("-------------------------------------------");
        }
    }
}