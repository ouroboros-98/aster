using Aster.Entity.Enemy;
using Aster.Entity.Player;
using Aster.Towers;
using Aster.Towers.Destabilizer;
using Aster.Utils.Attributes;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Core
{
    [CreateAssetMenu(fileName = "Configuration", menuName = "Aster/Configuration")]
    public partial class AsterConfiguration : ScriptableObject
    {
        [SerializeField] private float lightRayYPosition = 0.3670001f;
        public                   float LightRayYPosition => lightRayYPosition;
    }
}