using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private string connectionIp;

    //Main menu
    public void HostButton()
    {
        Variables.isHost = true;
        Variables.connectionIp = "127.0.0.1";
        SceneManager.LoadScene(Variables.gameSceneName);
    }
    public void ConnectButton()
    {
        Variables.isHost = false;
        Variables.connectionIp = connectionIp;
        if (connectionIp == null || connectionIp == "")
            Variables.connectionIp = "127.0.0.1";
        Debug.Log(connectionIp);
        SceneManager.LoadScene(Variables.gameSceneName);
    }
    public void ConnectionText(TMPro.TMP_InputField f)
    {
        connectionIp = f.text;
    }

    //Game Scene
    public void mouseSensivity(Slider s)
    {
        Variables.mouseSensivity = s.value;
    }
    public void toggleShirt(Toggle toggle)
    {
        bool newValue = toggle.isOn;
        Debug.Log(newValue);
        Client.instance.players[Client.instance.playerNumber].cloth.setShirtActive(newValue);

        NetCloth msg = new NetCloth();
        msg.playerNumber = Client.instance.playerNumber;
        msg.operationType = newValue ? NetCloth.ClothOperationType.SHIRT_ON : NetCloth.ClothOperationType.SHIRT_OFF;
        Client.instance.SendToServer(msg);
    }
    public void togglePants(Toggle toggle)
    {
        bool newValue = toggle.isOn;
        Client.instance.players[Client.instance.playerNumber].cloth.setPantsActive(newValue);

        NetCloth msg = new NetCloth();
        msg.playerNumber = Client.instance.playerNumber;
        msg.operationType = newValue ? NetCloth.ClothOperationType.PANTS_ON : NetCloth.ClothOperationType.PANTS_OFF;
        Client.instance.SendToServer(msg);
    }
    public void toggleBoots(Toggle toggle)
    {
        bool newValue = toggle.isOn;
        Client.instance.players[Client.instance.playerNumber].cloth.setBootsActive(newValue);

        NetCloth msg = new NetCloth();
        msg.playerNumber = Client.instance.playerNumber;
        msg.operationType = newValue ? NetCloth.ClothOperationType.BOOTS_ON : NetCloth.ClothOperationType.BOOTS_OFF;
        Client.instance.SendToServer(msg);
    }
}
