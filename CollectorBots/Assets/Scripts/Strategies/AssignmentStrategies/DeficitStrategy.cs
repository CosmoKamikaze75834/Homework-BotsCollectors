using System.Collections.Generic;
using UnityEngine;

public class DeficitStrategy : IResourceSelectionStrategy
{
    private IResourceStorage _storage;

    public DeficitStrategy(IResourceStorage storage) => _storage = storage;

    public Resource SelectResource(Vector3 collectorPosition, List<Resource> availableResources)
    {
        if(availableResources == null || availableResources.Count == 0)
            return null;

        ResourceType minimumResourceType = GetMinimumResourceType(availableResources);

        foreach (var resource in availableResources)
        {
            if(resource.ResourceType == minimumResourceType)
                return resource;

        }

        return null;
    }

    private ResourceType GetMinimumResourceType(List<Resource> availableResources)
    {
        ResourceType type = default;
        int minValue = int.MaxValue;

        foreach (Resource resource in availableResources)
        {
            int current = _storage.GetAmount(resource.ResourceType);

            if(current < minValue)
            {
                minValue = current;
                type = resource.ResourceType;
            }
        }

        return type;
    }
}