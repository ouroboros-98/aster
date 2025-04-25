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

        public static bool operator ==(SplitterParameters left, SplitterParameters right)
        {
            return left.splitCount == right.splitCount
                && Mathf.Approximately(left.splitConeAngle,      right.splitConeAngle)
                && Mathf.Approximately(left.spawnOffsetDistance, right.spawnOffsetDistance)
                && left.refract == right.refract;
        }

        public static bool operator !=(SplitterParameters left, SplitterParameters right)
        {
            return !(left == right);
        }
    }
}