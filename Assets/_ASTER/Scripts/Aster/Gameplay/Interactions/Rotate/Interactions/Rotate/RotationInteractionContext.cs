using Aster.Entity.Player;
using UnityEngine;

namespace Aster.Core
{
    public class RotationInteractionContext : InteractionContext<IRotatatble>
    {
        public bool Anchor { get; protected set; } = false;

        public RotationInteractionContext(PlayerController player, IRotatatble interactable) :
            base(player, interactable)
        {
        }
    }

    public class AnchorRotationInteractionContext : RotationInteractionContext
    {
        public AnchorRotationInteractionContext(PlayerController player, IRotatatble interactable) :
            base(player, interactable)
        {
            Anchor = true;
        }
    }
}