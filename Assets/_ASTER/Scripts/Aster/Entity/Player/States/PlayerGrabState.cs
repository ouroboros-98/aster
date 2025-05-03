using Aster.Core;
using Aster.Entity.StateMachine;

namespace Aster.Entity.Player.States
{
    public class PlayerGrabState : BaseEntityState
    {
        private readonly PlayerController _player;
        private readonly PlayerGrabber    _grabber;
        private readonly EntityMovement   _movement;

        public PlayerGrabState(PlayerController player, PlayerGrabber grabber, EntityMovement movement) : base(player)
        {
            _player   = player;
            _grabber  = grabber;
            _movement = movement;
        }

        public override void FixedUpdate()
        {
            _movement.HandleMovement();
            _grabber.HandleGrab();
        }

        public override void Update()
        {
            _grabber.HandleRelease();
        }
    }
}