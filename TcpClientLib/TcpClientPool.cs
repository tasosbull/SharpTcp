using System.Collections.Concurrent;
using System.Threading.Tasks;
using TcpCommonLib;



namespace TcpClientLib;
public class TcpClientPool
{
    private readonly ConcurrentBag<PersistentTcpClient> _clients = new();
    private readonly string _host;
    private readonly int _port;
    private readonly SemaphoreSlim _semaphore;

    public TcpClientPool(string host, int port, int initialSize = 2)
    {
        _host = host;
        _port = port;
        _semaphore = new SemaphoreSlim(2, 2); // limit to 2 concurrent operations

        for (int i = 0; i < initialSize; i++)
        {
            _clients.Add(new PersistentTcpClient(_host, _port));
        }
    }



    public async Task<TcpPacket?> SendTcpPacketAsync(TcpPacket packet)
    {
        await _semaphore.WaitAsync(); // limits concurrency

        PersistentTcpClient? client = null;

        try
        {
            if (!_clients.TryTake(out client))
                client = new PersistentTcpClient(_host, _port);

            return await client.SendTcpPacketAsync(packet);
        }
        finally
        {
            if (client != null)
                _clients.Add(client);

            _semaphore.Release();
        }
    }
}