using UnityEngine;

public class CargoHandler : MonoBehaviour
{
    [SerializeField] private float _height;

    private Transform _targetTransform;

    private float _minValue = 0;

    public void PickupResource(Transform resourceTransform)
    {
        if (resourceTransform == null)
            return;

        _targetTransform = resourceTransform;

        _targetTransform.SetParent(transform, true);
        _targetTransform.localPosition = new Vector3(_minValue, _height, _minValue);
    }

    public void DetachResource()
    {
        if(_targetTransform != null)
        {
            _targetTransform.SetParent(null);
            _targetTransform = null;
        }
    }
}