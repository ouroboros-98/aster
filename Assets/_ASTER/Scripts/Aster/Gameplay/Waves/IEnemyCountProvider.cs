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
        [SerializeField] private int _enemyCount = 1;

        public int GetEnemyCount()
        {
            return _enemyCount;
        }
    }

    [System.Serializable]
    public class RandomEnemyCount : IEnemyCountProvider
    {
        [SerializeField] private int _min = 1;
        [SerializeField] private int _max = 1;

        public int GetEnemyCount()
        {
            return Random.Range(_min, _max + 1);
        }
    }
}