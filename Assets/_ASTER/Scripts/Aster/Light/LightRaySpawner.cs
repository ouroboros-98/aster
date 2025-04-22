using Aster.Utils.Pool;

namespace Aster.Light
{
    using UnityEngine;

    public class LightRaySpawner : MonoBehaviour
    {
        [SerializeField] private LightRay lightRayPrefab;

        [SerializeField] private RayData rayData = new();

        [SerializeField] private LightRay rayObject;

        private Vector3    lastPosition;
        private Quaternion lastRotation;


        private void Start()
        {
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            
            rayData = new(rayData);
        }

        private void FixedUpdate()
        {
            rayData.Origin    = transform.position;
            rayData.Direction = transform.forward;
        }
    }
}