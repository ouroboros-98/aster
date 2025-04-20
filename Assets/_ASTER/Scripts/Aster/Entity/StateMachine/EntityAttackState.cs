using UnityEngine;

namespace Aster.Entity.StateMachine
{
    public class EntityAttackState : BaseEntityState
    {
        private EntityAttack _attack;
        public EntityAttackState(BaseEntityController entity) : base(entity)
        {
            _attack = Entity.Attack;
        }

        public override void Update()
        {
            _attack.HandleAttack(Time.deltaTime);
        }
    }
}