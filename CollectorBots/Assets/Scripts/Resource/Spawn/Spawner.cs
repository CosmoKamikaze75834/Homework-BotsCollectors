using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    private const float Delay = 3f;

    [SerializeField] private RandomPositionGenerator _position;
    [SerializeField] private List<ResourceSpawnConfig> _configs;

    private WaitForSeconds _wait = new WaitForSeconds(Delay);

    private Dictionary<ResourceType, ObjectPool<Resource>> _pools;

    private RandomResourceGenerator _resource;

    private void Awake()
    {
        _resource = new RandomResourceGenerator();

        _pools = new Dictionary<ResourceType, ObjectPool<Resource>>();

        foreach (var config in _configs)
        {
            ObjectPool<Resource> pool = new ObjectPool<Resource>(
                createFunc: () => Instantiate(config.Prefab),
                actionOnGet: (resource) => PrepareObject(resource),
            actionOnRelease: (resource) => resource.gameObject.SetActive(false),
            actionOnDestroy: (resource) => Destroy(resource.gameObject),
            collectionCheck: true,
            defaultCapacity: config.PoolCapacity,
            maxSize: config.PoolMaxSize);

            _pools.Add(config.Type, pool);
        }
    }

    private void Start() => StartCoroutine(ChangePosition());

    private void PrepareObject(Resource resource) => resource.gameObject.SetActive(true);

    public void ReceiveLocation(ResourceType type, Vector3 position)
    {
        if (_pools.TryGetValue(type, out var pool))
        {
            Resource resource = pool.Get();
            resource.Disappeared += ReturnObjectPool;
            resource.transform.position = position;
        }
    }

    private void ReturnObjectPool(Resource resource)
    {
        if (_pools.TryGetValue(resource.ResourceType, out var pool))
        {
            resource.Disappeared -= ReturnObjectPool;
            pool.Release(resource);
        }
    }

    private IEnumerator ChangePosition()
    {
        bool isWork = true;

        while (isWork)
        {
            ResourceType type = _resource.GetRandomResourceType();
            Vector3 position = _position.Spawning();
            ReceiveLocation(type, position);
            yield return _wait;
        }
    }
}