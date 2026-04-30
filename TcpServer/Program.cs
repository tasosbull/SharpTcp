using System.Text;
using TcpCommonLib;
using TcpServerLib;

public class Customer
{
    public Guid Id { get; set; }    
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Address { get; set; }   

    public override string ToString()
    {
        return $"Customer(Id: {Id}, Name: {Name}, Age: {Age}, Address: {Address})";
    } 
}
class ConcreteTcpServer : AsyncTcpServer
{
    public override  TcpPacket Handle(TcpPacket data)
    {
        Console.WriteLine($"Received {data.PayloadLength} bytes");

        Customer? customer = System.Text.Json.JsonSerializer.Deserialize<Customer>(Encoding.UTF8.GetString(data.Payload));   
        customer?.Name += " (processed by server)";
        string customerJson = System.Text.Json.JsonSerializer.Serialize(customer);
        List<byte> list = new List<byte>();
        list.AddRange(Encoding.UTF8.GetBytes(customerJson));    
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


