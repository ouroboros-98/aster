using System;
using System.Collections.Generic;
using System.Linq;
using Aster.Core;
using Aster.Utils;
using UnityEngine;

namespace _ASTER.Scripts.Aster.UI
{
    public class InteractableHighlighter : AsterMono
    {
        private static readonly int    Intensity     = Shader.PropertyToID("_Intensity");
        private                 bool   isHighlighted = false;
        private                 Action removeHighlightAction;

        [SerializeField] private List<MeshRenderer> meshRenderers;
        private                  List<Material>     _materials;

        private float materialIntensity = 1;

        private void Awake()
        {
            isHighlighted         = false;
            removeHighlightAction = delegate { };

            Reset();
            AddMaterials();
        }

        private void AddMaterials()
        {
            if (_materials != null) return;
            Material highlightMaterial = Config.UI.HighlightMaterial;
            _materials        = new();
            materialIntensity = highlightMaterial.GetFloat(Intensity);

            foreach (MeshRenderer renderer in meshRenderers)
            {
                List<Material> materials = renderer.materials.ToList();
                materials.Add(highlightMaterial);

                int index = materials.Count - 1;
                renderer.materials = materials.ToArray();

                Material addedMaterial = renderer.materials[index];
                addedMaterial.SetFloat(Intensity, 0f);

                _materials.Add(addedMaterial);
            }
        }

        public void Highlight()
        {
            if (isHighlighted) return;
            isHighlighted = true;
            _materials.ForEach(m => m.SetFloat(Intensity, materialIntensity));
        }

        public void Unhighlight()
        {
            if (!isHighlighted) return;
            isHighlighted = false;
            _materials.ForEach(m => m.SetFloat(Intensity, 0f));
        }

        private void Reset()
        {
            if (!transform.ScanForComponents(out MeshRenderer[] mr, children: true)) return;
            meshRenderers = new List<MeshRenderer>(mr);
        }
    }
}