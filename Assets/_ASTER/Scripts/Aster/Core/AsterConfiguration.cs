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
        private static AsterConfiguration _instance;

        public static AsterConfiguration Instance
        {
            get
            {
                if (_instance == null) _instance = Resources.Load<AsterConfiguration>("Configuration");

                return _instance;
            }
        }

        [SerializeField] private float lightRayYPosition = 0.3670001f;

        public float LightRayYPosition => lightRayYPosition;


        [SerializeField, BoxedProperty, Dropdown("UI")] private TowerOptionsAnimator.Configuration towerOptionsAnimator;
        public TowerOptionsAnimator.Configuration TowerOptionsAnimator => towerOptionsAnimator;
    }
}