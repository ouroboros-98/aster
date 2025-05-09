using System;
using Aster.Core;
using Aster.Utils;
using UnityEngine;

public class EntityHover : AsterMono
{
    [SerializeField]
    private float amplitude = 1f;

    [SerializeField]
    private float frequency = 1f;

    private float baseY;

    void Awake()
    {
        baseY = transform.position.y;
    }

    private void OnDisable()
    {
        transform.position = transform.position.With(y: baseY);
    }

    void Update()
    {
        float newY = baseY + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = transform.position.With(y: newY);
    }
}