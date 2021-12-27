using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Model;
using Microsoft.Extensions.Configuration;


namespace Quoridor
{
    public class Client
    {
        private const int ListenPort = 11000;
        private readonly UdpClient _server;
        private IViewer _viewer;
        private UserInterface _interface;
        private static Client _instance; 
        private static readonly object SyncRoot = new object();
        
        public void SetView(IViewer viewer)
        {
            _viewer = viewer;
        }
        public void SetInterface(UserInterface @interface)
        {
            _interface = @interface;
        }

        public async Task Receive()
        {
            while (true)
            {


                var result = await _server.ReceiveAsync();
                var message = SWrapperMessage.Parser.ParseFrom(result.Buffer);
                switch (message.MsgCase)
                {
                    case SWrapperMessage.MsgOneofCase.Confirm:
                        _interface.Close(message.Confirm.Color);
                        break;
                    case SWrapperMessage.MsgOneofCase.Move when message.Move.Action == Action.Move:
                    {
                        if (message.Move.Color == Color.Green)
                        {
                            _viewer.RenderUpperPlayer(message.Move.Coords.Top, message.Move.Coords.Left);
                        }
                        else
                        {
                            _viewer.RenderBottomPlayer(message.Move.Coords.Top, message.Move.Coords.Left);
                        }

                        break;
                    }
                    case SWrapperMessage.MsgOneofCase.Move:
                        _viewer.RenderWall(message.Move.Coords.Top, message.Move.Coords.Left);
                        break;
                    case SWrapperMessage.MsgOneofCase.GameState when message.GameState.Winning == Color.Red:
                        _viewer.RenderEnding(Color.Red.ToString());
                        break;
                    case SWrapperMessage.MsgOneofCase.GameState:
                    {
                        if (message.GameState.Winning == Color.Green)
                        {
                            _viewer.RenderEnding(Color.Green.ToString());
                        }

                        break;
                    }
                }
            }
        }
        
        public void SendMessage(CWrapperMessage message)
        {
            var data = message.ToByteArray();
            _server.Send(data, data.Length);
        }
        private Client()
        {
            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("configuration.json").Build();
            var connection = config.GetConnectionString("DefaultConnection");
            var endpoint = new IPEndPoint(IPAddress.Parse(connection), ListenPort);
            
            _server = new UdpClient();
            _server.Connect(endpoint);
        }
        public static Client GetInstance()
        {
            if (_instance != null) return _instance;
            lock (SyncRoot)
            {
                _instance = new Client();
            }
            return _instance;
        }
    }
}