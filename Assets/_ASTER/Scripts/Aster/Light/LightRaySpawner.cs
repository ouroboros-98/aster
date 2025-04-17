namespace Aster.Light
{
    using UnityEngine;

    public class LightRaySpawner : MonoBehaviour
    {
        [SerializeField] private LightRay lightRayPrefab;
        [SerializeField] private Color rayColor = Color.yellow;
        [SerializeField] private float rayIntensity = 1f;
        [SerializeField] private float rayMaxDistance = 10f;

        public void SpawnLightRay(Vector3 direction)
        {
            // Instantiate the LightRay prefab
            LightRay lightRay = Instantiate(lightRayPrefab, transform.position, Quaternion.identity);
        
            // Initialize the LightRay
            lightRay.Initialize(transform.position, direction, rayColor, rayIntensity);
        }

        private void Update()
        {
            // Example: Spawn a LightRay when the space key is pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnLightRay(transform.forward); // Spawn in the forward direction
            }
        }
    }
}