using UnityEngine;

namespace Aster.Core.Interactions
{
    public class TowerSpinner : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private float hoverAmount = 0.5f;
        [SerializeField] private float hoverSpeed = 2f;

        private float startY;

        private void Start()
        {
            startY = transform.position.y;
        }

        private void Update()
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
            Vector3 position = transform.position;
            position.y = startY + Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
            transform.position = position;
        }
    }
}