using System.Runtime.CompilerServices;
using Model;

namespace Server
{
    public class Lobby : IViewer
    {
        public Client RedPlayer { get; private set; }
        public Client GreenPlayer { get; private set; }
        private bool _waitForPlayer;
        private readonly Game _game;
        private SWrapperMessage _message;
        public bool InGame;
        public Lobby(Client client)
        {
            SetGreenPlayer(client);
            _game = new Game(false, this);
            InGame = true;
        }

        public bool ContainsPlayer(string password)
        {
            return RedPlayer.Password == password || GreenPlayer.Password == password;
        }

        public bool MakeMove(CMove move)
        {
            if (RedPlayer.Password == move.Password)
            {
                return ChooseMove(move);
            }
            return GreenPlayer.Password == move.Password && ChooseMove(move);
        }

        private bool ChooseMove(CMove move)
        {
            return move.Action == Action.Move ? 
                _game.MakeMove(new Cell(new CellCoords(move.Coords.Top, move.Coords.Left))) 
                : _game.PlaceWall(new Wall(new CellCoords(move.Coords.Top, move.Coords.Left), move.IsVertical));
        }

        public void StartGame()
        {
            _message = new SWrapperMessage
            {
                GameState = new SGameState()
                {
                    Winning = Color.White,
                }
            };
        }

        public bool IsWaiting()
        {
            return _waitForPlayer;
        }

        public void SetRedPlayer(Client client)
        {
            RedPlayer = client;
            _waitForPlayer = false;
        }

        private void SetGreenPlayer(Client client)
        {
            _waitForPlayer = true;
            GreenPlayer = client;
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
            InGame = false;
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
            _message = new SWrapperMessage()
            {
                GameState = new SGameState()
                {
                    Winning = Color.White,
                    State = 
                    {
                        new State(){Color = Color.Green, RemainingWalls = topCount},
                        new State(){Color = Color.Red, RemainingWalls = bottomCount}
                    }
                }
            };
        }

        public SWrapperMessage GetMessage()
        {
            return _message;
        }
    }
}