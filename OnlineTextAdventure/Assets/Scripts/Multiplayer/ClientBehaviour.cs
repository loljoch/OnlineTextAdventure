using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public class ClientBehaviour : MonoBehaviour
{
    public NetworkDriver driver;
    public NetworkConnection connection;
    public bool Done;

    private delegate void PacketHandler(DataStreamReader stream);
    private Dictionary<uint, PacketHandler> packetHandlers;

    private void Start()
    {
        InitializePacketHandlers();

        driver = NetworkDriver.Create();
        connection = default(NetworkConnection);

        var endPoint = NetworkEndPoint.LoopbackIpv4;
        endPoint.Port = 9000;
        connection = driver.Connect(endPoint);

        ClientSend.Initialize(driver, connection);
    }

    private void OnDestroy()
    {
        driver.Dispose();
    }

    private void Update()
    {
        driver.ScheduleUpdate().Complete();

        if (!connection.IsCreated)
        {
            if (!Done)
            {
                Debug.Log("Something went wrong during connect");
            }
            return;
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server");

                ClientSend.SendHello();
            } else if (cmd == NetworkEvent.Type.Data)
            {
                packetHandlers[stream.ReadUInt()](stream);
                Done = true;
            } else if(cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                connection = default(NetworkConnection);
            }
        }
    }

    private void InitializePacketHandlers()
    {
        packetHandlers = new Dictionary<uint, PacketHandler>()
        {
            {(uint)ServerPackets.helloReceived, ClientHandle.HelloReceived },
            {(uint)ServerPackets.echoText, ClientHandle.EchoText }
        };

        Debug.Log("Clientpackethandlers initialized");
    }
}
