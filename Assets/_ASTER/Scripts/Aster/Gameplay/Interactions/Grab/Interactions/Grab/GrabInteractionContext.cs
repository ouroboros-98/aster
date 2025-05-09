using Aster.Entity.Player;
using UnityEngine;

namespace Aster.Core
{
    public class GrabInteractionContext : InteractionContext<IGrabbable>
    {
        public readonly Vector3 Offset;

        public GrabInteractionContext(PlayerController player, IGrabbable interactable) : base(player, interactable)
        {
            Offset = interactable.GrabbableTransform.position - player.transform.position;
        }
    }
}