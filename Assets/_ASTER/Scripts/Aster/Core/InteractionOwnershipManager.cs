using System.Collections.Generic;
using System.Text;
using Aster.Entity.Player;
using UnityEngine;

namespace Aster.Core
{
    public class InteractionOwnershipManager
    {
        private Dictionary<GameObject, PlayerController> _interactableOwners;

        public InteractionOwnershipManager()
        {
            _interactableOwners = new();

            var gameEvents = AsterEvents.Instance;

            gameEvents.OnInteractionBegin += OnInteractionBegin;
            gameEvents.OnInteractionEnd   += OnInteractionEnd;
        }

        private void OnInteractionBegin(InteractionContext context)
        {
            _interactableOwners[context.Interactable.GameObject] = context.Player;

            LogOwnerships();
        }

        private void OnInteractionEnd(InteractionContext obj)
        {
            if (!_interactableOwners.ContainsKey(obj.Interactable.GameObject)) return;

            _interactableOwners[obj.Interactable.GameObject] = null;

            LogOwnerships();
        }

        public bool CanBeInteractedWith(IInteractable interactable)
        {
            return CanBeInteractedWith(interactable.GameObject);
        }

        public bool CanBeInteractedWith(GameObject go)
        {
            PlayerController interactingPlayer = _interactableOwners.GetValueOrDefault(go, null);
            return interactingPlayer == null;
        }

        void LogOwnerships()
        {
            StringBuilder s = new();
            s.AppendLine("Ownerships:");
            foreach (var (go, player) in _interactableOwners)
            {
                if (player == null) continue;

                s.AppendLine($"{player.name} owns {go.name}");
            }

            Debug.Log(s.ToString());
        }
    }
}