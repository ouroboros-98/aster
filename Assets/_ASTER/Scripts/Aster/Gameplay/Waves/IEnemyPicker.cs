using System;
using Aster.Core;
using Aster.Entity.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    public interface IEnemyPicker
    {
        public EnemyController GetEnemy();
    }

    [Serializable]
    [InlineProperty]
    public class EnemyPicker : IEnemyPicker
    {
        [SerializeField]
        [AssetList(Path = "_ASTER/Prefabs/Entity")]
        private EnemyController enemy;

        public EnemyController GetEnemy()
        {
            if (enemy == null)
            {
                enemy = AsterConfiguration.Instance.Entities.EnemyPrefab;
            }

            return enemy;
        }
    }
}