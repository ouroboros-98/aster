using System;
using Aster.Utils.Attributes;
using TNRD;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Aster.Gameplay.Waves
{
    // [System.Serializable]
    // public class BaseWave : IWaveElement
    // {
    //     [SerializeField] protected int       NumOfEnemies;
    //     [SerializeField] protected float     MinAngle;
    //     [SerializeField] protected float     MaxAngle;
    //     [SerializeField] protected float     delay;
    //     [SerializeField] protected SpawnType spawnType;
    //
    //     public float     Delay     => delay;
    //     public SpawnType SpawnType => spawnType;
    //
    //
    //     protected EnemySpawner EnemySpawner;
    //
    //     public void OnWaveStart(WaveExecutionContext context)
    //     {
    //         EnemySpawner = context.Spawner;
    //         for (int i = 0; i < NumOfEnemies; i++)
    //         {
    //             EnemySpawner.SpawnEnemy(Random.Range(MinAngle, MaxAngle));
    //         }
    //     }
    //
    //     public virtual void OnWaveEnd()
    //     {
    //         AsterEvents.Instance.OnWaveEnd?.Invoke(1);
    //     }
    //
    //     public void UpdateNumOfEnemies(int numOfEnemies)
    //     {
    //         NumOfEnemies = numOfEnemies;
    //     }
    // }
}