using System.Collections;
using UnityEngine;

public class DelayedPickupStrategy : IResourceCollectionStrategy
{
    private float _minDelay = 3f;
    private float _maxDelay = 5f;

    private Coroutine _delayCoroutine;

    public void Collect(Collector collector)
    {
        if (_delayCoroutine != null)
            collector.StopCoroutine(_delayCoroutine);

        _delayCoroutine = collector.StartCoroutine(DelayedPickupProgress(collector));
    }

    private IEnumerator DelayedPickupProgress(Collector collector)
    {
        float delay = Random.Range(_minDelay, _maxDelay);

        collector.Stop();

        yield return new WaitForSeconds(delay);

        Transform targetResourceTransform = collector.Target;

        if (targetResourceTransform != null)
        {
            collector.AttempToPickupCurrentResource();
            collector.Resume();
        }

        _delayCoroutine = null;
    }
}