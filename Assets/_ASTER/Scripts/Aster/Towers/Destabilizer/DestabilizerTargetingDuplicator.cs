using Aster.Core;
using UnityEngine;

namespace Aster.Towers.Destabilizer
{
    public class DestabilizerTargetingDuplicator : TargetingTowerDuplicator<Destabilizer>
    {
        protected override IRotatatble ConfigureRotatable(GameObject duplicate)
        {
            BaseRotatable originalRotatable = Original.GetComponent<BaseRotatable>();

            Destroy(duplicate.GetComponent<BaseRotatable>());

            BaseRotatable rotatable = duplicate.AddComponent<BaseRotatable>();

            rotatable.Radius                        = originalRotatable.Radius;
            rotatable.RotationHandler.RotationSpeed = 0;

            return rotatable;
        }
    }
}