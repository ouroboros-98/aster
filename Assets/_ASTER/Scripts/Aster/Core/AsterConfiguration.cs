using System;
using Aster.Entity.Enemy;
using Aster.Entity.Player;
using Aster.Towers;
using Aster.Towers.Destabilizer;
using Aster.UI;
using Aster.Utils.Attributes;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Core
{
    [CreateAssetMenu(fileName = "Configuration", menuName = "Aster/Configuration")]
    public partial class AsterConfiguration : ScriptableObject
    {
        private static AsterConfiguration _instance = null;

        public static AsterConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    AsterConfiguration[] loaded = Resources.LoadAll<AsterConfiguration>("");
                    Debug.Log($"Loaded {loaded.Length} AsterConfiguration");
                    _instance = loaded[0];
                    _instance.OnValidate();
                }

                return _instance;
            }
        }

        [SerializeField]
        private bool enableTitleScreen = true;

        [SerializeField]
        private float lightRayYPosition = 0.3670001f;

        public bool  EnableTitleScreen => enableTitleScreen;
        public float LightRayYPosition => lightRayYPosition;

        [SerializeField, BoxedProperty]
        private TowerOptionsAnimator.Configuration towerOptionsAnimator;

        public TowerOptionsAnimator.Configuration TowerOptionsAnimator => towerOptionsAnimator;

        private void OnValidate()
        {
            if (entities._config  == null) entities._config  = this;
            if (lightrays._config == null) lightrays._config = this;
            if (targeting._config == null) targeting._config = this;
            if (towers._config    == null) towers._config    = this;
            if (_uiConfig._config == null) _uiConfig._config = this;
        }
    }
}