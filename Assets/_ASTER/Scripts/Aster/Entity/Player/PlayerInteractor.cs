using System;
using System.Collections.Generic;
using System.Linq;
using _ASTER.Scripts.Aster.UI;
using Aster.Core;
using Aster.Utils;
using DependencyInjection;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Aster.Entity.Player
{
    public class PlayerInteractor : AsterMono
    {
        [SerializeField]
        private IInteractable interactable;

        [SerializeField]
        private PlayerController player;

        [SerializeField]
        private SerializableTimer cooldown = new(.1f);

        #region FIELDS

        private InteractionContext          _currentInteraction;
        private PlayerInputHandler          _inputHandler => player.PlayerInputHandler;
        private InteractionOwnershipManager _interactionOwnershipManager;
        private IInteractable[]             allInteractablesOfObject;
        private IRotatatble                 _rotatatble;

        private IInteractable[] _pendingInteractables;

        [Sirenix.OdinInspector.ReadOnly, SerializeField]
        private bool locked = false;

        [Sirenix.OdinInspector.ReadOnly, SerializeField]
        private bool shouldClear = false;

        [ShowNonSerializedField]
        private bool _interactButtonPressed;

        [ShowNonSerializedField]
        private bool _isInteracting;

        #endregion


        #region PROPERTIES

        [ShowNativeProperty] public bool          IsTargetingInteractable => interactable != null;
        [ShowNativeProperty] public IInteractable TargetedInteractable    => interactable;

        public IRotatatble Rotatatble => _rotatatble;

        [ShowNativeProperty]
        public bool CanInteract =>
            IsNotNull(interactable)
         && IsNull(_currentInteraction)
         && _interactionOwnershipManager.CanBeInteractedWith(interactable)
         && !cooldown.IsRunning;

        public bool IsInteracting => _isInteracting;

        #endregion

        private void Awake()
        {
            Reset();
        }

        private void Start()
        {
            _interactionOwnershipManager = GameManager.Instance.InteractionOwnershipManager;
        }

        private void AssignCurrentInteraction(InteractionContext context)
        {
            if (context.Player != player) return;
            _currentInteraction = context;
        }

        private void OnEnable()
        {
            if (_inputHandler == null) return;

            _inputHandler.OnInteract      += OnInteract;
            GameEvents.OnInteractionBegin += AssignCurrentInteraction;
            GameEvents.OnInteractionEnd   += OnInteractionEnd;
        }

        private void OnDisable()
        {
            if (_inputHandler == null) return;

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
            if (locked) return;
            if (shouldClear) Clear();
            if (_pendingInteractables != null)
            {
                ProcessInteractableEnter(_pendingInteractables);
                _pendingInteractables = null;
            }
        }

        private void HandleInput()
        {
            if (!CanInteract) return;

            foreach (IInteractable i in allInteractablesOfObject)
            {
                if (!i.CheckInput(_inputHandler)) continue;

                Action<PlayerController> interactAction = i.Interact();

                interactAction?.Invoke(player);
                _isInteracting = true;
                return;
            }
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
            if (!other.ScanForComponents(out IInteractable[] interactables, parents: true, children: true)) return;

            if (interactables == null || interactables.Length == 0) return;

            if (shouldClear && interactables.Contains(this.interactable))
            {
                shouldClear = false;
                return;
            }

            _pendingInteractables = interactables;
        }

        private void ProcessInteractableEnter(IInteractable[] interactables)
        {
            List<IInteractable> allInteractables = new();

            foreach (IInteractable interactable in interactables)
            {
                if (interactable.GameObject.CompareTag("Targeting")) continue;

                if (allInteractables.Count != 0 && interactable.GameObject != allInteractables[0].GameObject) continue;

                allInteractables.Add(interactable);
                this.interactable = interactable;

                if (this.interactable is IRotatatble rotatatble) _rotatatble = rotatatble;
            }

            if (this.interactable == null) return;

            allInteractablesOfObject = allInteractables.ToArray();

            this.interactable.GameObject.GetOrAddComponent<InteractableHighlighter>().Highlight();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.ScanForComponents(out IInteractable[] interactables, parents: true, children: true)) return;
            if (!interactables.Contains(interactable)) return;

            shouldClear = true;

        }

        public void Lock()
        {
            if (interactable == null) return;
            locked = true;
        }

        public void Unlock()
        {
            locked = false;
        }

        private void Clear()
        {
            this.interactable.GameObject.GetComponent<InteractableHighlighter>()?.Unhighlight();
            this.interactable = null;
            this._rotatatble  = null;

            allInteractablesOfObject = null;
            shouldClear              = false;
        }

        private void Reset()
        {
            ValidateComponent(ref player, self: true, parents: true);
        }
    }
}