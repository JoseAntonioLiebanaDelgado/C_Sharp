// Ejercicio Sensors de transit

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SensorsTransit
{
    public class Sensor
    {
        public int IdSensor { get; set; }
        public int NumVehicles { get; set; }

        public Sensor(int id, int vehicles)
        {
            IdSensor = id;
            NumVehicles = vehicles;
        }

        public Sensor EnviaDadesCentral()
        {
            // Simula un retraso en el envío de datos.
            Task.Delay(100).Wait();
            return this;
        }

        public void Connecta()
        {
            // Simula un retraso en la conexión.
            Task.Delay(200).Wait();
        }

        public void Reset()
        {
            NumVehicles = 0;
        }
    }

    class SensorsCiutat
    {
        private static int NumSensors;
        public int MitjanaVehicles { get; private set; }
        private int SumaVehicles;
        public int MaxVehicles { get; private set; }
        private List<Sensor> Sensors = new List<Sensor>();
        private List<Sensor> SensorsLocal = new List<Sensor>();
        private List<Sensor> SensorsLocalOrd = new List<Sensor>();

        public SensorsCiutat(int num)
        {
            NumSensors = num;
        }

        public void InicialitzaSensors()
        {
            Random rnd = new Random();
            for (int i = 0; i < NumSensors; i++)
            {
                Sensors.Add(new Sensor(i, rnd.Next(0, 100)));
            }
        }

        public void RebreDadesSensors()
        {
            var localSensors = new ConcurrentBag<Sensor>();
            Parallel.For(0, NumSensors, i =>
            {
                localSensors.Add(Sensors[i].EnviaDadesCentral());
            });

            SensorsLocal = new List<Sensor>(localSensors);
        }

        public void MostraDadesSensors()
        {
            Parallel.ForEach(SensorsLocal, sensor =>
            {
                Console.WriteLine($"Sensor nº {sensor.IdSensor} --- {sensor.NumVehicles} vehicles comptabilitzats.");
            });
        }

        public void ResetSensors()
        {
            Parallel.For(0, NumSensors, i =>
            {
                Sensors[i].Connecta();
                Sensors[i].Reset();
            });
        }

        public void CalcularMitjana()
        {
            long sumaLocal = 0;
            Parallel.ForEach(SensorsLocal, sensor =>
            {
                Interlocked.Add(ref sumaLocal, sensor.NumVehicles);
            });

            MitjanaVehicles = (int)(sumaLocal / NumSensors);
        }

        public void CalcularMaxVehicles()
        {
            int maxLocal = 0;
            Parallel.ForEach(SensorsLocal, sensor =>
            {
                int vehicleCount = sensor.NumVehicles;
                Interlocked.Exchange(ref maxLocal, Math.Max(maxLocal, vehicleCount));
            });

            MaxVehicles = maxLocal;
        }

        public void OrdenaVehicles()
        {
            SensorsLocalOrd = new List<Sensor>(SensorsLocal);
            SensorsLocalOrd.Sort((a, b) => a.NumVehicles.CompareTo(b.NumVehicles));
        }

        public void MostraDadesOrdenades()
        {
            foreach (var sensor in SensorsLocalOrd)
            {
                Console.WriteLine($"Sensor nº {sensor.IdSensor}: {sensor.NumVehicles} vehicles");
            }
        }

        private int Suma(int i, Sensor s)
        {
            return i + s.NumVehicles;
        }

        private int MesGran(int i, Sensor s)
        {
            return Math.Max(i, s.NumVehicles);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SensorsCiutat SensorsBCN = new SensorsCiutat(100);

            DateTime T1 = DateTime.Now;

            SensorsBCN.InicialitzaSensors();
            Console.WriteLine("Rebent dades dels sensors...");
            SensorsBCN.RebreDadesSensors();
            Console.WriteLine("Dades rebudes");

            SensorsBCN.MostraDadesSensors();

            Thread threadCalcularMitjana = new Thread(new ThreadStart(SensorsBCN.CalcularMitjana));
            Thread threadCalcularMaxVehicles = new Thread(new ThreadStart(SensorsBCN.CalcularMaxVehicles));

            threadCalcularMitjana.Start();
            threadCalcularMaxVehicles.Start();

            threadCalcularMitjana.Join();
            threadCalcularMaxVehicles.Join();

            Console.WriteLine($"Mitjana vehicles: {SensorsBCN.MitjanaVehicles}");
            Console.WriteLine($"Max vehicles: {SensorsBCN.MaxVehicles}");

            Console.WriteLine("Ordenant vehicles");
            SensorsBCN.OrdenaVehicles();
            SensorsBCN.MostraDadesOrdenades();

            Thread threadResetSensors = new Thread(new ThreadStart(SensorsBCN.ResetSensors));
            threadResetSensors.Start();
            threadResetSensors.Join();

            DateTime T2 = DateTime.Now;
            TimeSpan Diff = T2 - T1;
            Console.WriteLine("Tot el procés ha trigat {0}", Diff.ToString());

            Console.ReadLine();
        }
    }
}
