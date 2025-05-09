using Aster.Entity.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Aster.Core
{
    public interface IRotatatble : IInteractable
    {
        public RotationHandler RotationHandler   { get; }
        public Transform       RotationTransform { get; }
        public float           Radius            { get; }


        bool IInteractable.CheckInput(PlayerInputHandler input)
        {
            InputAction inputAction = input.RotationInteraction;
            return inputAction.WasPressedThisFrame();
        }
    }
}