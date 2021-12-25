using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor
{
    public class Client
    {
        private readonly UdpClient _client;
        private const int ListenPort = 11000;

        public Server(IPEndPoint endPoint)
        {
            _client = new UdpClient(endPoint);
        }

        public async Task Receive()
        {
            
        }


        public void Reply(string message, IPEndPoint endpoint)
        {

        }
    }
}