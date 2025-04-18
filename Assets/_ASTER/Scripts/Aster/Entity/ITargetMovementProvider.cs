using UnityEngine;

namespace Aster.Entity
{
    public interface ITargetMovementProvider
    {
        public Vector2 TargetMovement { get; }
    }
}