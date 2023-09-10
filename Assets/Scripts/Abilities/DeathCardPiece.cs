using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;

public class DeathCardPiece : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] string text = "Frammento di carta raccolto";

    Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        

        animator.SetTrigger("ability");
        
    }

    public void Pick()
    {

        GameManager.Instance.uiManager.StartShowPickableAnimation(sprite, text);


        DataSerializer.TryLoad("DeathAbilityPieces",out int numPieces);

        
        DataSerializer.Save("DeathAbilityPieces", numPieces + 1 );

        Destroy(gameObject);
    }


}
