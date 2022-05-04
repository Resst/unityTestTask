using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetMessage
{
    public OpCode opCode;

    public virtual void Serialize(ref DataStreamWriter stream)
    {
        stream.WriteByte((byte)opCode);
    }
    public virtual void Deserialize(DataStreamReader stream)
    {

    }

    public virtual void ReceivedOnClient()
    {

    }
    public virtual void ReceivedOnServer(NetworkConnection connection)
    {

    }


}
