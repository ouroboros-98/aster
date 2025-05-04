using System;
using System.Collections.Generic;
using Aster.Utils;
using UnityEngine;

namespace Aster.Core
{
    public class CentralPedestal : AsterSingleton<CentralPedestal>
    {
        [SerializeField] private List<BaseGrabbable> _grabbables = new();
        public                   bool                IsOccupied => _grabbables.Count > 0;

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.transform.ScanForComponent(out BaseGrabbable grabbable, parents: true))
            {
                if (!_grabbables.Contains(grabbable))
                {
                    _grabbables.Add(grabbable);
                }
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.transform.transform.ScanForComponent(out BaseGrabbable grabbable, parents: true))
            {
                if (_grabbables.Contains(grabbable))
                {
                    _grabbables.Remove(grabbable);
                }
            }
        }
    }
}