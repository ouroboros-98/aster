using Aster.Core;
using Aster.Core.Entity;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Entity
{
    public abstract class BaseEntityController : AsterMono
    {
        [SerializeField] protected EntityHP hp;
        public EntityHP HP => hp;
    }
}