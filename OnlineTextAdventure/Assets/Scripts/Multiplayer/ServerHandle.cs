using Unity.Networking.Transport;

public static class ServerHandle
{
    public static void Hello(DataStreamReader stream, NetworkConnection connection)
    {
        ServerSend.HelloReceived(connection);
    }

    public static void Text(DataStreamReader stream, NetworkConnection connection)
    {
        var text = stream.ReadString();
        ServerSend.EchoText(connection, text);
    }
}
