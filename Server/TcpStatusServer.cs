using System.Net;
using System.Net.Sockets;
using TcpStatusServer.Server;

namespace TcpStatusServer
{
    /// <summary>
    /// tcp server responsible for accepting client connections
    /// manage connection life cycle, and poll for status
    /// </summary>
    public class StatusServer
    {
        private readonly int _port;
        private readonly TcpListener _listener;
        private readonly List<ClientConnection> _clients = new();

        private bool _pollingEnabled = true;
        private int _pollIntervalSeconds = 5;

        /// <summary>
        /// create new server bounded to the given porn
        /// </summary>
        /// <param name="port">port number to listen for incoming tcp connections</param>
        public StatusServer(int port)
        {
            _port = port;
            _listener = new TcpListener(IPAddress.Any, port);
        }

        /// <summary>
        /// start the server and accept client connections
        /// launch a background task to poll connected clients
        /// </summary>
        /// <returns></returns>
        public async Task StartASync()
        {
            // start server
            _listener.Start();
            Console.WriteLine($"server started on port: {_port}");
            
            // run background polling logic
            _ = Task.Run(PollClientAsync);

            while (true)
            {
                // wait for a client to connect
                var tcpClient = await _listener.AcceptTcpClientAsync();
                var client = new ClientConnection(tcpClient, RemoveClient);

                // lock shared client list from concurrent access
                lock (_clients)
                {
                    _clients.Add(client);

                }

                Console.WriteLine("Client connected");

                // handle client communication asynchronously
                _ = client.HandleAsync();
            }
        }

        /// <summary>
        /// send STATUS command to all connected clients periodically 
        /// run in the background continuously 
        /// </summary>
        private async Task PollClientAsync()
        {
            while (true)
            {
                if (_pollingEnabled)
                {
                    // thread-safe iteration over connected clients
                    lock (_clients)
                    {
                        foreach (var client in _clients)
                        {
                            client.SendCommand("STATUS");
                        }
                    }
                }

                // wait before next cycle
                await Task.Delay(TimeSpan.FromSeconds(_pollIntervalSeconds));
            }
        }

        /// <summary>
        /// remove disconnected client from active list
        /// triggered by ClientConnection when the connection is closed
        /// </summary>
        /// <param name="client">disconnected client</param>
        private void RemoveClient(ClientConnection client)
        {
            // remove client safely 
            lock (_clients)
            {
                _clients.Remove(client);
            }

            Console.WriteLine("Client disconnected");
        }
    }
}