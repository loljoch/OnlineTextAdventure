using Unity.Networking.Transport;

public static class ClientSend
{
    #region Setup

    private static NetworkDriver driver;
    private static NetworkConnection connection;

    public static void Initialize(NetworkDriver driver, NetworkConnection connection)
    {
        ClientSend.driver = driver;
        ClientSend.connection = connection;
    }

    #endregion

    #region Functions

    public static void SendHello()
    {
        var writer = driver.BeginSend(connection);
        writer.WriteUInt((uint)ClientPackets.hello);

        driver.EndSend(writer);
    }

    public static void SendText(string text)
    {
        var writer = driver.BeginSend(connection);
        writer.WriteUInt((uint)ClientPackets.text);

        writer.WriteString(text);

        driver.EndSend(writer);
    }


    #endregion
}
