using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetCloth : NetMessage
{
    public int playerNumber;
    public ClothOperationType operationType;

    public NetCloth()
    {
        opCode = OpCode.CLOTH_CHANGE;
    }

    public NetCloth(DataStreamReader stream)
    {
        opCode = OpCode.CLOTH_CHANGE;
        Deserialize(stream);
    }

    public override void Serialize(ref DataStreamWriter stream)
    {
        stream.WriteByte((byte)opCode);
        stream.WriteInt(playerNumber);
        stream.WriteByte((byte)operationType);
    }

    public override void Deserialize(DataStreamReader stream)
    {
        playerNumber = stream.ReadInt();
        operationType = (ClothOperationType)stream.ReadByte();
    }

    public override void ReceivedOnClient()
    {
        if (playerNumber == Client.instance.playerNumber)
            return;

        PlayerCloth c = Client.instance.players[playerNumber].cloth;
        switch (operationType)
        {
            case ClothOperationType.SHIRT_ON:
                c.setShirtActive(true);
                break;
            case ClothOperationType.SHIRT_OFF:
                c.setShirtActive(false);
                break;
            case ClothOperationType.PANTS_ON:
                c.setPantsActive(true);
                break;
            case ClothOperationType.PANTS_OFF:
                c.setPantsActive(false);
                break;
            case ClothOperationType.BOOTS_ON:
                c.setBootsActive(true);
                break;
            case ClothOperationType.BOOTS_OFF:
                c.setBootsActive(false);
                break;
        }
    }

    public override void ReceivedOnServer(NetworkConnection connection)
    {
        Server.instance.Broadcast(this);
    }

    public enum ClothOperationType
    {
        SHIRT_ON = 0,
        SHIRT_OFF = 1,
        PANTS_ON = 2,
        PANTS_OFF = 3,
        BOOTS_ON = 4,
        BOOTS_OFF = 5
    }
}
