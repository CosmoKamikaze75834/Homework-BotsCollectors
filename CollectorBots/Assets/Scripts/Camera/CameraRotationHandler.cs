using UnityEngine;

public class CameraRotationHandler : InputHandler
{
    [SerializeField] private float _rotationSpeed;

    private float _threshold = 0.5f;

    private float _maxPitch = 80f;
    private float _minPitch = -80f;

    private float _zRotation = 0f;

    private float _currentYaw;
    private float _currentPitch;

    private void Start()
    {
        _currentYaw = transform.eulerAngles.y;
        _currentPitch = transform.eulerAngles.x;
    }

    private void OnEnable()
    {
        Controls.Camera.MiddleMouse.Enable();
        Controls.Camera.MouseDelta.Enable();
    }

    private void OnDisable()
    {
        Controls.Camera.MiddleMouse.Disable();
        Controls.Camera.MouseDelta.Disable();
    }

    public void UpdateRotation()
    {
        if (Controls.Camera.MiddleMouse.ReadValue<float>() > _threshold)
        {
            Vector2 delta = Controls.Camera.MouseDelta.ReadValue<Vector2>();

            _currentYaw += delta.x * _rotationSpeed;
            _currentPitch -= delta.y * _rotationSpeed;

            _currentPitch = Mathf.Clamp(_currentPitch, _minPitch, _maxPitch);

            transform.rotation = Quaternion.Euler(_currentPitch, _currentYaw, _zRotation);
        }
    }
}