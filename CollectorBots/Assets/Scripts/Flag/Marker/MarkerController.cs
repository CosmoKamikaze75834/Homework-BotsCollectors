using UnityEngine;

public class MarkerController : MonoBehaviour
{
    [SerializeField] private Marker _marker;

    private Marker _currentMarker;

    public Vector3? GetMarkerPosition()
    {
        if (HasMarker() == false)
            return null;

        return _currentMarker.transform.position;
    }

    public void MoveMarker(Vector3 position)
    {
        if (HasMarker() == false)
            return;

        _currentMarker.transform.position = position;
    }

    public void CreateMarker(Vector3 position)
    {
        if (HasMarker() == false)
            _currentMarker = Instantiate(_marker, position, Quaternion.identity);
    }

    public void DestroyMarker()
    {
        if (HasMarker())
        {
            Destroy(_currentMarker.gameObject);
            _currentMarker = null;
        }
    }

    private bool HasMarker() =>
        _currentMarker != null;
}