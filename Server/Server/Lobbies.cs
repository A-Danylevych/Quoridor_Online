using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class Lobbies
    {
        private List<Lobby> _games;
        private static Lobbies _instance;
        private static readonly object SyncRoot = new object();

        private Lobbies()
        {
            _games = new List<Lobby>();
        }
        public static Lobbies GetInstance()
        {
            if (_instance != null) return _instance;
            lock (SyncRoot)
            {
                _instance ??= new Lobbies();
            }
            return _instance;
        }

        public Client FindGame(Client client)
        {
            foreach (var game in _games.Where(game => game.IsWaiting() && game.InGame))
            {
                client.Color = Color.Red;
                game.SetRedPlayer(client);
                game.StartGame();
                return client;
            }

            client.Color = Color.Green;
            _games.Add(new Lobby(client));
            return client;
        }

        public void CloseFinished()
        {
            for (var i = 0; i < _games.Count; i++)
            {
                if (!_games[i].InGame)
                {
                    _games.RemoveAt(i);
                }
            }
        }

        public bool MakeTurn(CMove move)
        {
            return _games.Where(game => game.ContainsPlayer(move.Password) && 
                                        game.InGame).Select(game => game.MakeMove(move)).FirstOrDefault();
        }
        
        public Dictionary<Lobby, ICollection<SWrapperMessage>> GetMessages()
        {
            var dict = new Dictionary<Lobby, ICollection<SWrapperMessage>>();
            foreach (var game in _games)
            {
                dict[game] = game.GetMessages();
            }

            return dict;
        }
    }
}