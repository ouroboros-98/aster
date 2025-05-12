using System;
using System.Collections.Generic;
using Aster.Core;
using Aster.Entity.Enemy;
using Aster.Utils.Attributes;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TNRD;
using UnityEngine;

namespace Aster.Gameplay.Waves
{
    [Serializable]
    public class SpawnList : WaveElementBase
    {
        [Serializable]
        [InlineProperty]
        class Entry
        {
            [SerializeField]
            [InlineProperty]
            [HideLabel]
            private EnemyPicker _enemyPicker = new();

            [SerializeReference]
            [InlineProperty]
            [HideLabel]
            private IAnglePicker anglePicker = new AnglePickerConstant();

            public float           Angle => anglePicker.GetAngle();
            public EnemyController Enemy => _enemyPicker.GetEnemy();
        }

        [PropertyOrder(1)]
        [SerializeField]
        [Title("Spawn List")]
        [TableList(AlwaysExpanded = true)]
        [HideLabel]
        [InlineProperty]
        private Entry[] _entries;

        private bool _spawned = false;

        protected override void OnWaveStart()
        {
            _spawned = false;

            for (var i = 0; i < _entries.Length; i++)
            {
                Context.SpawnEnemy(_entries[i].Angle, _entries[i].Enemy, Enemies);
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
        [PropertyOrder(2)]
        [Title("Count")]
        [HorizontalGroup("Enemy", 0.5f)]
        [SerializeReference]
        [InlineProperty]
        [HideLabel]
        private IEnemyCountProvider enemyCount = new ConstantEnemyCount();

        [PropertyOrder(1)]
        [Title("Enemy")]
        [HorizontalGroup("Enemy", 0.5f)]
        [SerializeField]
        [InlineProperty]
        [HideLabel]
        private EnemyPicker enemyPicker = new();

        [PropertyOrder(3)]
        [Title("Angle")]
        [SerializeReference]
        [InlineProperty]
        [HideLabel]
        private IAnglePicker anglePicker = new AnglePickerConstant();

        private bool _spawned = false;

        protected override void OnWaveStart()
        {
            _spawned = false;

            for (int i = 0; i < enemyCount.GetEnemyCount(); i++)
            {
                Context.SpawnEnemy(anglePicker.GetAngle(), enemyPicker.GetEnemy(), Enemies);
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
    [InlineProperty]
    public class SpawnSingleEnemy : WaveElementBase
    {
        [PropertyOrder(1)]
        [HorizontalGroup("Enemy", 0.5f)]
        [Title("Enemy")]
        [SerializeField]
        [InlineProperty]
        [HideLabel]
        private EnemyPicker enemyPicker = new();

        [PropertyOrder(2)]
        [HorizontalGroup("Enemy", 0.5f)]
        [Title("Angle")]
        [SerializeReference]
        [InlineProperty]
        [HideLabel]
        private IAnglePicker anglePicker = new AnglePickerConstant();


        private bool _spawned = false;

        protected override void OnWaveStart()
        {
            _spawned = false;

            Context.SpawnEnemy(anglePicker.GetAngle(), enemyPicker.GetEnemy(), Enemies);
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
    [InlineProperty]
    public abstract class WaveElementBase : IWaveElement
    {
        [SerializeField, AllowNesting, NaughtyAttributes.ReadOnly]
        private WaveStatus _status = WaveStatus.NotStarted;

        [PropertyOrder(0)]
        [SerializeReference]
        [Title("Timing")]
        [HideLabel]
        [InlineProperty]
        private IWaveTimingHandler timingHandler = new AfterPrevious();

        protected List<EnemyController> Enemies;
        protected WaveExecutionContext  Context { get; private set; }

        public WaveStatus Status
        {
            get => _status;
            private set { _status = value; }
        }

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
            AsterEvents.Instance.OnWaveEnd?.Invoke();
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

            AsterEvents.Instance.OnWaveStart?.Invoke(Context.WaveIndex);
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
            // Debug.Log($"Wave: {Context.WaveIndex}, Enemy Spawned: {enemy.name}");
            // Enemies.Add(enemy);
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