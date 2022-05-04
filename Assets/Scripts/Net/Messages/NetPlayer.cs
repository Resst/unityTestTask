using Unity.Networking.Transport;
using UnityEngine;


public class NetPlayer : NetMessage
{
    public int playerNumber;

    //keyboard input
    public float vertical;
    public bool isRunning;
    public bool spacePressed;

    //mouse inputs
    public float mouseX, mouseY;

    //player transform data
    public Vector3 position = new Vector3();
    public Quaternion rotation = new Quaternion();

    public NetPlayer()
    {
        opCode = OpCode.PLAYER_INPUT;
    }

    public NetPlayer(DataStreamReader reader)
    {
        opCode = OpCode.PLAYER_INPUT;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter stream)
    {
        stream.WriteByte((byte)opCode);

        stream.WriteInt(playerNumber);

        stream.WriteFloat(vertical);
        stream.WriteByte((byte)(isRunning ? 1 : 0));
        stream.WriteByte((byte)(spacePressed ? 1 : 0));

        stream.WriteFloat(mouseX);
        stream.WriteFloat(mouseY);

        stream.WriteFloat(position.x);
        stream.WriteFloat(position.y);
        stream.WriteFloat(position.z);

        stream.WriteFloat(rotation.x);
        stream.WriteFloat(rotation.y);
        stream.WriteFloat(rotation.z);
        stream.WriteFloat(rotation.w);
    }
    public override void Deserialize(DataStreamReader stream)
    {
        playerNumber = stream.ReadInt();

        vertical = stream.ReadFloat();
        isRunning = stream.ReadByte() == 1;
        spacePressed = stream.ReadByte() == 1;

        mouseX = stream.ReadFloat();
        mouseY = stream.ReadFloat();

        position.x = stream.ReadFloat();
        position.y = stream.ReadFloat();
        position.z = stream.ReadFloat();

        rotation.x = stream.ReadFloat();
        rotation.y = stream.ReadFloat();
        rotation.z = stream.ReadFloat();
        rotation.w = stream.ReadFloat();
    }

    public override void ReceivedOnClient()
    {
        if (playerNumber == Client.instance.playerNumber)
            return;
        ServerPlayerController spc = (ServerPlayerController)Client.instance.players[playerNumber].pc;
        spc.Receive(this);
    }
    public override void ReceivedOnServer(NetworkConnection connection)
    {
        Server.instance.Broadcast(this);
    }
}
