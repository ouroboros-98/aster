using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasImageFader : MonoBehaviour
{
    [SerializeField] private Image canvasImage; // Assign the canvas image in the Inspector
    [SerializeField] private float fadeDuration = 1f; // Duration of the fade-out

    // Call this method to start fading out the image.
    private void FadeOut()
    {
        if (canvasImage != null)
        {
            canvasImage.DOFade(0f, fadeDuration).SetEase(Ease.Linear);
        }
        else
        {
            Debug.LogWarning("Canvas image not assigned.");
        }
    }

    private void Awake()
    {
        FadeOut();
    }
}