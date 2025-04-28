using UnityEngine;

namespace Aster.Towers
{
    [System.Serializable]
    public class SplitterParameters : BaseTowerParameters<SplitterParameters>
    {
        [SerializeField] private int   splitCount          = 2;
        [SerializeField] private float splitConeAngle      = 45;
        [SerializeField] private float spawnOffsetDistance = .1f;
        [SerializeField] private bool  refract             = false;
        [SerializeField] private float directionOffset     = 0f;

        public int   SplitCount          => splitCount;
        public float SplitConeAngle      => splitConeAngle;
        public float SpawnOffsetDistance => spawnOffsetDistance;
        public bool  Refract             => refract;
        public float DirectionOffset     => directionOffset;

        public SplitterParameters()
        {
        }

        public override SplitterParameters Clone()
        {
            return new SplitterParameters()
                   {
                       splitCount          = splitCount,
                       splitConeAngle      = splitConeAngle,
                       spawnOffsetDistance = spawnOffsetDistance,
                       refract             = refract,
                       directionOffset     = directionOffset
                   };
        }
    }
}