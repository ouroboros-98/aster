using Aster.Entity;

namespace _ASTER.Scripts.Aster.Entity.StateMachine
{
    public class EntityMoveState : BaseEntityState
    {
        private EntityMovement _movement;

        public EntityMoveState(BaseEntityController entity) : base(entity)
        {
            _movement = Entity.Movement;
        }

        public override void FixedUpdate()
        {
            _movement.HandleMovement();
        }
    }
}