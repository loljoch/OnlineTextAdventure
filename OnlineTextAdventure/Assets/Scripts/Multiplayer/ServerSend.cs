using Unity.Collections;
using Unity.Networking.Transport;

public static class ServerSend
{
    #region Setup
    private static NetworkDriver driver;

    public static void Initialize(NetworkDriver driver)
    {
        ServerSend.driver = driver;
    }

    #endregion


    #region Functions

    public static void HelloReceived(NetworkConnection connection)
    {
        var writer = driver.BeginSend(NetworkPipeline.Null, connection);
        writer.WriteUInt((uint)ServerPackets.helloReceived);

        writer.WriteString("I (the server) welcome you!");

        driver.EndSend(writer);
    }

    public static void EchoText(NetworkConnection connection, NativeString64 text)
    {
        var writer = driver.BeginSend(NetworkPipeline.Null, connection);
        writer.WriteUInt((uint)ServerPackets.echoText);

        writer.WriteString("Echo: " + text);

        driver.EndSend(writer);
    }


    #endregion
}
