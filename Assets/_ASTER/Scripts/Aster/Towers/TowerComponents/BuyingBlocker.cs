using Aster.Core;
using UnityEngine;
using Aster.Entity.Player;
using Aster.Light;

namespace Aster.Towers
{
    public class BuyingBlocker : BaseLightHittable
    {
        [SerializeField] private GameObject outlineObject;
        [SerializeField] private GameObject towerObject;
        private bool canShowOutline = true;
        protected override void Awake()
        {
            base.Awake();
            AsterEvents.Instance.OnGrabInteractionBegin += SetCanShowOutline;
            AsterEvents.Instance.OnInteractionEnd+= SetTrueCanShowOutline;
        }
        

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Grabber.OnTriggerEntered();
                if(player.Grabber.IsGrabbing&& canShowOutline)
                    outlineObject?.SetActive(true);
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
                if(player.Grabber.IsGrabbing&& canShowOutline)
                    outlineObject.SetActive(false);
            }
        }
        private void SetCanShowOutline(GrabInteractionContext canShow)
        {
            if (canShow.Interactable.GameObject == towerObject)
            {
                canShowOutline=false;
            }
        }
        private void SetTrueCanShowOutline(InteractionContext canShow)
        {
            if (canShow.Interactable.GameObject == towerObject)
            {
                canShowOutline=true;
                outlineObject.SetActive(false);
            }
        }
    }
}