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
            [SerializeField, Header("Player"), Label("Prefab"), AllowNesting] private PlayerController playerPrefab;

            [SerializeField, Label("Y Offset"), AllowNesting] private float playerPivotY = 0.96f;

            [SerializeField, Label("Rotate with Towers"), AllowNesting] private bool playerRotateWithTowers = false;

            [SerializeField, Header("Enemy"), Label("Prefab"), AllowNesting] private EnemyController enemyPrefab;

            public PlayerController PlayerPrefab           => playerPrefab;
            public float            PlayerPivotY           => playerPivotY;
            public bool             PlayerRotateWithTowers => playerRotateWithTowers;

            public EnemyController EnemyPrefab => enemyPrefab;
        }
    }
}