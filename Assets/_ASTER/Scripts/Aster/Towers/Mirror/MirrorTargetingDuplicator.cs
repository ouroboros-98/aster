using Aster.Core;
using NUnit.Framework.Internal.Commands;
using UnityEngine;

namespace Aster.Towers
{
    public class MirrorTargetingDuplicator : TargetingTowerDuplicator<Mirror>
    {
        protected override IRotatatble ConfigureRotatable(GameObject duplicate)
        {
            Debug.Log("Configuring rotatable", this);
            
            BaseRotatable originalRotatable = Original.GetComponent<BaseRotatable>();

            Destroy(duplicate.GetComponent<BaseRotatable>());

            BaseRotatable rotatable = duplicate.AddComponent<BaseRotatable>();

            rotatable.Radius = originalRotatable.Radius;
            rotatable.RotationHandler.RotationSpeed = 0;

            return rotatable;
        }
    }
}