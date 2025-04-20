using Aster.Core;
using UnityEngine;

namespace Aster.Entity.Player
{
    public class PlayerMovementProvider : ITargetMovementProvider
    {
        public  Vector2      TargetMovement => _inputHandler.Actions.Player.Move.ReadValue<Vector2>();
        private InputHandler _inputHandler;

        public PlayerMovementProvider(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }
    }
}