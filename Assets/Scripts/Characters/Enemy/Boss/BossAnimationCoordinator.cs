using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationCoordinator : MonoBehaviour
{
    [SerializeField] GameObject watchingEye;
    [SerializeField] GameObject animationSprite;
    [SerializeField] SpriteRenderer fogSprite;
    [SerializeField] Material hitMaterial;
    [SerializeField] float hitDuration = 0.3f;
    Material oldMaterial;

    private void Start()
    {
        oldMaterial = animationSprite.GetComponent<SpriteRenderer>().material;
    }

    public void SetFogAlpha0()
    {
        Color color = fogSprite.color;
        color.a = 0;
        fogSprite.color = color;
    }
    public void SetFogAlpha100()
    {
        Color color = fogSprite.color;
        color.a = 200;
        fogSprite.color = color;
    }

    public void DisableWatchingEye()
    {
        watchingEye.SetActive(false);
    }

    public void EnableWatchingEye()
    {
        watchingEye.SetActive(true);
    }

    public void EnableAnimationSprite()
    {
        animationSprite.SetActive(true);
    }

    public void DisableAnimationSprite() 
    {
        animationSprite.SetActive(false);
    }

    public void ChangeColor()
    {
        StartCoroutine(ChangeColorCoroutine());
    }

    IEnumerator ChangeColorCoroutine()
    {
        animationSprite.GetComponent<SpriteRenderer>().material = hitMaterial;
        yield return new WaitForSeconds(hitDuration);
        animationSprite.GetComponent<SpriteRenderer>().material = oldMaterial;
    }

}
