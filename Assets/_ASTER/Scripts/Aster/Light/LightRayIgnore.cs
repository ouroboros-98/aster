using System;
using System.Collections.Generic;
using Aster.Core;
using UnityEngine;

namespace Aster.Light
{
    public class LightRayIgnore : AsterMono
    {
        [SerializeField] private bool includeChildren;

        private List<Transform> _thisObjectIgnores;

        private static HashSet<Transform> _ignoreList;

        public static HashSet<Transform> IgnoreList
        {
            get
            {
                if (_ignoreList == null)
                {
                    _ignoreList = new HashSet<Transform>();
                }

                return _ignoreList;
            }
        }

        private void Awake()
        {
            _thisObjectIgnores = new List<Transform>();
        }

        private void OnEnable()
        {
            AddIgnores();
        }

        private void OnDisable()
        {
            RemoveIgnores();
        }

        private void AddIgnores()
        {
            _thisObjectIgnores.Clear();
            _thisObjectIgnores.Add(transform);
            if (includeChildren)
            {
                foreach (Transform child in transform)
                {
                    _thisObjectIgnores.Add(child);
                }
            }

            IgnoreList.UnionWith(_thisObjectIgnores);
        }

        private void RemoveIgnores()
        {
            IgnoreList.ExceptWith(_thisObjectIgnores);
        }
    }
}