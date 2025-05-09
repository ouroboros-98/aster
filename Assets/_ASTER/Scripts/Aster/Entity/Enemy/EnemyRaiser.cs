using UnityEngine;
using DG.Tweening;

public class RaiseOnTrigger : MonoBehaviour
{
    [SerializeField] private float raiseAmount  = 2f;
    [SerializeField] private float tweenDuration = 1f;
    [SerializeField] private float threshold     = 0.01f;

    private float baseY;
    private int   triggersInside;
    private Tween currentTween;

    private void Awake()
    {
        baseY = transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Block")) return;
        if (triggersInside++ > 0) return;    // only on first enter
        StartTween(baseY + raiseAmount);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Block")) return;
        if (--triggersInside > 0) return;    // only when all exits
        StartTween(baseY);
    }

    private void StartTween(float targetY)
    {
        float currentY = transform.position.y;
        float distance = Mathf.Abs(targetY - currentY);
        if (distance < threshold) return;     // already near target

        currentTween?.Kill();
        float factor = distance / raiseAmount;
        currentTween = transform
            .DOMoveY(targetY, tweenDuration * factor)
            .SetEase(Ease.OutQuad);
    }
}