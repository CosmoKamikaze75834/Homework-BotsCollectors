using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private const int TimeScanner = 10;

    [SerializeField] private BaseColonization _colonization;//процесс колонизации базы
    [SerializeField] private BaseUnitProduction _unitProduction;//запускает процесс создания ботов

    [SerializeField] private MarkerController _marker;//отвечает за логику маркера

    [SerializeField] private FlagPlacer _flagPlacer;//устанавливает флаг
    [SerializeField] private BaseSelectionView _selectionView;//меняет цвет базы

    [SerializeField] private BotSpawner _botSpawner;//спавнит ботов

    [SerializeField] private Scanner _scanner;//визуальный сканер
    [SerializeField] private ResourceRepository _resourceRepository;//содержит списки ресурсов и работает с ними
    [SerializeField] private ResourcesLocator _locator;//ищет ресурсы в радиусе
    [SerializeField] private DistributorResources _distributor;//распределяет ресурсы среди ботов

    [SerializeField] private Counter _counter;//визуальный счётчик
    [SerializeField] private CollectionPoint _collectionPoint;//точка бота

    [SerializeField] private PlacementArea _flagInstallationBoundaries;

    private WaitForSeconds _wait = new WaitForSeconds(TimeScanner);

    public DistributorResources DistributorResources => _distributor;
    public BaseSelectionView BaseSelectionView => _selectionView;
    public PlacementArea FlagInstallationBoundaries => _flagInstallationBoundaries;
    public FlagPlacer FlagPlacer => _flagPlacer;
    public CollectionPoint CollectionPoint => _collectionPoint;


    public MarkerController MarkerController => _marker;

    private bool _isWorking = true;


    private void Start()
    {
        StartCoroutine(ScanCycle());
        _colonization.BuilderTransferred += _distributor.DeleteBot;
        _locator.OnResourcesFound += HandleResourcesFound;
        _collectionPoint.ArrivedAtBase += HandleDelivery;
        _botSpawner.OnBotSpawned += HandleBotSpawned;
        _flagPlacer.FlagInstalled += _selectionView.Deselect;
    }

    private void OnDisable()
    {
        _colonization.BuilderTransferred -= _distributor.DeleteBot;
        _locator.OnResourcesFound -= HandleResourcesFound;
        _collectionPoint.ArrivedAtBase -= HandleDelivery;
        _botSpawner.OnBotSpawned -= HandleBotSpawned;
        _flagPlacer.FlagInstalled -= _selectionView.Deselect;
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

        if (_colonization.FlagPlacer.HasFlag())
        {
            List<Collector> collectors = _distributor.SetListFreeBots();

            if (collectors.Count > 0 && _distributor.BotsLeft())
            {
                Collector builder = collectors[0];

                _colonization.SendBotToFlag(builder);
            }
        }
        else
            _unitProduction.TryCreateBot();

        _distributor.Distribute();
    }

    private void HandleBotSpawned(Collector bot)
    {
        bot.SetPoint(_collectionPoint);
        _distributor.AddBot(bot);
    }
}