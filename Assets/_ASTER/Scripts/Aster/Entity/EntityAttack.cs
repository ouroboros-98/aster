using Aster.Core;
using Aster.Core.Entity;
using Aster.Entity;
using Aster.Entity.Enemy;
using Aster.Utils.Pool;
using UnityEngine;

namespace Aster.Entity
{
    [System.Serializable]
    public class EntityAttack
    {
        [SerializeField] public  int             damage               = 1;
        [SerializeField] public  float           initialTimeToAttack  = 3f;
        [SerializeField] private float           damageTakenPerSecond = 0.2f;
        private                  float           _currentTimeToAttack;
        private                  EnemyController _controller;

        private ITargetAttackProvider _attackProvider;
        private bool                  IsInitialized => (_attackProvider != null);

        public void Init(ITargetAttackProvider attackProvider, EnemyController controller)
        {
            _attackProvider = attackProvider;
            _controller     = controller;
        }


        public void HandleAttack(float time, EntityHP hp)
        {
            if (!IsInitialized)
            {
                Debug.LogError("EntityAttack is not initialized. Please call Init() before using HandleAttack.");
                return;
            }

            if (_currentTimeToAttack <= 0)
            {
                Debug.Log("attacking");
                SoundManager.Instance.Play("EnemyHit", true);
                _attackProvider.DoAttack(damage);
                _currentTimeToAttack = initialTimeToAttack;
            }
            else
            {
                _currentTimeToAttack -= time;
            }

            hp.ChangeBy(-damageTakenPerSecond * time);
            Debug.Log("damged");
            if (hp <= 0)
            {
                AsterEvents.Instance.OnEnemyDeath?.Invoke(_controller);
                EnemyPool.Instance.Return(_controller);
            }
        }
    }
}