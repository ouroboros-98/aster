using System;
using Aster.Entity.Enemy;
using Aster.Entity.Player;
using Aster.Utils.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aster.Core
{
    public partial class AsterConfiguration
    {
        private EntitiesConfiguration entities = new();
        public  EntitiesConfiguration Entities => entities;

        [BoxGroup("Player")]
        [LabelText("Prefab")]
        [PreviewField(50, ObjectFieldAlignment.Left)]
        [AssetsOnly]
        [SerializeField]
        private PlayerController playerPrefab;

        [BoxGroup("Player")]
        [LabelText("Y Offset")]
        [SerializeField]
        [Range(0, 3f)]
        private float playerPivotY = 0.96f;

        [BoxGroup("Player")]
        [LabelText("Rotation Controller Prefab")]
        [SerializeField]
        private PlayerRotationController playerRotationControllerPrefab;

        [BoxGroup("Player")]
        [LabelText("Tower Picker Prefab")]
        [SerializeField]
        private Canvas towerPickerPrefab;

        [BoxGroup("Player")]
        [LabelText("Rotate with Towers")]
        [SerializeField]
        private bool playerRotateWithTowers = false;

        [BoxGroup("Player")]
        [LabelText("Base Rotation Speed")]
        [SerializeField]
        private float playerBaseRotationSpeed = 10f;

        [BoxGroup("Enemy")]
        [LabelText("Prefab")]
        [SerializeField]
        private EnemyController enemyPrefab;

        [Serializable]
        public class EntitiesConfiguration
        {
            [HideInInspector]
            public AsterConfiguration _config;

            public PlayerController         PlayerPrefab                   => _config.playerPrefab;
            public PlayerRotationController PlayerRotationControllerPrefab => _config.playerRotationControllerPrefab;
            public Canvas                   TowerPickerPrefab              => _config.towerPickerPrefab;
            public float                    PlayerPivotY                   => _config.playerPivotY;
            public bool                     PlayerRotateWithTowers         => _config.playerRotateWithTowers;
            public float                    PlayerBaseRotationSpeed        => _config.playerBaseRotationSpeed;

            public EnemyController EnemyPrefab => _config.enemyPrefab;
        }
    }
}