using System;
using System.Collections.Generic;
using Aster.Core;
using Aster.Entity.Enemy;
using Aster.Utils.Attributes;
using TNRD;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    [Serializable]
    public class SpawnList : WaveElementBase
    {
        [SerializeField] private float[] angles;

        private bool _spawned = false;

        protected override void OnWaveStart()
        {
            _spawned = false;

            for (var i = 0; i < angles.Length; i++)
            {
                Context.Spawner.SpawnEnemy(angles[i]);
            }

            _spawned = true;
        }

        protected override bool ShouldWaveEnd()
        {
            return base.ShouldWaveEnd() && _spawned;
        }

        public override void Reset()
        {
            base.Reset();
            _spawned = false;
        }
    }

    [Serializable]
    public class SpawnMultipleEnemies : WaveElementBase
    {
        [SerializeReference, SerializeReferenceDropdown] private IEnemyCountProvider enemyCount;
        [SerializeReference, SerializeReferenceDropdown] private IAnglePicker        anglePicker;

        private bool _spawned = false;

        protected override void OnWaveStart()
        {
            _spawned = false;

            for (int i = 0; i < enemyCount.GetEnemyCount(); i++)
            {
                Context.Spawner.SpawnEnemy(anglePicker.GetAngle());
            }

            _spawned = true;
        }

        protected override bool ShouldWaveEnd()
        {
            return base.ShouldWaveEnd() && _spawned;
        }

        public override void Reset()
        {
            base.Reset();
            _spawned = false;
        }
    }

    [Serializable]
    public class SpawnSingleEnemy : WaveElementBase
    {
        [SerializeReference, SerializeReferenceDropdown] private IAnglePicker anglePicker;

        private bool _spawned = false;

        protected override void OnWaveStart()
        {
            _spawned = false;

            Context.Spawner.SpawnEnemy(anglePicker.GetAngle());
            _spawned = true;
        }

        protected override bool ShouldWaveEnd()
        {
            return base.ShouldWaveEnd() && _spawned;
        }

        public override void Reset()
        {
            base.Reset();
            _spawned = false;
        }
    }

    [Serializable]
    public abstract class WaveElementBase : IWaveElement
    {
        [SerializeReference, SerializeReferenceDropdown] private IWaveTimingHandler timingHandler;

        protected List<EnemyController> Enemies;
        protected WaveExecutionContext  Context { get; private set; }

        public WaveStatus Status { get; private set; } = WaveStatus.NotStarted;

        public virtual void Reset()
        {
            Enemies = new();
            Status  = WaveStatus.NotStarted;
            timingHandler?.Reset();
        }

        protected virtual void OnWavePrestart()
        {
        }

        protected virtual void OnWaveStart()
        {
        }

        protected virtual void OnWaveEnd()
        {
        }

        protected virtual bool ShouldWaveEnd() => Enemies.Count == 0;

        public void PreStart(WaveExecutionContext context)
        {
            Context = context;
            timingHandler?.OnPrestart(context);
            OnWavePrestart();

            Status = WaveStatus.PreStart;
        }

        private void Start()
        {
            Enemies.Clear();

            AsterEvents.Instance.OnEnemySpawn += OnEnemySpawn;
            AsterEvents.Instance.OnEnemyDeath += OnEnemyDeath;

            OnWaveStart();

            Status = WaveStatus.InProgress;
        }

        public virtual void Update()
        {
            if (Status == WaveStatus.PreStart)
            {
                if (timingHandler == null)
                    Start();
                else if (timingHandler.CanStart())
                    Start();
            }
            else if (Status == WaveStatus.InProgress)
            {
                if (ShouldWaveEnd()) End();
            }
        }

        protected virtual void OnEnemySpawn(EnemyController enemy)
        {
            Debug.Log($"Wave: {Context.WaveIndex}, Enemy Spawned: {enemy.name}");
            Enemies.Add(enemy);
        }

        protected virtual void OnEnemyDeath(EnemyController enemy)
        {
            if (!Enemies.Contains(enemy)) return;
            Debug.Log($"Wave: {Context.WaveIndex}, Enemy Died: {enemy.name}");
            Enemies.Remove(enemy);
        }

        private void End()
        {
            AsterEvents.Instance.OnEnemySpawn -= OnEnemySpawn;
            AsterEvents.Instance.OnEnemyDeath -= OnEnemyDeath;

            Status = WaveStatus.Done;

            Debug.Log($"Wave: {Context.WaveIndex} DONE");

            OnWaveEnd();
        }
    }
}