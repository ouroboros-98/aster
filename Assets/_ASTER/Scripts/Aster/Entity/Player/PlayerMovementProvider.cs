using Aster.Core.Core;
using UnityEngine;

namespace _ASTER.Scripts.Aster.Entity.Player
{
    public class PlayerMovementProvider : ITargetMovementProvider
    {
        public  Vector2      TargetMovement => _inputHandler.Actions.Player.Move.ReadValue<Vector2>();
        private InputHandler _inputHandler;

        public PlayerMovementProvider()
        {
            _inputHandler = InputHandler.Instance;
        }
    }
}