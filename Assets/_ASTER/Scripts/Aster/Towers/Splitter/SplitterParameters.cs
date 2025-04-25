using UnityEngine;

namespace Aster.Towers
{
    [System.Serializable]
    public struct SplitterParameters
    {
        [SerializeField] private int   splitCount;
        [SerializeField] private float splitConeAngle;
        [SerializeField] private float spawnOffsetDistance;
        [SerializeField] private bool  refract;
        [SerializeField] private float directionOffset;

        public int   SplitCount          => splitCount;
        public float SplitConeAngle      => splitConeAngle;
        public float SpawnOffsetDistance => spawnOffsetDistance;
        public bool  Refract             => refract;
        public float DirectionOffset     => directionOffset;

        public SplitterParameters(int   splitCount, float splitConeAngle, float spawnOffsetDistance, bool refract,
                                  float directionOffset)
        {
            this.splitCount          = splitCount;
            this.splitConeAngle      = splitConeAngle;
            this.spawnOffsetDistance = spawnOffsetDistance;
            this.refract             = refract;
            this.directionOffset     = directionOffset;
        }
    }
}