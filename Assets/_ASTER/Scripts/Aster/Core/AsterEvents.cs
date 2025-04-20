using System;
using Aster.Light;

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
        private AsterEvents()
        {
            OnInteractionBegin         =  delegate { };
            OnInteractionEnd   =  delegate { };
            OnRotationInteractionBegin =  delegate { };
            OnRotationInteractionBegin += context => OnInteractionBegin?.Invoke(context);
            OnLightPointAdded =  delegate { };
        }
    }
}