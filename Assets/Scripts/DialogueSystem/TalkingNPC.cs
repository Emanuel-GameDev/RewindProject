using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingNPC : MonoBehaviour
{
    

    public void DespawnOnReload()
    {
        GetComponent<SaveObjState>().ChangeObjectStateOnReload(false);
    }
}
