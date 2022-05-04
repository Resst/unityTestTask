using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayerController : PlayerController
{
    NetPlayer msg;
    NetPlayer toSend;
    public override void Process()
    {
        Send();
        Flush();
    }
    

    private void Send()
    {
        if (Client.instance.playerNumber != player.number || Client.instance.playerNumber == -1)
            return;

        toSend = new NetPlayer();
        //sending inputs and appplying them at the same time
        toSend.playerNumber = Client.instance.playerNumber;

        mouseX = toSend.mouseX = Input.GetAxis("Mouse X");
        mouseY = toSend.mouseY = Input.GetAxis("Mouse Y");

        vertical = toSend.vertical = Input.GetAxis("Vertical");
        isRunning = toSend.isRunning = Input.GetKey(KeyCode.LeftShift) && toSend.vertical > 0;
        spacePressed = toSend.spacePressed = Input.GetKeyDown(KeyCode.Space);


        //sending transform data
        toSend.position.x = player.transform.position.x;
        toSend.position.y = player.transform.position.y;
        toSend.position.z = player.transform.position.z;
        
        toSend.rotation.x = player.transform.rotation.x;
        toSend.rotation.y = player.transform.rotation.y;
        toSend.rotation.z = player.transform.rotation.z;
        toSend.rotation.w = player.transform.rotation.w;

        Client.instance.SendToServer(toSend);
    }

    public void Receive(NetPlayer newData)
    {
        msg = newData;
    }

    private void Flush()
    {
        if (msg != null)
        {
            mouseX = msg.mouseX;
            mouseY = msg.mouseY;

            vertical = msg.vertical;
            isRunning = msg.isRunning;
            spacePressed = msg.spacePressed;

            player.transform.position = msg.position;
            player.transform.rotation = msg.rotation;

            msg = null;
        }
    }
}
