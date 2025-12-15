using System.Net;
using System.Net.Sockets;

namespace TcpStatusServer
{
    public class StatusServer
    {
        private readonly int _port;
        private readonly TcpListener _listener;

        public StatusServer(int port)
        {
            this._port = port;
            this._listener = new TcpListener(IPAddress.Any, port);
        }

        public async Task StartASync()
        {
            this._listener.Start();
            Console.WriteLine($"server started on port: {this._port}");

            // _ = 
        }
    }
}