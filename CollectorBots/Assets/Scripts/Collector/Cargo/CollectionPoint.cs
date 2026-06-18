using System;
using UnityEngine;

public class CollectionPoint : MonoBehaviour
{
    public event Action<Collector, Resource> ArrivedAtBase;

    public void AcceptDelivery(Collector collector, Resource resource) => ArrivedAtBase?.Invoke(collector, resource);
}