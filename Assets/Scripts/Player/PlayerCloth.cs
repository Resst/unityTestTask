using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloth : MonoBehaviour
{
    [SerializeField] private GameObject shirt;
    [SerializeField] private GameObject shirtNaked;

    [SerializeField] private GameObject pants;
    [SerializeField] private GameObject pantsNaked;

    [SerializeField] private GameObject boots;
    [SerializeField] private GameObject bootsNaked;

    public void setShirtActive(bool active)
    {
        setPartActive(shirt, shirtNaked, active);
    }

    public void setPantsActive(bool active)
    {
        setPartActive(pants, pantsNaked, active);
    }

    public void setBootsActive(bool active)
    {
        setPartActive(boots, bootsNaked, active);
    }
    private void setPartActive(GameObject obj, GameObject objNaked, bool active)
    {
        obj.SetActive(active);
        objNaked.SetActive(!active);
    }
}
