using Aster.Utils;
using UnityEngine;

namespace Aster.UI
{
    public partial class TowerOptionsAnimator
    {
        [System.Serializable]
        public class Configuration
        {
            [SerializeField] private bool slideDown = true;

            [SerializeField] private float             baseSpeed = 2000f; // Pixels per second
            [SerializeField] private SerializableTimer idleTime  = new(3f);

            [SerializeField] private int onScreenBottomPadding  = 32;
            [SerializeField] private int offScreenBottomPadding = -128;


            public bool SlideDown => slideDown;

            public float             BaseSpeed => baseSpeed;
            public SerializableTimer IdleTime  => idleTime;

            public int OnScreenBottomPadding  => onScreenBottomPadding;
            public int OffScreenBottomPadding => offScreenBottomPadding;
        }
    }
}