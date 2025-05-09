using Aster.Core;
using UnityEngine;

public class EntityHover : AsterMono
{
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float frequency = 1f;

    private float baseY;

    void Start()
    {
        baseY = transform.position.y;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = baseY + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = pos;
    }
}
