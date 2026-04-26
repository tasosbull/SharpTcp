using System.Net;
using System.Net.Sockets;
using TcpCommonLib;

namespace TcpServerLib
{
    public abstract class AsyncTcpServer
    {

        public async Task StartAsync(int port, CancellationToken cancellationToken = default)
        {
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Console.WriteLine($"Server running on port {port}");
            cancellationToken.Register(() => listener.Stop());
            try
            {
                while (true)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    _ = HandleClientAsync(client);
                }
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception) { cancellationToken.ThrowIfCancellationRequested(); }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            Console.WriteLine("Client connected");

            try
            {
                using (client)
                using (var stream = client.GetStream())
                {
                    while (true)
                    {
                        var request = await Protocol.ReadTcpPacketAsync(stream);

                        if (request == null)
                            break;

                        var response = Handle(request);

                        await Protocol.WriteTcpPacketAsync(stream, response);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Client disconnected");
        }

        public abstract TcpPacket Handle(TcpPacket data);

    }
}