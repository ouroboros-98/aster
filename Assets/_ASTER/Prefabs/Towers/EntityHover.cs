using System;
using Aster.Core;
using Aster.Core.Interactions.Grab;
using Aster.Utils;
using UnityEngine;

public class EntityHover : AsterMono, IDisableOnGrab
{
    [SerializeField]
    private float amplitude = 1f;

    [SerializeField]
    private float frequency = 1f;

    private float baseY;

    private float t = 0f;

    void Awake()
    {
        baseY = transform.position.y;
    }

    private void OnEnable()
    {
        t = 0;
    }

    private void OnDisable()
    {
        transform.position = transform.position.With(y: baseY);
    }

    void Update()
    {
        t += Time.deltaTime;
        float newY = baseY + Mathf.Sin(t * frequency) * amplitude;
        transform.position = transform.position.With(y: newY);
    }
}