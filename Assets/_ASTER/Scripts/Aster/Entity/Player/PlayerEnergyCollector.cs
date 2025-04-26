using Aster.Core;
using Aster.Light;
using UnityEngine;

namespace Aster.Entity.Player
{
    public class PlayerEnergyCollector : AsterMono
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out LightPoint lightPoint)) return;

            lightPoint.Collect();
        }
    }
}