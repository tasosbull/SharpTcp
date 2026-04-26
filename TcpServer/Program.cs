using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using TcpCommonLib;
using TcpServerLib;


class ConcreteTcpServer : AsyncTcpServer
{
    public override  TcpPacket Handle(TcpPacket data)
    {
        Console.WriteLine($"Received {data.PayloadLength} bytes");

        var list = new List<byte>(data.Payload);
        list.Reverse();
        return new TcpPacket(list.ToArray(), data.ClientId, data.SignatureId);
    }
}

class TcpServer
{
    private static readonly int Port = 5000;

    static async Task Main()
    {

        ConcreteTcpServer server = new ConcreteTcpServer();
        await server.StartAsync(Port);


    }

}


