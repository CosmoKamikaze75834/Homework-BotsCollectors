using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputHandler : InputHandler
{
    public event Action<Vector2> Clicked;
    public event Action<Vector2> MouseMoved;

    private void OnEnable() =>
        Controls.Button.SelectBase.performed += OnSelectBase;

    private void OnDisable() =>
        Controls.Button.SelectBase.performed -= OnSelectBase;

    private void Update()
    {
        Vector2 mousePosition = GetMousePosition();
        MouseMoved?.Invoke(mousePosition);
    }

    private void OnSelectBase(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = GetMousePosition();

        Clicked?.Invoke(mousePosition);
    }

    private Vector2 GetMousePosition() =>
        Controls.Mouse.Position.ReadValue<Vector2>();
}