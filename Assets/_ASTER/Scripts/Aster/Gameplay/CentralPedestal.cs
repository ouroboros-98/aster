using System;
using System.Collections.Generic;
using Aster.Towers;
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
            if (!other.transform.transform.ScanForComponent(out BaseGrabbable grabbable, parents: true)) return;
            if (_grabbables.Contains(grabbable)) return;

            _grabbables.Add(grabbable);

            TryDisableTower(grabbable);
        }

        private void TryDisableTower(BaseGrabbable grabbable)
        {
            if (!grabbable.ScanForComponents(out BaseTower[] towers, parents: true)) return;

            foreach (BaseTower tower in towers)
            {
                tower.enabled = false;
            }
        }

        private void TryEnableTower(BaseGrabbable grabbable)
        {
            if (!grabbable.ScanForComponents(out BaseTower[] towers, parents: true)) return;

            foreach (BaseTower tower in towers)
            {
                tower.enabled = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (!other.transform.transform.ScanForComponent(out BaseGrabbable grabbable, parents: true)) return;
            if (!_grabbables.Contains(grabbable)) return;

            _grabbables.Remove(grabbable);

            TryEnableTower(grabbable);
        }
    }
}