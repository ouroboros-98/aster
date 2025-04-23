using Unity.VisualScripting;
using UnityEngine;

namespace Aster.Towers
{
    public class Mirror : BaseTower
    {
        private MirrorManipulator _mirrorManipulator;
        private Vector3           MirrorNormal => transform.forward;

        private void Awake()
        {
            _mirrorManipulator = new MirrorManipulator(this);
        }
    }
}