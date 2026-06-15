using System.Collections.Generic;
using UnityEngine;

public class DistributorResources : MonoBehaviour
{
    [SerializeField] private Collector[] _collectors;
    [SerializeField] private ResourceAssignmentStrategyFactory _strategyFactory;
    [SerializeField] private ResourceRepository _resourceRepository;

    private IResourceSelectionStrategy _strategy;

    private void Start() => _strategy = _strategyFactory.InitializeStrategy();

    public void Distribute()
    {
        List<Resource> availableResources = (List<Resource>)_resourceRepository.GetAvailableResources();

        if (availableResources.Count == 0)
            return;

        AssignTasksToCollectors(availableResources);
    }

    private void AssignTasksToCollectors(List<Resource> availableResources)
    {
        for (int i = 0; i < _collectors.Length; i++)
        {
            if (_collectors[i].IsBusy)
                continue;

            AssignTaskToCollector(_collectors[i], availableResources);
        }
    }

    private void AssignTaskToCollector(Collector collector, List<Resource> availableResources)
    {
        Resource selectedResource = _strategy.SelectResource(collector.transform.position,
                availableResources);

        if (selectedResource != null)
        {
            collector.SetTarget(selectedResource.transform.position, selectedResource.transform);
            _resourceRepository.Reserve(selectedResource);
            availableResources.Remove(selectedResource);
        }
    }
}