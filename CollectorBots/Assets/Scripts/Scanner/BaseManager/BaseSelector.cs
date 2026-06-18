using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(Renderer))]
public class BaseSelector : InputHandler
{
    [SerializeField] private Material _material;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        Controls.Button.SelectBase.Enable();

        Controls.Button.SelectBase.performed += OnSelectBase;
        Controls.Button.SelectBase.canceled += OnSelectBase;
    }

    private void OnDisable()
    {
        Controls.Button.SelectBase.Disable();

        Controls.Button.SelectBase.performed -= OnSelectBase;
        Controls.Button.SelectBase.canceled -= OnSelectBase;
    }

    private void OnSelectBase(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Controls.Mouse.Position.ReadValue<Vector2>();

        ProcessBeam(mousePosition);
    }

    private void ProcessBeam(Vector2 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Base"))
            {
                Debug.Log(" лик по базе");

                if(_renderer != null && _material != null)
                {
                    _renderer.material = _material;
                }
            }
        }
    }
}