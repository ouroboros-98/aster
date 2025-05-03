using UnityEngine;

namespace Aster.Gameplay.Waves
{
    public interface IAnglePicker
    {
        float GetAngle();
    }

    [System.Serializable]
    public class AnglePickerConstant : IAnglePicker
    {
        [SerializeField, Range(0, 180)] private float angle;

        public float GetAngle()
        {
            return angle;
        }
    }

    [System.Serializable]
    public class AnglePickerRange : IAnglePicker
    {
        [SerializeField, Range(0, 180)] private float minAngle;
        [SerializeField, Range(0, 180)] private float maxAngle;

        public float GetAngle()
        {
            return Random.Range(minAngle, maxAngle);
        }
    }
}