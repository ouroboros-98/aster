using System;
using System.Linq;
using Aster.Core;
using Aster.Utils;
using DependencyInjection;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Aster.Entity.Player
{
    public class PlayerInteractor : AsterMono
    {
        [SerializeField] private IInteractable     interactable;
        [SerializeField] private PlayerController  player;
        [SerializeField] private SerializableTimer cooldown = new(.1f);

        #region FIELDS

        private          InteractionContext _currentInteraction;
        [Inject] private InputHandler       _inputHandler;

        [ShowNonSerializedField] private bool _interactButtonPressed;
        [ShowNonSerializedField] private bool _isInteracting;

        #endregion


        #region PROPERTIES

        [ShowNativeProperty] public bool          IsTargetingInteractable => interactable != null;
        [ShowNativeProperty] public IInteractable TargetedInteractable    => interactable;

        [ShowNativeProperty]
        public bool CanInteract =>
            IsNotNull(interactable)
         && IsNull(_currentInteraction)
         && !cooldown.IsRunning;

        public bool IsInteracting => _isInteracting;

        #endregion

        private void Awake()
        {
            Reset();
        }

        private void AssignCurrentInteraction(InteractionContext context) => _currentInteraction = context;

        private void OnEnable()
        {
            _inputHandler.OnInteract      += OnInteract;
            GameEvents.OnInteractionBegin += AssignCurrentInteraction;
            GameEvents.OnInteractionEnd   += OnInteractionEnd;
        }

        private void OnDisable()
        {
            _inputHandler.OnInteract      -= OnInteract;
            GameEvents.OnInteractionBegin -= AssignCurrentInteraction;
            GameEvents.OnInteractionEnd   -= OnInteractionEnd;
        }

        public void OnInteract()
        {
            if (!CanInteract) return;

            _interactButtonPressed = true;
            print("Interact button pressed");
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (!CanInteract) return;

            if (!interactable.CheckInput(_inputHandler)) return;

            Action<PlayerController> interactAction = interactable.Interact();

            interactAction?.Invoke(player);
            _isInteracting = true;
        }

        private void OnInteractionEnd(InteractionContext context)
        {
            if (context != _currentInteraction) return;

            _isInteracting      = false;
            _currentInteraction = null;

            cooldown.Start();
        }

        private void LateUpdate()
        {
            _interactButtonPressed = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.ScanForComponent(out IInteractable interactable, parents: true, children: true)) return;
            this.interactable = interactable;

            print("Interactable found");
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.ScanForComponents(out IInteractable[] interactables, parents: true, children: true)) return;
            if (!interactables.Contains(interactable)) return;

            print("Interactable lost");
            this.interactable = null;
        }

        private void Reset()
        {
            ValidateComponent(ref player, self: true, parents: true);
        }
    }
}