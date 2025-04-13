using System;
using System.Collections.Generic;
using Aster.Core;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Utils.Pool {
    public class AsterMultiPool<T> : AsterSingleton<AsterMultiPool<T>> where T : AsterMono, IPoolable
    {
        [SerializeField] private List<Entry> poolEntries;

        private Dictionary<string, BaseAsterPool<T>> _baseMonoPools;

        private void Awake()
        {
            _baseMonoPools = new Dictionary<string, BaseAsterPool<T>>();

            poolEntries.ForEach(RegisterPool);
        }

        private void RegisterPool(AsterMultiPool<T>.Entry entry)
        {
            BaseAsterPool<T> pool = new BaseAsterPool<T>(this, entry.Settings);
            _baseMonoPools.Add(entry.Name, pool);
        }

        public IPool<T> GetPool(string poolName) => _baseMonoPools[poolName];


        [System.Serializable]
        private class Entry
        {
            [SerializeField] private string name;
            [ShowNativeProperty, SerializeField] public string Name => name;
            [SerializeField] private BaseAsterPool<T>.Settings settings;
            public BaseAsterPool<T>.Settings Settings => settings;
        }
    }
    }