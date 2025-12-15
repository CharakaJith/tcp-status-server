using TcpStatusServer;

class Program
{
    static async Task Main(string[] args)
    {
        var server = new StatusServer(5000);
        await server.StartASync();
    }
}
