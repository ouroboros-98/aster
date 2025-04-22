using UnityEngine;

namespace Aster.Towers
{
    [System.Serializable]
    public struct SplitterParameters
    {
        [SerializeField] private int   splitCount;
        [SerializeField] private float splitConeAngle;
        [SerializeField] private float spawnOffsetDistance;

        public int   SplitCount          => splitCount;
        public float SplitConeAngle      => splitConeAngle;
        public float SpawnOffsetDistance => spawnOffsetDistance;

        public SplitterParameters(int splitCount, float splitConeAngle, float spawnOffsetDistance)
        {
            this.splitCount          = splitCount;
            this.splitConeAngle      = splitConeAngle;
            this.spawnOffsetDistance = spawnOffsetDistance;
        }

        public static bool operator ==(SplitterParameters left, SplitterParameters right)
        {
            return left.splitCount == right.splitCount
                && Mathf.Approximately(left.splitConeAngle,      right.splitConeAngle)
                && Mathf.Approximately(left.spawnOffsetDistance, right.spawnOffsetDistance);
        }

        public static bool operator !=(SplitterParameters left, SplitterParameters right)
        {
            return !(left == right);
        }
    }
}