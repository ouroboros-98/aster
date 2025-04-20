using Aster.Utils.Pool;

namespace Aster.Light
{
    using UnityEngine;

    public class LightRaySpawner : MonoBehaviour
    {
        [SerializeField] private LightRay lightRayPrefab;
        [SerializeField] private Color rayColor = Color.yellow;
        [SerializeField] private float rayIntensity = 1f;
        private LightRay lightRaySpawned;
        private Vector3 lastPosition;
        private Quaternion lastRotation;

       

        private void Start()
        {
            SpawnLightRay(transform.forward);
            lastPosition = transform.position;
            lastRotation = transform.rotation;
        }

        private void SpawnLightRay(Vector3 direction)
        {
            // Instantiate the LightRay prefab
            LightRay lightRay = RayPool.Instance.Get();
        
            // Initialize the LightRay
            lightRay.Initialize(transform.position, direction, rayColor, rayIntensity);
            lightRaySpawned = lightRay;
        }

        private void Update()
        {
            // Check if the emitter has moved or rotated
            if (transform.position != lastPosition || transform.rotation != lastRotation)
            {
                // Update the light ray's origin and direction
                lightRaySpawned.Initialize(transform.position, transform.forward, rayColor, rayIntensity);

                // Update the last known position and rotation
                lastPosition = transform.position;
                lastRotation = transform.rotation;
            }
        }
    }
}