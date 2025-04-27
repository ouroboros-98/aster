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

        public Action<ILightRay> OnRayActivated;

        public Action<InteractionContext>         OnInteractionBegin;
        public Action<RotationInteractionContext> OnRotationInteractionBegin;
        public Action<InteractionContext>         OnInteractionEnd;
        public Action<int>                        OnLightPointAdded;
        public Action<int>                        OnLightPointRemoved;
        public Action<int>                        OnAttackLightSource;
        public Action                        OnLightSourceDestroyed;

        public Action<Vector3> OnEnemyDeath;
        public Action<int> OnWaveStart;
        public Action<int> OnWaveEnd;

        private AsterEvents()
        {
            OnRayActivated = delegate { };

            OnInteractionBegin         =  delegate { };
            OnInteractionEnd           =  delegate { };
            OnRotationInteractionBegin =  delegate { };
            OnRotationInteractionBegin += context => OnInteractionBegin?.Invoke(context);
            
            OnLightPointAdded =  delegate { };
            OnLightPointRemoved =  delegate { };
            
            OnAttackLightSource =  delegate { };
            OnLightSourceDestroyed =  delegate { };
            
            OnEnemyDeath = delegate { };
            OnWaveStart =  delegate { };
            OnWaveEnd    =  delegate { };
        }
    }
}