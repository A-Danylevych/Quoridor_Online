using System.Threading.Tasks;
using System;

namespace Server
{
    internal static class Program
    {
        private static void Main()
        {
            var server = new Server();
            var lobbies = Lobbies.GetInstance();
            
            Task.Factory.StartNew(async () => {
                while (true)
                {
                    await server.Receive();
                    server.Reply();
                    lobbies.CloseFinished();
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