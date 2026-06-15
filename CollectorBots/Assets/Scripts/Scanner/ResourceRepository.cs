using System.Collections.Generic;
using UnityEngine;

public class ResourceRepository : MonoBehaviour
{
    private List<Resource> _availableResources = new List<Resource>();
    private List<Resource> _reservedResources = new List<Resource>();

    public void UpdateResources(List<Resource> foundResources)
    {
        foreach (Resource resource in foundResources)
        {
            if (resource == null)
                continue;

            bool isFoundFreeResource = _availableResources.Contains(resource);
            bool isFoundReservedResource = _reservedResources.Contains(resource);

            if (isFoundFreeResource || isFoundReservedResource)
                continue;

            _availableResources.Add(resource);
        }
    }

    public IReadOnlyList<Resource> GetAvailableResources() => _availableResources;

    public void Reserve(Resource resource)
    {
        _availableResources.Remove(resource);
        _reservedResources.Add(resource);
    }

    public void Free(Resource resource) => _reservedResources.Remove(resource);
}