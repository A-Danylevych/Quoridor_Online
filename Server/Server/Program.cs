using System.Threading.Tasks;

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
                    
                }
            });
            
            
        }
    }
}