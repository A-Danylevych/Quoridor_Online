using Model;

namespace Quoridor.Controller
{
    public class Controller : IController
    {
        private string password;
        private Client client;

        public Controller(string password)
        {
            client = Client.GetInstance();
            this.password = password;
        }
        public void SetCell(int top, int left)
        {
            var message = new CWrapperMessage
            {
                Move = new CMove
                {
                    Password = password,
                    Action = Action.Move,
                    Coords = new Coords { Top = top, Left = left }
                }
            };
            client.SendMessage(message);
        }

        public void SetWall(int top, int left, bool isVertical)
        {
            var message = new CWrapperMessage
            {
                Move = new CMove
                {
                    IsVertical = isVertical,
                    Password = password,
                    Action = Action.Wall,
                    Coords = new Coords { Top = top, Left = left }
                }
            };
            client.SendMessage(message);
        }
    }
}

