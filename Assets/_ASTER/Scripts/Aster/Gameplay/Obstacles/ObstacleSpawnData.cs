using System;
using UnityEngine;

namespace Aster.Gameplay.Obstacles
{
    [Serializable]
    public class ObstacleSpawnData
    {
        public GameObject obstaclePrefab;
        public Vector3 position;
        public Vector3 rotation;  // New field for rotation

    }
}