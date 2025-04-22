using System;
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

        public Action<InteractionContext>              OnInteractionBegin;
        public Action<RotationInteractionContext> OnRotationInteractionBegin;
        public Action<InteractionContext>              OnInteractionEnd;
        public Action<LightPoint> OnLightPointAdded;
        public Action<int> OnLightPointRemoved;
        public Action<int> OnAttackLightSource;
        public Action<int> OnLightSourceDestroyed;
        public Action<Vector3> OnEnemyDeath;
        private AsterEvents()
        {
            OnInteractionBegin         =  delegate { };
            OnInteractionEnd   =  delegate { };
            OnRotationInteractionBegin =  delegate { };
            OnRotationInteractionBegin += context => OnInteractionBegin?.Invoke(context);
            OnLightPointAdded =  delegate { };
            OnAttackLightSource =  delegate { };
            OnLightSourceDestroyed =  delegate { };
            OnEnemyDeath = delegate { };
            OnLightPointRemoved =  delegate { };
        }
    }
}