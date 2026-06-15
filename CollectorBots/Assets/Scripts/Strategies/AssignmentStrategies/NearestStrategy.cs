using System.Collections.Generic;
using UnityEngine;

public class NearestStrategy : IResourceSelectionStrategy
{
    public Resource SelectResource(Vector3 collectorPosition, List<Resource> availableResources)
    {
        if (availableResources == null || availableResources.Count == 0)
            return null;

        Resource nearestResource = null;

        float minDistance = Mathf.Infinity;

        foreach (Resource resource in availableResources)
        {
            if (resource == null)
                continue;

            float distance = Vector3.Distance(collectorPosition, resource.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestResource = resource;
            }
        }

        return nearestResource;
    }
}