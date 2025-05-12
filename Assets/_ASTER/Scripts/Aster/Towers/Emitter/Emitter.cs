using System;
using System.Reflection;
using Aster.Core;
using Aster.Entity.Player;
using Aster.Light;
using Aster.Utils;
using Aster.Utils.Pool;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Android;

namespace Aster.Towers
{
    public class Emitter : BaseRotatable
    {
        protected override void Awake()
        {
            base.Awake();

            RotationHandler.InvertDelta    = true;
            RotationHandler.EnableBounding = true;
            RotationHandler.Bounds         = (70, 290);
        }

        public override Action<PlayerController> Interact()
        {
            return (player) =>
                   {
                       var context = new AnchorRotationInteractionContext(player, this);
                       GameEvents.OnRotationInteractionBegin?.Invoke(context);
                   };
        }

        [Button("Log Angle")]
        void LogAngle()
        {
            Debug.Log($"Emitter angle: {RotationHandler.CurrentAngle}");
        }
    }
}