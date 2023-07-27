using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Pickable : MonoBehaviour
{
    private Character character;
    [SerializeField] Ability ability;
    private Animator animator;

    public UnityEvent OnPickup;
    bool pickedUp = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (ability != null)
        {
            if (DataSerializer.TryLoad(ability.name, out pickedUp))
            {
                Destroy(gameObject);
                return;
            }

            animator.SetTrigger("ability");
        }
        else
        {
            //animator.SetTrigger("diary");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Character>() != null)
        {
            character = collision.gameObject.GetComponent<Character>();

            if (ability != null)
            {
                List<object> pickData = new List<object> { character, ability };

                PubSub.Instance.Notify(EMessageType.AbilityAnimStart, pickData);
                DataSerializer.Save(ability.name, pickedUp);
            }
            else
            {
            }
            OnPickup.Invoke();
            gameObject.SetActive(false);
        }
    }
}
