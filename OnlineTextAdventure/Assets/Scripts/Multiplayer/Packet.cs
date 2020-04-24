/// <summary>
/// Sent from server to client
/// </summary>
public enum ServerPackets
{
    helloReceived = 1,
    echoText = 2
}

/// <summary>
/// Sent from client to server
/// </summary>
public enum ClientPackets
{
    hello = 1,
    text = 2
}
