using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoomHandler : InputHandler
{
    private const float DefaultInputValue = 0f;

    [SerializeField] private float _heightChangeSpeed;

    private float _maxHeight = 20f;
    private float _minHeight = 5f;
    private float _zoomInput;

    private void OnEnable()
    {
        Controls.Camera.Zoom.Enable();

        Controls.Camera.Zoom.performed += OnZoom;
        Controls.Camera.Zoom.canceled += OnZoom;
    }

    private void OnDisable()
    {
        Controls.Camera.Zoom.Disable();

        Controls.Camera.Zoom.performed -= OnZoom;
        Controls.Camera.Zoom.canceled -= OnZoom;
    }

    public void ApplyZoom()
    {
        if (_zoomInput != DefaultInputValue)
        {
            Vector3 position = transform.position;
            position.y -= _zoomInput * _heightChangeSpeed * Time.deltaTime;

            position.y = Mathf.Clamp(position.y, _minHeight, _maxHeight);

            transform.position = position;
        }
    }

    private void OnZoom(InputAction.CallbackContext context) => _zoomInput = context.ReadValue<float>();
}