using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Client
    {
        private readonly UdpClient _client;
        
        public Client(UdpClient client)
        {
            _client = client;
        }
        
        public void Send(string message)
        {
            var datagram = Encoding.ASCII.GetBytes(message);
            _client.Send(datagram, datagram.Length);
        }
    }
}