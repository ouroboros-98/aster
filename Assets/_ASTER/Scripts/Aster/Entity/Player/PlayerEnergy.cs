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
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnLightPointAdded -= IncrementEnergy;

        }

        private void IncrementEnergy(LightPoint obj)
        {
            _playerEnergy++; 
        }

        public int GetPlayerEnergy()
        {
            return _playerEnergy;
        }

        public bool ReducePlayerEnergy(int value)
        {
            if (_playerEnergy < value)
                return false;
            _playerEnergy -= value;
            return true;
        }
    }
}