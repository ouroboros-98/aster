using UnityEngine;

namespace _ASTER.Scripts.Aster.Entity
{
    public interface ITargetMovementProvider
    {
        public Vector2 TargetMovement { get; }
    }
}