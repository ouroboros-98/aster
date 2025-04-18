using Aster.Entity.Player;

namespace Aster.Core
{
    public class InteractionContext<T> : InteractionContext where T : IInteractable
    {
        public new readonly T Interactable;

        public InteractionContext(PlayerController player, T interactable) : base(player, interactable)
        {
            Interactable = interactable;
        }
    }

    public class InteractionContext
    {
        public readonly PlayerController Player;
        public readonly IInteractable    Interactable;

        public InteractionContext(PlayerController player, IInteractable interactable)
        {
            Player       = player;
            Interactable = interactable;
        }

        public virtual void OnBegin()
        {
        }

        public virtual void OnEnd()
        {
        }
    }
}