using UnityEngine;

public class MarkerController : MonoBehaviour
{
    [SerializeField] private Marker _marker;

    private Marker _placementMarker;

    public Vector3? GetMarkerPosition()
    {
        if (_placementMarker == null)
            return null;

        return _placementMarker.transform.position;
    }

    public void MoveMarker(Vector3 position)
    {
        if (_placementMarker == null)
            return;

        _placementMarker.transform.position = position;
    }

    public void CreateMarker(Vector3 position)
    {
        if (_placementMarker == null)
            _placementMarker = Instantiate(_marker, position, Quaternion.identity);
    }

    public void DestroyMarker()
    {
        if (_placementMarker != null)
        {
            Destroy(_placementMarker.gameObject);
            _placementMarker = null;
        }
    }
}