using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGroundAttack : State
{
    BossBheaviour bossBheaviour;
    Vector3 startPosition;
    Vector3 endPosition;
    float elapsedTime;
    GameObject activeGround;
    GameObject nextGround;
    bool mustMove;

    public ChangeGroundAttack(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Enter()
    {
        startPosition = bossBheaviour.GetCurrentPosition().transform.position;
        endPosition = bossBheaviour.GetCurrentPosition().GetOppositePosition().transform.position;
        elapsedTime = 0f;
        if(bossBheaviour.GetCurrentPosition().GetHorizontalPosition() == eHorizontalPosition.Center)
        {
            mustMove = true;
        }
        else
        {
            mustMove = false;
        }
    }

    public override void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime <= bossBheaviour.GetGroundFadeInDuration())
        {
            GroundFadeIn();
        }
        else if(elapsedTime - bossBheaviour.GetGroundFadeInDuration() <= bossBheaviour.GetGroundShakeDuration())
        {
            GroundShake();
        }
        else if(elapsedTime - bossBheaviour.GetGroundFadeInDuration() - bossBheaviour.GetGroundShakeDuration() <= bossBheaviour.GetGroundFadeOutDuration())
        {
            GroundFadeOut();
        }
        else
        {
            bossBheaviour.ChangeState(eBossState.Start);
        }
    }

    private void GroundShake()
    {
        Debug.Log("GroundShake");
    }

    private void GroundFadeIn()
    {
        Debug.Log("GroundFadeIn");
    }

    private void GroundFadeOut()
    {
        Debug.Log("GroundFadeOut");
    }

}
