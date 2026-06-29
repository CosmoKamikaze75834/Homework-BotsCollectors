using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    [SerializeField] private BoxCollider _placement;

    public bool Inside(Vector3 point) =>
        _placement.bounds.Contains(point);
}