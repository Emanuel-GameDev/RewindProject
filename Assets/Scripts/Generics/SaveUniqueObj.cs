using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveUniqueObj : MonoBehaviour
{
    [SerializeField] bool startingStateActive = true;
    [HideInInspector] public bool objActive;

    private void OnEnable()
    {
        if (!DataSerializer.TryLoad(gameObject.name + "State", out objActive))
            objActive = startingStateActive;

        gameObject.SetActive(objActive);
    }


    public void ChangeObjectStateOnReload(bool state)
    {
        objActive = state;
        DataSerializer.Save(gameObject.name + "State", objActive);
    }
}
