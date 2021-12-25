using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Google.Protobuf;

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
                client.Password = message.LogIn.Password;
                client.EndPoint = client.EndPoint;
                _lobbies.FindGame(client);
            }
        }


        public void Reply()
        {
            var dict = _lobbies.GetMessages();
            foreach (var (lobby, message) in dict)
            {
                var data = message.ToByteArray();
                _client.Send(data, data.Length, lobby.GreenPlayer.EndPoint);
                _client.Send(data, data.Length, lobby.RedPlayer.EndPoint);
            } 
        }
    }
}