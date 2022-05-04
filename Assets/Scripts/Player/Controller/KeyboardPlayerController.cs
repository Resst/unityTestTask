using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KeyboardPlayerController : PlayerController
{
    public override void Process()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        vertical = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift) && vertical > 0;
        spacePressed = Input.GetKeyDown(KeyCode.Space);
    }
}