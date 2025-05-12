using Aster.Entity.Enemy;

namespace Aster.Utils.Pool
{
    public class EnemyPool : AsterPool<EnemyController>
    {
        protected override void OnGet(EnemyController obj)
        {
            obj.Pooled = true;
        }
    }
}