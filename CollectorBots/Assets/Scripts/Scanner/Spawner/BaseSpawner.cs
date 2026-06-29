using System;
using System.Collections;
using UnityEngine;

public class BaseSpawner : GeneralSpawner
{
    [SerializeField] private Base _base;
    [SerializeField] private float _shift;
    [SerializeField] private FlagPlacementValidator _blockChecker;

    public event Action<Base> OnBaseSpawned;

    public IEnumerator LaunchCreateBase(Transform spawnPoint)
    {
        Vector3 position = spawnPoint.position + transform.forward * _shift;

        yield return LaunchCreate(position);

        Base newBase = Create(position);

        _blockChecker.AddZone(newBase.FlagInstallationBoundaries);
    }

    private Base Create(Vector3 finalPosition)
    {
        Base newBase = Instantiate(_base, finalPosition, Quaternion.identity, SpawnContainer);

        OnBaseSpawned?.Invoke(newBase);

        return newBase;
    }
}