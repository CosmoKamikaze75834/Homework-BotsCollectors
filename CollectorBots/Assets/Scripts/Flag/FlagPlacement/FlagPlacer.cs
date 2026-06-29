using System;
using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Flag _flag;

    private Flag _currentFlag;

    public event Action FlagInstalled;

    public void PlaceFlag(Vector3 position)
    {
        if (_currentFlag == null)
        {
            _currentFlag = Instantiate(_flag, position, Quaternion.identity);
            FlagInstalled?.Invoke();
        }
        else
            _currentFlag.transform.position = position;
    }

    public void DestroyFlag()
    {
        Destroy(_currentFlag.gameObject);
        _currentFlag = null;
    }

    public bool HasFlag() => 
        _currentFlag != null;

    public Transform GetFlagTransform() =>
        _currentFlag != null ? _currentFlag.transform : null;
}