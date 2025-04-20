using Aster.Core;
using Aster.Entity;

namespace _ASTER.Scripts.Aster.Entity.Enemy
{
    public class PrimitiveEnemyAttackProvider : ITargetAttackProvider
    {
        public void DoAttack(int damage)
        {
            AsterEvents.Instance.OnAttackLightSource?.Invoke(damage);
        }
    }
}