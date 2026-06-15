using UnityEngine;

public class RandomPositionGenerator : MonoBehaviour
{
    [SerializeField] private BoxCollider _spawnAreaCollider;
    [SerializeField] private BoxCollider _excludedAreaCollider;
    [SerializeField] private float _height;

    private int _maxAttempts = 10;

    public Vector3 Spawning()
    {
        Bounds spawnBounds = _spawnAreaCollider.bounds;
        Vector3 candidatePosition = Vector3.zero;

        for (int i = 0; i < _maxAttempts; i++)
        {
            float x = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
            float z = Random.Range(spawnBounds.min.z, spawnBounds.max.z);

            candidatePosition = new Vector3(x, _height, z);

            if (_excludedAreaCollider != null && _excludedAreaCollider.bounds.Contains(candidatePosition))
                continue;

            return candidatePosition;
        }

        return candidatePosition;
    }
}