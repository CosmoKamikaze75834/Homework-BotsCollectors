using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private const int TimeScanner = 10;

    [SerializeField] private UnitConfig _unitConfig;//скриптбл обжект
    [SerializeField] private BotSpawner _spawner;//создаёт бота

    [SerializeField] private BaseConstructionCost _baseConstruction;

    [SerializeField] private Scanner _scanner;
    [SerializeField] private ResourceRepository _resourceRepository;
    [SerializeField] private ResourcesLocator _locator;
    [SerializeField] private DistributorResources _distributor;

    [SerializeField] private ResourceStorage _storage;// Для чистой работы с данными
    [SerializeField] private Counter _counter;// Для подписки на события(например, для UI)
    [SerializeField] private CollectionPoint _collectionPoint;

    private WaitForSeconds _wait = new WaitForSeconds(TimeScanner);

    private bool _isWorking = true;

    private void Start()
    {
        StartCoroutine(ScanCycle());
        _locator.OnResourcesFound += HandleResourcesFound;
        _collectionPoint.ArrivedAtBase += HandleDelivery;
        _spawner.OnBotSpawned += _distributor.AddBot;
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
        _counter.AcceptResource(resource);//ресурс учитывается базе
        _resourceRepository.Free(resource);//ресурс теперь не зарезервирован

        if (_unitConfig.Cost.CanAfford(_storage))//хватает ли русурсов для покупик бота?)
        {
            _unitConfig.Cost.Deduct(_counter);//вычитает ресурсы
            StartCoroutine(_spawner.LaunchCreateBot());
        }

        if (_baseConstruction.Cost.CanAfford(_storage))//хватает ли ресурсов для постройки базы?
        {

        }

        _distributor.Distribute();//отправляем бота сразу на новую цель
    }
}