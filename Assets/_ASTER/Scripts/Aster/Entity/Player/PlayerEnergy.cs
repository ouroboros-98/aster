using System;
using Aster.Core;
using Aster.Light;
using Aster.Utils;
using UnityEngine;

namespace Aster.Entity.Player
{
    public class PlayerEnergy : AsterSingleton<PlayerEnergy>
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

        private void IncrementEnergy(float energy)
        {
            _playerEnergy++;
        }

        public int GetPlayerEnergy()
        {
            return _playerEnergy;
        }

        public bool AttemptReduceEnergy(int value)
        {
            if (_playerEnergy < value)
                return false;

            _playerEnergy -= value;

            GameEvents.OnLightPointRemoved?.Invoke(value);

            return true;
        }
    }
}