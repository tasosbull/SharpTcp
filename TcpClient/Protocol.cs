using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

public static class Protocol
{
    public static async Task WriteMessageAsync(Stream stream, byte[] payload)
    {
        int length = payload.Length;
        byte[] lengthBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(length));

        await stream.WriteAsync(lengthBytes, 0, 4);
        await stream.WriteAsync(payload, 0, payload.Length);
        await stream.FlushAsync();
    }

    public static async Task<byte[]?> ReadMessageAsync(Stream stream)
    {
        byte[]? lengthBuffer = await ReadExactAsync(stream, 4);

        if (lengthBuffer == null)
            return null; // disconnected

        int length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(lengthBuffer, 0));

        if (length < 0 || length > 10_000_000)
            throw new InvalidDataException("Invalid message length");

        return await ReadExactAsync(stream, length);
    }

    private static async Task<byte[]?> ReadExactAsync(Stream stream, int size)
    {
        byte[] buffer = new byte[size];
        int offset = 0;

        while (offset < size)
        {
            int read = await stream.ReadAsync(buffer, offset, size - offset);

            if (read == 0)
                return null; // disconnected

            offset += read;
        }

        return buffer;
    }
}