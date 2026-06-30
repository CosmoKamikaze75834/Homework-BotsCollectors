using System.Collections.Generic;
using UnityEngine;

public class DistributorResources : MonoBehaviour
{
    [SerializeField] private List<Collector> _collectors;
    [SerializeField] private ResourceAssignmentStrategyFactory _strategyFactory;
    [SerializeField] private ResourceRepository _resourceRepository;

    private IResourceSelectionStrategy _strategy;

    private int _minValue = 1;

    public SelectionMode CurrentStrategy => 
        _strategyFactory.CurrentMode();

    private void Start() => 
        _strategy = _strategyFactory.CreateStrategy();

    public void Distribute()
    {
        var availableResources = _resourceRepository.GetAvailableResources();

        if (availableResources.Count == 0)
            return;

        AssignTasksToCollectors(availableResources);
    }

    public void AddBot(Collector bot)
    {
        _collectors.Add(bot);
        Distribute();
    }

    public void DeleteBot(Collector bot)
    {
        _collectors.Remove(bot);
        bot = null;
    }

    public List<Collector> SetListFreeBots()
    {
        List<Collector> freeCollector = new List<Collector>();

        for (int i = 0; i < _collectors.Count; i++)
        {
            if (_collectors[i].IsBusy)
                continue;

            freeCollector.Add(_collectors[i]);
        }

        return freeCollector;
    }

    public bool BotsLeft() => 
        _collectors.Count > _minValue;

    public int Count => 
        _collectors.Count;

    public void ChangeStrategy(SelectionMode mode)
    {
        _strategyFactory.SetSelectionMode(mode);
        _strategy = _strategyFactory.CreateStrategy();
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

        if (selectedResource != null)
        {
            collector.SetTarget(selectedResource.transform.position, selectedResource.transform);
            _resourceRepository.Reserve(selectedResource);
        }
    }
}