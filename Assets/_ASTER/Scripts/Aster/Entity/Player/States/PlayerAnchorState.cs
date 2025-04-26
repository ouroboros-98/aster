using Aster.Entity.StateMachine;
using UnityEngine;

namespace Aster.Entity.Player.States
{
    public class PlayerAnchorState : BaseEntityState
    {
        private PlayerAnchor _anchor;
        private Rigidbody    _rb;

        public PlayerAnchorState(BaseEntityController entity, PlayerAnchor anchor, Rigidbody rb) : base(entity)
        {
            _anchor = anchor;
            _rb     = rb;
        }

        public override void OnEnter()
        {
            _rb.isKinematic = true;
        }

        public override void OnExit()
        {
            _rb.isKinematic = false;
        }

        public override void FixedUpdate()
        {
            _anchor.HandleAnchoring();
        }
    }
}