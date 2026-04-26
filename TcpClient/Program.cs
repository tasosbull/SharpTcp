using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using TcpClientLib;
using TcpCommonLib;

class AsyncTcpClientExample
{


    static async Task Main()
    {
        var pool = new TcpClientPool("127.0.0.1", 5000, 3);

        var packet1 = new TcpPacket(Encoding.UTF8.GetBytes("Hello, Server!"), 123, 456);
        var packet2 = new TcpPacket(Encoding.UTF8.GetBytes("Another message"), 789, 101);


        TcpPacket? response1 = await pool.SendTcpPacketAsync(packet1);
        TcpPacket? response2 = await pool.SendTcpPacketAsync(packet2);

        if (response1 != null)
            Console.WriteLine("Response 1: " + Encoding.UTF8.GetString(response1.Payload));
        else
            Console.WriteLine("Response 1: null");
        if (response2 != null)
            Console.WriteLine("Response 2: " + Encoding.UTF8.GetString(response2.Payload));
        else
            Console.WriteLine("Response 2: null");

    }


}