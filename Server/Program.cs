using System;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Server.Quik;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.WriteLine("Connecting to QUIK");
                bool isConnected = Task.Run(QuikConnector.Connect).Result;
                Console.WriteLine(isConnected ? "Connected" : "Error while connection, check lua script");
                Console.WriteLine("Press Ender to exit...");
                Console.ReadLine();
            }
        }
    }
}
