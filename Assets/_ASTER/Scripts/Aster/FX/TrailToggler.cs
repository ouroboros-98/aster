using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aster.Core.FX
{
    public class TrailToggler : AsterMono
    {
        [SerializeField]
        private List<Transform> objects;

        private void OnEnable()
        {
            GameEvents.OnGameStartComplete    += OnGameStartComplete;
            GameEvents.OnLightSourceDestroyed += OnLightSourceDestroyed;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStartComplete    -= OnGameStartComplete;
            GameEvents.OnLightSourceDestroyed -= OnLightSourceDestroyed;
        }

        private void OnLightSourceDestroyed()
        {
            objects.ForEach(t => t.gameObject.SetActive(false));
        }

        private void OnGameStartComplete()
        {
            objects.ForEach(t => t.gameObject.SetActive(true));
        }
    }
}