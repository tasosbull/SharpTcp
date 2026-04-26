using System;
using System.Net.Sockets;
using System.Threading.Tasks;

public class PersistentTcpClient : IDisposable
{
    private readonly TcpClient _client;
    private readonly NetworkStream _stream;

    private readonly object _lock = new object();

    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public PersistentTcpClient(string host, int port)
    {
        _client = new TcpClient();
        _client.ReceiveTimeout = 5000;
        _client.SendTimeout = 5000;
        _client.Connect(host, port);
        _stream = _client.GetStream();
    }


    public async Task<TcpPacket?> SendTcpPacketAsync(TcpPacket packet)
    {
        await _semaphore.WaitAsync();

        try
        {
            await Protocol.WriteTcpPacketAsync(_stream, packet);
            return await Protocol.ReadTcpPacketAsync(_stream);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _stream?.Dispose();
        _client?.Close();
    }
}