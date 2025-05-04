using Aster.Core;
using Aster.Core.UI;
using UnityEngine;

namespace Aster.Entity.Player
{
    public class PlayerIndicator:AsterMono
    {
        [SerializeField] private PopUpIndicator indicator;
        
        private void Awake()
        {
            indicator.SetEnabled(false);
        }
        
        public void SetIndicator(bool enabled)
        {
            indicator.SetEnabled(enabled);
        }
        
    }
}