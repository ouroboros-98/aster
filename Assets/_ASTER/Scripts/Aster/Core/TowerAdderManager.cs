using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Aster.Core
{
    public class TowerAdderManager : AsterMono
    {
        [SerializeField] private GameObject towerToAdd;
        [SerializeField] private Transform  placeToAdd;
        [SerializeField] private int[]      wavesToDrop;
        private                  int        _waveCounter;
        private                  bool       _needToAdd;
        private                  bool       _canAdd = true;

        private void OnEnable()
        {
            AsterEvents.Instance.OnWaveStart       += IncrementCounter;
            AsterEvents.Instance.OnTryToPlaceTower += AddTower;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnWaveStart       -= IncrementCounter;
            AsterEvents.Instance.OnTryToPlaceTower -= AddTower;
        }

        private void IncrementCounter(int obj)
        {
            _waveCounter++;
            bool shouldAddTower = wavesToDrop.Contains(_waveCounter);

            debugPrint($"Increment WaveCounter: {_waveCounter}, shouldAddTower: {shouldAddTower}");

            if (shouldAddTower)
            {
                _needToAdd = true;
                AddTower();
            }
        }

        private void AddTower()
        {
            Instantiate(towerToAdd, placeToAdd.position, quaternion.identity);
            _needToAdd = false;
            _canAdd    = true;
        }

        private void Update()
        {
            if (_needToAdd)
            {
                if (_canAdd)
                {
                    AddTower();
                }
                else
                {
                    Vector3    center       = placeToAdd.position;
                    float      radius       = 3f;
                    Collider[] hitColliders = Physics.OverlapSphere(center, radius);
                    _canAdd = true;
                    foreach (Collider col in hitColliders)
                    {
                        if (col.CompareTag("Mirror"))
                        {
                            _canAdd = false;
                        }
                    }
                }
            }
        }
    }
}