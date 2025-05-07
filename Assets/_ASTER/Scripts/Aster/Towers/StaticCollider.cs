using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StaticCollider : MonoBehaviour
{
    private Vector3 _fixedPosition;

    private void Awake()
    {
        // remember the world‐space position at start
        _fixedPosition = transform.position;
    }

    private void LateUpdate()
    {
        // force the collider to stay at that world‐space position
        transform.position = _fixedPosition;
    }
}