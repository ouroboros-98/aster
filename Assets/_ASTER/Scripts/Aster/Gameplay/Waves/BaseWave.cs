using UnityEngine;

namespace Aster.Gameplay.Waves
{
    [CreateAssetMenu(fileName = "BaseWave", menuName = "Scriptable Objects/BaseWave")]
    public abstract class BaseWave : ScriptableObject
    {
        public abstract void OnWaveStart();
        public abstract void OnWaveEnd();
    }
}
