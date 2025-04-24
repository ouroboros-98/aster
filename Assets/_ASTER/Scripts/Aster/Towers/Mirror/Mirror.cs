using Aster.Light;
using Unity.VisualScripting;
using UnityEngine;

namespace Aster.Towers
{
    public class Mirror : BaseTower
    {
        private         MirrorLightReceiver _lightReceiver;
        public override LightReceiver       LightReceiver => _lightReceiver;

        private MirrorManipulator _mirrorManipulator;
        private Vector3           MirrorNormal => transform.forward;

        private void Awake()
        {
            _lightReceiver     = new MirrorLightReceiver(transform);
            _mirrorManipulator = new MirrorManipulator(this);
        }
    }

    [System.Serializable]
    public class MirrorLightReceiver : LightReceiver
    {
        private Transform _mirrorTransform;

        public MirrorLightReceiver(Transform mirrorTransform)
        {
            _mirrorTransform = mirrorTransform;
        }

        private Vector3 MirrorNormal => _mirrorTransform.forward;

        protected override bool ShouldAcceptHit(LightHit hit)
        {
            float angle = Vector3.Angle(-(Vector3)hit.Ray.Direction, MirrorNormal);
            return angle < 90f;
        }
    }
}