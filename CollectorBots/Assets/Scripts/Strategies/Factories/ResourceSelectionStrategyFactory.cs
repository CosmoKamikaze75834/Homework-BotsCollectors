using UnityEngine;

public class ResourceSelectionStrategyFactory : MonoBehaviour
{
    [SerializeField] private CollectionMode _selection;

    public IResourceCollectionStrategy InitializeStrategy()
    {
        if (_selection == CollectionMode.Direct)
            return new DirectPickupStrategy();
        else if (_selection == CollectionMode.Delayed)
            return new DelayedPickupStrategy();

        return null;
    }
}