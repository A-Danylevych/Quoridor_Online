using System.Threading.Tasks;
using System;

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