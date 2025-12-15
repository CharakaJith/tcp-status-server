namespace TcpStatusServer.Protocol
{
    /// <summary>
    /// define all protocol commands and response messages
    /// </summary>
    public static class ProtocolMessages
    {
        // server -> client
        public const string StatusReq = "STATUS";
        public const string StatusRes = "SET_OUTPUT";

        // client -> server
        public const string Ack = "ACK";
        public const string Busy = "BUSY";
        public const string StatusReply = "STATUS_REPLY";
        public const string Error = "ERROR";
        public const string Name = "NAME";
    }
}