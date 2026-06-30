using System;
using UnityEngine;

public class BaseConstructionSender : MonoBehaviour
{
    [SerializeField] private BaseSelectionView _selectionView;
    [SerializeField] private MarkerController _markerController;
    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private BaseColonization _baseColonization;
    [SerializeField] private DistributorResources _distributor;

    private Action _clearSelection;

    public void Select(Action OnBotSet)
    {
        _clearSelection = OnBotSet;
        _selectionView.Select();
    }

    public void Deselect() => 
        _selectionView.Deselect();

    public void CreateMarker(Vector3 positiion) => 
        _markerController.CreateMarker(positiion);

    public void DestroyMarker() => 
        _markerController.DestroyMarker();

    public bool HasFlag() => 
        _flagPlacer.HasFlag();

    public void PlaceFlag(Vector3 position) => 
        _flagPlacer.PlaceFlag(position);

    public Vector3? GetMarkerPosition() => 
        _markerController.GetMarkerPosition();

    public void MoveMarker(Vector3 position) =>
        _markerController.MoveMarker(position);

    public void SendBotToFlag(Collector collector)
    {
        if (_baseColonization.TrySendBotToFlag(collector))
            _clearSelection?.Invoke();
    }

    public void ChangeStartegy(SelectionMode selectionMode) =>
        _distributor.ChangeStrategy(selectionMode);

    public SelectionMode GetCurrentStrategy() => 
        _distributor.CurrentStrategy;
}