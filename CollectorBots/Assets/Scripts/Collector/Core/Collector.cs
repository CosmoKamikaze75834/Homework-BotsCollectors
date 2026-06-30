using System;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private ResourceSelectionStrategyFactory _factory;
    [SerializeField] private CollectorMovement _movement;
    [SerializeField] private CargoHandler _cargoHandler;
    [SerializeField] private CollectionPoint _collectionPoint;

    private CollectorState _state;
    private IResourceCollectionStrategy _strategy;
    private Vector3 _startPosition;
    private Base _ownerBase;

    public Transform Target { get; private set; }
    public bool IsBusy { get; private set; }

    public event Action<Collector> ArrivedToFlag;

    private void Start()
    {
        _startPosition = transform.position;
        _strategy = _factory.InitializeStrategy();
    }

    private void OnEnable() =>
        _movement.Arrived += OnArrivedAtDestination;

    private void OnDisable() =>
        _movement.Arrived -= OnArrivedAtDestination;

    public void SetTarget(Vector3 targetPosition, Transform target) =>
        Send(targetPosition, target, CollectorState.MovingToResource);

    public void SendToFlag(Vector3 targetPosition, Transform target) =>
        Send(targetPosition, target, CollectorState.MovingToFlag);

    public void OnArrivedAtDestination()
    {
        switch (_state)
        {
            case CollectorState.Idle:
                break;

            case CollectorState.MovingToResource:
                HandleResourceArrival();
                break;

            case CollectorState.MovingToBase:
                HandleBaseArrival();
                break;

            case CollectorState.MovingToFlag:
                HandleFlagArrival();
                break;
        }
    }

    public void AttemptToPickupCurrentResource() => 
        _cargoHandler.PickupResource(Target);

    public void Stop() => 
        _movement.Stop();

    public void Resume() => 
        _movement.Resume();

    public void SetBase(Base currentbase) =>
        _ownerBase = currentbase;

    public void SetPoint(CollectionPoint collectionPoint) =>
        _collectionPoint = collectionPoint;

    public void SetStartPosition(Vector3 startPosition) =>
        _startPosition = startPosition;

    private void Send(Vector3 targetPosition, Transform target, CollectorState state)
    {
        if (IsBusy)
            return;

        Target = target;
        IsBusy = true;
        _state = state;

        _movement.SetDestination(targetPosition);
    }

    private void SetIdle()
    {
        _state = CollectorState.Idle;
        IsBusy = false;
        Target = null;
    }

    private void HandleResourceArrival()
    {
        _strategy.Collect(this);
        _state = CollectorState.MovingToBase;
        _movement.SetDestination(_startPosition);
    }

    private void HandleBaseArrival()
    {
        Resource deliveredResource = null;

        if (Target != null)
            Target.TryGetComponent(out deliveredResource);

        if (deliveredResource != null)
        {
            _cargoHandler.DetachResource();
            deliveredResource.InvokeDisappearedEvent();
            _collectionPoint.AcceptDelivery(this, deliveredResource);
        }

        SetIdle();
    }

    private void HandleFlagArrival()
    {
        SetIdle();
        ArrivedToFlag?.Invoke(this);
    }
}