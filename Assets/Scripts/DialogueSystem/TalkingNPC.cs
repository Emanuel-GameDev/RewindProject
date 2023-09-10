using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingNPC : MonoBehaviour
{





    public void SpawnOnReload()
    {
        GetComponent<SaveObjState>().ChangeObjectStateOnReload(true);
    }

    public void DespawnOnReload()
    {
        GetComponent<SaveObjState>().ChangeObjectStateOnReload(false);
    }
}
