using UnityEngine;

public class CameraObserver : MonoBehaviour
{
    [SerializeField] private CameraZoomHandler _height;
    [SerializeField] private CameraRotationHandler _scroll;
    [SerializeField] private KeyboardMovementInput _movementInput;

    private void Update()
    {
        _movementInput.HandleMovementInput();
        _scroll.UpdateRotation();
        _height.ApplyZoom();
    }
}