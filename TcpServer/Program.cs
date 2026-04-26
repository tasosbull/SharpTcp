using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using TcpCommonLib;

class AsyncTcpServer
{
    private static readonly int Port = 5000;

    static async Task Main()
    {
        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();

        Console.WriteLine($"Server running on port {Port}");

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private static async Task HandleClientAsync(TcpClient client)
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

    private static TcpPacket Handle(TcpPacket data)
    {
        Console.WriteLine($"Received {data.PayloadLength} bytes");

        var list = new List<byte>(data.Payload);
        list.Reverse();
        return new TcpPacket(list.ToArray(), data.ClientId, data.SignatureId);

        
    }
}