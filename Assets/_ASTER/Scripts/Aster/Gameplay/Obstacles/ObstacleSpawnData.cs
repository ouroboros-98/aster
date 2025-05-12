using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Gameplay.Obstacles
{
    [Serializable]
    [InlineProperty]
    public class ObstacleSpawnData
    {
        [FormerlySerializedAs("obstaclePrefab")]
        [HideLabel]
        [PreviewField(60, ObjectFieldAlignment.Left)]
        public GameObject prefab;

        [VerticalGroup("Transform")]
        public Vector3 position, rotation;
    }
}