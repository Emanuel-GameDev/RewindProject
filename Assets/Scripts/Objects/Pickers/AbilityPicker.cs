using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AbilityPicker : MonoBehaviour
{
    [SerializeField] private Ability abilityToPick;

    private Animator animator;
    private bool pickedUp = false;
    private Character picker;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (abilityToPick != null)
        {
            if (DataSerializer.TryLoad(abilityToPick.name, out pickedUp))
            {
                Destroy(gameObject);
                return;
            }

            animator.SetTrigger("ability");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Character>() != null)
            picker = collision.gameObject.GetComponent<Character>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Character>() != null)
            picker = null;
    }

    public void Save()
    {
        if (picker == null)
        {
            Debug.LogError("Error: picker is null");
            return;
        }
        else if (abilityToPick == null)
        {
            Debug.LogError("Error: abilityToPick is null, you first need to assign it");
            return;
        }

        List<object> pickData = new List<object> { picker, abilityToPick };

        PubSub.Instance.Notify(EMessageType.AbilityAnimStart, pickData);
        DataSerializer.Save(abilityToPick.name, pickedUp);
    }

    public void Dismiss()
    {
        picker = null;
        abilityToPick = null;
        gameObject.SetActive(false);
    }

}
