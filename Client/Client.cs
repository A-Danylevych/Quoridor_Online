using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Model;


namespace Quoridor
{
    public class Client
    {
        private const int ListenPort = 11000;
        private readonly UdpClient _server;
        private IViewer _viewer;
        private UserInterface _interface;

        public Client(UserInterface userInterface, IViewer viewer)
        {
            _interface = userInterface;
            _viewer = viewer;
            _server = new UdpClient(endPoint);
        }

        public void SetView(IViewer viewer)
        {
            _viewer = viewer;
        }
        

        public async Task Receive()
        {
            var result = await _server.ReceiveAsync();
            var message = SWrapperMessage.Parser.ParseFrom(result.Buffer);
            if (message.MsgCase == SWrapperMessage.MsgOneofCase.Confirm)
            {
                _interface.Close(message.Confirm.Color);
            }
            else if(message.MsgCase == SWrapperMessage.MsgOneofCase.Move)
            {
                if (message.Move.Action == Action.Move)
                {
                    if (message.Move.Color == Color.Green)
                    {
                        _viewer.RenderUpperPlayer(message.Move.Coords.Top, message.Move.Coords.Left);
                    }
                    else
                    {
                        _viewer.RenderBottomPlayer(message.Move.Coords.Top, message.Move.Coords.Left);
                    }
                }
                else
                {
                    _viewer.RenderWall(message.Move.Coords.Top, message.Move.Coords.Left);
                } 
            }
            else if(message.MsgCase == SWrapperMessage.MsgOneofCase.GameState)
            {
                if(message.GameState.Winning == Color.Red)
                {
                    _viewer.RenderEnding(Color.Red.ToString());
                }
                else if(message.GameState.Winning == Color.Green)
                {
                    _viewer.RenderEnding(Color.Green.ToString());
                }
            }
        }
        
        public void SendMessage(CWrapperMessage message)
        {
            var data = message.ToByteArray();
            _server.Send(data, data.Length);
        }
    }
}