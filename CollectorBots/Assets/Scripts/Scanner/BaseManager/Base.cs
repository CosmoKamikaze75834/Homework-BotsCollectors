using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Base : MonoBehaviour
{
    private const int TimeScanner = 10;

    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _selectedMaterial;

    [SerializeField] private Flag _flag;

    [SerializeField] private UnitConfig _unitConfig;
    [SerializeField] private BotSpawner _spawner;

    [SerializeField] private BaseConstructionCost _baseConstruction;

    [SerializeField] private Scanner _scanner;
    [SerializeField] private ResourceRepository _resourceRepository;
    [SerializeField] private ResourcesLocator _locator;
    [SerializeField] private DistributorResources _distributor;

    [SerializeField] private ResourceStorage _storage;
    [SerializeField] private Counter _counter;
    [SerializeField] private CollectionPoint _collectionPoint;

    private WaitForSeconds _wait = new WaitForSeconds(TimeScanner);

    private bool _isWorking = true;

    private Renderer _renderer;

    private Flag _currentFlag;

    private void Start()
    {
        StartCoroutine(ScanCycle());
        _locator.OnResourcesFound += HandleResourcesFound;
        _collectionPoint.ArrivedAtBase += HandleDelivery;
        _spawner.OnBotSpawned += _distributor.AddBot;
        _renderer = GetComponent<Renderer>();
    }

    private void OnDisable()
    {
        _locator.OnResourcesFound -= HandleResourcesFound;
        _collectionPoint.ArrivedAtBase -= HandleDelivery;
        _spawner.OnBotSpawned -= _distributor.AddBot;
    }

    private IEnumerator ScanCycle()
    {
        while (_isWorking)
        {
            yield return LaunchScanner();
            _locator.Scan();
            yield return _wait;
        }
    }

    private IEnumerator LaunchScanner()
    {
        _scanner.gameObject.SetActive(true);
        yield return _scanner.Launch();
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

        if (_unitConfig.Cost.CanAfford(_storage))
        {
            _unitConfig.Cost.Deduct(_counter);
            StartCoroutine(_spawner.LaunchCreateBot());
        }

        if (_baseConstruction.Cost.CanAfford(_storage))
        {

        }

        _distributor.Distribute();
    }

    public void Select() => _renderer.material = _selectedMaterial;

    public void Deselect() => _renderer.material = _defaultMaterial;

    public void PlaceFlag(Vector3 position)
    {
        if(_currentFlag == null)
            _currentFlag = Instantiate(_flag, position, Quaternion.identity);
        else
            _currentFlag.transform.position = position;
    }
}