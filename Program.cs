using Microsoft.Extensions.Configuration;
using TcpStatusServer;

class Program
{
    static async Task Main(string[] args)
    {
        // laod configurations
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // read connection details
        var portStr = config["Server:Port"] ?? throw new InvalidOperationException("Server:Port not configured");
        var port = int.Parse(portStr);


        var server = new StatusServer(port);
        await server.StartASync();
    }
}
