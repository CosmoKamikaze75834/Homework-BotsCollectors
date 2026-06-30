using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private const int TimeScanner = 10;

    [SerializeField] private BaseConstructionSender _constructionSender;

    [SerializeField] private BaseColonization _colonization;
    [SerializeField] private BaseUnitProduction _unitProduction;

    [SerializeField] private MarkerController _marker;

    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private BaseSelectionView _selectionView;

    [SerializeField] private BotSpawner _botSpawner;

    [SerializeField] private Scanner _scanner;
    [SerializeField] private ResourceRepository _resourceRepository;
    [SerializeField] private ResourcesLocator _locator;
    [SerializeField] private DistributorResources _distributor;

    [SerializeField] private Counter _counter;
    [SerializeField] private CollectionPoint _collectionPoint;

    private WaitForSeconds _wait = new WaitForSeconds(TimeScanner);

    public DistributorResources DistributorResources => _distributor;
    public CollectionPoint CollectionPoint => _collectionPoint;

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

                _constructionSender.SendBotToFlag(builder);
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