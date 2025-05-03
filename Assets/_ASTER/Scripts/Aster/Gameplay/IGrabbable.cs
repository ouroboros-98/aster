using Aster.Entity.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Aster.Core
{
    public interface IGrabbable : IInteractable
    {
        public Transform GrabbableTransform { get; }

        void OnGrab();
        void DuringGrab();
        void OnRelease();

        bool IInteractable.CheckInput(PlayerInputHandler input)
        {
            InputAction inputAction = input.Grab;
            return inputAction.WasPressedThisFrame();
        }
    }
}