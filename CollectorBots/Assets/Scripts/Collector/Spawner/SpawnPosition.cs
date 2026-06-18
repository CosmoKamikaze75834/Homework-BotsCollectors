using UnityEngine;

public class SpawnPosition:MonoBehaviour
{
    [SerializeField] private float _step = 1f;

    private int _counter = 0;

    public Vector3 GetNextPosition(Transform point)
    {
        Vector3 basePosition = point.position;
        Vector3 offset = Vector3.forward * (_counter * _step);

        _counter++;

        return  basePosition + offset;
    }
}