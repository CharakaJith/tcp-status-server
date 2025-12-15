using System.Net.Sockets;
using System.Text;
using TcpStatusServer.Protocol;

namespace TcpStatusServer.Server
{
    /// <summary>
    /// represents a single client connection to the TCP server
    /// handles reading messages from the client and send commands
    /// manage disconnection events 
    /// </summary>
    public class ClientConnection
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly Action<ClientConnection> _onDisconnect;

        /// <summary>
        /// initialize a new client connection 
        /// </summary>
        /// <param name="client">connected TcpClient instance</param>
        /// <param name="onDisconnect">callback to trigger when client disconnects</param>
        public ClientConnection(TcpClient client, Action<ClientConnection> onDisconnect)
        {
            _client = client;
            _stream = client.GetStream();
            _onDisconnect = onDisconnect;
        }

        /// <summary>
        /// handle client communication asynchronously
        /// continuously read messages from client till disconnects 
        /// </summary>
        public async Task HandleAsync()
        {
            // buffer to read incoming bytes
            var buffer = new byte[1024];

            try
            {
                while(_client.Connected)
                {
                    // read incoming data asynchronously
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);

                    // check if connection is closed
                    if (bytesRead == 0)
                        break;

                    // decode and process the recieved message
                    var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    HandleMessage(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client error: {ex.Message}");
            }
            finally
            {
                // close client socket and notify server
                _client.Close();
                _onDisconnect(this);
            }
        }

        /// <summary>
        /// send command to the client
        /// check connection state and catch exceptions
        /// </summary>
        /// <param name="command">command string to send</param>
        public void SendCommand(string command)
        {
            // check if connection is closed
            if (!_client.Connected)
                return;

            try
            {
                // command string -> bytes and write
                var data = Encoding.UTF8.GetBytes($"{command} \n");
                _stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                // log failure context
                var endpoint = _client.Client.RemoteEndPoint?.ToString() ?? "unknown endpoint";

                Console.WriteLine(
                    $"Failed to send command '{command}' to client {endpoint}. " +
                    $"Error: {ex.GetType().Name} - {ex.Message}"
                );

                // close client socket on error
                _client.Close();
            }
        }

        /// <summary>
        /// process message from client
        /// identify message type and log meaningful message
        /// </summary>
        /// <param name="message">message recieved from client</param>
        private static void HandleMessage(string message)
        {
            // map message pattern to readable logs
            var output = message switch
            {
                var msg when msg.StartsWith(ProtocolMessages.Ack) => "ACK received from client",
                var msg when msg.StartsWith(ProtocolMessages.StatusReply) => $"Status received: {message}",
                var msg when msg.StartsWith(ProtocolMessages.Busy) => "Client is busy",
                var msg when msg.StartsWith(ProtocolMessages.Error) => $"Client error: {message}",
                _ => $"Unknown message: {message}"
            };

            Console.WriteLine(output);
        }
    }
}