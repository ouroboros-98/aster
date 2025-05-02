using Aster.Core.Entity;
using UnityEngine;

namespace Aster.Entity.StateMachine
{
    public class EntityAttackState : BaseEntityState
    {
        private EntityAttack _attack;
        private EntityHP _hp;
        public EntityAttackState(BaseEntityController entity) : base(entity)
        {
            _attack = Entity.Attack;
            _hp = Entity.HP;
        }

        public override void Update()
        {
            _attack.HandleAttack(Time.deltaTime, _hp);
        }
    }
}