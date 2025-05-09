using System;
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
        private int                    triggersCount;

        private bool _canPlace = false;

        private bool CanPlace
        {
            get => _canPlace;
            set
            {
                if (_canPlace == value) return;

                _canPlace = value;
                OnCanPlaceChanged?.Invoke(_canPlace);
            }
        }

        public event Action<bool> OnCanPlaceChanged;

        public IGrabbable CurrentGrabbable => _currentContext?.Interactable;
        public bool       IsGrabbing       => _currentContext != null;

        public PlayerGrabber(PlayerController player, PlayerInteractor interactor)
        {
            _player     = player;
            _interactor = interactor;

            _gameEvents                        =  AsterEvents.Instance;
            _gameEvents.OnGrabInteractionBegin += HandleGrabInteractionBegin;

            OnCanPlaceChanged = delegate { };

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
            if (!IsGrabbing || !CanPlace) return;

            Transform grabbable = _currentContext.Interactable.GrabbableTransform;

            // Vector3 playerPivot    = (config.Entities.PlayerPivotY * Vector3.up);
            // Vector3 playerPosition = _player.transform.position + playerPivot;

            Vector3 targetPosition = _player.transform.position + _currentContext.Offset;
            ;
            targetPosition = targetPosition.With(y: grabbable.position.y);

            Vector3 newPosition = Vector3.Lerp(
                                               grabbable.position,
                                               targetPosition,
                                               Time.fixedDeltaTime * _currentContext.Interactable.GrabFollowSpeed
                                              );

            grabbable.position = newPosition;

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
            return _player.PlayerInputHandler.Grab.WasReleasedThisFrame();
        }

        private void SetCanPlace(bool value)
        {
            CanPlace = value;
            // towerOptionsManager.SetCrossEnable(value);
        }

        public void OnTriggerEntered()
        {
            triggersCount++;
            SetCanPlace(triggersCount <= 1);
        }

        public void OnTriggerExited()
        {
            triggersCount = Mathf.Max(0, triggersCount - 1);
            SetCanPlace(triggersCount <= 1);
        }
    }
}