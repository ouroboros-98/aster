using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Aster.Core
{
    public class TowerAdderManager : MonoBehaviour
    {
        [SerializeField] private GameObject towerToAdd;
        [SerializeField] private Transform placeToAdd;
        [SerializeField] private int[] wavesToDrop;
        private int _waveCounter;
        private bool _needToAdd;
        private bool _placeToAddEmpty;
        private void OnEnable()
        {
            AsterEvents.Instance.OnWaveStart += IncrementCounter;
            // AsterEvents.Instance.OnTowerPlacedBetween += TowerPlaced;
            // AsterEvents.Instance.OnTowerRemovedBetween += TowerRemoved;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnWaveStart -= IncrementCounter;
            // AsterEvents.Instance.OnTowerPlacedBetween -= TowerPlaced;
            // AsterEvents.Instance.OnTowerRemovedBetween -= TowerRemoved;
        }

        private void IncrementCounter(int obj)
        {
            _waveCounter++;
            if (wavesToDrop.Contains(_waveCounter))
            {
                _needToAdd = true;
                AddTower();
            }
        }

        private void AddTower()
        {
            if (_placeToAddEmpty && _needToAdd)
            {
                Instantiate(towerToAdd, placeToAdd.position, quaternion.identity);
            }
        }

        private void TowerPlaced()
        {
            _placeToAddEmpty = false;
        }

        private void TowerRemoved()
        {
            _placeToAddEmpty = true;
        }
    }
}