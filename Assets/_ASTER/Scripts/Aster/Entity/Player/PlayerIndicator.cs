using Aster.Core;
using Aster.Core.UI;
using UnityEngine;

namespace Aster.Entity.Player
{
    public class PlayerIndicator : AsterMono
    {
        [SerializeField] private PopUpIndicator indicator;
        private int _triggerCount;

        private void Awake()
        {
            indicator.SetEnabled(false);
            _triggerCount = 0;
        }

        public void SetIndicator(bool enabled)
        {
            if (enabled)
                _triggerCount++;
            else
                _triggerCount--;

            if (_triggerCount < 0)
                _triggerCount = 0;

            indicator.SetEnabled(_triggerCount > 0);
        }
    }
}