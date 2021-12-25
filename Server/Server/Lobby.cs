using System.Net;
using Model;

namespace Server
{
    public class Lobby : IViewer
    {
        private Client _redPlayer;
        private Client _greenPlayer;
        private bool _waitForPlayer;
        private readonly Game _game;
        private SWrapperMessage _message;
        public Lobby(Client client)
        {
            SetGreenPlayer(client);
            _game = new Game(false, this);
        }

        public bool ContainsPlayer(string password)
        {
            return _redPlayer.Password == password || _greenPlayer.Password == password;
        }

        public bool MakeMove(CMove move)
        {
            if (_redPlayer.Password == move.Password)
            {
                return ChooseMove(move);
            }
            return _greenPlayer.Password == move.Password && ChooseMove(move);
        }

        private bool ChooseMove(CMove move)
        {
            return move.Action == Action.Move ? 
                _game.MakeMove(new Cell(new CellCoords(move.Coords.Top, move.Coords.Left))) 
                : _game.PlaceWall(new Wall(new CellCoords(move.Coords.Top, move.Coords.Left), move.IsVertical));
        }

        public bool IsWaiting()
        {
            return _waitForPlayer;
        }

        public void SetRedPlayer(Client client)
        {
            _redPlayer = client;
            _waitForPlayer = false;
        }

        public void SetGreenPlayer(Client client)
        {
            _waitForPlayer = true;
            _greenPlayer = client;
        }
        

        public void RenderEnding(Model.Color color)
        {
            var winColor = Color.Red;
            if (color == Model.Color.Green)
            {
                winColor = Color.Green;
            }
            _message = new SWrapperMessage
            {
                GameState = new SGameState()
                {
                    Winning = winColor,
                },
            };
        }

        public void RenderUpperPlayer(int top, int left)
        {
            _message = new SWrapperMessage
            {
                Move = new SMove()
                {
                    Color = Color.Green,
                    Action = Action.Move,
                    Coords = new Coords
                    {
                        Left = left,
                        Top = top,
                    }
                }
            };
        }

        public void RenderBottomPlayer(int top, int left)
        {
            _message = new SWrapperMessage
            {
                Move = new SMove()
                {
                    Color = Color.Red,
                    Action = Action.Move,
                    Coords = new Coords
                    {
                        Left = left,
                        Top = top,
                    }
                }
            };
        }

        public void RenderWall(int top, int left)
        {
            _message = new SWrapperMessage
            {
                Move = new SMove()
                {
                    Action = Action.Wall,
                    Coords = new Coords
                    {
                        Left = left,
                        Top = top,
                    }
                }
            };
        }

        public void RenderRemainingWalls(int topCount, int bottomCount)
        {
            throw new System.NotImplementedException();
        }
    }
}