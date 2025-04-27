using System;
using Aster.Core;
using Aster.Utils;
using DependencyInjection;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class TowerOptionsAnimator : AsterMono
{
    enum State
    {
        OnScreen,
        MovingDown,
        OffScreen,
        MovingUp
    }

    [SerializeField] private RectTransform         optionsPanel;
    [SerializeField] private float                 baseSpeed              = 2000f; // Pixels per second
    [SerializeField] private SerializableTimer     idleTime               = new(3f);
    [SerializeField] private int                   onScreenBottomPadding  = 32;
    [SerializeField] private int                   offScreenBottomPadding = -128;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [Inject]         private InputHandler          inputHandler;

    private Vector2 _originalPos;
    private float   _timer;
    private State   _state;
    private Tween   _currentTween;

    private void Awake()
    {
        ValidateComponent(ref layoutGroup);

        idleTime.OnTimerStop += SlideDown;
        _state               =  State.OnScreen;
    }

    private void OnEnable()
    {
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

        idleTime.Start();
    }

    public void OnGetTowerOptions()
    {
        if (_state == State.OffScreen || _state == State.MovingDown) SlideUp();
        if (_state == State.OnScreen)
        {
            idleTime.Start();
        }
    }

    private void SlideDown()
    {
        KillTween();

        _state = State.MovingDown;

        float distance = Mathf.Abs(onScreenBottomPadding - offScreenBottomPadding);
        float duration = distance / baseSpeed;

        _currentTween = AnimateBottomPadding(offScreenBottomPadding, duration)
           .OnComplete(() =>
                       {
                           layoutGroup.padding.bottom = offScreenBottomPadding;
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

        float distance = Mathf.Abs(onScreenBottomPadding - offScreenBottomPadding);
        float duration = .1f;

        _currentTween = AnimateBottomPadding(onScreenBottomPadding, duration).OnComplete(() =>
                     {
                         layoutGroup.padding.bottom =
                             onScreenBottomPadding;
                         idleTime.Start();
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

    private void KillTween()
    {
        if (_currentTween != null && _currentTween.IsActive())
            _currentTween.Complete();
    }
}