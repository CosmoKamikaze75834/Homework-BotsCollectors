using System;
using UnityEngine;

public class BaseColonization : MonoBehaviour
{
    [SerializeField] private BaseConstructionCost _baseConstruction;
    [SerializeField] private BaseSpawner _baseSpawner;

    [SerializeField] private ResourceStorage _storage;
    [SerializeField] private Counter _counter;

    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private MarkerController _markerController;

    private Collector _builder;
    private bool _builderSent;

    public FlagPlacer FlagPlacer => _flagPlacer;

    public event Action<Collector> BuilderTransferred;

    private void OnEnable() =>
        _baseSpawner.OnBaseSpawned += OnBaseSpawned;

    private void OnDisable() =>
        _baseSpawner.OnBaseSpawned -= OnBaseSpawned;

    public void HandleBuilderArrived(Collector collector)
    {
        _flagPlacer.DestroyFlag();

        _baseConstruction.Cost.Deduct(_counter);

        collector.ArrivedToFlag -= HandleBuilderArrived;

        CreateNewBase(collector);

        CompleteColonization(collector);
    }

    public void OnBaseSpawned(Base newBase)
    {
        _builder.SetBase(newBase);
        _builder.SetPoint(newBase.CollectionPoint);
        newBase.DistributorResources.AddBot(_builder);
        _builder.SetStartPosition(_builder.transform.position);
    }

    public bool TrySendBotToFlag(Collector collector)
    {
        if (CanSendBuilder() == false)
            return false;

        _builderSent = true;
        _builder = collector;

        Transform currentFlag = _flagPlacer.GetFlagTransform();

        collector.SendToFlag(currentFlag.transform.position, currentFlag);
        _markerController.DestroyMarker();

        _builder.ArrivedToFlag += HandleBuilderArrived;

        return true;
    }

    private bool CanSendBuilder() =>
        _baseConstruction.Cost.CanAfford(_storage) &&
           _builderSent == false;

    private void CreateNewBase(Collector collector) =>
        StartCoroutine(_baseSpawner.LaunchCreateBase(collector.transform));

    private void CompleteColonization(Collector collector)
    {
        _builderSent = false;
        BuilderTransferred?.Invoke(collector);
    }
}