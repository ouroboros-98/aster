using UnityEngine;

public class FixedAspectRatio : MonoBehaviour
{
    [Tooltip("Lock to this width/height ratio, e.g. 16/9")]
    public float targetAspect = 16f / 9f;

    Camera cam;
    float lastAspect;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        // only update when the ratio really changes
        if (Mathf.Abs(windowAspect - lastAspect) > 0.001f)
        {
            lastAspect = windowAspect;
            ApplyLetterbox(windowAspect);
        }
    }

    void ApplyLetterbox(float windowAspect)
    {
        float scale = windowAspect / targetAspect;
        Rect r = cam.rect;

        if (scale < 1f)
        {
            // taller than target: letterbox
            r.width = 1f;
            r.height = scale;
            r.x = 0;
            r.y = (1f - scale) / 2f;
        }
        else
        {
            // wider than target: pillarbox
            float inv = 1f / scale;
            r.width = inv;
            r.height = 1f;
            r.x = (1f - inv) / 2f;
            r.y = 0;
        }

        cam.rect = r;
    }
}