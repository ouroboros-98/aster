using Sirenix.OdinInspector;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    public interface IEnemyCountProvider
    {
        int GetEnemyCount();
    }

    [System.Serializable]
    public class ConstantEnemyCount : IEnemyCountProvider
    {
        [SerializeField]
        private int _enemyCount = 1;

        public ConstantEnemyCount(int enemyCount)
        {
            _enemyCount = enemyCount;
        }

        public ConstantEnemyCount() : this(1)
        {
        }

        public int GetEnemyCount()
        {
            return _enemyCount;
        }
    }

    [System.Serializable]
    public class RandomEnemyCount : IEnemyCountProvider
    {
        [SerializeField]
        [MinMaxSlider(1, 50, true)]
        [HideLabel]
        private Vector2Int _range = new(1, 10);

        public int GetEnemyCount()
        {
            return Random.Range(_range.x, _range.y + 1);
        }
    }
}