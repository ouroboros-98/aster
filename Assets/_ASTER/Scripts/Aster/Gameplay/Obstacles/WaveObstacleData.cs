using System;
using System.Collections.Generic;

namespace Aster.Gameplay.Obstacles
{
    [Serializable]
    public class WaveObstacleData
    {
        public int waveNumber;
        public List<ObstacleSpawnData> obstacles;
    }
}