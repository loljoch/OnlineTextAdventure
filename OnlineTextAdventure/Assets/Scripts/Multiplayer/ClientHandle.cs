using UnityEngine;
using Unity.Networking.Transport;

public static class ClientHandle
{
    public static void HelloReceived(DataStreamReader stream)
    {
        var text = stream.ReadString();
        Debug.Log(text);
    }

    public static void EchoText(DataStreamReader stream)
    {
        var text = stream.ReadString();
        Debug.Log(text);
    }
}
