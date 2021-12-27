using System.Threading.Tasks;
using System;

namespace Server
{
    internal class Program
    {
        private static void Main()
        {
            Task.Factory.StartNew(async () => {
                var server = new Server();
                while (true)
                {
                    await server.Receive();
                    await server.Reply();
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