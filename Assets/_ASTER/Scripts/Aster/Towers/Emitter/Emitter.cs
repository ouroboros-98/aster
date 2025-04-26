using System;
using System.Reflection;
using Aster.Core;
using Aster.Entity.Player;
using Aster.Light;
using Aster.Utils.Pool;
using UnityEngine;
using UnityEngine.Android;

namespace Aster.Towers
{
    public class Emitter : BaseRotatable
    {
        public override Action<PlayerController> Interact()
        {
            return (player) =>
                   {
                       var context = new AnchorRotationInteractionContext(player, this);
                       GameEvents.OnRotationInteractionBegin?.Invoke(context);
                   };
        }
    }
}