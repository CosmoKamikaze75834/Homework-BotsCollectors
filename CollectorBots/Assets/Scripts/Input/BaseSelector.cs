using UnityEngine;

public class BaseSelector : MonoBehaviour
{
    private const string NameBase = "Base";
    private const string NameGround = "Ground";

    [SerializeField] private RaycastHandler _raycast;
    [SerializeField] private FlagPlacementValidator _placementValidator;

    private BaseConstructionSender _selectedBase;

    private void OnEnable()
    {
        _raycast.OnRaycastHit += ProcessClick;
        _raycast.OnMouseMoveHit += ProcessMouseMove;
    }

    private void OnDisable()
    {
        _raycast.OnRaycastHit -= ProcessClick;
        _raycast.OnMouseMoveHit -= ProcessMouseMove;
    }

    private void ProcessClick(RaycastHit hit)
    {
        if (hit.collider.CompareTag(NameBase))
            SelectBase(hit);

        if (CheckGround(hit))
            PlaceFlag();
    }

    private bool CheckGround(RaycastHit hit) =>
        hit.collider.CompareTag(NameGround);

    private void ProcessMouseMove(RaycastHit hit)
    {
        if (_selectedBase == null)
            return;

        if (CheckGround(hit) && _placementValidator.CanPlace(hit.point))
            _selectedBase.MoveMarker(hit.point);
    }

    private void PlaceFlag()
    {
        if (_selectedBase == null)
            return;

        Vector3? position = _selectedBase.GetMarkerPosition();

        if (position != null)
            _selectedBase.PlaceFlag(position.Value);
    }

    private void SelectBase(RaycastHit hit)
    {
        if (hit.collider.gameObject.TryGetComponent(out BaseConstructionSender component) == false)
            return;

        if (component.HasFlag())
            return;

        if (_selectedBase != null && _selectedBase != component)
            return;

        if (_selectedBase != null && _selectedBase == component)
            DeselectBase();
        else
            ApplySelection(component, hit);
    }

    private void DeselectBase()
    {
        _selectedBase.Deselect();
        _selectedBase.DestroyMarker();
        _selectedBase = null;
    }

    private void ApplySelection(BaseConstructionSender component, RaycastHit hit)
    {
        _selectedBase = component;
        _selectedBase.Select(DeselectBase);
        _selectedBase.CreateMarker(hit.point);
    }
}