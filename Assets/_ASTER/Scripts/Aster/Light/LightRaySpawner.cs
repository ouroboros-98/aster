using Aster.Utils.Pool;
using UnityEngine.Serialization;

namespace Aster.Light
{
    using UnityEngine;

    public class LightRaySpawner : MonoBehaviour
    {
        [FormerlySerializedAs("lightRayPrefab")] [SerializeField] private LightRayObject lightRayObjectPrefab;

        [FormerlySerializedAs("rayData")] [SerializeField] private LightRay ray = new();

        [SerializeField] private LightRayObject rayObject;

        private Vector3    lastPosition;
        private Quaternion lastRotation;


        private void Start()
        {
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            
            ray = new(ray);
        }

        private void FixedUpdate()
        {
            ray.Origin    = transform.position;
            ray.Direction = transform.forward;
        }
    }
}