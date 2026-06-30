using UnityEngine;

public abstract class Area : MonoBehaviour
{
    [SerializeField] private BoxCollider _placement;

    public bool IsInside(Vector3 point) =>
    _placement.bounds.Contains(point);
}