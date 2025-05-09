using UnityEngine;
using Aster.Entity.Player;
using Aster.Light;

namespace Aster.Towers
{
    public class BuyingBlocker : BaseLightHittable
    {
        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Grabber.OnTriggerEntered();
            }
        }

        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            return new LightHitContext(lightHit, blockLight: false);
        }

        private void OnTriggerExit(Collider other)
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Grabber.OnTriggerExited();
            }
        }
    }
}