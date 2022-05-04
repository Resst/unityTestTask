using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
public class Server : MonoBehaviour
{
    //Singleton
    public static Server instance;
    private void Awake()
    {
        instance = this;
    }

    private NetworkDriver driver;
    private NativeList<NetworkConnection> connections;

    private bool isActive = false;

    public void Start()
    {
        if (Variables.isHost)
            Init(Variables.port);
        Client.instance.Init(Variables.connectionIp, Variables.port);
    }

    public void Init(ushort port)
    {
        //Initializing driver using our port
        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = port;

        //Success check
        if (driver.Bind(endpoint) != 0)
            Debug.LogError("Failed to bind to port " + port);
        else
            driver.Listen();

        //Initializing connections
        connections = new NativeList<NetworkConnection>(Client.instance.players.Length, Allocator.Persistent);
        isActive = true;
    }

    private void Update()
    {
        if (!isActive)
            return;

        //Update driver
        driver.ScheduleUpdate().Complete();

        CleanupConnections();
        AcceptNewConnections();
        ProcessEvents();

    }
    private void CleanupConnections()
    {
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i;
            }
        }
    }
    private void AcceptNewConnections()
    {
        NetworkConnection c;
        while ((c = driver.Accept()) != default(NetworkConnection))
        {
            connections.Add(c);
            Debug.Log("Accepted a connection");
        }
    }
    private void ProcessEvents()
    {
        DataStreamReader stream;
        for (int i = 0; i < connections.Length; i++)
        {
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    //Processing data
                    NetDataProcessor.ProcessData(stream, connections[i], true);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    //Processing disconnect
                    Debug.LogWarning("Client disconnected from server (Server side)");
                    connections[i] = default(NetworkConnection);

                }
            }
        }
    }
    public void SendToClient(NetworkConnection connection, NetMessage msg)
    {
        DataStreamWriter stream;
        driver.BeginSend(connection, out stream);
        msg.Serialize(ref stream);
        driver.EndSend(stream);
    }
    public void Broadcast(NetMessage msg)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            if (connections[i].IsCreated)
                SendToClient(connections[i], msg);
        }
    }

    public int GetFreePlayer()
    {
        for (int i = 0; i < Client.instance.players.Length; i++)
        {
            if (!Client.instance.players[i].isControlled)
            {
                Client.instance.players[i].isControlled = true;
                return Client.instance.players[i].number;
            }
        }
        return -1;
    }


    private void Shutdown()
    {
        if (isActive)
        {
            driver.Dispose();
            connections.Dispose();
            isActive = false;
        }
    }
    public void OnDestroy()
    {
        Shutdown();
    }
}
