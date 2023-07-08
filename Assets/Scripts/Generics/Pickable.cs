using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Pickable : MonoBehaviour
{
    [SerializeField] Ability ability;
    // pagina
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

        // Tryget componet per la pagina
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Character>() != null)
        {
            Character character = collision.gameObject.GetComponent<Character>();

            if (ability != null)
            {
                ability.Pick(character);
                DataSerializer.Save(ability.name, pickedUp);
            }

            // Aggiungere raccolta della pagina con else if

            OnPickup.Invoke();
            gameObject.SetActive(false);
        }
    }
}
