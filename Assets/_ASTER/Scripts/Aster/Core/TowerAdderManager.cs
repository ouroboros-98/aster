using System;
using System.Collections.Generic;
using System.Linq;
using Aster.Utils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Aster.Core
{
    public class TowerAdderManager : AsterMono
    {
        [Serializable]
        class Entry
        {
            [SerializeField]
            private int wave;

            [SerializeField]
            private GameObject tower;

            public int        Wave  => wave;
            public GameObject Tower => tower;
        }

        // [SerializeField] private GameObject towerToAdd;
        [SerializeField]
        private Transform placeToAdd;

        [SerializeField]
        private int[] wavesToDrop;

        private Queue<GameObject> towerQueue;

        private int  _waveCounter;
        private bool _needToAdd => towerQueue != null && towerQueue.Count > 0;
        private bool _canAdd = true;

        private CentralPedestal _centralPedestal;

        private void Awake()
        {
            _centralPedestal = CentralPedestal.Instance;
            towerQueue       = new();
        }

        // private void OnEnable()
        // {
        //     AsterEvents.Instance.OnWaveStart += IncrementCounter;
        // }
        //
        // private void OnDisable()
        // {
        //     AsterEvents.Instance.OnWaveStart -= IncrementCounter;
        // }

        // private void IncrementCounter(int obj)
        // {
        //     _waveCounter++;
        //     bool shouldAddTower = _entriesDict.ContainsKey(_waveCounter);
        //
        //     debugPrint($"Increment WaveCounter: {_waveCounter}, shouldAddTower: {shouldAddTower}");
        //
        //     if (shouldAddTower)
        //     {
        //         _needToAdd = true;
        //
        //         _entriesQueue.Enqueue(_entriesDict[_waveCounter]);
        //     }
        // }

        private void AddTower(GameObject tower)
        {
            Instantiate(tower, placeToAdd.position, quaternion.identity);
        }

        public void EnqueueTower(GameObject tower)
        {
            if (towerQueue == null)
            {
                towerQueue = new Queue<GameObject>();
            }

            towerQueue.Enqueue(tower);
        }

        private void CheckCanAdd()
        {
            _canAdd = true;
            Vector3    center       = placeToAdd.position;
            float      radius       = .5f;
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            foreach (Collider col in hitColliders)
            {
                if (col.transform.ScanForComponent(out BaseGrabbable grabbable, parents: true))
                {
                    _canAdd = false;
                    break;
                }
            }
        }

        private void Update()
        {
            if (!_needToAdd) return;
            if (_centralPedestal.IsOccupied) return;

            CheckCanAdd();

            if (!_canAdd) return;

            AddTower(towerQueue.Dequeue());
            _canAdd = false;
        }
    }
}