using System;
using System.Collections;
using UnityEngine;

public class BotSpawner : GeneralSpawner
{
    [SerializeField] private Collector _bot;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private SpawnPosition _spawnPosition;

    public event Action<Collector> OnBotSpawned;

    public IEnumerator LaunchCreateBot()
    {
        Vector3 position = _spawnPosition.GetNextPosition(_spawnPoint);

        yield return LaunchCreate();

        Collector newBot = Create(position);
    }

    private Collector Create(Vector3 finalPosition)
    {
        Collector bot = Instantiate(_bot, finalPosition, _spawnPoint.rotation);

        OnBotSpawned?.Invoke(bot);

        return bot;
    }
}