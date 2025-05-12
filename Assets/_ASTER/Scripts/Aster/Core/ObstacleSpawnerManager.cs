using System.Collections.Generic;
using Aster.Core;
using Aster.Gameplay.Obstacles;
using DG.Tweening;
using UnityEngine;

namespace Aster.Core
{
    public class ObstacleSpawnerManager : AsterMono
    {
        [SerializeField]
        private List<WaveObstacleData> waveObstacleData;

        [SerializeField]
        private float bottomStartPoint;

        private List<GameObject> _currentObstacles;
        private int              _waveCounter;

        private void Awake()
        {
            _currentObstacles = new List<GameObject>();
        }

        private void OnEnable()
        {
            // AsterEvents.Instance.OnWaveStart += HandleWaveStart;
        }

        private void OnDisable()
        {
            // AsterEvents.Instance.OnWaveStart -= HandleWaveStart;
        }

        private void HandleWaveStart(int obj)
        {
            _waveCounter++;
            WaveObstacleData data = waveObstacleData.Find(w => w.waveNumber == _waveCounter);

            if (data != null)
            {
                RemoveCurrentObstacles();
                SpawnObstacles(data);
            }
        }

        private void SpawnObstacles(WaveObstacleData data)
        {
            foreach (ObstacleSpawnData spawnData in data.obstacles)
            {
                SpawnObstacle(spawnData);
            }
        }

        public void SpawnObstacle(ObstacleSpawnData spawnData)
        {
            GameObject obj = Instantiate(spawnData.prefab);

            // Set the position (spawn below and rise)
            Vector3 startPosition = spawnData.position + Vector3.down * bottomStartPoint; // Spawn 5 units below
            obj.transform.position = startPosition;

            // Set the rotation
            obj.transform.rotation = Quaternion.Euler(spawnData.rotation);

            // Animate to target position over 1 second with ease-out
            obj.transform.DOMove(spawnData.position, 2f).SetEase(Ease.OutBack);

            // Animate rotation if needed (optional)
            // obj.transform.DORotate(spawnData.rotation, 1f, RotateMode.FastBeyond360).SetEase(Ease.OutBack);

            _currentObstacles.Add(obj);
        }


        public void RemoveCurrentObstacles()
        {
            foreach (var obj in _currentObstacles)
            {
                if (obj != null)
                {
                    // Animate down (5 units below current Y) over 1 second
                    obj.transform.DOMoveY(obj.transform.position.y - bottomStartPoint, 1f)
                       .SetEase(Ease.InBack)
                       .OnComplete(() => { Destroy(obj); });
                }
            }

            _currentObstacles.Clear(); // Clear the list immediately
        }
    }
}