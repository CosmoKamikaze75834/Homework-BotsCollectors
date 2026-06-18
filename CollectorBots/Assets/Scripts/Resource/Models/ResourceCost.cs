using System;
using System.Collections.Generic;
using UnityEngine;

//расходы ресурса
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

    //хватает ли накопленных ресурсов для постойки бота
    public bool CanAfford(IResourceStorage storage)
    {
        foreach (var item in _resourceDictionary)
        {
            int currentAmount = storage.GetAmount(item.Key);//Сколько у хранилища есть ресурсов этого типа

            if (currentAmount < item.Value)//если в хранилище меньше чем нам нужно
                return false;
        }

        return true;
    }

    //списать реусры определённого типа и количества
    public void Deduct(IResourceStorage storage)
    {
        foreach (var item in _resourceDictionary)
            storage.RemoveResource(item.Key, item.Value);
    }
}