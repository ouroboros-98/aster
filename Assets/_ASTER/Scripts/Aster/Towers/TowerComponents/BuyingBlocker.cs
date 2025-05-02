using UnityEngine;
using Aster.Entity.Player;
using Aster.Light;

namespace Aster.Towers
{
    public class BuyingBlocker : BaseLightHittable
    {
        private void OnTriggerEnter(Collider other)
        {
            var towerBuying = other.GetComponent<TowerBuying>();
            if (towerBuying != null)
            {
                towerBuying.OnTriggerEntered();
            }
        }
        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            return new LightHitContext(lightHit, blockLight: false);
        }

        private void OnTriggerExit(Collider other)
        {
            var towerBuying = other.GetComponent<TowerBuying>();
            if (towerBuying != null)
            {
                towerBuying.OnTriggerExited();
            }
        }
    }
}