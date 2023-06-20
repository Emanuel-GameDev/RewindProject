using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMessageType
{
    CameraSwitch,
    CheckpointVisited,
    ActiveAbilityChanged,
    AbilityPicked,
    TimeRewindStart,
    TimeRewindStop
}

public enum EAbilityState
{
    ready,
    active,
    cooldown
}



