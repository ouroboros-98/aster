using System;
using Aster.Core;
using Aster.Core.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ASTER.Scripts.Aster.UI
{
    public class LampHealthBar : AsterMono
    {
        private static readonly int Progress = Shader.PropertyToID("_Progress");

        [SerializeField]
        [Required]
        private MeshRenderer barMeshRenderer;

        private void Awake()
        {
            if (Config.EnableTitleScreen)
            {
                Hide();
                GameEvents.OnGameStartComplete += Show;
            }
        }

        void Hide() => barMeshRenderer.enabled = false;
        void Show() => barMeshRenderer.enabled = true;

        public void OnHPChanged(EntityHP.HPChangeContext context)
        {
            if (barMeshRenderer == null) return;

            float fillAmount = (float)context.CurrentValue / context.MaxValue;
            barMeshRenderer.material.SetFloat(Progress, fillAmount);
        }
    }
}