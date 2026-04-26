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

    public async Task<byte[]?> SendAsync(byte[] payload)
{
    await _semaphore.WaitAsync();

    try
    {
        await Protocol.WriteMessageAsync(_stream, payload);
        return await Protocol.ReadMessageAsync(_stream);
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