using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private ResourceSelectionStrategyFactory _factory;
    [SerializeField] private CollectorMovement _movement;
    [SerializeField] private CargoHandler _cargoHandler;
    [SerializeField] private CollectionPoint _collectionPoint;

    private CollectorState _state;

    private IResourceCollectionStrategy _strategies;

    private Vector3 _startPosition;

    public Transform Target { get; private set; }
    public bool IsBusy { get; private set; }

    private void Start()
    {
        _startPosition = transform.position;
        _strategies = _factory.InitializeStrategy();
    }

    private void OnEnable() => _movement.Arrived += OnArrivedAtDestination;

    private void OnDisable() => _movement.Arrived -= OnArrivedAtDestination;

    public void SetTarget(Vector3 targetPosition, Transform target)
    {
        if (IsBusy)
            return;

        Target = target;
        _state = CollectorState.MovingToResource;
        IsBusy = true;

        _movement.SetDestination(targetPosition);
    }

    public void OnArrivedAtDestination()
    {
        switch (_state)
        {
            case CollectorState.Idle:
                break;

            case CollectorState.MovingToResource:
                _strategies.Collect(this);
                _state = CollectorState.MovingToBase;
                _movement.SetDestination(_startPosition);
                break;

            case CollectorState.MovingToBase:
                Resource deliveredResource = null;

                if (Target != null)
                    Target.TryGetComponent(out deliveredResource);

                if (deliveredResource != null)
                {
                    _cargoHandler.DetachResource();
                    deliveredResource.InvokeDisappearedEvent();
                    _collectionPoint.AcceptDelivery(this, deliveredResource);
                }

                _state = CollectorState.Idle;
                IsBusy = false;
                Target = null;
                break;
        }
    }

    public void AttempToPickupCurrentResource() => _cargoHandler.PickupResource(Target);

    public void Stop() => _movement.Stop();

    public void Resume() => _movement.Resume();
}