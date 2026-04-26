
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TcpCommonLib;

public class TcpBinSerializer
{
    public static byte[]? SerializeToJsonBytes(object obj)
    {
        if (obj == null) return null;
        // JsonSerializer.SerializeToUtf8Bytes returns a byte[] directly
        return JsonSerializer.SerializeToUtf8Bytes(obj);
    }

    // Deserialize from a byte array
    public static T? DeserializeFromJsonBytes<T>(byte[] bytes)
    {
        if (bytes == null) return default;
        return JsonSerializer.Deserialize<T>(bytes);
    }

    // Using BinaryData for wrapping/interoperability
    public static void UseBinaryData(byte[] data)
{
    var binaryData = new BinaryData(data);
    // BinaryData provides properties like ToBytes() and methods to convert to/from strings/JSON
}
}