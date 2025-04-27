using Aster.Core;
using DependencyInjection;
using UnityEngine;
using DG.Tweening;

public class TowerOptionsAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform optionsPanel;
    [SerializeField] private float baseSpeed = 2000f; // Pixels per second
    [SerializeField] private float idleTime = 3f;
    [SerializeField] private Vector2 offScreenPosition = new Vector2(0, -1000);
    [Inject]         private InputHandler        inputHandler;
    private Vector2 _originalPos;
    private float _timer;
    private bool _isDown;
    private Tween _currentTween;
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
        if (optionsPanel != null)
            _originalPos = optionsPanel.anchoredPosition;
        
        
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (!_isDown && _timer >= idleTime)
            SlideDown();
    }

    public void OnGetTowerOptions()
    {
        _timer = 0f;
        if (_isDown)
            SlideUp();
    }

    private void SlideDown()
    {
        KillTween();
        _isDown = true;
        float distance = Vector2.Distance(optionsPanel.anchoredPosition, offScreenPosition);
        float duration = distance / baseSpeed;
        _currentTween = optionsPanel.DOAnchorPos(offScreenPosition, duration);
    }

    private void SlideUp()
    {
        KillTween();
        _isDown = false;
        float distance = Vector2.Distance(optionsPanel.anchoredPosition, _originalPos);
        float duration = distance / baseSpeed;
        _currentTween = optionsPanel.DOAnchorPos(_originalPos, duration);
    }

    private void KillTween()
    {
        if (_currentTween != null && _currentTween.IsActive())
            _currentTween.Kill();
    }
}