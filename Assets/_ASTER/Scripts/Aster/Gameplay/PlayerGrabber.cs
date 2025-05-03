using Aster.Entity.Player;
using Aster.Utils;
using UnityEngine;

namespace Aster.Core
{
    public class PlayerGrabber
    {
        private PlayerController       _player;
        private PlayerInteractor       _interactor;
        private GrabInteractionContext _currentContext;
        private AsterEvents            _gameEvents;
        private AsterConfiguration     config;

        public IGrabbable CurrentGrabbable => _currentContext?.Interactable;
        public bool       IsGrabbing       => _currentContext != null;

        public PlayerGrabber(PlayerController player, PlayerInteractor interactor)
        {
            _player     = player;
            _interactor = interactor;

            _gameEvents                        =  AsterEvents.Instance;
            _gameEvents.OnGrabInteractionBegin += HandleGrabInteractionBegin;

            config = AsterConfiguration.Instance;
        }

        private void HandleGrabInteractionBegin(GrabInteractionContext context)
        {
            if (context.Player != _player) return;

            _currentContext = context;
            _currentContext.Interactable.OnGrab();
        }


        public void HandleGrab()
        {
            if (!IsGrabbing) return;

            Transform grabbable = _currentContext.Interactable.GrabbableTransform;

            Vector3 playerPivot    = (config.Entities.PlayerPivotY * Vector3.up);
            Vector3 playerPosition = _player.transform.position + playerPivot;

            grabbable.position = playerPosition.With(y: grabbable.position.y);

            _currentContext.Interactable.DuringGrab();
        }

        public void HandleRelease()
        {
            if (!CheckShouldRelease()) return;
            Release();
        }

        private void Release()
        {
            Debug.Log($"Releasing {_currentContext.Interactable.GrabbableTransform.name}");
            _gameEvents.OnInteractionEnd?.Invoke(_currentContext);
            _currentContext.Interactable.OnRelease();
            _currentContext = null;
        }

        private bool CheckShouldRelease()
        {
            return _currentContext.Interactable.CheckInput(_player.PlayerInputHandler);
        }
    }
}