using System;
using _ASTER.Scripts.Aster.Entity;
using Aster.Core;
using Aster.Core.Entity;
using NaughtyAttributes;
using UnityEngine;

namespace Aster.Entity
{
    public abstract class BaseEntityController : AsterMono
    {
        [SerializeField] protected EntityHP       hp;
        [SerializeField] protected EntityMovement movement;
        [SerializeField] protected Rigidbody      rb;

        public EntityHP HP => hp;

        protected virtual void Awake()
        {
            ValidateComponent(ref rb);
        }
    }
}