public class TcpPacket
{
    public byte[] Payload { get; set; }
    public int PayloadLength => Payload.Length;
    public int SignatureId { get; set; } // Optional: for protocol-specific handling
    public int ClientId { get; set; } // Optional: track which client sent this packet  
    public int TotalSize => sizeof(int) * 3 + Payload.Length; // 4 bytes for length prefix + payload size


    public TcpPacket(byte[] payload, int clientId = 0, int signatureId = 0)
    {
        Payload = payload;
        ClientId = clientId;
        SignatureId = signatureId;
    }
}