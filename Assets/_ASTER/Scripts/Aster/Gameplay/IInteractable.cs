using System;
using Aster.Entity.Player;
using UnityEngine.InputSystem;

namespace Aster.Core
{
    public interface IInteractable
    {
        public bool CheckInput(InputHandler input);

        Action<PlayerController> Interact() =>
            player => AsterEvents.Instance.OnInteractionBegin?.Invoke(new(player, this));
    }
}