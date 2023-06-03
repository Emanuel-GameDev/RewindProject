using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private Ability ability;
    // pagina

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (TryGetComponent(out ability))
        {
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
                ability.Pick(character);

            // Aggiungere raccolta della pagina con else if
        }
    }
}
