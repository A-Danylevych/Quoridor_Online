using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private readonly UdpClient _client;
        private const int ListenPort = 11000;

        public Server() : this(new IPEndPoint(IPAddress.Any, ListenPort))
        {
        }

        public Server(IPEndPoint endPoint)
        {
            _client = new UdpClient(endPoint);
        }
        

        public async Task<Received> Receive()
        {
            var result = await _client.ReceiveAsync();
            return new Received()
            {
                Message = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length),
                Sender = result.RemoteEndPoint
            };
        }
        
        public void Reply(string message,IPEndPoint endpoint)
        {
            var datagram = Encoding.ASCII.GetBytes(message);
            _client.Send(datagram, datagram.Length,endpoint);
        }
    }
    public struct Received
    {
        public IPEndPoint Sender;
        public string Message;
    }
}