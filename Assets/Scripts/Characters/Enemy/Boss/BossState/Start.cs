using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Start : State
{
    BossBheaviour bossBheaviour;

    public Start(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Update()
    {
    }

    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        PubSub.Instance.RegisterFunction(EMessageType.SpawnBoss, Spawn);
        PubSub.Instance.RegisterFunction(EMessageType.BossfightStart, StartCombat);
    }

    private void Spawn (object obj)
    {
        bossBheaviour.SpawnTrigger();
    }

    private void StartCombat (object obj)
    {
        bossBheaviour.ExitNoiaTrigger();
    }

    public override void Exit()
    {
        base.Exit();
        PubSub.Instance.UnregisterFunction(EMessageType.SpawnBoss, Spawn);
        PubSub.Instance.UnregisterFunction(EMessageType.BossfightStart, StartCombat);
    }

}
