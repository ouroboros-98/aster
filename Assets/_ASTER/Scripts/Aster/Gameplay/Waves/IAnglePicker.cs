using Sirenix.OdinInspector;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    public interface IAnglePicker
    {
        float GetAngle();
    }

    [System.Serializable]
    [InlineProperty]
    public class AnglePickerConstant : IAnglePicker
    {
        [SerializeField, Range(0, 180)]
        private float angle;

        public float GetAngle()
        {
            return angle;
        }
    }

    [System.Serializable]
    [InlineProperty]
    public class AnglePickerRange : IAnglePicker
    {
        [SerializeField]
        [MinMaxSlider(0, 180, true)]
        [HideLabel]
        private Vector2 range;

        public float GetAngle()
        {
            return Random.Range(range.x, range.y);
        }
    }
}