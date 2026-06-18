using System.Collections.Generic;
using UnityEngine;

public class DistributorResources : MonoBehaviour
{
    [SerializeField] private List<Collector> _collectors;
    [SerializeField] private ResourceAssignmentStrategyFactory _strategyFactory;
    [SerializeField] private ResourceRepository _resourceRepository;

    private IResourceSelectionStrategy _strategy;

    private void Start() => _strategy = _strategyFactory.InitializeStrategy();

    public void Distribute()
    {
        var availableResources = _resourceRepository.GetAvailableResources();

        if (availableResources.Count == 0)
            return;

        AssignTasksToCollectors(availableResources);
    }

    private void AssignTasksToCollectors(IReadOnlyList<Resource> availableResources)
    {
        for (int i = 0; i < _collectors.Count; i++)
        {
            if (_collectors[i].IsBusy)
                continue;

            AssignTaskToCollector(_collectors[i], availableResources);
        }
    }

    private void AssignTaskToCollector(Collector collector, IReadOnlyList<Resource> availableResources)
    {
        Resource selectedResource = _strategy.SelectResource(collector.transform.position,
                availableResources);

        //Debug.Log($"[Distributor] Стратегия вернула ресурс для бота {collector.name}: {selectedResource != null}");

        if (selectedResource != null)
        {
            collector.SetTarget(selectedResource.transform.position, selectedResource.transform);
            _resourceRepository.Reserve(selectedResource);
        }
    }

    public void AddBot(Collector bot)
    {
        _collectors.Add(bot);
        Distribute();
    }
}