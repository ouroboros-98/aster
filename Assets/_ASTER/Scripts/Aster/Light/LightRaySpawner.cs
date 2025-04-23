using System.Collections;
using Aster.Utils.Pool;
using UnityEngine.Serialization;

namespace Aster.Light
{
    using UnityEngine;

    public class LightRaySpawner : MonoBehaviour
    {
        [FormerlySerializedAs("lightRayPrefab")] [SerializeField] private LightRayObject lightRayObjectPrefab;

        [FormerlySerializedAs("rayData")] [SerializeField] private ILightRay ray = new LightRay();

        [SerializeField] private LightRayObject rayObject;

        private Vector3    lastPosition;
        private Quaternion lastRotation;


        private void Start()
        {
            lastPosition = transform.position;
            lastRotation = transform.rotation;

            ray = ray.Clone();
        }

        private void FixedUpdate()
        {
            ray.Origin    = transform.position;
            ray.Direction = transform.forward;
        }
    }
}