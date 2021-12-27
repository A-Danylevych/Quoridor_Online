using System.Net;

namespace Server
{
    public class Client
    {
        public IPEndPoint EndPoint;
        public Color Color;
        public string Password;

        public Client(IPEndPoint point, Color color, string password)
        {
            EndPoint = point;
            Color = color;
            Password = password;
        }

        public Client()
        {
            
        }
    }
}