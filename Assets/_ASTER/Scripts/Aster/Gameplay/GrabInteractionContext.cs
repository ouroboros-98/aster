using Aster.Entity.Player;
using UnityEngine;

namespace Aster.Core
{
    public class GrabInteractionContext : InteractionContext<IGrabbable>
    {

        public GrabInteractionContext(PlayerController player, IGrabbable interactable) : base(player, interactable)
        {
        }
    }
}