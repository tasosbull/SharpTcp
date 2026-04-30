********************************************************************************************

public delegate Task<byte[]> RpcHandler(byte[] data);

public class RpcRegistry
{
    private readonly Dictionary<(int serviceId, int methodId), RpcHandler> _handlers
        = new();

    public void Register(int serviceId, int methodId, RpcHandler handler)
    {
        _handlers[(serviceId, methodId)] = handler;
    }

    public Task<byte[]> Invoke(int serviceId, int methodId, byte[] data)
    {
        if (_handlers.TryGetValue((serviceId, methodId), out var handler))
        {
            return handler(data);
        }

        throw new Exception("Handler not found");
    }
}

/*
registry.Register(1, 1, async data =>
{
    var request = Deserialize<MyRequest>(data);
    var response = new MyResponse { Result = "OK" };
    return Serialize(response);
});
*/

*********************************************************************************

[AttributeUsage(AttributeTargets.Class)]
public class ServiceAttribute : Attribute
{
    public int ServiceId { get; }
    public ServiceAttribute(int serviceId) => ServiceId = serviceId;
}

[AttributeUsage(AttributeTargets.Method)]
public class MethodAttribute : Attribute
{
    public int MethodId { get; }
    public MethodAttribute(int methodId) => MethodId = methodId;
}

[Service(1)]
public class UserService
{
    [Method(1)]
    public async Task<MyResponse> GetUser(MyRequest request)
    {
        return new MyResponse { Result = "User data" };
    }
}

public void RegisterServices(RpcRegistry registry)
{
    var types = Assembly.GetExecutingAssembly().GetTypes();

    foreach (var type in types)
    {
        var serviceAttr = type.GetCustomAttribute<ServiceAttribute>();
        if (serviceAttr == null) continue;

        var instance = Activator.CreateInstance(type);

        foreach (var method in type.GetMethods())
        {
            var methodAttr = method.GetCustomAttribute<MethodAttribute>();
            if (methodAttr == null) continue;

            registry.Register(serviceAttr.ServiceId, methodAttr.MethodId,
                async (data) =>
                {
                    var paramType = method.GetParameters()[0].ParameterType;
                    var request = Deserialize(data, paramType);

                    var result = method.Invoke(instance, new[] { request });

                    if (result is Task task)
                    {
                        await task;
                        var response = task.GetType().GetProperty("Result")?.GetValue(task);
                        return Serialize(response);
                    }

                    return Serialize(result);
                });
        }
    }
}

********************************************************************************************8

public interface IRpcService
{
    int ServiceId { get; }
}

public class UserService : IRpcService
{
    public int ServiceId => 1;

    public Dictionary<int, Func<byte[], Task<byte[]>>> Methods => new()
    {
        { 1, GetUser }
    };

    private async Task<byte[]> GetUser(byte[] data)
    {
        var request = Deserialize<MyRequest>(data);
        var response = new MyResponse { Result = "OK" };
        return Serialize(response);
    }
}



bonus

class RpcRequest
{
    public int ServiceId { get; set; }
    public int MethodId { get; set; }
    public byte[] Payload { get; set; }
}