using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using TcpClientLib;
using TcpCommonLib;
using Hyper;


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
class AsyncTcpClientExample
{


    static async Task Main()
    {
        Customer customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Age = 30,
            Address = "123 Main St"
        };

        string customerJson = System.Text.Json.JsonSerializer.Serialize(customer);
        var customerData = HyperSerializer<Customer>.Serialize(customer);
        var pool = new TcpClientPool("127.0.0.1", 5000, 3);

        var packet1 = new TcpPacket(customerData.ToArray(), 123, 456);
        var packet2 = new TcpPacket(customerData.ToArray(), 789, 101);


        TcpPacket? response1 = await pool.SendTcpPacketAsync(packet1);
        TcpPacket? response2 = await pool.SendTcpPacketAsync(packet2);


        if (response1 != null)
        {
            var customer1 = HyperSerializer<Customer>.Deserialize(response1?.Payload);
            Console.WriteLine("Response 1: " + customer1.ToString());
        }
        else
            Console.WriteLine("Response 1: null");
        if (response2 != null)
        {
            var customer2 = HyperSerializer<Customer>.Deserialize(response2?.Payload);
            Console.WriteLine("Response 2: " + customer2.ToString());
        }
        else
            Console.WriteLine("Response 2: null");
        Console.ReadLine();


        response1 = await pool.SendTcpPacketAsync(packet1);
        response2 = await pool.SendTcpPacketAsync(packet2);


        if (response1 != null)
        {
            var customer1 = HyperSerializer<Customer>.Deserialize(response1?.Payload);
            Console.WriteLine("Response 1: " + customer1.ToString());
        }
        else
            Console.WriteLine("Response 1: null");
        if (response2 != null)
        {
            var customer2 = HyperSerializer<Customer>.Deserialize(response2?.Payload);
            Console.WriteLine("Response 2: " + customer2.ToString());
        }
        else
            Console.WriteLine("Response 2: null");
        Console.ReadLine();


        response1 = await pool.SendTcpPacketAsync(packet1);
        response2 = await pool.SendTcpPacketAsync(packet2);


        if (response1 != null)
        {
            var customer1 = HyperSerializer<Customer>.Deserialize(response1?.Payload);
            Console.WriteLine("Response 1: " + customer1.ToString());
        }
        else
            Console.WriteLine("Response 1: null");
        if (response2 != null)
        {
            var customer2 = HyperSerializer<Customer>.Deserialize(response2?.Payload);
            Console.WriteLine("Response 2: " + customer2.ToString());
        }
        else
            Console.WriteLine("Response 2: null");
        Console.ReadLine();

        response1 = await pool.SendTcpPacketAsync(packet1);
        response2 = await pool.SendTcpPacketAsync(packet2);


        if (response1 != null)
        {
            var customer1 = HyperSerializer<Customer>.Deserialize(response1?.Payload);
            Console.WriteLine("Response 1: " + customer1.ToString());
        }
        else
            Console.WriteLine("Response 1: null");
        if (response2 != null)
        {
            var customer2 = HyperSerializer<Customer>.Deserialize(response2?.Payload);
            Console.WriteLine("Response 2: " + customer2.ToString());
        }
        else
            Console.WriteLine("Response 2: null");
        Console.ReadLine();

        response1 = await pool.SendTcpPacketAsync(packet1);
        response2 = await pool.SendTcpPacketAsync(packet2);


        if (response1 != null)
        {
            var customer1 = HyperSerializer<Customer>.Deserialize(response1?.Payload);
            Console.WriteLine("Response 1: " + customer1.ToString());
        }
        else
            Console.WriteLine("Response 1: null");
        if (response2 != null)
        {
            var customer2 = HyperSerializer<Customer>.Deserialize(response2?.Payload);
            Console.WriteLine("Response 2: " + customer2.ToString());
        }
        else
            Console.WriteLine("Response 2: null");
        Console.ReadLine();





    }


}