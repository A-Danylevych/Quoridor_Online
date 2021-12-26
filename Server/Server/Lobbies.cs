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
            foreach (var game in _games.Where(game => game.IsWaiting()))
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
            foreach (var lobby in _games.Where(lobby => !lobby.InGame))
            {
                _games.Remove(lobby);
            }
        }

        public bool MakeTurn(CMove move)
        {
            return _games.Where(game => game.ContainsPlayer(move.Password)).Select(game => game.MakeMove(move)).FirstOrDefault();
        }
        
        public Dictionary<Lobby, SWrapperMessage> GetMessages()
        {
            var dict = new Dictionary<Lobby, SWrapperMessage>();
            foreach (var game in _games)
            {
                dict[game] = game.GetMessage();
            }

            return dict;
        }
    }
}