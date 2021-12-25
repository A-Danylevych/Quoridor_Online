using System.Threading.Tasks;
using System;
using System.Net.Http.Headers;
using System.Net.Sockets;
using Google.Protobuf;

namespace Server
{
    internal static class Program
    {
        private static void Main()
        {
            var server = new Server();
            
            Task.Factory.StartNew(async () => {
                while (true)
                {
                    await server.Receive();
                    server.Reply();
                }
            });
            
            
            string read;
            do
            {
                read = Console.ReadLine();
            } while (read != "quit");
        }
    }
}