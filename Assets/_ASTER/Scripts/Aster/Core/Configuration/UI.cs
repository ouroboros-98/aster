using System;
using Aster.Utils.Attributes;
using UnityEngine;

namespace Aster.Core
{
    public partial class AsterConfiguration
    {
        [SerializeField, BoxedProperty] private UIConfiguration _uiConfig = new();
        public                                  UIConfiguration UI => _uiConfig;

        [Serializable]
        public class UIConfiguration
        {
            [SerializeField] private Material _highlightMaterial;

            public Material HighlightMaterial => _highlightMaterial;
        }
    }
}