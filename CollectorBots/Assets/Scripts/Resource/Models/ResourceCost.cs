using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourceCost
{
    [SerializeField] private List<ResourceEntry> _resourceEntries = new List<ResourceEntry>();

    private Dictionary<ResourceType, int> _resourceDictionary = new Dictionary<ResourceType, int>();

    public void BuildDictionary()
    {
        foreach (ResourceEntry entry in _resourceEntries)
        {
            if (_resourceDictionary.ContainsKey(entry.Type))
                _resourceDictionary[entry.Type] += entry.Amount;
            else
                _resourceDictionary.Add(entry.Type, entry.Amount);
        }
    }

    public bool CanAfford(IResourceStorage storage)
    {
        foreach (var item in _resourceDictionary)
        {
            int currentAmount = storage.GetAmount(item.Key);

            if (currentAmount < item.Value)
                return false;
        }

        return true;
    }

    public void Deduct(IResourceStorage storage)
    {
        foreach (var item in _resourceDictionary)
            storage.RemoveResource(item.Key, item.Value);
    }
}