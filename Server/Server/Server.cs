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
        private readonly Lobbies _lobbies;

        public Server() : this(new IPEndPoint(IPAddress.Any, ListenPort))
        {
            
        }

        public Server(IPEndPoint endPoint)
        {
            _client = new UdpClient(endPoint);
            _lobbies = Lobbies.GetInstance();
        }

        public async Task Receive()
        {
            var result = await _client.ReceiveAsync();
            var message = CWrapperMessage.Parser.ParseFrom(result.Buffer);
            if (message.MsgCase == CWrapperMessage.MsgOneofCase.Move)
            {
                _lobbies.MakeTurn(message.Move);
            }

            {
                var client = new Client();
                client.Password = client.Password;
                client.EndPoint = client.EndPoint;
                _lobbies.FindGame(client);
            }
        }


        public void Reply(string message,IPEndPoint endpoint)
        {
            
        }
    }
}