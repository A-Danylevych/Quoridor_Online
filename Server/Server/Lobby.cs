using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Model;

namespace Server
{
    public class Lobby : IViewer
    {
        public Client RedPlayer { get; private set; }
        public Client GreenPlayer { get; private set; }
        private bool _waitForPlayer;
        private Client _currentPlayer;
        private readonly Game _game;
        private List<SWrapperMessage> _messages;
        public bool InGame;
        public Lobby(Client client)
        {
            SetGreenPlayer(client);
            _game = new Game(false, this);
            InGame = true;
            _messages = new List<SWrapperMessage>();
        }

        private void SwitchPlayer()
        {
            _currentPlayer = _currentPlayer == RedPlayer ? GreenPlayer : RedPlayer;
        }

        public bool ContainsPlayer(string password)
        {
            return RedPlayer.Password == password || GreenPlayer.Password == password;
        }

        public bool MakeMove(CMove move)
        {
            if (_currentPlayer.Password != move.Password)
            {
                return false;
            }
            
            if (!ChooseMove(move)) return false;
            SwitchPlayer();
            return true;

        }

        private bool ChooseMove(CMove move)
        {
            
            return move.Action == Action.Move ? 
                _game.MakeMove(new Cell(new CellCoords(move.Coords.Top, move.Coords.Left))) 
                : _game.PlaceWall(new Wall(new CellCoords(move.Coords.Top, move.Coords.Left), move.IsVertical));
      
        }

        public void StartGame()
        {
            var message = new SWrapperMessage
            {
                GameState = new SGameState()
                {
                    Winning = Color.White,
                }
            };
            _messages.Add(message);
        }

        public bool IsWaiting()
        {
            return _waitForPlayer;
        }

        public void SetRedPlayer(Client client)
        {
            RedPlayer = client;
            _waitForPlayer = false;
            _currentPlayer = RedPlayer;
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
            var message = new SWrapperMessage
            {
                GameState = new SGameState()
                {
                    Winning = winColor,
                },
            };
            InGame = false;
            _messages.Add(message);
        }

        public void RenderUpperPlayer(int top, int left)
        {
            var message = new SWrapperMessage
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
            _messages.Add(message);
        }

        public void RenderBottomPlayer(int top, int left)
        {
            var message = new SWrapperMessage
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
            _messages.Add(message);
        }

        public void RenderWall(int top, int left)
        {
            var message = new SWrapperMessage
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
            _messages.Add(message);
        }

        public void RenderRemainingWalls(int topCount, int bottomCount)
        {
            var message = new SWrapperMessage()
            {
                Walls = new SWalls()
                {
                    GreenWalls = topCount,
                    RedWalls = bottomCount,
                }
            };
            _messages.Add(message);
        }

        public void ClearMessages()
        {
            _messages.Clear();
        }

        public ICollection<SWrapperMessage> GetMessages()
        {
            return _messages;
        }
    }
}