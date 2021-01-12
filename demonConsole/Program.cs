using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace demonConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var app = ConfidentialClientApplicationBuilder
                .Create("ae8ac5e8-76d9-4a3e-90ab-db98ab3c3c6c")
                .WithClientSecret("WNA5dTlI4LJPHn8YDTNKEC_-Nr6w.~q-2s")
                .WithAuthority("https://login.microsoftonline.com/45009c1d-fca9-4573-bd77-7bbf46a85d11")
                .Build();

            var authResult = app.AcquireTokenForClient(new[] {"api://0a49a349-2a13-4471-93d2-067dce77a7f2/.default"}).ExecuteAsync().Result;
            
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
            var result = client.GetAsync("https://localhost:5002/WeatherForecast").Result;

            var body = result.Content.ReadAsStringAsync().Result;
        }
    }
}