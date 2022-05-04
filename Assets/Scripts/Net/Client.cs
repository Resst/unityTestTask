using Unity.Networking.Transport;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Client instance;

    //Player Lists
    public Player[] players;
    private void Awake()
    {
        instance = this;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].number = i;
        }
    }


    private NetworkDriver driver;
    private NetworkConnection connection;

    private bool isActive = false;

    [System.NonSerialized]public int playerNumber = -1;

    public void Init(string ip, ushort port)
    {

        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.Parse(ip, port);

        Debug.Log("trying to connect to server");

        connection = driver.Connect(endpoint);

        isActive = true;
    }


    private void Update()
    {
        if (!isActive)
            return;

        //Update driver
        driver.ScheduleUpdate().Complete();

        ProcessEvents();
    }

    private void ProcessEvents()
    {
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("Connected to the server");
                SendToServer(new NetWelcome());
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                //Processing data
                NetDataProcessor.ProcessData(stream, connection);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                //Processing disconnect
                Debug.LogWarning("Client disconnected from server (Client side)");
                connection = default(NetworkConnection);
                players[playerNumber].isControlled = false;
                Shutdown();
            }
        }
    }

    public void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }

    void Shutdown()
    {
        if (isActive)
        {
            driver.Dispose();
            isActive = false;
        }
    }
    private void OnDestroy()
    {
        Shutdown();
    }

}
