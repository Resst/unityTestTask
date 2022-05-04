using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    public Player player;
    public CameraController camController;

    protected float vertical;
    protected bool isRunning;
    protected bool spacePressed;
    protected float mouseX, mouseY;

    void Update()
    {
        Process();
        SendToPlayer();
    }

    public abstract void Process();

    public void SendToPlayer()
    {
        player.receiveData(vertical, isRunning, spacePressed);
        camController.receiveData(mouseX, mouseY);
    }
}
