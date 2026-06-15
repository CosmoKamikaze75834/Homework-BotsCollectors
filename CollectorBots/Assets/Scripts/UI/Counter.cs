using System;
using UnityEngine;

public class Counter : MonoBehaviour, IResourceStorage
{
    [SerializeField] private ResourceStorage _resourceStorage;

    private int _resourceValue = 1;

    public event Action<ResourceType, int> Changed;

    public void AcceptResource(Resource resource)
    {
        ResourceType type = resource.ResourceType;

        _resourceStorage.AddResource(type, _resourceValue);

        Changed?.Invoke(type, GetAmount(type));
    }

    public int GetAmount(ResourceType type) => _resourceStorage.GetAmount(type);
}