using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private const int TimeScanner = 10;

    [SerializeField] private Scanner _scanner;
    [SerializeField] private ResourceRepository _resourceRepository;
    [SerializeField] private ResourcesLocator _locator;
    [SerializeField] private DistributorResources _distributor;

    [SerializeField] private ResourceStorage _storage;
    [SerializeField] private Counter _counter;
    [SerializeField] private CollectionPoint _collectionPoint;

    private WaitForSeconds _wait = new WaitForSeconds(TimeScanner);

    private bool _isWorking = true;

    private void Start()
    {
        StartCoroutine(ScanCycle());
        _locator.OnResourcesFound += HandleResourcesFound;
        _collectionPoint.ArrivedAtBase += HandleDelivery;
    }

    private void OnDisable()
    {
        _locator.OnResourcesFound -= HandleResourcesFound;
        _collectionPoint.ArrivedAtBase -= HandleDelivery;
    }

    private IEnumerator ScanCycle()
    {
        while (_isWorking)
        {
            yield return StartCoroutine(LaunchScanner());
            _locator.Scan();
            yield return _wait;
        }
    }

    private IEnumerator LaunchScanner()
    {
        _scanner.gameObject.SetActive(true);
        yield return StartCoroutine(_scanner.Launch());
        _scanner.gameObject.SetActive(false);
    }

    private void HandleResourcesFound(List<Resource> foundResources)
    {
        _resourceRepository.UpdateResources(foundResources);
        _distributor.Distribute();
    }

    private void HandleDelivery(Collector collector, Resource resource)
    {
        _counter.AcceptResource(resource);
        _resourceRepository.Free(resource);
        _distributor.Distribute();
    }
}