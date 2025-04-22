using Aster.Core;
using Aster.Light;
using UnityEngine;

namespace Aster.Entity.Player
{
    public class PlayerEnergy :MonoBehaviour
    {
        private int _playerEnergy;
        
        
        private void OnEnable()
        {
            AsterEvents.Instance.OnLightPointAdded += IncrementEnergy;
            AsterEvents.Instance.OnLightPointRemoved += ReducePlayerEnergy;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnLightPointAdded -= IncrementEnergy;
            AsterEvents.Instance.OnLightPointRemoved -= ReducePlayerEnergy;
        }

        private void IncrementEnergy(LightPoint obj)
        {
            _playerEnergy++; 
        }

        public int GetPlayerEnergy()
        {
            return _playerEnergy;
        }

        public void ReducePlayerEnergy(int value)
        {
            if (_playerEnergy < value)
                return;
            _playerEnergy -= value;
        }
    }
}