using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMessageType
{
    CameraSwitch,
    CheckpointVisited,
    ActiveAbilityChanged,
    AbilityPicked,
    AbilityAnimStart,
    TimeRewindStart,
    TimeRewindStop
}

public enum EAbilityState
{
    ready,
    active,
    cooldown
}



