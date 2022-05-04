using Unity.Networking.Transport;
using UnityEngine;

public class NetWelcome : NetMessage
{
    public int number;

    public NetWelcome()
    {
        opCode = OpCode.WELCOME;
    }
    public NetWelcome(DataStreamReader reader)
    {
        opCode = OpCode.WELCOME;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter stream)
    {
        stream.WriteByte((byte)opCode);
        stream.WriteInt(number);
    }
    public override void Deserialize(DataStreamReader stream)
    {
        number = stream.ReadInt();
    }

    public override void ReceivedOnClient()
    {
        Client.instance.playerNumber = number;
        ServerPlayerController spc = (ServerPlayerController)Client.instance.players[number].pc;

        Transform cam = spc.camController.transform.GetChild(0);
        cam.gameObject.SetActive(true);
    }

    public override void ReceivedOnServer(NetworkConnection connection)
    {
        NetWelcome msg = new NetWelcome();
        msg.number = Server.instance.GetFreePlayer();
        Server.instance.SendToClient(connection, msg);
    }

}
