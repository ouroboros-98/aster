using System;
using System.Collections.Generic;
using Aster.Core;
using Aster.Core.Entity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _ASTER.Scripts.Aster.UI
{
    public class ShaderHealthIndicator : AsterMono
    {
        private static readonly int Progress = Shader.PropertyToID("_Progress");

        [FormerlySerializedAs("barMeshRenderer")]
        [SerializeField]
        [Required]
        private Renderer healthMeshRenderer;

        [ShowInInspector]
        private Material[] healthMaterials;

        [SerializeField]
        private bool hideInTitleScreen = true;

        private void Awake()
        {
            GetMaterials();

            if (hideInTitleScreen && Config.EnableTitleScreen)
            {
                Hide();
                GameEvents.OnGameStartComplete += Show;
            }
        }

        private void GetMaterials()
        {
            List<Material> materials = new List<Material>();
            foreach (Material material in healthMeshRenderer.materials)
            {
                if (!material.HasFloat(Progress)) continue;
                materials.Add(material);
            }

            healthMaterials = materials.ToArray();
        }

        void Hide()
        {
            if (healthMeshRenderer == null) return;
            healthMeshRenderer.enabled = false;
        }

        void Show()
        {
            if (healthMeshRenderer == null) return;
            healthMeshRenderer.enabled = true;
        }

        public void OnHPChanged(EntityHP.HPChangeContext context)
        {
            if (healthMaterials == null) return;

            float fillAmount = (float)context.CurrentValue / context.MaxValue;
            foreach (Material material in healthMaterials)
            {
                material.SetFloat(Progress, fillAmount);
            }
            // healthMeshRenderer.material.SetFloat(Progress, fillAmount);
        }
    }
}