using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour, IResourceStorage
{
    [SerializeField] private List<ResourceEntry> _storage = new List<ResourceEntry>();

    private int _minValue = 0;

    public int GetAmount(ResourceType type)
    {
        foreach (var resource in _storage)
        {
            if (resource.Type == type)
                return resource.Amount;
        }

        return _minValue;
    }

    public void AddResource(ResourceType type, int amount)
    {
        for (int i = 0; i < _storage.Count; i++)
        {
            if (_storage[i].Type == type)
            {
                ResourceEntry resource = _storage[i];
                resource.Amount += amount;
                _storage[i] = resource;
                return;
            }
        }

        _storage.Add(new ResourceEntry { Type = type, Amount = amount });
    }
}