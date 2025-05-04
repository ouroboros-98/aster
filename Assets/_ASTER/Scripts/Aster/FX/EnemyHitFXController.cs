using System;
using UnityEngine;

namespace Aster.Core.FX
{
    public class EnemyHitFXController : AsterMono
    {
        private MeshRenderer mr;
        private Material     mat;

        private void Awake()
        {
            ValidateComponent(ref mr);
            mat = mr.materials[0];
        }

        public void Show() => mr.enabled = true;
        public void Hide() => mr.enabled = false;
    }
}