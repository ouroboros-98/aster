using Aster.Entity;
using UnityEngine;

namespace Aster.Entity
{
    [System.Serializable]
    public class EntityAttack
    {
        [SerializeField] public int damage = 1;
        [SerializeField] public float initialTimeToAttack = 3f;
        private float _currentTimeToAttack;
        
        private ITargetAttackProvider _attackProvider;
        private bool IsInitialized => (_attackProvider != null);
        
        public void Init(ITargetAttackProvider attackProvider)
        {
            _attackProvider = attackProvider;
        }


        public void HandleAttack(float time)
        {
            if (!IsInitialized)
            {
                Debug.LogError("EntityAttack is not initialized. Please call Init() before using HandleAttack.");
                return;
            }
            if (_currentTimeToAttack <= 0)
            {
                Debug.Log("attacking");
                _attackProvider.DoAttack(damage);
                _currentTimeToAttack = initialTimeToAttack;

            }
            else
            {
                _currentTimeToAttack -= time;
            }
        }
    }
}