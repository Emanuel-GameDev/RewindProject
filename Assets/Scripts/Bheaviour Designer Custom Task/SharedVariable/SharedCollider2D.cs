using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedCollider2D : SharedVariable<Collider2D>
    {
        public static implicit operator SharedCollider2D(Collider2D value) { return new SharedCollider2D { mValue = value }; }
    }
}
