using UnityEngine;

public class BaseSelector : MonoBehaviour
{
    [SerializeField] private RaycastHandler _raycast;
    [SerializeField] private Base _baseFlag;
    [SerializeField] private FlagPlacementMarker _marker;

    private Base _base;
    private FlagPlacementMarker _placementMarker;

    private void OnEnable()
    {
        _raycast.OnRaycastHit += ProcessBeam;
        _raycast.OnMouseMoveHit += MoveMarker;
    }

    private void OnDisable()
    {
        _raycast.OnRaycastHit -= ProcessBeam;
        _raycast.OnMouseMoveHit -= MoveMarker;
    }

    private void ProcessBeam(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Base"))
        {
            Debug.Log("Кликнули по базе");

            if (_base != null)
                _base.Deselect();

            if (hit.collider.gameObject.TryGetComponent(out Base component))
            {
                SetBase(component);
                component.Select();

                if (_placementMarker == null)
                {
                    _placementMarker = Instantiate(_marker, hit.point, Quaternion.identity);
                    Debug.Log($"Marker created at {_placementMarker.transform.position}");
                }
            }
        }

        //if (hit.collider.CompareTag("Ground"))
        //{
        //    if(_base!= null)
        //    {
        //        Debug.Log("Ставим флаг");
        //        Vector3 coordinates = hit.point;//коррдинаты места куда кликнул игрок.
        //        Debug.Log(coordinates);
        //        _baseFlag.PlaceFlag(coordinates);
        //    }
        //}
    }

    private void SetBase(Base currentBase)
    {
        _base = currentBase;
    }

    private void MoveMarker(RaycastHit hit)
    {
        if(_placementMarker != null)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                if (_base != null)
                {
                    Debug.Log("попали в землю");

                    Debug.Log($"Hit point = {hit.point}");
                    Debug.Log($"Marker position before = {_placementMarker.transform.position}");

                    _placementMarker.transform.position = hit.point;

                    Debug.Log($"Marker position after = {_placementMarker.transform.position}");
                }
            }
        }
    }
}