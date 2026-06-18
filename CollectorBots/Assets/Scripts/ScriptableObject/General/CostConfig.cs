using UnityEngine;

public class CostConfig : ScriptableObject
{
    [SerializeField] private ResourceCost _cost;

    public ResourceCost Cost => _cost;

    private void OnEnable() => _cost.BuildDictionary();
}