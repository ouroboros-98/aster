using _ASTER.Scripts.Aster.Entity.StateMachine;
using Aster.Entity;

namespace Aster.Entity.Player
{
    public class PlayerController : BaseEntityController
    {
        private ITargetMovementProvider movementProvider;

        protected override void Awake()
        {
            base.Awake();

            movementProvider = new PlayerMovementProvider();
            movement.Init(rb, movementProvider);
        }

        protected override void SetupStateMachine()
        {
            StateMachine = new();

            var moveState = new EntityMoveState(this);

            At(moveState, moveState, When(() => false));

            StateMachine.SetState(moveState);
        }
    }
}