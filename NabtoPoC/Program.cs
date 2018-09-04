using System;
using Nabto.Client;
namespace NabtoPoC
{
    class Program
    {
        static void Main(string[] args)
        {
            string profile = Guid.NewGuid().ToString();
            NabtoClient nabto = new NabtoClient();
            nabto.CreateSelfSignedProfile(profile, "bar");
            Session session = nabto.CreateSession(profile, "bar");
            //TODO
            Console.WriteLine("Starting Nabto...");
            nabto.Dispose();

        }
    }
}
