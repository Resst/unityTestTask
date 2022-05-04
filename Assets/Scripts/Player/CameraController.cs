using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float xSensivityScale = 10;

    private float minAngleX = -10;
    private float maxAngleX = 60;
    private Quaternion maxAngle;
    private Quaternion minAngle;

    private Transform camCenter;
    private Transform player;

    //received from controller
    private float mouseX, mouseY;
    void Start()
    {
        maxAngle = Quaternion.Euler(maxAngleX, 0, 0);
        minAngle = Quaternion.Euler(minAngleX, 0, 0);

        camCenter = transform;
        player = camCenter.parent;

    }

    void Update()
    {
        Scroll();
        HorizontalRot();
        VerticalRot();
    }

    void Scroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Transform t = transform.GetChild(0);
        t.Translate(0, 0, scroll * xSensivityScale);

        if (t.localPosition.z > -1)
            t.Translate(0, 0, -t.localPosition.z - 1);

        if(t.localPosition.z < -10)
        {
            t.Translate(0, 0, -t.localPosition.z - 10);
        }

    }

    void HorizontalRot()
    {
        float x = mouseX * Variables.mouseSensivity * xSensivityScale * Time.deltaTime;
        player.Rotate(Vector3.up, x);
    }
    void VerticalRot()
    {
        float y = mouseY * Variables.mouseSensivity * Time.deltaTime;
        Quaternion q;
        if (y > 0)
            q = Quaternion.Lerp(camCenter.localRotation, maxAngle, y);
        else
            q = Quaternion.Lerp(camCenter.localRotation, minAngle, -y);

        camCenter.localRotation = q;
    }

    public void receiveData(float x, float y)
    {
        this.mouseX = x;
        this.mouseY = y;
    }
}
