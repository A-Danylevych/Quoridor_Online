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

        private Server(IPEndPoint endPoint)
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
                Console.WriteLine(message);
                _lobbies.MakeTurn(message.Move);
            }

            
            var client = new Client
            {
                Password = message.LogIn.Password
            };
            client.EndPoint = client.EndPoint;
            client = _lobbies.FindGame(client);
            var responseMessage = new SWrapperMessage()
            {
                Confirm = new SConfirm()
                {
                    Color = client.Color,
                }
            };
            await _client.SendAsync(responseMessage.ToByteArray(), responseMessage.ToByteArray().Length,
                result.RemoteEndPoint);
            
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