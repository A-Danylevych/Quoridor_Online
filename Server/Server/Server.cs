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
            Console.WriteLine(message);
            if (message.MsgCase == CWrapperMessage.MsgOneofCase.Move)
            {
                _lobbies.MakeTurn(message.Move);
            }
            else
            {
                var client = new Client
                {
                    Password = message.LogIn.Password,
                    EndPoint = result.RemoteEndPoint
                };
                client = _lobbies.FindGame(client);
                var responseMessage = new SWrapperMessage()
                {
                    Confirm = new SConfirm()
                    {
                        Color = client.Color,
                    }
                };
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(responseMessage);
                Console.ForegroundColor = ConsoleColor.White;
                await _client.SendAsync(responseMessage.ToByteArray(), responseMessage.ToByteArray().Length,
                    result.RemoteEndPoint);
            }
            
            
        }


        public async Task Reply()
        {
            var dict = _lobbies.GetMessages();
            foreach (var (lobby, messages) in dict)
            {
                if (messages == null)
                {
                    continue;
                }

                foreach (var message in messages)
                {
                    if (message == null)
                    {
                        continue;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.White;
                    var data = message.ToByteArray();
                    await _client.SendAsync(data, data.Length, lobby.GreenPlayer.EndPoint);
                    await _client.SendAsync(data, data.Length, lobby.RedPlayer.EndPoint);
                }
                lobby.ClearMessages();
            } 
            _lobbies.CloseFinished();
        }
    }
}