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

            [SerializeField, Label("Rotation Controller Prefab")]
            private PlayerRotationController playerRotationControllerPrefab;

            [SerializeField, Label("Tower Picker Prefab")]
            private Canvas towerPickerPrefab;
            
            [SerializeField, Label("Rotate with Towers"), AllowNesting] private bool playerRotateWithTowers = false;

            [SerializeField, Header("Enemy"), Label("Prefab"), AllowNesting] private EnemyController enemyPrefab;

            public PlayerController         PlayerPrefab                   => playerPrefab;
            public PlayerRotationController PlayerRotationControllerPrefab => playerRotationControllerPrefab;
            public Canvas TowerPickerPrefab => towerPickerPrefab;
            public float                    PlayerPivotY                   => playerPivotY;
            public bool                     PlayerRotateWithTowers         => playerRotateWithTowers;

            public EnemyController EnemyPrefab => enemyPrefab;
        }
    }
}