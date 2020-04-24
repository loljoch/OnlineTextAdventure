using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Collections;
using Unity.Networking.Transport;

public class ServerBehaviour : MonoBehaviour
{
    public NetworkDriver driver;
    private NativeList<NetworkConnection> connections;

    private delegate void PacketHandler(DataStreamReader stream, NetworkConnection connection);
    private Dictionary<uint, PacketHandler> packetHandlers;

    private void Start()
    {
        InitializePacketHandlers();

        driver = NetworkDriver.Create();
        var endPoint = NetworkEndPoint.AnyIpv4;
        endPoint.Port = 9000;

        if (driver.Bind(endPoint) != 0)
        {
            Debug.Log($"Failed to bind to port {endPoint}");
        } else
        {
            driver.Listen();
        }

        connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

        ServerSend.Initialize(driver);
    }

    private void OnDestroy()
    {
        driver.Dispose();
        connections.Dispose();
    }

    private void Update()
    {
        driver.ScheduleUpdate().Complete();

        //Clean up old connections
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i; //Go back because we removed an index
            }
        }

        //Accept new connection - returns an up-to-date connection list
        NetworkConnection connection;
        while ((connection = driver.Accept()) != default(NetworkConnection))
        {
            connections.Add(connection);
            Debug.Log("Accepted a connection");
        }


        //Start querying driver events that might have happened since the last update
        DataStreamReader stream;
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections[i].IsCreated)
            {
                continue;
            }
            
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                //Process events
                if (cmd == NetworkEvent.Type.Data)
                {
                    packetHandlers[stream.ReadUInt()](stream, connections[i]);

                    //Handle disconnections
                } else if(cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    connections[i] = default(NetworkConnection);
                }
            }
        }
    }

    private void InitializePacketHandlers()
    {
        packetHandlers = new Dictionary<uint, PacketHandler>()
        {
            {(uint)ClientPackets.hello, ServerHandle.Hello },
            {(uint)ClientPackets.text, ServerHandle.Text }
        };

        Debug.Log("Serverpackethandlers initialized");
    }
}
