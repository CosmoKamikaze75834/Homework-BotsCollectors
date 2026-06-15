using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardMovementInput : InputHandler
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _minBounds;
    [SerializeField] private Vector3 _maxBounds;

    private Vector2 _moveInput;

    private void OnEnable()
    {
        Controls.Camera.Move.Enable();

        Controls.Camera.Move.performed += OnMove;
        Controls.Camera.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        Controls.Camera.Move.Disable();

        Controls.Camera.Move.performed -= OnMove;
        Controls.Camera.Move.canceled -= OnMove;
    }

    public void HandleMovementInput()
    {
        if (_moveInput != Vector2.zero)
            ChangePosition(_moveInput);
    }

    private void OnMove(InputAction.CallbackContext context) => _moveInput = context.ReadValue<Vector2>();

    private void ChangePosition(Vector2 direction)
    {
        direction *= Time.deltaTime * _speed;

        Vector3 movement = transform.forward * direction.y + transform.right * direction.x;

        Vector3 newPosition = transform.position + movement;

        newPosition = LimitPosition(newPosition);

        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }

    private Vector3 LimitPosition(Vector3 positionToLimit)
    {
        positionToLimit = Vector3.Min(positionToLimit, _maxBounds);
        positionToLimit = Vector3.Max(positionToLimit, _minBounds);

        return positionToLimit;
    }
}