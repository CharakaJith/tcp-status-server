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
    }
}