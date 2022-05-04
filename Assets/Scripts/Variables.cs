using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables : MonoBehaviour
{
    public static float mouseSensivity = 50;
    public static bool isHost;
    public static ushort port = 7777;
    public static string connectionIp = "127.0.0.1";

    public static readonly string gameSceneName = "GameScene";
}
