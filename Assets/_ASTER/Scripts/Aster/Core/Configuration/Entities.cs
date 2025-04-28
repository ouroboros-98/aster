using Aster.Entity.Enemy;
using Aster.Entity.Player;
using Aster.Utils.Attributes;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Core
{
    public partial class AsterConfiguration
    {
        [SerializeField, BoxedProperty] private EntitiesConfiguration entities;
        public                                  EntitiesConfiguration Entities => entities;

        [System.Serializable]
        public class EntitiesConfiguration
        {
            [SerializeField, BoxGroup("Player"), Label("Prefab")] private PlayerController playerPrefab;
            [SerializeField, BoxGroup("Enemy"), Label( "Prefab")] private EnemyController  enemyPrefab;

            [SerializeField, BoxGroup("Player"), Label("Player Flight Y")] private float playerPivotY = 0.96f;

            [SerializeField, BoxGroup("Player"), Label("Player Rotate with Towers")]
            private bool playerRotateWithTowers = false;

            public PlayerController PlayerPrefab           => playerPrefab;
            public float            PlayerPivotY           => playerPivotY;
            public bool             PlayerRotateWithTowers => playerRotateWithTowers;

            public EnemyController EnemyPrefab => enemyPrefab;
        }
    }
}