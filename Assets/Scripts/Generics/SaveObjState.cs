using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveObjState : MonoBehaviour
{
    [SerializeField] bool startingStateActive =true;
    bool objActive;

    private void OnEnable()
    {
        if (!DataSerializer.TryLoad(SceneManager.GetActiveScene().name + name + "State", out objActive))
            objActive = startingStateActive;

        gameObject.SetActive(objActive);
    }


    public void ChangeObjectStateOnReload(bool state)
    {
        objActive = state;
        DataSerializer.Save(SceneManager.GetActiveScene().name + name + "State", objActive);
    }

}
