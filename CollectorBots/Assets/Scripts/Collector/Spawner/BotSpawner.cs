using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Collector _bot;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _spawnContainer;
    [SerializeField] private EffectPlayer _effect;
    [SerializeField] private SpawnPosition _spawnPosition;

    private int _minTimeCreate = 3;
    private int _maxTimeCreate = 5;

    public event Action<Collector> OnBotSpawned;

    public IEnumerator LaunchCreateBot()
    {
        Vector3 position = _spawnPosition.GetNextPosition(_spawnPoint);

        _effect.SetPosition(position);
        _effect.Launch();

        int timeCreate = Random.Range(_minTimeCreate, _maxTimeCreate);
        yield return new WaitForSeconds(timeCreate);

        Collector newBot = Create(position);

        _effect.Complete();
    }

    private Collector Create(Vector3 finalPosition)
    {
        Collector bot = Instantiate(_bot, finalPosition, _spawnPoint.rotation, _spawnContainer);

        OnBotSpawned?.Invoke(bot);

        return bot;
    }
}