using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputHandler : InputHandler
{
    public event Action<Vector2> Clicked;
    public event Action<Vector2> MouseMoved;

    private void OnEnable()
    {
        Controls.Button.SelectBase.Enable();

        Controls.Button.SelectBase.performed += OnSelectBase;
    }

    private void OnDisable()
    {
        Controls.Button.SelectBase.Disable();

        Controls.Button.SelectBase.performed -= OnSelectBase;
    }

    private void Update()
    {
        Vector2 mousePosition = Controls.Mouse.Position.ReadValue<Vector2>();
        MouseMoved?.Invoke(mousePosition);
    }

    private void OnSelectBase(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Controls.Mouse.Position.ReadValue<Vector2>();

        Clicked?.Invoke(mousePosition);
    }
}