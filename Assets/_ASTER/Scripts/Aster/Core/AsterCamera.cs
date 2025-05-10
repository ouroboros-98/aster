using System;
using Aster.Utils;
using UnityEngine;

namespace Aster.Core
{
    public class AsterCamera : AsterSingleton<AsterCamera>
    {
        [SerializeField]
        private Transform _aimPoint;

        public Transform AimPoint => _aimPoint;

        private void Awake()
        {
            GetComponent<Camera>().enabled = false;
        }
    }
}