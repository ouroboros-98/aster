using System;
using System.Collections.Generic;
using Aster.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Aster.Utils.Pool
{
    [DefaultExecutionOrder(-100)]
    public class AsterPool<T> : AsterSingleton<AsterPool<T>>, IPool<T> where T : AsterMono, IPoolable
    {
        [SerializeField]
        private int initialSize;

        [SerializeField]
        private T prefab;

        [SerializeField]
        private Transform parent;

        private BaseAsterPool<T> _baseAsterPool;

        private void Awake()
        {
            _baseAsterPool = new BaseAsterPool<T>(this, initialSize, prefab, parent);
        }

        public T Get()
        {
            T obj = _baseAsterPool.Get();
            OnGet(obj);
            return obj;
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            T obj = _baseAsterPool.Get(position, rotation);
            OnGet(obj);
            return obj;
        }

        protected virtual void OnGet(T obj)
        {
        }

        public void Return(T obj) => _baseAsterPool.Return(obj);

        public void ReturnDelayed(T obj, float delay) => _baseAsterPool.ReturnDelayed(obj, delay);
    }
}