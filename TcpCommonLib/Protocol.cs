
using System.Net;


namespace TcpCommonLib;
public static class Protocol
{


    public static async Task WriteTcpPacketAsync(Stream stream, TcpPacket packet)
    {
        byte[] lengthBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(packet.PayloadLength));
        byte[] clientIdBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(packet.ClientId));
        byte[] signatureIdBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(packet.SignatureId));

        await stream.WriteAsync(lengthBytes, 0, 4);
        await stream.WriteAsync(clientIdBytes, 0, 4);
        await stream.WriteAsync(signatureIdBytes, 0, 4);
        await stream.WriteAsync(packet.Payload, 0, packet.Payload.Length);
        await stream.FlushAsync();
    }


    public static async Task<TcpPacket?> ReadTcpPacketAsync(Stream stream)
    {
        byte[]? lengthBuffer = await ReadExactAsync(stream, 4);

        if (lengthBuffer == null)
            return null; // disconnected

        int length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(lengthBuffer, 0));

        if (length < 0 || length > 10_000_000)
            throw new InvalidDataException("Invalid packet length");

        byte[]? clientIdBuffer = await ReadExactAsync(stream, 4);
        byte[]? signatureIdBuffer = await ReadExactAsync(stream, 4);

        if (clientIdBuffer == null || signatureIdBuffer == null)
            return null; // disconnected

        int clientId = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(clientIdBuffer, 0));
        int signatureId = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(signatureIdBuffer, 0));

        byte[]? payload = await ReadExactAsync(stream, length);

        if (payload == null)
            return null; // disconnected

        return new TcpPacket(payload, clientId, signatureId);
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