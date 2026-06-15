using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;

    public ResourceType ResourceType => _resourceType;

    public event Action<Resource> Disappeared;

    public void InvokeDisappearedEvent() => Disappeared?.Invoke(this);
}