using Aster.Entity.Player;

namespace Aster.Core
{
    public class RotationInteractionContext : InteractionContext<IRotatatble>
    {
        public RotationInteractionContext(PlayerController player, IRotatatble interactable) :
            base(player, interactable)
        {
        }
    }
}