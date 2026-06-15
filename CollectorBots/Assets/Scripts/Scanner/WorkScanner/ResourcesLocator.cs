using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLocator : MonoBehaviour
{
    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private float _detectionRadius;

    private List<Resource> _resources = new List<Resource>();

    public event Action<List<Resource>> OnResourcesFound;

    public void Scan()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionRadius, _resourceLayer);

        _resources.Clear();

        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out Resource resource))
                _resources.Add(resource);
        }

        OnResourcesFound?.Invoke(new List<Resource>(_resources));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}