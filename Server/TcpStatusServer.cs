using System.Net;
using System.Net.Sockets;
using TcpStatusServer.Server;

namespace TcpStatusServer
{
    public class StatusServer
    {
        private readonly int _port;
        private readonly TcpListener _listener;
        private readonly List<ClientConnection> _clients = new();

        private bool _pollingEnabled = true;
        private int _pollIntervalSeconds = 5;

        public StatusServer(int port)
        {
            _port = port;
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public async Task StartASync()
        {
            _listener.Start();
            Console.WriteLine($"server started on port: {_port}");

            _ = Task.Run(PollClientAsync);

            while (true)
            {
                var tcpClient = await _listener.AcceptTcpClientAsync();
                var client = new ClientConnection(tcpClient, RemoveClient);

                lock (_clients)
                {
                    _clients.Add(client);

                }

                Console.WriteLine("Client connected");

                _ = client.HandleAsync();
            }
        }

        private async Task PollClientAsync()
        {
            while (true)
            {
                if (_pollingEnabled)
                {
                    lock (_clients)
                    {
                        foreach (var client in _clients)
                        {
                            client.SendCommand("STATUS");
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(_pollIntervalSeconds));
            }
        }

        private void RemoveClient(ClientConnection client)
        {
            lock (_clients)
            {
                _clients.Remove(client);
            }

            Console.WriteLine("Client disconnected");
        }
    }
}