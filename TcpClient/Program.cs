using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

class AsyncTcpClientExample
{
    

    static async Task Main()
    {
        var pool = new TcpClientPool("127.0.0.1", 5000, 3);

        byte[]? response1 = await pool.SendAsync(System.Text.Encoding.UTF8.GetBytes("Message 1"));
        byte[]? response2 = await pool.SendAsync(System.Text.Encoding.UTF8.GetBytes("Message 2"));

        if (response1 != null)
            Console.WriteLine("Response 1: " + Encoding.UTF8.GetString(response1));
        else
            Console.WriteLine("Response 1: null");
        if (response2 != null)
            Console.WriteLine("Response 2: " + Encoding.UTF8.GetString(response2));
        else
            Console.WriteLine("Response 2: null");
       
    }

    
}