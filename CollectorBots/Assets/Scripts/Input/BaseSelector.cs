using UnityEngine;

public class BaseSelector : MonoBehaviour
{
    private const string NameBase = "Base";
    private const string NameGround = "Ground";

    [SerializeField] private RaycastHandler _raycast;
    [SerializeField] private FlagPlacementValidator _placementValidator;

    private Base _selectedBase;

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

    //перемещаем мышкой по уровню
    private void ProcessMouseMove(RaycastHit hit)
    {
        if (_selectedBase == null)//база выбрана
            return;

        if (CheckGround(hit) && _placementValidator.CanPlace(hit.point))//на земле и в безопасной зоне
            _selectedBase.MarkerController.MoveMarker(hit.point);//передвигаем маркер если он есть
    }

    //логика клика по земле
    private void PlaceFlag()
    {
        if (_selectedBase == null)
            return;

        Vector3? position = _selectedBase.MarkerController.GetMarkerPosition();//если установлен маркер

        if (position != null)
            _selectedBase.FlagPlacer.PlaceFlag(position.Value);//ставим флаг
    }

    //логика выбора базы
    private void SelectBase(RaycastHit hit)
    {
        if (hit.collider.gameObject.TryGetComponent(out Base component) == false)
            return;

        if (component.FlagPlacer.HasFlag())
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
        _selectedBase.BaseSelectionView.Deselect();
        _selectedBase.MarkerController.DestroyMarker();
        _selectedBase = null;
    }

    private void ApplySelection(Base component, RaycastHit hit)
    {
        _selectedBase = component;
        component.BaseSelectionView.Select();
        component.MarkerController.CreateMarker(hit.point);
    }
}