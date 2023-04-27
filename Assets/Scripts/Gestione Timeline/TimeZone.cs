using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TimeZone 
{
    [SerializeField] public eZone zone;
    [SerializeField] public PlayableAsset timeline;
    public float actualTime { get; private set; } = 0;

    public void SetActualTime(float time)
    {
        actualTime = time;
    }
}

public enum eZone
{
    A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
}