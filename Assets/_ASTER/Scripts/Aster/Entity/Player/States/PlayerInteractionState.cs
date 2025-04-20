using Aster.Entity.StateMachine;

namespace Aster.Entity.Player.States
{
    public class PlayerInteractionState : BaseEntityState
    {
        private readonly PlayerController _player;
        private readonly PlayerInteractor _interactor;

        public PlayerInteractionState(PlayerController player, PlayerInteractor interactor) : base(player)
        {
            _player     = player;
            _interactor = interactor;
        }
    }
}