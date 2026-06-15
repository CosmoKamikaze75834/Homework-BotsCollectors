using System.Collections;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    private const float Threshold = 0.01f;

    [SerializeField] private Vector3 _minScale;
    [SerializeField] private Vector3 _maxScale;

    [SerializeField] private float _speed;
    [SerializeField] private int _repeatCount = 3;

    private void Start() => transform.localScale = _minScale;

    public IEnumerator Launch()
    {
        for (int i = 0; i < _repeatCount; i++)
        {
            transform.localScale = _minScale;

            while (Vector3.Distance(transform.localScale, _maxScale) > Threshold)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, _maxScale, _speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}