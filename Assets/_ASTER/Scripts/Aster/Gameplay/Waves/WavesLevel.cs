using System;
using System.Collections.Generic;
using System.Linq;
using Aster.Core;
using Aster.Entity.Enemy;
using NUnit.Framework;
using TNRD;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Waves/New Level")]
    public class WavesLevel : ScriptableObject
    {
        [SerializeReference, SerializeReferenceDropdown] private IWaveElement[] waves;

        public IReadOnlyList<IWaveElement> Waves => waves.ToList();

        public void Reset()
        {
            foreach (IWaveElement wave in waves)
            {
                wave.Reset();
            }
        }
    }

    public class LevelExecution
    {
        private readonly WavesLevel _level;

        public bool IsDone { get; private set; }

        public LevelExecution(WavesLevel level)
        {
            _level = level;
            _level.Reset();
            IsDone = false;
        }

        public void Initialize(EnemySpawner spawner)
        {
            int          currentIndex = 0;
            IWaveElement previous     = null;

            _level.Reset();

            foreach (IWaveElement wave in _level.Waves)
            {
                WaveExecutionContext context = new WaveExecutionContext(spawner, currentIndex, previous);
                wave.PreStart(context);
                previous = wave;
                currentIndex++;
            }
        }

        public void Update()
        {
            bool allDone = true;

            foreach (IWaveElement wave in _level.Waves)
            {
                if (wave.Status == WaveStatus.Done) continue;

                allDone = false;
                wave.Update();
            }

            if (allDone) IsDone = true;
        }
    }
}