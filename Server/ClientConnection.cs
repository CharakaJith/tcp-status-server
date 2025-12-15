using System.Net.Sockets;
using System.Text;

namespace TcpStatusServer.Server
{
    public class ClientConnection
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly Action<ClientConnection> _onDisconnect;

        public ClientConnection(TcpClient client, Action<ClientConnection> onDisconnect)
        {
            this._client = client;
            this._stream = client.GetStream();
            this._onDisconnect = onDisconnect;
        }

        public async Task HandleAsync()
        {
            var buffer = new byte[1024];

            try
            {
                while(this._client.Connected)
                {
                    int bytesRead = await this._stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                        break;

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
                this._client.Close();
                this._onDisconnect(this);
            }
        }

        public void SendCommand(string command)
        {
            if (!this._client.Connected)
                return;

            try
            {
                var data = Encoding.UTF8.GetBytes($"{command} \n");
                this._stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                this._client.Close();

            }
        }

        private void HandleMessage(string message)
        {
            var output = message switch
            {
                var msg when msg.StartsWith("ACK") => "ACK received from client",
                var msg when msg.StartsWith("STATUS_REPLY") => $"Status received: {message}",
                var msg when msg.StartsWith("BUSY") => "Client is busy",
                var msg when msg.StartsWith("ERROR") => $"Client error: {message}",
                _ => $"Unknown message: {message}"
            };

            Console.WriteLine(output);
        }
    }
}