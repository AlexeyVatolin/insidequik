using System;
using System.Reflection;
using Microsoft.Owin.Hosting;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8000";
            try
            {
                using (WebApp.Start(url))
                {
                    Console.WriteLine("Server running on {0}", url);

                    Console.WriteLine("Press Enter to exit...");
                    Console.ReadLine();
                }
            }
            catch (TargetInvocationException)
            {
                Console.WriteLine("Error: Server is already running");
            }
        }
    }
}
