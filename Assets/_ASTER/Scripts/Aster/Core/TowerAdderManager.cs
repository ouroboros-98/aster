using System;
using System.Collections.Generic;
using System.Linq;
using Aster.Utils;
using Unity.Mathematics;
using UnityEngine;

namespace Aster.Core
{
    public class TowerAdderManager : AsterMono
    {
        [Serializable]
        class Entry
        {
            [SerializeField] private int        wave;
            [SerializeField] private GameObject tower;

            public int        Wave  => wave;
            public GameObject Tower => tower;
        }

        [SerializeField] private Entry[] entries;

        // [SerializeField] private GameObject towerToAdd;
        [SerializeField] private Transform placeToAdd;
        [SerializeField] private int[]     wavesToDrop;

        private Queue<Entry> _entriesQueue;

        private Dictionary<int, Entry> _entriesDict;

        private int  _waveCounter;
        private bool _needToAdd;
        private bool _canAdd = true;

        private CentralPedestal _centralPedestal;

        private void Awake()
        {
            _centralPedestal = CentralPedestal.Instance;
            _entriesQueue    = new();
            _entriesDict     = new();
            foreach (Entry entry in entries)
            {
                _entriesDict.Add(entry.Wave, entry);
            }
        }

        private void OnEnable()
        {
            AsterEvents.Instance.OnWaveStart += IncrementCounter;
        }

        private void OnDisable()
        {
            AsterEvents.Instance.OnWaveStart -= IncrementCounter;
        }

        private void IncrementCounter(int obj)
        {
            _waveCounter++;
            bool shouldAddTower = _entriesDict.ContainsKey(_waveCounter);

            debugPrint($"Increment WaveCounter: {_waveCounter}, shouldAddTower: {shouldAddTower}");

            if (shouldAddTower)
            {
                _needToAdd = true;

                _entriesQueue.Enqueue(_entriesDict[_waveCounter]);
            }
        }

        private void AddTower(Entry entry)
        {
            Instantiate(entry.Tower, placeToAdd.position, quaternion.identity);
            _canAdd = true;
        }

        private void Update()
        {
            if (!_needToAdd) return;

            // _canAdd = true;
            // Vector3    center       = placeToAdd.position;
            // float      radius       = 3f;
            // Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            // foreach (Collider col in hitColliders)
            // {
            //     if (col.transform.ScanForComponent(out BaseGrabbable grabbable, parents: true))
            //     {
            //         _canAdd = false;
            //         break;
            //     }
            // }

            // if (_canAdd)
            if (!_centralPedestal.IsOccupied)
            {
                if (_entriesQueue.Count > 0)
                {
                    AddTower(_entriesQueue.Dequeue());
                    _canAdd = false;
                }
            }
        }
    }
}