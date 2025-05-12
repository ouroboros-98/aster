using System.Collections;
using Aster.Core;
using Aster.Utils.Pool;
using UnityEngine.Serialization;

namespace Aster.Light
{
    using UnityEngine;

    public class LightRaySpawner : AsterMono
    {
        [FormerlySerializedAs("lightRayPrefab")]
        [SerializeField]
        private LightRayObject lightRayObjectPrefab;

        [FormerlySerializedAs("rayData")]
        [SerializeField]
        private ILightRay ray = new LightRay();

        [SerializeField]
        private LightRayObject rayObject;

        [SerializeField]
        [Range(0, 2)]
        private float spawnDistance = .4f;

        private Vector3    lastPosition;
        private Quaternion lastRotation;


        private void Start()
        {
            lastPosition = transform.position;
            lastRotation = transform.rotation;

            ray = ray.Clone();

            ray.OnDestroy += () => { print("Spawner ray destroyed"); };
        }

        private void FixedUpdate()
        {
            ray.MaxDistance = Config.Lightrays.MaxDistance;

            Vector3 direction = transform.forward;
            Vector3 position  = transform.position + direction * spawnDistance;

            ray.Origin    = position;
            ray.Direction = direction;
        }
    }
}