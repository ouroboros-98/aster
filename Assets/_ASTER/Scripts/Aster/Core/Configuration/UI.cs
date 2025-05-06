using System;
using Aster.Utils.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aster.Core
{
    public partial class AsterConfiguration
    {
        private UIConfiguration _uiConfig = new();

        public UIConfiguration UI => _uiConfig;

        [BoxGroup("UI")]
        [SerializeField]
        private Material highlightMaterial;

        public class UIConfiguration
        {
            public AsterConfiguration _config;

            public Material HighlightMaterial => _config.highlightMaterial;
        }
    }
}