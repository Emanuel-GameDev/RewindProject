using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ChangeMainThemeTrigger : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] bool mustRevertToMainTheme;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(targetLayer.value, 2f))) 
        {
            if(mustRevertToMainTheme)
            {
                AudioManager.Instance.ResetMainTheme();
            }
            else if (audioClip != null)
            {
                AudioManager.Instance.ChangeMainTheme(audioClip);
            }
        }
   
    }

}
