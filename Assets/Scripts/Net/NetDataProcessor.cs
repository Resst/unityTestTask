using Unity.Networking.Transport;
public static class NetDataProcessor
{
    public static void ProcessData(DataStreamReader stream, NetworkConnection connection, bool isServer = false)
    {
        NetMessage msg = null;
        OpCode opCode = (OpCode)stream.ReadByte();
        switch (opCode)
        {
            case OpCode.PLAYER_INPUT:
                msg = new NetPlayer(stream);
                break;
            case OpCode.WELCOME:
                msg = new NetWelcome(stream);
                break;
            case OpCode.CLOTH_CHANGE:
                msg = new NetCloth(stream);
                break;
        }

        if (isServer)
            msg.ReceivedOnServer(connection);
        else
            msg.ReceivedOnClient();
    }
}

public enum OpCode
{
    PLAYER_INPUT = 1,
    WELCOME = 2,
    CLOTH_CHANGE = 3
}


