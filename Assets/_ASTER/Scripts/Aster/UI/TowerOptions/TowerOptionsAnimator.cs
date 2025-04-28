using System;
using Aster.Core;
using Aster.Utils;
using DependencyInjection;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace Aster.UI
{
    public partial class TowerOptionsAnimator : AsterMono
    {
        enum State
        {
            OnScreen,
            MovingDown,
            OffScreen,
            MovingUp
        }

        private Configuration _configuration;

        [SerializeField] private RectTransform         optionsPanel;
        [SerializeField] private HorizontalLayoutGroup layoutGroup;
        [Inject]         private InputHandler          inputHandler;

        private Vector2 _originalPos;
        private float   _timer;
        private State   _state;
        private Tween   _currentTween;

        private bool SlideDownEnabled => _configuration.SlideDown;

        private void Awake()
        {
            ValidateComponent(ref layoutGroup);

            _configuration = AsterConfiguration.Instance.TowerOptionsAnimator;

            _state = State.OnScreen;
        }

        private void OnEnable()
        {
            _configuration.IdleTime.OnTimerStop += SlideDown;

            // Subscribe to events from InputHandler
            if (inputHandler != null)
            {
                inputHandler.OnR1          += OnGetTowerOptions;
                inputHandler.OnL1          += OnGetTowerOptions;
                inputHandler.OnSelectTower += OnGetTowerOptions;
            }
        }

        private void OnDisable()
        {
            _configuration.IdleTime.OnTimerStop -= SlideDown;

            if (inputHandler != null)
            {
                inputHandler.OnR1          -= OnGetTowerOptions;
                inputHandler.OnL1          -= OnGetTowerOptions;
                inputHandler.OnSelectTower -= OnGetTowerOptions;
            }
        }


        private void Start()
        {
            if (optionsPanel != null) _originalPos = optionsPanel.anchoredPosition;

            _configuration.IdleTime.Start();
        }

        public void OnGetTowerOptions()
        {
            if (!SlideDownEnabled) return;

            if (_state == State.OffScreen || _state == State.MovingDown) SlideUp();
            if (_state == State.OnScreen)
            {
                _configuration.IdleTime.Start();
            }
        }

        private void SlideDown()
        {
            if (!SlideDownEnabled) return;

            KillTween();

            _state = State.MovingDown;

            float distance = Mathf.Abs(_configuration.OnScreenBottomPadding - _configuration.OffScreenBottomPadding);
            float duration = distance / _configuration.BaseSpeed;

            _currentTween = AnimateBottomPadding(_configuration.OffScreenBottomPadding, duration)
               .OnComplete(() =>
                           {
                               layoutGroup.padding.bottom = _configuration.OffScreenBottomPadding;
                               _state                     = State.OffScreen;
                           });
        }

        private void SetBottomPadding(int x)
        {
            layoutGroup.padding.bottom = x;
            LayoutRebuilder.ForceRebuildLayoutImmediate(optionsPanel);
        }

        private void SlideUp()
        {
            KillTween();

            _state = State.MovingUp;

            float distance = Mathf.Abs(_configuration.OnScreenBottomPadding - _configuration.OffScreenBottomPadding);
            float duration = .1f;

            _currentTween = AnimateBottomPadding(_configuration.OnScreenBottomPadding, duration).OnComplete(() =>
                         {
                             layoutGroup.padding.bottom =
                                 _configuration.OnScreenBottomPadding;
                             _configuration.IdleTime.Start();
                             _state = State.OnScreen;
                         });
        }

        private TweenerCore<int, int, NoOptions> AnimateBottomPadding(int endPos, float duration)
        {
            return DOTween.To(() => layoutGroup.padding.bottom,
                              SetBottomPadding,
                              endPos,
                              duration)
                ;
        }

        private void Update()
        {
            if (!SlideDownEnabled && _state != State.OnScreen)
            {
                SlideUp();
            }

            if (SlideDownEnabled
             && _state == State.OnScreen
             && !_configuration.IdleTime.IsRunning)
            {
                _configuration.IdleTime.Start();
            }
        }

        private void KillTween()
        {
            if (_currentTween != null && _currentTween.IsActive())
                _currentTween.Complete();
        }

        private void Reset()
        {
            _configuration = Config.TowerOptionsAnimator;
        }
    }
}