using System.Collections.Generic;
using UnityEngine;

public interface IResourceSelectionStrategy
{
    Resource SelectResource(Vector3 collectorPosition, List<Resource> availableResources);
}