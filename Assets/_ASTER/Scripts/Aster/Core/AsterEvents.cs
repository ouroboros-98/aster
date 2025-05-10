using System;
using Aster.Entity.Enemy;
using Aster.Light;
using UnityEngine;

namespace Aster.Core
{
    public class AsterEvents
    {
        private static AsterEvents _instance;

        public static AsterEvents Instance
        {
            get
            {
                if (_instance == null) _instance = new AsterEvents();

                return _instance;
            }
        }

        public Action<ILightRay> OnRayActivated;

        public Action<InteractionContext>         OnInteractionBegin;
        public Action<RotationInteractionContext> OnRotationInteractionBegin;
        public Action<GrabInteractionContext>     OnGrabInteractionBegin;
        public Action<InteractionContext>         OnInteractionEnd;

        public Action<int> OnLightPointAdded;
        public Action<int> OnLightPointRemoved;

        public Action<int> OnAttackLightSource;
        public Action      OnLightSourceDestroyed;
        
        public Action<EnemyController> OnEnemySpawn;
        public Action<EnemyController> OnEnemyDeath;

        public Action<int> OnWaveStart;
        public Action<int> OnWaveEnd;
        public Action<int> OnLevelEnd;

        public Action OnTryToPlaceTower;
        public Action OnBetweenPlaceTaken;
        
        public Action AllEnemiesDead;
        public Action OnGameStartComplete;

        private AsterEvents()
        {
            OnRayActivated = delegate { };

            OnInteractionBegin = delegate { };
            OnInteractionEnd   = delegate { };

            OnRotationInteractionBegin =  delegate { };
            OnRotationInteractionBegin += context => OnInteractionBegin?.Invoke(context);

            OnGrabInteractionBegin =  delegate { };
            OnGrabInteractionBegin += context => OnInteractionBegin?.Invoke(context);

            OnLightPointAdded   = delegate { };
            OnLightPointRemoved = delegate { };

            OnAttackLightSource    = delegate { };
            OnLightSourceDestroyed = delegate { };

            OnEnemySpawn = delegate { };
            OnEnemyDeath = delegate { };

            OnWaveStart = delegate { };
            OnWaveEnd   = delegate { };

            OnLevelEnd     = delegate { };
            AllEnemiesDead = delegate { };
            OnTryToPlaceTower = delegate { };
            OnGameStartComplete = delegate { };
        }
    }
}