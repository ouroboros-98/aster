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
            
            TestRotatable originalRotatable = Original.GetComponent<TestRotatable>();

            Destroy(duplicate.GetComponent<TestRotatable>());

            TestRotatable rotatable = duplicate.AddComponent<TestRotatable>();

            rotatable.Radius = originalRotatable.Radius;
            rotatable.RotationHandler.RotationSpeed = 0;

            return rotatable;
        }
    }
}