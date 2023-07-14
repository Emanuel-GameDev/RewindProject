using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D playerCollider;
    private bool coroutineRunning = false;

    [SerializeField] private bool playerCanGoDown = true;

    [Tooltip("Changes time for the ignore collision player-platform")]
    [SerializeField] private float snapOffset = 1f;

    private PlayerInputs inputs;


    private void Start()
    {
        inputs = PlayerController.instance.inputs;

        inputs.Player.Down.performed += StartPlatformDisabling;
    }

    private void OnDisable()
    {

        inputs.Player.Down.performed -= StartPlatformDisabling;
    }

    private void StartPlatformDisabling(InputAction.CallbackContext obj)
    {
        if (playerCanGoDown && !coroutineRunning && playerCollider != null && 
            !playerCollider.gameObject.GetComponent<PlayerController>().isJumping)
        {
            Debug.Log("lkawjsf");
            StartCoroutine(DisablePlatform());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Character>() != null
            && !coroutineRunning)
        {
            Character character = collision.gameObject.GetComponent<Character>();
            playerCollider = character.gameObject.GetComponent<Collider2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Character>() != null
            && !coroutineRunning)
        {
            playerCollider = null;
        }
    }

    private IEnumerator DisablePlatform()
    {
        coroutineRunning = true;
        BoxCollider2D platCollider = GetComponent<BoxCollider2D>();

        // Disattivo collisione
        if (playerCollider != null && platCollider != null)
            Physics2D.IgnoreCollision(playerCollider, platCollider);

        yield return new WaitForSeconds(snapOffset);

        // Riattivo collisione
        if (playerCollider != null && platCollider != null)
            Physics2D.IgnoreCollision(playerCollider, platCollider, false);

        coroutineRunning = false;

    }
}
