using System.Collections.Generic;
using System;
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

        public void FindGame(Client client)
        {
            foreach (var game in _games.Where(game => game.IsWaiting()))
            {
                client.Color = Color.Red;
                game.SetRedPlayer(client);
                return;
            }

            client.Color = Color.Green;
            _games.Add(new Lobby(client));
        }

        public bool MakeTurn(CMove move)
        {
            return _games.Where(game => game.ContainsPlayer(move.Password)).Select(game => game.MakeMove(move)).FirstOrDefault();
        }
    }
}