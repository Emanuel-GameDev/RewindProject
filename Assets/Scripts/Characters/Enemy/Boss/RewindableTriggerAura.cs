using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RewindableTriggerAura : MonoBehaviour
{
    bool parryIsActive;
    bool registered = false;
    private void Awake()
    {
        DisactiveParry(null);
        if (!registered)
        {
            registered = true;
            PubSub.Instance.RegisterFunction(EMessageType.ParryStart, ActiveParry);
            PubSub.Instance.RegisterFunction(EMessageType.ParryStop, DisactiveParry);
        }
    }

    public bool GetParryIsActive()
    {
        return parryIsActive;
    }

    public void ActiveParry(object obj)
    {
        parryIsActive = true;
        GetComponent<Light2D>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    public void DisactiveParry(object obj)
    {
        parryIsActive = false;
        GetComponent<Light2D>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

}
