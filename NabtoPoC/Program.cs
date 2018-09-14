using System;
using System.IO;
using System.Net.Sockets;

namespace NabtoPoC
{

    class Program
    {

       
        static void Main(string[] args)
        {
            try
            {
                WaggleSocket waggleSocket = new WaggleSocket();
                waggleSocket.initSocket();
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey(true);
            }

        }
               
    }
}
