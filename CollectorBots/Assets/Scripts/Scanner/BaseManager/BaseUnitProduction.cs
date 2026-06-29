using UnityEngine;

public class BaseUnitProduction : MonoBehaviour
{
    [SerializeField] private UnitConfig _unitConfig;
    [SerializeField] private ResourceStorage _storage;
    [SerializeField] private Counter _counter;
    [SerializeField] private BotSpawner _botSpawner;
    [SerializeField] private DistributorResources _distributorResources;

    private int _maxBots = 5;

    public void TryCreateBot()
    {
        if (_unitConfig.Cost.CanAfford(_storage) && _distributorResources.Count < _maxBots)
        {
            _unitConfig.Cost.Deduct(_counter);

            StartCoroutine(_botSpawner.LaunchCreateBot());
        }
    }
}