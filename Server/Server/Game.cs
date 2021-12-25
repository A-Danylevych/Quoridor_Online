namespace Server
{
    public class Game
    {
        private Client _RedPlayer;
        private Client _GreenPlayer;
        private bool _gameOver;

        public bool IsPlaying()
        {
            return _gameOver;
        }

        public void SetRedPlayer(Client client)
        {
            _RedPlayer = client;
        }

        public void SetGreenPlayer(Client client)
        {
            
        }
        
        

    }
}